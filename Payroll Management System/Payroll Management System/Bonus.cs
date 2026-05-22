using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Payroll_Management_System
{
    public partial class Bonus : Form
    {
        OleDbConnection con = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Payroll.accdb;");
        OleDbCommand cmd;
        OleDbDataAdapter da;
        DataTable dt;
        int selectedID = -1;

        public Bonus()
        {
            InitializeComponent();
        }

        private void Bonus_Load(object sender, EventArgs e)
        {
            LoadBonusData();
        }

        private void LoadBonusData()
        {
            try
            {
                con.Open();
                da = new OleDbDataAdapter("SELECT * FROM Bonus", con);
                dt = new DataTable();
                da.Fill(dt);
                dgvBonus.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading bonus data: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtName.Text == "" || txtAmount.Text == "")
            {
                MessageBox.Show("Please fill all required fields.");
                return;
            }

            try
            {
                con.Open();
                string query = "INSERT INTO Bonus ([Name], [Description], [Amount]) VALUES (@Name, @Description, @Amount)";
                cmd = new OleDbCommand(query, con);
                cmd.Parameters.AddWithValue("@Name", txtName.Text);
                cmd.Parameters.AddWithValue("@Description", txtDescription.Text);
                cmd.Parameters.AddWithValue("@Amount", txtAmount.Text);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Bonus saved successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving bonus: " + ex.Message);
            }
            finally
            {
                con.Close();
                ClearFields();
                LoadBonusData();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (selectedID == -1)
            {
                MessageBox.Show("Please select a bonus to edit.");
                return;
            }

            try
            {
                con.Open();
                string query = "UPDATE Bonus SET [Name]=@Name, [Description]=@Description, [Amount]=@Amount WHERE ID=@ID";
                cmd = new OleDbCommand(query, con);
                cmd.Parameters.AddWithValue("@Name", txtName.Text);
                cmd.Parameters.AddWithValue("@Description", txtDescription.Text);
                cmd.Parameters.AddWithValue("@Amount", txtAmount.Text);
                cmd.Parameters.AddWithValue("@ID", selectedID);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Bonus updated successfully.");
                ClearFields();
                LoadBonusData();
                selectedID = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating bonus: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedID == -1)
            {
                MessageBox.Show("Please select a bonus to delete.");
                return;
            }

            DialogResult dr = MessageBox.Show("Are you sure you want to delete this bonus?", "Confirm Delete", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                try
                {
                    con.Open();
                    string query = "DELETE FROM Bonus WHERE ID=@ID";
                    cmd = new OleDbCommand(query, con);
                    cmd.Parameters.AddWithValue("@ID", selectedID);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Bonus deleted successfully.");
                    ClearFields();
                    LoadBonusData();
                    selectedID = -1;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error deleting bonus: " + ex.Message);
                }
                finally
                {
                    con.Close();
                }
            }
        }

        private void dgvBonus_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                selectedID = Convert.ToInt32(dgvBonus.Rows[e.RowIndex].Cells["ID"].Value);
                txtName.Text = dgvBonus.Rows[e.RowIndex].Cells["Name"].Value.ToString();
                txtDescription.Text = dgvBonus.Rows[e.RowIndex].Cells["Description"].Value.ToString();
                txtAmount.Text = dgvBonus.Rows[e.RowIndex].Cells["Amount"].Value.ToString();
            }
        }

        private void ClearFields()
        {
            txtName.Clear();
            txtDescription.Clear();
            txtAmount.Clear();
        }
    }
}

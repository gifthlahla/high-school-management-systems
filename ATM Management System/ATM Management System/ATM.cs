using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace ATM_Management_System
{
    public partial class ATM : Form
    {
        string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=ATM.accdb;";
        OleDbConnection conn;

        public ATM()
        {
            InitializeComponent();
            conn = new OleDbConnection(connectionString);
            LoadATMs();
        }

        private void LoadATMs()
        {
            try
            {
                conn.Open();
                OleDbDataAdapter da = new OleDbDataAdapter("SELECT * FROM ATM", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvATM.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading ATMs: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void ClearFields()
        {
            txtATMID.Clear();
            txtLocation.Clear();
            txtBalance.Clear();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtATMID.Text == "" || txtLocation.Text == "" || txtBalance.Text == "")
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            try
            {
                conn.Open();
                string query = "INSERT INTO ATM (ATMID, Location, CashBalance) VALUES (@id, @loc, @bal)";
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", txtATMID.Text);
                cmd.Parameters.AddWithValue("@loc", txtLocation.Text);
                cmd.Parameters.AddWithValue("@bal", Convert.ToDecimal(txtBalance.Text));
                cmd.ExecuteNonQuery();
                MessageBox.Show("ATM added successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding ATM: " + ex.Message);
            }
            finally
            {
                conn.Close();
                LoadATMs();
                ClearFields();  // Auto clear
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (txtATMID.Text == "")
            {
                MessageBox.Show("Please select an ATM to update.");
                return;
            }

            try
            {
                conn.Open();
                string query = "UPDATE ATM SET Location = @loc, CashBalance = @bal WHERE ATMID = @id";
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("@loc", txtLocation.Text);
                cmd.Parameters.AddWithValue("@bal", Convert.ToDecimal(txtBalance.Text));
                cmd.Parameters.AddWithValue("@id", txtATMID.Text);
                cmd.ExecuteNonQuery();
                MessageBox.Show("ATM updated successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating ATM: " + ex.Message);
            }
            finally
            {
                conn.Close();
                LoadATMs();
                ClearFields();  // Auto clear
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (txtATMID.Text == "")
            {
                MessageBox.Show("Please select an ATM to delete.");
                return;
            }

            try
            {
                conn.Open();
                string query = "DELETE FROM ATM WHERE ATMID = @id";
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", txtATMID.Text);
                cmd.ExecuteNonQuery();
                MessageBox.Show("ATM deleted successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting ATM: " + ex.Message);
            }
            finally
            {
                conn.Close();
                LoadATMs();
                ClearFields();  // Auto clear
            }
        }

        private void dgvATM_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvATM.Rows[e.RowIndex].Cells[0].Value != null)
            {
                txtATMID.Text = dgvATM.Rows[e.RowIndex].Cells["ATMID"].Value.ToString();
                txtLocation.Text = dgvATM.Rows[e.RowIndex].Cells["Location"].Value.ToString();
                txtBalance.Text = dgvATM.Rows[e.RowIndex].Cells["CashBalance"].Value.ToString();
            }
        }
    }
}


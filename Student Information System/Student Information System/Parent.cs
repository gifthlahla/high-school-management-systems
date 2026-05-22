using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Student_Information_System
{
    public partial class Parent : Form
    {
        OleDbConnection con = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=StudentInfo.accdb");
        int selectedParentID = -1;

        public Parent()
        {
            InitializeComponent();
            LoadStudentIDs();
            LoadParents();
        }

        private void LoadStudentIDs()
        {
            try
            {
                con.Open();
                OleDbCommand cmd = new OleDbCommand("SELECT ID FROM Students", con);
                OleDbDataReader reader = cmd.ExecuteReader();

                cmbStudentId.Items.Clear();
                while (reader.Read())
                {
                    cmbStudentId.Items.Add(reader["ID"].ToString());
                }

                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading student IDs: " + ex.Message);
                con.Close();
            }
        }

        private void cmbStudentId_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbStudentId.SelectedIndex != -1)
            {
                try
                {
                    con.Open();
                    OleDbCommand cmd = new OleDbCommand("SELECT [Name] FROM Students WHERE ID = ?", con);
                    cmd.Parameters.AddWithValue("?", cmbStudentId.Text);
                    OleDbDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        lblStudentName.Text = reader["Name"].ToString();
                    }
                    else
                    {
                        lblStudentName.Text = " - ";
                    }

                    con.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error : " + ex.Message);
                    con.Close();
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (cmbStudentId.SelectedIndex == -1 || txtName.Text == "")
            {
                MessageBox.Show("Please enter all fields.");
                return;
            }

            try
            {
                con.Open();
                OleDbCommand cmd = new OleDbCommand("INSERT INTO Parents (StudentID, Name, Email, Phone, ID) VALUES (?, ?, ?, ?, ?)", con);
                cmd.Parameters.AddWithValue("?", cmbStudentId.Text);
                cmd.Parameters.AddWithValue("?", txtName.Text);
                cmd.Parameters.AddWithValue("?", txtEmail.Text);
                cmd.Parameters.AddWithValue("?", txtPhone.Text);
                cmd.Parameters.AddWithValue("?", txtID.Text);
                cmd.ExecuteNonQuery();
                con.Close();

                MessageBox.Show("Parent record saved.");
                LoadParents();
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving: " + ex.Message);
                con.Close();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedParentID == -1)
            {
                MessageBox.Show("Select a parent record to update.");
                return;
            }

            try
            {
                con.Open();
                OleDbCommand cmd = new OleDbCommand("UPDATE Parents SET StudentID = ?, Name = ?, Email = ?, Phone = ?, ID = ? WHERE ID = ?", con);
                cmd.Parameters.AddWithValue("?", cmbStudentId.Text);
                cmd.Parameters.AddWithValue("?", txtName.Text);
                cmd.Parameters.AddWithValue("?", txtEmail.Text);
                cmd.Parameters.AddWithValue("?", txtPhone.Text);
                cmd.Parameters.AddWithValue("?", txtID.Text);
                cmd.Parameters.AddWithValue("?", selectedParentID);
                cmd.ExecuteNonQuery();
                con.Close();

                MessageBox.Show("Parent record updated.");
                LoadParents();
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating: " + ex.Message);
                con.Close();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedParentID == -1)
            {
                MessageBox.Show("Select a parent record to delete.");
                return;
            }

            DialogResult confirm = MessageBox.Show("Are you sure you want to delete this parent record?", "Confirm", MessageBoxButtons.YesNo);
            if (confirm == DialogResult.Yes)
            {
                try
                {
                    con.Open();
                    OleDbCommand cmd = new OleDbCommand("DELETE FROM Parents WHERE ID = ?", con);
                    cmd.Parameters.AddWithValue("?", selectedParentID);
                    cmd.ExecuteNonQuery();
                    con.Close();

                    MessageBox.Show("Parent record deleted.");
                    LoadParents();
                    ClearInputs();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error deleting: " + ex.Message);
                    con.Close();
                }
            }
        }

        private void LoadParents()
        {
            try
            {
                con.Open();
                OleDbDataAdapter da = new OleDbDataAdapter("SELECT * FROM Parents", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvParents.DataSource = dt;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading parents: " + ex.Message);
                con.Close();
            }
        }

        private void dgvParents_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvParents.Rows[e.RowIndex];
                selectedParentID = Convert.ToInt32(row.Cells["ID"].Value);
                cmbStudentId.Text = row.Cells["StudentID"].Value.ToString();
                txtName.Text = row.Cells["Name"].Value.ToString();
                txtEmail.Text = row.Cells["Email"].Value.ToString();
                txtPhone.Text = row.Cells["Phone"].Value.ToString();
                txtID.Text = row.Cells["ID"].Value.ToString();
            }
        }

        private void ClearInputs()
        {
            cmbStudentId.SelectedIndex = -1;
            txtName.Clear();
            txtID.Clear();
            txtEmail.Clear();
            txtPhone.Clear();
            lblStudentName.Text = "";
            selectedParentID = -1;
        }
    }
}

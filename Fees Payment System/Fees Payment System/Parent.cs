using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Fees_Payment_System
{
    public partial class Parent : Form
    {
        private OleDbConnection conn;
        private string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=FeesPayment.accdb;";
        int selectedParentID = -1;

        public Parent()
        {
            InitializeComponent();
            conn = new OleDbConnection(connectionString);
        }

        private void Parent_Load(object sender, EventArgs e)
        {
            // Load parent data into DataGridView
            LoadParentData();
            LoadStudentIDs();
        }

        private void LoadStudentIDs()
        {
            try
            {
                conn.Open();
                OleDbCommand cmd = new OleDbCommand("SELECT StudentID FROM Students", conn);
                OleDbDataReader reader = cmd.ExecuteReader();

                cmbStudentID.Items.Clear();
                while (reader.Read())
                {
                    cmbStudentID.Items.Add(reader["StudentID"].ToString());
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading student IDs: " + ex.Message);
                conn.Close();
            }
        }

        private void LoadParentData()
        {
            try
            {
                conn.Open();
                string query = "SELECT * FROM Parent";
                OleDbDataAdapter adapter = new OleDbDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dgvParent.DataSource = dt;
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading parent data: " + ex.Message);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtName.Text) || string.IsNullOrEmpty(txtPhone.Text) || string.IsNullOrEmpty(txtEmail.Text))
                {
                    MessageBox.Show("Please fill in all fields.");
                    return;
                }

                conn.Open();
                string query = "INSERT INTO Parent (ParentName, PhoneNumber, Email, StudentID) VALUES (?, ?, ?, ?)";
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("?", txtName.Text);
                cmd.Parameters.AddWithValue("?", txtPhone.Text);
                cmd.Parameters.AddWithValue("?", txtEmail.Text);
                cmd.Parameters.AddWithValue("?", cmbStudentID.Text);

                cmd.ExecuteNonQuery();
                conn.Close();
                MessageBox.Show("Parent record saved successfully.");
                LoadParentData(); // Reload parent data
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving parent data: " + ex.Message);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (selectedParentID == -1)
            {
                MessageBox.Show("Select a parent record to update.");
                return;
            }

            try
            {
                conn.Open();
                OleDbCommand cmd = new OleDbCommand("UPDATE Parents SET StudentID = ?, ParentName = ?, Email = ?, PhoneNumber = ?, WHERE ParentID = ?", conn);
                cmd.Parameters.AddWithValue("?", cmbStudentID.Text);
                cmd.Parameters.AddWithValue("?", txtName.Text);
                cmd.Parameters.AddWithValue("?", txtEmail.Text);
                cmd.Parameters.AddWithValue("?", txtPhone.Text);
                cmd.Parameters.AddWithValue("?", selectedParentID);
                cmd.ExecuteNonQuery();
                conn.Close();

                MessageBox.Show("Parent record updated.");
                LoadParentData();
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating: " + ex.Message);
                conn.Close();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtPrntID.Text))
                {
                    MessageBox.Show("Please select a parent record to delete.");
                    return;
                }

                conn.Open();
                string query = "DELETE FROM Parent WHERE ParentID = ?";
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("?", txtPrntID.Text);
                cmd.ExecuteNonQuery();
                conn.Close();

                MessageBox.Show("Parent record deleted successfully.");
                LoadParentData(); // Reload parent data
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting parent record: " + ex.Message);
            }
        }

        private void dgvParent_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvParent.Rows[e.RowIndex];
                selectedParentID = Convert.ToInt32(row.Cells["ParentID"].Value);
                txtPrntID.Text = row.Cells["ParentID"].Value.ToString();
                txtName.Text = row.Cells["ParentName"].Value.ToString();
                txtPhone.Text = row.Cells["PhoneNumber"].Value.ToString();
                txtEmail.Text = row.Cells["Email"].Value.ToString();
                cmbStudentID.Text = row.Cells["StudentID"].Value.ToString();
            }
        }

        private void cmbStudentID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbStudentID.SelectedIndex != -1)
            {
                try
                {
                    conn.Open();
                    OleDbCommand cmd = new OleDbCommand("SELECT [StudentName] FROM Students WHERE StudentID = ?", conn);
                    cmd.Parameters.AddWithValue("?", cmbStudentID.Text);
                    OleDbDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        lblStudent.Text = reader["StudentName"].ToString();
                    }
                    else
                    {
                        lblStudent.Text = " - ";
                    }

                    conn.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error fetching student name: " + ex.Message);
                    conn.Close();
                }
            }
            else
            {
                lblStudent.Text = " - ";
            }
        }

        private void ClearInputs()
        {
            cmbStudentID.SelectedIndex = -1;
            txtName.Clear();
            txtPrntID.Clear();
            txtEmail.Clear();
            txtPhone.Clear();
            lblStudent.Text = "";
            selectedParentID = -1;
        }
    }
}

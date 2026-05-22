using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Hospital_Management_System
{
    public partial class Accounts : Form
    {
        OleDbConnection conn = new OleDbConnection(
            @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=HospitalManagement.accdb"
        );

        int selectedUserId = -1;

        public Accounts()
        {
            InitializeComponent();
        }

        private void Accounts_Load(object sender, EventArgs e)
        {
            LoadUsers();
        }

        private void LoadUsers()
        {
            try
            {
                conn.Open();
                OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT * FROM Users", conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dgvAccounts.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading users: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                string query = "INSERT INTO Users (Username, Password) VALUES (?, ?)";
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("?", txtUsername.Text);
                cmd.Parameters.AddWithValue("?", txtPassword.Text);
                cmd.ExecuteNonQuery();

                MessageBox.Show("User added successfully.");
                LoadUsers();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving user: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (selectedUserId < 0)
            {
                MessageBox.Show("Please select a user to edit.");
                return;
            }

            try
            {
                conn.Open();
                string query = "UPDATE Users SET Username=?, Password=? WHERE ID=?";
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("?", txtUsername.Text);
                cmd.Parameters.AddWithValue("?", txtPassword.Text);
                cmd.Parameters.AddWithValue("?", selectedUserId);
                cmd.ExecuteNonQuery();

                MessageBox.Show("User updated successfully.");
                LoadUsers();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error editing user: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedUserId < 0)
            {
                MessageBox.Show("Please select a user to delete.");
                return;
            }

            try
            {
                conn.Open();
                string query = "DELETE FROM Users WHERE ID=?";
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("?", selectedUserId);
                cmd.ExecuteNonQuery();

                MessageBox.Show("User deleted successfully.");
                LoadUsers();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting user: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void ClearFields()
        {
            txtUsername.Text = "";
            txtPassword.Text = "";
            selectedUserId = -1;
        }

        private void dgvAccounts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                selectedUserId = Convert.ToInt32(dgvAccounts.Rows[e.RowIndex].Cells["ID"].Value);
                txtUsername.Text = dgvAccounts.Rows[e.RowIndex].Cells["Username"].Value.ToString();
                txtPassword.Text = dgvAccounts.Rows[e.RowIndex].Cells["Password"].Value.ToString();
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close(); // Close form when the close icon is clicked
        }
    }
}

using System;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Windows.Forms;

namespace Employee_Attendance_System
{
    public partial class Account : Form
    {
        private OleDbConnection conn;
        private string selectedUsername = null;

        public Account()
        {
            InitializeComponent();

            string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EmpAttend.accdb");
            conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + dbPath);
            LoadUsers();
        }
        private void LoadUsers()
        {
            try
            {
                conn.Open();
                string query = "SELECT Username, Password FROM [User]";
                OleDbDataAdapter adapter = new OleDbDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dgvUsers.DataSource = dt;
                dgvUsers.Columns["Username"].HeaderText = "Username";
                dgvUsers.Columns["Password"].HeaderText = "Password";
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
            if (txtUsername.Text == "" || txtPassword.Text == "")
            {
                MessageBox.Show("Please enter username and password.");
                return;
            }

            try
            {
                conn.Open();
                string query = "INSERT INTO [User] ([Username], [Password]) VALUES (@user, @pass)";
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("@user", txtUsername.Text);
                cmd.Parameters.AddWithValue("@pass", txtPassword.Text);
                cmd.ExecuteNonQuery();

                MessageBox.Show("User added.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding user (maybe username already exists): " + ex.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
                LoadUsers();
                ClearFields();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedUsername == null)
            {
                MessageBox.Show("Please select a user to update.");
                return;
            }

            try
            {
                conn.Open();
                string query = "UPDATE [User] SET [Password] = @pass WHERE [Username] = @user";
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("@pass", txtPassword.Text);
                cmd.Parameters.AddWithValue("@user", selectedUsername);
                cmd.ExecuteNonQuery();

                MessageBox.Show("User updated.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating user: " + ex.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
                LoadUsers();
                ClearFields();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedUsername == null)
            {
                MessageBox.Show("Please select a user to delete.");
                return;
            }

            DialogResult result = MessageBox.Show("Are you sure you want to delete this user?", "Confirm Delete", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                try
                {
                    conn.Open();
                    string query = "DELETE FROM [User] WHERE [Username] = @user";
                    OleDbCommand cmd = new OleDbCommand(query, conn);
                    cmd.Parameters.AddWithValue("@user", selectedUsername);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("User deleted.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error deleting user: " + ex.Message);
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                    LoadUsers();
                    ClearFields();
                }
            }
        }

        private void dgvUsers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvUsers.Rows[e.RowIndex].Cells[0].Value != null)
            {
                DataGridViewRow row = dgvUsers.Rows[e.RowIndex];
                selectedUsername = row.Cells[0].Value.ToString();
                txtUsername.Text = selectedUsername;
                txtPassword.Text = row.Cells[1].Value.ToString();

                dgvUsers.ClearSelection();
                row.Selected = true;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void ClearFields()
        {
            txtUsername.Clear();
            txtPassword.Clear();
            selectedUsername = null;
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
            Employee_Attendance_System.Menu.GoToPreviousButton();
        }
    }
}

using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Student_Information_System
{
    public partial class Account : Form
    {
        private string selectedUsername = null;
        private OleDbConnection conn;
        private string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=StudentInfo.accdb";
        
        public Account()
        {
            InitializeComponent();
            conn = new OleDbConnection(connectionString);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
            Student_Information_System.Menu.GoToPreviousButton();
        }

        private void pctSearch_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            selectedUsername = txtUsername.Text;

            conn.Open();
            string query = "SELECT COUNT(*) FROM [User] WHERE Username = @user";
            OleDbCommand cmd = new OleDbCommand(query, conn);
            cmd.Parameters.AddWithValue("@user", username);
            
            int count = Convert.ToInt32(cmd.ExecuteScalar());
            if (count > 0)
            {
                MessageBox.Show("Account exists");
                btnEdit.Enabled = true;
                btnDelete .Enabled = true;
            }
            else
            {
                MessageBox.Show("Account not found");
            }
            conn.Close();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtPassword.Clear();
            txtUsername.Clear();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            // update logic
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
                btnClear.PerformClick();
            }
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            // delete logic
            DialogResult result = MessageBox.Show("Are you sure you want to delete this account?", "Delete Account", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    string query = "DELETE FROM [User] WHERE [Username] = ?";
                    OleDbCommand cmd = new OleDbCommand(query, conn);
                    cmd.Parameters.AddWithValue("?", selectedUsername);

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Account deleted successfully!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
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
                btnClear.PerformClick();
            }
        }
    }
}

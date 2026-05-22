using System;
using System.Data;
using System.Data.OleDb;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace Student_Information_System
{
    public partial class Login : Form
    {
        private OleDbConnection connection;
        private int failedAttempts = 0;

        public Login()
        {
            InitializeComponent();

            // Set up connection string to CarDealer.accdb
            string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "StudentInfo.accdb");
            string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + dbPath;
            connection = new OleDbConnection(connectionString);
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            // Limit login attempts
            if (failedAttempts >= 3)
            {
                MessageBox.Show("Your account is locked due to multiple failed login attempts. Please try again later.", "Account Locked", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                connection.Open();
                
                // Use parameterized query to avoid SQL injection
                string query = "SELECT COUNT(*) FROM [User] WHERE Username = @user AND Password = @pass";
                OleDbCommand cmd = new OleDbCommand(query, connection);
                cmd.Parameters.AddWithValue("@user", username);
                cmd.Parameters.AddWithValue("@pass", password);

                int count = Convert.ToInt32(cmd.ExecuteScalar());

                if (count > 0)
                {
                    // Reset failed login attempts after successful login
                    failedAttempts = 0;

                    MessageBox.Show("Login successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Hide();
                    // Load main menu form
                    Menu menuForm = new Menu();
                    menuForm.Show();
                }
                else
                {
                    failedAttempts++;
                    MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPassword.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}

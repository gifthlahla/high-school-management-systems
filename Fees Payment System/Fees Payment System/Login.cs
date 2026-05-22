using System;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Windows.Forms;

namespace Fees_Payment_System
{
    public partial class Login : Form
    {
        private OleDbConnection connection;

        public Login()
        {
            InitializeComponent();

            // Set up connection string to FeePayment database
            string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FeesPayment.accdb");
            string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + dbPath;
            connection = new OleDbConnection(connectionString);
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            try
            {
                connection.Open();

                string query = "SELECT COUNT(*) FROM [User] WHERE Username = @user AND Password = @pass";
                OleDbCommand cmd = new OleDbCommand(query, connection);
                cmd.Parameters.AddWithValue("@user", username);
                cmd.Parameters.AddWithValue("@pass", password);

                int count = Convert.ToInt32(cmd.ExecuteScalar());

                if (count > 0)
                {
                    MessageBox.Show("Login successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.Hide();

                    // Load main menu or dashboard form
                    Menu menuForm = new Menu();
                    menuForm.Show();
                }
                else
                {
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

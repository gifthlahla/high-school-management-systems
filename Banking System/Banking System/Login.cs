using System;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Banking_System
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        // Connection string to your Access database (.accdb)
        private string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Bank.accdb";

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Please enter both username and password.", "Missing Fields", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (OleDbConnection con = new OleDbConnection(connectionString))
                {
                    string query = "SELECT COUNT(*) FROM [Login] WHERE Username=? AND Password=?";
                    using (OleDbCommand cmd = new OleDbCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("?", txtUsername.Text);
                        cmd.Parameters.AddWithValue("?", txtPassword.Text);

                        con.Open();
                        int result = (int)cmd.ExecuteScalar();
                        con.Close();

                        if (result > 0)
                        {
                            MessageBox.Show("Login successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            Menu menu = new Menu(); // Ensure your main form is named 'Menu'
                            menu.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during login: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}

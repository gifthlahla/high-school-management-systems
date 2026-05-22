using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Banking_System
{
    public partial class Bank : Form
    {
        private string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Bank.accdb";

        public Bank()
        {
            InitializeComponent();
        }

        private void Bank_Load(object sender, EventArgs e)
        {
            LoadDashboardData(); // Automatically refresh stats on load
        }

        private void LoadDashboardData()
        {
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // Total customers
                    string customerQuery = "SELECT COUNT(*) FROM [Customer]";
                    OleDbCommand cmdCustomers = new OleDbCommand(customerQuery, conn);
                    int customerCount = Convert.ToInt32(cmdCustomers.ExecuteScalar());
                    lblTotalCustomers.Text = customerCount.ToString();

                    // Total accounts
                    string accountQuery = "SELECT COUNT(*) FROM [Account]";
                    OleDbCommand cmdAccounts = new OleDbCommand(accountQuery, conn);
                    int accountCount = Convert.ToInt32(cmdAccounts.ExecuteScalar());
                    lblTotalAccounts.Text = accountCount.ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading dashboard data: " + ex.Message);
                }
            }
        }
    }
}

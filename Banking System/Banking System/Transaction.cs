using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Banking_System
{
    public partial class Transaction : Form
    {
        string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Bank.accdb";

        public Transaction()
        {
            InitializeComponent();
            LoadAccounts();
            LoadTransactions();
            LoadHistory();
        }

        private void LoadHistory()
        {
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                string query = "SELECT * FROM [Transactions]";
                OleDbDataAdapter da = new OleDbDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvTransactions.DataSource = dt;
            }
        }
        private void LoadAccounts()
        {
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                string query = "SELECT [AccountNumber] FROM [Account]";
                OleDbDataAdapter da = new OleDbDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                cmbAccountNumber.DataSource = dt;
                cmbAccountNumber.DisplayMember = "AccountNumber";
                cmbAccountNumber.ValueMember = "AccountNumber";
            }
        }

        private void LoadTransactions()
        {
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                string query = "SELECT [TransactionID] FROM [Transactions]";
                OleDbDataAdapter da = new OleDbDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                cmbTransactionID.DataSource = dt;
                cmbTransactionID.DisplayMember = "TransactionID";
                cmbTransactionID.ValueMember = "TransactionID";
            }
        }

        private void cmbAccountNumber_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Load customer name based on the selected account number
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                string query = "SELECT [Name] FROM [Customer] WHERE [AccountNumber] = ?";
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("?", cmbAccountNumber.SelectedValue.ToString());

                try
                {
                    conn.Open();
                    OleDbDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        lblCustomerName.Text = reader["Name"].ToString();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtAmount.Text) || cmbAccountNumber.SelectedIndex == -1 || cmbTransactionType.SelectedIndex == -1)
            {
                MessageBox.Show("Please fill all fields.");
                return;
            }

            string transactionType = cmbTransactionType.SelectedItem.ToString();
            decimal amount = Convert.ToDecimal(txtAmount.Text);
            string accountNumber = cmbAccountNumber.SelectedValue.ToString();
            string customerName = lblCustomerName.Text;

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                string query = "INSERT INTO [Transactions] ([TransactionType], [Amount], [CustomerName], [AccountNumber]) VALUES (?, ?, ?, ?)";
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("?", transactionType);
                cmd.Parameters.AddWithValue("?", amount);
                cmd.Parameters.AddWithValue("?", customerName);
                cmd.Parameters.AddWithValue("?", accountNumber);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Transaction added successfully.");
                    LoadTransactions();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (cmbTransactionID.SelectedIndex == -1 || string.IsNullOrEmpty(txtAmount.Text) || cmbAccountNumber.SelectedIndex == -1 || cmbTransactionType.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a transaction to edit and fill all fields.");
                return;
            }

            int transactionID = Convert.ToInt32(cmbTransactionID.SelectedValue);
            string transactionType = cmbTransactionType.SelectedItem.ToString();
            decimal amount = Convert.ToDecimal(txtAmount.Text);
            string accountNumber = cmbAccountNumber.SelectedValue.ToString();
            string customerName = lblCustomerName.Text;

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                string query = "UPDATE [Transactions] SET [TransactionType] = ?, [Amount] = ?, [CustomerName] = ?, [AccountNumber] = ? WHERE [TransactionID] = ?";
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("?", transactionType);
                cmd.Parameters.AddWithValue("?", amount);
                cmd.Parameters.AddWithValue("?", customerName);
                cmd.Parameters.AddWithValue("?", accountNumber);
                cmd.Parameters.AddWithValue("?", transactionID);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Transaction updated successfully.");
                    LoadTransactions();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (cmbTransactionID.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a transaction to delete.");
                return;
            }

            int transactionID = Convert.ToInt32(cmbTransactionID.SelectedValue);

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                string query = "DELETE FROM [Transactions] WHERE [TransactionID] = ?";
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("?", transactionID);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Transaction deleted successfully.");
                    LoadTransactions();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void dgvTransactions_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Get the selected row's details and display them
                DataGridViewRow row = dgvTransactions.Rows[e.RowIndex];
                cmbTransactionID.SelectedValue = row.Cells["TransactionID"].Value.ToString();
                cmbTransactionType.SelectedItem = row.Cells["TransactionType"].Value.ToString();
                txtAmount.Text = row.Cells["Amount"].Value.ToString();
                lblCustomerName.Text = row.Cells["CustomerName"].Value.ToString();
                cmbAccountNumber.SelectedValue = row.Cells["AccountNumber"].Value.ToString();
            }
        }

        private void Transaction_Load(object sender, EventArgs e)
        {
            LoadTransactions();
        }
    }
}

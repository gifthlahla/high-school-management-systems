using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace ATM_Management_System
{
    public partial class Transaction : Form
    {
        string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=ATM.accdb";

        public Transaction()
        {
            InitializeComponent();
            LoadTransactions();
            LoadAccounts();
            LoadTransactionIDs();

            // Connect the event handler (in case it's not connected in Designer)
            cmbTransactionID.SelectedIndexChanged += cmbTransactionID_SelectedIndexChanged;
        }

        private void LoadAccounts()
        {
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                string query = "SELECT AccountNumber FROM [Account]";
                OleDbDataAdapter da = new OleDbDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                cmbAccount.DataSource = dt;
                cmbAccount.DisplayMember = "AccountNumber";
                cmbAccount.ValueMember = "AccountNumber";
            }
        }

        private void LoadTransactionIDs()
        {
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                string query = "SELECT TransactionID FROM [Transaction]";
                OleDbDataAdapter da = new OleDbDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                cmbTransactionID.DataSource = dt;
                cmbTransactionID.DisplayMember = "TransactionID";
                cmbTransactionID.ValueMember = "TransactionID";
            }
        }

        private void LoadTransactions()
        {
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                string query = "SELECT * FROM [Transaction]";
                OleDbDataAdapter da = new OleDbDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvTransaction.DataSource = dt;
            }
        }

        private void ClearFields()
        {
            cmbTransactionID.SelectedIndex = -1;
            cmbAccount.SelectedIndex = -1;
            txtAmount.Clear();
            cmbTransactionType.SelectedIndex = -1;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (cmbAccount.SelectedIndex == -1 || txtAmount.Text == "" || cmbTransactionType.SelectedIndex == -1)
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            decimal amount = Convert.ToDecimal(txtAmount.Text);
            string transactionType = cmbTransactionType.SelectedItem.ToString();
            string accountNumber = cmbAccount.SelectedValue.ToString();
            decimal newBalance;

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();

                string balanceQuery = "SELECT Balance FROM [Account] WHERE AccountNumber = ?";
                OleDbCommand balanceCmd = new OleDbCommand(balanceQuery, conn);
                balanceCmd.Parameters.AddWithValue("?", accountNumber);
                decimal currentBalance = Convert.ToDecimal(balanceCmd.ExecuteScalar());

                if (transactionType == "Deposit")
                    newBalance = currentBalance + amount;
                else if (transactionType == "Withdrawal")
                {
                    if (currentBalance < amount)
                    {
                        MessageBox.Show("Insufficient funds.");
                        return;
                    }
                    newBalance = currentBalance - amount;
                }
                else
                {
                    MessageBox.Show("Invalid transaction type.");
                    return;
                }

                string updateBalanceQuery = "UPDATE [Account] SET Balance = ? WHERE AccountNumber = ?";
                OleDbCommand updateCmd = new OleDbCommand(updateBalanceQuery, conn);
                updateCmd.Parameters.AddWithValue("?", newBalance);
                updateCmd.Parameters.AddWithValue("?", accountNumber);
                updateCmd.ExecuteNonQuery();

                string insertQuery = "INSERT INTO [Transaction] (TransactionType, Amount, AccountNumber) VALUES (?, ?, ?)";
                OleDbCommand insertCmd = new OleDbCommand(insertQuery, conn);
                insertCmd.Parameters.AddWithValue("?", transactionType);
                insertCmd.Parameters.AddWithValue("?", amount);
                insertCmd.Parameters.AddWithValue("?", accountNumber);
                insertCmd.ExecuteNonQuery();

                MessageBox.Show("Transaction added successfully.");
                LoadTransactions();
                LoadTransactionIDs();
                ClearFields();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (cmbTransactionID.SelectedIndex == -1 || cmbAccount.SelectedIndex == -1 || txtAmount.Text == "" || cmbTransactionType.SelectedIndex == -1)
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            string transactionID = cmbTransactionID.SelectedValue.ToString();
            string transactionType = cmbTransactionType.SelectedItem.ToString();
            decimal amount = Convert.ToDecimal(txtAmount.Text);
            string accountNumber = cmbAccount.SelectedValue.ToString();

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();

                string updateQuery = "UPDATE [Transaction] SET TransactionType = ?, Amount = ?, AccountNumber = ? WHERE TransactionID = ?";
                OleDbCommand cmd = new OleDbCommand(updateQuery, conn);
                cmd.Parameters.AddWithValue("?", transactionType);
                cmd.Parameters.AddWithValue("?", amount);
                cmd.Parameters.AddWithValue("?", accountNumber);
                cmd.Parameters.AddWithValue("?", transactionID);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Transaction updated successfully.");
                LoadTransactions();
                LoadTransactionIDs();
                ClearFields();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (cmbTransactionID.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a transaction to delete.");
                return;
            }

            string transactionID = cmbTransactionID.SelectedValue.ToString();

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();

                string deleteQuery = "DELETE FROM [Transaction] WHERE TransactionID = ?";
                OleDbCommand cmd = new OleDbCommand(deleteQuery, conn);
                cmd.Parameters.AddWithValue("?", transactionID);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Transaction deleted successfully.");
                LoadTransactions();
                LoadTransactionIDs();
                ClearFields();
            }
        }

        private void cmbTransactionID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbTransactionID.SelectedIndex == -1)
                return;

            string transactionID = cmbTransactionID.SelectedValue.ToString();

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM [Transaction] WHERE TransactionID = ?";
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("?", transactionID);
                OleDbDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    cmbTransactionType.SelectedItem = reader["TransactionType"].ToString();
                    txtAmount.Text = reader["Amount"].ToString();
                    cmbAccount.SelectedValue = reader["AccountNumber"].ToString();
                }
            }
        }

        private void dgvTransaction_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvTransaction.Rows[e.RowIndex];
                cmbTransactionID .Text = row.Cells["TransactionID"].Value.ToString();
                cmbTransactionType .Text = row.Cells["TransactionType"].Value.ToString();
                txtAmount .Text = row.Cells["Amount"].Value.ToString();
                cmbAccount.SelectedValue = row.Cells["AccountNumber"].Value.ToString();
        }
    }
}

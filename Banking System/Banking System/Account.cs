using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Banking_System
{
    public partial class Account : Form
    {
        string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Bank.accdb";

        public Account()
        {
            InitializeComponent();
            LoadAccounts();
        }

        private void LoadAccounts()
        {
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                string query = "SELECT [AccountNumber], [AccountType], [Balance] FROM [Account]";
                OleDbDataAdapter da = new OleDbDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvAccount.DataSource = dt;
            }
        }

        private void ClearFields()
        {
            txtAccountNumber.Clear();
            cmbAccountType.SelectedIndex = -1;
            txtBalance.Clear();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtAccountNumber.Text == "" || cmbAccountType.Text == "" || txtBalance.Text == "")
            {
                MessageBox.Show("Please fill all fields.");
                return;
            }

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                string query = "INSERT INTO [Account] ([AccountNumber], [AccountType], [Balance]) VALUES (?, ?, ?)";
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("?", txtAccountNumber.Text);
                cmd.Parameters.AddWithValue("?", cmbAccountType.Text);
                cmd.Parameters.AddWithValue("?", Convert.ToDecimal(txtBalance.Text));

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Account saved successfully.");
                    LoadAccounts();
                    ClearFields();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (txtAccountNumber.Text == "")
            {
                MessageBox.Show("Please select an account from the table to edit.");
                return;
            }

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                string query = "UPDATE [Account] SET [AccountType] = ?, [Balance] = ? WHERE [AccountNumber] = ?";
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("?", cmbAccountType.Text);
                cmd.Parameters.AddWithValue("?", Convert.ToDecimal(txtBalance.Text));
                cmd.Parameters.AddWithValue("?", txtAccountNumber.Text);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Account updated successfully.");
                    LoadAccounts();
                    ClearFields();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (txtAccountNumber.Text == "")
            {
                MessageBox.Show("Please select an account from the table to delete.");
                return;
            }

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                string query = "DELETE FROM [Account] WHERE [AccountNumber] = ?";
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("?", txtAccountNumber.Text);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Account deleted successfully.");
                    LoadAccounts();
                    ClearFields();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void dgvAccount_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvAccount.Rows[e.RowIndex];
                txtAccountNumber.Text = row.Cells["AccountNumber"].Value.ToString();
                cmbAccountType.Text = row.Cells["AccountType"].Value.ToString();
                txtBalance.Text = row.Cells["Balance"].Value.ToString();
            }
        }
    }
}

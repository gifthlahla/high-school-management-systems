using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace ATM_Management_System
{
    public partial class Account : Form
    {
        private string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=ATM.accdb";

        public Account()
        {
            InitializeComponent();
            LoadAccounts();
        }

        private void LoadAccounts()
        {
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                string query = "SELECT * FROM Account";
                OleDbDataAdapter da = new OleDbDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvAccounts.DataSource = dt;
            }
        }

        private void ClearFields()
        {
            txtAccountID.Clear();
            cmbAccountType.SelectedIndex = 0; // Reset ComboBox to default selection
            txtBalance.Clear();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtAccountID.Text == "" || cmbAccountType.SelectedIndex == -1 || txtBalance.Text == "")
            {
                MessageBox.Show("Please fill all fields.");
                return;
            }

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                string query = "INSERT INTO Account (AccountNumber, AccountType, Balance) VALUES (@id, @type, @balance)"; // Updated field name
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", txtAccountID.Text); // Using AccountNumber instead of AccountID
                cmd.Parameters.AddWithValue("@type", cmbAccountType.SelectedItem.ToString());
                cmd.Parameters.AddWithValue("@balance", Convert.ToDecimal(txtBalance.Text));
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }

            LoadAccounts();
            ClearFields();
            MessageBox.Show("Account added.");
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (txtAccountID.Text == "")
            {
                MessageBox.Show("Please select an account to update.");
                return;
            }

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                string query = "UPDATE Account SET AccountType = @type, Balance = @balance WHERE AccountNumber = @id"; // Updated field name
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("@type", cmbAccountType.SelectedItem.ToString());
                cmd.Parameters.AddWithValue("@balance", Convert.ToDecimal(txtBalance.Text));
                cmd.Parameters.AddWithValue("@id", txtAccountID.Text); // Using AccountNumber instead of AccountID
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }

            LoadAccounts();
            ClearFields();
            MessageBox.Show("Account updated.");
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (txtAccountID.Text == "")
            {
                MessageBox.Show("Please select an account to delete.");
                return;
            }

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                string query = "DELETE FROM Account WHERE AccountNumber = @id"; // Updated field name
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", txtAccountID.Text); // Using AccountNumber instead of AccountID
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }

            LoadAccounts();
            ClearFields();
            MessageBox.Show("Account deleted.");
        }

        private void dgvAccounts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtAccountID.Text = dgvAccounts.Rows[e.RowIndex].Cells["AccountNumber"].Value.ToString(); // Updated field name
                cmbAccountType.SelectedItem = dgvAccounts.Rows[e.RowIndex].Cells["AccountType"].Value.ToString();
                txtBalance.Text = dgvAccounts.Rows[e.RowIndex].Cells["Balance"].Value.ToString();
            }
        }
    }
}

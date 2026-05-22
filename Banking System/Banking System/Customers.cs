using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Banking_System
{
    public partial class Customers : Form
    {
        string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Bank.accdb";

        public Customers()
        {
            InitializeComponent();
            LoadCustomers();
            LoadAccounts();
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

        private void LoadCustomers()
        {
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                string query = "SELECT * FROM [Customer]";
                OleDbDataAdapter da = new OleDbDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvCustomers.DataSource = dt;
            }
        }

        private void ClearFields()
        {
            txtCustomerID.Clear();
            txtName.Clear();
            txtPhone.Clear();
            cmbAccountNumber.SelectedIndex = -1;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtName.Text == "" || txtPhone.Text == "" || cmbAccountNumber.SelectedIndex == -1)
            {
                MessageBox.Show("Please fill all fields.");
                return;
            }

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                string query = "INSERT INTO [Customer] ([Name], [Phone], [AccountNumber]) VALUES (@name, @phone, @account)";
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("@name", txtName.Text);
                cmd.Parameters.AddWithValue("@phone", txtPhone.Text);
                cmd.Parameters.AddWithValue("@account", cmbAccountNumber.SelectedValue.ToString());

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Customer added successfully.");
                    LoadCustomers();
                    ClearFields();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (txtCustomerID.Text == "")
            {
                MessageBox.Show("Please select a customer to update.");
                return;
            }

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                string query = "UPDATE [Customer] SET [Name] = @name, [Phone] = @phone, [AccountNumber] = @account WHERE [CustomerID] = @id";
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("@name", txtName.Text);
                cmd.Parameters.AddWithValue("@phone", txtPhone.Text);
                cmd.Parameters.AddWithValue("@account", cmbAccountNumber.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@id", txtCustomerID.Text);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Customer updated successfully.");
                    LoadCustomers();
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
            if (txtCustomerID.Text == "")
            {
                MessageBox.Show("Please select a customer to delete.");
                return;
            }

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                string query = "DELETE FROM [Customer] WHERE [CustomerID] = @id";
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", txtCustomerID.Text);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Customer deleted successfully.");
                    LoadCustomers();
                    ClearFields();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void dgvCustomers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvCustomers.Rows[e.RowIndex];
                txtCustomerID.Text = row.Cells["CustomerID"].Value.ToString();
                txtName.Text = row.Cells["Name"].Value.ToString();
                txtPhone.Text = row.Cells["Phone"].Value.ToString();
                cmbAccountNumber.SelectedValue = row.Cells["AccountNumber"].Value.ToString();
            }
        }
    }
}

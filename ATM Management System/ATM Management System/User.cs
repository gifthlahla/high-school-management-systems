using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace ATM_Management_System
{
    public partial class User : Form
    {
        string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=ATM.accdb";

        public User()
        {
            InitializeComponent();
            LoadUsers();
            LoadAccounts();
        }

        private void LoadAccounts()
        {
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                string query = "SELECT AccountNumber FROM Account";
                OleDbDataAdapter da = new OleDbDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                cmbAccountNumber.DataSource = dt;
                cmbAccountNumber.DisplayMember = "AccountNumber";
                cmbAccountNumber.ValueMember = "AccountNumber";
            }
        }

        private void LoadUsers()
        {
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                string query = "SELECT * FROM [User]";
                OleDbDataAdapter da = new OleDbDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvUsers.DataSource = dt;
            }
        }

        private void ClearFields()
        {
            txtUserID.Clear();
            txtName.Clear();
            txtPhone.Clear();
            cmbAccountNumber.SelectedIndex = -1;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtName.Text == "" || txtPhone.Text == "" || cmbAccountNumber.SelectedIndex == -1)
            {
                MessageBox.Show("Please fill all fields.");
                return;
            }

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                string query = "INSERT INTO [User] (Name, Phone, AccountNumber) VALUES (@name, @phone, @account)";
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("@name", txtName.Text);
                cmd.Parameters.AddWithValue("@phone", txtPhone.Text);
                cmd.Parameters.AddWithValue("@account", cmbAccountNumber.SelectedValue.ToString());

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("User added successfully.");
                    LoadUsers();
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
            if (txtUserID.Text == "")
            {
                MessageBox.Show("Please select a user to update.");
                return;
            }

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                string query = "UPDATE [User] SET Name = @name, Phone = @phone, AccountNumber = @account WHERE UserID = @id";
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("@name", txtName.Text);
                cmd.Parameters.AddWithValue("@phone", txtPhone.Text);
                cmd.Parameters.AddWithValue("@account", cmbAccountNumber.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@id", txtUserID.Text);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("User updated successfully.");
                    LoadUsers();
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
            if (txtUserID.Text == "")
            {
                MessageBox.Show("Please select a user to delete.");
                return;
            }

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                string query = "DELETE FROM [User] WHERE UserID = @id";
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", txtUserID.Text);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("User deleted successfully.");
                    LoadUsers();
                    ClearFields();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void dgvUsers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvUsers.Rows[e.RowIndex];
                txtUserID.Text = row.Cells["UserID"].Value.ToString();
                txtName.Text = row.Cells["Name"].Value.ToString();
                txtPhone.Text = row.Cells["Phone"].Value.ToString();
                cmbAccountNumber.SelectedValue = row.Cells["AccountNumber"].Value.ToString();
            }
        }
    }
}

using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Fees_Payment_System
{
    public partial class Account : Form
    {
        private string selectedUsername = null;
        private OleDbConnection conn;
        private string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=FeesPayment.accdb";

        public Account()
        {
            InitializeComponent();
            conn = new OleDbConnection(connectionString);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
            Fees_Payment_System.Menu.GoToPreviousButton(); // Adjust according to your menu's structure
        }

        private void LoadAccounts()
        {
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                string query = "SELECT * FROM [User]";
                OleDbDataAdapter da = new OleDbDataAdapter(query, conn);
                DataTable dt = new DataTable();
                try
                {
                    conn.Open();
                    da.Fill(dt);
                    dgvAccounts.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading data: " + ex.Message);
                }
            }
        }

        private void Account_Load(object sender, EventArgs e)
        {
            // Load data on form load
            LoadAccounts();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtUsername.Text == "" || txtPassword.Text == "")
            {
                MessageBox.Show("Please enter username and password.");
                return;
            }

            try
            {
                conn.Open();
                string query = "INSERT INTO [User] ([Username], [Password]) VALUES (@user, @pass)";
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("@user", txtUsername.Text);
                cmd.Parameters.AddWithValue("@pass", txtPassword.Text);
                cmd.ExecuteNonQuery();

                MessageBox.Show("User added.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding user (maybe username already exists): " + ex.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
                LoadAccounts();  // Reload data after adding
                btnClear.PerformClick();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (selectedUsername == null)
            {
                MessageBox.Show("Please select a user to update.");
                return;
            }

            try
            {
                conn.Open();
                string query = "UPDATE [User] SET [Password] = @pass WHERE [Username] = @user";
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("@pass", txtPassword.Text);
                cmd.Parameters.AddWithValue("@user", selectedUsername);
                cmd.ExecuteNonQuery();

                MessageBox.Show("User updated.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating user: " + ex.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
                LoadAccounts();  // Reload data after updating
                btnClear.PerformClick();
            }
        }

        private void dgvAccounts_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvAccounts.Rows[e.RowIndex].Cells[0].Value != null)
            {
                DataGridViewRow row = dgvAccounts.Rows[e.RowIndex];
                selectedUsername = row.Cells[0].Value.ToString();
                txtUsername.Text = selectedUsername;
                txtPassword.Text = row.Cells[1].Value.ToString();

                dgvAccounts.ClearSelection();
                row.Selected = true;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtUsername.Clear();
            txtPassword.Clear();
            selectedUsername = null;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvAccounts.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a record to delete.");
                return;
            }

            int selectedRowIndex = dgvAccounts.SelectedCells[0].RowIndex;
            DataGridViewRow selectedRow = dgvAccounts.Rows[selectedRowIndex];
            string selectedUsername = selectedRow.Cells["Username"].Value.ToString();

            DialogResult result = MessageBox.Show("Are you sure you want to delete this account?", "Delete Account", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    string query = "DELETE FROM [User] WHERE [Username] = ?";
                    OleDbCommand cmd = new OleDbCommand(query, conn);
                    cmd.Parameters.AddWithValue("?", selectedUsername);

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Account deleted successfully!");
                        LoadAccounts();  // Reload data after deletion
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
        }
    }
}

using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace ATM_Management_System
{
    public partial class Card : Form
    {
        string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=ATM.accdb";

        public Card()
        {
            InitializeComponent();
            LoadAccounts();
            LoadCards();
        }

        // Method to load Account Numbers into the ComboBox
        private void LoadAccounts()
        {
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                string query = "SELECT AccountNumber FROM Account";
                OleDbDataAdapter da = new OleDbDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                cmbAccount.DataSource = dt;
                cmbAccount.DisplayMember = "AccountNumber";  // Display AccountNumber in ComboBox
                cmbAccount.ValueMember = "AccountNumber";    // Use AccountNumber as Value
            }
        }

        // Method to load all cards into the DataGridView
        private void LoadCards()
        {
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                string query = "SELECT * FROM Card";
                OleDbDataAdapter da = new OleDbDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvCard.DataSource = dt;
            }
        }

        // Method to clear all fields after operation
        private void ClearFields()
        {
            txtCardNumber.Clear();
            txtPin.Clear();
            dtpExpiryDate.Value = DateTime.Now;  // Reset DateTimePicker to current date
            cmbAccount.SelectedIndex = -1;  // Reset ComboBox
        }

        // Button click event to add a new card
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtCardNumber.Text == "" || txtPin.Text == "" || cmbAccount.SelectedIndex == -1)
            {
                MessageBox.Show("All fields are required.");
                return;
            }

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                string query = "INSERT INTO Card (CardNumber, ExpiryDate, Pin, AccountNumber) VALUES (@card, @date, @pin, @account)";
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("@card", txtCardNumber.Text);
                cmd.Parameters.AddWithValue("@date", dtpExpiryDate.Value);
                cmd.Parameters.AddWithValue("@pin", txtPin.Text);
                cmd.Parameters.AddWithValue("@account", cmbAccount.SelectedValue.ToString());  // AccountNumber from ComboBox

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Card added successfully.");
                    LoadCards();  // Refresh the DataGridView
                    ClearFields(); // Clear the input fields
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        // Button click event to update an existing card
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (txtCardNumber.Text == "")
            {
                MessageBox.Show("Select a card to update.");
                return;
            }

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                string query = "UPDATE Card SET ExpiryDate = @date, Pin = @pin, AccountNumber = @account WHERE CardNumber = @card";
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("@date", dtpExpiryDate.Value);
                cmd.Parameters.AddWithValue("@pin", txtPin.Text);
                cmd.Parameters.AddWithValue("@account", cmbAccount.SelectedValue.ToString());  // AccountNumber from ComboBox
                cmd.Parameters.AddWithValue("@card", txtCardNumber.Text);  // CardNumber from TextBox

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Card updated successfully.");
                    LoadCards();  // Refresh the DataGridView
                    ClearFields(); // Clear the input fields
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        // Button click event to delete a card
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (txtCardNumber.Text == "")
            {
                MessageBox.Show("Select a card to delete.");
                return;
            }

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                string query = "DELETE FROM Card WHERE CardNumber = @card";
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("@card", txtCardNumber.Text);  // CardNumber from TextBox

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Card deleted successfully.");
                    LoadCards();  // Refresh the DataGridView
                    ClearFields(); // Clear the input fields
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        // CellClick event to load the selected card's details into the input fields
        private void dgvCard_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvCard.Rows[e.RowIndex];
                txtCardNumber.Text = row.Cells["CardNumber"].Value.ToString();
                dtpExpiryDate.Value = Convert.ToDateTime(row.Cells["ExpiryDate"].Value);
                txtPin.Text = row.Cells["Pin"].Value.ToString();
                cmbAccount.SelectedValue = row.Cells["AccountNumber"].Value.ToString();
            }
        }
    }
}

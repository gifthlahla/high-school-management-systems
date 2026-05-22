using System;
using System.Data;
using System.Windows.Forms;
using System.Data.OleDb;

namespace Fees_Payment_System
{
    public partial class Fee : Form
    {
        // Assuming you have a connection string defined somewhere
        private OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=FeesPayment.accdb");

        public Fee()
        {
            InitializeComponent();
            LoadFees();
        }

        // Method to load all fees into the DataGridView
        private void LoadFees()
        {
            try
            {
                conn.Open();
                string query = "SELECT * FROM Fees";
                OleDbDataAdapter adapter = new OleDbDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dgvFees.DataSource = dt;
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading fees: " + ex.Message);
            }
        }

        // Save button click event
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtFeeType.Text) || string.IsNullOrEmpty(txtAmount.Text) || string.IsNullOrEmpty(c.Text))
                {
                    MessageBox.Show("Please fill in all the fields.");
                    return;
                }

                string query = "INSERT INTO Fees (FeeID, FeeType, Amount, DueDate) VALUES (?, ?, ?, ?)";
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("?", c.Text); // Fee ID
                cmd.Parameters.AddWithValue("?", txtFeeType.Text); // Fee Type
                cmd.Parameters.AddWithValue("?", txtAmount.Text); // Amount
                cmd.Parameters.AddWithValue("?", dtpDueDate.Value); // Due Date

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

                MessageBox.Show("Fee saved successfully.");
                LoadFees(); // Refresh the DataGridView
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving fee: " + ex.Message);
            }
        }

        // Update button click event
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtFeeType.Text) || string.IsNullOrEmpty(txtAmount.Text) || string.IsNullOrEmpty(c.Text))
                {
                    MessageBox.Show("Please fill in all the fields.");
                    return;
                }

                string query = "UPDATE Fees SET FeeType = ?, Amount = ?, DueDate = ? WHERE FeeID = ?";
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("?", txtFeeType.Text); // Fee Type
                cmd.Parameters.AddWithValue("?", txtAmount.Text); // Amount
                cmd.Parameters.AddWithValue("?", dtpDueDate.Value); // Due Date
                cmd.Parameters.AddWithValue("?", c.Text); // Fee ID

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

                MessageBox.Show("Fee updated successfully.");
                LoadFees(); // Refresh the DataGridView
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating fee: " + ex.Message);
            }
        }

        // Delete button click event
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(c.Text))
                {
                    MessageBox.Show("Please enter a Fee ID to delete.");
                    return;
                }

                string query = "DELETE FROM Fees WHERE FeeID = ?";
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("?", c.Text); // Fee ID

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

                MessageBox.Show("Fee deleted successfully.");
                LoadFees(); // Refresh the DataGridView
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting fee: " + ex.Message);
            }
        }

        // DataGridView cell click event to load selected row data into form fields
        private void dgvFees_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    DataGridViewRow row = dgvFees.Rows[e.RowIndex];
                    c.Text = row.Cells["FeeID"].Value.ToString(); // Fee ID
                    txtFeeType.Text = row.Cells["FeeType"].Value.ToString(); // Fee Type
                    txtAmount.Text = row.Cells["Amount"].Value.ToString(); // Amount
                    dtpDueDate.Value = Convert.ToDateTime(row.Cells["DueDate"].Value); // Due Date
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message);
            }
        }
    }
}

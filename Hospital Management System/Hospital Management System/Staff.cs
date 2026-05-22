using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Hospital_Management_System
{
    public partial class Staff : Form
    {
        private string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=HospitalManagement.accdb";

        public Staff()
        {
            InitializeComponent();
        }

        // Load the data into DataGridView when the form loads
        private void Staff_Load(object sender, EventArgs e)
        {
            LoadStaffData();
        }

        // Load staff data into DataGridView
        private void LoadStaffData()
        {
            try
            {
                string query = "SELECT * FROM Staff";

                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);
                    dgvStaff.DataSource = dataTable;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        // Save new staff member
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string query = "INSERT INTO Staff (StaffName, Gender, Phone, Address, Position, DateJoined) VALUES (@StaffName, @Gender, @Phone, @Address, @Position, @DateJoined)";

                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    OleDbCommand command = new OleDbCommand(query, connection);
                    command.Parameters.AddWithValue("@StaffName", txtStaffName.Text);
                    command.Parameters.AddWithValue("@Gender", cmbGender.SelectedItem.ToString());
                    command.Parameters.AddWithValue("@Phone", txtPhone.Text);
                    command.Parameters.AddWithValue("@Address", txtAddress.Text);
                    command.Parameters.AddWithValue("@Position", cmbPosition.SelectedItem.ToString());
                    command.Parameters.AddWithValue("@DateJoined", dtpDateJoined.Value);

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();

                    MessageBox.Show("Staff member added successfully.");
                    LoadStaffData(); // Reload data grid view
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        // Update staff member's information
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                // Ensure a row is selected
                if (dgvStaff.SelectedRows.Count > 0)
                {
                    int staffID = Convert.ToInt32(dgvStaff.SelectedRows[0].Cells[0].Value); // Assuming StaffID is in the first column

                    string query = "UPDATE Staff SET StaffName = @StaffName, Gender = @Gender, Phone = @Phone, Address = @Address, Position = @Position, DateJoined = @DateJoined WHERE StaffID = @StaffID";

                    using (OleDbConnection connection = new OleDbConnection(connectionString))
                    {
                        OleDbCommand command = new OleDbCommand(query, connection);
                        command.Parameters.AddWithValue("@StaffName", txtStaffName.Text);
                        command.Parameters.AddWithValue("@Gender", cmbGender.SelectedItem.ToString());
                        command.Parameters.AddWithValue("@Phone", txtPhone.Text);
                        command.Parameters.AddWithValue("@Address", txtAddress.Text);
                        command.Parameters.AddWithValue("@Position", cmbPosition.SelectedItem.ToString());
                        command.Parameters.AddWithValue("@DateJoined", dtpDateJoined.Value);
                        command.Parameters.AddWithValue("@StaffID", staffID);

                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();

                        MessageBox.Show("Staff member updated successfully.");
                        LoadStaffData(); // Reload data grid view
                    }
                }
                else
                {
                    MessageBox.Show("Please select a staff member to update.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        // Delete selected staff member
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                // Ensure a row is selected
                if (dgvStaff.SelectedRows.Count > 0)
                {
                    int staffID = Convert.ToInt32(dgvStaff.SelectedRows[0].Cells[0].Value); // Assuming StaffID is in the first column

                    string query = "DELETE FROM Staff WHERE StaffID = @StaffID";

                    using (OleDbConnection connection = new OleDbConnection(connectionString))
                    {
                        OleDbCommand command = new OleDbCommand(query, connection);
                        command.Parameters.AddWithValue("@StaffID", staffID);

                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();

                        MessageBox.Show("Staff member deleted successfully.");
                        LoadStaffData(); // Reload data grid view
                    }
                }
                else
                {
                    MessageBox.Show("Please select a staff member to delete.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        // Event when selecting a row in the DataGridView
        private void dgvStaff_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Populate the text boxes with the selected row's data (for update)
            if (dgvStaff.SelectedRows.Count > 0)
            {
                txtStaffName.Text = dgvStaff.SelectedRows[0].Cells[1].Value.ToString();
                cmbGender.SelectedItem = dgvStaff.SelectedRows[0].Cells[2].Value.ToString();
                txtPhone.Text = dgvStaff.SelectedRows[0].Cells[3].Value.ToString();
                txtAddress.Text = dgvStaff.SelectedRows[0].Cells[4].Value.ToString();
                cmbPosition.SelectedItem = dgvStaff.SelectedRows[0].Cells[5].Value.ToString();
                dtpDateJoined.Value = Convert.ToDateTime(dgvStaff.SelectedRows[0].Cells[6].Value);
            }
        }
    }
}

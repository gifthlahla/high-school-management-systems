using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Hospital_Management_System
{
    public partial class Medicines : Form
    {
        private string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=HospitalManagement.accdb";

        public Medicines()
        {
            InitializeComponent();
        }

        // Load medicines data into DataGridView
        private void Medicines_Load(object sender, EventArgs e)
        {
            LoadMedicinesData();
        }

        // Method to load medicines data from the database
        private void LoadMedicinesData()
        {
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT * FROM Medicines";
                    OleDbDataAdapter adapter = new OleDbDataAdapter(query, conn);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    dgvMedicine.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading data: " + ex.Message);
                }
            }
        }

        // Save new medicine data to the database
        private void btnSave_Click(object sender, EventArgs e)
        {
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO Medicines (MedicineName, MedicineType, DateMade, ExpDate) " +
                                   "VALUES (@MedicineName, @MedicineType, @DateMade, @ExpDate)";
                    OleDbCommand cmd = new OleDbCommand(query, conn);

                    cmd.Parameters.AddWithValue("@MedicineName", txtMedicine.Text);
                    cmd.Parameters.AddWithValue("@MedicineType", cmbType.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@DateMade", dtpDateMade.Value);
                    cmd.Parameters.AddWithValue("@ExpDate", dtpExpDate.Value);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Medicine data saved successfully!");
                    LoadMedicinesData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error saving data: " + ex.Message);
                }
            }
        }

        // Update existing medicine data
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvMedicine.SelectedRows.Count == 1)
            {
                int medicineId = Convert.ToInt32(dgvMedicine.SelectedRows[0].Cells["MedicineID"].Value);

                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        string query = "UPDATE Medicines SET MedicineName = @MedicineName, MedicineType = @MedicineType, " +
                                       "DateMade = @DateMade, ExpDate = @ExpDate WHERE MedicineID = @MedicineID";

                        OleDbCommand cmd = new OleDbCommand(query, conn);

                        cmd.Parameters.AddWithValue("@MedicineName", txtMedicine.Text);
                        cmd.Parameters.AddWithValue("@MedicineType", cmbType.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@DateMade", dtpDateMade.Value);
                        cmd.Parameters.AddWithValue("@ExpDate", dtpExpDate.Value);
                        cmd.Parameters.AddWithValue("@MedicineID", medicineId);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Medicine data updated successfully!");
                        LoadMedicinesData();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error updating data: " + ex.Message);
                    }
                }
            }
        }

        // Delete selected medicine record
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvMedicine.SelectedRows.Count == 1)
            {
                int medicineId = Convert.ToInt32(dgvMedicine.SelectedRows[0].Cells["MedicineID"].Value);

                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        string query = "DELETE FROM Medicines WHERE MedicineID = @MedicineID";
                        OleDbCommand cmd = new OleDbCommand(query, conn);
                        cmd.Parameters.AddWithValue("@MedicineID", medicineId);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Medicine data deleted successfully!");
                        LoadMedicinesData();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error deleting data: " + ex.Message);
                    }
                }
            }
        }

        // Handle row click to load selected medicine data into the form
        private void dgvMedicine_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvMedicine.Rows[e.RowIndex];
                txtMedicine.Text = row.Cells["MedicineName"].Value.ToString();
                cmbType.SelectedItem = row.Cells["MedicineType"].Value.ToString();
                dtpDateMade.Value = Convert.ToDateTime(row.Cells["DateMade"].Value);
                dtpExpDate.Value = Convert.ToDateTime(row.Cells["ExpDate"].Value);
            }
        }
    }
}

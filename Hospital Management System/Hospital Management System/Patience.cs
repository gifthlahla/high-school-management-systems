using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Hospital_Management_System
{
    public partial class Patients : Form
    {
        // Connection string to connect to your Access database
        private string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=HospitalManagement.accdb";

        public Patients()
        {
            InitializeComponent();
        }

        // Load patient data into the DataGridView
        private void Patients_Load(object sender, EventArgs e)
        {
            LoadPatientsData();
        }

        // Method to load patient data from the database
        private void LoadPatientsData()
        {
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT * FROM Patients";
                    OleDbDataAdapter adapter = new OleDbDataAdapter(query, conn);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    dgvPatients.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading data: " + ex.Message);
                }
            }
        }

        // Save new patient data to the database
        private void btnSave_Click(object sender, EventArgs e)
        {
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO Patients (PatientName, Address, Gender, BloodGroup, DOB, Diagnostics) " +
                                   "VALUES (@PatientName, @Address, @Gender, @BloodGroup, @DOB, @Diagnostics)";
                    OleDbCommand cmd = new OleDbCommand(query, conn);

                    cmd.Parameters.AddWithValue("@PatientName", txtPatientName.Text);
                    cmd.Parameters.AddWithValue("@Address", txtAddress.Text);
                    cmd.Parameters.AddWithValue("@Gender", cmbGender.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@BloodGroup", cmbBloodGroup.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@DOB", dtpDOB.Value);
                    cmd.Parameters.AddWithValue("@Diagnostics", txtDiagnostics.Text);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Patient data saved successfully!");
                    LoadPatientsData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error saving data: " + ex.Message);
                }
            }
        }

        // Update existing patient data
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvPatients.SelectedRows.Count == 1)
            {
                int patientId = Convert.ToInt32(dgvPatients.SelectedRows[0].Cells["PatientID"].Value);

                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        string query = "UPDATE Patients SET PatientName = @PatientName, Address = @Address, Gender = @Gender, " +
                                       "BloodGroup = @BloodGroup, DOB = @DOB, Diagnostics = @Diagnostics WHERE PatientID = @PatientID";

                        OleDbCommand cmd = new OleDbCommand(query, conn);

                        cmd.Parameters.AddWithValue("@PatientName", txtPatientName.Text);
                        cmd.Parameters.AddWithValue("@Address", txtAddress.Text);
                        cmd.Parameters.AddWithValue("@Gender", cmbGender.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@BloodGroup", cmbBloodGroup.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@DOB", dtpDOB.Value);
                        cmd.Parameters.AddWithValue("@Diagnostics", txtDiagnostics.Text);
                        cmd.Parameters.AddWithValue("@PatientID", patientId);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Patient data updated successfully!");
                        LoadPatientsData();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error updating data: " + ex.Message);
                    }
                }
            }
        }

        // Delete selected patient record
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvPatients.SelectedRows.Count == 1)
            {
                int patientId = Convert.ToInt32(dgvPatients.SelectedRows[0].Cells["PatientID"].Value);

                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        string query = "DELETE FROM Patients WHERE PatientID = @PatientID";
                        OleDbCommand cmd = new OleDbCommand(query, conn);
                        cmd.Parameters.AddWithValue("@PatientID", patientId);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Patient data deleted successfully!");
                        LoadPatientsData();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error deleting data: " + ex.Message);
                    }
                }
            }
        }

        // Handle row click to load selected patient data into the form
        private void dgvPatients_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvPatients.Rows[e.RowIndex];
                txtPatientName.Text = row.Cells["PatientName"].Value.ToString();
                txtAddress.Text = row.Cells["Address"].Value.ToString();
                cmbGender.SelectedItem = row.Cells["Gender"].Value.ToString();
                cmbBloodGroup.SelectedItem = row.Cells["BloodGroup"].Value.ToString();
                dtpDOB.Value = Convert.ToDateTime(row.Cells["DOB"].Value);
                txtDiagnostics.Text = row.Cells["Diagnostics"].Value.ToString();
            }
        }
    }
}

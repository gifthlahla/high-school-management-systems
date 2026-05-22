using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Hospital_Management_System
{
    public partial class Diagnostics : Form
    {
        // Connection string to the Access database
        private string connStr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=HospitalManagement.accdb";
        private OleDbConnection conn;

        public Diagnostics()
        {
            InitializeComponent();
            conn = new OleDbConnection(connStr);
        }

        private void Diagnostics_Load(object sender, EventArgs e)
        {
            LoadDiagnoses();
        }

        // Load all diagnostics records into the DataGridView
        private void LoadDiagnoses()
        {
            using (OleDbConnection conn = new OleDbConnection(connStr))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT * FROM [Diagnostics]";
                    OleDbDataAdapter adapter = new OleDbDataAdapter(query, conn);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    dgvDiagnistics.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading data: " + ex.Message);
                }
            }
        }

        // Save the new diagnosis
        private void btnSave_Click(object sender, EventArgs e)
        {
            string patientName = txtPatientName.Text;
            string symptoms = lblSymptoms.Text;
            string diagnosis = lblDiagnosis.Text;
            string medicine = cmbMedicine.Text;

            string query = "INSERT INTO [Diagnostics] (PatientName, Symptoms, Diagnosis, Medicine) VALUES (?, ?, ?, ?)";
            OleDbCommand cmd = new OleDbCommand(query, conn);
            cmd.Parameters.AddWithValue("?", patientName);
            cmd.Parameters.AddWithValue("?", symptoms);
            cmd.Parameters.AddWithValue("?", diagnosis);
            cmd.Parameters.AddWithValue("?", medicine);

            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                MessageBox.Show("Diagnostic information saved successfully.");
                LoadDiagnoses(); // Reload data grid
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        // Update an existing diagnosis
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string patientName = txtPatientName.Text;
            string symptoms = lblSymptoms.Text;
            string diagnosis = lblDiagnosis.Text;
            string medicine = cmbMedicine.Text;

            int id = Convert.ToInt32(dgvDiagnistics.CurrentRow.Cells["ID"].Value); // Assuming "ID" is the primary key column

            string query = "UPDATE [Diagnostics] SET PatientName = ?, Symptoms = ?, Diagnosis = ?, Medicine = ? WHERE ID = ?";
            OleDbCommand cmd = new OleDbCommand(query, conn);
            cmd.Parameters.AddWithValue("?", patientName);
            cmd.Parameters.AddWithValue("?", symptoms);
            cmd.Parameters.AddWithValue("?", diagnosis);
            cmd.Parameters.AddWithValue("?", medicine);
            cmd.Parameters.AddWithValue("?", id);

            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                MessageBox.Show("Diagnostic information updated successfully.");
                LoadDiagnoses(); // Reload data grid
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        // Delete a diagnostic record
        private void btnDelete_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(dgvDiagnistics.CurrentRow.Cells["ID"].Value); // Assuming "ID" is the primary key column

            string query = "DELETE FROM [Diagnostics] WHERE ID = ?";
            OleDbCommand cmd = new OleDbCommand(query, conn);
            cmd.Parameters.AddWithValue("?", id);

            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                MessageBox.Show("Diagnostic record deleted successfully.");
                LoadDiagnoses(); // Reload data grid
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        // This method will be triggered when selecting a patient name
        private void txtPatientName_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Logic for when the patient name selection changes (optional)
        }

        // Handle the cell click event in the DataGridView (optional)
        private void dgvDiagnistics_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // You can implement logic to display selected row's data in the form fields for editing
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvDiagnistics.Rows[e.RowIndex];
                txtPatientName.Text = row.Cells["PatientName"].Value.ToString();
                lblSymptoms.Text = row.Cells["Symptoms"].Value.ToString();
                lblDiagnosis.Text = row.Cells["Diagnosis"].Value.ToString();
                cmbMedicine.Text = row.Cells["Medicine"].Value.ToString();
            }
        }
    }
}

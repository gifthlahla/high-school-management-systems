using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Hospital_Management_System
{
    public partial class DashBoard : Form
    {
        // Connection string to the Access database
        private string connStr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\HospitalManagement.accdb;Persist Security Info=False;";
        private OleDbConnection conn;

        public DashBoard()
        {
            InitializeComponent();
            conn = new OleDbConnection(connStr);
        }

        // Load event handler
        private void DashBoard_Load(object sender, EventArgs e)
        {
            // Update the labels with the patient and staff counts when the form loads
            UpdateCounts();
        }

        // Method to update the patient and staff counts
        private void UpdateCounts()
        {
            try
            {
                // Open the database connection
                conn.Open();

                // SQL query to get the count of patients
                string patientQuery = "SELECT COUNT(*) FROM Patients"; // Assuming there is a Patients table
                OleDbCommand cmd = new OleDbCommand(patientQuery, conn);
                int patientCount = Convert.ToInt32(cmd.ExecuteScalar()); // Get the patient count
                lblPatients.Text = patientCount.ToString(); // Update the Patients label

                // SQL query to get the count of staff
                string staffQuery = "SELECT COUNT(*) FROM Staff"; // Assuming there is a Staff table
                cmd = new OleDbCommand(staffQuery, conn);
                int staffCount = Convert.ToInt32(cmd.ExecuteScalar()); // Get the staff count
                lblStaff.Text = staffCount.ToString(); // Update the Staff label
            }
            catch (Exception ex)
            {
                // Handle any errors that occur during the database operation
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                // Close the database connection
                conn.Close();
            }
        }

        // Paint event handler for panel1 (you can add custom painting code here if needed)
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            // Custom drawing code for panel1 (if needed)
        }
    }
}

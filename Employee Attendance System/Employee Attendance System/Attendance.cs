using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Employee_Attendance_System
{
    public partial class Attendance : Form
    {
        private OleDbConnection conn;
        private OleDbDataAdapter da;
        private DataTable dt;

        public Attendance()
        {
            InitializeComponent();
            // Initialize the database connection
            conn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\EmpAttend.accdb");
        }

        private void Attendance_Load(object sender, EventArgs e)
        {
            // Load data into the DataGridView
            LoadAttendanceData();
            // Populate Employee IDs into the combo box
            LoadEmployeeIDs();
        }

        // Method to load attendance data into the DataGridView
        private void LoadAttendanceData()
        {
            try
            {
                string query = "SELECT AttendanceID, EmployeeID, EmployeeName, Date, Status FROM Attendance";
                da = new OleDbDataAdapter(query, conn);
                dt = new DataTable();
                da.Fill(dt);
                dgvAttendance.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading attendance data: " + ex.Message);
            }
        }

        // Method to load employee IDs into the combo box
        private void LoadEmployeeIDs()
        {
            try
            {
                string query = "SELECT EmployeeID FROM Employees";
                OleDbDataAdapter employeeDa = new OleDbDataAdapter(query, conn);
                DataTable employeeTable = new DataTable();
                employeeDa.Fill(employeeTable);

                cmbEmployeeID.DataSource = employeeTable;
                cmbEmployeeID.DisplayMember = "EmployeeID";
                cmbEmployeeID.ValueMember = "EmployeeID";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading employee IDs: " + ex.Message);
            }
        }

        // Method to handle saving attendance data
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(cmbEmployeeID.Text) || string.IsNullOrEmpty(cmbStatus.Text))
                {
                    MessageBox.Show("Please fill in all the fields.");
                    return;
                }

                // Insert new attendance record into the database
                string query = "INSERT INTO Attendance (EmployeeID, EmployeeName, Date, Status) VALUES (@EmployeeID, @EmployeeName, @Date, @Status)";
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("@EmployeeID", cmbEmployeeID.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@EmployeeName", lblEmployeeName.Text); // Ensure employee name is filled
                cmd.Parameters.AddWithValue("@Date", dtpDate.Value);
                cmd.Parameters.AddWithValue("@Status", cmbStatus.SelectedItem.ToString());

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

                MessageBox.Show("Attendance record saved successfully!");
                LoadAttendanceData(); // Refresh the data grid
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving attendance: " + ex.Message);
            }
        }

        // Method to handle editing an attendance record
        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvAttendance.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please select a record to edit.");
                    return;
                }

                int selectedRowIndex = dgvAttendance.SelectedRows[0].Index;
                string attendanceID = dgvAttendance.Rows[selectedRowIndex].Cells[0].Value.ToString();

                string query = "UPDATE Attendance SET EmployeeID = @EmployeeID, EmployeeName = @EmployeeName, Date = @Date, Status = @Status WHERE AttendanceID = @AttendanceID";
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("@EmployeeID", cmbEmployeeID.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@EmployeeName", lblEmployeeName.Text); // Ensure employee name is filled
                cmd.Parameters.AddWithValue("@Date", dtpDate.Value);
                cmd.Parameters.AddWithValue("@Status", cmbStatus.SelectedItem.ToString());
                cmd.Parameters.AddWithValue("@AttendanceID", attendanceID);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

                MessageBox.Show("Attendance record updated successfully!");
                LoadAttendanceData(); // Refresh the data grid
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating attendance: " + ex.Message);
            }
        }

        // Method to handle deleting an attendance record
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvAttendance.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please select a record to delete.");
                    return;
                }

                int selectedRowIndex = dgvAttendance.SelectedRows[0].Index;
                string attendanceID = dgvAttendance.Rows[selectedRowIndex].Cells[0].Value.ToString();

                string query = "DELETE FROM Attendance WHERE AttendanceID = @AttendanceID";
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("@AttendanceID", attendanceID);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

                MessageBox.Show("Attendance record deleted successfully!");
                LoadAttendanceData(); // Refresh the data grid
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting attendance: " + ex.Message);
            }
        }

        // Handle cell click in the DataGridView to load selected record into form fields
        private void dgvAttendance_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    DataGridViewRow row = dgvAttendance.Rows[e.RowIndex];

                    lblAttendanceID.Text = row.Cells[0].Value.ToString();
                    cmbEmployeeID.SelectedValue = row.Cells[1].Value.ToString();
                    lblEmployeeName.Text = row.Cells[2].Value.ToString();
                    dtpDate.Value = Convert.ToDateTime(row.Cells[3].Value);
                    cmbStatus.SelectedItem = row.Cells[4].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading record: " + ex.Message);
            }
        }
    }
}

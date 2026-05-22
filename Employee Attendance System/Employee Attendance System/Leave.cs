using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Employee_Attendance_System
{
    public partial class Leave : Form
    {
        OleDbConnection conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=EmpAttend.accdb");

        public Leave()
        {
            InitializeComponent();
        }

        private void Leave_Load(object sender, EventArgs e)
        {
            // Load employee details into ComboBox
            LoadEmployeeDetails();
            // Load leave records into DataGridView
            LoadLeaveRecords();
        }

        private void LoadEmployeeDetails()
        {
            try
            {
                conn.Open();
                OleDbDataAdapter da = new OleDbDataAdapter("SELECT EmployeeID, EmployeeName FROM Employees", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                cmbEmployeeID.DisplayMember = "EmployeeName";
                cmbEmployeeID.ValueMember = "EmployeeID";
                cmbEmployeeID.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading employee details: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void LoadLeaveRecords()
        {
            try
            {
                conn.Open();
                OleDbDataAdapter da = new OleDbDataAdapter("SELECT * FROM LeaveRequests", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvLeave.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading leave records: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void cmbEmployeeID_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Load employee's name and position when employee ID is selected
            if (cmbEmployeeID.SelectedValue != null)
            {
                string selectedEmployeeID = cmbEmployeeID.SelectedValue.ToString();
                LoadEmployeeInfo(selectedEmployeeID);
            }
        }

        private void LoadEmployeeInfo(string employeeID)
        {
            try
            {
                conn.Open();
                OleDbCommand cmd = new OleDbCommand("SELECT EmployeeName, Position FROM Employees WHERE EmployeeID = @EmployeeID", conn);
                cmd.Parameters.AddWithValue("@EmployeeID", employeeID);
                OleDbDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    lblEmployeeName.Text = reader["EmployeeName"].ToString();
                    lblPosition.Text = reader["Position"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading employee info: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                OleDbCommand cmd = new OleDbCommand("INSERT INTO LeaveRequests (EmployeeID, EmployeeName, Position, StartDate, EndDate, Status) VALUES (@EmployeeID, @EmployeeName, @Position, @StartDate, @EndDate, @Status)", conn);
                cmd.Parameters.AddWithValue("@EmployeeID", cmbEmployeeID.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@EmployeeName", lblEmployeeName.Text);
                cmd.Parameters.AddWithValue("@Position", lblPosition.Text);
                cmd.Parameters.AddWithValue("@StartDate", dtpStartDate.Value);
                cmd.Parameters.AddWithValue("@EndDate", dtpEndDate.Value);
                cmd.Parameters.AddWithValue("@Status", cmbStatus.SelectedItem.ToString());

                cmd.ExecuteNonQuery();
                MessageBox.Show("Leave request saved successfully.");
                LoadLeaveRecords(); // Reload leave records
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving leave request: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvLeave.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please select a leave record to update.");
                    return;
                }

                DataGridViewRow selectedRow = dgvLeave.SelectedRows[0];
                int leaveID = Convert.ToInt32(selectedRow.Cells["LeaveID"].Value);

                conn.Open();
                OleDbCommand cmd = new OleDbCommand("UPDATE LeaveRequests SET EmployeeID = @EmployeeID, EmployeeName = @EmployeeName, Position = @Position, StartDate = @StartDate, EndDate = @EndDate, Status = @Status WHERE LeaveID = @LeaveID", conn);
                cmd.Parameters.AddWithValue("@LeaveID", leaveID);
                cmd.Parameters.AddWithValue("@EmployeeID", cmbEmployeeID.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@EmployeeName", lblEmployeeName.Text);
                cmd.Parameters.AddWithValue("@Position", lblPosition.Text);
                cmd.Parameters.AddWithValue("@StartDate", dtpStartDate.Value);
                cmd.Parameters.AddWithValue("@EndDate", dtpEndDate.Value);
                cmd.Parameters.AddWithValue("@Status", cmbStatus.SelectedItem.ToString());

                cmd.ExecuteNonQuery();
                MessageBox.Show("Leave request updated successfully.");
                LoadLeaveRecords(); // Reload leave records
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating leave request: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvLeave.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please select a leave record to delete.");
                    return;
                }

                DataGridViewRow selectedRow = dgvLeave.SelectedRows[0];
                int leaveID = Convert.ToInt32(selectedRow.Cells["LeaveID"].Value);

                conn.Open();
                OleDbCommand cmd = new OleDbCommand("DELETE FROM LeaveRequests WHERE LeaveID = @LeaveID", conn);
                cmd.Parameters.AddWithValue("@LeaveID", leaveID);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Leave request deleted successfully.");
                LoadLeaveRecords(); // Reload leave records
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting leave request: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void dgvLeave_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // This will allow you to edit the selected record when clicking on a row
            if (e.RowIndex >= 0)
            {
                DataGridViewRow selectedRow = dgvLeave.Rows[e.RowIndex];
                cmbEmployeeID.SelectedValue = selectedRow.Cells["EmployeeID"].Value.ToString();
                lblEmployeeName.Text = selectedRow.Cells["EmployeeName"].Value.ToString();
                lblPosition.Text = selectedRow.Cells["Position"].Value.ToString();
                dtpStartDate.Value = Convert.ToDateTime(selectedRow.Cells["StartDate"].Value);
                dtpEndDate.Value = Convert.ToDateTime(selectedRow.Cells["EndDate"].Value);
                cmbStatus.SelectedItem = selectedRow.Cells["Status"].Value.ToString();
            }
        }
    }
}

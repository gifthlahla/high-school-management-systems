using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Payroll_Management_System
{
    public partial class Attendance : Form
    {
        private OleDbConnection conn = new OleDbConnection(
            @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Payroll.accdb;");
        private OleDbCommand cmd;
        private OleDbDataAdapter da;
        private DataTable dt;
        private int selectedID = -1;

        public Attendance()
        {
            InitializeComponent();
            cmbEmployeeID.SelectedIndexChanged += cmbEmployeeID_SelectedIndexChanged;
        }

        private void Attendance_Load(object sender, EventArgs e)
        {
            LoadEmployees();
            SetupPeriodPicker();
            LoadAttendances();
        }

        private void LoadEmployees()
        {
            dt = new DataTable();
            da = new OleDbDataAdapter(
                "SELECT EmployeeID FROM Employees ORDER BY EmployeeID;",
                conn);
            da.Fill(dt);

            cmbEmployeeID.DisplayMember = "EmployeeID";
            cmbEmployeeID.ValueMember = "EmployeeID";
            cmbEmployeeID.DataSource = dt;
            cmbEmployeeID.SelectedIndex = -1;
        }

        private void SetupPeriodPicker()
        {
            dtpPeriod.Format = DateTimePickerFormat.Custom;
            dtpPeriod.CustomFormat = "MMMM yyyy";
            dtpPeriod.ShowUpDown = true;
        }

        private void cmbEmployeeID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbEmployeeID.SelectedIndex < 0) return;

            int empId = (int)cmbEmployeeID.SelectedValue;
            cmd = new OleDbCommand(
                "SELECT EmployeeName, Department FROM Employees WHERE EmployeeID = ?;",
                conn);
            cmd.Parameters.AddWithValue("?", empId);

            try
            {
                conn.Open();
                using (var rdr = cmd.ExecuteReader())
                {
                    if (rdr.Read())
                    {
                        lblEmployeeName.Text = rdr["EmployeeName"].ToString();
                        lblDepartment.Text = rdr["Department"].ToString();
                    }
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

        private void LoadAttendances()
        {
            dt = new DataTable();
            string sql =
              "SELECT A.ID, A.EmployeeID, E.EmployeeName, E.Department, " +
              "A.Present, A.Absent, A.Excused, A.PeriodDate " +
              "FROM Attendance AS A " +
              "LEFT JOIN Employees AS E ON A.EmployeeID = E.EmployeeID " +
              "ORDER BY A.PeriodDate DESC, A.EmployeeID;";

            da = new OleDbDataAdapter(sql, conn);
            da.Fill(dt);
            dgvAttendances.DataSource = dt;
        }

        private void ClearFields()
        {
            cmbEmployeeID.SelectedIndex = -1;
            lblEmployeeName.Clear();
            lblDepartment.Clear();
            txtPresent.Clear();
            txtAbsent.Clear();
            txtExcused.Clear();
            dtpPeriod.Value = DateTime.Now;
            selectedID = -1;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (cmbEmployeeID.SelectedIndex < 0 ||
                string.IsNullOrWhiteSpace(txtPresent.Text) ||
                string.IsNullOrWhiteSpace(txtAbsent.Text) ||
                string.IsNullOrWhiteSpace(txtExcused.Text))
            {
                MessageBox.Show("Please fill all fields.");
                return;
            }

            try
            {
                DateTime period = new DateTime(dtpPeriod.Value.Year, dtpPeriod.Value.Month, 1);

                conn.Open();
                cmd = new OleDbCommand(
                  "INSERT INTO Attendance " +
                  "(EmployeeID, Present, Absent, Excused, PeriodDate) " +
                  "VALUES (?,?,?,?,?);",
                  conn);

                cmd.Parameters.AddWithValue("?", cmbEmployeeID.SelectedValue);
                cmd.Parameters.AddWithValue("?", int.Parse(txtPresent.Text));
                cmd.Parameters.AddWithValue("?", int.Parse(txtAbsent.Text));
                cmd.Parameters.AddWithValue("?", int.Parse(txtExcused.Text));
                cmd.Parameters.AddWithValue("?", period);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Attendance record saved.");
                ClearFields();
                LoadAttendances();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving attendance: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (selectedID < 0)
            {
                MessageBox.Show("Select a record to edit.");
                return;
            }

            try
            {
                DateTime period = new DateTime(dtpPeriod.Value.Year, dtpPeriod.Value.Month, 1);

                conn.Open();
                cmd = new OleDbCommand(
                  "UPDATE Attendance SET " +
                  "EmployeeID=?, Present=?, Absent=?, Excused=?, PeriodDate=? " +
                  "WHERE ID=?;",
                  conn);

                cmd.Parameters.AddWithValue("?", cmbEmployeeID.SelectedValue);
                cmd.Parameters.AddWithValue("?", int.Parse(txtPresent.Text));
                cmd.Parameters.AddWithValue("?", int.Parse(txtAbsent.Text));
                cmd.Parameters.AddWithValue("?", int.Parse(txtExcused.Text));
                cmd.Parameters.AddWithValue("?", period);
                cmd.Parameters.AddWithValue("?", selectedID);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Attendance updated.");
                ClearFields();
                LoadAttendances();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating attendance: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedID < 0)
            {
                MessageBox.Show("Select a record to delete.");
                return;
            }

            if (MessageBox.Show("Really delete?", "Confirm",
                MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            try
            {
                conn.Open();
                cmd = new OleDbCommand("DELETE FROM Attendance WHERE ID = ?;", conn);
                cmd.Parameters.AddWithValue("?", selectedID);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Record deleted.");
                ClearFields();
                LoadAttendances();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting attendance: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void dgvAttendances_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = dgvAttendances.Rows[e.RowIndex];
            selectedID = Convert.ToInt32(row.Cells["ID"].Value);
            cmbEmployeeID.Text = row.Cells["EmployeeID"].Value.ToString();
            txtPresent.Text = row.Cells["Present"].Value.ToString();
            txtAbsent.Text = row.Cells["Absent"].Value.ToString();
            txtExcused.Text = row.Cells["Excused"].Value.ToString();

            if (row.Cells["PeriodDate"].Value != DBNull.Value)
                dtpPeriod.Value = Convert.ToDateTime(row.Cells["PeriodDate"].Value);
        }
    }
}

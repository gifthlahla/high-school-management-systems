
using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Payroll_Management_System
{
    public partial class Issue_Salary : Form
    {
        private OleDbConnection conn = new OleDbConnection(
            @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Payroll.accdb;");
        private OleDbCommand cmd;
        private OleDbDataAdapter da;
        private DataTable dtEmployees, dtBonuses;
        private int selectedID = -1;

        public Issue_Salary()
        {
            InitializeComponent();

            // Configure period picker
            dtpPeriod.Format = DateTimePickerFormat.Custom;
            dtpPeriod.CustomFormat = "MMMM yyyy";
            dtpPeriod.ShowUpDown = true;
        }

        private void Issue_Salary_Load(object sender, EventArgs e)
        {
            LoadEmployees();
            LoadBonuses();
            LoadIssueSalary();
        }

        private void LoadEmployees()
        {
            dtEmployees = new DataTable();
            da = new OleDbDataAdapter(
              "SELECT EmployeeID, EmployeeName, [Position], BaseSalary FROM Employees ORDER BY EmployeeID;",
              conn);
            da.Fill(dtEmployees);

            cmbEmployeeID.DisplayMember = "EmployeeID";
            cmbEmployeeID.ValueMember = "EmployeeID";
            cmbEmployeeID.DataSource = dtEmployees;
        }

        private void LoadBonuses()
        {
            dtBonuses = new DataTable();
            da = new OleDbDataAdapter(
              "SELECT ID, [Amount] FROM [Bonus] UNION SELECT 0 AS ID, 0 AS Amount FROM [Bonus] ORDER BY ID;",
              conn);
            da.Fill(dtBonuses);

            cmbBonus.DisplayMember = "ID";
            cmbBonus.ValueMember = "Amount";
            cmbBonus.DataSource = dtBonuses;
            cmbBonus.SelectedIndex = 0;   // "0" corresponds to None
        }

        private void cmbEmployeeID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbEmployeeID.SelectedIndex < 0) return;
            var row = ((DataRowView)cmbEmployeeID.SelectedItem).Row;

            lblEmployeeName.Text = row["EmployeeName"].ToString();
            lblPosition.Text = row["Position"].ToString();
            lblSalary.Text = Convert.ToDecimal(row["BaseSalary"]).ToString("F2");
        }

        private void cmbBonus_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblBonusAmount.Text = ((decimal)cmbBonus.SelectedValue).ToString("F2");
            FetchAttendanceAndAdvance();
            ComputeNetPay();
        }

        private void FetchAttendanceAndAdvance()
        {
            if (cmbEmployeeID.SelectedIndex < 0) return;
            int empId = (int)cmbEmployeeID.SelectedValue;
            DateTime period = new DateTime(dtpPeriod.Value.Year, dtpPeriod.Value.Month, 1);

            // 1) Attendance:
            cmd = new OleDbCommand(
              "SELECT Present, Absent, Excused FROM Attendance " +
              "WHERE EmployeeID=? AND PeriodDate=?;",
              conn);
            cmd.Parameters.AddWithValue("?", empId);
            cmd.Parameters.AddWithValue("?", period);

            try
            {
                conn.Open();
                var rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    lblPresent.Text = rdr["Present"].ToString();
                    lblAbsent.Text = rdr["Absent"].ToString();
                    lblExcused.Text = rdr["Excused"].ToString();
                }
                else
                {
                    // No attendance record
                    lblPresent.Text = lblAbsent.Text = lblExcused.Text = "0";
                    MessageBox.Show("No attendance found for this period. Please add it in the Attendance form.");
                }
                rdr.Close();
            }
            finally { conn.Close(); }

            try
            {
                using (OleDbConnection con = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Payroll.accdb"))
                {
                    conn.Open();
                    using (OleDbCommand cmnd = new OleDbCommand(
                        " SELECT IIf(SUM([Amount]) Is Null, 0,SUM([Amount])) FROM [Advance] WHERE EmployeeID = ? AND PeriodDate = ?",
                        conn))
                    {
                        cmnd.Parameters.AddWithValue("?", empId);
                        cmnd.Parameters.AddWithValue("?", period);

                        var result = cmnd.ExecuteScalar();
                        decimal adv = Convert.ToDecimal(result);
                        lblAdvanceAmount.Text = adv.ToString("F2");
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                MessageBox.Show("An unexpected error occurred: " + ex.Message);
            }
            finally { conn.Close(); }
        }

        private void ComputeNetPay()
        {
            if (string.IsNullOrWhiteSpace(lblSalary.Text)) return;

            decimal basePerHour = decimal.Parse(lblSalary.Text);
            const int hoursPerDay = 8;

            int present = int.Parse(lblPresent.Text);
            decimal dailyPay = basePerHour * hoursPerDay;

            decimal bonus = decimal.Parse(lblBonusAmount.Text);
            decimal advance = decimal.Parse(lblAdvanceAmount.Text);

            decimal gross = present * dailyPay + bonus;
            decimal net = gross - advance;

            lblNetTotal.Text = net.ToString("F2");
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (cmbEmployeeID.SelectedIndex < 0) return;

            DateTime period = new DateTime(dtpPeriod.Value.Year, dtpPeriod.Value.Month, 1);

            conn.Open();
            cmd = new OleDbCommand(
              "INSERT INTO IssueSalary " +
              "(EmployeeID, PeriodDate, BaseSalary, PresentDays, AbsentDays, ExcusedDays, BonusAmount, AdvanceAmount, NetPay) " +
              "VALUES (?,?,?,?,?,?,?,?,?);",
              conn);

            cmd.Parameters.AddWithValue("?", cmbEmployeeID.SelectedValue);
            cmd.Parameters.AddWithValue("?", period);
            cmd.Parameters.AddWithValue("?", decimal.Parse(lblSalary.Text));
            cmd.Parameters.AddWithValue("?", int.Parse(lblPresent.Text));
            cmd.Parameters.AddWithValue("?", int.Parse(lblAbsent.Text));
            cmd.Parameters.AddWithValue("?", int.Parse(lblExcused.Text));
            cmd.Parameters.AddWithValue("?", decimal.Parse(lblBonusAmount.Text));
            cmd.Parameters.AddWithValue("?", decimal.Parse(lblAdvanceAmount.Text));
            cmd.Parameters.AddWithValue("?", decimal.Parse(lblNetTotal.Text));

            cmd.ExecuteNonQuery();
            conn.Close();

            LoadIssueSalary();
            MessageBox.Show("Salary issued.");
        }

        private void LoadIssueSalary()
        {
            var dt = new DataTable();
            da = new OleDbDataAdapter(
              "SELECT * FROM IssueSalary ORDER BY PeriodDate DESC, EmployeeID;",
              conn);
            da.Fill(dt);
            dgvIssueSalary.DataSource = dt;
        }

        // Fired when the user clicks on a row in the DataGridView.
        // Loads that record into all the inputs so you can edit and recompute.
        private void dgvIssueSalary_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = dgvIssueSalary.Rows[e.RowIndex];
            // Grab the primary key
            selectedID = Convert.ToInt32(row.Cells["ID"].Value);

            // Employee
            int empId = Convert.ToInt32(row.Cells["EmployeeID"].Value);
            cmbEmployeeID.SelectedValue = empId;    // Triggers lblEmployeeName, lblPosition, lblSalary to fill

            // Period
            dtpPeriod.Value = Convert.ToDateTime(row.Cells["PeriodDate"].Value);

            // Attendance days
            lblPresent.Text = row.Cells["PresentDays"].Value.ToString();
            lblAbsent.Text = row.Cells["AbsentDays"].Value.ToString();
            lblExcused.Text = row.Cells["ExcusedDays"].Value.ToString();

            // Bonus
            decimal bonusAmt = Convert.ToDecimal(row.Cells["BonusAmount"].Value);
            // Find the matching item in cmbBonus by Amount
            for (int i = 0; i < cmbBonus.Items.Count; i++)
            {
                if ((decimal)((DataRowView)cmbBonus.Items[i]).Row["Amount"] == bonusAmt)
                {
                    cmbBonus.SelectedIndex = i;
                    break;
                }
            }

            // Advance & NetPay
            lblAdvanceAmount.Text = row.Cells["AdvanceAmount"].Value.ToString();
            lblNetTotal.Text = row.Cells["NetPay"].Value.ToString();
        }

        // Fired when the user clicks "Edit" to save changes to an existing row.
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (selectedID < 0)
            {
                MessageBox.Show("Please select a record first (click in the grid).");
                return;
            }

            // Re-compute using whatever is currently in the fields
            ComputeNetPay();

            DateTime period = new DateTime(dtpPeriod.Value.Year, dtpPeriod.Value.Month, 1);

            try
            {
                conn.Open();
                cmd = new OleDbCommand(
                  "UPDATE IssueSalary SET " +
                  "EmployeeID=?, PeriodDate=?, BaseSalary=?, PresentDays=?, AbsentDays=?, ExcusedDays=?, " +
                  "BonusAmount=?, AdvanceAmount=?, NetPay=? " +
                  "WHERE ID=?;",
                  conn);

                cmd.Parameters.AddWithValue("?", cmbEmployeeID.SelectedValue);
                cmd.Parameters.AddWithValue("?", period);
                cmd.Parameters.AddWithValue("?", decimal.Parse(lblSalary.Text));
                cmd.Parameters.AddWithValue("?", int.Parse(lblPresent.Text));
                cmd.Parameters.AddWithValue("?", int.Parse(lblAbsent.Text));
                cmd.Parameters.AddWithValue("?", int.Parse(lblExcused.Text));
                cmd.Parameters.AddWithValue("?", decimal.Parse(lblBonusAmount.Text));
                cmd.Parameters.AddWithValue("?", decimal.Parse(lblAdvanceAmount.Text));
                cmd.Parameters.AddWithValue("?", decimal.Parse(lblNetTotal.Text));
                cmd.Parameters.AddWithValue("?", selectedID);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Record updated successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating record: " + ex.Message);
            }
            finally
            {
                conn.Close();
                LoadIssueSalary();
            }
        }

        private void ClearFields()
        {
            cmbEmployeeID.SelectedIndex = -1;
            lblEmployeeName.Clear();
            lblPosition.Clear();
            lblSalary.Clear();
            dtpPeriod.Value = DateTime.Now;
            lblPresent.Clear();
            lblAbsent.Clear();
            lblExcused.Clear();
            cmbBonus.SelectedIndex = 0;
            lblBonusAmount.Clear();
            lblAdvanceAmount.Clear();
            lblNetTotal.Clear();
            selectedID = -1;
            dgvIssueSalary.ClearSelection();
        }
    }
}

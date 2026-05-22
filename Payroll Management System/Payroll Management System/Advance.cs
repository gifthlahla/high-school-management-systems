using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Payroll_Management_System
{
    public partial class Advance : Form
    {
        private OleDbConnection conn = new OleDbConnection(
            @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Payroll.accdb;");
        private OleDbCommand cmd;
        private OleDbDataAdapter da;
        private DataTable dt;
        private int selectedID = -1;

        public Advance()
        {
            InitializeComponent();
            cmbEmployee.SelectedIndexChanged += cmbEmployee_SelectedIndexChanged;
        }

        private void Advance_Load(object sender, EventArgs e)
        {
            LoadEmployees();
            InitPurposeCombo();
            LoadAdvances();

            dtpPeriod.Format = DateTimePickerFormat.Custom;
            dtpPeriod.CustomFormat = "MMMM/yyyy";
            dtpPeriod.ShowUpDown = true;
        }

        private void LoadEmployees()
        {
            dt = new DataTable();
            da = new OleDbDataAdapter(
                "SELECT EmployeeID, EmployeeName FROM Employees ORDER BY EmployeeName;",
                conn);
            da.Fill(dt);
            cmbEmployee.DisplayMember = "EmployeeName";
            cmbEmployee.ValueMember = "EmployeeID";
            cmbEmployee.DataSource = dt;
            cmbEmployee.SelectedIndex = -1;
        }

        private void InitPurposeCombo()
        {
            cmbPurposeOFAdvance.Items.Clear();
            cmbPurposeOFAdvance.Items.AddRange(new object[] {
                "Medical", "Travel", "Salary Advance", "Emergency", "Other"
            });
            cmbPurposeOFAdvance.SelectedIndex = -1;
        }

        private void cmbEmployee_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbEmployee.SelectedIndex < 0) return;

            int empId = (int)cmbEmployee.SelectedValue;
            cmd = new OleDbCommand(
                "SELECT [Department], [Position] FROM Employees WHERE EmployeeID = ?",
                conn);
            cmd.Parameters.AddWithValue("?", empId);

            try
            {
                conn.Open();
                var rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    lblDepartment.Text = rdr["Department"].ToString();
                    lblPosition.Text = rdr["Position"].ToString();
                }
                rdr.Close();
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

        private void LoadAdvances()
        {
            dt = new DataTable();
            string sql =
              "SELECT A.ID, E.EmployeeName, A.[Department], A.[Position], A.PurposeOfAdvance, A.Amount, A.PeriodDate " +
              "FROM Advance AS A " +
              "LEFT JOIN Employees AS E ON A.EmployeeID = E.EmployeeID " +
              "ORDER BY A.ID;";
            da = new OleDbDataAdapter(sql, conn);
            da.Fill(dt);
            dgvAdvances.DataSource = dt;
        }

        private void ClearFields()
        {
            cmbEmployee.SelectedIndex = -1;
            lblDepartment.Clear();
            lblPosition.Clear();
            cmbPurposeOFAdvance.SelectedIndex = -1;
            txtAmount.Clear();
            dtpPeriod.Value = DateTime.Now;
            selectedID = -1;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (cmbEmployee.SelectedIndex < 0 ||
                cmbPurposeOFAdvance.SelectedIndex < 0 ||
                string.IsNullOrWhiteSpace(txtAmount.Text))
            {
                MessageBox.Show("Please select an employee, purpose, and enter amount.");
                return;
            }

            try
            {
                conn.Open();
                cmd = new OleDbCommand(
                  "INSERT INTO Advance " +
                  "(EmployeeID, [Department], [Position], [PurposeOfAdvance], [Amount], [AdvanceDate], [PeriodDate]) " +
                  "VALUES (?,?,?,?,?,?,?);",
                  conn);

                DateTime selectedPeriod = new DateTime(dtpPeriod.Value.Year, dtpPeriod.Value.Month, 1);

                cmd.Parameters.AddWithValue("?", cmbEmployee.SelectedValue);
                cmd.Parameters.AddWithValue("?", lblDepartment.Text);
                cmd.Parameters.AddWithValue("?", lblPosition.Text);
                cmd.Parameters.AddWithValue("?", cmbPurposeOFAdvance.Text);
                cmd.Parameters.AddWithValue("?", decimal.Parse(txtAmount.Text));
                cmd.Parameters.AddWithValue("?", DateTime.Now.ToString("dd/MM/yyyy")); // AdvanceDate
                cmd.Parameters.AddWithValue("?", selectedPeriod); // PeriodDate

                cmd.ExecuteNonQuery();
                MessageBox.Show("Advance saved.");
                ClearFields();
                LoadAdvances();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving advance: " + ex.Message);
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
                MessageBox.Show("Select an advance to edit.");
                return;
            }

            try
            {
                conn.Open();
                cmd = new OleDbCommand(
                  "UPDATE [Advance] SET " +
                  "EmployeeID=?, [Department]=?, [Position]=?, " +
                  "[PurposeOfAdvance]=?, [Amount]=?, [PeriodDate]=? " +
                  "WHERE ID=?;",
                  conn);

                DateTime selectedPeriod = new DateTime(dtpPeriod.Value.Year, dtpPeriod.Value.Month, 1);

                cmd.Parameters.AddWithValue("?", cmbEmployee.SelectedValue);
                cmd.Parameters.AddWithValue("?", lblDepartment.Text);
                cmd.Parameters.AddWithValue("?", lblPosition.Text);
                cmd.Parameters.AddWithValue("?", cmbPurposeOFAdvance.Text);
                cmd.Parameters.AddWithValue("?", decimal.Parse(txtAmount.Text));
                cmd.Parameters.AddWithValue("?", selectedPeriod);
                cmd.Parameters.AddWithValue("?", selectedID);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Advance updated.");
                ClearFields();
                LoadAdvances();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating advance: " + ex.Message);
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
                MessageBox.Show("Select an advance to delete.");
                return;
            }

            if (MessageBox.Show("Really delete this advance?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            try
            {
                conn.Open();
                cmd = new OleDbCommand("DELETE FROM [Advance] WHERE ID = ?;", conn);
                cmd.Parameters.AddWithValue("?", selectedID);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Advance deleted.");
                ClearFields();
                LoadAdvances();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting advance: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void dgvAdvances_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = dgvAdvances.Rows[e.RowIndex];
            selectedID = Convert.ToInt32(row.Cells["ID"].Value);
            cmbEmployee.Text = row.Cells["EmployeeName"].Value.ToString();
            lblDepartment.Text = row.Cells["Department"].Value.ToString();
            lblPosition.Text = row.Cells["Position"].Value.ToString();
            cmbPurposeOFAdvance.Text = row.Cells["PurposeOfAdvance"].Value.ToString();
            txtAmount.Text = row.Cells["Amount"].Value.ToString();

            if (row.Cells["PeriodDate"].Value != DBNull.Value)
            {
                dtpPeriod.Value = Convert.ToDateTime(row.Cells["PeriodDate"].Value);
            }
        }

        private void cmbEmployee_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (cmbEmployee.SelectedIndex != -1)
            {
                try
                {
                    conn.Open();
                    OleDbCommand cmd = new OleDbCommand("SELECT [Department], [Position] FROM Employees WHERE EmployeeName = ?", conn);
                    cmd.Parameters.AddWithValue("?", cmbEmployee.Text);
                    OleDbDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        lblDepartment.Text = reader["Department"].ToString();
                        lblPosition.Text = reader["Position"].ToString ();
                    }

                    conn.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error : " + ex.Message);
                    conn.Close();
                }
            }
        }
    }
}

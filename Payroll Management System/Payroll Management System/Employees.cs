using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Payroll_Management_System
{
    public partial class Employees : Form
    {
        private OleDbConnection conn;
        private string connString = 
            @"Provider=Microsoft.ACE.OLEDB.12.0;" +
            @"Data Source=" + Application.StartupPath + @"\Payroll.accdb;";

        public Employees()
        {
            InitializeComponent();
            conn = new OleDbConnection(connString);
        }

        private void Employees_Load(object sender, EventArgs e)
        {
            // Static initialization of combo‐boxes:
            LoadData();
            // Now just load the grid:
            RefreshGrid();
        }

        private void LoadData()
        {
            cmbGender.Items.Clear();
            cmbGender.Items.AddRange(new object[] {
                "Male", "Female", "Other"
            });

            cmbPosition.Items.Clear();
            cmbPosition.Items.AddRange(new object[] {
                "Manager","Clerk","Accountant","HR","Receptionist"
            });

            cmbQualifications.Items.Clear();
            cmbQualifications.Items.AddRange(new object[] {
                "High School","Diploma", "Bachelor's","Master's", "PhD"});

            cmbDepartment.Items.Clear();
            cmbDepartment.Items.AddRange(new object[] {
                "Finance","Human Resources","Operations","IT","Sales"
            });
        }
        
        private void RefreshGrid()
        {
            var dt = new DataTable();
            using (var da = new OleDbDataAdapter(
                "SELECT * FROM Employees ORDER BY EmployeeID;",
                conn))
            {
                da.Fill(dt);
            }
            dgvEmployees.DataSource = dt;
        }

        private void ClearInputs()
        {
            txtEmployee.Clear();
            txtPhone.Clear();
            cmbGender.SelectedIndex = -1;
            dtpDOB.Value = DateTime.Today;
            txtSalaryperHour.Clear();
            cmbPosition.SelectedIndex = -1;
            cmbQualifications.SelectedIndex = -1;
            txtAddress.Clear();
            dtpDateJoined.Value = DateTime.Today;
            cmbDepartment.SelectedIndex = -1;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string sql ="INSERT INTO Employees ([EmployeeName], [Phone], [Gender], [DateOfBirth], [BaseSalary], [Position], [Qualifications], [Address], DateJoined, [Department]) VALUES (?,?,?,?,?,?,?,?,?,?)";
            using (var cmd = new OleDbCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("?", txtEmployee.Text);
                cmd.Parameters.AddWithValue("?", txtPhone.Text);
                cmd.Parameters.AddWithValue("?", cmbGender.Text);
                cmd.Parameters.AddWithValue("?", dtpDOB.Value.Date);
                cmd.Parameters.AddWithValue("?", double.Parse(txtSalaryperHour.Text));
                cmd.Parameters.AddWithValue("?", cmbPosition.Text);
                cmd.Parameters.AddWithValue("?", cmbQualifications.Text);
                cmd.Parameters.AddWithValue("?", txtAddress.Text);
                cmd.Parameters.AddWithValue("?", dtpDateJoined.Value.Date);
                cmd.Parameters.AddWithValue("?", cmbDepartment.Text);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }

            RefreshGrid();
            ClearInputs();
        }

        private void dgvEmployees_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = dgvEmployees.Rows[e.RowIndex];
            // Load into inputs
            txtEmployee.Text           = row.Cells["EmployeeName"].Value.ToString();
            txtPhone.Text              = row.Cells["Phone"].Value.ToString();
            cmbGender.Text             = row.Cells["Gender"].Value.ToString();
            dtpDOB.Value               = Convert.ToDateTime(row.Cells["DateOfBirth"].Value);
            txtSalaryperHour.Text      = row.Cells["BaseSalary"].Value.ToString();
            cmbPosition.Text           = row.Cells["Position"].Value.ToString();
            cmbQualifications.Text     = row.Cells["Qualifications"].Value.ToString();
            txtAddress.Text            = row.Cells["Address"].Value.ToString();
            dtpDateJoined.Value        = Convert.ToDateTime(row.Cells["DateJoined"].Value);
            cmbDepartment.Text         = row.Cells["Department"].Value.ToString();
            // Keep track of selected ID
            dgvEmployees.Tag           = row.Cells["EmployeeID"].Value;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvEmployees.Tag == null) return;
            const string sql = @"
                UPDATE Employees SET
                  [EmployeeName] = ?, [Phone] = ?, [Gender] = ?, [DateOfBirth] = ?, [BaseSalary] = ?,
                  [Position] = ?, [Qualifications] = ?, [Address] = ?, [DateJoined] = ?, [Department] = ?
                WHERE [EmployeeID] = ?;";

            using (var cmd = new OleDbCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("?", txtEmployee.Text);
                cmd.Parameters.AddWithValue("?", txtPhone.Text);
                cmd.Parameters.AddWithValue("?", cmbGender.Text);
                cmd.Parameters.AddWithValue("?", dtpDOB.Value.Date);
                cmd.Parameters.AddWithValue("?", double.Parse(txtSalaryperHour.Text));
                cmd.Parameters.AddWithValue("?", cmbPosition.Text);
                cmd.Parameters.AddWithValue("?", cmbQualifications.Text);
                cmd.Parameters.AddWithValue("?", txtAddress.Text);
                cmd.Parameters.AddWithValue("?", dtpDateJoined.Value.Date);
                cmd.Parameters.AddWithValue("?", cmbDepartment.Text);
                cmd.Parameters.AddWithValue("?", dgvEmployees.Tag);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }

            RefreshGrid();
            ClearInputs();
            dgvEmployees.Tag = null;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvEmployees.Tag == null) return;

            var id = dgvEmployees.Tag;
            if (MessageBox.Show("Really delete selected employee?", 
                "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
                != DialogResult.Yes) return;

            using (var cmd = new OleDbCommand(
                "DELETE FROM Employees WHERE EmployeeID = ?;", conn))
            {
                cmd.Parameters.AddWithValue("?", id);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }

            RefreshGrid();
            ClearInputs();
            dgvEmployees.Tag = null;
        }
    }
}

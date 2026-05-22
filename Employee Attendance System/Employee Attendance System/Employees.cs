using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Employee_Attendance_System
{
    public partial class Employees : Form
    {
        OleDbConnection connection;
        OleDbDataAdapter dataAdapter;
        DataTable dataTable;

        public Employees()
        {
            InitializeComponent();
            connection = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=EmpAttend.accdb;");
            LoadEmployees();
        }

        private void LoadEmployees()
        {
            try
            {
                connection.Open();
                string query = "SELECT * FROM Employees";
                dataAdapter = new OleDbDataAdapter(query, connection);
                dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                dgvStaff.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                connection.Open();
                string query = "INSERT INTO Employees (EmployeeName, Gender, Phone, DateOfBirth, Address, Position, DateJoined) " +
                               "VALUES (?, ?, ?, ?, ?, ?, ?)";
                OleDbCommand command = new OleDbCommand(query, connection);
                command.Parameters.AddWithValue("?", txtName.Text);
                command.Parameters.AddWithValue("?", cmbGender.SelectedItem.ToString());
                command.Parameters.AddWithValue("?", txtPhone.Text);
                command.Parameters.AddWithValue("?", dtpDOB.Value);
                command.Parameters.AddWithValue("?", txtAddress.Text);
                command.Parameters.AddWithValue("?", cmbPosition.SelectedItem.ToString());
                command.Parameters.AddWithValue("?", dtpDateJoined.Value);

                command.ExecuteNonQuery();
                MessageBox.Show("Employee added successfully!");
                LoadEmployees();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvStaff.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please select an employee to edit.");
                    return;
                }

                DataGridViewRow row = dgvStaff.SelectedRows[0];
                int employeeID = Convert.ToInt32(row.Cells["EmployeeID"].Value);

                connection.Open();
                string query = "UPDATE Employees SET EmployeeName = ?, Gender = ?, Phone = ?, DateOfBirth = ?, Address = ?, Position = ?, DateJoined = ? WHERE EmployeeID = ?";
                OleDbCommand command = new OleDbCommand(query, connection);
                command.Parameters.AddWithValue("?", txtName.Text);
                command.Parameters.AddWithValue("?", cmbGender.SelectedItem.ToString());
                command.Parameters.AddWithValue("?", txtPhone.Text);
                command.Parameters.AddWithValue("?", dtpDOB.Value);
                command.Parameters.AddWithValue("?", txtAddress.Text);
                command.Parameters.AddWithValue("?", cmbPosition.SelectedItem.ToString());
                command.Parameters.AddWithValue("?", dtpDateJoined.Value);
                command.Parameters.AddWithValue("?", employeeID);

                command.ExecuteNonQuery();
                MessageBox.Show("Employee updated successfully!");
                LoadEmployees();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvStaff.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please select an employee to delete.");
                    return;
                }

                DataGridViewRow row = dgvStaff.SelectedRows[0];
                int employeeID = Convert.ToInt32(row.Cells["EmployeeID"].Value);

                connection.Open();
                string query = "DELETE FROM Employees WHERE EmployeeID = ?";
                OleDbCommand command = new OleDbCommand(query, connection);
                command.Parameters.AddWithValue("?", employeeID);

                command.ExecuteNonQuery();
                MessageBox.Show("Employee deleted successfully!");
                LoadEmployees();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void dgvStaff_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvStaff.Rows[e.RowIndex];
                txtName.Text = row.Cells["EmployeeName"].Value.ToString();
                cmbGender.SelectedItem = row.Cells["Gender"].Value.ToString();
                txtPhone.Text = row.Cells["Phone"].Value.ToString();
                dtpDOB.Value = Convert.ToDateTime(row.Cells["DateOfBirth"].Value);
                txtAddress.Text = row.Cells["Address"].Value.ToString();
                cmbPosition.SelectedItem = row.Cells["Position"].Value.ToString();
                dtpDateJoined.Value = Convert.ToDateTime(row.Cells["DateJoined"].Value);
            }
        }

        private void Employees_Load(object sender, EventArgs e)
        {
            // Initialize form (like combo boxes and data grids)
            LoadEmployees();
        }
    }
}

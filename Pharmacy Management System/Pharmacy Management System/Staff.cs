using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Pharmacy_Management_System
{
    public partial class Staff : Form
    {
        //–– Database connection & helpers
        private OleDbConnection con = new OleDbConnection(
            @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Pharmacy.accdb"
        );
        private OleDbCommand cmd;
        private OleDbDataAdapter da;
        private DataTable dt;

        //–– Holds the currently selected staff ID
        private int selectedStaffID = -1;

        public Staff()
        {
            InitializeComponent();
            PopulatePositions();
            LoadStaff();
        }

        private void Staff_Load(object sender, EventArgs e)
        {
            // If you wired Load in Designer, call these here instead:
            // PopulatePositions();
            // LoadStaff();
        }

        private void PopulatePositions()
        {
            cmbPosition.Items.Clear();
            cmbPosition.Items.Add("Pharmacist");
            cmbPosition.Items.Add("Technician");
            cmbPosition.Items.Add("Cashier");
            cmbPosition.Items.Add("Manager");
            cmbPosition.SelectedIndex = 0;
        }

        private void LoadStaff()
        {
            try
            {
                con.Open();
                da = new OleDbDataAdapter("SELECT * FROM Staffs", con);
                dt = new DataTable();
                da.Fill(dt);
                dgvStaff.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading staff: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void ClearFields()
        {
            txtName.Clear();
            txtRegNumber.Clear();
            txtAddress.Clear();
            txtPhone.Clear();
            dtpDOB.Value = DateTime.Today;
            cmbPosition.SelectedIndex = 0;
            selectedStaffID = -1;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Full Name is required.");
                return;
            }

            try
            {
                con.Open();
                cmd = new OleDbCommand(
                    "INSERT INTO Staffs (FullName, RegNumber, Address, Phone, DOB, Position) " +
                    "VALUES (?, ?, ?, ?, ?, ?)",
                    con
                );
                cmd.Parameters.AddWithValue("?", txtName.Text);
                cmd.Parameters.AddWithValue("?", txtRegNumber.Text);
                cmd.Parameters.AddWithValue("?", txtAddress.Text);
                cmd.Parameters.AddWithValue("?", txtPhone.Text);
                cmd.Parameters.AddWithValue("?", dtpDOB.Value.Date);
                cmd.Parameters.AddWithValue("?", cmbPosition.Text);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Staff member added!");
                LoadStaff();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving staff: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (selectedStaffID == -1)
            {
                MessageBox.Show("Select a staff member to edit.");
                return;
            }

            try
            {
                con.Open();
                cmd = new OleDbCommand(
                    "UPDATE Staffs " +
                    "SET FullName=?, RegNumber=?, Address=?, Phone=?, DOB=?, Position=? " +
                    "WHERE ID=?",
                    con
                );
                cmd.Parameters.AddWithValue("?", txtName.Text);
                cmd.Parameters.AddWithValue("?", txtRegNumber.Text);
                cmd.Parameters.AddWithValue("?", txtAddress.Text);
                cmd.Parameters.AddWithValue("?", txtPhone.Text);
                cmd.Parameters.AddWithValue("?", dtpDOB.Value.Date);
                cmd.Parameters.AddWithValue("?", cmbPosition.Text);
                cmd.Parameters.AddWithValue("?", selectedStaffID);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Staff member updated!");
                LoadStaff();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating staff: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedStaffID == -1)
            {
                MessageBox.Show("Select a staff member to delete.");
                return;
            }

            if (MessageBox.Show(
                    "Are you sure you want to delete this staff member?",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                ) != DialogResult.Yes) return;

            try
            {
                con.Open();
                cmd = new OleDbCommand("DELETE FROM Staffs WHERE ID=?", con);
                cmd.Parameters.AddWithValue("?", selectedStaffID);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Staff member deleted!");
                LoadStaff();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting staff: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void dgvStaff_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgvStaff.Rows[e.RowIndex];
            selectedStaffID = Convert.ToInt32(row.Cells["ID"].Value);
            txtName.Text = row.Cells["FullName"].Value.ToString();
            txtRegNumber.Text = row.Cells["RegNumber"].Value.ToString();
            txtAddress.Text = row.Cells["Address"].Value.ToString();
            txtPhone.Text = row.Cells["Phone"].Value.ToString();
            dtpDOB.Value = Convert.ToDateTime(row.Cells["DOB"].Value);
            cmbPosition.Text = row.Cells["Position"].Value.ToString();
        }
    }
}

using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Pharmacy_Management_System
{
    public partial class Customer : Form
    {
        //–– Database objects
        private OleDbConnection con = new OleDbConnection(
            @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Pharmacy.accdb"
        );
        private OleDbCommand cmd;
        private OleDbDataAdapter da;
        private DataTable dt;

        //–– Holds the currently selected record’s ID
        private int selectedCustomerID = -1;

        public Customer()
        {
            InitializeComponent();
            PopulateGenderCombo();
            LoadCustomers();
        }

        private void Customer_Load(object sender, EventArgs e)
        {
            // (If you wired up Load in Designer, you can call these here instead)
            // PopulateGenderCombo();
            // LoadCustomers();
        }

        private void PopulateGenderCombo()
        {
            cmbGender.Items.Clear();
            cmbGender.Items.Add("Male");
            cmbGender.Items.Add("Female");
            cmbGender.Items.Add("Other");
            cmbGender.SelectedIndex = 0;
        }

        private void LoadCustomers()
        {
            try
            {
                con.Open();
                da = new OleDbDataAdapter("SELECT * FROM Customers", con);
                dt = new DataTable();
                da.Fill(dt);
                dgvCustomers.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading customers: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void ClearFields()
        {
            txtName.Clear();
            cmbGender.SelectedIndex = 0;
            txtAddress.Clear();
            txtPhone.Clear();
            dtpDOB.Value = DateTime.Today;
            selectedCustomerID = -1;
        }
        private void Check()
        {
            double num;
            if (double.TryParse(txtPhone.Text, out num))
            {
                // Do nothing
            }
            else
            {
                MessageBox.Show("Field Phone can only accept integral values",
                    "");
                return;
            }
            if (txtPhone.TextLength > 12)
            {
                MessageBox.Show("Field Phone can't exceed 12 charecters",
                    "");
                return;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                MessageBox.Show("Name and Phone are required.");
                return;
            }
            Check();
            try
            {
                con.Open();
                cmd = new OleDbCommand(
                    "INSERT INTO Customers (Name, Gender, Address, Phone, DOB) VALUES (?, ?, ?, ?, ?)",
                    con
                );
                cmd.Parameters.AddWithValue("?", txtName.Text);
                cmd.Parameters.AddWithValue("?", cmbGender.Text);
                cmd.Parameters.AddWithValue("?", txtAddress.Text);
                cmd.Parameters.AddWithValue("?", txtPhone.Text);
                cmd.Parameters.AddWithValue("?", dtpDOB.Value.Date);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Customer added!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding customer: " + ex.Message);
            }
            finally
            {
                con.Close();
                LoadCustomers();
                ClearFields();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedCustomerID == -1)
            {
                MessageBox.Show("Select a customer to update.");
                return;
            }

            try
            {
                con.Open();
                cmd = new OleDbCommand(
                    "UPDATE Customers SET Name=?, Gender=?, Address=?, Phone=?, DOB=? WHERE ID=?",
                    con
                );
                cmd.Parameters.AddWithValue("?", txtName.Text);
                cmd.Parameters.AddWithValue("?", cmbGender.Text);
                cmd.Parameters.AddWithValue("?", txtAddress.Text);
                cmd.Parameters.AddWithValue("?", txtPhone.Text);
                cmd.Parameters.AddWithValue("?", dtpDOB.Value.Date);
                cmd.Parameters.AddWithValue("?", selectedCustomerID);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Customer updated!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating customer: " + ex.Message);
            }
            finally
            {
                con.Close();
                LoadCustomers();
                ClearFields();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedCustomerID == -1)
            {
                MessageBox.Show("Select a customer to delete.");
                return;
            }

            var ok = MessageBox.Show(
                "Really delete this customer?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );
            if (ok != DialogResult.Yes) return;

            try
            {
                con.Open();
                cmd = new OleDbCommand("DELETE FROM Customers WHERE ID=?", con);
                cmd.Parameters.AddWithValue("?", selectedCustomerID);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Customer deleted!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting customer: " + ex.Message);
            }
            finally
            {
                con.Close();
                LoadCustomers();
                ClearFields();
            }
        }

        private void dgvCustomers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgvCustomers.Rows[e.RowIndex];
            selectedCustomerID = Convert.ToInt32(row.Cells["ID"].Value);
            txtName.Text = row.Cells["Name"].Value.ToString();
            cmbGender.Text = row.Cells["Gender"].Value.ToString();
            txtAddress.Text = row.Cells["Address"].Value.ToString();
            txtPhone.Text = row.Cells["Phone"].Value.ToString();
            dtpDOB.Value = Convert.ToDateTime(row.Cells["DOB"].Value);
        }
    }
}

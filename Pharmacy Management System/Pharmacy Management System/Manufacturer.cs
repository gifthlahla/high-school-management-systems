using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Pharmacy_Management_System
{
    public partial class Manufacturer : Form
    {
        OleDbConnection con = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Pharmacy.accdb");
        OleDbCommand cmd;
        OleDbDataAdapter da;
        DataTable dt;
        private int selectedManuID = -1;  // store the selected row's ID

        public Manufacturer()
        {
            InitializeComponent();
            LoadManufacturers();
        }

        private void LoadManufacturers()
        {
            try
            {
                con.Open();
                da = new OleDbDataAdapter("SELECT * FROM Manufacturers", con);
                dt = new DataTable();
                da.Fill(dt);
                dgvManufacturer.DataSource = dt;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message);
                con.Close();
            }
        }

        private void ClearFields()
        {
            txtManufacturer.Clear();
            txtAddress.Clear();
            txtPhone.Clear();
            selectedManuID = -1; // reset the selected ID
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtManufacturer.Text == "" || txtPhone.Text == "")
            {
                MessageBox.Show("Please enter all required fields.");
                return;
            }

            try
            {
                con.Open();
                cmd = new OleDbCommand("INSERT INTO Manufacturers (Name, Address, Phone) VALUES (?, ?, ?)", con);
                cmd.Parameters.AddWithValue("?", txtManufacturer.Text);
                cmd.Parameters.AddWithValue("?", txtAddress.Text);
                cmd.Parameters.AddWithValue("?", txtPhone.Text);
                cmd.ExecuteNonQuery();
                con.Close();

                MessageBox.Show("Manufacturer added successfully!");
                LoadManufacturers();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding manufacturer: " + ex.Message);
                con.Close();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedManuID == -1)
            {
                MessageBox.Show("Please select a manufacturer to update.");
                return;
            }

            try
            {
                con.Open();
                cmd = new OleDbCommand("UPDATE Manufacturers SET Name=?, Address=?, Phone=? WHERE ID=?", con);
                cmd.Parameters.AddWithValue("?", txtManufacturer.Text);
                cmd.Parameters.AddWithValue("?", txtAddress.Text);
                cmd.Parameters.AddWithValue("?", txtPhone.Text);
                cmd.Parameters.AddWithValue("?", selectedManuID);
                cmd.ExecuteNonQuery();
                con.Close();

                MessageBox.Show("Manufacturer updated successfully!");
                LoadManufacturers();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating manufacturer: " + ex.Message);
                con.Close();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedManuID == -1)
            {
                MessageBox.Show("Please select a manufacturer to delete.");
                return;
            }

            try
            {
                con.Open();
                cmd = new OleDbCommand("DELETE FROM Manufacturers WHERE ID=?", con);
                cmd.Parameters.AddWithValue("?", selectedManuID);
                cmd.ExecuteNonQuery();
                con.Close();

                MessageBox.Show("Manufacturer deleted successfully!");
                LoadManufacturers();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting manufacturer: " + ex.Message);
                con.Close();
            }
        }

        private void dgvManufacturer_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvManufacturer.Rows[e.RowIndex];
                selectedManuID = Convert.ToInt32(row.Cells["ID"].Value);
                txtManufacturer.Text = row.Cells["Name"].Value.ToString();
                txtAddress.Text = row.Cells["Address"].Value.ToString();
                txtPhone.Text = row.Cells["Phone"].Value.ToString();
            }
        }
    }
}

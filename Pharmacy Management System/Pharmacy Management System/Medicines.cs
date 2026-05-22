using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Pharmacy_Management_System
{
    public partial class Medicines : Form
    {
        //–– DB connection
        private OleDbConnection con = new OleDbConnection(
            @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Pharmacy.accdb"
        );
        private OleDbCommand cmd;
        private OleDbDataAdapter da;
        private DataTable dt;

        //–– Holds selected record
        private int selectedMedicineID = -1;

        public Medicines()
        {
            InitializeComponent();
        }

        private void Medicines_Load(object sender, EventArgs e)
        {
            PopulateTypeCombo();
            PopulateManufacturerCombo();
            LoadMedicines();
        }

        private void PopulateTypeCombo()
        {
            cmbType.Items.Clear();
            cmbType.Items.AddRange(new[] {
                "Tablet", "Capsule", "Syrup", "Injection", "Ointment"
            });
            cmbType.SelectedIndex = 0;
        }

        private void PopulateManufacturerCombo()
        {
            try
            {
                con.Open();
                da = new OleDbDataAdapter("SELECT ID, Name FROM Manufacturers", con);
                dt = new DataTable();
                da.Fill(dt);
                cmbManufacturerID.DataSource = dt;
                cmbManufacturerID.DisplayMember = "ID";
                cmbManufacturerID.ValueMember = "ID";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading manufacturers: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void LoadMedicines()
        {
            try
            {
                con.Open();
                string sql = @"
                    SELECT 
                      m.ID, 
                      m.MedicineName, 
                      m.Type, 
                      m.Quantity, 
                      m.Price, 
                      m.ManufacturerID,
                      n.Name AS ManufacturerName
                    FROM Medicines AS m
                    LEFT JOIN Manufacturers AS n
                      ON m.ManufacturerID = n.ID";
                da = new OleDbDataAdapter(sql, con);
                dt = new DataTable();
                da.Fill(dt);
                dgvMedicines.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading medicines: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void ClearFields()
        {
            txtMedicine.Clear();
            cmbType.SelectedIndex = 0;
            txtQuantity.Clear();
            txtPrice.Clear();
            cmbManufacturerID.SelectedIndex = 0;
            lblManufacturerName.Clear();
            selectedMedicineID = -1;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMedicine.Text))
            {
                MessageBox.Show("Medicine Name is required.");
                return;
            }

            try
            {
                con.Open();
                cmd = new OleDbCommand(
                    "INSERT INTO Medicines (MedicineName, Type, Quantity, Price, ManufacturerID) " +
                    "VALUES (?, ?, ?, ?, ?)",
                    con
                );
                cmd.Parameters.AddWithValue("?", txtMedicine.Text);
                cmd.Parameters.AddWithValue("?", cmbType.Text);
                cmd.Parameters.AddWithValue("?", int.Parse(txtQuantity.Text));
                cmd.Parameters.AddWithValue("?", decimal.Parse(txtPrice.Text));
                cmd.Parameters.AddWithValue("?", cmbManufacturerID.SelectedValue);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Medicine added!");
                LoadMedicines();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving medicine: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedMedicineID == -1)
            {
                MessageBox.Show("Select a medicine to update.");
                return;
            }

            try
            {
                con.Open();
                cmd = new OleDbCommand(
                    "UPDATE Medicines SET MedicineName=?, Type=?, Quantity=?, Price=?, ManufacturerID=? " +
                    "WHERE ID=?",
                    con
                );
                cmd.Parameters.AddWithValue("?", txtMedicine.Text);
                cmd.Parameters.AddWithValue("?", cmbType.Text);
                cmd.Parameters.AddWithValue("?", int.Parse(txtQuantity.Text));
                cmd.Parameters.AddWithValue("?", decimal.Parse(txtPrice.Text));
                cmd.Parameters.AddWithValue("?", cmbManufacturerID.SelectedValue);
                cmd.Parameters.AddWithValue("?", selectedMedicineID);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Medicine updated!");
                LoadMedicines();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating medicine: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedMedicineID == -1)
            {
                MessageBox.Show("Select a medicine to delete.");
                return;
            }

            if (MessageBox.Show(
                    "Are you sure you want to delete this medicine?",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                ) != DialogResult.Yes) return;

            try
            {
                con.Open();
                cmd = new OleDbCommand("DELETE FROM Medicines WHERE ID=?", con);
                cmd.Parameters.AddWithValue("?", selectedMedicineID);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Medicine deleted!");
                LoadMedicines();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting medicine: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void dgvMedicines_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgvMedicines.Rows[e.RowIndex];
            selectedMedicineID        = Convert.ToInt32(row.Cells["ID"].Value);
            txtMedicine.Text          = row.Cells["MedicineName"].Value.ToString();
            cmbType.Text              = row.Cells["Type"].Value.ToString();
            txtQuantity.Text          = row.Cells["Quantity"].Value.ToString();
            txtPrice.Text             = row.Cells["Price"].Value.ToString();
            cmbManufacturerID.SelectedValue = row.Cells["ManufacturerID"].Value;
            lblManufacturerName.Text  = row.Cells["ManufacturerName"].Value.ToString();
        }

        private void cmbManufacturerID_SelectedIndexChanged(object sender, EventArgs e)
        {
            // When user picks an ID, show its name
            if (cmbManufacturerID.SelectedValue == null) return;

            try
            {
                con.Open();
                cmd = new OleDbCommand(
                    "SELECT Name FROM Manufacturers WHERE ID = ?",
                    con
                );
                cmd.Parameters.AddWithValue("?", cmbManufacturerID.SelectedValue);
                object result = cmd.ExecuteScalar();
                lblManufacturerName.Text = result.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading name: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
    }
}

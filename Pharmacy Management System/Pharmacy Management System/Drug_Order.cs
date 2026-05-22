using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Pharmacy_Management_System
{
    public partial class Drug_Order : Form
    {
        private OleDbConnection con = new OleDbConnection(
            @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Pharmacy.accdb"
        );
        private OleDbCommand cmd;
        private OleDbDataAdapter da;
        private DataTable dtGrid;
        private decimal grandTotal;

        public Drug_Order()
        {
            InitializeComponent();
            InitializeBillingGrid();
            PopulateCustomers();
            PopulateMedicines();

            // wire up quantity change event (if not already wired in Designer)
            this.txtQuantity.TextChanged += txtQuantity_TextChanged;
        }

        private void Drug_Order_Load(object sender, EventArgs e)
        {
            NewTransaction();
        }

        private void InitializeBillingGrid()
        {
            dtGrid = new DataTable();
            dtGrid.Columns.Add("MedicineID", typeof(int));
            dtGrid.Columns.Add("MedicineName", typeof(string));
            dtGrid.Columns.Add("Quantity", typeof(int));
            dtGrid.Columns.Add("UnitPrice", typeof(decimal));
            dtGrid.Columns.Add("LineTotal", typeof(decimal));

            dgvBilling.DataSource = dtGrid;
            dgvBilling.Columns["MedicineID"].Visible = false;
        }

        private void PopulateCustomers()
        {
            DataTable dtCust = new DataTable();
            da = new OleDbDataAdapter("SELECT ID, [Name] FROM Customers", con);
            da.Fill(dtCust);

            cmbCustomerName.DataSource = dtCust;
            cmbCustomerName.DisplayMember = "Name";
            cmbCustomerName.ValueMember = "ID";
            cmbCustomerName.SelectedIndexChanged += cmbCustomerName_SelectedIndexChanged;
        }

        private void PopulateMedicines()
        {
            DataTable dtMed = new DataTable();
            da = new OleDbDataAdapter("SELECT ID, MedicineName, Price FROM Medicines", con);
            da.Fill(dtMed);

            cmbMedicineName.DataSource = dtMed;
            cmbMedicineName.DisplayMember = "MedicineName";
            cmbMedicineName.ValueMember = "ID";
            cmbMedicineName.SelectedIndexChanged += cmbMedicineName_SelectedIndexChanged;
        }

        private void NewTransaction()
        {
            if (cmbCustomerName.Items.Count > 0) cmbCustomerName.SelectedIndex = 0;
            if (cmbMedicineName.Items.Count > 0) cmbMedicineName.SelectedIndex = 0;

            txtQuantity.Clear();
            lblPhone.Clear();
            lblGender.Clear();
            lblDOB.Clear();
            lblPrice.Clear();
            lblTotal.Clear();

            dtGrid.Rows.Clear();
            grandTotal = 0m;
            label12.Text = "$0.00";
        }

        private void cmbCustomerName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbCustomerName.SelectedValue == null) return;
            int cid = Convert.ToInt32(cmbCustomerName.SelectedValue);

            try
            {
                con.Open();
                cmd = new OleDbCommand(
                    "SELECT Phone, Gender, DOB FROM Customers WHERE ID = ?", con
                );
                cmd.Parameters.AddWithValue("?", cid);
                OleDbDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    lblPhone.Text = rdr["Phone"].ToString();
                    lblGender.Text = rdr["Gender"].ToString();
                    lblDOB.Text = Convert.ToDateTime(rdr["DOB"]).ToShortDateString();
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading customer data: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void cmbMedicineName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbMedicineName.SelectedValue == null) return;
            int mid = Convert.ToInt32(cmbMedicineName.SelectedValue);

            try
            {
                con.Open();
                cmd = new OleDbCommand("SELECT Price FROM Medicines WHERE ID = ?", con);
                cmd.Parameters.AddWithValue("?", mid);
                object o = cmd.ExecuteScalar();
                if (o != null)
                    lblPrice.Text = Convert.ToDecimal(o).ToString("0.00");
                else
                    lblPrice.Text = "0.00";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading medicine price: " + ex.Message);
            }
            finally
            {
                con.Close();
            }

            UpdateLineTotal();
        }

        private void txtQuantity_TextChanged(object sender, EventArgs e)
        {
            UpdateLineTotal();
        }

        private void UpdateLineTotal()
        {
            decimal price;
            int qty;
            if (Decimal.TryParse(lblPrice.Text, out price) &&
                Int32.TryParse(txtQuantity.Text, out qty))
            {
                lblTotal.Text = (price * qty).ToString("0.00");
            }
            else
            {
                lblTotal.Clear();
            }
        }

        private void btnAddToBill_Click(object sender, EventArgs e)
        {
            int qty;
            if (!Int32.TryParse(txtQuantity.Text, out qty) || qty <= 0)
            {
                MessageBox.Show("Enter a valid quantity.");
                return;
            }

            int mid = Convert.ToInt32(cmbMedicineName.SelectedValue);
            string name = cmbMedicineName.Text;
            decimal unit;
            Decimal.TryParse(lblPrice.Text, out unit);
            decimal lineTotal = unit * qty;

            dtGrid.Rows.Add(mid, name, qty, unit, lineTotal);
            grandTotal += lineTotal;
            label12.Text = "$" + grandTotal.ToString("0.00");

            cmbMedicineName.SelectedIndex = 0;
            txtQuantity.Clear();
        }

        private void btnNewTrans_Click(object sender, EventArgs e)
        {
            NewTransaction();
        }

        private void btnPrintReceipt_Click(object sender, EventArgs e)
        {
            if (dtGrid.Rows.Count == 0)
            {
                MessageBox.Show("No items to bill.");
                return;
            }

            int orderId;
            try
            {
                con.Open();
                // Insert master record
                cmd = new OleDbCommand(
                    "INSERT INTO Orders (CustomerID, OrderDate, Total) VALUES (?, ?, ?)",
                    con
                );
                cmd.Parameters.AddWithValue("?", Convert.ToInt32(cmbCustomerName.SelectedValue));
                cmd.Parameters.AddWithValue("?", DateTime.Now);
                cmd.Parameters.AddWithValue("?", grandTotal);
                cmd.ExecuteNonQuery();

                // Get new OrderID
                cmd.CommandText = "SELECT @@IDENTITY";
                orderId = Convert.ToInt32(cmd.ExecuteScalar());

                // Insert detail lines
                foreach (DataRow r in dtGrid.Rows)
                {
                    cmd.CommandText = @"
                        INSERT INTO OrderItems 
                          (OrderID, MedicineID, Quantity, UnitPrice, LineTotal)
                        VALUES (?, ?, ?, ?, ?)";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("?", orderId);
                    cmd.Parameters.AddWithValue("?", (int)r["MedicineID"]);
                    cmd.Parameters.AddWithValue("?", (int)r["Quantity"]);
                    cmd.Parameters.AddWithValue("?", (decimal)r["UnitPrice"]);
                    cmd.Parameters.AddWithValue("?", (decimal)r["LineTotal"]);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Receipt printed and saved!\nOrder # " + orderId);
                NewTransaction();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving transaction: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void dgvBilling_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // optional: implement row removal or editing here
        }
    }
}

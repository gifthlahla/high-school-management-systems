using System;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Pharmacy_Management_System
{
    public partial class DashBoard : Form
    {
        private OleDbConnection con = new OleDbConnection(
            @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Pharmacy.accdb"
        );

        public DashBoard()
        {
            InitializeComponent();
            this.Load += DashBoard_Load;
        }

        private void DashBoard_Load(object sender, EventArgs e)
        {
            lblCustomers.Text = GetTableCount("Customers").ToString();
            lblStaff.Text = GetTableCount("Staffs").ToString();
            lblManufacturers.Text = GetTableCount("Manufacturers").ToString();
            lblMedicines.Text = GetTableCount("Medicines").ToString();
        }

        private int GetTableCount(string tableName)
        {
            int count = 0;
            try
            {
                con.Open();
                string query = string.Format("SELECT COUNT(*) FROM {0}", tableName);
                using (var cmd = new OleDbCommand(query, con))
                {
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                        count = Convert.ToInt32(result);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    string.Format("Error counting rows in {0}: {1}", tableName, ex.Message),
                    "Dashboard Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            finally
            {
                con.Close();
            }
            return count;
        }
    }
}

using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Payroll_Management_System
{
    public partial class DashBoard : Form
    {
        private OleDbConnection con;
        public DashBoard()
        {
            InitializeComponent();
            con = new OleDbConnection(
                @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Payroll.accdb;"
            );
        }

        private void DashBoard_Load(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "SELECT COUNT(*) FROM Employees";
                    lblManager.Text = cmd.ExecuteScalar().ToString();
                }
                
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "SELECT COUNT(*) FROM IssueSalary";
                    lblSalary.Text = cmd.ExecuteScalar().ToString();
                }
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "SELECT COUNT(*) FROM Bonus";
                    lblBonus.Text = cmd.ExecuteScalar().ToString();
                }
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "SELECT COUNT(*) FROM Advance";
                    lblAdvance.Text = cmd.ExecuteScalar().ToString();
                }                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading dashboard data:\n" + ex.Message,
                                "Dashboard Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }
    }
}

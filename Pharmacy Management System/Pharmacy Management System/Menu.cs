using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pharmacy_Management_System
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public partial class Menu : Form
    {
        private Button crntbtn;
        private Panel lftbrderbtn;

        // Declare instances of the forms
        private DashBoard dashboardForm;
        private Customer customerForm;
        private Drug_Order  drugorderForm;
        private Medicines medicinesForm;
        private Staff sellerForm;
        private Manufacturer manufacturerForm;

        public Menu()
        {
            // Initialize components
            InitializeComponent();

            // Create instances of the forms
            dashboardForm = new DashBoard();
            customerForm = new Customer();
            drugorderForm = new Drug_Order();
            medicinesForm = new Medicines();
            sellerForm = new Staff();
            manufacturerForm = new Manufacturer();

            // Create the left border panel
            lftbrderbtn = new Panel
            {
                Size = new Size(5, 40)
            };
            pnlTab.Controls.Add(lftbrderbtn);
        }

        private void disablebtn()
        {
            if (crntbtn != null)
            {
                crntbtn.BackColor = Color.Transparent;
                crntbtn.ForeColor = Color.FromArgb(239, 238, 238);
            }
        }

        private void activationbtn(object senderbtn, Color customcolor)
        {
            if (senderbtn != null)
            {
                disablebtn();
                crntbtn = (Button)senderbtn;
                crntbtn.BackColor = Color.FromArgb(238, 236, 237);
                crntbtn.ForeColor = Color.FromArgb(0, 95, 65);
                lftbrderbtn.BackColor = Color.FromArgb(0, 95, 65);
                lftbrderbtn.Location = new Point(0, crntbtn.Location.Y);
                lftbrderbtn.BringToFront();
            }
        }
        
        private void btnDashboard_Click(object sender, EventArgs e)
        {
            activationbtn(sender, Color.DeepSkyBlue);
            dashboardForm.TopLevel = false;
            pnlDisplay.Controls.Add(dashboardForm);
            dashboardForm.BringToFront();
            dashboardForm.Show();
        }

        private void btnMedicines_Click(object sender, EventArgs e)
        {
            activationbtn(sender, Color.DeepSkyBlue);
            medicinesForm.TopLevel = false;
            pnlDisplay.Controls.Add(medicinesForm);
            medicinesForm.BringToFront();
            medicinesForm.Show();
        }

        private void btnCustomers_Click(object sender, EventArgs e)
        {
            activationbtn(sender, Color.DeepSkyBlue);
            customerForm.TopLevel = false;
            pnlDisplay.Controls.Add(customerForm);
            customerForm.BringToFront();
            customerForm.Show();
        }

        private void btnManufacturer_Click(object sender, EventArgs e)
        {
            activationbtn(sender, Color.DeepSkyBlue);
            manufacturerForm.TopLevel = false;
            pnlDisplay.Controls.Add(manufacturerForm);
            manufacturerForm.BringToFront();
            manufacturerForm.Show();
        }

        private void btnSeller_Click(object sender, EventArgs e)
        {
            activationbtn(sender, Color.DeepSkyBlue);
            sellerForm.TopLevel = false;
            pnlDisplay.Controls.Add(sellerForm);
            sellerForm.BringToFront();
            sellerForm.Show();
        }

        private void btnDrug_Order_Click(object sender, EventArgs e)
        {
            activationbtn(sender, Color.DeepSkyBlue);
            drugorderForm.TopLevel = false;
            pnlDisplay.Controls.Add(drugorderForm);
            drugorderForm.BringToFront();
            drugorderForm.Show();
            
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            activationbtn(sender, Color.DeepSkyBlue);
            DialogResult Leave = MessageBox.Show("Do you want to exit?", "Confirm to exit...", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (Leave == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void Menu_Load(object sender, EventArgs e)
        {
            btnDashboard.PerformClick();
        }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hospital_Management_System
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
        private Staff staffForm;
        private Patients patientsForm;
        private Diagnostics diagnosticsForm;
        private Medicines medicineForm;
        private Accounts accForm;

        public Menu()
        {
            // Initialize components
            InitializeComponent();

            // Create instances of the forms
            dashboardForm = new DashBoard();
            staffForm = new Staff();
            patientsForm = new Patients();
            diagnosticsForm = new Diagnostics();
            medicineForm = new Medicines();
            accForm = new Accounts();

            // Create the left border panel
            lftbrderbtn = new Panel
            {
                Size = new Size(5, 40)
            };
            pnlTab.Controls.Add(lftbrderbtn);
        }
        public static Button previousButton;
        public static void GoToPreviousButton()
        {
            if (previousButton != null)
            {
                previousButton.PerformClick();
            }
        }
        private void disablebtn()
        {
            if (crntbtn != null)
            {
                crntbtn.BackColor = Color.Transparent;
                crntbtn.ForeColor = Color.White;
            }
        }

        private void activationbtn(object senderbtn, Color customcolor)
        {
            if (senderbtn != null)
            {
                disablebtn();
                crntbtn = (Button)senderbtn;
                crntbtn.BackColor = Color.FromArgb(242, 248, 252);
                crntbtn.ForeColor = Color.FromArgb(107, 208, 248);
                lftbrderbtn.BackColor = Color.FromArgb(107, 208, 248);
                lftbrderbtn.Location = new Point(0, crntbtn.Location.Y);
                lftbrderbtn.BringToFront();
            }
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
            else
            {
                GoToPreviousButton();
            }
        }
        private void btnAccounts_Click(object sender, EventArgs e)
        {
            activationbtn(sender, Color.DeepSkyBlue);
            try
            {
                if (accForm == null || accForm.IsDisposed || !accForm.Visible)
                {
                    accForm =new Accounts();
                    accForm.FormClosed += (s, args) => accForm = null;
                    accForm.Show();
                }
                else
                {
                    accForm.BringToFront();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            
        }

        private void btnMedicines_Click(object sender, EventArgs e)
        {
            previousButton = (Button)sender;
            activationbtn(sender, Color.DeepSkyBlue);
            medicineForm.TopLevel = false;
            pnlDisplay.Controls.Add(medicineForm);
            medicineForm.BringToFront();
            medicineForm.Show();
        }

        private void btnDiagnostics_Click(object sender, EventArgs e)
        {
            previousButton = (Button)sender;
            activationbtn(sender, Color.DeepSkyBlue);
            diagnosticsForm.TopLevel = false;
            pnlDisplay.Controls.Add(diagnosticsForm);
            diagnosticsForm.BringToFront();
            diagnosticsForm.Show();
        }

        private void btnPatience_Click(object sender, EventArgs e)
        {
            previousButton = (Button)sender;
            activationbtn(sender, Color.DeepSkyBlue);
            patientsForm.TopLevel = false;
            pnlDisplay.Controls.Add(patientsForm);
            patientsForm.BringToFront();
            patientsForm.Show();
        }

        private void btnStaff_Click(object sender, EventArgs e)
        {
            previousButton = (Button)sender;
            activationbtn(sender, Color.DeepSkyBlue);
            staffForm.TopLevel = false;
            pnlDisplay.Controls.Add(staffForm);
            staffForm.BringToFront();
            staffForm.Show();
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            previousButton = (Button)sender;
            activationbtn(sender, Color.DeepSkyBlue);
            dashboardForm.TopLevel = false;
            pnlDisplay.Controls.Add(dashboardForm);
            dashboardForm.BringToFront();
            dashboardForm.Show();
        }

        private void Menu_Load(object sender, EventArgs e)
        {
            btnDashboard.PerformClick();
        }
    }
}


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Employee_Attendance_System
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public partial class Menu : Form
    {
        private Button crntbtn;
        private Panel lftbrderbtn;

        // Declare instances of the forms
        private Account accForm;
        private Employees employeesForm;
        private Attendance attendanceForm;
        private Leave leaveForm;

        public Menu()
        {
            // Initialize components
            InitializeComponent();

            // Create instances of the forms
            accForm = new Account();
            employeesForm = new Employees();
            attendanceForm = new Attendance();
            leaveForm = new Leave();

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
                crntbtn.BackColor = Color.FromArgb(255, 255, 246);
                crntbtn.ForeColor = Color.FromArgb(120, 118, 93);
                lftbrderbtn.BackColor = Color.FromArgb(120, 118, 93);
                lftbrderbtn.Location = new Point(0, crntbtn.Location.Y);
                lftbrderbtn.BringToFront();
            }
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnEmployee_Click(object sender, EventArgs e)
        {
            previousButton = (Button)sender; 
            activationbtn(sender, Color.DeepSkyBlue);
            employeesForm.TopLevel = false;
            pnlDisplay.Controls.Add(employeesForm);
            employeesForm.BringToFront();
            employeesForm.Show();
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

        private void btnAttendance_Click(object sender, EventArgs e)
        {
            previousButton = (Button)sender;
            activationbtn(sender, Color.DeepSkyBlue);
            attendanceForm.TopLevel = false;
            pnlDisplay.Controls.Add(attendanceForm);
            attendanceForm.BringToFront();
            attendanceForm.Show();
        }

        private void btnLeave_Click(object sender, EventArgs e)
        {
            previousButton = (Button)sender; 
            activationbtn(sender, Color.DeepSkyBlue);
            leaveForm.TopLevel = false;
            pnlDisplay.Controls.Add(leaveForm);
            leaveForm.BringToFront();
            leaveForm.Show();
        }
        private void btnAccount_Click(object sender, EventArgs e)
        {
            activationbtn(sender, Color.DeepSkyBlue);
            try
            {
                if (accForm == null || accForm.IsDisposed || !accForm.Visible)
                {
                    accForm = new Account();
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

        private void Menu_Load(object sender, EventArgs e)
        {
            btnEmployee.PerformClick();
        }
    }
}


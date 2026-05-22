using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Payroll_Management_System
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
        private Employees employeesForm;
        private Bonus  bonusForm;
        private Advance advanceForm;
        private Attendance attendanceForm;
        private Issue_Salary issuesalaryForm;

        public Menu()
        {
            // Initialize components
            InitializeComponent();

            // Create instances of the forms
            dashboardForm = new DashBoard();
            employeesForm = new Employees();
            bonusForm = new Bonus();
            advanceForm = new Advance();
            attendanceForm = new Attendance();
            issuesalaryForm = new Issue_Salary();

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
                crntbtn.ForeColor = Color.FromArgb(21, 21, 21);
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

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            activationbtn(sender, Color.DeepSkyBlue);
            dashboardForm.TopLevel = false;
            pnlDisplay.Controls.Add(dashboardForm);
            dashboardForm.BringToFront();
            dashboardForm.Show();
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

        private void btnEmployees_Click(object sender, EventArgs e)
        {
            activationbtn(sender, Color.DeepSkyBlue);
            employeesForm.TopLevel = false;
            pnlDisplay.Controls.Add(employeesForm);
            employeesForm.BringToFront();
            employeesForm.Show();
        }

        private void btnBonus_Click(object sender, EventArgs e)
        {
            activationbtn(sender, Color.DeepSkyBlue);
            bonusForm.TopLevel = false;
            pnlDisplay.Controls.Add(bonusForm);
            bonusForm.BringToFront();
            bonusForm.Show();
        }

        private void btnAdvance_Click(object sender, EventArgs e)
        {
            activationbtn(sender, Color.DeepSkyBlue);
            advanceForm.TopLevel = false;
            pnlDisplay.Controls.Add(advanceForm);
            advanceForm.BringToFront();
            advanceForm.Show();
        }

        private void btnAttendance_Click(object sender, EventArgs e)
        {
            activationbtn(sender, Color.DeepSkyBlue);
            attendanceForm.TopLevel = false;
            pnlDisplay.Controls.Add(attendanceForm);
            attendanceForm.BringToFront();
            attendanceForm.Show();
        }

        private void btnIssueSalary_Click(object sender, EventArgs e)
        {
            activationbtn(sender, Color.DeepSkyBlue);
            issuesalaryForm.TopLevel = false;
            pnlDisplay.Controls.Add(issuesalaryForm);
            issuesalaryForm.BringToFront();
            issuesalaryForm.Show();
        }

        private void Menu_Load(object sender, EventArgs e)
        {
            btnDashboard.PerformClick();
        }

    }
}


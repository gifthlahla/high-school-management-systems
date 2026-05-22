using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Voting_System
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public partial class Menu : Form
    {
        private Button crntbtn;
        private Panel lftbrderbtn;

        // Declare instances of the forms
        private Results resultsForm;
        private Students studentsForm;
        private Voters  votersForm;
        private Candidates candidatesForm;
        private Settings settingsForm;

        public Menu()
        {
            // Initialize components
            InitializeComponent();

            // Create instances of the forms
            resultsForm = new Results();
            studentsForm  = new Students();
            votersForm  = new Voters();
            candidatesForm = new Candidates();
            settingsForm = new Settings();

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
                crntbtn.ForeColor = Color.FromArgb(255, 126, 124);
            }
        }

        private void activationbtn(object senderbtn, Color customcolor)
        {
            if (senderbtn != null)
            {
                disablebtn();
                crntbtn = (Button)senderbtn;
                crntbtn.BackColor = Color.White;
                crntbtn.ForeColor = Color.FromArgb(119, 101, 101);
                lftbrderbtn.BackColor = Color.FromArgb(119, 101, 101);
                lftbrderbtn.Location = new Point(0, crntbtn.Location.Y);
                lftbrderbtn.BringToFront();
            }
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            activationbtn(sender, Color.DeepSkyBlue);
            resultsForm.TopLevel = false;
            pnlDisplay.Controls.Add(resultsForm);
            resultsForm.BringToFront();
            resultsForm.Show();
        }

        private void btnStudents_Click(object sender, EventArgs e)
        {
            activationbtn(sender, Color.DeepSkyBlue);
            studentsForm.TopLevel = false;
            pnlDisplay.Controls.Add(studentsForm);
            studentsForm.BringToFront();
            studentsForm.Show();
        }

        private void btnVoters_Click(object sender, EventArgs e)
        {
            activationbtn(sender, Color.DeepSkyBlue);
            votersForm.TopLevel = false;
            pnlDisplay.Controls.Add(votersForm);
            votersForm.BringToFront();
            votersForm.Show();
        }

        private void btnCandidates_Click(object sender, EventArgs e)
        {
            activationbtn(sender, Color.DeepSkyBlue);
            candidatesForm.TopLevel = false;
            pnlDisplay.Controls.Add(candidatesForm);
            candidatesForm.BringToFront();
            candidatesForm.Show();
        }

        private void btnUpdates_Click(object sender, EventArgs e)
        {
            activationbtn(sender, Color.DeepSkyBlue);
            settingsForm.TopLevel = false;
            pnlDisplay.Controls.Add(settingsForm);
            settingsForm.BringToFront();
            settingsForm.Show();
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

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Menu_Load(object sender, EventArgs e)
        {
            btnResults.PerformClick();
        }
    }
}
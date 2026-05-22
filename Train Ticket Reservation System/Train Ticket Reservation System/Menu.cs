
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Train_Ticket_Reservation_System
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public partial class Menu : Form
    {
        private Button crntbtn;
        private Panel lftbrderbtn;

        // Declare instances of the forms
        private Train trainForm;
        private Passangers passangerForm;
        private Ticket ticketForm;
        private Cancellation cancelForm;
        private Accounts accForm;

        public Menu()
        {
            // Initialize components
            InitializeComponent();

            // Create instances of the forms
            trainForm = new Train();
            passangerForm = new Passangers();
            ticketForm = new Ticket();
            cancelForm = new Cancellation();
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
                crntbtn.ForeColor = Color.FromArgb(21, 21, 21);
            }
        }

        private void activationbtn(object senderbtn, Color customcolor)
        {
            if (senderbtn != null)
            {
                disablebtn();
                crntbtn = (Button)senderbtn;
                crntbtn.BackColor = Color.WhiteSmoke;
                crntbtn.ForeColor = Color.FromArgb(138, 138, 136);
                lftbrderbtn.BackColor = Color.FromArgb(138, 138, 136);
                lftbrderbtn.Location = new Point(0, crntbtn.Location.Y);
                lftbrderbtn.BringToFront();
            }
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

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnTrains_Click(object sender, EventArgs e)
        {
            previousButton = (Button)sender;
            activationbtn(sender, Color.DeepSkyBlue);
            trainForm.TopLevel = false;
            pnlDisplay.Controls.Add(trainForm);
            trainForm.BringToFront();
            trainForm.Show();
        }

        private void btnPassangers_Click(object sender, EventArgs e)
        {
            previousButton = (Button)sender;
            activationbtn(sender, Color.DeepSkyBlue);
            passangerForm.TopLevel = false;
            pnlDisplay.Controls.Add(passangerForm);
            passangerForm.BringToFront();
            passangerForm.Show();
        }

        private void btnTickets_Click(object sender, EventArgs e)
        {
            previousButton = (Button)sender;
            activationbtn(sender, Color.DeepSkyBlue);
            ticketForm.TopLevel = false;
            pnlDisplay.Controls.Add(ticketForm);
            ticketForm.BringToFront();
            ticketForm.Show();
        }

        private void btnCancellation_Click(object sender, EventArgs e)
        {
            previousButton = (Button)sender;
            activationbtn(sender, Color.DeepSkyBlue);
            cancelForm.TopLevel = false;
            pnlDisplay.Controls.Add(cancelForm);
            cancelForm.BringToFront();
            cancelForm.Show();
        }

        private void btnAccounts_Click(object sender, EventArgs e)
        {
            activationbtn(sender, Color.DeepSkyBlue);
            try
            {
                if (accForm == null || accForm.IsDisposed || !accForm.Visible)
                {
                    accForm = new Accounts();
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
            btnTrains.PerformClick();
        }
    }
}



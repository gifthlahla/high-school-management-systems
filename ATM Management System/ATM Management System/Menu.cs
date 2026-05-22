using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ATM_Management_System
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public partial class Menu : Form
    {
        private Button crntbtn;
        private Panel lftbrderbtn;

        // Declare instances of the forms
        private ATM atmForm;
        private Card cardForm;
        private Account accountForm;
        private Transaction transactionForm;
        private User userForm;
        private Authentication authenticateForm;

        public Menu()
        {
            // Initialize components
            InitializeComponent();

            // Create instances of the forms
            atmForm = new ATM();
            cardForm = new Card();
            accountForm = new Account();
            transactionForm = new Transaction();
            userForm = new User();
            authenticateForm = new Authentication();

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
                crntbtn.BackColor = Color.FromArgb(255, 255, 239);
                crntbtn.ForeColor = Color.FromArgb(229, 128, 0);
                lftbrderbtn.BackColor = Color.FromArgb(229, 128, 0);
                lftbrderbtn.Location = new Point(0, crntbtn.Location.Y);
                lftbrderbtn.BringToFront();
            }
        }
        private void btnUser_Click(object sender, EventArgs e)
        {
            previousButton = (Button)sender;
            activationbtn(sender, Color.DeepSkyBlue);
            userForm.TopLevel = false;
            pnlDisplay.Controls.Add(userForm);
            userForm.BringToFront();
            userForm.Show();
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

        private void Menu_Load(object sender, EventArgs e)
        {
            btnATM.PerformClick();
        }

        private void btnTrans_Click(object sender, EventArgs e)
        {
            previousButton = (Button)sender;
            activationbtn(sender, Color.DeepSkyBlue);
            transactionForm.TopLevel = false;
            pnlDisplay.Controls.Add(transactionForm);
            transactionForm.BringToFront();
            transactionForm.Show();
        }

        private void btnAuthenticate_Click(object sender, EventArgs e)
        {
            activationbtn(sender, Color.DeepSkyBlue);
            try
            {
                if (authenticateForm == null || authenticateForm.IsDisposed || !authenticateForm.Visible)
                {
                    authenticateForm = new Authentication();
                    authenticateForm.FormClosed += (s, args) => authenticateForm = null;
                    authenticateForm.Show();
                }
                else
                {
                    authenticateForm.BringToFront();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnATM_Click(object sender, EventArgs e)
        {
            previousButton = (Button)sender;
            activationbtn(sender, Color.DeepSkyBlue);
            atmForm.TopLevel = false;
            pnlDisplay.Controls.Add(atmForm);
            atmForm.BringToFront();
            atmForm.Show();
        }

        private void btnCard_Click(object sender, EventArgs e)
        {
            previousButton = (Button)sender;
            activationbtn(sender, Color.DeepSkyBlue);
            cardForm.TopLevel = false;
            pnlDisplay.Controls.Add(cardForm);
            cardForm.BringToFront();
            cardForm.Show();
        }

        private void btnAcc_Click(object sender, EventArgs e)
        {
            previousButton = (Button)sender;
            activationbtn(sender, Color.DeepSkyBlue);
            accountForm.TopLevel = false;
            pnlDisplay.Controls.Add(accountForm);
            accountForm.BringToFront();
            accountForm.Show();
        }
    }
}



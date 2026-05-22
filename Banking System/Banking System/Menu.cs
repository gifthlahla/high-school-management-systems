using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Banking_System
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public partial class Menu : Form
    {
        private Button crntbtn;
        private Panel lftbrderbtn;

        // Declare instances of the forms
        private Bank bankForm;
        private Account accountForm;
        private Transaction transactionForm;
        private Customers customersForm;
        private Authentication authenticateForm;

        public Menu()
        {
            // Initialize components
            InitializeComponent();

            // Create instances of the forms
            bankForm = new Bank();
            accountForm = new Account();
            transactionForm = new Transaction();
            customersForm = new Customers();
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
                crntbtn.ForeColor = Color.FromArgb(30, 34, 46);
                lftbrderbtn.BackColor = Color.FromArgb(30, 34, 46);
                lftbrderbtn.Location = new Point(0, crntbtn.Location.Y);
                lftbrderbtn.BringToFront();
            }
        }
        private void btnUser_Click(object sender, EventArgs e)
        {
            previousButton = (Button)sender;
            activationbtn(sender, Color.DeepSkyBlue);
            customersForm.TopLevel = false;
            pnlDisplay.Controls.Add(customersForm);
            customersForm.BringToFront();
            customersForm.Show();
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
            btnBank.PerformClick();
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
            bankForm.TopLevel = false;
            pnlDisplay.Controls.Add(bankForm);
            bankForm.BringToFront();
            bankForm.Show();
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



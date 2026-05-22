using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fees_Payment_System
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public partial class Menu : Form
    {
        private Button crntbtn;
        private Panel lftbrderbtn;

        // Declare instances of the forms
        private Student studentForm;
        private Fee feeForm;
        private Payment paymentForm;
        private Parent parentForm;
        private Account accForm;

        public Menu()
        {
            // Initialize components
            InitializeComponent();

            // Create instances of the forms
            studentForm = new Student();
            feeForm = new Fee();
            paymentForm = new Payment();
            parentForm = new Parent();
            accForm = new Account();

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
                crntbtn.ForeColor = Color.FromArgb(29, 112, 159);
                lftbrderbtn.BackColor = Color.FromArgb(29, 112, 159);
                lftbrderbtn.Location = new Point(0, crntbtn.Location.Y);
                lftbrderbtn.BringToFront();
            }
        }

        private void btnStudent_Click(object sender, EventArgs e)
        {
            previousButton = (Button)sender;
            activationbtn(sender, Color.DeepSkyBlue);
            studentForm.TopLevel = false;
            pnlDisplay.Controls.Add(studentForm);
            studentForm.BringToFront();
            studentForm.Show();
        }

        private void btnFee_Click(object sender, EventArgs e)
        {
            previousButton = (Button)sender;
            activationbtn(sender, Color.DeepSkyBlue);
            feeForm.TopLevel = false;
            pnlDisplay.Controls.Add(feeForm);
            feeForm.BringToFront();
            feeForm.Show();
        }

        private void btnAccounts_Click(object sender, EventArgs e)
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

        private void btnPayment_Click(object sender, EventArgs e)
        {
            previousButton = (Button)sender;
            activationbtn(sender, Color.DeepSkyBlue);
            paymentForm.TopLevel = false;
            pnlDisplay.Controls.Add(paymentForm);
            paymentForm.BringToFront();
            paymentForm.Show();
        }

        private void btnParent_Click(object sender, EventArgs e)
        {
            previousButton = (Button)sender;
            activationbtn(sender, Color.DeepSkyBlue);
            parentForm.TopLevel = false;
            pnlDisplay.Controls.Add(parentForm);
            parentForm.BringToFront();
            parentForm.Show();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Menu_Load(object sender, EventArgs e)
        {
            btnStudent.PerformClick();
        }
    }
}



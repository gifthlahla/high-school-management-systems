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
    public partial class Welcome : Form
    {
        public Welcome()
        {
            InitializeComponent();
        }

        private void btnAdmin_Click(object sender, EventArgs e)
        {
            AdminLGN adminLogin = new AdminLGN();
            adminLogin.Show();
            this.Hide();
        }

        private void btnVote_Click(object sender, EventArgs e)
        {
            VoterLGN voterLogin = new VoterLGN();
            voterLogin.Show();
            this.Hide();
        }
    }
}

using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Voting_System
{
    public partial class VoterLGN : Form
    {
        OleDbConnection conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Vote.accdb");
        public VoterLGN()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string studentID = txtUsername.Text.Trim();
            string nationalID = txtPassword.Text.Trim();

            OleDbCommand cmd = new OleDbCommand("SELECT * FROM Students WHERE StudentID = @id AND NationalID = @nid", conn);
            cmd.Parameters.AddWithValue("@id", studentID);
            cmd.Parameters.AddWithValue("@nid", nationalID);

            conn.Open();
            OleDbDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                // Save StudentID temporarily (e.g., static variable or public property)
                Session.CurrentStudentID = studentID;
                Session.CurrentStudentName = dr["FullName"].ToString();
                Session.CurrentGrade = dr["Grade"].ToString();

                this.Hide();
                new Vote().Show();
            }
            else
            {
                MessageBox.Show("Invalid credentials.");
            }

        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            Welcome welcome = new Welcome();
            welcome.Show();
            this.Hide();
        }
    }
}

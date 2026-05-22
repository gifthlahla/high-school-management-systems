using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Voting_System
{
    public partial class Voters : Form
    {
        OleDbConnection con = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Vote.accdb");

        public Voters()
        {
            InitializeComponent();
            LoadVotedStudents();
        }

        private void LoadVotedStudents()
        {
            try
            {
                con.Open();
                var da = new OleDbDataAdapter(
                    "SELECT StudentID, Name, Grade, Status FROM Votes WHERE Status = 'Voted'", con);
                var dt = new DataTable();
                da.Fill(dt);
                dgvVoters.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load voters: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

    }
}
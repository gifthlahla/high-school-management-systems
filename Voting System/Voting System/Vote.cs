using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Voting_System
{
    public partial class Vote : Form
    {
        OleDbConnection conn;

        public Vote()
        {
            InitializeComponent();
            conn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Vote.accdb");
        }

        private void Vote_Load(object sender, EventArgs e)
        {
            LoadCandidates("President", cmbPresident);
            LoadCandidates("Vice President", cmbVicePresident);
            LoadCandidates("Secretary", cmbSecretary);
            LoadCandidates("Treasurer", cmbTreasurer);
            LoadCandidates("JHS Governor", cmbJHSGovernor);
            LoadCandidates("SHS Governor", cmbSHSGovernor);
            LoadCandidates("JHS Vice Governor", cmbJHSViceGovernor);
            LoadCandidates("SHS Vice Governor", cmbSHSViceGovernor);
        }

        private void LoadCandidates(string position, ComboBox combo)
        {
            try
            {
                conn.Open();
                OleDbCommand cmd = new OleDbCommand("SELECT StudentName FROM Candidates WHERE [Position] = @position", conn);
                cmd.Parameters.AddWithValue("@position", position);
                OleDbDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    combo.Items.Add(dr["StudentName"].ToString());
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading candidates: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void btnVote_Click(object sender, EventArgs e)
        {
            List<Tuple<string, string>> votes = new List<Tuple<string, string>>();

            if (!string.IsNullOrWhiteSpace(cmbPresident.Text))
                votes.Add(new Tuple<string, string>("President", cmbPresident.Text));

            if (!string.IsNullOrWhiteSpace(cmbVicePresident.Text))
                votes.Add(new Tuple<string, string>("Vice President", cmbVicePresident.Text));

            if (!string.IsNullOrWhiteSpace(cmbSecretary.Text))
                votes.Add(new Tuple<string, string>("Secretary", cmbSecretary.Text));

            if (!string.IsNullOrWhiteSpace(cmbTreasurer.Text))
                votes.Add(new Tuple<string, string>("Treasurer", cmbTreasurer.Text));

            if (!string.IsNullOrWhiteSpace(cmbJHSGovernor.Text))
                votes.Add(new Tuple<string, string>("JHS Governor", cmbJHSGovernor.Text));

            if (!string.IsNullOrWhiteSpace(cmbSHSGovernor.Text))
                votes.Add(new Tuple<string, string>("SHS Governor", cmbSHSGovernor.Text));

            if (!string.IsNullOrWhiteSpace(cmbJHSViceGovernor.Text))
                votes.Add(new Tuple<string, string>("JHS Vice Governor", cmbJHSViceGovernor.Text));

            if (!string.IsNullOrWhiteSpace(cmbSHSViceGovernor.Text))
                votes.Add(new Tuple<string, string>("SHS Vice Governor", cmbSHSViceGovernor.Text));

            if (votes.Count == 0)
            {
                MessageBox.Show("Please vote for at least one position.");
                return;
            }

            try
            {
                conn.Open();

                // Check if the student has already voted
                OleDbCommand checkCmd = new OleDbCommand("SELECT * FROM Votes WHERE StudentID = @id", conn);
                checkCmd.Parameters.AddWithValue("@id", Session.CurrentStudentID);
                OleDbDataReader reader = checkCmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Close();
                    MessageBox.Show("You have already voted!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.Hide();
                    new Welcome().Show(); // back to login
                    return;
                }
                reader.Close();

                // Insert into Votes table
                OleDbCommand voteCmd = new OleDbCommand("INSERT INTO Votes (StudentID, Name, Grade, Status) VALUES (@id, @name, @grade, 'Voted')", conn);
                voteCmd.Parameters.AddWithValue("@id", Session.CurrentStudentID);
                voteCmd.Parameters.AddWithValue("@name", Session.CurrentStudentName);
                voteCmd.Parameters.AddWithValue("@grade", Session.CurrentGrade);
                voteCmd.ExecuteNonQuery();

                // Update vote count for each selected candidate
                foreach (var vote in votes)
                {
                    OleDbCommand updateCmd = new OleDbCommand("UPDATE VotesRate SET VotesCount = VotesCount + 1 WHERE [Position] = @position AND Fullname = @fullname", conn);
                    updateCmd.Parameters.AddWithValue("@position", vote.Item1);
                    updateCmd.Parameters.AddWithValue("@fullname", vote.Item2);
                    updateCmd.ExecuteNonQuery();
                }

                MessageBox.Show("Your vote has been successfully cast!", "Success");
                this.Hide();
                new Welcome().Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error casting vote: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            cmbPresident.SelectedIndex = -1;
            cmbVicePresident.SelectedIndex = -1;
            cmbSecretary.SelectedIndex = -1;
            cmbTreasurer.SelectedIndex = -1;
            cmbJHSGovernor.SelectedIndex = -1;
            cmbSHSGovernor.SelectedIndex = -1;
            cmbJHSViceGovernor.SelectedIndex = -1;
            cmbSHSViceGovernor.SelectedIndex = -1;
        }
    }
}

using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;
using System.Collections.Generic;



namespace Voting_System
{
    public partial class FnlResults : Form
    {
        public FnlResults()
        {
            InitializeComponent();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void FnlResults_Load(object sender, EventArgs e)
        {
            // Update this to your actual Access or Excel database file path
            string connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\\Vote.accdb";

            // Map of position names to corresponding label controls on your form
            Dictionary<string, Label> positionLabels = new Dictionary<string, Label>()
            {
            { "President", lblPresident },
            { "Vice President", lblVicePresident },
            { "Secretary", lblSecretary },
            { "Treasurer", lblTreasurer },
            { "SHS Governor", lblSHSGovernor },
            { "JHS Governor", lblJHSGovernor },
            { "SHS Vice Governor", SHSViceGovernor },
            { "JHS Vice Governor", lblJHSViceGovernor }
            };
            
            using (OleDbConnection conn = new OleDbConnection(connString))
            {
                conn.Open();

                foreach (KeyValuePair<string, Label> entry in positionLabels)
                {
                    string position = entry.Key;
                    Label label = entry.Value;

                    string query = "SELECT TOP 1 CandidateName, VoteCount FROM VotesRate WHERE Position = ? ORDER BY VoteCount DESC";

                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        // OleDb uses positional parameters (?)
                        cmd.Parameters.AddWithValue("?", position);

                        using (OleDbDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string candidateName = reader["CandidateName"].ToString();
                                int voteCount = Convert.ToInt32(reader["VoteCount"]);
                                label.Text = candidateName + " - " + voteCount.ToString() + " votes";
                            }
                            else
                            {
                                label.Text = "No data";
                            }
                        }
                    }
                }

                conn.Close();
            }
        }
    }
}

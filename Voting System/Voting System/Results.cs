using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Voting_System
{
    public partial class Results : Form
    {
        // ← point this at your .accdb
        private string connectionString =
            @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Vote.accdb";

        public Results()
        {
            InitializeComponent();
        }

        // your original: opens the final-results form
        private void btnViewResults_Click(object sender, EventArgs e)
        {
            FnlResults finalResults = new FnlResults();
            finalResults.Show();
        }

        // whenever Position changes...
        private void cmbPosition_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadVotes();
        }

        // whenever Party changes...
        private void cmbParty_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadVotes();
        }

        // clear filters and reload
        private void btnResfresh_Click(object sender, EventArgs e)
        {
            cmbPosition.SelectedIndex = -1;
            cmbParty.SelectedIndex = -1;
            LoadData();
        }

        private void LoadData()
        {
            string sql = "SELECT [Position], [Party], Fullname, VotesCount " +
                         "FROM VotesRate ";
                        
            using (var cn = new OleDbConnection(connectionString))
            using (var cmd = new OleDbCommand(sql, cn))
            {
                var dt = new DataTable();
                try
                {
                    cn.Open();
                    new OleDbDataAdapter(cmd).Fill(dt);
                    dgvResults.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        "Error loading votes:\n" + ex.Message,
                        "Database Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        // core filtering logic
        private void LoadVotes()
        {
            string pos = cmbPosition.Text.Trim();
            string party = cmbParty.Text.Trim();

            string sql = "SELECT [Position], [Party], Fullname, VotesCount " +
                         "FROM VotesRate WHERE 1=1";

            // add WHERE clauses as needed
            if (!string.IsNullOrEmpty(pos))
                sql += " AND [Position] = ?";
            if (!string.IsNullOrEmpty(party))
                sql += " AND [Party] = ?";

            sql += " ORDER BY VotesCount DESC";

            using (var cn = new OleDbConnection(connectionString))
            using (var cmd = new OleDbCommand(sql, cn))
            {
                // parameters must match order of '?'
                if (!string.IsNullOrEmpty(pos))
                    cmd.Parameters.AddWithValue("?", pos);
                if (!string.IsNullOrEmpty(party))
                    cmd.Parameters.AddWithValue("?", party);

                var dt = new DataTable();
                try
                {
                    cn.Open();
                    new OleDbDataAdapter(cmd).Fill(dt);
                    dgvResults.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        "Error loading votes:\n" + ex.Message,
                        "Database Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
                finally { cn.Close(); }
            }
        }

        private void Results_Load(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}

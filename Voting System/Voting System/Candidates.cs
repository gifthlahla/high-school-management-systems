using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Voting_System
{
    public partial class Candidates : Form
    {
        private OleDbConnection con = new OleDbConnection(
            "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Vote.accdb");

        public Candidates()
        {
            InitializeComponent();
            LoadCandidates();
        }

        // Populate DataGridView
        private void LoadCandidates()
        {
            try
            {
                con.Open();
                var da = new OleDbDataAdapter(
                    "SELECT * " +
                    "FROM [Candidates]", con);
                var dt = new DataTable();
                da.Fill(dt);
                dgvCandidates.DataSource = dt;
            }
            finally { con.Close(); }
        }

        // As-you-type lookup of student
        private void txtFndStdnt_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFndStdnt.Text))
            {
                lblStudentID.Clear();
                lblStudentName.Clear();
                lblGrade.Clear();
                return;
            }

            try
            {
                con.Open();
                var cmd = new OleDbCommand(
                    "SELECT FullName, [Grade] FROM Students WHERE StudentID = ?", con);
                cmd.Parameters.AddWithValue("?", txtFndStdnt.Text.Trim());
                var rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    lblStudentID.Text = txtFndStdnt.Text.Trim();
                    lblStudentName.Text = rdr["FullName"].ToString();
                    lblGrade.Text = rdr["Grade"].ToString();
                }
                else
                {
                    lblStudentID.Clear();
                    lblStudentName.Clear();
                    lblGrade.Clear();
                }
            }
            finally { con.Close(); }
        }

        // Save new candidate
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (lblStudentID.Text == "")
            {
                MessageBox.Show("Please select a valid student first.");
                return;
            }

            try
            {
                con.Open();

                // Insert into Candidates table
                var cmd = new OleDbCommand(
                    "INSERT INTO [Candidates] " +
                    "(StudentID, StudentName, [Grade], [Position], Party) " +
                    "VALUES (?, ?, ?, ?, ?)", con);

                cmd.Parameters.AddWithValue("?", Convert.ToInt32(lblStudentID.Text));
                cmd.Parameters.AddWithValue("?", lblStudentName.Text);
                cmd.Parameters.AddWithValue("?", lblGrade.Text);
                cmd.Parameters.AddWithValue("?", cmbPosition.Text);
                cmd.Parameters.AddWithValue("?", cmbParty.Text);
                cmd.ExecuteNonQuery();

                // Insert into VoteRate table
                var cmdVoteRate = new OleDbCommand(
                    "INSERT INTO VoteRate ([Position], [Party], Fullname, VotesCount) " +
                    "VALUES (?, ?, ?, ?)", con);

                cmdVoteRate.Parameters.AddWithValue("?", cmbPosition.Text);
                cmdVoteRate.Parameters.AddWithValue("?", cmbParty.Text);
                cmdVoteRate.Parameters.AddWithValue("?", lblStudentName.Text);
                cmdVoteRate.Parameters.AddWithValue("?", 0);
                cmdVoteRate.ExecuteNonQuery();

                MessageBox.Show("Candidate registered.");
                LoadCandidates();
                ClearForm();
            }
            finally { con.Close(); }
        }

        // Edit existing candidate
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (lblCandidateID.Text == "") return;

            try
            {
                con.Open();

                // Update Candidates table
                var cmd = new OleDbCommand(
                    "UPDATE Candidates SET " +
                    "StudentID=?, StudentName=?, [Grade]=?, [Position]=?, [Party]=? " +
                    "WHERE CandidateID=?", con);

                cmd.Parameters.AddWithValue("?", Convert.ToInt32(lblStudentID.Text));
                cmd.Parameters.AddWithValue("?", lblStudentName.Text);
                cmd.Parameters.AddWithValue("?", lblGrade.Text);
                cmd.Parameters.AddWithValue("?", cmbPosition.Text);
                cmd.Parameters.AddWithValue("?", cmbParty.Text);
                cmd.Parameters.AddWithValue("?", Convert.ToInt32(lblCandidateID.Text));
                cmd.ExecuteNonQuery();

                // Update VoteRate table
                var cmdVoteRate = new OleDbCommand(
                    "UPDATE VotesRate SET [Party]=?, Fullname=? " +
                    "WHERE [Position]=? AND Fullname=?", con);

                cmdVoteRate.Parameters.AddWithValue("?", cmbParty.Text);
                cmdVoteRate.Parameters.AddWithValue("?", lblStudentName.Text);
                cmdVoteRate.Parameters.AddWithValue("?", cmbPosition.Text);
                cmdVoteRate.Parameters.AddWithValue("?", lblStudentName.Text);
                cmdVoteRate.ExecuteNonQuery();

                MessageBox.Show("Candidate updated.");
                LoadCandidates();
                ClearForm();
            }
            finally { con.Close(); }
        }

        // Delete candidate
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lblCandidateID.Text == "") return;

            try
            {
                con.Open();

                // Delete from Candidates table
                var cmd = new OleDbCommand(
                    "DELETE FROM [Candidates] WHERE CandidateID=?", con);
                cmd.Parameters.AddWithValue("?", Convert.ToInt32(lblCandidateID.Text));
                cmd.ExecuteNonQuery();

                // Delete from VoteRate table
                var cmdVoteRate = new OleDbCommand(
                    "DELETE FROM VotesRate WHERE [Position]=? AND Fullname=?", con);
                cmdVoteRate.Parameters.AddWithValue("?", cmbPosition.Text);
                cmdVoteRate.Parameters.AddWithValue("?", lblStudentName.Text);
                cmdVoteRate.ExecuteNonQuery();

                MessageBox.Show("Candidate removed.");
                LoadCandidates();
                ClearForm();
            }
            finally { con.Close(); }
        }

        // When user clicks a row in the grid
        private void dgvCandidates_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dgvCandidates.Rows[e.RowIndex];

            lblCandidateID.Text = row.Cells["CandidateID"].Value.ToString();
            lblStudentID.Text = row.Cells["StudentID"].Value.ToString();
            lblStudentName.Text = row.Cells["StudentName"].Value.ToString();
            lblGrade.Text = row.Cells["Grade"].Value.ToString();
            cmbPosition.Text = row.Cells["Position"].Value.ToString();
            cmbParty.Text = row.Cells["Party"].Value.ToString();
        }

        // Clear all fields
        private void ClearForm()
        {
            lblCandidateID.Clear();
            lblStudentID.Clear();
            lblStudentName.Clear();
            lblGrade.Clear();
            cmbPosition.SelectedIndex = -1;
            cmbParty.SelectedIndex = -1;
            txtFndStdnt.Clear();
        }
    }
}

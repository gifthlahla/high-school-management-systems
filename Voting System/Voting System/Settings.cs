using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Printing;


namespace Voting_System
{
    public partial class Settings : Form
    {
        // ← point this at your .accdb
        private string connectionString =
            @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Vote.accdb";

        public Settings()
        {
            InitializeComponent();
        }

        private void btnVerify_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();
            string query = "SELECT COUNT(*) FROM [User] WHERE Username = ? AND Password = ?";

            using (OleDbConnection con = new OleDbConnection(connectionString))
            using (OleDbCommand cmd = new OleDbCommand(query, con))
            {
                cmd.Parameters.AddWithValue("?", username);
                cmd.Parameters.AddWithValue("?", password);

                con.Open();
                int count = (int)cmd.ExecuteScalar();

                if (count == 1)
                {
                    pnlB.Visible = true;
                    pnlB.BringToFront();
                    pnlA.Visible = false;
                    txtUsername.Clear();
                    txtPassword.Clear();
                }
                else
                {
                    MessageBox.Show("Invalid username or password.");
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string newUsername = txtNewUsername.Text.Trim();
            string newPassword = txtNewPassword.Text.Trim();
            string confirmPassword = txtConfirmPassword.Text.Trim();
            string oldUsername = txtUsername.Text.Trim();

            if (newPassword != confirmPassword)
            {
                MessageBox.Show("Passwords do not match.");
                return;
            }

            string updateQuery = "UPDATE [User] SET Username = ?, [Password] = ? WHERE Username = ?";

            using (OleDbConnection con = new OleDbConnection(connectionString))
            using (OleDbCommand cmd = new OleDbCommand(updateQuery, con))
            {
                cmd.Parameters.AddWithValue("?", newUsername);
                cmd.Parameters.AddWithValue("?", newPassword);
                cmd.Parameters.AddWithValue("?", oldUsername);

                con.Open();
                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Credentials updated successfully.");
                    pnlA.Visible = true;
                    pnlA.BringToFront();
                    pnlB.Visible = false;
                    txtNewUsername.Clear();
                    txtNewPassword.Clear();
                    txtConfirmPassword.Clear();
                }
                else
                {
                    MessageBox.Show("Update failed. Please try again.");
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            txtNewUsername.Clear();
            txtNewPassword.Clear();
            txtConfirmPassword.Clear();
            pnlA.Visible = true;
            pnlA.BringToFront();
            pnlB.Visible = false;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            // Optional confirmation before clearing
            DialogResult result = MessageBox.Show("Do you want to clear the results after printing?", "Confirm", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                ClearAllTables();
                MessageBox.Show("All results cleared.");
            }
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            dgvResults.Visible = false;
            dgvResults.AllowUserToAddRows = false;
            dgvResults.AllowUserToDeleteRows = false;
        }

        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            int x = 50;
            int y = 50;
            int rowHeight = 25;
            
            Font font = new Font("Arial", 10);
            Brush brush = Brushes.Black;
            
            // Headers
            for (int i = 0; i < dgvResults.Columns.Count; i++)
            {
                e.Graphics.DrawString(dgvResults.Columns[i].HeaderText, font, brush, x, y);
                x += 120;
            }
            
            y += rowHeight;
            x = 50;
            
            // Rows
            foreach (DataGridViewRow row in dgvResults.Rows)
            {
                if (row.IsNewRow) continue;
                
                for (int i = 0; i < dgvResults.Columns.Count; i++)
                {
                    string value = row.Cells[i].Value.ToString();
                    e.Graphics.DrawString(value, font, brush, x, y);
                    x += 120;
                }
                
                y += rowHeight;
                x = 50;
            }
        }

        private void btnBackUp_Click(object sender, EventArgs e)
        {
            PrintDocument printDocument = new PrintDocument();
            PrintPreviewDialog printPreviewDialog = new PrintPreviewDialog();
            printDocument.PrintPage += new PrintPageEventHandler(PrintDocument_PrintPage);

            string query = "SELECT * FROM VotesRate ORDER BY Position, VotesCount DESC";
            DataTable dt = new DataTable();

            using (OleDbConnection con = new OleDbConnection(connectionString))
            using (OleDbDataAdapter da = new OleDbDataAdapter(query, con))
            {
                da.Fill(dt);
            }

            dgvResults.DataSource = dt;

            // Show print preview
            printPreviewDialog.Document =  printDocument;
            printPreviewDialog.ShowDialog();

            // Optional confirmation before clearing
            DialogResult result = MessageBox.Show("Do you want to clear the results after printing?", "Confirm", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                ClearAllTables();
                MessageBox.Show("All results cleared.");
            }
        }
        private void ClearAllTables()
        {
            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                con.Open();
                string[] tables = { "Candidates", "Votes", "VotesRate" };
                foreach (string table in tables)
                {
                    using (OleDbCommand cmd = new OleDbCommand("DELETE FROM " + table, con))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}

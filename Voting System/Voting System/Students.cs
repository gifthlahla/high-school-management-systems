using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Voting_System
{
    public partial class Students : Form
    {
        OleDbConnection con = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Vote.accdb");

        public Students()
        {
            InitializeComponent();
            LoadStudents();
        }

        private void LoadStudents()
        {
            try
            {
                con.Open();
                OleDbDataAdapter da = new OleDbDataAdapter("SELECT * FROM Students", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvStudents.DataSource = dt;
            }
            finally
            {
                con.Close();
            }
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                OleDbCommand cmd = new OleDbCommand("INSERT INTO Students (FullName, Grade, NationalID, ContactNo, Email) VALUES (?, ?, ?, ?, ?)", con);
                cmd.Parameters.AddWithValue("?", txtName.Text);
                cmd.Parameters.AddWithValue("?", cmbGrade.Text);
                cmd.Parameters.AddWithValue("?", txtNationalID.Text);
                cmd.Parameters.AddWithValue("?", txtContact.Text);
                cmd.Parameters.AddWithValue("?", txtEmail.Text);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Student saved successfully!");
            }
            finally
            {
                con.Close();
                LoadStudents();
                ClearForm();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (lblStudentID.Text == "") return;

            try
            {
                con.Open();
                OleDbCommand cmd = new OleDbCommand("UPDATE Students SET FullName=?, Grade=?, NationalID=?, ContactNo=?, Email=? WHERE StudentID=?", con);
                cmd.Parameters.AddWithValue("?", txtName.Text);
                cmd.Parameters.AddWithValue("?", cmbGrade.Text);
                cmd.Parameters.AddWithValue("?", txtNationalID.Text);
                cmd.Parameters.AddWithValue("?", txtContact.Text);
                cmd.Parameters.AddWithValue("?", txtEmail.Text);
                cmd.Parameters.AddWithValue("?", Convert.ToInt32(lblStudentID.Text));
                cmd.ExecuteNonQuery();

                MessageBox.Show("Student updated successfully!");
            }
            finally
            {
                con.Close();
                LoadStudents();
                ClearForm();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lblStudentID.Text == "") return;

            try
            {
                con.Open();
                OleDbCommand cmd = new OleDbCommand("DELETE FROM Students WHERE StudentID=?", con);
                cmd.Parameters.AddWithValue("?", Convert.ToInt32(lblStudentID.Text));
                cmd.ExecuteNonQuery();

                MessageBox.Show("Student deleted successfully!");
            }
            finally
            {
                con.Close();
                LoadStudents();
                ClearForm();
            }
        }

        private void dgvStudents_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvStudents.Rows[e.RowIndex];
                lblStudentID.Text = row.Cells["StudentID"].Value.ToString();
                txtName.Text = row.Cells["FullName"].Value.ToString();
                cmbGrade.Text = row.Cells["Grade"].Value.ToString();
                txtNationalID.Text = row.Cells["NationalID"].Value.ToString();
                txtContact.Text = row.Cells["ContactNo"].Value.ToString();
                txtEmail.Text = row.Cells["Email"].Value.ToString();
            }
        }

        private void ClearForm()
        {
            lblStudentID.Text = "";
            txtName.Clear();
            cmbGrade.SelectedIndex = -1;
            txtNationalID.Clear();
            txtContact.Clear();
            txtEmail.Clear();
        }

        private void txtFndStdnt_TextChanged(object sender, EventArgs e)
        {
            string searchText = txtFndStdnt.Text.Trim();

            if (string.IsNullOrEmpty(searchText))
            {
                dgvStudents.DataSource = null;
                return;
            }

            try
            {
                con.Open();
                var cmd = new OleDbCommand(
                    "SELECT StudentID, FullName, Grade, NationalID, ContactNo, Email " +
                    "FROM Students WHERE StudentID LIKE ?", con);

                // Use wildcards for partial matching (if needed)
                cmd.Parameters.AddWithValue("?", "%" + searchText + "%");

                var adapter = new OleDbDataAdapter(cmd);
                var dt = new DataTable();
                adapter.Fill(dt);

                dgvStudents.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void Students_Load(object sender, EventArgs e)
        {

        }

    }
}

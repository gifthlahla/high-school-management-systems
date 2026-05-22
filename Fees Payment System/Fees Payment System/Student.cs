using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Fees_Payment_System
{
    public partial class Student : Form
    {
        private OleDbConnection con;
        private OleDbDataAdapter da;
        private DataTable dt;
        private BindingSource bs;

        public Student()
        {
            InitializeComponent();
        }

        private void Student_Load(object sender, EventArgs e)
        {
            // 1) Initialize DB connection
            con = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=FeesPayment.accdb");

            // 2) Load the grid with existing student data
            LoadStudents();
        }

        private void LoadStudents()
        {
            try
            {
                con.Open();
                string query = "SELECT * FROM Students";
                OleDbDataAdapter adapter = new OleDbDataAdapter(query, con);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dgvStudents.DataSource = dt;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading Students: " + ex.Message);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Basic validation
                if (txtName.Text == "") throw new Exception("Name required");

                string sql = @"
                    INSERT INTO Students
                      (StudentName, Phone, Email, Form)
                    VALUES
                      (@Name, @Phone, @Email, @Form)";

                using (var cmd = new OleDbCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@Name", txtName.Text);
                    cmd.Parameters.AddWithValue("@Phone", txtPhone.Text);
                    cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                    cmd.Parameters.AddWithValue("@Form", cmbForm.Text);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }

                MessageBox.Show("Student saved.");
                LoadStudents();
                btnClear_Click(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving: " + ex.Message);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvStudents.SelectedRows.Count == 0)
                    throw new Exception("Select a row to edit");

                var row = dgvStudents.SelectedRows[0];
                var id = row.Cells["StudentID"].Value;

                string sql = @"
                    UPDATE Students SET
                      StudentName = @Name,
                      Phone = @Phone,
                      Email = @Email,
                      Form = @Form
                    WHERE StudentID = @ID";

                using (var cmd = new OleDbCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@Name", txtName.Text);
                    cmd.Parameters.AddWithValue("@Phone", txtPhone.Text);
                    cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                    cmd.Parameters.AddWithValue("@Form", cmbForm.Text);
                    cmd.Parameters.AddWithValue("@ID", id);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }

                MessageBox.Show("Student updated.");
                LoadStudents();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating: " + ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvStudents.SelectedRows.Count == 0)
                    throw new Exception("Select a row to delete");

                var id = dgvStudents.SelectedRows[0].Cells["StudentID"].Value;
                string sql = "DELETE FROM Students WHERE StudentID = @ID";

                using (var cmd = new OleDbCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@ID", id);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }

                MessageBox.Show("Student deleted.");
                LoadStudents();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting: " + ex.Message);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtName.Clear();
            txtPhone.Clear();
            txtEmail.Clear();
            cmbForm.SelectedIndex = 0;
        }

        private void dgvStudents_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // When user clicks a row, fill the form
            if (e.RowIndex < 0) return;
            var r = dgvStudents.Rows[e.RowIndex];
            txtName.Text = r.Cells["StudentName"].Value.ToString();
            txtPhone.Text = r.Cells["Phone"].Value.ToString();
            txtEmail.Text = r.Cells["Email"].Value.ToString();
            cmbForm.Text = r.Cells["Form"].Value.ToString();
        }
    }
}

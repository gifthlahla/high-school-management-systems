using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Student_Information_System
{
    public partial class Teacher : Form
    {
        OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=StudentInfo.accdb");

        public Teacher()
        {
            InitializeComponent();
            DisplayTeachers();
        }

        private void DisplayTeachers()
        {
            try
            {
                conn.Open();
                OleDbDataAdapter da = new OleDbDataAdapter("SELECT * FROM Teachers", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvTeachers.DataSource = dt;
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                conn.Close();
            }
        }

        private void ClearInputs()
        {
            txtName.Clear();
            txtPhone.Clear();
            txtEmail.Clear();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtName.Text == "" || txtPhone.Text == "" || txtEmail.Text == "")
            {
                MessageBox.Show("Please fill all fields.");
                return;
            }

            try
            {
                conn.Open();
                string query = "INSERT INTO Teachers (Name, Phone, Email) VALUES (?, ?, ?)";
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("?", txtName.Text);
                cmd.Parameters.AddWithValue("?", txtPhone.Text);
                cmd.Parameters.AddWithValue("?", txtEmail.Text);
                cmd.ExecuteNonQuery();
                conn.Close();
                MessageBox.Show("Teacher Added!");
                DisplayTeachers();
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                conn.Close();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvTeachers.CurrentRow == null || dgvTeachers.CurrentRow.Cells["ID"].Value == null)
            {
                MessageBox.Show("Please select a teacher to edit.");
                return;
            }

            int id = Convert.ToInt32(dgvTeachers.CurrentRow.Cells["ID"].Value);

            try
            {
                conn.Open();
                string query = "UPDATE Teachers SET Name=?, Phone=?, Email=? WHERE ID=?";
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("?", txtName.Text);
                cmd.Parameters.AddWithValue("?", txtPhone.Text);
                cmd.Parameters.AddWithValue("?", txtEmail.Text);
                cmd.Parameters.AddWithValue("?", id);
                cmd.ExecuteNonQuery();
                conn.Close();
                MessageBox.Show("Teacher Updated!");
                DisplayTeachers();
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                conn.Close();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvTeachers.CurrentRow == null || dgvTeachers.CurrentRow.Cells["ID"].Value == null)
            {
                MessageBox.Show("Please select a teacher to delete.");
                return;
            }

            int id = Convert.ToInt32(dgvTeachers.CurrentRow.Cells["ID"].Value);

            try
            {
                conn.Open();
                string query = "DELETE FROM Teachers WHERE ID=?";
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("?", id);
                cmd.ExecuteNonQuery();
                conn.Close();
                MessageBox.Show("Teacher Deleted!");
                DisplayTeachers();
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                conn.Close();
            }
        }

        private void dgvTeachers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtName.Text = dgvTeachers.Rows[e.RowIndex].Cells["Name"].Value.ToString();
                txtPhone.Text = dgvTeachers.Rows[e.RowIndex].Cells["Phone"].Value.ToString();
                txtEmail.Text = dgvTeachers.Rows[e.RowIndex].Cells["Email"].Value.ToString();
            }
        }
    }
}

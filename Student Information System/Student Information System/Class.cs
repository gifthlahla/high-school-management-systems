using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Student_Information_System
{
    public partial class Class : Form
    {
        // Connection string for the Access database
        private string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\StudentInfo.accdb";

        public Class()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtName.Text) || string.IsNullOrEmpty(txtGender.Text) || cmbTeacher.SelectedItem == null)
            {
                MessageBox.Show("Please fill all the fields.");
                return;
            }

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO Classes (ClassID, ClassName, Grade, Teacher) VALUES (?, ?, ?, ?)";
                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("?", txtID.Text);
                    cmd.Parameters.AddWithValue("?", txtName.Text);
                    cmd.Parameters.AddWithValue("?", txtGender.Text);
                    cmd.Parameters.AddWithValue("?", cmbTeacher.SelectedItem.ToString());

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Class record saved successfully!");
                    LoadClassData();
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtID.Text) || string.IsNullOrEmpty(txtName.Text) || string.IsNullOrEmpty(txtGender.Text) || cmbTeacher.SelectedItem == null)
            {
                MessageBox.Show("Please fill all the fields.");
                return;
            }

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                string query = "UPDATE Classes SET ClassName = ?, Grade = ?, Teacher = ? WHERE ClassID = ?";
                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("?", txtName.Text);
                    cmd.Parameters.AddWithValue("?", txtGender.Text);
                    cmd.Parameters.AddWithValue("?", cmbTeacher.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("?", txtID.Text);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Class record updated successfully!");
                    LoadClassData();
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtID.Text))
            {
                MessageBox.Show("Please provide the Class ID to delete.");
                return;
            }

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                string query = "DELETE FROM Classes WHERE ClassID = ?";
                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("?", txtID.Text);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Class record deleted successfully!");
                    LoadClassData();
                }
            }
        }

        private void LoadClassData()
        {
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Classes";
                using (OleDbDataAdapter da = new OleDbDataAdapter(query, conn))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvClass.DataSource = dt;
                }
            }
        }

        private void dgvClass_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvClass.Rows[e.RowIndex];
                txtID.Text = row.Cells["ClassID"].Value.ToString();
                txtName.Text = row.Cells["ClassName"].Value.ToString();
                txtGender.Text = row.Cells["Grade"].Value.ToString();
                cmbTeacher.SelectedItem = row.Cells["Teacher"].Value.ToString();
            }
        }

        private void LoadTeachers()
        {
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            try
            {
                conn.Open();
                OleDbCommand cmd = new OleDbCommand("SELECT Name FROM Teachers", conn);
                OleDbDataReader reader = cmd.ExecuteReader();

                cmbTeacher.Items.Clear();
                while (reader.Read())
                {
                    cmbTeacher.Items.Add(reader["Name"].ToString());
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading Teachers: " + ex.Message);
                conn.Close();
            }
        }
        private void Class_Load(object sender, EventArgs e)
        {
            LoadClassData();
            LoadTeachers();
        }
    }
}

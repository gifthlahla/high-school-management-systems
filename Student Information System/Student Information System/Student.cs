using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Student_Information_System
{
    public partial class Student : Form
    {
        private OleDbConnection conn;
        private string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\StudentInfo.accdb";

        public Student()
        {
            InitializeComponent();
            conn = new OleDbConnection(connectionString);
        }

        private void Student_Load(object sender, EventArgs e)
        {
            LoadStudents();
            LoadClasses();
        }

        private void LoadStudents()
        {
            try
            {
                conn.Open();
                OleDbDataAdapter dataAdapter = new OleDbDataAdapter("SELECT * FROM Students", conn);
                DataTable dt = new DataTable();
                dataAdapter.Fill(dt);
                dgvStudents.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void LoadClasses()
        {
            try
            {
                conn.Open();
                OleDbCommand cmd = new OleDbCommand("SELECT ClassName FROM Classes", conn);
                OleDbDataReader reader = cmd.ExecuteReader();

                cmbClass.Items.Clear();
                while (reader.Read())
                {
                    cmbClass.Items.Add(reader["ClassName"].ToString());
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading Classes: " + ex.Message);
                conn.Close();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtPhone.Text) || string.IsNullOrWhiteSpace(txtEmail.Text) || cmbClass.SelectedIndex == -1)
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            try
            {
                conn.Open();
                string query = "INSERT INTO Students (Name, Phone, Email, Class) VALUES (@Name, @Phone, @Email, @Class)";
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("@Name", txtName.Text);
                cmd.Parameters.AddWithValue("@Phone", txtPhone.Text);
                cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                cmd.Parameters.AddWithValue("@Class", cmbClass.SelectedItem.ToString());
                cmd.ExecuteNonQuery();

                MessageBox.Show("Student saved successfully.");
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                conn.Close();
                LoadStudents();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtID.Text) || string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtPhone.Text) || string.IsNullOrWhiteSpace(txtEmail.Text) || cmbClass.SelectedIndex == -1)
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            try
            {
                conn.Open();
                string query = "UPDATE Students SET Name = @Name, Phone = @Phone, Email = @Email, Class = @Class WHERE ID = @ID";
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("@ID", int.Parse(txtID.Text));
                cmd.Parameters.AddWithValue("@Name", txtName.Text);
                cmd.Parameters.AddWithValue("@Phone", txtPhone.Text);
                cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                cmd.Parameters.AddWithValue("@Class", cmbClass.SelectedItem.ToString());
                cmd.ExecuteNonQuery();

                MessageBox.Show("Student updated successfully.");
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                conn.Close();
                LoadStudents();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtID.Text))
            {
                MessageBox.Show("Please select a student to delete.");
                return;
            }

            try
            {
                conn.Open();
                string query = "DELETE FROM Students WHERE ID = @ID";
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("@ID", int.Parse(txtID.Text));
                cmd.ExecuteNonQuery();

                MessageBox.Show("Student deleted successfully.");     
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                conn.Close();
                LoadStudents();
            }
        }

        private void dgvStudents_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvStudents.Rows[e.RowIndex];
                txtID.Text = row.Cells["ID"].Value.ToString();
                txtName.Text = row.Cells["Name"].Value.ToString();
                txtPhone.Text = row.Cells["Phone"].Value.ToString();
                txtEmail.Text = row.Cells["Email"].Value.ToString();
                cmbClass.SelectedItem = row.Cells["Class"].Value.ToString();
            }
        }

        private void ClearFields()
        {
            // Clears text in objects
            txtID.Clear();
            txtName.Clear();
            txtPhone.Clear();
            txtEmail.Clear();
            cmbClass.SelectedIndex = -1;
        }
    }
}

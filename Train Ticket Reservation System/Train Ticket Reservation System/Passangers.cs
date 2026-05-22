using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Train_Ticket_Reservation_System
{
    public partial class Passangers : Form
    {
        string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Train.accdb";

        public Passangers()
        {
            InitializeComponent();
        }
        private void Passangers_Load(object sender, EventArgs e)
        {
            cmbGender.Items.AddRange(new string[] { "Male", "Female" });
            LoadPassengers();
        }

        private void LoadPassengers()
        {
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                string query = "SELECT * FROM Passengers";
                OleDbDataAdapter adapter = new OleDbDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dgvPassangers.DataSource = dt;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                string query = "INSERT INTO Passengers (FullName, Gender, Address, Phone, PassportNo, Nationality) " +
                               "VALUES (@FullName, @Gender, @Address, @Phone, @PassportNo, @Nationality)";
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("@FullName", txtFullName.Text);
                cmd.Parameters.AddWithValue("@Gender", cmbGender.Text);
                cmd.Parameters.AddWithValue("@Address", txtAddress.Text);
                cmd.Parameters.AddWithValue("@Phone", txtPhone.Text);
                cmd.Parameters.AddWithValue("@PassportNo", txtPassport.Text);
                cmd.Parameters.AddWithValue("@Nationality", txtNationality.Text);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                LoadPassengers();
                ClearInputs();
            }
        }

        private void dgvPassangers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtFullName.Text = dgvPassangers.Rows[e.RowIndex].Cells["FullName"].Value.ToString();
                cmbGender.Text = dgvPassangers.Rows[e.RowIndex].Cells["Gender"].Value.ToString();
                txtAddress.Text = dgvPassangers.Rows[e.RowIndex].Cells["Address"].Value.ToString();
                txtPhone.Text = dgvPassangers.Rows[e.RowIndex].Cells["Phone"].Value.ToString();
                txtPassport.Text = dgvPassangers.Rows[e.RowIndex].Cells["PassportNo"].Value.ToString();
                txtNationality.Text = dgvPassangers.Rows[e.RowIndex].Cells["Nationality"].Value.ToString();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvPassangers.CurrentRow != null)
            {
                int passengerID = Convert.ToInt32(dgvPassangers.CurrentRow.Cells["PassengerID"].Value);

                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    string query = "UPDATE Passengers SET FullName=@FullName, Gender=@Gender, Address=@Address, Phone=@Phone, " +
                                   "PassportNo=@PassportNo, Nationality=@Nationality WHERE PassengerID=@PassengerID";
                    OleDbCommand cmd = new OleDbCommand(query, conn);
                    cmd.Parameters.AddWithValue("@FullName", txtFullName.Text);
                    cmd.Parameters.AddWithValue("@Gender", cmbGender.Text);
                    cmd.Parameters.AddWithValue("@Address", txtAddress.Text);
                    cmd.Parameters.AddWithValue("@Phone", txtPhone.Text);
                    cmd.Parameters.AddWithValue("@PassportNo", txtPassport.Text);
                    cmd.Parameters.AddWithValue("@Nationality", txtNationality.Text);
                    cmd.Parameters.AddWithValue("@PassengerID", passengerID);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    LoadPassengers();
                    ClearInputs();
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvPassangers.CurrentRow != null)
            {
                int passengerID = Convert.ToInt32(dgvPassangers.CurrentRow.Cells["PassengerID"].Value);
                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    string query = "DELETE FROM Passengers WHERE PassengerID=@PassengerID";
                    OleDbCommand cmd = new OleDbCommand(query, conn);
                    cmd.Parameters.AddWithValue("@PassengerID", passengerID);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    LoadPassengers();
                    ClearInputs();
                }
            }
        }

        private void ClearInputs()
        {
            txtFullName.Clear();
            cmbGender.SelectedIndex = -1;
            txtAddress.Clear();
            txtPhone.Clear();
            txtPassport.Clear();
            txtNationality.Clear();
        }
    }
}
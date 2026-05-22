using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Train_Ticket_Reservation_System
{
    public partial class Ticket : Form
    {
        string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Train.accdb";

        public Ticket()
        {
            InitializeComponent();
        }

        private void Ticket_Load(object sender, EventArgs e)
        {
            LoadPassengers();
            LoadFlights();
            LoadTickets();
        }

        private void LoadPassengers()
        {
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                string query = "SELECT PassengerID, FullName FROM Passengers";
                OleDbDataAdapter adapter = new OleDbDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                cmbPassenger.DataSource = dt;
                cmbPassenger.DisplayMember = "FullName";
                cmbPassenger.ValueMember = "PassengerID";
            }
        }

        private void LoadFlights()
        {
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                string query = "SELECT TrainID, TrainNumber FROM Trains";
                OleDbDataAdapter adapter = new OleDbDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                cmbFlight.DataSource = dt;
                cmbFlight.DisplayMember = "TrainNumber";
                cmbFlight.ValueMember = "TrainID";
            }
        }

        private void LoadTickets()
        {
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                string query = @"SELECT T.TicketID, P.FullName, F.TrainNumber, T.SeatNumber, T.BookingDate
                         FROM (Tickets T 
                         INNER JOIN Passengers P ON T.PassengerID = P.PassengerID)
                         INNER JOIN Trains F ON T.TrainID = F.TrainID";

                OleDbDataAdapter adapter = new OleDbDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dgvTickets.DataSource = dt;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int passengerId = Convert.ToInt32(cmbPassenger.SelectedValue);
            int flightId = Convert.ToInt32(cmbFlight.SelectedValue);
            string seat = txtSeatNumber.Text.Trim();
            DateTime bookingDate = dtpBookingDate.Value;

            if (string.IsNullOrWhiteSpace(seat))
            {
                MessageBox.Show("Please enter a seat number.");
                return;
            }

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();

                // Step 1: Check for duplicate seat on the same flight
                string checkQuery = "SELECT COUNT(*) FROM Tickets WHERE TrainID = ? AND SeatNumber = ?";
                OleDbCommand checkCmd = new OleDbCommand(checkQuery, conn);
                checkCmd.Parameters.AddWithValue("?", flightId);
                checkCmd.Parameters.AddWithValue("?", seat);

                int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                if (count > 0)
                {
                    MessageBox.Show("This seat is already booked for the selected Train. Please choose another.");
                    return;
                }

                // Step 2: Insert new ticket
                string insertQuery = "INSERT INTO Tickets (PassengerID, TrainID, SeatNumber, BookingDate) " +
                                     "VALUES (?, ?, ?, ?)";
                OleDbCommand insertCmd = new OleDbCommand(insertQuery, conn);
                insertCmd.Parameters.AddWithValue("?", passengerId);
                insertCmd.Parameters.AddWithValue("?", flightId);
                insertCmd.Parameters.AddWithValue("?", seat);
                insertCmd.Parameters.AddWithValue("?", bookingDate.ToString("dd/MM/yyyy"));

                insertCmd.ExecuteNonQuery();
                conn.Close();

                LoadTickets();
                ClearInputs();
                MessageBox.Show("Ticket booked successfully!");
            }
        }


        private void ClearInputs()
        {
            txtSeatNumber.Clear();
            cmbPassenger.SelectedIndex = -1;
            cmbFlight.SelectedIndex = -1;
            dtpBookingDate.Value = DateTime.Today;
        }

        private void dgvTickets_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                cmbPassenger.Text = dgvTickets.Rows[e.RowIndex].Cells["FullName"].Value.ToString();
                cmbFlight.Text = dgvTickets.Rows[e.RowIndex].Cells["TrainNumber"].Value.ToString();
                dtpBookingDate.Value = Convert.ToDateTime(dgvTickets.Rows[e.RowIndex].Cells["BookingDate"].Value);
                txtSeatNumber.Text = dgvTickets.Rows[e.RowIndex].Cells["SeatNumber"].Value.ToString();
            }
        }
    }
}

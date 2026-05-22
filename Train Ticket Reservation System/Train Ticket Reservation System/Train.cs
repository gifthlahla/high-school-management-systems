using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Train_Ticket_Reservation_System
{
    public partial class Train : Form
    {
        string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Train.accdb";
        public Train()
        {
            InitializeComponent();
        }

        private void Train_Load(object sender, EventArgs e)
        {
            LoadTrains();
        }
        private void LoadTrains()
        {
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                string query = "SELECT * FROM Trains";
                OleDbDataAdapter adapter = new OleDbDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dgvTrains.DataSource = dt;
                dgvTrains.Columns["DepartureTime"].DefaultCellStyle.Format = "HH:mm";
                dgvTrains.Columns["ArrivalTime"].DefaultCellStyle.Format = "HH:mm";
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                string query = "INSERT INTO Trains (TrainNumber, Origin, Destination, DepartureTime, ArrivalTime, SeatsAvailable) " +
                               "VALUES (@TrainNumber, @Origin, @Destination, @DepartureTime, @ArrivalTime, @SeatsAvailable)";
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("@TrainNumber", txtFlightNumber.Text);
                cmd.Parameters.AddWithValue("@Origin", txtOrigin.Text);
                cmd.Parameters.AddWithValue("@Destination", txtDestination.Text);
                cmd.Parameters.AddWithValue("@DepartureTime", dtpDeparture.Value.ToString("HH:mm"));
                cmd.Parameters.AddWithValue("@ArrivalTime", dtpArrival.Value.ToString("HH:mm"));
                cmd.Parameters.AddWithValue("@SeatsAvailable", nudSeats.Value);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                LoadTrains();
                ClearInputs();
            }
        }

        private void dgvTrains_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtFlightNumber.Text = dgvTrains.Rows[e.RowIndex].Cells["TrainNumber"].Value.ToString();
                txtOrigin.Text = dgvTrains.Rows[e.RowIndex].Cells["Origin"].Value.ToString();
                txtDestination.Text = dgvTrains.Rows[e.RowIndex].Cells["Destination"].Value.ToString();
                dtpDeparture.Value = Convert.ToDateTime(dgvTrains.Rows[e.RowIndex].Cells["DepartureTime"].Value);
                dtpArrival.Value = Convert.ToDateTime(dgvTrains.Rows[e.RowIndex].Cells["ArrivalTime"].Value);
                nudSeats.Value = Convert.ToInt32(dgvTrains.Rows[e.RowIndex].Cells["SeatsAvailable"].Value);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvTrains.CurrentRow != null)
            {
                int flightID = Convert.ToInt32(dgvTrains.CurrentRow.Cells["TrainID"].Value);
                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    string query = "UPDATE Trains SET TrainNumber=@TrainNumber, Origin=@Origin, Destination=@Destination, " +
                                   "DepartureTime=@DepartureTime, ArrivalTime=@ArrivalTime, SeatsAvailable=@SeatsAvailable " +
                                   "WHERE TrainID=@FTrainID";
                    OleDbCommand cmd = new OleDbCommand(query, conn);
                    cmd.Parameters.AddWithValue("@TrainNumber", txtFlightNumber.Text);
                    cmd.Parameters.AddWithValue("@Origin", txtOrigin.Text);
                    cmd.Parameters.AddWithValue("@Destination", txtDestination.Text);
                    cmd.Parameters.AddWithValue("@DepartureTime", dtpDeparture.Value.ToString("HH:mm"));
                    cmd.Parameters.AddWithValue("@ArrivalTime", dtpArrival.Value.ToString("HH:mm"));
                    cmd.Parameters.AddWithValue("@SeatsAvailable", nudSeats.Value);
                    cmd.Parameters.AddWithValue("@TrainID", flightID);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    LoadTrains();
                    ClearInputs();
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvTrains.CurrentRow != null)
            {
                int trainID = Convert.ToInt32(dgvTrains.CurrentRow.Cells["TrainID"].Value);
                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    string query = "DELETE FROM Trains WHERE TrainID=@TrainID";
                    OleDbCommand cmd = new OleDbCommand(query, conn);
                    cmd.Parameters.AddWithValue("@TrainID", trainID);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    LoadTrains();
                    ClearInputs();
                }
            }
        }

        private void ClearInputs()
        {
            txtFlightNumber.Clear();
            txtOrigin.Clear();
            txtDestination.Clear();
            dtpDeparture.Value = DateTime.Now;
            dtpArrival.Value = DateTime.Now;
            nudSeats.Value = 1;
        }
    }
}
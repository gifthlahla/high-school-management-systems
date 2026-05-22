using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;


namespace Train_Ticket_Reservation_System
{
    public partial class Cancellation : Form
    {
        string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Train.accdb";

        public Cancellation()
        {
            InitializeComponent();
        }

        private void Cancellation_Load(object sender, EventArgs e)
        {
            LoadTickets();
            LoadCancellations();
            LoadTicketsIntoComboBox();
        }

        private void LoadTickets()
        {
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                string query = @"
            SELECT 
                T.TicketID, 
                P.FullName & ' - ' & F.TrainNumber AS TicketInfo
            FROM 
                ((Tickets T
                INNER JOIN Passengers P ON T.PassengerID = P.PassengerID)
                INNER JOIN Trains F ON T.TrainID = F.TrainID)";

                OleDbDataAdapter adapter = new OleDbDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                cmbTicket.DataSource = dt;
                cmbTicket.DisplayMember = "TicketInfo";
                cmbTicket.ValueMember = "TicketID";
            }
        }


        private void LoadCancellations()
        {
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                string query = @"SELECT C.CancelID, C.TicketID, P.FullName, F.TrainNumber, 
                                C.CancellationDate, C.Reason
                         FROM ((Cancellations C 
                         INNER JOIN Tickets T ON C.TicketID = T.TicketID)
                         INNER JOIN Passengers P ON T.PassengerID = P.PassengerID)
                         INNER JOIN Trains F ON T.TrainID = F.TrainID";

                OleDbDataAdapter adapter = new OleDbDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dgvCancellations.DataSource = dt;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (cmbTicket.SelectedValue == null)
            {
                MessageBox.Show("Please select a valid ticket.");
                return;
            }

            int ticketId = Convert.ToInt32(cmbTicket.SelectedValue);
            string reason = cmbReason.Text.Trim();
            DateTime cancelDate = dtpCancelDate.Value;

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                string query = "INSERT INTO Cancellations (TicketID, CancellationDate, Reason) VALUES (?, ?, ?)";

                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.Add("?", OleDbType.Integer).Value = ticketId;
                cmd.Parameters.Add("?", OleDbType.Date).Value = cancelDate.ToString("dd/MM/yyyy");
                cmd.Parameters.Add("?", OleDbType.VarChar).Value = reason;

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

                MessageBox.Show("Booking cancelled successfully.");
                LoadCancelledBookings();
                ClearInputs();
            }
        }

        private void LoadTicketsIntoComboBox()
        {
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                string query = @"
            SELECT T.TicketID, 
                   'TCK' & T.TicketID & ' - ' & P.FullName AS DisplayName
            FROM Tickets T
            INNER JOIN Passengers P ON T.PassengerID = P.PassengerID";

                OleDbDataAdapter da = new OleDbDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                cmbTicket.DataSource = dt;
                cmbTicket.DisplayMember = "DisplayName";  // what is shown
                cmbTicket.ValueMember = "TicketID";       // what is used in code
            }
        }

        private void LoadCancelledBookings()
        {
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                string query = @"
            SELECT 
                C.CancelID,
                C.TicketID,
                P.FullName,
                F.TrainNumber,
                C.CancellationDate,
                C.Reason
            FROM 
                ((Cancellations AS C
                INNER JOIN Tickets AS T ON C.TicketID = T.TicketID)
                INNER JOIN Passengers AS P ON T.PassengerID = P.PassengerID)
                INNER JOIN Trains AS F ON T.TrainID = F.TrainID
            ORDER BY C.CancellationDate DESC";

                OleDbDataAdapter da = new OleDbDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvCancellations.DataSource = dt;
            }
        }

        private void ClearInputs()
        {
            cmbTicket.SelectedIndex = -1;
            cmbReason.SelectedIndex = -1;
            dtpCancelDate.Value = DateTime.Today;
        }

        private void dgvCancellations_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                cmbTicket.Text = dgvCancellations.Rows[e.RowIndex].Cells["TicketID"].Value.ToString();
                cmbReason.Text = dgvCancellations.Rows[e.RowIndex].Cells["Reason"].Value.ToString();
                dtpCancelDate.Value = Convert.ToDateTime(dgvCancellations.Rows[e.RowIndex].Cells["CancellationDate"].Value);

            }
        }
    }
}
using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Fees_Payment_System
{
    public partial class Payment : Form
    {
        OleDbConnection con = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=FeesPayment.accdb");

        public Payment()
        {
            InitializeComponent();
            LoadPaymentIDs();
            LoadFeeTypes();
            LoadPayments();
            LoadStudentIDs();
        }

        private void LoadPaymentIDs()
        {
            con.Open();
            OleDbCommand cmd = new OleDbCommand("SELECT PaymentID FROM Payment", con);
            OleDbDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                cmbPaymentID.Items.Add(dr["PaymentID"].ToString());
            }
            dr.Close();
            con.Close();
        }

        private void LoadFeeTypes()
        {
            con.Open();
            OleDbCommand cmd = new OleDbCommand("SELECT FeeType FROM Fees", con);
            OleDbDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                cmbFeeType.Items.Add(dr["FeeType"].ToString());
            }
            dr.Close();
            con.Close();
        }
        private void cmbFeeType_SelectedIndexChanged(object sender, EventArgs e)
        {
            con.Open();
            OleDbCommand cmd = new OleDbCommand("SELECT Amount FROM Fees WHERE FeeType = @f", con);
            cmd.Parameters.AddWithValue("@f", cmbFeeType.Text);
            OleDbDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                lblFeeAmount.Text = dr["Amount"].ToString();
            }
            dr.Close();
            con.Close();
        }

        private void btnCalculateBalance_Click(object sender, EventArgs e)
        {
            decimal feeAmount, amountPaid;

            if (decimal.TryParse(lblFeeAmount.Text, out feeAmount) &&
                decimal.TryParse(txtAmountPaid.Text, out amountPaid))
            {
                decimal balance = feeAmount - amountPaid;
                lblBalance.Text = balance.ToString("0.00");
            }
            else
            {
                MessageBox.Show("Please enter valid numbers for Fee Amount and Amount Paid.");
            }

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (cmbStudentID.Text == "" || cmbFeeType.Text == "" ||
                lblFeeAmount.Text == "" || txtAmountPaid.Text == "" || lblBalance.Text == "" || cmbPaymentMethod.Text == "")
            {
                MessageBox.Show("Please fill in all required fields.");
                return;
            }

            con.Open();
            OleDbCommand cmd = new OleDbCommand(
                "INSERT INTO Payment (StudentID, PaymentDate, FeeType, FeeAmount, PaymentMethod, AmountPaid, Balance, [Form]) " +
                "VALUES (?, ?, ?, ?, ?, ?, ?, ?)", con);
            cmd.Parameters.AddWithValue("?", cmbStudentID.Text);
            cmd.Parameters.AddWithValue("?", dtpPaymentDate.Value);
            cmd.Parameters.AddWithValue("?", cmbFeeType.Text);
            cmd.Parameters.AddWithValue("?", Convert.ToDecimal(lblFeeAmount.Text));
            cmd.Parameters.AddWithValue("?", cmbPaymentMethod.Text);
            cmd.Parameters.AddWithValue("?", Convert.ToDecimal(txtAmountPaid.Text));
            cmd.Parameters.AddWithValue("?", Convert.ToDecimal(lblBalance.Text));
            cmd.Parameters.AddWithValue("?", lblForm.Text);
            cmd.ExecuteNonQuery();
            con.Close();

            MessageBox.Show("Payment record saved successfully.");
            LoadPayments();
            ClearFields();
        }

        private void LoadPayments()
        {
            con.Open();
            OleDbDataAdapter da = new OleDbDataAdapter("SELECT * FROM Payment", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dgvPayments.DataSource = dt;
            con.Close();
        }

        private void ClearFields()
        {
            cmbPaymentID.Text = "";
            cmbStudentID.Text = "";
            cmbFeeType.Text = "";
            lblFeeAmount.Clear();
            cmbPaymentMethod.Text = "";
            txtAmountPaid.Clear();
            lblBalance.Clear();
            lblForm.Clear();
            dtpPaymentDate.Value = DateTime.Today;
        }

        private void dgvPayments_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvPayments.Rows[e.RowIndex];
                cmbStudentID.Text = row.Cells["StudentID"].Value.ToString();
                dtpPaymentDate.Value = Convert.ToDateTime(row.Cells["PaymentDate"].Value);
                cmbFeeType.Text = row.Cells["FeeType"].Value.ToString();
                lblFeeAmount.Text = row.Cells["FeeAmount"].Value.ToString();
                cmbPaymentMethod.Text = row.Cells["PaymentMethod"].Value.ToString();
                txtAmountPaid.Text = row.Cells["AmountPaid"].Value.ToString();
                lblBalance.Text = row.Cells["Balance"].Value.ToString();
                lblForm.Text = row.Cells["Form"].Value.ToString();
            }
        }

        private void cmbPaymentID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cmbPaymentID.Text)) return;
            try
            {
                con.Open();
                var cmd = new OleDbCommand("SELECT * FROM Payment WHERE PaymentID = ?", con);
                cmd.Parameters.AddWithValue("?", cmbPaymentID.Text);
                var rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    dtpPaymentDate.Value = Convert.ToDateTime(rdr["PaymentDate"]);
                    cmbPaymentMethod.Text = rdr["PaymentMethod"].ToString();
                    cmbFeeType.Text = rdr["FeeType"].ToString();
                    lblFeeAmount.Text = rdr["FeeAmount"].ToString();
                    cmbStudentID.Text = rdr["StudentID"].ToString();
                    txtAmountPaid.Text = rdr["AmountPaid"].ToString();
                    lblBalance.Text = rdr["Balance"].ToString();
                }
                rdr.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { con.Close(); }
        }

        private void cmbStudentID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cmbStudentID.Text)) return;
            LoadStudentDetails(cmbStudentID.Text);
        }
        private void LoadStudentDetails(string studentId)
        {
            try
            {
                con.Open();
                var cmd = new OleDbCommand("SELECT StudentName, [Form] FROM Students WHERE StudentID = ?", con);
                cmd.Parameters.AddWithValue("?", studentId);
                var rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    lblStudentName.Text = rdr["StudentName"].ToString();
                    lblForm.Text = rdr["Form"].ToString();
                }
                rdr.Close();
            }
            catch { }
            finally { con.Close(); }
        }
        private void LoadStudentIDs()
        {
            cmbStudentID.Items.Clear();
            try
            {
                con.Open();
                var cmd = new OleDbCommand("SELECT StudentID FROM Students", con);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                    cmbStudentID.Items.Add(rdr["StudentID"].ToString());
                rdr.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { con.Close(); }
        }

    }
}

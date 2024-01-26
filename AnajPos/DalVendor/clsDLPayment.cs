using AnajPos.BALVendor;
using AnajPos.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnajPos.DalVendor
{
    class clsDLPayment
    {
        SqlConnection conn = new SqlConnection(clsConnectionString.cs);
        public int Insert(clsBlPayment BlPayment)
        {
            int HowManyInsert = 0;
            using (SqlCommand cmd = new SqlCommand("usp_InsertPayment", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PayId", BlPayment.PaymentID);
                cmd.Parameters.AddWithValue("@PayDate", BlPayment.PaymentDate);
                cmd.Parameters.AddWithValue("@VendorId", BlPayment.VendorID);
                cmd.Parameters.AddWithValue("@HeadId", BlPayment.HeadID);
                cmd.Parameters.AddWithValue("@Remark", BlPayment.PayRemark);
                cmd.Parameters.AddWithValue("@Amount", BlPayment.Amount);
                cmd.Parameters.AddWithValue("@PreviousAmount", BlPayment.PreviousAmount);
                cmd.Parameters.AddWithValue("@EntryDate", BlPayment.EntryDate);
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                HowManyInsert = cmd.ExecuteNonQuery();

            }
            return HowManyInsert;

        }
        public int NewId()
        {
            int newId = 0;
            using (SqlCommand cmd = new SqlCommand("select max(PayId) from tblPayment", conn))
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                try
                {
                    newId = (int)cmd.ExecuteScalar();
                    newId++;
                }
                catch
                {

                    newId = 1001;
                }
            }

            return newId;

        }
        public int FirstId()
        {
            int FirstId = 0;
            using (SqlCommand cmd = new SqlCommand("select min(PayId) from tblPayment", conn))
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                try
                {
                    FirstId = (int)cmd.ExecuteScalar();
                }
                catch
                {
                    FirstId = 0;
                }

            }
            return FirstId;
        }
        public int LastId()
        {
            int LastId = 0;
            using (SqlCommand cmd = new SqlCommand("select max(PayId) from tblPayment", conn))
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                try
                {
                    LastId = (int)cmd.ExecuteScalar();
                }
                catch
                {
                    LastId = 0;
                }

            }
            return LastId;
        }
        public DataTable ViewPayment(int PayId)
        {
            DataTable dt = new DataTable();

            SqlDataAdapter adpt = new SqlDataAdapter("usp_ViewPaymentByID", conn);
            adpt.SelectCommand.CommandType = CommandType.StoredProcedure;
            adpt.SelectCommand.Parameters.AddWithValue("@id", PayId);
            adpt.Fill(dt);
            return dt;
        }

        public int Update(clsBlPayment BlPayment)
        {
            int HowManyInsert = 0;
            using (SqlCommand cmd = new SqlCommand("usp_UpdatePayment", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PayId", BlPayment.PaymentID);
                cmd.Parameters.AddWithValue("@PayDate", BlPayment.PaymentDate);
                cmd.Parameters.AddWithValue("@VendorId", BlPayment.VendorID);
                cmd.Parameters.AddWithValue("@HeadId", BlPayment.HeadID);
                cmd.Parameters.AddWithValue("@Remark", BlPayment.PayRemark);
                cmd.Parameters.AddWithValue("@Amount", BlPayment.Amount);
                cmd.Parameters.AddWithValue("@PreviousAmount", BlPayment.PreviousAmount);
                //cmd.Parameters.AddWithValue("@EntryDate", BlPayment.EntryDate);
                
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                HowManyInsert = cmd.ExecuteNonQuery();

            }
            return HowManyInsert;

        }

        public DataTable ViewScreen(string DayDate)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter adpt = new SqlDataAdapter("usp_ViewPayment", conn);
            adpt.SelectCommand.CommandType = CommandType.StoredProcedure;
            adpt.SelectCommand.Parameters.AddWithValue("@date", DayDate);
            adpt.Fill(dt);
            return dt;
        }
    }
}

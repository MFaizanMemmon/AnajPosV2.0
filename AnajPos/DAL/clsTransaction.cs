using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnajPos.BAL;
using AnajPos.DAL;
using System.Data.SqlClient;
using System.Data;

namespace AnajPos.DAL
{
    class clsTransaction
    {
        SqlConnection conn = new SqlConnection(clsConnectionString.cs);
        public int Insert(clsBlTransaction BlTransaction)
        {
            int HowManyInsert = 0;
            using (SqlCommand cmd = new SqlCommand("usp_InsertTransaction", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TransactionDate", BlTransaction.TransactionDate);
                cmd.Parameters.AddWithValue("@TranType", BlTransaction.TransactionType);
                cmd.Parameters.AddWithValue("@TranID", BlTransaction.TransactionId);
                cmd.Parameters.AddWithValue("@CustomerName", BlTransaction.CustomerId);
                cmd.Parameters.AddWithValue("@Remark", BlTransaction.Remark);
                cmd.Parameters.AddWithValue("@Dr", BlTransaction.Dr);
                cmd.Parameters.AddWithValue("@Cr",BlTransaction.Cr);
                cmd.Parameters.AddWithValue("@PostingDate", BlTransaction.PostingDate);
                cmd.Parameters.AddWithValue("@TranTypeUrdu", BlTransaction.TranTypeUrdu);
                if (conn.State != ConnectionState.Open)
                {

                    conn.Open();
                }
                HowManyInsert = cmd.ExecuteNonQuery();

            }
            return HowManyInsert;

        }

        public int Update(clsBlTransaction BlTransaction)
        {
            int HowManyInsert = 0;
            using (SqlCommand cmd = new SqlCommand("usp_UpdateTransaction", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TransactionDate", BlTransaction.TransactionDate);
                cmd.Parameters.AddWithValue("@TranType", BlTransaction.TransactionType);
                cmd.Parameters.AddWithValue("@TranID", BlTransaction.TransactionId);
                cmd.Parameters.AddWithValue("@CustomerName", BlTransaction.CustomerId);
                cmd.Parameters.AddWithValue("@Remark", BlTransaction.Remark);
                cmd.Parameters.AddWithValue("@Dr", BlTransaction.Dr);
                cmd.Parameters.AddWithValue("@Cr", BlTransaction.Cr);
                cmd.Parameters.AddWithValue("@TranTypeUrdu", BlTransaction.TranTypeUrdu);
                //cmd.Parameters.AddWithValue("@PostingDate", BlTransaction.PostingDate);
                if (conn.State != ConnectionState.Open)
                {

                    conn.Open();
                }
                HowManyInsert = cmd.ExecuteNonQuery();
                conn.Close();
            }
            return HowManyInsert;

        }

        public int UpdateAdjustment(clsBlTransaction BlTransaction)
        {
            int HowManyInsert = 0;
            using (SqlCommand cmd = new SqlCommand("usp_UpdateTransactionAdjustment", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TransactionDate", BlTransaction.TransactionDate);
                cmd.Parameters.AddWithValue("@TranType", BlTransaction.TransactionType);
                cmd.Parameters.AddWithValue("@TranID", BlTransaction.TransactionId);
                cmd.Parameters.AddWithValue("@CustomerName", BlTransaction.CustomerId);
                cmd.Parameters.AddWithValue("@Remark", BlTransaction.Remark);
                cmd.Parameters.AddWithValue("@Dr", BlTransaction.Dr);
                cmd.Parameters.AddWithValue("@Cr", BlTransaction.Cr);
                cmd.Parameters.AddWithValue("@TranTypeUrdu", BlTransaction.TranTypeUrdu);
                //cmd.Parameters.AddWithValue("@PostingDate", BlTransaction.PostingDate);
                if (conn.State != ConnectionState.Open)
                {

                    conn.Open();
                }
                HowManyInsert = cmd.ExecuteNonQuery();
                conn.Close();
            }
            return HowManyInsert;

        }
        public int UpdateReceipt(clsBlTransaction BlTransaction)
        {
            int HowManyUpdate = 0;
            using (SqlCommand cmd = new SqlCommand("usp_UpdateReceiptTransaction", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TranDate", BlTransaction.TransactionDate);
                cmd.Parameters.AddWithValue("@TranId", BlTransaction.TransactionId);
                cmd.Parameters.AddWithValue("@CustomerName", BlTransaction.CustomerId);
                cmd.Parameters.AddWithValue("@Remark", BlTransaction.Remark);
                cmd.Parameters.AddWithValue("@Dr", BlTransaction.Dr);
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                HowManyUpdate = cmd.ExecuteNonQuery();
                conn.Close();
            }
            return HowManyUpdate;
        }


    }
}

using AnajPos.BAL_Cash_BANK;
using AnajPos.DAL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnajPos.DAL_Cash_Bank
{
    class clsChartOfAccountLedger
    {
        SqlConnection conn = new SqlConnection(clsConnectionString.cs);
        public int Insert(clsBLChartOfAccountLedger BlLedger)
        {
            int HowManyInsert = 0;
            using (SqlCommand cmd = new SqlCommand("usp_InsertChartOfAccountLedgers", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TransactionDate",BlLedger.TransactionDate);
                cmd.Parameters.AddWithValue("@TransactionType",BlLedger.TransactionType);
                cmd.Parameters.AddWithValue("@TransactionID",BlLedger.TransactionId);
                cmd.Parameters.AddWithValue("@ChartOfAccountId",BlLedger.ChartOfAccountId);
                cmd.Parameters.AddWithValue("@Remark",BlLedger.Remark);
                cmd.Parameters.AddWithValue("@Dr",BlLedger.Dr);
                cmd.Parameters.AddWithValue("@Cr",BlLedger.Cr);

                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                HowManyInsert = cmd.ExecuteNonQuery();

            }
            return HowManyInsert;

        }

        // Update Ledger
        public int UpdateLedger(clsBLChartOfAccountLedger BlLedger)
        {
            int HowManyInsert = 0;
            using (SqlCommand cmd = new SqlCommand("UpdaterLedger", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TransactionDate", BlLedger.TransactionDate);
                cmd.Parameters.AddWithValue("@TransactionType", BlLedger.TransactionType);
                cmd.Parameters.AddWithValue("@TransactionID", BlLedger.TransactionId);
                cmd.Parameters.AddWithValue("@ChartOfAccountId", BlLedger.ChartOfAccountId);
                cmd.Parameters.AddWithValue("@Remark", BlLedger.Remark);
                cmd.Parameters.AddWithValue("@Dr", BlLedger.Dr);
                cmd.Parameters.AddWithValue("@Cr", BlLedger.Cr);

                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                HowManyInsert = cmd.ExecuteNonQuery();

            }
            return HowManyInsert;

        }

        public DataTable View()
        {
            DataTable dt = new DataTable();

            using (SqlCommand cmd = new SqlCommand("usp_tblBankView", conn))
            {
                SqlDataAdapter adpt = new SqlDataAdapter(cmd);
                adpt.Fill(dt);
            }
            return dt;
        }
        public DataTable ViewBankByID(int BankId)
        {
            DataTable dt = new DataTable();

            using (SqlCommand cmd = new SqlCommand("usp_tblBankViewByID", conn))
            {
                SqlDataAdapter adpt = new SqlDataAdapter(cmd);
                adpt.SelectCommand.CommandType = CommandType.StoredProcedure;
                adpt.SelectCommand.Parameters.AddWithValue("@id", BankId);
                adpt.Fill(dt);
            }
            return dt;
        }

        public int UpdateBank(clsBLChartOfAccountLedger BlBank)
        {
            int HowManyUpdate = 0;
            using (SqlCommand cmd = new SqlCommand("usp_UpdateBank", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
               
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                HowManyUpdate = cmd.ExecuteNonQuery();
            }
            return HowManyUpdate;
        }

        public int DeleteLedger(clsBLChartOfAccountLedger BlLedger)
        {
            int HowManyUpdate = 0;
            using (SqlCommand cmd = new SqlCommand("usp_DeleteLedger", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TransactionType", BlLedger.TransactionType);
                cmd.Parameters.AddWithValue("@TransactionId", BlLedger.TransactionId);
                cmd.Parameters.AddWithValue("@ChartOfAccountId", BlLedger.ChartOfAccountId);
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                HowManyUpdate = cmd.ExecuteNonQuery();
            }
            return HowManyUpdate;
        }

    }
}

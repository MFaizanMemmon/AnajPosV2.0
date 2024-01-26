using AnajPos.BAL;
using AnajPos.DAL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnajPos.BAL_Cash_BANK;

namespace AnajPos.DAL_Cash_Bank
{
    class clsBank
    {
        SqlConnection conn = new SqlConnection(clsConnectionString.cs);
        public int Insert(clsBlBank BlBank)
        {
            int HowManyInsert = 0;
            using (SqlCommand cmd = new SqlCommand("usp_tblBankInsert", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BankId",BlBank.BankId);
                cmd.Parameters.AddWithValue("@BankTitle",BlBank.BankTitle);
                cmd.Parameters.AddWithValue("@BankName",BlBank.BankName);
                cmd.Parameters.AddWithValue("@BankNameUrdu",BlBank.BankNameUrdu);
                cmd.Parameters.AddWithValue("@OpeningDate",BlBank.OpeningDate);
                cmd.Parameters.AddWithValue("@OpeningBalance",BlBank.OpeningBalance);
                cmd.Parameters.AddWithValue("@IsActive",BlBank.IsActive);
                cmd.Parameters.AddWithValue("@AccountId",BlBank.AccountId);
                cmd.Parameters.AddWithValue("@AccountNo", BlBank.AccountNo);
                cmd.Parameters.AddWithValue("@Address", BlBank.Address);
                
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

        public int Update(clsBlBank BlBank)
        {
            int HowManyUpdate = 0;
            using (SqlCommand cmd = new SqlCommand("usp_UpdateBank", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BankId", BlBank.BankId);
                cmd.Parameters.AddWithValue("@BankTitle", BlBank.BankTitle);
                cmd.Parameters.AddWithValue("@BankName", BlBank.BankName);
                cmd.Parameters.AddWithValue("@BankNameUrdu", BlBank.BankNameUrdu);
                cmd.Parameters.AddWithValue("@OpeningDate", BlBank.OpeningDate);
                cmd.Parameters.AddWithValue("@OpeningBalance", BlBank.OpeningBalance);
                cmd.Parameters.AddWithValue("@IsActive", BlBank.IsActive);
                cmd.Parameters.AddWithValue("@AccountId", BlBank.AccountId);
                cmd.Parameters.AddWithValue("@AccountNo", BlBank.AccountNo);
                cmd.Parameters.AddWithValue("@Address", BlBank.Address);
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                
                HowManyUpdate = cmd.ExecuteNonQuery();
            }
            return HowManyUpdate;
        }

        public int MinId()
        {
            int minId = 0;
            using (SqlCommand cmd = new SqlCommand("select Min(BankId) from tblBank", conn))
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                try
                {
                    minId = (int)cmd.ExecuteScalar();

                }

                catch
                {

                    minId = 0;
                }

            }
            return minId;
        }

        public int MaxId()
        {
            int max = 0;
            using (SqlCommand cmd = new SqlCommand("select max(BankId) from tblBank", conn))
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                try
                {
                    max = (int)cmd.ExecuteScalar();
                }
                catch
                {
                    max = 0;
                }


            }
            return max;
        }
        public int NewId()
        {
            int newId = 0;
            using (SqlCommand cmd = new SqlCommand("select max(BankId) from tblBank", conn))
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

                    newId = 1;
                }
            }

            return newId;

        }

        public int BankAccountId(int BankId)
        {
            int Amount = 0;
            using (SqlCommand cmd = new SqlCommand("usp_BankAccountId", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", BankId);
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                var result = cmd.ExecuteScalar();

                if (result != DBNull.Value)
                {
                    Amount = Convert.ToInt32(result);
                }

                conn.Close();
            }
            return Amount;
        }

        public int BankChartOfAccountId(int CustomerId)
        {
            int Amount = 0;
            using (SqlCommand cmd = new SqlCommand("usp_GetBankAccountid", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BankId", CustomerId);
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                var result = cmd.ExecuteScalar();

                if (result != DBNull.Value)
                {
                    Amount = Convert.ToInt32(result);
                }

                conn.Close();
            }
            return Amount;
        }

    }
}

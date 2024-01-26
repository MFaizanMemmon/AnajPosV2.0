using AnajPos.BAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnajPos.DAL
{
    class clsHeadOfAcc
    {
        SqlConnection conn = new SqlConnection(clsConnectionString.cs);
        public int Insert(clsBlHeadOfAcc BlHeadAccount)
        {
            int HowManyInsert = 0;
            using (SqlCommand cmd = new SqlCommand("usp_InsertHeadOfAcc", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HeadId",BlHeadAccount.HeadId);
                cmd.Parameters.AddWithValue("@HeadOfAcc", BlHeadAccount.HeadAccName);
                cmd.Parameters.AddWithValue("@HeadOfAccName", BlHeadAccount.HeadAccNameUrdu);
                cmd.Parameters.AddWithValue("@Remark", BlHeadAccount.Remark);
                cmd.Parameters.AddWithValue("@OpeningDate", BlHeadAccount.OpeningDate);
                cmd.Parameters.AddWithValue("@OpeningAmount", BlHeadAccount.OpeningAmount);
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
            using (SqlCommand cmd = new SqlCommand("select max(HeadId) from tblHeadOffAcc", conn))
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

                    newId = 101;
                }
            }

            return newId;

        }
        public DataTable ViewHead()
        {
            DataTable dt = new DataTable();
            using (SqlDataAdapter adtp = new SqlDataAdapter("usp_HeadOfAcc", conn))
            {
                adtp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adtp.Fill(dt);
            }
            return dt;
        }

        public int Update(clsBlHeadOfAcc BlHeadAccount)
        {
            int HowManyInsert = 0;
            using (SqlCommand cmd = new SqlCommand("usp_UpdateHead", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HeadId", BlHeadAccount.HeadId);
                cmd.Parameters.AddWithValue("@HeadOfAcc", BlHeadAccount.HeadAccName);
                cmd.Parameters.AddWithValue("@HeadOfAccName", BlHeadAccount.HeadAccNameUrdu);
                cmd.Parameters.AddWithValue("@Remark", BlHeadAccount.Remark);
                cmd.Parameters.AddWithValue("@OpeningDate", BlHeadAccount.OpeningDate);
                cmd.Parameters.AddWithValue("@OpeningAmount", BlHeadAccount.OpeningAmount);
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                HowManyInsert = cmd.ExecuteNonQuery();

            }
            return HowManyInsert;

        }


    }
}

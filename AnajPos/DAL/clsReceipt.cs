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
    class clsReceipt
    {
        SqlConnection conn = new SqlConnection(clsConnectionString.cs);
        public int Insert(clsBlReceipt BlReceipt)
        {
            int HowManyInsert = 0;
            using (SqlCommand cmd = new SqlCommand("usp_InsertReceipt", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RecId", BlReceipt.RecId);
                cmd.Parameters.AddWithValue("@RecDate", BlReceipt.RecDate);
                cmd.Parameters.AddWithValue("@Zone", BlReceipt.Zone);
                cmd.Parameters.AddWithValue("@Area", BlReceipt.Area);
                cmd.Parameters.AddWithValue("@CustomerID", BlReceipt.Customer);
                cmd.Parameters.AddWithValue("@HeadOfAcc", BlReceipt.Head);
                cmd.Parameters.AddWithValue("@AmountInEnglish", BlReceipt.AmountInEnglish);
                cmd.Parameters.AddWithValue("@Remark", BlReceipt.Remark);
                cmd.Parameters.AddWithValue("@Amount", BlReceipt.Amount);
                cmd.Parameters.AddWithValue("@PreviousAmount", BlReceipt.PreviousAmount);
                cmd.Parameters.AddWithValue("@LastBill", BlReceipt.LastBill);
                cmd.Parameters.AddWithValue("@LastReceipt", BlReceipt.LastBill);
                cmd.Parameters.AddWithValue("@CurrentBalance", BlReceipt.CurrentBalance);
                
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
            using (SqlCommand cmd = new SqlCommand("select max(RecId) from tblReceipt", conn))
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
            using (SqlCommand cmd=new SqlCommand("select min(RecId) from tblReceipt",conn))
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
            using (SqlCommand cmd = new SqlCommand("select max(RecId) from tblReceipt", conn))
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
        public DataTable ViewReceipt(int RecID)
        {
            DataTable dt = new DataTable();
            string query = string.Format("select * from tblReceipt where RecId = {0}", RecID);
            SqlDataAdapter adpt = new SqlDataAdapter(query,conn);
            adpt.Fill(dt);
            return dt;
        }

        public int Update(clsBlReceipt BlReceipt)
        {
            int HowManyInsert = 0;
            using (SqlCommand cmd = new SqlCommand("usp_UpdateReceipt", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RecId", BlReceipt.RecId);
                cmd.Parameters.AddWithValue("@RecDate", BlReceipt.RecDate);
                cmd.Parameters.AddWithValue("@Zone", BlReceipt.Zone);
                cmd.Parameters.AddWithValue("@Area", BlReceipt.Area);
                cmd.Parameters.AddWithValue("@CustomerID", BlReceipt.Customer);
                cmd.Parameters.AddWithValue("@HeadOfAcc", BlReceipt.Head);
                cmd.Parameters.AddWithValue("@AmountInEnglish", BlReceipt.AmountInEnglish);
                cmd.Parameters.AddWithValue("@Remark", BlReceipt.Remark);
                cmd.Parameters.AddWithValue("@Amount", BlReceipt.Amount);
                cmd.Parameters.AddWithValue("@PreviousAmount", BlReceipt.PreviousAmount);
                cmd.Parameters.AddWithValue("@LastBill", BlReceipt.LastBill);
                cmd.Parameters.AddWithValue("@LastReceipt", BlReceipt.LastBill);
                cmd.Parameters.AddWithValue("@CurrentBalance", BlReceipt.CurrentBalance);
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
            SqlDataAdapter adpt = new SqlDataAdapter("usp_ViewReceiving", conn);
            adpt.SelectCommand.CommandType = CommandType.StoredProcedure;
            adpt.SelectCommand.Parameters.AddWithValue("@date",DayDate);
            adpt.Fill(dt);
            return dt;
        }

    }
}

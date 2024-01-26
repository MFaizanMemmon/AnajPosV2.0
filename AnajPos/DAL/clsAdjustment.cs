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
    class clsAdjustment
    {
        SqlConnection conn = new SqlConnection(clsConnectionString.cs);
        public int Insert(clsBlAdjustment BlAdjustment)
        {
            int HowManyInsert = 0;
            using (SqlCommand cmd = new SqlCommand("InsertAdjustment", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", BlAdjustment.Id);
                cmd.Parameters.AddWithValue("@TransactionDate", BlAdjustment.TranTime);
                cmd.Parameters.AddWithValue("@ZoneId", BlAdjustment.ZoneId);
                cmd.Parameters.AddWithValue("@AdderssId", BlAdjustment.AddressId);
                cmd.Parameters.AddWithValue("@CustomerName", BlAdjustment.CustomerId);

                cmd.Parameters.AddWithValue("@Dr", BlAdjustment.Dr);
                cmd.Parameters.AddWithValue("@Cr", BlAdjustment.Cr);
                cmd.Parameters.AddWithValue("@Notes", BlAdjustment.Remark);
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
            using (SqlCommand cmd = new SqlCommand("select max(AdjustmentId) from tblAdjustment", conn))
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
        public DataTable ViewAdjustment(string ViewDate)
        {
            DataTable dt = new DataTable();
            using (SqlDataAdapter adtp=new SqlDataAdapter("usp_ViewAdjustment",conn))
            {
                adtp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adtp.SelectCommand.Parameters.AddWithValue("@date",ViewDate);
                adtp.Fill(dt);
            }
            return dt;
        }
        public DataTable ViewAdjustment()
        {
            DataTable dt = new DataTable();
            using (SqlDataAdapter adtp = new SqlDataAdapter("usp_ViewAllAdjustment", conn))
            {
                adtp.SelectCommand.CommandType = CommandType.StoredProcedure;
               // adtp.SelectCommand.Parameters.AddWithValue("@date", BlAdjustment.TranTime);
                adtp.Fill(dt);
            }
            return dt;
        }
        public DataTable ViewAdjustmentById(clsBlAdjustment BlAdjustment)
        {
            DataTable dt = new DataTable();
            using (SqlDataAdapter adtp = new SqlDataAdapter("usp_ViewSingleAdjustment", conn))
            {
                adtp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adtp.SelectCommand.Parameters.AddWithValue("@id", BlAdjustment.Id);
                adtp.Fill(dt);
            }
            return dt;
        }
        public int Update(clsBlAdjustment BlAdjustment)
        {
            int HowManyInsert = 0;
            using (SqlCommand cmd = new SqlCommand("usp_UpdateAdjustment", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", BlAdjustment.Id);
                cmd.Parameters.AddWithValue("@TransactionDate", BlAdjustment.TranTime);
                cmd.Parameters.AddWithValue("@ZoneId", BlAdjustment.ZoneId);
                cmd.Parameters.AddWithValue("@AdderssId", BlAdjustment.AddressId);
                cmd.Parameters.AddWithValue("@CustomerName", BlAdjustment.CustomerId);

                cmd.Parameters.AddWithValue("@Dr", BlAdjustment.Dr);
                cmd.Parameters.AddWithValue("@Cr", BlAdjustment.Cr);
                cmd.Parameters.AddWithValue("@Notes", BlAdjustment.Remark);
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

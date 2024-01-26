using AnajPos.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnajPos.BALVendor;

namespace AnajPos.DalVendor
{
    class clsDlVendorAdjustment
    {
        SqlConnection conn = new SqlConnection(clsConnectionString.cs);
        public int Insert(clsBlVendorAdjustment BlAdjustment)
        {
            int HowManyInsert = 0;
            using (SqlCommand cmd = new SqlCommand("usp_InsertVendorAdjustment", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", BlAdjustment.AdjustmentId);
                cmd.Parameters.AddWithValue("@date", BlAdjustment.AdjustmentDate);
                cmd.Parameters.AddWithValue("@vendorId", BlAdjustment.VendorId);
                cmd.Parameters.AddWithValue("@dr", BlAdjustment.Dr);
                cmd.Parameters.AddWithValue("@cr", BlAdjustment.Cr);
                cmd.Parameters.AddWithValue("@notes", BlAdjustment.Notes);
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
            using (SqlCommand cmd = new SqlCommand("select max(AdjustmentId) from tblVendorAdjustment", conn))
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
            using (SqlDataAdapter adtp = new SqlDataAdapter("usp_AdjustmentbyDate", conn))
            {
                adtp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adtp.SelectCommand.Parameters.AddWithValue("@date", ViewDate);
                adtp.Fill(dt);
            }
            return dt;
        }
        public DataTable ViewAdjustment()
        {
            DataTable dt = new DataTable();
            using (SqlDataAdapter adtp = new SqlDataAdapter("usp_VendorAdjustmentAll", conn))
            {
                adtp.SelectCommand.CommandType = CommandType.StoredProcedure;
                // adtp.SelectCommand.Parameters.AddWithValue("@date", BlAdjustment.TranTime);
                adtp.Fill(dt);
            }
            return dt;
        }
        public DataTable ViewAdjustmentById(clsBlVendorAdjustment BlAdjustment)
        {
            DataTable dt = new DataTable();
            using (SqlDataAdapter adtp = new SqlDataAdapter("usp_AdjustmentbyId", conn))
            {
                adtp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adtp.SelectCommand.Parameters.AddWithValue("@id", BlAdjustment.AdjustmentId);
                adtp.Fill(dt);
            }
            return dt;
        }
        public int Update(clsBlVendorAdjustment BlAdjustment)
        {
            int HowManyInsert = 0;
            using (SqlCommand cmd = new SqlCommand("usp_UpdateVendorAdjustment", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", BlAdjustment.AdjustmentId);
                cmd.Parameters.AddWithValue("@date", BlAdjustment.AdjustmentDate);
                cmd.Parameters.AddWithValue("@vendorId", BlAdjustment.VendorId);
                cmd.Parameters.AddWithValue("@dr", BlAdjustment.Dr);
                cmd.Parameters.AddWithValue("@cr", BlAdjustment.Cr);
                cmd.Parameters.AddWithValue("@notes", BlAdjustment.Notes);
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

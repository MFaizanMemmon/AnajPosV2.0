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
    class clsDlVendor
    {
        SqlConnection conn = new SqlConnection(clsConnectionString.cs);
        public int Insert(clsBlVendor BlVendor)
        {
            int HowManyInsert = 0;
            using (SqlCommand cmd = new SqlCommand("usp_InsertVendor", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@VendorId", BlVendor.VendorId);
                cmd.Parameters.AddWithValue("@VendorName", BlVendor.VendorName);
                cmd.Parameters.AddWithValue("@AddressId", BlVendor.AddressName);
                cmd.Parameters.AddWithValue("@ProperAddress", BlVendor.ProperAddress);
                cmd.Parameters.AddWithValue("@Picture", BlVendor.Image);
                cmd.Parameters.AddWithValue("@Phone1", BlVendor.Phone1);
                cmd.Parameters.AddWithValue("@Phone2", BlVendor.Phone2);
                cmd.Parameters.AddWithValue("@Telephone", BlVendor.Telephone);
                cmd.Parameters.AddWithValue("@Remark", BlVendor.Remark);
                cmd.Parameters.AddWithValue("@Date", BlVendor.ClosingDate);
                cmd.Parameters.AddWithValue("@OpeningAmount", BlVendor.OpeningAmount);
                cmd.Parameters.AddWithValue("@IsActive", BlVendor.IsActive);
                if (conn.State != ConnectionState.Open)
                {

                    conn.Open();
                }
                HowManyInsert = cmd.ExecuteNonQuery();

            }
            return HowManyInsert;

        }

        public DataTable View(clsBlVendor BlVendor)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter adpt = new SqlDataAdapter("usp_AllVendorByID", conn);
            adpt.SelectCommand.CommandType = CommandType.StoredProcedure;
            adpt.SelectCommand.Parameters.AddWithValue("@id", BlVendor.VendorId);
            adpt.Fill(dt);
            return dt;
        }

        public DataTable ViewByAddress(clsBlVendor BlVendor)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter adpt = new SqlDataAdapter("usp_ViewVendorByAddress", conn);
            adpt.SelectCommand.CommandType = CommandType.StoredProcedure;
            adpt.SelectCommand.Parameters.AddWithValue("@id", BlVendor.AddressName);
            adpt.Fill(dt);
            return dt;
        }

        public DataTable DisplayVendor()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter adpt = new SqlDataAdapter("usp_DisplayVendor", conn);
            adpt.SelectCommand.CommandType = CommandType.StoredProcedure;
            adpt.Fill(dt);
            return dt;
        }

        public int Update(clsBlVendor BlVendor)
        {
            int HowManyUpdate = 0;
            using (SqlCommand cmd = new SqlCommand("usp_UpdateVendor", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@VendorId", BlVendor.VendorId);
                cmd.Parameters.AddWithValue("@VendorName", BlVendor.VendorName);
                cmd.Parameters.AddWithValue("@AddressId", BlVendor.AddressName);
                cmd.Parameters.AddWithValue("@ProperAddress", BlVendor.ProperAddress);
                cmd.Parameters.AddWithValue("@Picture", BlVendor.Image);
                cmd.Parameters.AddWithValue("@Phone1", BlVendor.Phone1);
                cmd.Parameters.AddWithValue("@Phone2", BlVendor.Phone2);
                cmd.Parameters.AddWithValue("@Telephone", BlVendor.Telephone);
                cmd.Parameters.AddWithValue("@Remark", BlVendor.Remark);
                cmd.Parameters.AddWithValue("@Date", BlVendor.ClosingDate);
                cmd.Parameters.AddWithValue("@OpeningAmount", BlVendor.OpeningAmount);
                cmd.Parameters.AddWithValue("@IsActive", BlVendor.IsActive);

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
            using (SqlCommand cmd = new SqlCommand("select Min(VendorID) from tblVendor", conn))
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
            using (SqlCommand cmd = new SqlCommand("select max(VendorID) from tblVendor", conn))
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
            using (SqlCommand cmd = new SqlCommand("select max(VendorID) from tblVendor", conn))
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

        public DateTime GetClosingDate(int VendorID)
        {
            DateTime ClosingDate = DateTime.Now;
            string query = string.Format("select ClosingDate from tblVendor where VendorId = {0}", VendorID);
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                ClosingDate = (DateTime)cmd.ExecuteScalar();
            }

            return ClosingDate;
        }

        public int VendorLastBalance(int VendorId)
        {
            int Amount = 0;
            using (SqlCommand cmd = new SqlCommand("usp_VendorBalance", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", VendorId);
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                Amount = (int)cmd.ExecuteScalar();
                conn.Close();
            }
            return Amount;
        }

        public int VendorAccountId(int VendorId)
        {
            int Amount = 0;
            using (SqlCommand cmd = new SqlCommand("usp_VendorAccountId", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", VendorId);
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

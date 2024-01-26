using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnajPos.BAL;

namespace AnajPos.DAL
{
    class clsAddress
    {
        SqlConnection conn = new SqlConnection(clsConnectionString.cs);
        public int Insert(clsBlAddress BlAddress)
        {
            int HowManyInsert = 0;
            using (SqlCommand cmd = new SqlCommand("usp_tblAddressInsert", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@AddressName", BlAddress.AddressName);
                cmd.Parameters.AddWithValue("@AddressNameUrdu", BlAddress.AddressNameUrdu);
                cmd.Parameters.AddWithValue("@zoneId", BlAddress.ZoneId);
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

            using (SqlCommand cmd = new SqlCommand("usp_tblAddressView", conn))
            {
                SqlDataAdapter adpt = new SqlDataAdapter(cmd);
                adpt.Fill(dt);
            }
            return dt;
        }

        public int Update(clsBlAddress BlAddress)
        {
            int HowManyUpdate = 0;
            using (SqlCommand cmd = new SqlCommand("usp_tblAddressUpdate", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@AddressId", BlAddress.AddressId);
                cmd.Parameters.AddWithValue("@AddressName", BlAddress.AddressName);
                cmd.Parameters.AddWithValue("@AddressNameUrdu",BlAddress.AddressNameUrdu);
                cmd.Parameters.AddWithValue("@ZoneId", BlAddress.ZoneId);
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                HowManyUpdate = cmd.ExecuteNonQuery();
            }
            return HowManyUpdate;
        }
        public DataTable ViewAddressByZone(clsBlAddress BlAddress)
        {
            DataTable dt = new DataTable();
            string query = string.Format("select * from tblAddress where ZoneId ={0} order by AddressName asc",BlAddress.AddressId);
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                SqlDataAdapter adpt = new SqlDataAdapter(cmd);
                adpt.Fill(dt);
            }
            return dt;
        }

    }
}

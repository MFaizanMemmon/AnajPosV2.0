using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnajPos.BAL;
using System.Data.SqlClient;
using System.Data;

namespace AnajPos.DAL
{
    class clsZone
    {
        SqlConnection conn = new SqlConnection(clsConnectionString.cs);
        public int InsertZone(clsBlZone BlZone)
        {
            int HowManyInsert = 0;
            using (SqlCommand cmd = new SqlCommand("usp_tblZoneInsert", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@zone", BlZone.ZoneName);
                cmd.Parameters.AddWithValue("@zoneUrdu", BlZone.ZoneNameUrdu);
                if (conn.State!=ConnectionState.Open)
                {
                    conn.Open();
                }
                HowManyInsert = cmd.ExecuteNonQuery();
   
            }
            return HowManyInsert;

        }

        public DataTable ViewZone()
        {
            DataTable dt = new DataTable();

            using (SqlCommand cmd = new SqlCommand("usp_tblZoneView", conn))
            {
                SqlDataAdapter adpt = new SqlDataAdapter(cmd);
                adpt.Fill(dt);
            }
            return dt;
        }

        public int UpdateZone(clsBlZone BlZone)
        {
            int HowManyUpdate = 0;
            using (SqlCommand cmd=new SqlCommand("usp_tblZoneUpdate",conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", BlZone.ZoneId);
                cmd.Parameters.AddWithValue("@zoneName", BlZone.ZoneName);
                cmd.Parameters.AddWithValue("@zoneNameUrdu", BlZone.ZoneNameUrdu);
                if (conn.State !=ConnectionState.Open)
                {
                    conn.Open();
                }
                HowManyUpdate = cmd.ExecuteNonQuery();
            }
            return HowManyUpdate;
        }

    }
}

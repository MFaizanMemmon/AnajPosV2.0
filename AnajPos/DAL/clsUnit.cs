using AnajPos.BAL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnajPos.DAL
{
    class clsUnit
    {
        SqlConnection conn = new SqlConnection(clsConnectionString.cs);
        public int Insert(clsBlUnit blUnit)
        {
            int HowManyInsert = 0;
            using (SqlCommand cmd = new SqlCommand("usp_InsertUnit", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UnitName", blUnit.UnitName);
                //cmd.Parameters.AddWithValue("@UnitNameInUrdu", blUnit.UnitNameInUrdu);

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

            using (SqlCommand cmd = new SqlCommand("usp_GetUnit", conn))
            {
                SqlDataAdapter adpt = new SqlDataAdapter(cmd);
                adpt.Fill(dt);
            }
            return dt;
        }

        public int Update(clsBlUnit blUnit)
        {
            int HowManyUpdate = 0;
            using (SqlCommand cmd = new SqlCommand("usp_UpdateUnit", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UnitId", blUnit.UnitId);
                cmd.Parameters.AddWithValue("@UnitName", blUnit.UnitName);

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

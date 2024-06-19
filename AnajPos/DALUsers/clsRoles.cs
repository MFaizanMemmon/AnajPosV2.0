using AnajPos.BAL;
using AnajPos.BALUsers;
using AnajPos.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnajPos.DALUsers
{
    internal class clsRoles
    {
        SqlConnection conn = new SqlConnection(clsConnectionString.cs);
        public int Insert(clsBLRoles BlRole)
        {
            int HowManyInsert = 0;
            using (SqlCommand cmd = new SqlCommand("usp_InsertRole", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RoleName", BlRole.RoleName);
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

            using (SqlCommand cmd = new SqlCommand("ups_GetRole", conn))
            {
                SqlDataAdapter adpt = new SqlDataAdapter(cmd);
                adpt.Fill(dt);
            }
            return dt;
        }
    }
}

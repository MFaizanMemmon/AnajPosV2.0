using AnajPos.BALUsers;
using AnajPos.DAL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnajPos.DALUsers
{
    internal class clsUsers
    {
        SqlConnection conn = new SqlConnection(clsConnectionString.cs);
        public int Insert(clsBLUsers BLUsers)
        {
            int HowManyInsert = 0;
            using (SqlCommand cmd = new SqlCommand("ups_InsertUserData", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserName", BLUsers.UserName);
                cmd.Parameters.AddWithValue("@Passward", BLUsers.Phone);
                cmd.Parameters.AddWithValue("@Email", BLUsers.UserEmail);

                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                HowManyInsert = cmd.ExecuteNonQuery();

            }
            return HowManyInsert;

        }
        //public DataTable View()
        //{
        //    DataTable dt = new DataTable();

        //    using (SqlCommand cmd = new SqlCommand("ups_GetRole", conn))
        //    {
        //        SqlDataAdapter adpt = new SqlDataAdapter(cmd);
        //        adpt.Fill(dt);
        //    }
        //    return dt;
        //}
    }
}

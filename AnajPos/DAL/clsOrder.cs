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
    class clsOrder
    {
        SqlConnection conn = new SqlConnection(clsConnectionString.cs);
        public int Insert(clsBlOrder BlOrder)
        {
            int HowManyInsert = 0;
            using (SqlCommand cmd = new SqlCommand("usp_InsertOrder", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@OrderId", BlOrder.OrderID);
                cmd.Parameters.AddWithValue("@OrderDate", BlOrder.OrderDate);
                cmd.Parameters.AddWithValue("@ZoneId", BlOrder.ZoneId);
                cmd.Parameters.AddWithValue("@AddressId", BlOrder.AddressId);
                cmd.Parameters.AddWithValue("@CustomerID", BlOrder.CustomerID);
                cmd.Parameters.AddWithValue("@pageNo", BlOrder.PageNo);
                cmd.Parameters.AddWithValue("@Bilty", BlOrder.Bilty);
                cmd.Parameters.AddWithValue("@Remark", BlOrder.Remark);
                cmd.Parameters.AddWithValue("@Transport", BlOrder.Transport);
                cmd.Parameters.AddWithValue("@EntryType", BlOrder.EntryType);
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

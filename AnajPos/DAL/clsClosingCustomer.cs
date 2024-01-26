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
    class clsClosingCustomer
    {
        SqlConnection conn = new SqlConnection(clsConnectionString.cs);
        public int Insert(clsBlClosingCustomer BlClosing)
        {
            int HowManyInsert = 0;
            using (SqlCommand cmd = new SqlCommand("usp_InsertClosing", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ClosingDate",BlClosing.ClosingDate);
                cmd.Parameters.AddWithValue("@CustomerID", BlClosing.CustomerId);
                cmd.Parameters.AddWithValue("@Remark", BlClosing.Remark);
                cmd.Parameters.AddWithValue("@Amount", BlClosing.Amount);
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                HowManyInsert = cmd.ExecuteNonQuery();

            }
            return HowManyInsert;

        }
        public DateTime GetClosing(int CustomerId)
        {
            DateTime LastClosing;
            string q = string.Format("select max(ClosingDate) from tblClosing where CustomerId = '{0}'",CustomerId);
            using (SqlCommand cmd=new SqlCommand(q,conn))
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                LastClosing = (DateTime)cmd.ExecuteScalar();
            }
            return LastClosing;
        }
    }
}

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
    class clsCustomer
    {
        SqlConnection conn = new SqlConnection(clsConnectionString.cs);
        public int Insert(clsBlCustomer BlCustomer)
        {
            int HowManyInsert = 0;
            using (SqlCommand cmd = new SqlCommand("usp_InsertCustomer", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CustomerID", BlCustomer.CustomerID);
                cmd.Parameters.AddWithValue("@CustomerName", BlCustomer.CustomerName);
                cmd.Parameters.AddWithValue("@CustomerNameUrdu", BlCustomer.CustomerNameUrdu);
                cmd.Parameters.AddWithValue("@ZoneID", BlCustomer.ZoneId);
                cmd.Parameters.AddWithValue("@AddressId", BlCustomer.AddressId);
                cmd.Parameters.AddWithValue("@ProperAddress",BlCustomer.ProperAddress);
                cmd.Parameters.AddWithValue("@Picture", BlCustomer.Picture);
                cmd.Parameters.AddWithValue("@Phone1", BlCustomer.Phone1);
                cmd.Parameters.AddWithValue("@Phone2", BlCustomer.Phone2);
                cmd.Parameters.AddWithValue("@Telephone", BlCustomer.Telephone);
                cmd.Parameters.AddWithValue("@Remark", BlCustomer.Remark);
                cmd.Parameters.AddWithValue("@ClosingDate", BlCustomer.ClosingDate);
                cmd.Parameters.AddWithValue("@OpeningAmount", BlCustomer.OpeningAmount);
                cmd.Parameters.AddWithValue("@IsActice", BlCustomer.IsActive);
                if (conn.State != ConnectionState.Open)
                {

                    conn.Open();
                }
                HowManyInsert = cmd.ExecuteNonQuery();

            }
            return HowManyInsert;

        }

        public DataTable View(clsBlCustomer BlCustomer)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter adpt = new SqlDataAdapter("usp_ViewCustomerAllbyId", conn);
            adpt.SelectCommand.CommandType = CommandType.StoredProcedure;
            adpt.SelectCommand.Parameters.AddWithValue("@id", BlCustomer.CustomerID);
            adpt.Fill(dt);
            return dt;
        }

        public DataTable ViewByAddress(clsBlCustomer BlCustomer)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter adpt = new SqlDataAdapter("usp_ViewCustomerAllbyAddress", conn);
            adpt.SelectCommand.CommandType = CommandType.StoredProcedure;
            adpt.SelectCommand.Parameters.AddWithValue("@id", BlCustomer.AddressId);
            adpt.Fill(dt);
            return dt;
       
 
        }

        public DataTable DisplayCustomer()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter adpt = new SqlDataAdapter("usp_DisplayCustomer", conn);
            adpt.SelectCommand.CommandType = CommandType.StoredProcedure;
            adpt.Fill(dt);
            return dt;
        }

        public int Update(clsBlCustomer BlCustomer)
        {
            int HowManyUpdate = 0;
            using (SqlCommand cmd = new SqlCommand("usp_UpdateCustomer", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CustomerID", BlCustomer.CustomerID);
                cmd.Parameters.AddWithValue("@CustomerName", BlCustomer.CustomerName);
                cmd.Parameters.AddWithValue("@CustomerNameUrdu", BlCustomer.CustomerNameUrdu);
                cmd.Parameters.AddWithValue("@ZoneID", BlCustomer.ZoneId);
                cmd.Parameters.AddWithValue("@AddressId", BlCustomer.AddressId);
                cmd.Parameters.AddWithValue("@ProperAddress", BlCustomer.ProperAddress);
                cmd.Parameters.AddWithValue("@Picture", BlCustomer.Picture);
                cmd.Parameters.AddWithValue("@Phone1", BlCustomer.Phone1);
                cmd.Parameters.AddWithValue("@Phone2", BlCustomer.Phone2);
                cmd.Parameters.AddWithValue("@Telephone", BlCustomer.Telephone);
                cmd.Parameters.AddWithValue("@Remark", BlCustomer.Remark);
                cmd.Parameters.AddWithValue("@ClosingDate", BlCustomer.ClosingDate);
                cmd.Parameters.AddWithValue("@OpeningAmount", BlCustomer.OpeningAmount);
                cmd.Parameters.AddWithValue("@IsActice", BlCustomer.IsActive);
                
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
            using (SqlCommand cmd=new SqlCommand("select Min(CustomerId) from tblCustomer",conn))
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                try
                {
                    minId = (int)cmd.ExecuteScalar();
                
                }
            
                catch {

                    minId = 0;
                }
               
            }
            return minId;
        }

        public int MaxId()
        {
            int max = 0;
            using (SqlCommand cmd = new SqlCommand("select max(CustomerId) from tblCustomer", conn))
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
            using (SqlCommand cmd=new SqlCommand("select max(CustomerId) from tblCustomer ",conn))
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

        public DateTime GetClosingDate(int CustomerId)
        {
            DateTime ClosingDate = DateTime.Now;
            string query = string.Format("select ClosingDate from tblCustomer where CustomerId = {0}", CustomerId);
            using (SqlCommand cmd=new SqlCommand(query,conn))
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                ClosingDate = (DateTime)cmd.ExecuteScalar();
            }

            return ClosingDate;
        }
        
        public int CustomerLastBalance(int CustomerId)
        {
            int Amount = 0;
            using (SqlCommand cmd = new SqlCommand("usp_CustomeBalance", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", CustomerId);
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                Amount = (int)cmd.ExecuteScalar();
                conn.Close();
            }
            return Amount;
        }

        public int CustomerAccountId(int CustomerId)
        {
            int Amount = 0;
            using (SqlCommand cmd = new SqlCommand("usp_CustomerAccountId", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", CustomerId);
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

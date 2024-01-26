using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using AnajPos.BAL;
using AnajPos.DAL;
using System.Data;
using AnajPos.BALVendor;

namespace AnajPos.DAL
{
    class clsSaleReturn
    {
        SqlConnection conn = new SqlConnection(clsConnectionString.cs);
        public int InsertReturn(clsBlSaleReturn SaleR)
        {
            int HowManyInsert = 0;
            using (SqlConnection conn = new SqlConnection(clsConnectionString.cs))
            {
                using (SqlCommand cmd = new SqlCommand("usp_InsertReturn", conn))
                {
                   
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ReturnId", SaleR.ReturnId);
                    cmd.Parameters.AddWithValue("@Zoneid", SaleR.ZoneId);
                    cmd.Parameters.AddWithValue("@Addressid", SaleR.AddressId);
                    cmd.Parameters.AddWithValue("@CustomerId", SaleR.CustomerId);
                    cmd.Parameters.AddWithValue("@ReturnDate", SaleR.ReturnDate);
                    cmd.Parameters.AddWithValue("@Remark", SaleR.Remark);
                    cmd.Parameters.AddWithValue("@Amount", SaleR.Amount);
                    if (conn.State != ConnectionState.Open)
                    {
                        conn.Open();
                    }
                    HowManyInsert = cmd.ExecuteNonQuery();
                }

            }
            return HowManyInsert;
        }

        public int InsertReturnProduct(clsBlReturnProcduct ReturnProduct)
        {
            int HowManyInsert = 0;
            using (SqlConnection conn = new SqlConnection(clsConnectionString.cs))
            {
                using (SqlCommand cmd = new SqlCommand("usp_InsertReturnProduct", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ReturnId", ReturnProduct.ReturnId);
                    cmd.Parameters.AddWithValue("@ReturnDate", ReturnProduct.ReturnDate);
                    cmd.Parameters.AddWithValue("@CustomerVendorAccountId", ReturnProduct.CustomerVendorAccountId);
                    cmd.Parameters.AddWithValue("@ProductId", ReturnProduct.ProductId);
                    cmd.Parameters.AddWithValue("@Qty", ReturnProduct.Qty);
                    cmd.Parameters.AddWithValue("@ProductName", ReturnProduct.ProductName);
                    cmd.Parameters.AddWithValue("@Kg", ReturnProduct.Kg);
                    cmd.Parameters.AddWithValue("@Weight", ReturnProduct.Weight);
                    cmd.Parameters.AddWithValue("@Price", ReturnProduct.Price);
                    cmd.Parameters.AddWithValue("@Amount", ReturnProduct.Amount);
                    cmd.Parameters.AddWithValue("@Type", ReturnProduct.Type);

                    if (conn.State != ConnectionState.Open)
                    {
                        conn.Open();
                    }
                    HowManyInsert = cmd.ExecuteNonQuery();
                }

            }
            return HowManyInsert;
        }

        public DataTable GetReturndetail(int ReturnId, string ReturnType)
        {
            DataTable dt = new DataTable();

            using (SqlCommand cmd = new SqlCommand("usp_ReturnDetail", conn))
            {

                SqlDataAdapter adpt = new SqlDataAdapter(cmd);
                adpt.SelectCommand.CommandType = CommandType.StoredProcedure;
                adpt.SelectCommand.Parameters.AddWithValue("@ReturnId", ReturnId);
                adpt.SelectCommand.Parameters.AddWithValue("@TransactionType", ReturnType);
                adpt.Fill(dt);
            }
            return dt;
        }
        public int NewId()
        {
            int newId = 0;
            using (SqlConnection conn = new SqlConnection(clsConnectionString.cs))
            {
                using (SqlCommand cmd = new SqlCommand("select max(ReturnID) from tblSaleR", conn))
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
                        newId = 1001;
                    }

                }
            }
            return newId;
        }
    }
}

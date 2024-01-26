using AnajPos.BAL;
using AnajPos.BALVendor;
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
    class clsDlPurchaseReturn
    {
        public int InsertReturn(clsBlPurchaseReturn PurchaseR)
        {
            int HowManyInsert = 0;
            using (SqlConnection conn = new SqlConnection(clsConnectionString.cs))
            {
                using (SqlCommand cmd = new SqlCommand("usp_InsertPurchaseR", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@rid", PurchaseR.ReturnId);
                    cmd.Parameters.AddWithValue("@ZoneId", PurchaseR.ZoneId);
                    cmd.Parameters.AddWithValue("@AreaId", PurchaseR.AreaId);
                    cmd.Parameters.AddWithValue("@vid", PurchaseR.VendorId);
                    cmd.Parameters.AddWithValue("@date", PurchaseR.ReturnDate);
                    cmd.Parameters.AddWithValue("@remark", PurchaseR.Remark);
                    cmd.Parameters.AddWithValue("@amount", PurchaseR.Amount);
                    cmd.Parameters.AddWithValue("@isLedger",PurchaseR.IsLedger);
                    if (conn.State != ConnectionState.Open)
                    {
                        conn.Open();
                    }
                    HowManyInsert = cmd.ExecuteNonQuery();
                }

            }
            return HowManyInsert;
        }
        public int NewId()
        {
            int newId = 0;
            using (SqlConnection conn = new SqlConnection(clsConnectionString.cs))
            {
                using (SqlCommand cmd = new SqlCommand("select max(ReturnID) from tblPurchaseR", conn))
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

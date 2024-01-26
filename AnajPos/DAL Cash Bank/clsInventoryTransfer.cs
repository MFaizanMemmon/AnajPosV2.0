using AnajPos.BAL_Cash_BANK;
using AnajPos.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnajPos.DAL_Cash_Bank
{
    class clsInventoryTransfer
    {
        SqlConnection conn = new SqlConnection(clsConnectionString.cs);
        public int Insert(clsBlinventoryTransfer blinventoryTransfer)
        {
            int HowManyInsert = 0;
            using (SqlCommand cmd = new SqlCommand("InsertUpdateInventoryTransfer", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@InventoryTransferId", blinventoryTransfer.InventoryTransferID);
                cmd.Parameters.AddWithValue("@TransferDate", blinventoryTransfer.TransferDate);
                cmd.Parameters.AddWithValue("@ToLocationID", blinventoryTransfer.ToLocationID);
                cmd.Parameters.AddWithValue("@FromLocationID", blinventoryTransfer.FromLocationID);
                cmd.Parameters.AddWithValue("@ProductId", blinventoryTransfer.ProductId);
                cmd.Parameters.AddWithValue("@TransferQty ", blinventoryTransfer.transferQty);
                cmd.Parameters.AddWithValue("@Notes", blinventoryTransfer.Notes);

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

            using (SqlCommand cmd = new SqlCommand("usp_viewInvertorytransfer", conn))
            {
                SqlDataAdapter adpt = new SqlDataAdapter(cmd);
                adpt.Fill(dt);
            }
            return dt;
        }
        public DataTable ViewTransferByDate(DateTime transferDate)
        {
            DataTable dt = new DataTable();

            using (SqlCommand cmd = new SqlCommand("usp_ViewInventoryTransferByDate", conn))
            {
                SqlDataAdapter adpt = new SqlDataAdapter(cmd);
                adpt.SelectCommand.CommandType = CommandType.StoredProcedure;
                adpt.SelectCommand.Parameters.AddWithValue("@TransferDate", transferDate);
                adpt.Fill(dt);
            }
          
            return dt;
        }
       
        public int update(clsBlinventoryTransfer blinventoryTransfer)
        {
            int HowManyInsert = 0;
            using (SqlCommand cmd = new SqlCommand("InsertUpdateInventoryTransfer", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@InventoryTransferId", blinventoryTransfer.InventoryTransferID);
                cmd.Parameters.AddWithValue("@TransferDate", blinventoryTransfer.TransferDate);
                cmd.Parameters.AddWithValue("@ToLocationID", blinventoryTransfer.ToLocationID);
                cmd.Parameters.AddWithValue("@FromLocationID", blinventoryTransfer.FromLocationID);
                cmd.Parameters.AddWithValue("@ProductId", blinventoryTransfer.ProductId);
                cmd.Parameters.AddWithValue("@TransferQty ", blinventoryTransfer.transferQty);
                cmd.Parameters.AddWithValue("@Notes", blinventoryTransfer.Notes);

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

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
    class clsStock
    {
        SqlConnection conn = new SqlConnection(clsConnectionString.cs);
        public int Insert(clsBlStock BlStock)
        {
            int HowManyInsert = 0;
            using (SqlCommand cmd = new SqlCommand("usp_InsertStock", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProductTransDate",BlStock.ProductTransDate);
                cmd.Parameters.AddWithValue("@ProductTransId",BlStock.ProductTransId);
                cmd.Parameters.AddWithValue("@ProductID",BlStock.ProductId);
                cmd.Parameters.AddWithValue("@UnitID", BlStock.UnitID);
                cmd.Parameters.AddWithValue("@TransactionType", BlStock.TransactionType);
                cmd.Parameters.AddWithValue("@WarehouseID", BlStock.WarehouseId);
                cmd.Parameters.AddWithValue("@StockIn", BlStock.StockIn);
                cmd.Parameters.AddWithValue("@StockOut",BlStock.StockOut);
                cmd.Parameters.AddWithValue("@Amount", BlStock.Amount);
                cmd.Parameters.AddWithValue("@IsDeleted", BlStock.IsDeleted);
               
                if (conn.State != ConnectionState.Open)
                {

                    conn.Open();
                }
                HowManyInsert = cmd.ExecuteNonQuery();

            }
            return HowManyInsert;

        }

        public int Delete(clsBlStock BlStock)
        {
            int HowManyDelete = 0;
            using (SqlCommand cmd = new SqlCommand("usp_DeleteStock", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
              
                cmd.Parameters.AddWithValue("@ProductTransId", BlStock.ProductTransId);
                cmd.Parameters.AddWithValue("@ProductID", BlStock.ProductId);
                cmd.Parameters.AddWithValue("@TransactionType",BlStock.TransactionType);
               
                if (conn.State != ConnectionState.Open)
                {

                    conn.Open();
                }
                HowManyDelete = cmd.ExecuteNonQuery();

            }
            return HowManyDelete;

        }

        public int GetStockByProductIdAndWarehouseId(int ProductId,int WarehouseId)
        {
            int Amount = 0;
            using (SqlCommand cmd = new SqlCommand("usp_GetStockByProductAndLocationId", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProductId", ProductId);
                cmd.Parameters.AddWithValue("@LocationId", WarehouseId);
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

        public DataTable GetAllStock(int WarehouseId)
        {
            DataTable dt = new DataTable();

            using (SqlCommand cmd = new SqlCommand("usp_GetStock", conn))
            {

                SqlDataAdapter adpt = new SqlDataAdapter(cmd);
                adpt.SelectCommand.CommandType = CommandType.StoredProcedure;
                adpt.SelectCommand.Parameters.AddWithValue("@Warehouseid", WarehouseId);
                adpt.Fill(dt);
            }
            return dt;
        }

        public DataTable GetAllStockByWarehouseId(int WarehouseId)
        {
            DataTable dt = new DataTable();

            using (SqlCommand cmd = new SqlCommand("usp_GetStockByWarehouseId", conn))
            {

                SqlDataAdapter adpt = new SqlDataAdapter(cmd);
                adpt.SelectCommand.CommandType = CommandType.StoredProcedure;
                adpt.SelectCommand.Parameters.AddWithValue("@Warehouseid", WarehouseId);
                adpt.Fill(dt);
            }
            return dt;
        }
        public DataTable GetStockLedger(int ProductId,int WarehouseId)
        {
            DataTable dt = new DataTable();

            using (SqlCommand cmd = new SqlCommand("usp_StockLedger", conn))
            {

                SqlDataAdapter adpt = new SqlDataAdapter(cmd);
                adpt.SelectCommand.CommandType = CommandType.StoredProcedure;
                adpt.SelectCommand.Parameters.AddWithValue("@ProductId", ProductId);
                adpt.SelectCommand.Parameters.AddWithValue("@WarehouseId", WarehouseId);
                adpt.Fill(dt);
            }
            return dt;
        }


        //_____________ SAQIB ___________ 1/13/2024
        public DataTable GetStockView()
        {
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand("usp_tblStockView", conn))
            {
                SqlDataAdapter adpt = new SqlDataAdapter(cmd);
                adpt.SelectCommand.CommandType = CommandType.StoredProcedure;
                adpt.Fill(dt);
            }
            return dt;
        }

        public clsBlStock GetStockByStockId(int stockId)
        {
            clsBlStock stock = new clsBlStock();


            conn.Open(); // Open the connection
            using (SqlCommand cmd = new SqlCommand("usp_getStockById", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StockId", stockId);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // Map the columns from the SqlDataReader to the properties of clsBlStock
                        stock.StockId = Convert.IsDBNull(reader["StockId"]) ? 0 : Convert.ToInt32(reader["StockId"]);
                        stock.ProductId = Convert.IsDBNull(reader["ProductId"]) ? 0 : Convert.ToInt32(reader["ProductId"]);
                        stock.ProductName = Convert.IsDBNull(reader["ProductName"]) ? "" : reader["ProductName"].ToString();
                        stock.WarehouseName = Convert.IsDBNull(reader["WarehouseName"]) ? "" : reader["WarehouseName"].ToString();
                        stock.WarehouseId = Convert.IsDBNull(reader["WarehouseId"]) ? 0 : Convert.ToInt32(reader["WarehouseId"]);
                        stock.StockIn = Convert.IsDBNull(reader["StockIn"]) ? 0 : Convert.ToDecimal(reader["StockIn"]);
                        stock.StockOut = Convert.IsDBNull(reader["StockOut"]) ? 0 : Convert.ToDecimal(reader["StockOut"]);
                    }
                }
            }
            conn.Close();

            return stock;
        }

        public int Update(clsBlStock bl)
        {
            int HowManyUpdate = 0;
            using (SqlCommand cmd = new SqlCommand("usp_tblUpdateStock", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StockId", bl.StockId);
                cmd.Parameters.AddWithValue("@ProductID", bl.ProductId);
                cmd.Parameters.AddWithValue("@WarehouseID", bl.WarehouseId);
                cmd.Parameters.AddWithValue("@StockIn", bl.StockId);
                cmd.Parameters.AddWithValue("@StockOut", bl.StockOut);
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                HowManyUpdate = cmd.ExecuteNonQuery();
            }
            return HowManyUpdate;
        }

        public int InsertAdjustment(clsBlStock BlStock)
        {
            int HowManyInsert = 0;
            using (SqlCommand cmd = new SqlCommand("usp_InsertStock", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProductTransDate", BlStock.ProductTransDate);
                cmd.Parameters.AddWithValue("@ProductTransId", BlStock.ProductTransId);
                cmd.Parameters.AddWithValue("@ProductID", BlStock.ProductId);
                cmd.Parameters.AddWithValue("@UnitID", BlStock.UnitID);
                cmd.Parameters.AddWithValue("@TransactionType", BlStock.TransactionType);
                cmd.Parameters.AddWithValue("@WarehouseID", BlStock.WarehouseId);
                cmd.Parameters.AddWithValue("@StockIn", BlStock.StockIn);
                cmd.Parameters.AddWithValue("@StockOut", BlStock.StockOut);
                cmd.Parameters.AddWithValue("@Amount", BlStock.Amount);
                cmd.Parameters.AddWithValue("@IsDeleted", BlStock.IsDeleted);

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using AnajPos.BAL;
using System.Data;

namespace AnajPos.DAL
{
    class clsProduct
    {
        SqlConnection conn = new SqlConnection(clsConnectionString.cs);
        public int Insert(clsBlProduct BlProduct)
        {
            int HowManyInsert = 0;
            using (SqlCommand cmd = new SqlCommand("usp_InsertProduct", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProductID",BlProduct.ProductId);
                cmd.Parameters.AddWithValue("@ProductName", BlProduct.ProductName);
                cmd.Parameters.AddWithValue("@ProductNameUrdu",BlProduct.ProductNameUrdu);
                cmd.Parameters.AddWithValue("@Company",BlProduct.Company);
                cmd.Parameters.AddWithValue("@ProductCetegory", BlProduct.Cetegory);
                cmd.Parameters.AddWithValue("@Pakaging", BlProduct.Pakaging);
                cmd.Parameters.AddWithValue("@PurchaseRate",BlProduct.PurchaseRate);
                cmd.Parameters.AddWithValue("@SaleRate",BlProduct.SaleRate);
                //cmd.Parameters.AddWithValue("@OpeningDate",BlProduct.OeningDate);
                cmd.Parameters.AddWithValue("@OpeningAmount",BlProduct.OpeningStock);
                cmd.Parameters.AddWithValue("@Remark", BlProduct.Remark);
                cmd.Parameters.AddWithValue("@WarehouseId", BlProduct.WarehouseId);
                cmd.Parameters.AddWithValue("@PurchaseUnitId", BlProduct.PurchaseUnitId);
                cmd.Parameters.AddWithValue("@SaleUnitId", BlProduct.SaleUnitId);
                cmd.Parameters.AddWithValue("@MeasurementProduct", BlProduct.MeasurementProduct);
                cmd.Parameters.AddWithValue("@OpeningStock", BlProduct.OpeningStock);
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                HowManyInsert = cmd.ExecuteNonQuery();
                conn.Close();
            }
            return HowManyInsert;
        }
        public int MaxId()
        {
            int _maxId = 0;
            using (SqlCommand cmd=new SqlCommand("select Max(ProductId) from tblProduct",conn))
            {
               
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                try
                {
                    _maxId = (int)cmd.ExecuteScalar();
                }
                catch { _maxId = 0; }
                conn.Close();
            }
            return _maxId;
        }
        public int MinId()
        {
            int _MinId;
            using (SqlCommand cmd=new SqlCommand("Select Min(ProductId) from tblProduct",conn))
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                try
                {
                    _MinId = (int)cmd.ExecuteScalar();
                }
                catch 
                {
                    _MinId = 0;
                }
            }
            return _MinId;
        }
        public DataTable GetAllData(int Id)
        {
            DataTable dt = new DataTable();
            string query = string.Format("select * from tblProduct where ProductId = {0}", Id);
            using (SqlDataAdapter adpt=new SqlDataAdapter(query,conn))
            {
                adpt.Fill(dt);
            }
            return dt;
        }
        public int Update(clsBlProduct BlProduct)
        {
            int HowManyInsert = 0;
            using (SqlCommand cmd = new SqlCommand("usp_UpdateProduct", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProductID", BlProduct.ProductId);
                cmd.Parameters.AddWithValue("@ProductName", BlProduct.ProductName);
                cmd.Parameters.AddWithValue("@ProductNameUrdu", BlProduct.ProductNameUrdu);
                cmd.Parameters.AddWithValue("@Company", BlProduct.Company);
                cmd.Parameters.AddWithValue("@ProductCetegory", BlProduct.Cetegory);
                cmd.Parameters.AddWithValue("@Pakaging", BlProduct.Pakaging);
                cmd.Parameters.AddWithValue("@PurchaseRate", BlProduct.PurchaseRate);
                cmd.Parameters.AddWithValue("@SaleRate", BlProduct.SaleRate);
                //cmd.Parameters.AddWithValue("@OpeningDate",BlProduct.OeningDate);
                cmd.Parameters.AddWithValue("@OpeningAmount", BlProduct.OpeningStock);
                cmd.Parameters.AddWithValue("@Remark", BlProduct.Remark);
                cmd.Parameters.AddWithValue("@WarehouseId", BlProduct.WarehouseId);
                cmd.Parameters.AddWithValue("@PurchaseUnitId", BlProduct.PurchaseUnitId);
                cmd.Parameters.AddWithValue("@SaleUnitId", BlProduct.SaleUnitId);
                cmd.Parameters.AddWithValue("@MeasurementProduct", BlProduct.MeasurementProduct);
                cmd.Parameters.AddWithValue("@OpeningStock", BlProduct.OpeningStock);
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                HowManyInsert = cmd.ExecuteNonQuery();
                conn.Close();
            }
            return HowManyInsert;
        }
        public DataTable ViewProduct()
        {
            DataTable dt = new DataTable();
            string query = string.Format("select ProductId,ProductName,ProductNameUrdu,ProductCetegory,Pakaging from tblProduct");
            using (SqlDataAdapter adpt = new SqlDataAdapter(query, conn))
            {
                adpt.Fill(dt);
            }
            return dt;
        }
        public DataTable ViewProductSearch(string searchText)
        {
            DataTable dt = new DataTable();
            string query = string.Format("select ProductId,ProductName,ProductNameUrdu,ProductCetegory,Pakaging from tblProduct where ProductName like '%{0}%'",searchText);
            using (SqlDataAdapter adpt = new SqlDataAdapter(query, conn))
            {
                adpt.Fill(dt);
            }
            return dt;
        }
        public DataTable GetAllProduct()
        {
            DataTable dt = new DataTable();
            string query = string.Format("select ProductId,ProductName,ProductNameUrdu,Pakaging from tblProduct order by ProductName asc");
            using (SqlDataAdapter adpt = new SqlDataAdapter(query, conn))
            {
                adpt.Fill(dt);
            }
            return dt;
        }

        public DataTable GetAllUnit()
        {
            DataTable dt = new DataTable();
      
            using (SqlDataAdapter adpt = new SqlDataAdapter("select * from tblUnit order by UnitName asc", conn))
            {
                adpt.Fill(dt);
            }
            return dt;
        }

    }
}

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
    class clsExpense
    {
        SqlConnection conn = new SqlConnection(clsConnectionString.cs);
        public int Insert(clsBlExpense BlExpense)
        {
            int HowManyInsert = 0;
            using (SqlCommand cmd = new SqlCommand("InsertUpdateExpense", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ExpenseId", BlExpense.ExpenseId);
                cmd.Parameters.AddWithValue("@ExpDate",BlExpense.ExpenseDate);
                cmd.Parameters.AddWithValue("@ExpenseType", BlExpense.ExpenseTypeID);
                cmd.Parameters.AddWithValue("@PaymentMode", BlExpense.PaymentModeID);
                cmd.Parameters.AddWithValue("@Amount", BlExpense.Amount);
                cmd.Parameters.AddWithValue("@Notes", BlExpense.Notes);

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

            using (SqlCommand cmd = new SqlCommand("usp_tBlExpenseView", conn))
            {
                SqlDataAdapter adpt = new SqlDataAdapter(cmd);
                adpt.Fill(dt);
            }
            return dt;
        }
        public DataTable ViewExpenseByDate(DateTime ExpDate)
        {
            DataTable dt = new DataTable();

            using (SqlCommand cmd = new SqlCommand("usp_ViewExpenseByDate", conn))
            {
                SqlDataAdapter adpt = new SqlDataAdapter(cmd);
                adpt.SelectCommand.CommandType = CommandType.StoredProcedure;
                adpt.SelectCommand.Parameters.AddWithValue("@ExpDate", ExpDate);
                adpt.Fill(dt);
            }
            return dt;
        }

        public int Update(clsBlExpense BlExpense)
        {
            int HowManyUpdate = 0;
            using (SqlCommand cmd = new SqlCommand("InsertUpdateExpense", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ExpenseId",BlExpense.ExpenseId);
                cmd.Parameters.AddWithValue("@ExpDate", BlExpense.ExpenseDate);
                cmd.Parameters.AddWithValue("@ExpenseType", BlExpense.ExpenseTypeID);
                cmd.Parameters.AddWithValue("@PaymentMode", BlExpense.PaymentModeID);
                cmd.Parameters.AddWithValue("@Amount", BlExpense.Amount);
                cmd.Parameters.AddWithValue("@Notes", BlExpense.Notes);

                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                HowManyUpdate = cmd.ExecuteNonQuery();
            }
            return HowManyUpdate;
        }

      

      
        
    }
}

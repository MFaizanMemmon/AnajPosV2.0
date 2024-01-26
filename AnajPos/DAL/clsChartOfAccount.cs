using AnajPos.BAL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnajPos.DAL
{
    class clsChartOfAccount
    {
        SqlConnection conn = new SqlConnection(clsConnectionString.cs);
        public int Insert(clsBlChartOfAccounts BlCoa)
        {
            int insertedId = 0;
            using (SqlCommand cmd = new SqlCommand("usp_InsertChartOfAccountExpeseHead", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@AccountName", BlCoa.AccountName);
                cmd.Parameters.AddWithValue("@ParentId", BlCoa.ParentId);
                cmd.Parameters.AddWithValue("@Dr", BlCoa.Dr);
                cmd.Parameters.AddWithValue("@Cr", BlCoa.Cr);

                SqlParameter insertedIdParam = new SqlParameter("@InsertedId", SqlDbType.Int);
                insertedIdParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(insertedIdParam);

                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                cmd.ExecuteNonQuery();
                insertedId = Convert.ToInt32(insertedIdParam.Value);


            }
            return insertedId;

        }

        public DataTable ViewChartOfAccountParenttID(int ParentID)
        {
            DataTable dt = new DataTable();

            using (SqlCommand cmd = new SqlCommand("usp_GetChartOfAccount", conn))
            {
              
                SqlDataAdapter adpt = new SqlDataAdapter(cmd);
                adpt.SelectCommand.CommandType = CommandType.StoredProcedure;
                adpt.SelectCommand.Parameters.AddWithValue("@ParentId", ParentID);
                adpt.Fill(dt);
            }
            return dt;
        }

        public DataTable ViewChartOfAccountPaymentMode()
        {
            DataTable dt = new DataTable();

            using (SqlCommand cmd = new SqlCommand("usp_GetPaymentMethod", conn))
            {
                SqlDataAdapter adpt = new SqlDataAdapter(cmd);
                adpt.Fill(dt);
            }
            return dt;
        }

        public int Update(clsBlChartOfAccounts BlCoa)
        {
            int HowManyUpdate = 0;
            using (SqlCommand cmd = new SqlCommand("usp_UpdateChartOfAccountExpenseHead", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@AccountId", BlCoa.AccountId);
                cmd.Parameters.AddWithValue("@AccountName", BlCoa.AccountName);

                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                HowManyUpdate = cmd.ExecuteNonQuery();
            }
            return HowManyUpdate;
        }

        public DataTable GetChartOfAccounts()
        {
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand("Usp_ChartOfAccountDropDown_sp", conn))
            {

                SqlDataAdapter adpt = new SqlDataAdapter(cmd);
                adpt.SelectCommand.CommandType = CommandType.StoredProcedure;
                adpt.Fill(dt);
            }
            return dt;
        }
        public DataTable GetLedgers(int AccountID, string StartDate, string EndDate)
        {
            DataTable dt = new DataTable();
            using(SqlCommand cmd = new SqlCommand("usp_GetLedger_sp",conn))
            {
                SqlDataAdapter adpt = new SqlDataAdapter(cmd);
                adpt.SelectCommand.CommandType = CommandType.StoredProcedure;
                adpt.SelectCommand.Parameters.AddWithValue("@AccountID", AccountID);
                adpt.SelectCommand.Parameters.AddWithValue("@StartDate", StartDate);
                adpt.SelectCommand.Parameters.AddWithValue("@EndDate", EndDate);
                adpt.Fill(dt);
            }
                return dt;
        }
    }


}

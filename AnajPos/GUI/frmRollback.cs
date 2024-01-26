using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AnajPos.GUI;
using System.Data.SqlClient;
using AnajPos.DAL;
using AnajPos.BAL_Cash_BANK;
using AnajPos.DAL_Cash_Bank;

namespace AnajPos.GUI
{
    public partial class frmRollback : frmTemplete
    {
        clsBLChartOfAccountLedger blChartOfAccoutLedger = new clsBLChartOfAccountLedger();
        clsChartOfAccountLedger dlChartOfAccountLedger = new clsChartOfAccountLedger();
        clsCustomer dlCustomer = new clsCustomer();
        public frmRollback()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != string.Empty)
            {
                if (DeleteTransaction() > 0)
                {
                    UpdateInvoice();
                    blChartOfAccoutLedger.TransactionId = int.Parse(textBox1.Text);
                    blChartOfAccoutLedger.TransactionType = "Create Sale Invoice";
                    blChartOfAccoutLedger.ChartOfAccountId = dlCustomer.CustomerAccountId(GetCustomerIdByInvoiceId(int.Parse(textBox1.Text)));
                    dlChartOfAccountLedger.DeleteLedger(blChartOfAccoutLedger);
                    MessageBox.Show("Your Invoice has been Rollback...");
                }
            }
        }

        public int GetCustomerIdByInvoiceId(int InvoiceId)
        {
            int CustomerId = 0;
            using (SqlConnection conn = new SqlConnection(clsConnectionString.cs))
            {
                string query = string.Format("select CustomerId from tblInvoice where Sid = {0}", InvoiceId);
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (conn.State != ConnectionState.Open)
                    {
                        conn.Open();
                    }
                    CustomerId = (int)cmd.ExecuteScalar();
                }
            }



            return CustomerId;
        }
        private int DeleteTransaction()
        {
            int HowmanyDelete = 0;
            using (SqlConnection conn = new SqlConnection(DAL.clsConnectionString.cs))
            {
                string q = string.Format("delete from tblTransaction Where TranID={0} and TranType='{1}'", int.Parse(textBox1.Text), "Invoice");
                using (SqlCommand cmd = new SqlCommand(q, conn))
                {
                    if (conn.State != ConnectionState.Open)
                    {
                        conn.Open();
                    }
                    HowmanyDelete = (int)cmd.ExecuteNonQuery();
                }

            }
            return HowmanyDelete;
        }
        private void UpdateInvoice()
        {
            using (SqlConnection conn = new SqlConnection(DAL.clsConnectionString.cs))
            {
                string q = string.Format("update tblInvoice set PostingType = '{0}' where Sid={1}", "Order", int.Parse(textBox1.Text));
                using (SqlCommand cmd = new SqlCommand(q, conn))
                {
                    if (conn.State != ConnectionState.Open)
                    {
                        conn.Open();
                    }
                    cmd.ExecuteNonQuery();
                }
            }

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

    }
}

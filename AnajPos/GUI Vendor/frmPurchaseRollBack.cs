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
using AnajPos.BAL_Cash_BANK;
using AnajPos.DAL;
using AnajPos.DAL_Cash_Bank;
using AnajPos.DalVendor;

namespace AnajPos.GUI_Vendor
{
    public partial class frmPurchaseRollBack : frmTemplete
    {

        clsBLChartOfAccountLedger blChartOfAccoutLedger = new clsBLChartOfAccountLedger();
        clsChartOfAccountLedger dlChartOfAccountLedger = new clsChartOfAccountLedger();
        clsDlVendor dlVendor = new clsDlVendor();
        public frmPurchaseRollBack()
        {
            InitializeComponent();
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
        private int DeleteTransaction()
        {
            int HowmanyDelete = 0;
            using (SqlConnection conn = new SqlConnection(DAL.clsConnectionString.cs))
            {
                string q = string.Format("delete from tblTransactionVendor Where TranID={0} and LedgerType='{1}'", int.Parse(textBox1.Text), " Purchase Invoice");
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
                string q = string.Format("update tblPurchase set PostingType = '{0}' where Pid={1}", "Order", int.Parse(textBox1.Text));
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

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != string.Empty)
            {
                if (DeleteTransaction() > 0)
                {
                    UpdateInvoice();
                    blChartOfAccoutLedger.TransactionId = int.Parse(textBox1.Text);
                    blChartOfAccoutLedger.TransactionType = "Create Purchase Invoice";
                    blChartOfAccoutLedger.ChartOfAccountId = dlVendor.VendorAccountId(GetVendorIdByInvoiceId(int.Parse(textBox1.Text)));
                    dlChartOfAccountLedger.DeleteLedger(blChartOfAccoutLedger);

                    MessageBox.Show("Your Invoice has been Rollback...");
                }
            }
        }
        public int GetVendorIdByInvoiceId(int InvoiceId)
        {
            int CustomerId = 0;
            using (SqlConnection conn = new SqlConnection(clsConnectionString.cs))
            {
                string query = string.Format("select VendorId from tblPurchase where PId = {0}", InvoiceId);
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

    }
}

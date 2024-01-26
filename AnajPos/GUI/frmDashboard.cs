using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AnajPos.DAL;
using AnajPos.GUI;
using AnajPos.GUI_Bank_Cash;
using AnajPos.GUI_Vendor;
using AnajPos.Reports;
using AnajPos.VendorReporting;


namespace AnajPos.GUI
{
    public partial class frmDashboard : frmTemplete
    {
        public frmDashboard()
        {
            InitializeComponent();
        }

        private void frmDashboard_Load(object sender, EventArgs e)
        {
            //FillGrid();
            //DailyProfit();
            lblPendingSaleOrder.Text = PendingSaleOrder().ToString();
            lblPendingPurchseOrder.Text = PendingPurchseOrder().ToString();
            


        }
        //private void FillGrid()
        //{
        //    string query = "SELECT InvoiceID, 'Sale Receipt' AS 'Description', Cast(SUM(Price * Qty) as int) AS 'Total' " +
        //                   "FROM tblcountersaleorpurchase " +
        //                   "WHERE CONVERT(date, InvDate, 120) = @SelectedDate " +
        //                   "GROUP BY InvoiceID";

        //    using (SqlConnection connection = new SqlConnection(clsConnectionString.cs))
        //    {
        //        using (SqlCommand command = new SqlCommand(query, connection))
        //        {
        //            // Use parameters to avoid SQL injection and ensure correct data type
        //            command.Parameters.AddWithValue("@SelectedDate", SqlDbType.Date).Value = dtpCounterSale.Value.Date;

        //            connection.Open();

        //            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
        //            {
        //                DataTable dataTable = new DataTable();
        //                adapter.Fill(dataTable);

        //                dgvCounterSale.DataSource = dataTable;

        //                int total = CalculateColumnTotal(dgvCounterSale, "Total");
        //                txtTotalSale.Text = total.ToString();
        //            }
        //        }
        //    }
        //}


        //private void DailyProfit()
        //{
        //    string query = "SELECT gz.ProductName, CAST(SUM(ISNULL(i.Price, 0) * ISNULL(i.Qty, 0)) AS INT) AS 'Today Sale' " +
        //                   "FROM tblcountersaleorpurchase i " +
        //                   "INNER JOIN tblProduct gz ON i.ProductId = gz.ProductID " +
        //                   "WHERE CAST(i.InvDate AS DATE) = @SelectedDate " +
        //                   "GROUP BY gz.ProductName";

        //    using (SqlConnection connection = new SqlConnection(clsConnectionString.cs))
        //    {
        //        using (SqlCommand command = new SqlCommand(query, connection))
        //        {
        //            // Use parameters to avoid SQL injection and ensure correct data type
        //            command.Parameters.AddWithValue("@SelectedDate", SqlDbType.Date).Value = dtpCounterSale.Value.Date;

        //            connection.Open();

        //            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
        //            {
        //                DataTable dataTable = new DataTable();
        //                adapter.Fill(dataTable);

        //                dgvCounterSale2.DataSource = dataTable;
        //            }
        //        }
        //    }
        //}


        //private int CalculateColumnTotal(DataGridView dataGridView, string columnName)
        //{
        //    int total = 0;

        //    if (dataGridView.Columns.Contains(columnName))
        //    {
        //        foreach (DataGridViewRow row in dataGridView.Rows)
        //        {
        //            if (row.Cells[columnName].Value != null &&
        //                int.TryParse(row.Cells[columnName].Value.ToString(), out int value))
        //            {
        //                total += value;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        // Column not found, handle the error as needed (e.g., throw an exception, return a default value, etc.)
        //    }

        //    return total;
        //}


        private int PendingSaleOrder()
        {
            int pendingOrdersCount = 0;

            using (SqlConnection conn = new SqlConnection(clsConnectionString.cs))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(Sid) FROM tblInvoice WHERE PostingType = 'Order'", conn))
                {
                    try
                    {
                        conn.Open();
                        pendingOrdersCount = (int)cmd.ExecuteScalar();
                    }
                    catch (Exception ex)
                    {
                        // Handle exceptions here, e.g., log or display an error message
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }
            }

            return pendingOrdersCount;
        }

        private int PendingPurchseOrder()
        {
            int pendingOrdersCount = 0;

            using (SqlConnection conn = new SqlConnection(clsConnectionString.cs))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(PId) FROM tblPurchase WHERE PostingType = 'Order'", conn))
                {
                    try
                    {
                        conn.Open();
                        pendingOrdersCount = (int)cmd.ExecuteScalar();
                    }
                    catch (Exception ex)
                    {
                        // Handle exceptions here, e.g., log or display an error message
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }
            }

            return pendingOrdersCount;
        }
        private void customerAccountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmCreateCustomer fcc = new frmCreateCustomer();
            fcc.ShowDialog();
        }

        private void createProductToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmProduct fp = new frmProduct();
            fp.ShowDialog();
        }

        private void createHeadOfAccountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmCreateBankAccount cb = new frmCreateBankAccount();
            cb.ShowDialog();
        }

        private void createInvoiceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmInvoice fi = new frmInvoice();
            fi.ShowDialog();
        }

        private void createReceiptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmReceipt fr = new frmReceipt();
            fr.ShowDialog();
        }

        private void closingWithCustomerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmClosingCustomer fcc = new frmClosingCustomer();
            fcc.ShowDialog();
        }

        private void customerAdjustmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAdjustment fad = new frmAdjustment();
            fad.ShowDialog();
        }

        private void customerLedgerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSelectCustomerLedger fsc = new frmSelectCustomerLedger();
            fsc.ShowDialog();
        }

        private void customerOutstandingReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Reports.frmCustomerOutstandingReport co = new Reports.frmCustomerOutstandingReport();
            co.ShowDialog();
        }

        private void btnInvoice_Click(object sender, EventArgs e)
        {
            frmInvoice fi = new frmInvoice();
            fi.ShowDialog();
        }

        private void btnReceipt_Click(object sender, EventArgs e)
        {
            frmReceipt fr = new frmReceipt();
            fr.ShowDialog();
        }

        private void btnAdjustment_Click(object sender, EventArgs e)
        {
            frmAdjustment adj = new frmAdjustment();
            adj.ShowDialog();
        }

        private void btnLedger_Click(object sender, EventArgs e)
        {
            frmSelectCustomerLedger sl = new frmSelectCustomerLedger();
            sl.ShowDialog();
        }

        private void btnOutstanding_Click(object sender, EventArgs e)
        {
            Reports.frmCustomerOutstandingReport co = new Reports.frmCustomerOutstandingReport();
            co.ShowDialog();
        }

        private void backUpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmBackup backup = new frmBackup();
            backup.ShowDialog();
        }

        private void restoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmRestore restore = new frmRestore();
            restore.ShowDialog();
        }

        private void btnDailyReport_Click(object sender, EventArgs e)
        {

        }

        private void createSaleReturnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSaleReturn sr = new frmSaleReturn();
          
            sr.ShowDialog();
        }

        private void btnSaleReturn_Click(object sender, EventArgs e)
        {
            frmSaleReturn sr = new frmSaleReturn();
            sr.ShowDialog();
        }

        private void rollbackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmRollback rb = new frmRollback();
            rb.ShowDialog();
        }

        private void aboutUsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAboutUs ab = new frmAboutUs();
            ab.ShowDialog();
        }

        private void createVendorAccountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmCreateVendor fcv = new frmCreateVendor();
            fcv.ShowDialog();
        }

        private void purchaseInvoiceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmPurchaseInvoice fpi = new frmPurchaseInvoice();
            fpi.ShowDialog();
            lblPendingPurchseOrder.Text = PendingPurchseOrder().ToString();
        }

        private void paymentVoucherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmPayment pay = new frmPayment();
            pay.ShowDialog();
        }

        private void vendorAdjustmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmVendorAdjestment adj = new frmVendorAdjestment();
            adj.ShowDialog();
        }

        private void purchaseReturnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmPurchaseReturn sr = new frmPurchaseReturn();
           
            sr.ShowDialog();
        }

        private void receiptToPaidToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmReceiptToPaid rtp = new frmReceiptToPaid();
            rtp.ShowDialog();

        }

        private void vendorOutstandingReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmVendorOutstandingReport vo = new frmVendorOutstandingReport();
            vo.ShowDialog();
        }

        private void vendorLedgerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmVendorSelectLedger vsl = new frmVendorSelectLedger();
            vsl.ShowDialog();
        }

        private void rollbackPurchaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmPurchaseRollBack pr = new frmPurchaseRollBack();
            pr.ShowDialog();
        }

        private void btnCounterSale_Click(object sender, EventArgs e)
        {
            frmCounterSaleOrPurchase2 cp = new frmCounterSaleOrPurchase2();
            cp.ShowDialog();
            //FillGrid(); 
            //DailyProfit();
            
        }

        private void createExpenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmCreateExpense exp = new frmCreateExpense();
            exp.ShowDialog();
        }

        private void createCashAccountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmCreateCashAccount ca = new frmCreateCashAccount();
            ca.ShowDialog();
        }

        private void tsButtonNewSale_Click(object sender, EventArgs e)
        {
            frmInvoice fi = new frmInvoice();
            fi.ShowDialog();
            lblPendingSaleOrder.Text = PendingSaleOrder().ToString();
        }

        private void tsButtonSalePayment_Click(object sender, EventArgs e)
        {
            frmReceipt fr = new frmReceipt();
            fr.ShowDialog();
        }

        private void tsButtonNewPurchase_Click(object sender, EventArgs e)
        {
            frmPurchaseInvoice fpi = new frmPurchaseInvoice();
            fpi.ShowDialog();
        }

        private void tsButtonPurchasePayment_Click(object sender, EventArgs e)
        {
            frmPayment pay = new frmPayment();
            pay.ShowDialog();
        }

        private void toolStripMenuItem11_Click(object sender, EventArgs e)
        {
            frmProduct fp = new frmProduct();
            fp.ShowDialog();
        }

        private void tsButtonNewExpenses_Click(object sender, EventArgs e)
        {
            frmCreateExpense exp = new frmCreateExpense();
            exp.ShowDialog();
        }

        private void inventoryTransferToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmInventoryTransfer InvTransfer = new frmInventoryTransfer();
            InvTransfer.ShowDialog();
        }

        private void toolStripMenuItem10_Click(object sender, EventArgs e)
        {
            frmProductStock ps = new frmProductStock();
            ps.ShowDialog();
        }

        private void allLedgersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            frmLedgers ld = new frmLedgers();
            ld.ShowDialog();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void txButtonAllExpenses_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem12_Click(object sender, EventArgs e)
        {

        }

        private void btnShowDashboard_Click(object sender, EventArgs e)
        {
            
        }

        private void stockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmProductStock fs = new frmProductStock();
            fs.ShowDialog();

        }

        private void dtpCounterSale_ValueChanged(object sender, EventArgs e)
        {
            
            //FillGrid();
            //DailyProfit();
           

        }

        private void accountReceivableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLedgers ld = new frmLedgers();
            ld.LedgerType = "Customer Ledger";
            ld.ShowDialog();
        }

        private void receivableBalanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLedgers ld = new frmLedgers();
            ld.LedgerType = "Vendor Ledger";
            ld.ShowDialog();
        }

        private void cashReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLedgers ld = new frmLedgers();
            ld.LedgerType = "Cash";
            ld.ShowDialog();
        }

        private void bankLedgerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLedgers ld = new frmLedgers();
            ld.LedgerType = "Bank";
            ld.ShowDialog();
        }

        private void counterSaleReturnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmCounterSaleReturn cr = new frmCounterSaleReturn();
            cr.ShowDialog();
        }

        private void accountPaybleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmCustomerOutstandingReport co = new frmCustomerOutstandingReport();
            co.ShowDialog();

        }

        private void paybleBalanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmVendorOutstandingReport vo = new frmVendorOutstandingReport();
            vo.ShowDialog();
        }

        private void toolStripMenuItem9_Click(object sender, EventArgs e)
        {
            frmStock s = new frmStock();
            s.ShowDialog();
        }

        private void tsButtonStock_ButtonClick(object sender, EventArgs e)
        {

        }

        private void counterSaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmCounterSaleOrPurchase2 cs = new frmCounterSaleOrPurchase2();
            cs.ShowDialog();
        }
    }
}

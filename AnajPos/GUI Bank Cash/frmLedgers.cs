using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AnajPos.DAL;
using AnajPos.GUI;
using AnajPos.GUI_Bank_Cash;

namespace AnajPos.GUI_Bank_Cash
{
    public partial class frmLedgers : frmTemplete
    {
        public string LedgerType { get; set; }
        clsChartOfAccount dLAccount = new clsChartOfAccount();
        public frmLedgers()
        {
            InitializeComponent();
        }

        private void frmLedgers_Load(object sender, EventArgs e)
        {
            dptStartDate.Value = DateTime.Now.AddYears(-1);

            if (LedgerType == "Customer Ledger")
            {
                DataTable dtCounterSaleMode = dLAccount.ViewChartOfAccountParenttID(10);

                cmbChartOfAccounts.DataSource = dtCounterSaleMode;
                cmbChartOfAccounts.ValueMember = "AccountId";
                cmbChartOfAccounts.DisplayMember = "AccountName";
                cmbChartOfAccounts.SelectedIndex = -1;

            }
            else if (LedgerType == "Vendor Ledger")
            {
                DataTable dtCounterSaleMode = dLAccount.ViewChartOfAccountParenttID(11);

                cmbChartOfAccounts.DataSource = dtCounterSaleMode;
                cmbChartOfAccounts.ValueMember = "AccountId";
                cmbChartOfAccounts.DisplayMember = "AccountName";
                cmbChartOfAccounts.SelectedIndex = -1;
            }
            else if (LedgerType == "Cash")
            {
                DataTable dtCounterSaleMode = dLAccount.ViewChartOfAccountParenttID(13);

                cmbChartOfAccounts.DataSource = dtCounterSaleMode;
                cmbChartOfAccounts.ValueMember = "AccountId";
                cmbChartOfAccounts.DisplayMember = "AccountName";
                cmbChartOfAccounts.SelectedIndex = -1;

                dptStartDate.Value = DateTime.Now;

            }
            else if (LedgerType == "Bank")
            {

                DataTable dtCounterSaleMode = dLAccount.ViewChartOfAccountParenttID(22);

                cmbChartOfAccounts.DataSource = dtCounterSaleMode;
                cmbChartOfAccounts.ValueMember = "AccountId";
                cmbChartOfAccounts.DisplayMember = "AccountName";
                cmbChartOfAccounts.SelectedIndex = -1;

            }

        }

        private void cmbChartOfAccounts_SelectedValueChanged(object sender, EventArgs e)
        {
            
        }

        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            if (cmbChartOfAccounts.SelectedIndex != -1)
            {
                dvgViewLedgers.DataSource = dLAccount.GetLedgers((int)cmbChartOfAccounts.SelectedValue, (string)dptStartDate.Text, (string)dptEndDate.Text);
            }

            dvgViewLedgers.Columns["SNO"].HeaderText = "No";
            dvgViewLedgers.Columns["TranDate"].HeaderText = "Date";
            //dvgViewLedgers.Columns["Price"].HeaderText = "ColumnPrice";

            //// Set column widths
            dvgViewLedgers.Columns[0].Width = 50;  // Set the width in pixels
            dvgViewLedgers.Columns[1].Width = 100; // Set the width in pixels
            dvgViewLedgers.Columns[2].Width = 80; // Set the width in pixels
            dvgViewLedgers.Columns[3].Width = 300; // Set the width in pixels
            dvgViewLedgers.Columns[4].Width = 250; // Set the width in pixels
            dvgViewLedgers.Columns[5].Width = 80; // Set the width in pixels
            dvgViewLedgers.Columns[6].Width = 80; // Set the width in pixels
            dvgViewLedgers.Columns[7].Width = 120; // Set the width in pixels
        }

        private void btnPrintInvoice_Click(object sender, EventArgs e)
        {
            if (cmbChartOfAccounts.SelectedIndex != -1)
            {
                CashBankReporting.frmViewLedger inv = new CashBankReporting.frmViewLedger();
                inv.AccountId = (int)cmbChartOfAccounts.SelectedValue;
                inv.AccountName = cmbChartOfAccounts.Text;
                inv.StartDate = dptStartDate.Value.ToString();
                inv.EndDate = dptEndDate.Value.ToString();

                inv.ShowDialog();
            }
        }
    }
}

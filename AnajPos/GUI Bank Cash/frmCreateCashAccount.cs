using AnajPos.BAL;
using AnajPos.BAL_Cash_BANK;
using AnajPos.DAL;
using AnajPos.DAL_Cash_Bank;
using AnajPos.GUI;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnajPos.GUI_Bank_Cash
{
    public partial class frmCreateCashAccount : frmTemplete
    {
        clsBlChartOfAccounts blCoa = new clsBlChartOfAccounts();
        clsChartOfAccount dlCoa = new clsChartOfAccount();
        clsBLChartOfAccountLedger blCoaLedger = new clsBLChartOfAccountLedger();
        clsChartOfAccountLedger dlCoaLedger = new clsChartOfAccountLedger();
        public frmCreateCashAccount()
        {
            InitializeComponent();
        }
        private bool IsValidate()
        {
            if (txtCreateCashAccount.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Please Enter Cash Account");
                txtCreateCashAccount.Focus();
                return false;
            }
            else if (txtOpeningBalance.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Please Enter Opening Balance");
                txtOpeningBalance.Focus();
                return false;
            }
            return true;
        }

        private void frmCreateCashAccount_Load(object sender, EventArgs e)
        {
            dgvCashAccount.DataSource = dlCoa.ViewChartOfAccountParenttID(13);
            dgvCashAccount.Columns["Dr"].Visible = false;
            dgvCashAccount.Columns["Cr"].Visible = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (IsValidate())
            {
                blCoa.AccountName = txtCreateCashAccount.Text;
                blCoa.ParentId = 13;
                blCoa.Dr = Convert.ToDecimal(txtOpeningBalance.Text);
                blCoa.Cr = 0;
                int CoaInsertedId = dlCoa.Insert(blCoa);
                if (CoaInsertedId > 0)
                {
                    blCoaLedger.TransactionDate = dtpOpeningDate.Value;
                    blCoaLedger.TransactionType = "Chart of Account Opening Balance";
                    blCoaLedger.TransactionId = 0;
                    blCoaLedger.ChartOfAccountId = CoaInsertedId;
                    blCoaLedger.Remark = "Opening Balance";
                    blCoaLedger.Dr = int.Parse(txtOpeningBalance.Text);
                    blCoaLedger.Cr = 0;

                    if (dlCoaLedger.Insert(blCoaLedger) > 0)
                    {

                        txtCreateCashAccount.Clear();
                        txtOpeningBalance.Clear();
                        MessageBox.Show("Account has been Created...");
                        dgvCashAccount.DataSource = dlCoa.ViewChartOfAccountParenttID(13);
                    }
                  
                }


            }
        }
    }
}

using AnajPos.BAL_Cash_BANK;
using AnajPos.DAL;
using AnajPos.DAL_Cash_Bank;
using AnajPos.GUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls.WebParts;
using System.Windows.Forms;

namespace AnajPos.GUI_Bank_Cash
{
    public partial class frmCreateBankAccount : frmTemplete
    {
        clsBank DlBank = new clsBank();
        clsBlBank blBank = new clsBlBank();
        clsBLChartOfAccountLedger blCoaLedger = new clsBLChartOfAccountLedger();
        clsChartOfAccountLedger dlCoaLedger = new clsChartOfAccountLedger();
        public frmCreateBankAccount()
        {
            InitializeComponent();
        }

        private void frmCreateBankAccount_Load(object sender, EventArgs e)
        {
           

            lblFirst.Text = DlBank.MinId().ToString();
            lblLast.Text = DlBank.MaxId().ToString();
            lblCurrent.Text = lblFirst.Text;
            FillDateInFields();
            this.Focus();


        }

        private void banksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmViewBank vb = new frmViewBank();
            vb.ShowDialog();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            EnableField();
            ClearFields();
            txtBankId.Text = DlBank.NewId().ToString();
            
            btnSave.Enabled = true;
            btnNew.Enabled = false;
            txtBankTitle.Focus();
         
        }

        private void ClearFields()
        {
            txtBankId.Text = string.Empty;
            txtBankTitle.Text = string.Empty;
            txtBankName.Text = string.Empty;
            txtBankNameUrdu.Text = string.Empty;
            txtAccountNo.Text = string.Empty;
            chkIsActive.Checked = false;
            txtAddress.Text = string.Empty;
            dtpOpeningDate.Value = DateTime.Now;
            txtOpeningBalance.Text = string.Empty;
            
        }
        private void EnableField()
        {
            txtBankTitle.Enabled = true;
            txtBankId.Enabled = true;
            txtBankName.Enabled = true;
            txtBankNameUrdu.Enabled = true;
            txtAccountNo.Enabled = true;
            txtAddress.Enabled = true;
            dtpOpeningDate.Enabled = true;
            txtOpeningBalance.Enabled = true;
            chkIsActive.Enabled = true;

        }
        private void DisableField()
        {
            txtBankTitle.Enabled = false;
            txtBankId.Enabled = false;
            txtBankName.Enabled = false;
            txtBankNameUrdu.Enabled = false;
            txtAccountNo.Enabled = false;
            txtAddress.Enabled = false;
            dtpOpeningDate.Enabled = false;
            txtOpeningBalance.Enabled = false;
            chkIsActive.Enabled = false;
        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            lblCurrent.Text = lblFirst.Text;
            FillDateInFields();
        }
        private void FillDateInFields()
        {
           
            DataTable dt = DlBank.ViewBankByID(Convert.ToInt16(lblCurrent.Text));
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                txtBankId.Text = dr[0].ToString();
                txtBankTitle.Text = dr[1].ToString();
                txtBankName.Text = dr[2].ToString();
                txtBankNameUrdu.Text = dr[3].ToString(); 
                txtAddress.Text = dr[5].ToString();
                dtpOpeningDate.Value = Convert.ToDateTime(dr[6]);
                txtOpeningBalance.Text = dr[7].ToString();
                txtAccountNo.Text = dr[4].ToString();
                chkIsActive.Checked = (bool)dr[8];

            }
        }

       

        private void btnLast_Click(object sender, EventArgs e)
        {
            lblCurrent.Text = lblLast.Text;
            FillDateInFields();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            int NextData = Convert.ToInt32(lblCurrent.Text);
            NextData++;
            if (NextData >= Convert.ToInt32(lblFirst.Text) && NextData <= Convert.ToInt32(lblLast.Text))
            {
                lblCurrent.Text = NextData.ToString();
                FillDateInFields();
            }
            else
            {
                MessageBox.Show("This is Last Data");
            }
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            int PreviousData = Convert.ToInt32(lblCurrent.Text);
            PreviousData--;
            if (PreviousData >= Convert.ToInt16(lblFirst.Text) && PreviousData <= Convert.ToInt32(lblLast.Text))
            {
                lblCurrent.Text = PreviousData.ToString();
                FillDateInFields();
            }
            else
            {
                MessageBox.Show("This is First Data");
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (IsValidate())
            {
                blBank.BankId = int.Parse(txtBankId.Text);
                blBank.BankName = txtBankName.Text;
                blBank.BankTitle = txtBankTitle.Text;
                blBank.BankNameUrdu = txtBankNameUrdu.Text;
                blBank.AccountNo = txtAccountNo.Text;
                blBank.Address = txtAddress.Text;
                blBank.OpeningDate = dtpOpeningDate.Value;
                blBank.OpeningBalance = decimal.Parse(txtOpeningBalance.Text);
                blBank.IsActive = chkIsActive.Checked;
                // 8 is Chart of Account ID
                blBank.AccountId = 8;
                if (DlBank.Insert(blBank) > 0)
                {
                    blCoaLedger.TransactionDate = dtpOpeningDate.Value;
                    blCoaLedger.TransactionType = "Create Bank Account";
                    blCoaLedger.TransactionId = int.Parse(txtBankId.Text);
                    blCoaLedger.ChartOfAccountId = DlBank.BankAccountId(int.Parse(txtBankId.Text));
                    blCoaLedger.Remark = "Opening Balance";
                    blCoaLedger.Dr = int.Parse(txtOpeningBalance.Text);
                    blCoaLedger.Cr = 0;
                    dlCoaLedger.Insert(blCoaLedger);

                    MessageBox.Show("Bank has been Created");                    
                   
                    DisableField();
                    btnSave.Enabled = false;
                    btnNew.Enabled = true;
                }

            }
        }

        private bool IsValidate()
        {
            if (txtBankId.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Please Enter Bank ID");
                txtBankId.Focus();
                return false;
            }
            else if (txtBankName.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Please Enter Bank Name");
                txtBankName.Focus();
                return false;
            }
            else if (txtBankTitle.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Please Enter Bank Title");
                txtBankTitle.Focus();
                return false;
            }
            else if (txtBankNameUrdu.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Please Enter Bank Name Urdu");
                txtBankNameUrdu.Focus();
                return false;
            }
            else if (txtAccountNo.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Please Enter Account No");
                txtAccountNo.Focus();
                return false;
            }
            else if (txtOpeningBalance.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Please Enter Opening Balance");
                txtOpeningBalance.Focus();
                return false;
            }
            else if (txtAddress.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Please Enter Proper Address");
                txtAddress.Focus();
                return false;
            }

            return true;
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            if (IsValidate())
            {
                blBank.BankId = int.Parse(txtBankId.Text);
                blBank.BankName = txtBankName.Text;
                blBank.BankTitle = txtBankTitle.Text;
                blBank.BankNameUrdu = txtBankNameUrdu.Text;
                blBank.AccountNo = txtAccountNo.Text;
                blBank.Address = txtAddress.Text;
                blBank.OpeningDate = dtpOpeningDate.Value;
                blBank.OpeningBalance = decimal.Parse(txtOpeningBalance.Text);
                blBank.IsActive = chkIsActive.Checked;
                // 8 is Chart of Account ID
                blBank.AccountId = 8;
                if (DlBank.Update(blBank) > 0)
                {
                    blCoaLedger.TransactionDate = dtpOpeningDate.Value;
                    blCoaLedger.TransactionType = "Create Bank Account";
                    blCoaLedger.TransactionId = int.Parse(txtBankId.Text);
                    blCoaLedger.ChartOfAccountId = DlBank.BankChartOfAccountId(int.Parse(txtBankId.Text));
                    blCoaLedger.Remark = txtAccountNo.Text;
                    blCoaLedger.Dr = decimal.Parse(txtOpeningBalance.Text);
                    blCoaLedger.Cr = 0;
                    dlCoaLedger.UpdateLedger(blCoaLedger);

                    MessageBox.Show("Bank has been Updated");
                   
                    DisableField();
                    txtBankId.ReadOnly = true;
                    btnNew.Enabled = btnSave.Enabled = btnView.Enabled = btnFirst.Enabled = btnNext.Enabled = btnPrevious.Enabled = btnLast.Enabled = true;
                    btnModify.Enabled = false;
                   
                }

            }
        }

        private void txtBankTitle_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.SelectNextControl((Control)sender, true, true, true, true);
                e.SuppressKeyPress = true;
            }
        }

        private void alterationModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EnableField();
            txtBankId.ReadOnly = true;
            btnNew.Enabled = btnSave.Enabled = btnView.Enabled = btnFirst.Enabled = btnNext.Enabled = btnPrevious.Enabled = btnLast.Enabled = false;
            btnModify.Enabled = true;
        }

        private void txtOpeningBalance_KeyPress(object sender, KeyPressEventArgs e)
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

        private void btnView_Click(object sender, EventArgs e)
        {
            frmViewBank vb = new frmViewBank();
            vb.ShowDialog();
        }

        private void frmCreateBankAccount_KeyDown(object sender, KeyEventArgs e)
        {
          
        }

        private void txtBankName_TextChanged(object sender, EventArgs e)
        {
            txtBankNameUrdu.Text = txtBankName.Text;
        }
    }
}

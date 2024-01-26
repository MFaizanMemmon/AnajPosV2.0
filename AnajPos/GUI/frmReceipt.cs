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
using AnajPos.DAL;
using AnajPos.BAL;
using AnajPos.BAL_Cash_BANK;
using AnajPos.DAL_Cash_Bank;

namespace AnajPos.GUI
{
    public partial class frmReceipt : frmTemplete
    {
        clsZone cz = new clsZone();
        clsCustomer DlCustomer = new clsCustomer();
        clsBlCustomer BlCustomer = new clsBlCustomer();
        clsAddress DlAddress = new clsAddress();
        clsBlAddress BlAddress = new clsBlAddress();
        clsTransaction DlTransaction = new clsTransaction();
        clsBlTransaction BlTransaction = new clsBlTransaction();
        clsHeadOfAcc DlHead = new clsHeadOfAcc();
        clsReceipt DlReceipt = new clsReceipt();
        clsBlReceipt BlReceipt = new clsBlReceipt();
        clsChartOfAccount dlCoa = new clsChartOfAccount();
        clsBLChartOfAccountLedger blCoaLedger = new clsBLChartOfAccountLedger();
        clsChartOfAccountLedger dlCoaLedger = new clsChartOfAccountLedger();
        
        public frmReceipt()
        {
            InitializeComponent();
        }

        private void frmReceipt_Load(object sender, EventArgs e)
        {
            cmbZone.SelectedValueChanged -= new EventHandler(cmbZone_SelectedValueChanged);
            DataTable dtView = cz.ViewZone();
            cmbZone.DataSource = dtView;
            cmbZone.ValueMember = "Id";
            cmbZone.DisplayMember = "Name of Zone";
            cmbZone.SelectedIndex = -1;
            cmbCustomer.Text = string.Empty;
            cmbZone.SelectedValueChanged += new EventHandler(cmbZone_SelectedValueChanged);

            //cmbHead.DataSource = DlHead.ViewHead();
            //cmbHead.DisplayMember = "HeadOfAcc";
            //cmbHead.ValueMember = "HeadId";
            DataTable dtViewPaymentMode = dlCoa.ViewChartOfAccountPaymentMode();
            cmbHead.DataSource = dtViewPaymentMode;
            cmbHead.ValueMember = "AccountId";
            cmbHead.DisplayMember = "AccountName";
            cmbHead.SelectedIndex = -1;

            lblFirstID.Text = DlReceipt.FirstId().ToString();
            lblLastID.Text = DlReceipt.LastId().ToString();
            lblCurrentId.Text = lblFirstID.Text;


            FillDataInField();
            DisableField();
        }

        private void FillDataInField()
        {
            DataTable dt = DlReceipt.ViewReceipt(int.Parse(lblCurrentId.Text));
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                txtRecId.Text = dr[0].ToString();
                dtpRec.Value = (DateTime)dr[1];
                cmbZone.SelectedValue = (int)dr[2];
                cmbAddress.SelectedValue = (int)dr[3];   
                cmbCustomer.SelectedValue = (int)dr[4];       
               
                cmbHead.SelectedValue = (int)dr[5];
                //txtAmountInEnglish.Text = dr[6].ToString();
                txtRemark.Text = dr[7].ToString();
                txtAmount.Text = dr[8].ToString();
                lblPreviousBalance.Text = dr[9].ToString();
                lblCurrentBalance.Text = dr[12].ToString();

                if (cmbCustomer.Text == string.Empty)
                {
                     cmbCustomer.Text = "Deactive Customer";
                }
            }


        }
        private void ClearData()
        {
            txtRecId.Text = string.Empty;
            cmbHead.SelectedIndex = -1;
            cmbZone.SelectedIndex = -1;
            txtAmount.TextChanged -= new EventHandler(txtAmount_TextChanged);
            txtAmount.Text = string.Empty;
            txtAmount.TextChanged += new EventHandler(txtAmount_TextChanged);
            //txtAmountInEnglish.Text = string.Empty;
            txtRemark.Text = string.Empty;
            lblCurrentBalance.Text = "0";
            lblPreviousBalance.Text = "0";
            cmbAddress.DataSource = null;
            cmbCustomer.DataSource = null;
            cmbAddress.SelectedIndex = -1;
            cmbCustomer.SelectedIndex = -1;
            cmbCustomer.Text = string.Empty;
        }
        private void EnableField()
        {
            txtRecId.Enabled = true;
            dtpRec.Enabled = true;
            cmbZone.Enabled = true;
            cmbAddress.Enabled = true;
            cmbCustomer.Enabled = true;
            txtAmount.Enabled = true;
           // txtAmountInEnglish.Enabled = true;
            txtRemark.Enabled = true;
            cmbHead.Enabled = true;
        }

        private void DisableField()
        {
            txtRecId.Enabled = false;
            dtpRec.Enabled = false;
            cmbZone.Enabled = false;
            cmbAddress.Enabled = false;
            cmbCustomer.Enabled = false;
            cmbHead.Enabled = false;
            txtAmount.Enabled = false;
            //txtAmountInEnglish.Enabled = false;
            txtRemark.Enabled = false;
        }
        private void cmbZone_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cmbZone.SelectedIndex != -1)
            {
                BlAddress.AddressId = (int)cmbZone.SelectedValue;
                cmbAddress.DataSource = DlAddress.ViewAddressByZone(BlAddress);
                cmbAddress.DisplayMember = "AddressName";
                cmbAddress.ValueMember = "AddressID";
                cmbAddress.SelectedIndex = -1;
                cmbCustomer.Text = string.Empty;
            }
        }

        private void cmbAddress_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                BlCustomer.AddressId = Convert.ToInt32(cmbAddress.SelectedValue);
                DataTable dt = DlCustomer.ViewByAddress(BlCustomer);
                cmbCustomer.DataSource = dt;
                cmbCustomer.DisplayMember = "CustomerName";
                cmbCustomer.ValueMember = "CustomerId";
                //  cmbCustomer.SelectedIndex = -1;
            }
            catch
            { }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
          
            btnSave.Enabled = true;
            btnNew.Enabled = false;
            EnableField();
            ClearData();
            txtRecId.Text = DlReceipt.NewId().ToString();
            btnFirst.Enabled = false;
            btnNext.Enabled = false;
            btnPrevious.Enabled = false;
            btnLast.Enabled = false;
            dtpRec.Focus();
            dtpRec.Value = DateTime.Now;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (IsValidated())
            {
                BlReceipt.RecId = int.Parse(txtRecId.Text);
                BlReceipt.RecDate = dtpRec.Value;
                BlReceipt.Head = (int)cmbHead.SelectedValue;
                BlReceipt.Zone = (int)cmbZone.SelectedValue;
                BlReceipt.Area = (int)cmbAddress.SelectedValue;
                BlReceipt.Customer = (int)cmbCustomer.SelectedValue;
                BlReceipt.Amount = int.Parse(txtAmount.Text);
                BlReceipt.AmountInEnglish = "Not Available";
                BlReceipt.Remark = txtRemark.Text;
                BlReceipt.PreviousAmount = int.Parse(lblPreviousBalance.Text);
                BlReceipt.LastBill = string.Empty;
                BlReceipt.LastReceipt=string.Empty;
                BlReceipt.CurrentBalance=int.Parse(lblCurrentBalance.Text);
                
                if (DlReceipt.Insert(BlReceipt)>0)
                {
                    //MessageBox.Show("Your data has been saved....");
                    BlTransaction.TransactionDate = dtpRec.Value;
                    BlTransaction.TransactionType = "Receipt";
                    BlTransaction.TransactionId = int.Parse(txtRecId.Text);
                    BlTransaction.CustomerId = (int)cmbCustomer.SelectedValue;
                    BlTransaction.Remark = txtRemark.Text;
                    BlTransaction.Dr = int.Parse(txtAmount.Text);
                    BlTransaction.Cr = 0;
                    BlTransaction.PostingDate = DateTime.Now;
                    BlTransaction.TranTypeUrdu = "رسید";
                    if (DlTransaction.Insert(BlTransaction) > 0)
                    {
                        //Dr Entry
                        blCoaLedger.TransactionDate = dtpRec.Value;
                        blCoaLedger.TransactionType = "Customer Reveiving Voucher";
                        blCoaLedger.TransactionId = int.Parse(txtRecId.Text);
                        blCoaLedger.ChartOfAccountId =  (int)cmbHead.SelectedValue;
                        blCoaLedger.Remark = txtRemark.Text;
                        blCoaLedger.Dr = int.Parse(txtAmount.Text);
                        blCoaLedger.Cr = 0;
                        dlCoaLedger.Insert(blCoaLedger);
                        //Cr Entry
                        blCoaLedger.TransactionDate = dtpRec.Value;
                        blCoaLedger.TransactionType = "Customer Reveiving Voucher";
                        blCoaLedger.TransactionId = int.Parse(txtRecId.Text);
                        blCoaLedger.ChartOfAccountId = DlCustomer.CustomerAccountId((int)cmbCustomer.SelectedValue);
                        blCoaLedger.Remark = txtRemark.Text;
                        blCoaLedger.Dr = 0;
                        blCoaLedger.Cr = int.Parse(txtAmount.Text);
                        dlCoaLedger.Insert(blCoaLedger);

                        MessageBox.Show("Your Transaction has been done.");
                        DisableField();
                        btnSave.Enabled = false;
                        btnNew.Enabled = true;
                        lblLastID.Text = DlReceipt.LastId().ToString();
                        btnFirst.Enabled = btnNext.Enabled = btnPrevious.Enabled = btnLast.Enabled = true;
                        btnPrint.Focus();
                    }
                }

            }
        }
        private bool IsValidated()
        {
            if (cmbCustomer.SelectedIndex == -1)
            {
                MessageBox.Show("Please Enter Customer Name");
                return false;
            }
            else if (txtAmount.Text==string.Empty)
            {
                MessageBox.Show("Please Enter Amount");
                return false;
            }
           
            else if (txtRemark.Text==string.Empty)
            {
                MessageBox.Show("Please Enter Remark");
                return false;
            }
            else if (cmbHead.SelectedIndex == -1)
            {
                MessageBox.Show("Please Enter Head of Account");
                return false;   
            }
	

            return true;
        }

        private void cmbCustomer_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cmbCustomer.SelectedValue == null)
            {
                return;
            }
            try
            {
                lblPreviousBalance.Text = DlCustomer.CustomerLastBalance((int)cmbCustomer.SelectedValue).ToString();
            }
            catch 
            {
                
               
            }
            
        }

        private void txtAmount_TextChanged(object sender, EventArgs e)
        {
            if (txtAmount.Text.Length > 9)
            {
                txtAmount.Text = string.Empty;
                lblCurrentBalance.Text = lblPreviousBalance.Text;
            }
            if (txtAmount.Text != string.Empty)
            {
                int CurrentAmount = 0;
                CurrentAmount = int.Parse(lblPreviousBalance.Text) - int.Parse(txtAmount.Text);
                lblCurrentBalance.Text = CurrentAmount.ToString();
                
            }
        
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            lblCurrentId.Text = lblLastID.Text;
            FillDataInField();
        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            lblCurrentId.Text = lblFirstID.Text;
            FillDataInField();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            int NextId = Convert.ToInt32(lblCurrentId.Text);
            if (NextId < int.Parse(lblLastID.Text))
            {
                NextId++;
                lblCurrentId.Text = NextId.ToString();
                FillDataInField();
            }
            else
            {
                MessageBox.Show("This is Last Id ...");
            }
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            int PreviousId = Convert.ToInt32(lblCurrentId.Text);
            if (PreviousId > int.Parse(lblFirstID.Text))
            {
                PreviousId--;
                lblCurrentId.Text = PreviousId.ToString();
                FillDataInField();
            }
            else
            {
                MessageBox.Show("This is First Id...");
            }
        }

        private void viewCustomerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmViewCustomer fvc = new frmViewCustomer();
            fvc.ShowDialog();
        }

        private void createCustomerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmCreateCustomer fcc = new frmCreateCustomer();
            fcc.ShowDialog();
        }   

        private void btnModify_Click(object sender, EventArgs e)
        {
            if (IsValidated())
            {
                BlReceipt.RecId = int.Parse(txtRecId.Text);
                BlReceipt.RecDate = dtpRec.Value;
                BlReceipt.Head = (int)cmbHead.SelectedValue;
                BlReceipt.Zone = (int)cmbZone.SelectedValue;
                BlReceipt.Area = (int)cmbAddress.SelectedValue;
                BlReceipt.Customer = (int)cmbCustomer.SelectedValue;
                BlReceipt.Amount = int.Parse(txtAmount.Text);
                BlReceipt.AmountInEnglish = "Not Available";
                BlReceipt.Remark = txtRemark.Text;
                BlReceipt.PreviousAmount = int.Parse(lblPreviousBalance.Text);
                BlReceipt.LastBill = string.Empty;
                BlReceipt.LastReceipt=string.Empty;
                BlReceipt.CurrentBalance=int.Parse(lblCurrentBalance.Text);

                if (DlReceipt.Update(BlReceipt) > 0)
                {
                    BlTransaction.TransactionDate = dtpRec.Value;
                    BlTransaction.TransactionType = "Receipt";
                    BlTransaction.TransactionId = int.Parse(txtRecId.Text);
                    BlTransaction.CustomerId = (int)cmbCustomer.SelectedValue;
                    BlTransaction.Remark = txtRemark.Text;
                    BlTransaction.Dr = int.Parse(txtAmount.Text);
                    BlTransaction.Cr = 0;
                    BlTransaction.PostingDate = DateTime.Now;
                    BlTransaction.TranTypeUrdu = "رسید";
                    if (DlTransaction.UpdateReceipt(BlTransaction)> 0)
                    {
                        //Dr Entry
                        blCoaLedger.TransactionDate = dtpRec.Value;
                        blCoaLedger.TransactionType = "Customer Reveiving Voucher";
                        blCoaLedger.TransactionId = int.Parse(txtRecId.Text);
                        blCoaLedger.ChartOfAccountId = (int)cmbHead.SelectedValue;
                        blCoaLedger.Remark = txtRemark.Text;
                        blCoaLedger.Dr = int.Parse(txtAmount.Text);
                        blCoaLedger.Cr = 0;
                        dlCoaLedger.UpdateLedger(blCoaLedger);
                        //Cr Entry
                        blCoaLedger.TransactionDate = dtpRec.Value;
                        blCoaLedger.TransactionType = "Customer Reveiving Voucher";
                        blCoaLedger.TransactionId = int.Parse(txtRecId.Text);
                        blCoaLedger.ChartOfAccountId = DlCustomer.CustomerAccountId((int)cmbCustomer.SelectedValue);
                        blCoaLedger.Remark = txtRemark.Text;
                        blCoaLedger.Dr = 0;
                        blCoaLedger.Cr = int.Parse(txtAmount.Text);
                        dlCoaLedger.UpdateLedger(blCoaLedger);
                        MessageBox.Show("Your data has been updated....");
                        DisableField();
                        btnModify.Enabled = false;
                        btnNew.Enabled = btnPrint.Enabled = true;
                        btnNext.Enabled = btnPrevious.Enabled = btnFirst.Enabled = btnLast.Enabled = true;
                        btnNew.Focus();
                    }
                }
            }
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnNext.Enabled = btnPrevious.Enabled = btnFirst.Enabled = btnLast.Enabled = false;
            btnNew.Enabled = btnNew.Enabled = btnPrint.Enabled = false;
            btnModify.Enabled = true;
            EnableField();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (btnPrint.Text=="Print In Urdu")
            {
                Reports.frmPrintReceiptUrdu fru = new Reports.frmPrintReceiptUrdu();
                fru.RecId = int.Parse(txtRecId.Text);
                fru.ShowDialog();
            }
            else
            {
                Reports.frmReceipt fr = new Reports.frmReceipt();
                fr.RecId = int.Parse(txtRecId.Text);
                fr.ShowDialog();
            }
        }

        private void createHeadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmHeadOfAcc fh = new frmHeadOfAcc();
            fh.ShowDialog();
            cmbHead.DataSource = null;
            cmbHead.DataSource = DlHead.ViewHead();
            cmbHead.DisplayMember = "HeadOfAcc";
            cmbHead.ValueMember = "HeadId";
        }

        private void viewReceiptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmViewReceipt fvr = new frmViewReceipt();
            fvr.ShowDialog();
           
        }

        private void changePrintingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (btnPrint.Text=="Print In English")
            {
                btnPrint.Text = "Print In Urdu";
            }
            else if (btnPrint.Text=="Print In Urdu")
            {
                btnPrint.Text = "Print In English";
            }
        }

        private void dtpRec_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.SelectNextControl((Control)sender, true, true, true, true);
                e.SuppressKeyPress = true;
            }
        }

        private void frmReceipt_KeyDown(object sender, KeyEventArgs e)
        {
           
            if (e.KeyCode == Keys.F1)
            
            {
                if (btnNew.Enabled==true)
                {
                    btnNew_Click(null, new EventArgs());
                }
            }
            else if (e.KeyCode == Keys.F2)
            {
                if (btnSave.Enabled==true)
                {
                    btnSave_Click(null, new EventArgs());
                }
            }
            else if (e.KeyCode == Keys.F3)
            {
                if (btnModify.Enabled == true)
                {
                    btnModify_Click(null, new EventArgs());
                }
            }
            else if (e.KeyCode == Keys.F5)
            {
                if (btnPrint.Enabled == true)
                {
                    btnPrint_Click(null, new EventArgs());
                }
            }
            else if (e.KeyCode == Keys.F6)
            {
                if (btnFirst.Enabled == true)
                {
                    btnFirst_Click(null, new EventArgs());
                }
            }
            else if (e.KeyCode == Keys.F7)
            {
                if (btnNext.Enabled == true)
                {
                    btnNext_Click(null, new EventArgs());
                }
            }
            else if (e.KeyCode == Keys.F8)
            {
                if (btnPrevious.Enabled == true)
                {
                    btnPrevious_Click(null, new EventArgs());
                }
            }
            else if (e.KeyCode == Keys.F9)
            {
                if (btnLast.Enabled == true)
                {
                    btnLast_Click(null, new EventArgs());
                }
            }
        }

        private void txtAmount_KeyPress(object sender, KeyPressEventArgs e)
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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtSearch.Text != string.Empty)
            {
                if (int.Parse(txtSearch.Text) >= int.Parse(lblFirstID.Text) && int.Parse(txtSearch.Text) <= int.Parse(lblLastID.Text))
                {
                    lblCurrentId.Text = txtSearch.Text;
                    FillDataInField();
                }
                else
                {
                    MessageBox.Show("Data has no found...");
                }
            }
        }

      
    }
}

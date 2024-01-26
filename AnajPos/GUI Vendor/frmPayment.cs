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
using AnajPos.DalVendor;
using AnajPos.BALVendor;
using AnajPos.BAL_Cash_BANK;
using AnajPos.DAL_Cash_Bank;
using System.Data.SqlClient;

namespace AnajPos.GUI_Vendor
{
    public partial class frmPayment : frmTemplete
    {
        clsZone cz = new clsZone();
        clsAddress BLAddress = new clsAddress();
        clsAddress DlAddress = new clsAddress();
        clsHeadOfAcc DlHead = new clsHeadOfAcc();
        clsBlAddress BlAddress = new clsBlAddress();
        clsBlVendor BlVendor = new clsBlVendor();
        clsDlVendor DlVendor = new clsDlVendor();
        clsBlPayment BlPayment = new clsBlPayment();
        clsDLPayment DlPayment = new clsDLPayment();
        clsChartOfAccount dlCoa = new clsChartOfAccount();
        clsBLChartOfAccountLedger blCoaLedger = new clsBLChartOfAccountLedger();
        clsChartOfAccountLedger dlCoaLedger = new clsChartOfAccountLedger();
        public frmPayment()
        {
            InitializeComponent();
        }
        private bool IsValidated()
        {
            if (cmbCustomer.SelectedIndex == -1)
            {
                MessageBox.Show("Please Enter Vendor Name");
                return false;
            }
            else if (txtAmount.Text == string.Empty)
            {
                MessageBox.Show("Please Enter Amount");
                return false;
            }

            else if (txtRemark.Text == string.Empty)
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


        private void frmPayment_Load(object sender, EventArgs e)
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

            lblFirstID.Text = DlPayment.FirstId().ToString();
            lblLastID.Text = DlPayment.LastId().ToString();
            lblCurrentId.Text = lblFirstID.Text;


            FillDataInField();
            DisableField();

        }

        private void cmbZone_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cmbZone.SelectedIndex != -1)
            {
                BlAddress.AddressId = (int)cmbZone.SelectedValue;
                cmbAddress.DataSource = DlAddress.ViewAddressByZone(BlAddress);
                cmbAddress.DisplayMember = "AddressName";
                cmbAddress.ValueMember = "AddressID";
            }
        }

        private void cmbAddress_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                BlVendor.AddressName = Convert.ToInt32(cmbAddress.SelectedValue);
                DataTable dt = DlVendor.ViewByAddress(BlVendor);
                cmbCustomer.DataSource = dt;
                cmbCustomer.DisplayMember = "VendorName";
                cmbCustomer.ValueMember = "VendorID";
                //  cmbCustomer.SelectedIndex = -1;
            }
            catch
            { }
        }

        private void viewCustomerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmViewVendor fv = new frmViewVendor();
            fv.ShowDialog();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (IsValidated())
            {
                BlPayment.PaymentID = int.Parse(txtRecId.Text);
                BlPayment.PaymentDate = dtpRec.Value;
                BlPayment.HeadID = (int)cmbHead.SelectedValue;
                //BlReceipt.Zone = (int)cmbZone.SelectedValue;
                //BlReceipt.Area = (int)cmbAddress.SelectedValue;
                BlPayment.VendorID = (int)cmbCustomer.SelectedValue;
                BlPayment.Amount = int.Parse(txtAmount.Text);
                //BlPayment.AmountInEnglish = "Not Available";
                BlPayment.PayRemark = txtRemark.Text;
                BlPayment.PreviousAmount = int.Parse(lblPreviousBalance.Text);
                //BlReceipt.LastBill = string.Empty;
                //BlReceipt.LastReceipt = string.Empty;
                //BlReceipt.CurrentBalance = int.Parse(lblCurrentBalance.Text);
                BlPayment.EntryDate = DateTime.Now;
                //
                if (DlPayment.Insert(BlPayment) > 0)
                {
                    DateTime TransactionDate = dtpRec.Value;
                    string TransactionType = "Payment Voucher";
                    int TransactionId = int.Parse(txtRecId.Text);
                    int VendorId = (int)cmbCustomer.SelectedValue;
                    string Remark = txtRemark.Text;
                    int Dr = 0;
                    int Cr = int.Parse(txtAmount.Text);
                    DateTime PostingDate = DateTime.Now;
                    //string TranTypeUrdu = "خرید";
                    //if (DlTransaction.Insert(BlTransaction) > 0)
                    //{
                    //    MessageBox.Show("Your ledger has been done...");
                    //    lblIsPost.Text = "Invoice";

                    //}
                    int HowManyInsert = 0;
                    using (SqlConnection conn = new SqlConnection(clsConnectionString.cs))
                    {
                        string q = string.Format("insert into tblTransactionVendor values ('{0}','{1}',{2},{3},'{4}',{5},{6},'{7}')", TransactionDate.ToString("yyyy-MM-dd"), TransactionType, TransactionId, VendorId, Remark, Dr, Cr, PostingDate.ToString("yyyy-MM-dd"));
                        using (SqlCommand cmd1 = new SqlCommand(q, conn))
                        {

                            if (conn.State != ConnectionState.Open)
                            {

                                conn.Open();
                            }
                            HowManyInsert = cmd1.ExecuteNonQuery();

                        }
                    }
                    //Dr Entry
                    blCoaLedger.TransactionDate = dtpRec.Value;
                    blCoaLedger.TransactionType = "Vendor Payment Voucher";
                    blCoaLedger.TransactionId = int.Parse(txtRecId.Text);
                    blCoaLedger.ChartOfAccountId = (int)cmbHead.SelectedValue;
                    blCoaLedger.Remark = txtRemark.Text;
                    blCoaLedger.Dr = 0;
                    blCoaLedger.Cr = int.Parse(txtAmount.Text);
                    dlCoaLedger.Insert(blCoaLedger);
                    //Cr Entry
                    blCoaLedger.TransactionDate = dtpRec.Value;
                    blCoaLedger.TransactionType = "Vendor Payment Voucher";
                    blCoaLedger.TransactionId = int.Parse(txtRecId.Text);
                    blCoaLedger.ChartOfAccountId = DlVendor.VendorAccountId((int)cmbCustomer.SelectedValue);
                    blCoaLedger.Remark = txtRemark.Text;
                    blCoaLedger.Dr = int.Parse(txtAmount.Text);
                    blCoaLedger.Cr = 0;
                    dlCoaLedger.Insert(blCoaLedger);

                    MessageBox.Show("Your Transaction has been done.");
                    DisableField();
                    btnSave.Enabled = false;
                    btnNew.Enabled = true;
                    lblLastID.Text = DlPayment.LastId().ToString();
                    btnFirst.Enabled = btnNext.Enabled = btnPrevious.Enabled = btnLast.Enabled = true;
                    btnPrint.Focus();
                }

            }

        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            btnSave.Enabled = true;
            btnNew.Enabled = false;
            EnableField();
            ClearData();
            txtRecId.Text = DlPayment.NewId().ToString();
            btnFirst.Enabled = false;
            btnNext.Enabled = false;
            btnPrevious.Enabled = false;
            btnLast.Enabled = false;
            dtpRec.Focus();
            dtpRec.Value = DateTime.Now;
        }

        private void cmbCustomer_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                lblPreviousBalance.Text = DlVendor.VendorLastBalance((int)cmbCustomer.SelectedValue).ToString();
            }
            catch
            {


            }
        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            lblCurrentId.Text = lblFirstID.Text;
            MessageBox.Show(lblCurrentId.Text);
            FillDataInField();
        }

        private void FillDataInField()
        {
            DataTable dt = DlPayment.ViewPayment(int.Parse(lblCurrentId.Text));
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
                txtAmount.Text = dr[6].ToString();
                lblPreviousBalance.Text = dr[8].ToString();
                lblCurrentBalance.Text = dr[9].ToString();

                if (cmbCustomer.Text == string.Empty)
                {
                    cmbCustomer.Text = "Deactive Customer";
                }
            }

        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            lblCurrentId.Text = lblLastID.Text;
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

        private void btnModify_Click(object sender, EventArgs e)
        {
            if (IsValidated())
            {
                BlPayment.PaymentID = int.Parse(txtRecId.Text);
                BlPayment.PaymentDate = dtpRec.Value;
                BlPayment.HeadID = (int)cmbHead.SelectedValue;
                //BlReceipt.Zone = (int)cmbZone.SelectedValue;
                //BlReceipt.Area = (int)cmbAddress.SelectedValue;
                BlPayment.VendorID = (int)cmbCustomer.SelectedValue;
                BlPayment.Amount = int.Parse(txtAmount.Text);
                //BlPayment.AmountInEnglish = "Not Available";
                BlPayment.PayRemark = txtRemark.Text;
                BlPayment.PreviousAmount = int.Parse(lblPreviousBalance.Text);
                //BlReceipt.LastBill = string.Empty;
                //BlReceipt.LastReceipt = string.Empty;
                //BlReceipt.CurrentBalance = int.Parse(lblCurrentBalance.Text);
                BlPayment.EntryDate = DateTime.Now;
                if (DlPayment.Update(BlPayment) > 0)
                {
                    DateTime TransactionDate = dtpRec.Value;
                    string TransactionType = "Payment Voucher";
                    int TransactionId = int.Parse(txtAmount.Text);
                    int VendorId = (int)cmbCustomer.SelectedValue;
                    string Remark = txtRemark.Text;
                    int Dr = 0;
                    int Cr = int.Parse(txtAmount.Text);
                    DateTime PostingDate = DateTime.Now;
                    //string TranTypeUrdu = "خرید";
                    //if (DlTransaction.Insert(BlTransaction) > 0)
                    //{
                    //    MessageBox.Show("Your ledger has been done...");
                    //    lblIsPost.Text = "Invoice";

                    //}
                    int HowManyInsert = 0;
                    using (SqlConnection conn = new SqlConnection(clsConnectionString.cs))
                    {
                        string q = string.Format("Update tblTransactionVendor set TransactionDate = '{0}',VendorId = {1} , Remark = '{2}',Dr= {3},Cr={4} where LedgerType = '{5}' and TranId = {6}", 
                            TransactionDate.ToString("yyyy-MM-dd"),VendorId,Remark,Dr,Cr,TransactionType,int.Parse(txtRecId.Text));
                        using (SqlCommand cmd1 = new SqlCommand(q, conn))
                        {

                            if (conn.State != ConnectionState.Open)
                            {

                                conn.Open();
                            }
                            HowManyInsert = cmd1.ExecuteNonQuery();

                        }
                    }


                    //Dr Entry
                    blCoaLedger.TransactionDate = dtpRec.Value;
                    blCoaLedger.TransactionType = "Vendor Payment Voucher";
                    blCoaLedger.TransactionId = int.Parse(txtRecId.Text);
                    blCoaLedger.ChartOfAccountId = (int)cmbHead.SelectedValue;
                    blCoaLedger.Remark = txtRemark.Text;
                    blCoaLedger.Dr = 0;
                    blCoaLedger.Cr = int.Parse(txtAmount.Text);
                    dlCoaLedger.UpdateLedger(blCoaLedger);
                    //Cr Entry
                    blCoaLedger.TransactionDate = dtpRec.Value;
                    blCoaLedger.TransactionType = "Vendor Payment Voucher";
                    blCoaLedger.TransactionId = int.Parse(txtRecId.Text);
                    blCoaLedger.ChartOfAccountId = DlVendor.VendorAccountId((int)cmbCustomer.SelectedValue);
                    blCoaLedger.Remark = txtRemark.Text;
                    blCoaLedger.Dr = int.Parse(txtAmount.Text);
                    blCoaLedger.Cr = 0;
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

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnNext.Enabled = btnPrevious.Enabled = btnFirst.Enabled = btnLast.Enabled = false;
            btnNew.Enabled = btnNew.Enabled = btnPrint.Enabled = false;
            btnModify.Enabled = true;
            EnableField();
        }

        private void txtAmount_TextChanged_1(object sender, EventArgs e)
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

        private void createCustomerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmCreateVendor cv = new frmCreateVendor();
            cv.ShowDialog();
        }

        private void createHeadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmHeadOfAcc ha = new frmHeadOfAcc();
            ha.ShowDialog();
        }

        private void viewReceiptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmViewPayment vp = new frmViewPayment();
            vp.ShowDialog();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            VendorReporting.frmPrintPaymentVoucher pv = new VendorReporting.frmPrintPaymentVoucher();
            pv.PaymentVoucherId = int.Parse(txtRecId.Text);
            pv.ShowDialog();
        }

        private void dtpRec_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.SelectNextControl((Control)sender, true, true, true, true);
                e.SuppressKeyPress = true;
            }
        }

        private void frmPayment_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                if (btnNew.Enabled == true)
                {
                    btnNew_Click(null, new EventArgs());
                }
            }
            else if (e.KeyCode == Keys.F2)
            {
                if (btnSave.Enabled == true)
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


    }
}

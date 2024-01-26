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
using AnajPos.BAL;
using AnajPos.DAL;
using AnajPos.BAL_Cash_BANK;
using AnajPos.DAL_Cash_Bank;

namespace AnajPos.GUI
{
    public partial class frmAdjustment : frmTemplete
    {
        clsZone cz = new clsZone();
        clsCustomer DlCustomer = new clsCustomer();
        clsBlCustomer BlCustomer = new clsBlCustomer();
        clsAddress DlAddress = new clsAddress();
        clsBlAddress BlAddress = new clsBlAddress();
        clsTransaction DlTransaction = new clsTransaction();
        clsBlTransaction BlTransaction = new clsBlTransaction();
        clsAdjustment DlAdjustment = new clsAdjustment();
        clsBlAdjustment BlAdjustment = new clsBlAdjustment();
        clsBLChartOfAccountLedger blCoaLedger = new clsBLChartOfAccountLedger();
        clsChartOfAccountLedger dlCoaLedger = new clsChartOfAccountLedger();
        public frmAdjustment()
        {
            InitializeComponent();
        }

        private void frmAdjustment_Load(object sender, EventArgs e)
        {
            cmbZone.SelectedValueChanged -= new EventHandler(cmbZone_SelectedValueChanged);
            DataTable dtView = cz.ViewZone();
            cmbZone.DataSource = dtView;
            cmbZone.ValueMember = "Id";
            cmbZone.DisplayMember = "Name of Zone";
            cmbZone.SelectedIndex = -1;
            cmbCustomer.Text = string.Empty;
            cmbZone.SelectedValueChanged += new EventHandler(cmbZone_SelectedValueChanged);
            DisableField();
            btnNew.Focus();
            
            dgViewTransaction.DataSource = DlAdjustment.ViewAdjustment(dtpViewRecord.Text);
           
        }

        private void cmbZone_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cmbZone.SelectedIndex != -1)
            {
                BlAddress.AddressId = (int)cmbZone.SelectedValue;
                cmbAddress.DataSource = DlAddress.ViewAddressByZone(BlAddress);
                cmbAddress.DisplayMember = "AddressName";
                cmbAddress.ValueMember = "AddressID";
            //    cmbAddress.SelectedIndex = -1;
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

        private void button4_Click(object sender, EventArgs e)
        {
            if (button4.Text == "Update")
            {
                BlAdjustment.Id = int.Parse(txtId.Text);
                BlAdjustment.TranTime = dtpTransactionDate.Value;
                BlAdjustment.ZoneId = (int)cmbZone.SelectedValue;
                BlAdjustment.AddressId = (int)cmbAddress.SelectedValue;
                BlAdjustment.CustomerId = (int)cmbCustomer.SelectedValue;
                BlAdjustment.Dr = int.Parse(txtDr.Text);
                BlAdjustment.Cr = int.Parse(txtCr.Text);
                BlAdjustment.Remark = txtNotes.Text;
                if (DlAdjustment.Update(BlAdjustment)> 0)
                {
                    BlTransaction.TransactionDate = dtpTransactionDate.Value;
                    BlTransaction.TransactionType = "Adjustment";
                    BlTransaction.TransactionId = int.Parse(txtId.Text);
                    BlTransaction.CustomerId = (int)cmbCustomer.SelectedValue;
                    BlTransaction.Remark = txtNotes.Text;
                    BlTransaction.Dr = int.Parse(txtDr.Text);
                    BlTransaction.Cr = int.Parse(txtCr.Text);
                    BlTransaction.PostingDate = DateTime.Now;
                    BlTransaction.TranTypeUrdu = "ایڈجسٹمینٹ";
                    if (DlTransaction.UpdateAdjustment(BlTransaction) > 0)
                    {
                        //Dr Entry
                        blCoaLedger.TransactionDate = dtpTransactionDate.Value;
                        blCoaLedger.TransactionType = "Customer Adjustment";
                        blCoaLedger.TransactionId = int.Parse(txtId.Text);
                        blCoaLedger.ChartOfAccountId = DlCustomer.CustomerAccountId((int)cmbCustomer.SelectedValue);
                        blCoaLedger.Remark = txtNotes.Text;
                        blCoaLedger.Dr = int.Parse(txtDr.Text);
                        blCoaLedger.Cr = int.Parse(txtCr.Text);
                        dlCoaLedger.UpdateLedger(blCoaLedger);

                        MessageBox.Show("Your Adjustment has been Updated...");

                        dtpViewRecord.Value = dtpTransactionDate.Value;
                        button4.Text = "Save";
                        button4.Enabled = false;
                        btnNew.Enabled = true;
                        ResetControl();
                        DisableField();
                        button4.Enabled = false;
                        btnNew.Enabled = true;
                    }
                }
            }
            else
            {
                BlAdjustment.Id = int.Parse(txtId.Text);
                BlAdjustment.TranTime = dtpTransactionDate.Value;
                BlAdjustment.ZoneId = (int)cmbZone.SelectedValue;
                BlAdjustment.AddressId = (int)cmbAddress.SelectedValue;
                BlAdjustment.CustomerId = (int)cmbCustomer.SelectedValue;
                BlAdjustment.Dr = int.Parse(txtDr.Text);
                BlAdjustment.Cr = int.Parse(txtCr.Text);
                BlAdjustment.Remark = txtNotes.Text;
                if (DlAdjustment.Insert(BlAdjustment) > 0)
                {
                    BlTransaction.TransactionDate = dtpTransactionDate.Value;
                    BlTransaction.TransactionType = "Adjustment";
                    BlTransaction.TransactionId = int.Parse(txtId.Text);
                    BlTransaction.CustomerId = (int)cmbCustomer.SelectedValue;
                    BlTransaction.Remark = txtNotes.Text;
                    BlTransaction.Dr = int.Parse(txtDr.Text);
                    BlTransaction.Cr = int.Parse(txtCr.Text);
                    BlTransaction.PostingDate = DateTime.Now;
                    BlTransaction.TranTypeUrdu = "ایڈجسٹمینٹ";
                    if (DlTransaction.Insert(BlTransaction) > 0)
                    {
                        //Dr Entry
                        blCoaLedger.TransactionDate = dtpTransactionDate.Value;
                        blCoaLedger.TransactionType = "Customer Adjustment";
                        blCoaLedger.TransactionId = int.Parse(txtId.Text);
                        blCoaLedger.ChartOfAccountId = DlCustomer.CustomerAccountId((int)cmbCustomer.SelectedValue);
                        blCoaLedger.Remark = txtNotes.Text;
                        blCoaLedger.Dr = int.Parse(txtDr.Text);
                        blCoaLedger.Cr = int.Parse(txtCr.Text);
                        dlCoaLedger.Insert(blCoaLedger);

                        MessageBox.Show("Your Adjustment has been done...");
                        dtpViewRecord.Value = dtpTransactionDate.Value;
                        ResetControl();
                        DisableField();
                        button4.Enabled = false;
                        btnNew.Enabled = true;
                    }

                }
            }
        }
        private void ResetControl()
        {
            txtId.Text = string.Empty;
            cmbZone.SelectedIndex = -1;
            cmbAddress.DataSource = null;
            cmbCustomer.DataSource = null;
            dtpTransactionDate.Value = DateTime.Now;
            txtDr.Text = "0";
            txtCr.Text = "0";
            txtNotes.Text = string.Empty;
            cmbCustomer.Text = string.Empty;
            cmbAddress.Text = string.Empty;
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            EnableField();
            txtId.Text = DlAdjustment.NewId().ToString();
            button4.Enabled = true;
            btnNew.Enabled = false;
            txtId.Focus();
        }
        private void EnableField()
        {
            txtId.Enabled = true;
            cmbZone.Enabled = true;
            cmbAddress.Enabled = true;
            cmbCustomer.Enabled = true;
            txtDr.Enabled = true;
            txtCr.Enabled = true;
            txtNotes.Enabled = true;
        }
        private void DisableField()
        {
            txtId.Enabled = false;
            cmbCustomer.Enabled = false;
            cmbZone.Enabled = false;
            cmbAddress.Enabled = false;
            txtDr.Enabled = false;
            txtCr.Enabled = false;
            txtNotes.Enabled = false;
        }

        private void txtDr_KeyPress(object sender, KeyPressEventArgs e)
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

        private void dtpViewRecord_ValueChanged(object sender, EventArgs e)
        {
            
            dgViewTransaction.DataSource = DlAdjustment.ViewAdjustment(dtpViewRecord.Text);
            chkViewAllData.Checked = false; 
        }

        private void dgViewTransaction_DoubleClick(object sender, EventArgs e)
        {
            int GetRow = dgViewTransaction.Rows.GetFirstRow(DataGridViewElementStates.Selected);
            txtId.Text = dgViewTransaction.Rows[GetRow].Cells[0].Value.ToString();
            BlAdjustment.Id = int.Parse(txtId.Text);
            DataTable dt = DlAdjustment.ViewAdjustmentById(BlAdjustment);
            DataRow dr = dt.Rows[0];
            txtId.Text = dr[0].ToString();
            dtpTransactionDate.Value = (DateTime)dr[1];
            cmbZone.SelectedValue = (int)dr[2];
            cmbAddress.SelectedValue = (int)dr[3];
            cmbCustomer.SelectedValue = (int)dr[4];
            txtDr.Text = dr[5].ToString();
            txtCr.Text = dr[6].ToString();
            txtNotes.Text = dr[7].ToString();
            button4.Text = "Update";
            btnNew.Enabled = false;
            button4.Enabled = true;
            EnableField();
        }

        private void chkViewAllData_CheckedChanged(object sender, EventArgs e)
        {
            if (chkViewAllData.Checked)
            {
                dgViewTransaction.DataSource = DlAdjustment.ViewAdjustment();
            }
        }

        private void txtId_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.SelectNextControl((Control)sender, true, true, true, true);
                e.SuppressKeyPress = true;
            }
        }
     
    }
}

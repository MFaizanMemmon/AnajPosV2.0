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
using System.IO;
using AnajPos.BAL_Cash_BANK;
using AnajPos.DAL_Cash_Bank;

namespace AnajPos.GUI_Vendor
{
    public partial class frmCreateVendor : frmTemplete
    {
        clsZone cz = new clsZone();
        clsDlVendor DlVendor = new clsDlVendor();
        clsBlVendor BlVendor=new clsBlVendor();
        clsAddress DlAddress = new clsAddress();
        clsBlAddress BlAddress = new clsBlAddress();
        //clsTransaction DlTransaction = new clsTransaction();
        //clsBlTransaction BlTransaction = new clsBlTransaction();
        
        clsBLChartOfAccountLedger blCoaLedger = new clsBLChartOfAccountLedger();
        clsChartOfAccountLedger dlCoaLedger = new clsChartOfAccountLedger();

        public frmCreateVendor()
        {
            InitializeComponent();
        }

        private void EnableOrDisableField(bool FieldTYpe)
        {
            txtVendorID.Enabled = FieldTYpe;
            cmbZone.Enabled = FieldTYpe;
            txtVendorName.Enabled = FieldTYpe;
            cmbAddress.Enabled = FieldTYpe;
            txtProperAddress.Enabled = FieldTYpe;
            dtpOpeningDate.Enabled = FieldTYpe;
            txtOpeningBalance.Enabled = FieldTYpe;
            mtbPhone.Enabled = FieldTYpe;
            mtbPhone2.Enabled = FieldTYpe;
            chkIsActive.Enabled = FieldTYpe;
            mtbTel.Enabled = FieldTYpe;
            txtRemark.Enabled = FieldTYpe;
        }
        
        private void ClearField()
        {
            //txtVendorID.Text = string.Empty;
            cmbZone.SelectedIndex = -1;
            txtVendorName.Text = string.Empty;
            cmbAddress.SelectedIndex = -1;
            txtProperAddress.Text = string.Empty;
            dtpOpeningDate.Value = DateTime.Now;
            txtOpeningBalance.Text = string.Empty;
            mtbPhone.Text = string.Empty;
            mtbPhone.Text = string.Empty;
            mtbTel.Text = string.Empty;
            txtRemark.Text = string.Empty;
            chkIsActive.Checked = false;
        }
        private bool DataValidated()
        {
            if (cmbZone.SelectedIndex == -1)
            {
                cmbZone.Focus();
                MessageBox.Show("Please Select the Zone");
                return false;
            }
            else if (txtVendorName.Text == string.Empty)
            {
                txtVendorName.Focus();
                MessageBox.Show("Please Enter Vendor Name");
                return false;
            }
            else if (cmbAddress.SelectedIndex == -1)
            {
                cmbAddress.Focus();
                MessageBox.Show("Please Enter Address");
                return false;
            }
            else if (txtOpeningBalance.Text == string.Empty)
            {
                txtOpeningBalance.Focus();
                MessageBox.Show("Please Enter Opening Amount");
                return false;
            }
            else if (dtpOpeningDate.Value >= DateTime.Now)
            {
                dtpOpeningDate.Focus();
                MessageBox.Show("Please Select Less Today Date");
                return false;
            }
            
            return true;
        }
        private byte[] InsertCustomerImage()
        {
            MemoryStream ms = new MemoryStream();
            pbCustomer.Image.Save(ms, pbCustomer.Image.RawFormat);
            return ms.GetBuffer();
        }

        private void FillDateInFields()
        {
            //txtCustomerID.Text = lblFirst.Text;
            BlVendor.VendorId = int.Parse(lblCurrent.Text);
            DataTable dt = DlVendor.View(BlVendor);
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                txtVendorID.Text = dr[0].ToString();
                txtVendorName.Text = dr[1].ToString();
                //txtCustomerNameUrdu.Text = dr[2].ToString();
                cmbZone.SelectedValue = (int)dr[2];
                cmbAddress.SelectedValue = (int)dr[3];
                txtProperAddress.Text = dr[4].ToString();
                pbCustomer.Image = RetriveImage((byte[])dr[5]);
                mtbPhone.Text = dr[6].ToString();
                mtbPhone2.Text = dr[7].ToString();
                mtbTel.Text = dr[8].ToString();
                txtRemark.Text = dr[9].ToString();
                dtpOpeningDate.Value = (DateTime)dr[10];
                txtOpeningBalance.Text = dr[11].ToString();
                chkIsActive.Checked = (bool)dr[12];

            }
        }
        private Image RetriveImage(byte[] StudentPhoto)
        {
            MemoryStream ms = new MemoryStream(StudentPhoto);
            return Image.FromStream(ms);

        }
        
        private void btnNew_Click(object sender, EventArgs e)
        {
            EnableOrDisableField(true);
            txtVendorID.Text = DlVendor.NewId().ToString();
            ClearField();
            btnSave.Enabled = true;
            btnNew.Enabled = false;
            cmbZone.Focus();
            pbCustomer.Image = Properties.Resources.NoImage;
        }
       
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (DataValidated())
            {
                BlVendor.VendorId=Convert.ToInt32(txtVendorID.Text);
                BlVendor.VendorName=txtVendorName.Text;
                BlVendor.AddressName=(int)cmbAddress.SelectedValue;
                BlVendor.ProperAddress=txtProperAddress.Text;
                BlVendor.IsActive=chkIsActive.Checked;
                BlVendor.ClosingDate=dtpOpeningDate.Value;
                BlVendor.OpeningAmount=Convert.ToInt32(txtOpeningBalance.Text);
                BlVendor.Phone1=mtbPhone.Text;
                BlVendor.Phone2=mtbPhone2.Text;
                BlVendor.Telephone=mtbPhone2.Text;
                BlVendor.Remark=txtRemark.Text;
                BlVendor.Image = InsertCustomerImage();
                if(DlVendor.Insert(BlVendor) > 0)
                {
                    //BlTransaction.TransactionDate = dtpOpeningDate.Value;
                    //BlTransaction.TransactionType = "Ledger";
                    //BlTransaction.TransactionId = 0;
                    //BlTransaction.CustomerId = int.Parse(txtVendorID.Text);
                    //BlTransaction.Remark = txtRemark.Text;
                    //BlTransaction.Dr = int.Parse(txtOpeningBalance.Text);
                    //BlTransaction.Cr = 0;
                    //BlTransaction.PostingDate = DateTime.Now;
                    //BlTransaction.TranTypeUrdu = "کھاتا بنا";
                    //DlTransaction.Insert(BlTransaction);
                    //Dr Entry
                    //blCoaLedger.TransactionDate = dtpOpeningDate.Value;
                    //blCoaLedger.TransactionType = "Create Vendor Account";
                    //blCoaLedger.TransactionId = int.Parse(txtVendorID.Text);
                    //blCoaLedger.ChartOfAccountId = 0;
                    //blCoaLedger.Remark = txtRemark.Text;
                    //blCoaLedger.Dr = 0;
                    //blCoaLedger.Cr = 0;
                    //dlCoaLedger.Insert(blCoaLedger);
                    //Cr Entry
                    blCoaLedger.TransactionDate = dtpOpeningDate.Value;
                    blCoaLedger.TransactionType = "Create Vendor Account";
                    blCoaLedger.TransactionId = int.Parse(txtVendorID.Text);
                    blCoaLedger.ChartOfAccountId = DlVendor.VendorAccountId(int.Parse(txtVendorID.Text));
                    blCoaLedger.Remark = txtRemark.Text;
                    blCoaLedger.Dr = 0;
                    blCoaLedger.Cr = int.Parse(txtOpeningBalance.Text);
                    dlCoaLedger.Insert(blCoaLedger);


                    btnSave.Enabled = false;
                    btnNew.Enabled = true;
                    //ClearFields();
                    //txtCustomerID.Text = string.Empty;
                    EnableOrDisableField(false);
                    lblLast.Text = DlVendor.MaxId().ToString();   
                    
                    MessageBox.Show("Account has been Created...");
                }
            }
        }

        private void frmCreateVendor_Load(object sender, EventArgs e)
        {
            cmbZone.SelectedValueChanged -= new EventHandler(cmbZone_SelectedValueChanged);
            DataTable dtView = cz.ViewZone();
            cmbZone.DataSource = dtView;
            cmbZone.ValueMember = "Id";
            cmbZone.DisplayMember = "Name of Zone";
            cmbZone.SelectedIndex = -1;
            cmbZone.SelectedValueChanged += new EventHandler(cmbZone_SelectedValueChanged);

            lblFirst.Text = DlVendor.MinId().ToString();
            lblLast.Text = DlVendor.MaxId().ToString();
            lblCurrent.Text = lblFirst.Text;
            FillDateInFields();
            this.Focus();

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

        private void pbCustomer_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                pbCustomer.Image = new Bitmap(ofd.FileName);

            }  
        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            lblCurrent.Text = lblFirst.Text;
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

        private void btnLast_Click(object sender, EventArgs e)
        {
            lblCurrent.Text = lblLast.Text;
            FillDateInFields();
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            if (DataValidated())
            {
                BlVendor.VendorId = int.Parse(txtVendorID.Text);
                BlVendor.VendorName = txtVendorName.Text;
                //BlVendor.CustomerNameUrdu = txtCustomerNameUrdu.Text;
                //BlVendor.ZoneId = (int)cmbZone.SelectedValue;
                BlVendor.AddressName = (int)cmbAddress.SelectedValue;
                BlVendor.IsActive = chkIsActive.Checked;
                BlVendor.ProperAddress = txtProperAddress.Text;
                BlVendor.Image = InsertCustomerImage();
                BlVendor.ClosingDate = dtpOpeningDate.Value;
                BlVendor.OpeningAmount = int.Parse(txtOpeningBalance.Text);
                BlVendor.Phone1 = mtbPhone.Text;
                BlVendor.Phone2 = mtbPhone2.Text;
                BlVendor.Telephone = mtbTel.Text;
                BlVendor.Remark = txtRemark.Text;
                if (DlVendor.Update(BlVendor) > 0)
                {
                    blCoaLedger.TransactionDate = dtpOpeningDate.Value;
                    blCoaLedger.TransactionType = "Create Vendor Account";
                    blCoaLedger.TransactionId = int.Parse(txtVendorID.Text);
                    blCoaLedger.ChartOfAccountId = DlVendor.VendorAccountId(int.Parse(txtVendorID.Text));
                    blCoaLedger.Remark = txtRemark.Text;
                    blCoaLedger.Dr = 0;
                    blCoaLedger.Cr = int.Parse(txtOpeningBalance.Text);
                    dlCoaLedger.UpdateLedger(blCoaLedger);

                    MessageBox.Show("Data has been Updated...");
                    DisableField();
                    btnModify.Enabled = false;
                    btnNew.Enabled = true;
                    btnFirst.Enabled = btnNext.Enabled = btnPrevious.Enabled = btnLast.Enabled = true;
                    btnView.Enabled = true;
            
                }
            }
        }
        private void DisableField()
        {
            txtVendorID.Enabled = false;
            txtVendorID.Enabled = false;
            txtCustomerNameUrdu.Enabled = false;
            cmbZone.Enabled = false;
            cmbAddress.Enabled = false;
            txtProperAddress.Enabled = false;
            dtpOpeningDate.Enabled = false;
            txtOpeningBalance.Enabled = false;
            mtbPhone.Enabled = false;
            mtbPhone2.Enabled = false;
            mtbTel.Enabled = false;
            txtRemark.Enabled = false;
            pbCustomer.Enabled = false;
            chkIsActive.Enabled = false;
        }
        private void alterationModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EnableOrDisableField(true);
            txtVendorID.ReadOnly = true;
            btnNew.Enabled = btnSave.Enabled = btnView.Enabled = btnFirst.Enabled = btnNext.Enabled = btnPrevious.Enabled = btnLast.Enabled = false;
            btnModify.Enabled = true;
       
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            frmViewVendor vd = new frmViewVendor();
            vd.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtSearch.Text != string.Empty)
            {
                if (int.Parse(txtSearch.Text) >= int.Parse(lblFirst.Text) && int.Parse(txtSearch.Text) <= int.Parse(lblLast.Text))
                {
                    lblCurrent.Text = txtSearch.Text;
                    FillDateInFields();
                }
                else
                {
                    MessageBox.Show("Your data has been not found...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }

        private void cmbZone_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.SelectNextControl((Control)sender, true, true, true, true);
                e.SuppressKeyPress = true;
            }
        }

        private void createZoneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmZone fz = new frmZone();
            fz.ShowDialog();

            cmbZone.SelectedValueChanged -= new EventHandler(cmbZone_SelectedValueChanged);
            cmbZone.DataSource = null;
            DataTable dtView = cz.ViewZone();
            cmbZone.DataSource = dtView;
            cmbZone.ValueMember = "Id";
            cmbZone.DisplayMember = "Name of Zone";
            cmbZone.SelectedIndex = -1;
            cmbZone.SelectedValueChanged += new EventHandler(cmbZone_SelectedValueChanged);
        }

        private void createAddresssToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                frmAddress fa = new frmAddress();
                fa.ShowDialog();
                cmbAddress.DataSource = null;
                if (cmbZone.SelectedIndex == -1)
                {
                    cmbZone.SelectedIndex = 1;
                    return;
                }
                BlAddress.AddressId = (int)cmbZone.SelectedValue;
                cmbAddress.DataSource = DlAddress.ViewAddressByZone(BlAddress);
                cmbAddress.DisplayMember = "AddressName";
                cmbAddress.ValueMember = "AddressID";
            }
            catch
            {

               
            }
        }

        private void frmCreateVendor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                // MessageBox.Show("apka kam ho rha hai");
                //Coke.Click += new EventHandler(btnNew_Click);
                //Coke.PerformClick();
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
                else
                {
                    MessageBox.Show("Please Create the Customer");
                }
            }
            else if (e.KeyCode == Keys.F3)
            {
                if (btnModify.Enabled == true)
                {
                    btnModify_Click(null, new EventArgs());
                }
                else
                {
                    MessageBox.Show("Please Alteration Mode On");
                }
            }
            else if (e.KeyCode == Keys.F5)
            {
                if (btnView.Enabled == true)
                {
                    btnView_Click(null, new EventArgs());
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
            else if (e.KeyCode == Keys.F10)
            {
                if (pbCustomer.Enabled == true)
                {
                    pbCustomer_Click(null, new EventArgs());
                }
            }
            //else if (e.KeyCode == Keys.Escape)
            //{
            //    this.Close();
            //}
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

        
        
    }
}

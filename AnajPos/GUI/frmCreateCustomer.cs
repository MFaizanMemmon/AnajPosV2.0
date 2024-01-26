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
using System.IO;
using AnajPos.BAL_Cash_BANK;
using AnajPos.DAL_Cash_Bank;

namespace AnajPos.GUI
{
    public partial class frmCreateCustomer : frmTemplete
    {
        clsZone cz = new clsZone();
        clsCustomer DlCustomer = new clsCustomer();
        clsBlCustomer BlCustomer = new clsBlCustomer();
        clsAddress DlAddress = new clsAddress();
        clsBlAddress BlAddress = new clsBlAddress();
        clsTransaction DlTransaction = new clsTransaction();
        clsBlTransaction BlTransaction = new clsBlTransaction();
        clsBLChartOfAccountLedger blCoaLedger = new clsBLChartOfAccountLedger();
        clsChartOfAccountLedger dlCoaLedger = new clsChartOfAccountLedger();

        public frmCreateCustomer()
        {
            InitializeComponent();
        }

        private void frmCreateCustomer_Load(object sender, EventArgs e)
        {
            cmbZone.SelectedValueChanged -= new EventHandler(cmbZone_SelectedValueChanged);
            DataTable dtView = cz.ViewZone();
            cmbZone.DataSource = dtView;
            cmbZone.ValueMember = "Id";
            cmbZone.DisplayMember = "Name of Zone";
            cmbZone.SelectedIndex = -1;
            cmbZone.SelectedValueChanged += new EventHandler(cmbZone_SelectedValueChanged);

            lblFirst.Text = DlCustomer.MinId().ToString();
            lblLast.Text = DlCustomer.MaxId().ToString();
            lblCurrent.Text = lblFirst.Text;
            FillDateInFields();
            this.Focus();

        }
        private void FillDateInFields()
        {
            //txtCustomerID.Text = lblFirst.Text;
            BlCustomer.CustomerID = int.Parse(lblCurrent.Text);
            DataTable dt= DlCustomer.View(BlCustomer);
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                txtCustomerID.Text=dr[0].ToString();
                txtCustomerName.Text = dr[1].ToString();
                txtCustomerNameUrdu.Text = dr[2].ToString();
                cmbZone.SelectedValue = (int)dr[3];
                cmbAddress.SelectedValue = (int)dr[4];
                txtProperAddress.Text = dr[5].ToString();
                pbCustomer.Image = RetriveImage((byte[])dr[6]);
                mtbPhone.Text = dr[7].ToString();
                mtbPhone2.Text = dr[8].ToString();
                mtbTel.Text = dr[9].ToString();
                txtRemark.Text = dr[10].ToString();
                dtpOpeningDate.Value =(DateTime) dr[11];
                txtOpeningBalance.Text = dr[12].ToString();
                chkIsActive.Checked = (bool)dr[13];

            }
        }
        private Image RetriveImage(byte[] StudentPhoto)
        {
            MemoryStream ms = new MemoryStream(StudentPhoto);
            return Image.FromStream(ms);

        }

        private void btnView_Click(object sender, EventArgs e)
        {
            frmViewCustomer fvc = new frmViewCustomer();
            fvc.ShowDialog();
        }

        private void createZoneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmZone fz = new frmZone();
            fz.ShowDialog();
        }

        private void createAddressToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddress fa = new frmAddress();
            fa.ShowDialog();
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (IsValidated())
            {
                BlCustomer.CustomerID = int.Parse(txtCustomerID.Text);
                BlCustomer.CustomerName = txtCustomerName.Text;
                BlCustomer.CustomerNameUrdu = txtCustomerNameUrdu.Text;
                BlCustomer.ZoneId = (int)cmbZone.SelectedValue;
                BlCustomer.AddressId = (int)cmbAddress.SelectedValue;
                BlCustomer.IsActive = chkIsActive.Checked;
                BlCustomer.ProperAddress = txtProperAddress.Text;
                BlCustomer.Picture = InsertCustomerImage();
                BlCustomer.ClosingDate = dtpOpeningDate.Value;
                BlCustomer.OpeningAmount = int.Parse(txtOpeningBalance.Text);
                BlCustomer.Phone1 = mtbPhone.Text;
                BlCustomer.Phone2 = mtbPhone2.Text;
                BlCustomer.Telephone = mtbTel.Text;
                BlCustomer.Remark = txtRemark.Text;
                if (DlCustomer.Insert(BlCustomer)> 0)
                {
                    

                    BlTransaction.TransactionDate = dtpOpeningDate.Value;
                    BlTransaction.TransactionType = "Ledger";
                    BlTransaction.TransactionId = 0;
                    BlTransaction.CustomerId = int.Parse(txtCustomerID.Text);
                    BlTransaction.Remark = txtRemark.Text;
                    BlTransaction.Dr = 0;
                    BlTransaction.Cr = int.Parse(txtOpeningBalance.Text);
                    BlTransaction.PostingDate = DateTime.Now;
                    BlTransaction.TranTypeUrdu = "کھاتا بنا";
                    if (DlTransaction.Insert(BlTransaction) > 0)
                    {
                        //Dr Entry
                        blCoaLedger.TransactionDate = dtpOpeningDate.Value;
                        blCoaLedger.TransactionType = "Create Customer Account";
                        blCoaLedger.TransactionId = int.Parse(txtCustomerID.Text);
                        blCoaLedger.ChartOfAccountId = DlCustomer.CustomerAccountId(int.Parse(txtCustomerID.Text));
                        blCoaLedger.Remark = txtRemark.Text;
                        blCoaLedger.Dr = int.Parse(txtOpeningBalance.Text);
                        blCoaLedger.Cr = 0;
                        dlCoaLedger.Insert(blCoaLedger);
                        //Cr Entry
                        //blCoaLedger.TransactionDate = dtpOpeningDate.Value;
                        //blCoaLedger.TransactionType = "Create Customer Account";
                        //blCoaLedger.TransactionId = int.Parse(txtCustomerID.Text);
                        //blCoaLedger.ChartOfAccountId = 0;
                        //blCoaLedger.Remark = txtRemark.Text;
                        //blCoaLedger.Dr = 0;
                        //blCoaLedger.Cr = 0;
                        //dlCoaLedger.Insert(blCoaLedger);

                        MessageBox.Show("Your Account has been Created...");
                        btnSave.Enabled = false;
                        btnNew.Enabled = true;
                        //ClearFields();
                        //txtCustomerID.Text = string.Empty;
                        DisableField();
                        lblLast.Text = DlCustomer.MaxId().ToString();   
                    }
                
                }

                
            }
        }

        private byte[] InsertCustomerImage()
        {
            MemoryStream ms = new MemoryStream();
            pbCustomer.Image.Save(ms, pbCustomer.Image.RawFormat);
            return ms.GetBuffer();
        }
        private bool IsValidated()
        {
            if (txtCustomerID.Text == string.Empty)
            {
                return false;
            }
            else if(txtCustomerName.Text == string.Empty)
            {
                return false;
            }
            else if (txtCustomerNameUrdu.Text== string.Empty)
            {
                return false;
            }
            else if (cmbZone.SelectedIndex==-1)
            {
                return false;
            }
            else if (cmbAddress.SelectedIndex == -1)
            {
                return false;
            }
            else if (dtpOpeningDate.Value > DateTime.Now)
            {
                return false;
            }
            else if (txtOpeningBalance.Text== string.Empty)
            {
                return false;
            }
            
            return true;

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

        private void btnNew_Click(object sender, EventArgs e)
        {
            EnableField();
            txtCustomerID.Text = DlCustomer.NewId().ToString();
            ClearFields();
            btnSave.Enabled = true;
            btnNew.Enabled = false;
            cmbZone.Focus();
            pbCustomer.Image = Properties.Resources.NoImage;
        }
        private void ClearFields()
        {
            txtCustomerName.Text = string.Empty;
            txtCustomerNameUrdu.Text = string.Empty;
            cmbAddress.SelectedIndex = -1;
            cmbZone.SelectedIndex = -1;
            chkIsActive.Checked = false;
            txtProperAddress.Text = string.Empty;
            dtpOpeningDate.Value = DateTime.Now;
            txtOpeningBalance.Text = string.Empty;
            mtbPhone.Text = string.Empty;
            mtbPhone2.Text = string.Empty;
            mtbTel.Text = string.Empty;
            txtRemark.Text = string.Empty;
        }
        private void EnableField()
        {
            txtCustomerID.Enabled = true;
            txtCustomerName.Enabled = true;
            txtCustomerNameUrdu.Enabled = true;
            cmbZone.Enabled = true;
            cmbAddress.Enabled = true;
            txtProperAddress.Enabled = true;
            dtpOpeningDate.Enabled = true;
            txtOpeningBalance.Enabled = true;
            mtbPhone.Enabled = true;
            mtbPhone2.Enabled = true;
            mtbTel.Enabled = true;
            txtRemark.Enabled = true;
            pbCustomer.Enabled = true;
            chkIsActive.Enabled = true;

        }
        private void DisableField()
        {
            txtCustomerID.Enabled = false;
            txtCustomerName.Enabled = false;
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

        private void btnLast_Click(object sender, EventArgs e)
        {
            lblCurrent.Text = lblLast.Text;
            FillDateInFields();
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
            if (NextData >= Convert.ToInt32(lblFirst.Text) && NextData <=Convert.ToInt32(lblLast.Text))
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

        private void createZoneToolStripMenuItem_Click_1(object sender, EventArgs e)
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

        private void alterationModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EnableField();
            txtCustomerID.ReadOnly = true;
            btnNew.Enabled = btnSave.Enabled = btnView.Enabled = btnFirst.Enabled = btnNext.Enabled = btnPrevious.Enabled = btnLast.Enabled = false;
            btnModify.Enabled = true;
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            if (IsValidated())
            {
                BlCustomer.CustomerID = int.Parse(txtCustomerID.Text);
                BlCustomer.CustomerName = txtCustomerName.Text;
                BlCustomer.CustomerNameUrdu = txtCustomerNameUrdu.Text;
                BlCustomer.ZoneId = (int)cmbZone.SelectedValue;
                BlCustomer.AddressId = (int)cmbAddress.SelectedValue;
                BlCustomer.IsActive = chkIsActive.Checked;
                BlCustomer.ProperAddress = txtProperAddress.Text;
                BlCustomer.Picture = InsertCustomerImage();
                BlCustomer.ClosingDate = dtpOpeningDate.Value;
                BlCustomer.OpeningAmount = int.Parse(txtOpeningBalance.Text);
                BlCustomer.Phone1 = mtbPhone.Text;
                BlCustomer.Phone2 = mtbPhone2.Text;
                BlCustomer.Telephone = mtbTel.Text;
                BlCustomer.Remark = txtRemark.Text;

                if (DlCustomer.Update(BlCustomer)>0)
                {

                    BlTransaction.TransactionDate = dtpOpeningDate.Value;
                    BlTransaction.TransactionType = "Ledger";
                    BlTransaction.TransactionId = 0;
                    BlTransaction.CustomerId = int.Parse(txtCustomerID.Text);
                    BlTransaction.Remark = txtRemark.Text;
                    BlTransaction.Dr = 0;
                    BlTransaction.Cr = int.Parse(txtOpeningBalance.Text);
                    //BlTransaction.PostingDate = DateTime.Now;
                    BlTransaction.TranTypeUrdu = "کھاتا بنا";
                    if (DlTransaction.Update(BlTransaction) > 0)
                    {
                        blCoaLedger.TransactionDate = dtpOpeningDate.Value;
                        blCoaLedger.TransactionType = "Create Customer Account";
                        blCoaLedger.TransactionId = int.Parse(txtCustomerID.Text);
                        blCoaLedger.ChartOfAccountId = DlCustomer.CustomerAccountId(int.Parse(txtCustomerID.Text));
                        blCoaLedger.Remark = txtRemark.Text;
                        blCoaLedger.Dr = int.Parse(txtOpeningBalance.Text);
                        blCoaLedger.Cr = 0;
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

       // public Button Coke = new Button();

        private void frmCreateCustomer_KeyDown(object sender, KeyEventArgs e)
        {
            
            if (e.KeyCode==Keys.F1)
            {
               // MessageBox.Show("apka kam ho rha hai");
                //Coke.Click += new EventHandler(btnNew_Click);
                //Coke.PerformClick();
                if (btnNew.Enabled == true)
                {
                    btnNew_Click(null, new EventArgs());
                }
                
            }
            else if (e.KeyCode==Keys.F2)
            {
                if (btnSave.Enabled==true)
                {
                    btnSave_Click(null, new EventArgs());   
                }
                else
                {
                    MessageBox.Show("Please Create the Customer");
                }
            }
            else if (e.KeyCode== Keys.F3)
            {
                if (btnModify.Enabled==true)
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
                if (btnView.Enabled==true)
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
            else if (e.KeyCode== Keys.F8)
            {
                if (btnPrevious.Enabled == true)
                {
                    btnPrevious_Click(null, new EventArgs());
                }
                
            }
            else if (e.KeyCode == Keys.F9)
            {
                if (btnLast.Enabled==true)
                {
                    btnLast_Click(null, new EventArgs());
                }
            }
            else if (e.KeyCode== Keys.F10)
            {
                if (pbCustomer.Enabled==true)
                {
                    pbCustomer_Click(null, new EventArgs());
                }
            }
            //else if (e.KeyCode == Keys.Escape)
            //{
            //    this.Close();
            //}
        }

        private void cmbZone_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.SelectNextControl((Control)sender, true, true, true, true);
                e.SuppressKeyPress = true;
            }

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
                    MessageBox.Show("Your data has been not found...","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
               
            }
        }

        private void txtCustomerName_TextChanged(object sender, EventArgs e)
        {
            txtCustomerNameUrdu.Text = txtCustomerName.Text;
        }
    }
}

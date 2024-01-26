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
using System.Data.SqlClient;
using System.Web.UI.WebControls.WebParts;
using AnajPos.GUI_Bank_Cash;
using System.Web.UI.WebControls;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace AnajPos.GUI
{
    public partial class frmProduct : frmTemplete
    {

        clsBlProduct BlProduct = new clsBlProduct();
        clsProduct DlProduct = new clsProduct();
        clsWarehouse DlWarehoues = new clsWarehouse();
        clsCategory DlCategory = new clsCategory();
        clsCompany DlCompany = new clsCompany();
        clsUnit DlUnit = new clsUnit();
        public frmProduct()
        {
            InitializeComponent();
            this.Width = 640;
            
        }

        private void frmProduct_Load(object sender, EventArgs e)
        {
            try
            {
                DataTable dtUnit = DlProduct.GetAllUnit();
                cmbUnit.DataSource = dtUnit;
                cmbUnit.ValueMember = "UnitId";
                cmbUnit.DisplayMember = "UnitName";
                cmbUnit.SelectedIndex = 0;
            }
            catch 
            {

               
            }
           


            DataTable dtFirstUnit = DlProduct.GetAllUnit();
            cmbFirstUnit.DataSource = dtFirstUnit;
            cmbFirstUnit.ValueMember = "UnitId";
            cmbFirstUnit.DisplayMember = "UnitName";
            cmbFirstUnit.SelectedIndex = -1;

            DataTable dtSecondaryUnit = DlProduct.GetAllUnit();
            cmbSecondUnit.DataSource = dtSecondaryUnit;
            cmbSecondUnit.ValueMember = "UnitId";
            cmbSecondUnit.DisplayMember = "UnitName";
            cmbSecondUnit.SelectedIndex = -1;


            DataTable dtStockLcation = DlWarehoues.View();
            cmbStockLocation.DataSource = dtStockLcation;
            cmbStockLocation.ValueMember = "WarehouseId";
            cmbStockLocation.DisplayMember = "WarehouseName";
            cmbStockLocation.SelectedIndex = -1;



            lblFirst.Text = DlProduct.MinId().ToString();
            lblLast.Text = DlProduct.MaxId().ToString();
            lblCurrent.Text = lblFirst.Text;
            GetDataInField();
            DisableField();
            txtProductName.Focus();
            cmbFirstUnit.SelectedValue = 1;
            cmbSecondUnit.SelectedValue = 1;

            
        }
        private void GetDataInField()
        {
            DataTable dt = DlProduct.GetAllData(int.Parse(lblCurrent.Text));
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                txtProductID.Text = dr[0].ToString();
                txtProductName.Text = dr[1].ToString();
                txtProductNameUrdu.Text = dr[2].ToString();
                cmbCompany.Text = dr[3].ToString();
                cmbCetegory.Text = dr[4].ToString();
                txtPackaging.Text = dr[5].ToString();
                txtPurchaseRate.Text = dr[6].ToString();
                txtSaleRate.Text = dr[7].ToString();
                txtStock.Text = dr[8].ToString();
                cmbStockLocation.SelectedValue = (int)dr[10];
               
                //if (int.Parse(txtKiloGram.Text) > 0)
                //{
                //    chkIsKg.Checked=true;
                //}
                //else
                //{
                //    chkIsKg.Checked = false;
                //}
            }
            else
            {
                MessageBox.Show("Data has no Found...");
            }
        }
        private void EnableField()
        {

            //txtProductID.Enabled = false;
            txtProductName.Enabled = true;
            txtProductNameUrdu.Enabled = true;
            cmbCompany.Enabled = true;
            cmbCetegory.Enabled = true;
            //txtKiloGram.Enabled = true;
            txtPurchaseRate.Enabled = true;
            txtSaleRate.Enabled = true;
            txtStock.Enabled = true;
            cmbStockLocation.Enabled = true;
            txtPackaging.Enabled = true;
            cmbUnit.Enabled = true;
            //rtxtRemark.Enabled = true;
            //chkIsKg.Enabled = true;
        }
        private void DisableField()
        {
            txtProductID.Enabled = false;
            txtProductName.Enabled = false;
            txtProductNameUrdu.Enabled = false;
            cmbCompany.Enabled = false;
            cmbCetegory.Enabled = false;
            //txtKiloGram.Enabled = false;
            txtPurchaseRate.Enabled = false;
            txtSaleRate.Enabled = false;
            txtStock.Enabled = false;
            cmbStockLocation.Enabled = false;
            cmbStockLocation.Enabled = false;
            txtPackaging.Enabled = false;
            cmbUnit.Enabled = false;
            //rtxtRemark.Enabled = false;
            //chkIsKg.Enabled = false;
        }
        private void ClearField()
        {
            txtProductID.Text = string.Empty;
            txtProductName.Text = string.Empty;
            txtProductNameUrdu.Text = string.Empty;
            cmbCompany.Text = string.Empty;
            cmbCetegory.Text = string.Empty;
           // txtKiloGram.Text = string.Empty;
            txtPurchaseRate.Text = string.Empty;
            txtSaleRate.Text = string.Empty;
            txtStock.Text = string.Empty;
            //txtRemark.Text = string.Empty;
            cmbStockLocation.SelectedIndex = -1;
            txtPackaging.Text = string.Empty;
            txtProductID.Text = string.Empty;
            cmbUnit.SelectedIndex = -1;
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            frmSearchProduct fsp = new frmSearchProduct();
            fsp.IsOpenProduct = true;
            fsp.ShowDialog();
            MessageBox.Show(Properties.Settings.Default.ProductID.ToString());
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            EnableField();
            ClearField();
            int NewId = int.Parse(lblLast.Text);
            NewId++;
            txtProductID.Text = NewId.ToString();
            btnNew.Enabled = false;
            btnSave.Enabled = true;
            txtProductName.Focus();
        }
        private bool IsValidated()
        {
            if (txtProductID.Text.Trim() == string.Empty)
            {
              
                return false;
            }
            else if (txtProductName.Text.Trim()== string.Empty)
            {
                MessageBox.Show("Please Enter Product Name...");
                txtProductName.Focus();
                return false;
            }
            else if (txtProductNameUrdu.Text.Trim() ==string.Empty)
            {
                MessageBox.Show("Please Enter Product Name in Urdu...");
                txtProductNameUrdu.Focus();
                return false;
            }
            else if (txtPackaging.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Please Enter Packaging...");
                txtPackaging.Focus();
                return false;
            }
            else if (cmbStockLocation.SelectedIndex == -1)
            {
                MessageBox.Show("Please Enter Location...");
                cmbStockLocation.Focus();
                return false;
            }
            else if (txtStock.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Please Enter Location...");
                cmbStockLocation.Focus();
                return false;
            }
            else if (txtPurchaseRate.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Please Enter Purchaes Rate...");
                txtPurchaseRate.Focus();
                return false;
            }
            else if (txtSaleRate.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Please Enter Sale Rate");
                txtSaleRate.Focus();
                return false;
            }
            return true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (IsProductAlreadyExists())
            {
                MessageBox.Show("This Product is already exists in database please enter new product...");
                return;
            }

            if (IsValidated())
            {
                BlProduct.ProductId = int.Parse(txtProductID.Text);
                BlProduct.ProductName = txtProductName.Text;
                BlProduct.ProductNameUrdu = txtProductNameUrdu.Text;
                BlProduct.Company = cmbCompany.Text;
                BlProduct.Cetegory = cmbCetegory.Text;
                BlProduct.Pakaging = int.Parse(txtPackaging.Text);
                BlProduct.PurchaseRate = int.Parse(txtPurchaseRate.Text);
                BlProduct.SaleRate = int.Parse(txtSaleRate.Text);
                BlProduct.OpeningStock = int.Parse(txtStock.Text);
                BlProduct.Remark = string.Empty;
                BlProduct.WarehouseId = (int)cmbStockLocation.SelectedValue;
                BlProduct.PurchaseUnitId = (int)cmbUnit.SelectedValue;
                BlProduct.SaleUnitId = 0;
                BlProduct.MeasurementProduct = int.Parse(txtHowMany.Text);
      
                if (DlProduct.Insert(BlProduct) > 0)
                {
                    MessageBox.Show("Your data has been inserted...");
                    DisableField();
                    btnSave.Enabled = false;
                    btnNew.Enabled = true;
                    lblLast.Text = DlProduct.MaxId().ToString();
                    lblCurrent.Text = lblLast.Text;
                    btnNew.Focus();
                }
            }
        }
        private bool IsProductAlreadyExists()
        {
            using (SqlConnection conn=new SqlConnection(DAL.clsConnectionString.cs))
            {
                string q = string.Format("select * from tblProduct where ProductName ='{0}' and WarehouseId = {1}", txtProductName.Text,cmbStockLocation.SelectedValue);
                SqlDataAdapter adpt = new SqlDataAdapter(q, conn);
                DataTable dt = new DataTable();
                adpt.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    return true;
                }
                else
                    return false;
            }
        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            lblCurrent.Text = lblFirst.Text;
            GetDataInField();
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            lblCurrent.Text = lblLast.Text;
            GetDataInField();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (int.Parse(lblCurrent.Text) < int.Parse(lblLast.Text))
            {
                int nextId = int.Parse(lblCurrent.Text);
                nextId++;
                lblCurrent.Text = nextId.ToString();
                GetDataInField();
            }
            else
            {
                MessageBox.Show("This is Last Id...");
            }
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            if (int.Parse(lblCurrent.Text) > int.Parse(lblFirst.Text))
            {
                int PreviousId = int.Parse(lblCurrent.Text);
                PreviousId--;
                lblCurrent.Text = PreviousId.ToString();
                GetDataInField();
            }
            else
            {
                MessageBox.Show("This is First Id ...");
            }
        }

        private void chkIsKg_CheckedChanged(object sender, EventArgs e)
        {
            //if (chkIsKg.Checked)
            //{
            //    txtKiloGram.Focus();
            //    txtKiloGram.Text = string.Empty;
            //    txtKiloGram.Enabled = true;
            //}
            //else
            //{
            //    txtKiloGram.Text = "0";
            //    txtKiloGram.Enabled = false;
            //}
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            if (IsValidated())
            {
                BlProduct.ProductId = int.Parse(txtProductID.Text);
                BlProduct.ProductName = txtProductName.Text;
                BlProduct.ProductNameUrdu = txtProductNameUrdu.Text;
                BlProduct.Company = cmbCompany.Text;
                BlProduct.Cetegory = cmbCetegory.Text;
                BlProduct.Pakaging = int.Parse(txtPackaging.Text);
                BlProduct.PurchaseRate = int.Parse(txtPurchaseRate.Text);
                BlProduct.SaleRate = int.Parse(txtSaleRate.Text);
                BlProduct.OpeningStock = int.Parse(txtStock.Text);
                BlProduct.Remark = string.Empty;
                BlProduct.WarehouseId = (int)cmbStockLocation.SelectedValue;
               
                if (DlProduct.Update(BlProduct) > 0)
                {
                    MessageBox.Show("Your data has been Updated...");
                    DisableField();
                    btnModify.Enabled = false;
                    btnNew.Enabled = btnSave.Enabled = btnView.Enabled = btnFirst.Enabled = btnNext.Enabled = btnPrevious.Enabled = btnLast.Enabled = true;

                }
            }
        }

        private void alterationModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EnableField();
            btnModify.Enabled = true;
            btnNew.Enabled = btnSave.Enabled = btnView.Enabled = btnFirst.Enabled = btnNext.Enabled = btnPrevious.Enabled = btnLast.Enabled = false;

        }

        private void txtProductName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.SelectNextControl((Control)sender, true, true, true, true);
                e.SuppressKeyPress = true;
            }
        }

        private void txtKiloGram_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as System.Web.UI.WebControls.TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void frmProduct_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
            else if (e.KeyCode == Keys.F1)
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
                if (btnLast.Enabled==true)
                {
                    btnLast_Click(null, new EventArgs());
                }
            }
        }

        private void cmbFirstUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbSecondUnit.Text == cmbFirstUnit.Text)
            {
                txtHowMany.Enabled = false;
                lblFirstUnit.Text = "...";
                lblSecondaryUnit.Text = "...";
            }
            else
            {
                txtHowMany.Enabled = true;
                if (cmbFirstUnit.SelectedItem != null)
                {
                    object selectedItem = ((DataRowView)cmbFirstUnit.SelectedItem).Row["UnitName"];
                    if (selectedItem != null)
                    {
                        lblFirstUnit.Text = selectedItem.ToString();

                        if (cmbSecondUnit.SelectedItem != null)
                        {
                            object selectedItemSecondUnit = ((DataRowView)cmbSecondUnit.SelectedItem).Row["UnitName"];
                            lblSecondaryUnit.Text = selectedItemSecondUnit.ToString();
                        }
                      
                    }

                }
            }

            
        }

        private void cmbSecondUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbFirstUnit.Text == cmbSecondUnit.Text)
            {
                txtHowMany.Enabled = false;
                lblSecondaryUnit.Text = "...";
                lblFirstUnit.Text = "...";
            }
            else
            {
                txtHowMany.Enabled = true;
                if (cmbSecondUnit.SelectedItem != null)
                {
                    object selectedItem = ((DataRowView)cmbSecondUnit.SelectedItem).Row["UnitName"];

                    if (selectedItem != null)
                    {
                        lblSecondaryUnit.Text = selectedItem.ToString();

                        if (cmbFirstUnit.SelectedItem != null)
                        {
                            object selectedItemFirstUnit = ((DataRowView)cmbFirstUnit.SelectedItem).Row["UnitName"];
                            lblFirstUnit.Text = selectedItemFirstUnit.ToString();
                        }
                        
                    }
                }
            }
            
        }

        private void addWarehouseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmWarehouse warehouse = new frmWarehouse();
            warehouse.ShowDialog();
            DataTable dtStockLcation = DlWarehoues.View();
            cmbStockLocation.DataSource = dtStockLcation;
            cmbStockLocation.ValueMember = "WarehouseId";
            cmbStockLocation.DisplayMember = "WarehouseName";
            cmbStockLocation.SelectedIndex = -1;
        }

        private void addCategoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmCategory category = new frmCategory();
            category.ShowDialog();
            DataTable dtStockLocation = DlCategory.View();
            cmbStockLocation.DataSource = dtStockLocation;
            cmbStockLocation.ValueMember = "CategoryId";
            cmbStockLocation.DisplayMember = "CategoryName";
            cmbStockLocation.SelectedIndex = -1;
        }

        private void addCompanyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmCompany Company = new frmCompany();
            Company.ShowDialog();
            DataTable dtStockLcation = DlCompany.View();
            cmbStockLocation.DataSource = dtStockLcation;
            cmbStockLocation.ValueMember = "CompanyId";
            cmbStockLocation.DisplayMember = "CompanyName";
            cmbStockLocation.SelectedIndex = -1;
        }

        private void addUnitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmUnit Unit = new frmUnit();
            Unit.ShowDialog();
            DataTable dtStockLcation = DlUnit.View();
            cmbStockLocation.DataSource = dtStockLcation;
            cmbStockLocation.ValueMember = "UnitId";
            cmbStockLocation.DisplayMember = "UnitName";
            cmbStockLocation.SelectedIndex = -1;
        }

        private void cmbUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbUnit.SelectedIndex != 0)
            {
                cmbUnit.SelectedIndex = 0;
            }
        }

        private void txtProductName_TextChanged(object sender, EventArgs e)
        {
            txtProductNameUrdu.Text = txtProductName.Text;
        }
    }
}

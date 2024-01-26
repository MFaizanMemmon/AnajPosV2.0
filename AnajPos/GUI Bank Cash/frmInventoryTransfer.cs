using AnajPos.GUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AnajPos.DAL_Cash_Bank;
using AnajPos.BAL_Cash_BANK;
using AnajPos.DAL;
using System.Data.SqlClient;
using AnajPos.BAL;

namespace AnajPos.GUI_Bank_Cash
{
    public partial class frmInventoryTransfer : frmTemplete
    {

        clsInventoryTransfer clsInventoryTransfer = new clsInventoryTransfer();
        clsBlinventoryTransfer clsBlinventoryTransfer = new clsBlinventoryTransfer();
        clsProduct DlProduct = new clsProduct();
        clsWarehouse DlWarehoues = new clsWarehouse();
        clsStock DlStock = new clsStock();
        clsBlStock BlStock = new clsBlStock();



        public frmInventoryTransfer()
        {
            InitializeComponent();
        }


        private bool IsValidate()
        {
            if (cmbToLocation.SelectedIndex == -1)
            {

                cmbToLocation.Focus();
                MessageBox.Show("Please Select To Location");
                return false;
            }
            else if (cmbFromLocation.SelectedIndex == -1)
            {

                cmbFromLocation.Focus();
                MessageBox.Show("Please Select From Location");
                return false;
            }
            else if (txtQty.Text.Trim() == string.Empty)
            {
                txtQty.Clear();
                txtQty.Focus();
                MessageBox.Show("Please Enter quantity");
                return false;

            }
            else if (txtNotes.Text.Trim() == string.Empty)
            {
                txtNotes.Clear();
                txtNotes.Focus();
                MessageBox.Show("Please Enter Notes");
                return false;
            }
            else if (int.Parse(txtQty.Text.Trim()) > int.Parse(lblTotalStock.Text.Trim()))
            {
                txtQty.Clear();
                txtQty.Focus();
                MessageBox.Show("Quantity Should be less than or equal to Stock Quantity");
                return false;
            }
            return true;
        }

        private void ClearField()
        {
            txtQty.Text = string.Empty;
            cmProduct.SelectedValueChanged -= new EventHandler(cmProduct_SelectedValueChanged);
            cmProduct.SelectedIndex = -1;
            cmProduct.SelectedValueChanged += new EventHandler(cmProduct_SelectedValueChanged);
            txtNotes.Text = string.Empty;
            cmbToLocation.SelectedValueChanged -= new EventHandler(cmbToLocation_SelectedValueChanged);
            cmbToLocation.SelectedIndex = -1;
            cmbToLocation.SelectedValueChanged += new EventHandler(cmbToLocation_SelectedValueChanged);
            lblTotalStock.Text = string.Empty;
            cmbFromLocation.SelectedValueChanged -= new EventHandler(cmbFromLocation_SelectedValueChanged);
            cmbFromLocation.SelectedIndex = -1;
            cmbFromLocation.SelectedValueChanged += new EventHandler(cmbFromLocation_SelectedValueChanged);

        }

        private void frmInventoryTransfer_Load(object sender, EventArgs e)
        {
            //dgvInventoryTransfer.DataSource = clsInventoryTransfer.ViewExpenseByDate(dptTransfareSort.Value);
            //dgvInventoryTransfer.Columns[6].Visible = false;
            //dgvInventoryTransfer.Columns[7].Visible = false;


            cmProduct.SelectedValueChanged -= new EventHandler(cmProduct_SelectedValueChanged);
            cmProduct.DataSource = DlProduct.GetAllProduct();
            cmProduct.DisplayMember = "ProductName";
            cmProduct.ValueMember = "ProductId";
            cmProduct.SelectedIndex = -1;
            cmProduct.SelectedValueChanged += new EventHandler(cmProduct_SelectedValueChanged);

            cmbToLocation.SelectedValueChanged -= new EventHandler(cmbToLocation_SelectedValueChanged);
            DataTable dtStockLcation = DlWarehoues.View();
            cmbToLocation.DataSource = dtStockLcation;
            cmbToLocation.ValueMember = "WarehouseId";
            cmbToLocation.DisplayMember = "WarehouseName";
            cmbToLocation.SelectedIndex = -1;
            cmbToLocation.SelectedValueChanged += new EventHandler(cmbToLocation_SelectedValueChanged);


            cmbFromLocation.SelectedValueChanged -= new EventHandler(cmbFromLocation_SelectedValueChanged);
            DataTable dtfromStockLcation = DlWarehoues.View();
            cmbFromLocation.DataSource = dtfromStockLcation;
            cmbFromLocation.ValueMember = "WarehouseId";
            cmbFromLocation.DisplayMember = "WarehouseName";
            cmbFromLocation.SelectedIndex = -1;
            cmbFromLocation.SelectedValueChanged += new EventHandler(cmbFromLocation_SelectedValueChanged);

            dgvInventoryTransfer.DataSource = clsInventoryTransfer.ViewTransferByDate(dptTransfareSort.Value);
            dgvInventoryTransfer.Columns[5].Visible = false;
            dgvInventoryTransfer.Columns[6].Visible = false;
            dgvInventoryTransfer.Columns[7].Visible = false;
        }

        private void cmProduct_SelectedValueChanged(object sender, EventArgs e)
        {

        }

        private void cmbToLocation_SelectedValueChanged(object sender, EventArgs e)
        {

        }

        private void cmbFromLocation_SelectedValueChanged(object sender, EventArgs e)
        {

            if (cmProduct.SelectedValue != null && cmbFromLocation.SelectedValue != null)
            {
                lblTotalStock.Text = DlStock.GetStockByProductIdAndWarehouseId((int)cmProduct.SelectedValue, (int)cmbFromLocation.SelectedValue).ToString();

            }



        }

        private void dgvInventoryTransfer_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                int GetRow = dgvInventoryTransfer.Rows.GetFirstRow(DataGridViewElementStates.Selected);
                clsBlinventoryTransfer.InventoryTransferID = (int)dgvInventoryTransfer.Rows[GetRow].Cells[0].Value;
                dptTransferDate.Value = (DateTime)dgvInventoryTransfer.Rows[GetRow].Cells[1].Value;
                cmProduct.SelectedValue = (int)dgvInventoryTransfer.Rows[GetRow].Cells[5].Value;
                cmbFromLocation.SelectedValue = (int)dgvInventoryTransfer.Rows[GetRow].Cells[6].Value;
                cmbToLocation.SelectedValue = (int)dgvInventoryTransfer.Rows[GetRow].Cells[7].Value;
                txtQty.Text = dgvInventoryTransfer.Rows[GetRow].Cells[4].Value.ToString();
                txtNotes.Text = dgvInventoryTransfer.Rows[GetRow].Cells[8].Value.ToString();
            }
            catch
            {


            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {

                using (SqlConnection conn = new SqlConnection(clsConnectionString.cs))
                {

                    if (IsValidate())
                    {
                        clsBlinventoryTransfer.TransferDate = dptTransferDate.Value;
                        clsBlinventoryTransfer.FromLocationID = (int)cmbFromLocation.SelectedValue;
                        clsBlinventoryTransfer.ToLocationID = (int)cmbToLocation.SelectedValue;
                        clsBlinventoryTransfer.ProductId = (int)cmProduct.SelectedValue;
                        clsBlinventoryTransfer.transferQty = Convert.ToDecimal(txtQty.Text);
                        clsBlinventoryTransfer.Notes = txtNotes.Text;

                        int InsertTransferEntry = clsInventoryTransfer.Insert(clsBlinventoryTransfer);

                        //BlStock.ProductTransId= (int)
                        //BlStock.ProductTransDate = dptTransferDate.Value;
                        //BlStock.TransactionType = "Inventory Transfer";
                        //BlStock.ProductId = (int)cmProduct.SelectedValue;
                        //BlStock.UnitID = 1;
                        //BlStock.WarehouseId = (int)cmbFromLocation.SelectedValue;
                        //BlStock.StockIn = 0;
                        //BlStock.StockOut = int.Parse(txtQty.Text);
                        //BlStock.Amount = 0;
                        //BlStock.IsDeleted = false;


                        //int InsertIntoStock1 = DlStock.Insert(BlStock);

                        //BlStock.ProductTransDate = dptTransferDate.Value;
                        //BlStock.TransactionType = "Inventory Transfer";
                        //BlStock.ProductId = (int)cmProduct.SelectedValue;
                        //BlStock.UnitID = 2;
                        //BlStock.WarehouseId = (int)cmbToLocation.SelectedValue;
                        //BlStock.StockIn = int.Parse(txtQty.Text);
                        //BlStock.StockOut = 0;
                        //BlStock.Amount = 0;
                        //BlStock.IsDeleted = false;

                        //int InsertIntoStock = DlStock.Insert(BlStock);

                        //if (InsertTransferEntry >0   && InsertIntoStock > 0 && InsertIntoStock1>0)
                        //{
                        MessageBox.Show("Transfer has been created...");
                        dgvInventoryTransfer.DataSource = clsInventoryTransfer.ViewTransferByDate(dptTransfareSort.Value);

                        dgvInventoryTransfer.Columns[5].Visible = false;
                        dgvInventoryTransfer.Columns[6].Visible = false;
                        dgvInventoryTransfer.Columns[7].Visible = false;

                        ClearField();


                        //}
                    }
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show("Error..." + ex.Message);
            }
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            if (IsValidate())
            {
                clsBlinventoryTransfer.TransferDate = dptTransferDate.Value;
                clsBlinventoryTransfer.FromLocationID = (int)cmbFromLocation.SelectedValue;
                clsBlinventoryTransfer.ToLocationID = (int)cmbFromLocation.SelectedValue;
                clsBlinventoryTransfer.ProductId = (int)cmProduct.SelectedValue;
                clsBlinventoryTransfer.transferQty = Convert.ToDecimal(txtQty.Text);
                clsBlinventoryTransfer.Notes = txtNotes.Text;
                if (clsInventoryTransfer.update(clsBlinventoryTransfer) > 0)
                {
                    MessageBox.Show("Transfer has been created...");
                    dgvInventoryTransfer.DataSource = clsInventoryTransfer.ViewTransferByDate(dptTransfareSort.Value);
                    dgvInventoryTransfer.Columns[5].Visible = false;
                    dgvInventoryTransfer.Columns[6].Visible = false;
                    dgvInventoryTransfer.Columns[7].Visible = false;

                }

            }
        }

        private void dptTransfareSort_ValueChanged(object sender, EventArgs e)
        {
            dgvInventoryTransfer.DataSource = clsInventoryTransfer.ViewTransferByDate(dptTransfareSort.Value);
            dgvInventoryTransfer.Columns[5].Visible = false;
            dgvInventoryTransfer.Columns[6].Visible = false;
            dgvInventoryTransfer.Columns[7].Visible = false;

        }
    }
}

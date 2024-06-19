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
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnajPos.GUI_Bank_Cash
{
    public partial class frmStockAdjustment : frmTemplete
    {
        clsBlChartOfAccounts blCoa = new clsBlChartOfAccounts();
        clsChartOfAccount dlCoa = new clsChartOfAccount();
        clsProduct DlProduct = new clsProduct();
        clsWarehouse DlWarehoues = new clsWarehouse();
        clsStock DlStock = new clsStock();
        clsBlStock BlStock = new clsBlStock();

        public frmStockAdjustment()
        {
            InitializeComponent();
        }
        private bool IsValidate()
        {
            if (txtStockIn.Text.Trim() == string.Empty || txtStockOut.Text.Trim() == string.Empty || txtRemarks.Text.Trim() == string.Empty || txtRemarks.Text.Trim() == string.Empty || cmbProduct.SelectedIndex == -1 || cmbLocation.SelectedIndex == -1 )
            {
                return false;
            }
            else
            {
                if (int.TryParse(txtStockIn.Text.Trim(), out _) && int.TryParse(txtStockOut.Text.Trim(), out _))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private void frmStock_Load(object sender, EventArgs e)
        {
            //_________ Load Data ______________
            DataTable dt2 = new DataTable();
            dt2 = DlProduct.ViewProduct();
            cmbProduct.DataSource = dt2;
            cmbProduct.ValueMember = "ProductId";
            cmbProduct.DisplayMember = "ProductName";
            cmbProduct.SelectedIndex = -1;

            DataTable dtStockLcation = DlWarehoues.View();
            cmbLocation.DataSource = dtStockLcation;
            cmbLocation.ValueMember = "WarehouseId";
            cmbLocation.DisplayMember = "WarehouseName";
            cmbLocation.SelectedIndex = -1;

            loadDataGrid();
            stock_GV.Columns["StockAdjID"].Visible = false;

            // Set AutoSizeMode for columns
            foreach (DataGridViewColumn column in stock_GV.Columns)
            {
                // Set your desired AutoSizeMode (e.g., AllCells, Fill, etc.)
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (IsValidate())
            {
                BlStock.ProductId = (int)cmbProduct.SelectedValue;
                BlStock.WarehouseId = (int)cmbLocation.SelectedValue;
                BlStock.StockIn = decimal.Parse(txtStockIn.Text);
                BlStock.StockOut = decimal.Parse(txtStockOut.Text);
                BlStock.ProductTransDate = DateTime.Now;
                BlStock.Remarks = txtRemarks.Text;
                BlStock.TransactionType = "Stock";
                if (DlStock.Insert(BlStock) > 0)
                {
                    MessageBox.Show("Insert Successfully");
                    cmbProduct.SelectedIndex = -1;
                    cmbLocation.SelectedValue = -1;
                    txtStockIn.Text = string.Empty;
                    txtRemarks.Text = string.Empty;
                   
                    txtRemarks.Text = string.Empty;
                    loadDataGrid();
                }
            }
            else
            {
                MessageBox.Show("Please Enter Proper Data...");
            }

            
        }
        private void loadDataGrid()
        {
            DataTable dt = DlStock.GetStockView();
            stock_GV.DataSource = dt;
            stock_GV.Columns[4].Width = 0;
        }

        private void stock_GV_DoubleClick(object sender, EventArgs e)
        {
          
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
        }

        private void txtStockOut_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

        }

        private void cmbLocation_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbProduct.SelectedValue != null && cmbLocation.SelectedValue != null)
                {
                    lblTotalStock.Text = DlStock.GetStockByProductIdAndWarehouseId((int)cmbProduct.SelectedValue, (int)cmbLocation.SelectedValue).ToString();
                }
            }
            catch
            {


            }
        }
    }
}

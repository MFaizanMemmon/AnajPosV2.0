using AnajPos.CashBankReporting;
using AnajPos.DAL;
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
    public partial class frmProductStock : frmTemplete
    {
        clsWarehouse DlWarehoues = new clsWarehouse();
        clsStock dlStock = new clsStock();
        public frmProductStock()
        {
            InitializeComponent();
        }

        private void frmProductStock_Load(object sender, EventArgs e)
        {
            cmbLocation.SelectedValueChanged -= new EventHandler(cmbLocation_SelectedValueChanged);
            DataTable dtStockLcation = DlWarehoues.View();
            DataRow selectAllRow = dtStockLcation.NewRow();
            selectAllRow["WarehouseId"] = 0;
            selectAllRow["WarehouseName"] = "Select All";
            dtStockLcation.Rows.InsertAt(selectAllRow, 0);
            cmbLocation.DataSource = dtStockLcation;
            cmbLocation.ValueMember = "WarehouseId";
            cmbLocation.DisplayMember = "WarehouseName";
            cmbLocation.SelectedIndex = -1;
            cmbLocation.SelectedValueChanged += new EventHandler(cmbLocation_SelectedValueChanged);
        }

        private void cmbLocation_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                if ((int)cmbLocation.SelectedValue == 0)
                {
                    dgViewStock.DataSource = dlStock.GetAllStock((int)cmbLocation.SelectedValue);
                }
                else
                {
                    dgViewStock.DataSource = dlStock.GetAllStockByWarehouseId((int)cmbLocation.SelectedValue);
                }
               
            }
            catch 
            {

               
            }
        }

        private void dgViewStock_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {

                // Assuming you want to get values from "dr" and "cr" columns
                string GetProductId = dgViewStock.Rows[e.RowIndex].Cells["ProductId"].Value.ToString();


                // Do something with the values (e.g., display in a MessageBox)
                frmViewStockLedger sl = new frmViewStockLedger();
                sl.ProductId = int.Parse(GetProductId);
                sl.WarehouseId = (int)cmbLocation.SelectedValue;
                sl.ShowDialog();
            }
        }

        private void cmbLocation_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnPrintInvoice_Click(object sender, EventArgs e)
        {
            if (cmbLocation.SelectedIndex != -1)
            {
                if (cmbLocation.SelectedIndex == 0)
                {

                }
                else
                {

                    frmViewStock vs = new frmViewStock();
                    vs.WarehouseId = (int)cmbLocation.SelectedIndex;
                    vs.ShowDialog();
                }
            }
        }
    }
}

using AnajPos.BAL;
using AnajPos.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls.WebParts;
using System.Windows.Forms;

namespace AnajPos.GUI
{
    public partial class frmWarehouse : frmTemplete
    {
        clsBlWarehouse BlWarehouse = new clsBlWarehouse();
        clsWarehouse DlWarehouse = new clsWarehouse();
        public frmWarehouse()
        {
            InitializeComponent();
            dgvWarehouse.DataSource = DlWarehouse.View();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (IsValidate())
            {
                BlWarehouse.WarehouseName = txtWarehouseName.Text;
                if (BlWarehouse.WarehouseId > 0)
                {
                    if (DlWarehouse.Update(BlWarehouse) > 0)
                    {
                        MessageBox.Show("Warehouse has been Updated...");
                        dgvWarehouse.DataSource = DlWarehouse.View();
                        BlWarehouse.WarehouseId = 0;
                        btnSave.Text = "Save";
                        txtWarehouseName.Text = string.Empty;
                    }
                }
                else
                {
                    if (DlWarehouse.Insert(BlWarehouse) > 0)
                    {
                        MessageBox.Show("Warehouse has been created...");
                        dgvWarehouse.DataSource = DlWarehouse.View();
                        txtWarehouseName.Text = string.Empty;
                    }
                }
                
                
                
            }
        }

        private bool IsValidate()
        {
            if (txtWarehouseName.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Please Enter Warehouse Name ...");
                txtWarehouseName.Focus();
                return false;
            }
            return true;
        }

        private void dgvWarehouse_DoubleClick(object sender, EventArgs e)
        {
            int GetRow = dgvWarehouse.Rows.GetFirstRow(DataGridViewElementStates.Selected);
            BlWarehouse.WarehouseId = (int)dgvWarehouse.Rows[GetRow].Cells[0].Value;
            BlWarehouse.WarehouseName= dgvWarehouse.Rows[GetRow].Cells[1].Value.ToString();
            btnSave.Text = "Update";
            txtWarehouseName.Text = BlWarehouse.WarehouseName;
        }

        
    }
}

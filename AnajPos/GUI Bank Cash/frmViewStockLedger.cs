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
using System.Windows.Forms;

namespace AnajPos.GUI_Bank_Cash
{
    public partial class frmViewStockLedger : frmTemplete
    {
        public int ProductId { get; set; }
        public int WarehouseId { get; set; }

        clsStock dlStock = new clsStock();
        public frmViewStockLedger()
        {
            InitializeComponent();
        }

        private void frmViewStockLedger_Load(object sender, EventArgs e)
        {
            dgViewStock.DataSource = dlStock.GetStockLedger(ProductId,WarehouseId);
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnajPos.CashBankReporting
{
    public partial class frmViewStock : Form
    {
        public int WarehouseId { get; set; }

        public frmViewStock()
        {
            InitializeComponent();
        }

        private void frmViewStock_Load(object sender, EventArgs e)
        {
            crptStockReport sr = new crptStockReport();
            sr.SetParameterValue("@WarehouseId",WarehouseId);
            crystalReportViewer1.ReportSource = null;
            crystalReportViewer1.ReportSource = sr;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AnajPos.VendorReporting;
using AnajPos.GUI;

namespace AnajPos.VendorReporting
{
    public partial class frmVendorOutstandingReport : frmTemplete
    {
        public frmVendorOutstandingReport()
        {
            InitializeComponent();
        }

        private void frmVendorOutstandingReport_Load(object sender, EventArgs e)
        {
            crptVendorOutstanding vo = new crptVendorOutstanding();
            crystalReportViewer1.ReportSource = null;
            crystalReportViewer1.Refresh();
            crystalReportViewer1.ReportSource = vo;
            
        }
    }
}

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

namespace AnajPos.Reports
{
    public partial class frmCustomerOutstandingReport : frmTemplete
    {
        public frmCustomerOutstandingReport()
        {
            InitializeComponent();
        }

        private void frmCustomerOutstandingReport_Load(object sender, EventArgs e)
        {
            Reports.crptCustomerOutstanding co = new Reports.crptCustomerOutstanding();
            crystalReportViewer1.ReportSource = null;
            
            crystalReportViewer1.ReportSource = co;
            crystalReportViewer1.Refresh();
        }
    }
}

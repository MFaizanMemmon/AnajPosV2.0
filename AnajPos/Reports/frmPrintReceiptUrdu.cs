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
    public partial class frmPrintReceiptUrdu : frmTemplete
    {
        public int RecId { get; set; }
        public frmPrintReceiptUrdu()
        {
            InitializeComponent();
        }

        private void frmPrintReceiptUrdu_Load(object sender, EventArgs e)
        {
            Reports.crptReceiptInUrdu cr = new Reports.crptReceiptInUrdu();
            cr.SetParameterValue("@id", RecId);
            crystalReportViewer1.ReportSource = null;
            crystalReportViewer1.ReportSource = cr;
        }
    }
}

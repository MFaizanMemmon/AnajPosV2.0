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
using AnajPos.Reports;

namespace AnajPos.Reports
{
    public partial class frmViewLedgerUrduDefault : frmTemplete
    {
        public int CustomerId { get; set; }
        public frmViewLedgerUrduDefault()
        {
            InitializeComponent();
        }

        private void frmViewLedgerUrduDefault_Load(object sender, EventArgs e)
        {
            crptLedgerUrdu cr = new crptLedgerUrdu();
            cr.SetParameterValue("@cid",CustomerId);
            crystalReportViewer1.ReportSource = cr;

        }
    }
}

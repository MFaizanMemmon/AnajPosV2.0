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
    public partial class frmLedger : frmTemplete
    {
        public int CustomerId { get; set; }
        public frmLedger()
        {
            InitializeComponent();
        }

        private void frmLedger_Load(object sender, EventArgs e)
        {
            Reports.crptViewLedgerDefaultEnglish cr = new Reports.crptViewLedgerDefaultEnglish();
            cr.SetParameterValue("@cid", CustomerId);
            crystalReportViewer1.ReportSource = cr;
        }
    }
}

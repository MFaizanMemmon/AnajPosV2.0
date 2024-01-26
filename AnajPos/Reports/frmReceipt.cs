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
    public partial class frmReceipt : frmTemplete
    {
        public int RecId { get; set; }
        public frmReceipt()
        {
            InitializeComponent();
        }

        private void frmReceipt_Load(object sender, EventArgs e)
        {
            Reports.CrptReceiptInEnglish rec = new Reports.CrptReceiptInEnglish();
            rec.SetParameterValue("@id",RecId);
            crystalReportViewer1.ReportSource = null;
            crystalReportViewer1.ReportSource = rec;
        }
    }
}

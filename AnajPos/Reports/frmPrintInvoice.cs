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
    public partial class frmPrintInvoice : frmTemplete
    {
        public int Id { get; set; }
        public frmPrintInvoice()
        {
            InitializeComponent();
        }

        private void frmPrintInvoice_Load(object sender, EventArgs e)
        {
            Reports.crptInvoice inv = new Reports.crptInvoice();
            inv.SetParameterValue("@id", Id);
            inv.SetParameterValue("@id", Id);
            crystalReportViewer1.ReportSource = null;
            crystalReportViewer1.ReportSource = inv;
        }
    }
}

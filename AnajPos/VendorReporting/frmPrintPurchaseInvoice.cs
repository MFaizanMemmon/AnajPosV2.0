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
using AnajPos.VendorReporting;

namespace AnajPos.VendorReporting
{
    public partial class frmPrintPurchaseInvoice : frmTemplete
    {
        public int Id { get; set; }
        public frmPrintPurchaseInvoice()
        {
            InitializeComponent();
        }

        private void frmPrintPurchaseInvoice_Load(object sender, EventArgs e)
        {
            crpPurchaeInvoice pi = new crpPurchaeInvoice();
            pi.SetParameterValue("@id", Id);
           
            crystalReportViewer1.ReportSource = null;
            crystalReportViewer1.ReportSource = pi;
        }
    }
}

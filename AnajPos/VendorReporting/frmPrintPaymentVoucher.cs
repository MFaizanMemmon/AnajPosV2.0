using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnajPos.VendorReporting
{
    public partial class frmPrintPaymentVoucher : Form
    {

        public int PaymentVoucherId { get; set; }
        public frmPrintPaymentVoucher()
        {
            InitializeComponent();
        }

        private void frmPrintPaymentVoucher_Load(object sender, EventArgs e)
        {
            crptPaymentVoucher pv = new crptPaymentVoucher();
            pv.SetParameterValue("@PaymentId", PaymentVoucherId);
            crystalReportViewer1.ReportSource = null;
            
            crystalReportViewer1.ReportSource = pv;
            
        }
    }
}

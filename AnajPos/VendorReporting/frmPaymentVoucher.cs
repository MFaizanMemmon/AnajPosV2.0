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

namespace AnajPos.VendorReporting
{
    public partial class frmPaymentVoucher : frmTemplete
    {
        public int Id { get; set; }
        public frmPaymentVoucher()
        {
            InitializeComponent();
        }

        private void frmPaymentVoucher_Load(object sender, EventArgs e)
        {
            crptPaymentVoucher pv = new crptPaymentVoucher();
            pv.SetParameterValue("@id", Id);
            //crystalReportViewer1.ReportSource = null;
            crystalReportViewer1.ReportSource = pv;
            crystalReportViewer1.Refresh();

        }
    }
}

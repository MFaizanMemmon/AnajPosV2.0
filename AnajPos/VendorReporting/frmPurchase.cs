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
    public partial class frmPurchase : frmTemplete
    {
        public int Id { get; set; }
        public frmPurchase()
        {
            InitializeComponent();
        }

        private void frmPurchase_Load(object sender, EventArgs e)
        {
            //crptPurchase purch = new crptPurchase();
            //purch.SetParameterValue("@id", Id);
            //purch.SetParameterValue("@id", Id);
            //crystalReportViewer1.ReportSource = null;
            //crystalReportViewer1.ReportSource = purch;
        }

    }
}

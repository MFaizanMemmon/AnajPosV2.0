using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnajPos.CashBankReporting
{
    public partial class frmViewCounterReceipt : Form
    {
        public int Id { get; set; }
        public frmViewCounterReceipt()
        {
            InitializeComponent();
        }

        private void frmViewCounterReceipt_Load(object sender, EventArgs e)
        {
            CashBankReporting.crptCounterReceipt inv = new CashBankReporting.crptCounterReceipt();
            inv.SetParameterValue("@id", Id);
            
            crystalReportViewer1.ReportSource = null;
            crystalReportViewer1.ReportSource = inv;
        }
    }
}

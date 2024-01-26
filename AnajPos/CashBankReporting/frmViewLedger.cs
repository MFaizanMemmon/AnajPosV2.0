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
    public partial class frmViewLedger : Form
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public frmViewLedger()
        {
            InitializeComponent();
        }

        private void frmViewLedger_Load(object sender, EventArgs e)
        {
            CashBankReporting.AllLedger inv = new CashBankReporting.AllLedger();
            inv.SetParameterValue("@StartDate", StartDate);
            inv.SetParameterValue("@EndDate", EndDate);
            inv.SetParameterValue("@AccountID", AccountId);
            inv.SetParameterValue("AccountName", AccountName);
            //inv.DataDefinition.FormulaFields["MyVariableFormula"].Text = "\"" + AccountName + "\"";


            crystalReportViewer1.ReportSource = null;
            crystalReportViewer1.ReportSource = inv;
        }
    }
}

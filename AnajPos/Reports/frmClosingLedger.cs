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
    public partial class frmClosingLedger : frmTemplete
    {
        public int CustomerId { get; set; }
        public DateTime ClosingDate { get; set; }
        public DateTime LedgerStartDate { get; set; }
        public frmClosingLedger()
        {
            InitializeComponent();
        }

        private void frmClosingLedger_Load(object sender, EventArgs e)
        {
            CrptClosingLedgerInEnglish CLE = new CrptClosingLedgerInEnglish();
            CLE.SetParameterValue("@CustomerId",CustomerId);
            CLE.SetParameterValue("@ClosingDate",ClosingDate);
            crystalReportViewer1.ReportSource = CLE;
            label1.Text = LedgerStartDate.ToShortDateString();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
        
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dateTimePicker1.Value <= LedgerStartDate)
            {
                MessageBox.Show("Your Selected date is wrong");
            }
            else
            {
                CrptClosingLedgerInEnglish CLE = new CrptClosingLedgerInEnglish();
                CLE.SetParameterValue("@CustomerId", CustomerId);
                CLE.SetParameterValue("@ClosingDate", dateTimePicker1.Value);
                crystalReportViewer1.ReportSource = CLE;

            }
        }

       
    }
}

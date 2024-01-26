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
    public partial class frmVendorLedger :frmTemplete 
    {
        public int Id { get; set; }
        public DateTime CDate { get; set; }
        public frmVendorLedger()
        {
            InitializeComponent();
        }

        private void frmVendorLedger_Load(object sender, EventArgs e)
        {
            crptVendorLedger vl = new crptVendorLedger();
            vl.SetParameterValue("@id",Id);
            vl.SetParameterValue("@CustomerId",Id);
            vl.SetParameterValue("@ClosingDate",CDate);
            crystalReportViewer1.ReportSource = null;
            crystalReportViewer1.ReportSource = vl;
            crystalReportViewer1.Refresh();

            lblClosingDate.Text = CDate.ToShortDateString();
            dateTimePicker1.Value = CDate;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dateTimePicker1.Value >= Convert.ToDateTime(lblClosingDate.Text))
            {
                crptVendorLedger vl = new crptVendorLedger();
                vl.SetParameterValue("@id", Id);
                vl.SetParameterValue("@CustomerId", Id);
                vl.SetParameterValue("@ClosingDate", dateTimePicker1.Value);
                crystalReportViewer1.ReportSource = null;
                crystalReportViewer1.ReportSource = vl;
                crystalReportViewer1.Refresh();
            }
            else
            {
                MessageBox.Show("You Should Get the Closing Date " + lblClosingDate.Text);
            }
        }
    }
}

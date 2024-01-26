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
    public partial class frmGatePas : frmTemplete
    {
        public int Id { get; set; }
        public frmGatePas()
        {
            InitializeComponent();
        }

        private void frmGatePas_Load(object sender, EventArgs e)
        {
            Reports.crptGatePas gp = new Reports.crptGatePas();
            gp.SetParameterValue("@id", Id);
            gp.SetParameterValue("@id", Id);
            crystalReportViewer1.ReportSource = null;
            crystalReportViewer1.ReportSource = gp;

        }
    }
}

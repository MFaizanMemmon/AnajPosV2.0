using AnajPos.DAL_Cash_Bank;
using AnajPos.GUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnajPos.GUI_Bank_Cash
{
    public partial class frmViewBank : frmTemplete
    {
        clsBank dlBank = new clsBank();
        DataTable dt = new DataTable();
        public frmViewBank()
        {
            InitializeComponent();
        }

        private void txtClient_TextChanged(object sender, EventArgs e)
        {
            DataView dvCleint = dt.DefaultView;
            dvCleint.RowFilter = "BankName like '%" + txtClient.Text + "%'";
        }

        private void frmViewBank_Load(object sender, EventArgs e)
        {
            dt = dlBank.View();
            dgViewCustomer.DataSource = dt;
        }
    }
}

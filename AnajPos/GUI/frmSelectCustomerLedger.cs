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
using AnajPos.DAL;
using AnajPos.BAL;
using AnajPos.Reports;

namespace AnajPos.GUI
{
    public partial class frmSelectCustomerLedger : frmTemplete
    {
        clsZone cz = new clsZone();
        clsCustomer DlCustomer = new clsCustomer();
        clsBlCustomer BlCustomer = new clsBlCustomer();
        clsAddress DlAddress = new clsAddress();
        clsBlAddress BlAddress = new clsBlAddress();
        clsTransaction DlTransaction = new clsTransaction();
        clsBlTransaction BlTransaction = new clsBlTransaction();
        clsAdjustment DlAdjustment = new clsAdjustment();
        clsBlAdjustment BlAdjustment = new clsBlAdjustment();
        public frmSelectCustomerLedger()
        {
            InitializeComponent();
        }

        private void frmSelectCustomerLedger_Load(object sender, EventArgs e)
        {
            cmbZone.SelectedValueChanged -= new EventHandler(cmbZone_SelectedValueChanged);
            DataTable dtView = cz.ViewZone();
            cmbZone.DataSource = dtView;
            cmbZone.ValueMember = "Id";
            cmbZone.DisplayMember = "Name of Zone";
            cmbZone.SelectedIndex = -1;
            cmbCustomer.Text = string.Empty;
            cmbZone.SelectedValueChanged += new EventHandler(cmbZone_SelectedValueChanged);
        }

        private void cmbZone_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cmbZone.SelectedIndex != -1)
            {
                BlAddress.AddressId = (int)cmbZone.SelectedValue;
                cmbAddress.DataSource = DlAddress.ViewAddressByZone(BlAddress);
                cmbAddress.DisplayMember = "AddressName";
                cmbAddress.ValueMember = "AddressID";
                //    cmbAddress.SelectedIndex = -1;
               
            }
        }

        private void cmbAddress_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                BlCustomer.AddressId = Convert.ToInt32(cmbAddress.SelectedValue);
                DataTable dt = DlCustomer.ViewByAddress(BlCustomer);
                cmbCustomer.DataSource = dt;
                cmbCustomer.DisplayMember = "CustomerName";
                cmbCustomer.ValueMember = "CustomerId";
                //  cmbCustomer.SelectedIndex = -1;
                if (dt.Rows.Count <= 0)
                {
                        cmbCustomer.Text = string.Empty;   
                }
            }
            catch
            { }
        }

        private void cmbCustomer_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                dtpStarting.Value = DlCustomer.GetClosingDate((int)cmbCustomer.SelectedValue);    
            }
            catch 
            {
                
            }
            try
            {
                clsClosingCustomer DlClosing = new clsClosingCustomer();
                label4.Text = DlClosing.GetClosing((int)cmbCustomer.SelectedValue).ToShortDateString();
            }
            catch 
            {
                label4.Text = dtpStarting.Text;
               
            }
           
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            if (cmbCustomer.SelectedIndex == -1)
            {
                MessageBox.Show("Please Select the Customer");
                return;
            }
            if (checkBox1.Checked)
            {
                frmClosingLedger Cl = new frmClosingLedger();
                Cl.CustomerId =(int) cmbCustomer.SelectedValue;
                Cl.ClosingDate = Convert.ToDateTime(label4.Text);
                Cl.LedgerStartDate = dtpStarting.Value;
                Cl.ShowDialog();
            }
            else
            {
                frmLedger fl = new frmLedger();
                fl.CustomerId = (int)cmbCustomer.SelectedValue;
                fl.ShowDialog();

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (cmbCustomer.SelectedIndex != -1)
            {
                frmViewLedgerUrduDefault fl = new frmViewLedgerUrduDefault();
                fl.CustomerId = (int)cmbCustomer.SelectedValue;
                fl.ShowDialog();   
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (cmbCustomer.SelectedIndex > -1)
            {
                frmGridLedger gl = new frmGridLedger();
                gl.CustomerId = (int)cmbCustomer.SelectedValue;
                gl.CustomerName = cmbCustomer.Text + " " + cmbAddress.Text;
                gl.ShowDialog();
            }
           
        }

        private void cmbZone_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.SelectNextControl((Control)sender, true, true, true, true);
                e.SuppressKeyPress = true;
            }
        }
    }
}

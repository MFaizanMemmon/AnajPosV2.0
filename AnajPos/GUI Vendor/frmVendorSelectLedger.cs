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
using AnajPos.DalVendor;
using AnajPos.VendorReporting;

namespace AnajPos.GUI_Vendor
{
    public partial class frmVendorSelectLedger : frmTemplete
    {
        public frmVendorSelectLedger()
        {
            InitializeComponent();
        }
        clsZone cz = new clsZone();
        clsAddress BLAddress = new clsAddress();
        clsAddress DlAddress = new clsAddress();
        clsHeadOfAcc DlHead = new clsHeadOfAcc();
        clsBlAddress BlAddress = new clsBlAddress();
        clsBlVendor BlVendor = new clsBlVendor();
        clsDlVendor DlVendor = new clsDlVendor();
        
        private void frmVendorSelectLedger_Load(object sender, EventArgs e)
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
            }
        }

        private void cmbAddress_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                BlVendor.AddressName = Convert.ToInt32(cmbAddress.SelectedValue);
                DataTable dt = DlVendor.ViewByAddress(BlVendor);
                cmbCustomer.DataSource = dt;
                cmbCustomer.DisplayMember = "VendorName";
                cmbCustomer.ValueMember = "VendorID";
                //  cmbCustomer.SelectedIndex = -1;
            }
            catch
            { }
        }

        private void cmbCustomer_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                label4.Text = DlVendor.GetClosingDate((int)cmbCustomer.SelectedValue).ToShortDateString();   
            }
            catch
            { 

            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            frmVendorLedger vl = new frmVendorLedger();
            vl.Id = (int)cmbCustomer.SelectedValue;
            vl.CDate = Convert.ToDateTime(label4.Text);
            vl.ShowDialog();
        }

       
    }
}

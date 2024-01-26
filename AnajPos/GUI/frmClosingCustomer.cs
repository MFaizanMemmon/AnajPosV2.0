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

namespace AnajPos.GUI
{
    public partial class frmClosingCustomer : frmTemplete
    {
        clsZone cz = new clsZone();
        clsBlAddress BlAddress = new clsBlAddress();
        clsAddress DlAddress = new clsAddress();
        clsBlCustomer BlCustomer = new clsBlCustomer();
        clsCustomer DlCustomer = new clsCustomer();
        clsClosingCustomer DlClosing = new clsClosingCustomer();
        clsBlClosingCustomer BlClosing = new clsBlClosingCustomer();
        public frmClosingCustomer()
        {
            InitializeComponent();
        }

        private void frmClosingCustomer_Load(object sender, EventArgs e)
        {
            cmbZone.SelectedValueChanged -= new EventHandler(cmbZone_SelectedValueChanged);
            DataTable dtView = cz.ViewZone();
            cmbZone.DataSource = dtView;
            cmbZone.ValueMember = "Id";
            cmbZone.DisplayMember = "Name of Zone";
            cmbZone.SelectedIndex = -1;
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
                BlCustomer.AddressId = Convert.ToInt32(cmbAddress.SelectedValue);
                DataTable dt = DlCustomer.ViewByAddress(BlCustomer);
                cmbCustomer.DataSource = dt;
                cmbCustomer.DisplayMember = "CustomerName";
                cmbCustomer.ValueMember = "CustomerId";
                //  cmbCustomer.SelectedIndex = -1;
            }
            catch
            { }
        }

        private void cmbCustomer_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                lblBalance.Text = DlCustomer.CustomerLastBalance((int)cmbCustomer.SelectedValue).ToString();
                lblCustomerName.Text = cmbCustomer.Text;
                lblDate.Text = DateTime.Now.ToShortDateString();
            }
            catch
            {


            }
        }

        private void btnClosing_Click(object sender, EventArgs e)
        {
            DialogResult dg = MessageBox.Show("Are you sure yow wana Closing With Customer", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dg == DialogResult.Yes)
            {
                if (IsValidated())
                {
                    BlClosing.ClosingDate = Convert.ToDateTime(lblDate.Text);
                    BlClosing.CustomerId = (int)cmbCustomer.SelectedValue;
                    BlClosing.Remark = txtRemark.Text;
                    BlClosing.Amount = int.Parse(lblBalance.Text);
                    if (DlClosing.Insert(BlClosing) > 0)
                    {
                        MessageBox.Show("Yor Closing has been done...");
                    }
                }
            }
        }
        private  bool IsValidated()
        {
            if (cmbCustomer.SelectedIndex==-1)
            {
                return false;
            }
            else if (txtRemark.Text.Trim() == string.Empty)
            {
                return false;
            }
            return true;
        }
    }
}

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
    public partial class frmAddress : frmTemplete
    {
        clsZone cz = new clsZone();
        clsBlAddress blAddress = new clsBlAddress();
        clsAddress dlAddress = new clsAddress();
        public frmAddress()
        {
            InitializeComponent();
        }

        private void frmAddress_Load(object sender, EventArgs e)
        {
            DataTable dtView = cz.ViewZone();
            cmbZone.DataSource = dtView;
            cmbZone.ValueMember = "Id";
            cmbZone.DisplayMember = "Name of Zone";
            cmbZone.SelectedIndex = -1;

            DataTable dt = dlAddress.View();
            dgViewAddress.DataSource = dt;
            dgViewAddress.Columns[4].Width = 0;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (cmbZone.SelectedIndex != -1 && txtAddress.Text.Trim()!=string.Empty && txtAddressUrdu.Text.Trim() != string.Empty && btnSave.Text=="Save")
            {
                blAddress.AddressName = txtAddress.Text;
                blAddress.AddressNameUrdu = txtAddressUrdu.Text;
                blAddress.ZoneId = (int)cmbZone.SelectedValue;
                
                if (dlAddress.Insert(blAddress) > 0)
                {
                    MessageBox.Show("Your Address has Been Created...");
                    cmbZone.SelectedIndex = -1;
                    txtAddress.Text = string.Empty;
                    txtAddressUrdu.Text = string.Empty;
                    blAddress.AddressId = 0;
                    DataTable dt = dlAddress.View();
                    dgViewAddress.DataSource = dt;
                    dgViewAddress.Columns[4].Width = 0;
                    cmbZone.Focus();
                }
            }
            else if (cmbZone.SelectedIndex != -1 && txtAddress.Text.Trim()!=string.Empty && txtAddressUrdu.Text.Trim() != string.Empty && btnSave.Text=="Update")
            {
                blAddress.AddressName = txtAddress.Text;
                blAddress.AddressNameUrdu = txtAddressUrdu.Text;
                blAddress.ZoneId = (int)cmbZone.SelectedValue;
                if (dlAddress.Update(blAddress)>0)
                {
                    MessageBox.Show("Your Address has been Updated");
                    cmbZone.SelectedIndex = -1;
                    txtAddress.Text = string.Empty;
                    txtAddressUrdu.Text = string.Empty;
                    blAddress.AddressId = 0;
                    DataTable dt = dlAddress.View();
                    dgViewAddress.DataSource = dt;
                    dgViewAddress.Columns[4].Width = 0;
                    btnSave.Text = "Save";
                    cmbZone.Focus();
                }
            }
        }

        private void dgViewAddress_DoubleClick(object sender, EventArgs e)
        {
            int GetRow = dgViewAddress.Rows.GetFirstRow(DataGridViewElementStates.Selected);
            blAddress.AddressId = (int) dgViewAddress.Rows[GetRow].Cells[0].Value;
            txtAddress.Text = dgViewAddress.Rows[GetRow].Cells[1].Value.ToString();
            txtAddressUrdu.Text = dgViewAddress.Rows[GetRow].Cells[2].Value.ToString();
            cmbZone.SelectedValue = (int)dgViewAddress.Rows[GetRow].Cells[4].Value;
            btnSave.Text = "Update";

        }

        private void cmbZone_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.SelectNextControl((Control)sender, true, true, true, true);
                e.SuppressKeyPress = true;
            }
        }

        private void frmAddress_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void txtAddress_TextChanged(object sender, EventArgs e)
        {
            txtAddressUrdu.Text = txtAddress.Text;
        }
    }
}

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
    public partial class frmZone : frmTemplete
    {
        public frmZone()
        {
            InitializeComponent();
        }
        clsZone czone = new clsZone();
        clsBlZone cbz = new clsBlZone();
        private void frmZone_Load(object sender, EventArgs e)
        {
            DataTable dtView = czone.ViewZone();
            dgViewZone.DataSource = dtView;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtZone.Text.Trim() != string.Empty && txtZoneUrdu.Text.Trim() != string.Empty && btnSave.Text == "Save")
            {
                cbz.ZoneName = txtZone.Text;
                cbz.ZoneNameUrdu=txtZoneUrdu.Text;
                if (czone.InsertZone(cbz)>0)
                {
                    MessageBox.Show("Your Zone has been Created...");
                    txtZone.Text = txtZoneUrdu.Text = string.Empty;
                    txtZone.Focus();
                    DataTable dtView = czone.ViewZone();
                    dgViewZone.DataSource = dtView;
                    btnSave.Text = "Save";
                    txtZone.Focus();
                }
            }
            else if (txtZone.Text.Trim() != string.Empty && txtZoneUrdu.Text.Trim() != string.Empty && btnSave.Text == "Update")
            {
                cbz.ZoneName = txtZone.Text;
                cbz.ZoneNameUrdu = txtZoneUrdu.Text;
                if (czone.UpdateZone(cbz) > 0)
                {
                    MessageBox.Show("Your Zone has been Updated...");
                    txtZone.Text = txtZoneUrdu.Text = string.Empty;
                    txtZone.Focus();
                    DataTable dtView = czone.ViewZone();
                    dgViewZone.DataSource = dtView;
                    btnSave.Text = "Update";
                    cbz.ZoneId = 0;
                    txtZone.Focus();
                }
            }
            else
            {
                MessageBox.Show("Please Enter Proper data");
            }
        }

        private void dgViewZone_DoubleClick(object sender, EventArgs e)
        {
            int GetRow = dgViewZone.Rows.GetFirstRow(DataGridViewElementStates.Selected);
            txtZone.Text = dgViewZone.Rows[GetRow].Cells[1].Value.ToString();
            txtZoneUrdu.Text = dgViewZone.Rows[GetRow].Cells[2].Value.ToString();
            cbz.ZoneId = (int) dgViewZone.Rows[GetRow].Cells[0].Value;
            btnSave.Text = "Update";
        }

        private void txtZone_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.SelectNextControl((Control)sender, true, true, true, true);
                e.SuppressKeyPress = true;
            }
        }

        private void frmZone_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void txtZone_TextChanged(object sender, EventArgs e)
        {
            txtZoneUrdu.Text = txtZone.Text;
        }
    }
}

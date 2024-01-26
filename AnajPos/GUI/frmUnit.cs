using AnajPos.BAL;
using AnajPos.DAL;
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
    public partial class frmUnit : frmTemplete
    {
        clsBlUnit BlUnit = new clsBlUnit();
        clsUnit DlUnit = new clsUnit();
        public frmUnit()
        {
            InitializeComponent();
            dgvUnit.DataSource = DlUnit.View();
            dgvUnit.Columns["UnitNameUrdu"].Visible = false;
        }
        private bool IsValidate()
        {
            if (txtUnitName.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Please Enter Unit Name ...");
                txtUnitName.Focus();
                return false;
            }
            return true;
        }

        private void dgvUnit_DoubleClick(object sender, EventArgs e)
        {
            int GetRow = dgvUnit.Rows.GetFirstRow(DataGridViewElementStates.Selected);
            BlUnit.UnitId = (int)dgvUnit.Rows[GetRow].Cells[0].Value;
            BlUnit.UnitName = dgvUnit.Rows[GetRow].Cells[1].Value.ToString();
            btnSave.Text = "Update";
            txtUnitName.Text = BlUnit.UnitName;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (IsValidate())
            {
                BlUnit.UnitName = txtUnitName.Text;
                if (BlUnit.UnitId > 0)
                {
                    if (DlUnit.Update(BlUnit) > 0)
                    {
                        MessageBox.Show("Unit Name has been Updated...");
                        dgvUnit.DataSource = DlUnit.View();
                        BlUnit.UnitId = 0;
                        btnSave.Text = "Save";
                        txtUnitName.Text = string.Empty;
                    }
                }
                else
                {
                    if (DlUnit.Insert(BlUnit) > 0)
                    {
                        MessageBox.Show("Unit has been created...");
                        dgvUnit.DataSource = DlUnit.View();
                        txtUnitName.Text = string.Empty;
                    }
                }



            }
        }

        
    }
}

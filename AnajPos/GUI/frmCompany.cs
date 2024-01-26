using AnajPos.BAL;
using AnajPos.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnajPos.GUI
{
    public partial class frmCompany : frmTemplete
    {
        clsBlCompany blCompany = new clsBlCompany();
        clsCompany DlCompany = new clsCompany();
        public frmCompany()
        {
            InitializeComponent();
            dgvCompany.DataSource = DlCompany.View();

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (IsValidate())
            {
                blCompany.CompanyName = txtComapanyName.Text;
                if (blCompany.ComapnyId > 0)
                {
                    if (DlCompany.Update(blCompany) > 0)
                    {
                        MessageBox.Show("Company has been Updated...");
                        dgvCompany.DataSource = DlCompany.View();
                        blCompany.ComapnyId = 0;
                        btnSave.Text = "Save";
                        txtComapanyName.Text = string.Empty;
                    }
                }
                else
                {
                    if (DlCompany.Insert(blCompany) > 0)
                    {
                        MessageBox.Show("Company has been created...");
                        dgvCompany.DataSource = DlCompany.View();
                        txtComapanyName.Text = string.Empty;
                    }
                }



            }
        }

        private bool IsValidate()
        {
            if (txtComapanyName.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Please Enter Comapny Name ...");
                txtComapanyName.Focus();
                return false;
            }
            return true;
        }

        private void dgvCompany_DoubleClick(object sender, EventArgs e)
        {
            int GetRow = dgvCompany.Rows.GetFirstRow(DataGridViewElementStates.Selected);
            blCompany.ComapnyId = (int)dgvCompany.Rows[GetRow].Cells[0].Value;
            blCompany.CompanyName = dgvCompany.Rows[GetRow].Cells[1].Value.ToString();
            btnSave.Text = "Update";
            txtComapanyName.Text = blCompany.CompanyName;
        }

       
    }
}

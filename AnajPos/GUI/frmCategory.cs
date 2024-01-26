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
    public partial class frmCategory : frmTemplete
    {
        clsBlCategory BlCategory = new clsBlCategory();
        clsCategory DLCategory = new clsCategory();

        public frmCategory()
        {
            InitializeComponent();
        }
        private bool IsValidate()
        {
            if (txtCategoryName.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Please Enter Category Name ...");
                txtCategoryName.Focus();
                return false;
            }
            return true;
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            if (IsValidate())
            {
                BlCategory.CategoryName = txtCategoryName.Text;
                if (BlCategory.CategoryId > 0)
                {
                    if (DLCategory.Update(BlCategory) > 0)
                    {
                        MessageBox.Show("Category has been Updated...");
                        dgvCategory.DataSource = DLCategory.View();
                        BlCategory.CategoryId = 0;
                        btnSave.Text = "Save";
                        txtCategoryName.Text = string.Empty;
                    }
                }
                else
                {
                    if (DLCategory.Insert(BlCategory) > 0)
                    {
                        MessageBox.Show("Category has been created...");
                        dgvCategory.DataSource = DLCategory.View();
                        txtCategoryName.Text = string.Empty;
                    }
                }
            }
        }

        private void dgvCategory_DoubleClick(object sender, EventArgs e)
        {
            int GetRow = dgvCategory.Rows.GetFirstRow(DataGridViewElementStates.Selected);
            BlCategory.CategoryId = (int)dgvCategory.Rows[GetRow].Cells[0].Value;
            BlCategory.CategoryName = dgvCategory.Rows[GetRow].Cells[1].Value.ToString();
            btnSave.Text = "Update";
            txtCategoryName.Text = BlCategory.CategoryName;
        }

        private void frmCategory_Load(object sender, EventArgs e)
        {
            dgvCategory.DataSource = DLCategory.View();
        }
    }
}

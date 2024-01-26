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
    public partial class frmExpenseHead : frmTemplete
    {
        clsBlChartOfAccounts BlCoa = new clsBlChartOfAccounts();
        clsChartOfAccount DlCoa = new clsChartOfAccount();
        public frmExpenseHead()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (IsValidate())
            {
                BlCoa.AccountName = txtExpenseHeadName.Text;
                // It's Chart of Account Expense ID
                BlCoa.ParentId = 2;

                if (BlCoa.AccountId > 0)
                {
                    if (DlCoa.Update(BlCoa) > 0)
                    {
                        MessageBox.Show("Warehouse has been Updated...");
                        dgvWarehouse.DataSource = DlCoa.ViewChartOfAccountParenttID(2);
                        BlCoa.AccountId = 0;
                        btnSave.Text = "Save";
                        txtExpenseHeadName.Text = string.Empty;
                    }
                }
                else
                {
                    if (DlCoa.Insert(BlCoa) > 0)
                    {
                        MessageBox.Show("Expense Head has been created...");
                        dgvWarehouse.DataSource = DlCoa.ViewChartOfAccountParenttID(2);
                        txtExpenseHeadName.Text = string.Empty;
                    }
                }



            }
        }

        private bool IsValidate()
        {
            if (txtExpenseHeadName.Text == string.Empty)
            {
                MessageBox.Show("Please Enter Expense Name");
                txtExpenseHeadName.Focus();
                return false;
            }
            return true;
        }

        private void frmExpenseHead_Load(object sender, EventArgs e)
        {
            DataTable dt = DlCoa.ViewChartOfAccountParenttID(2);

            // Specify the condition to identify the row to be removed
            string condition = "AccountId = 1041";

            // Use the Select method to find the rows that match the condition
            DataRow[] rowsToDelete = dt.Select(condition);

            // Remove each matching row
            foreach (DataRow row in rowsToDelete)
            {
                dt.Rows.Remove(row);
            }

            // Optional: Reset the row state to reflect the changes
            dt.AcceptChanges();
            dgvWarehouse.DataSource = dt;
            dgvWarehouse.Columns["Dr"].Visible = false;
            dgvWarehouse.Columns["Cr"].Visible = false;
            txtExpenseHeadName.Text = string.Empty;
        }

        private void dgvWarehouse_DoubleClick(object sender, EventArgs e)
        {
            int GetRow = dgvWarehouse.Rows.GetFirstRow(DataGridViewElementStates.Selected);
            BlCoa.AccountId = (int)dgvWarehouse.Rows[GetRow].Cells[0].Value;
            BlCoa.AccountName = dgvWarehouse.Rows[GetRow].Cells[1].Value.ToString();
            btnSave.Text = "Update";
            txtExpenseHeadName.Text = BlCoa.AccountName;
        }
    }
}

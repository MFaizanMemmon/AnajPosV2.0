using AnajPos.BAL_Cash_BANK;
using AnajPos.DAL;
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
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Windows.Forms;

namespace AnajPos.GUI_Bank_Cash
{
    public partial class frmCreateExpense : frmTemplete
    {
        clsChartOfAccount dlCoa = new clsChartOfAccount();
        clsBlExpense blExpense = new clsBlExpense();
        clsExpense dlExpense = new clsExpense();
        clsBLChartOfAccountLedger blCoaLedger = new clsBLChartOfAccountLedger();
        clsChartOfAccountLedger dlCoaLedger = new clsChartOfAccountLedger();

        public frmCreateExpense()
        {
            InitializeComponent();

        }

        private bool IsValidate()
        {
            if (cmbExpenseType.SelectedIndex == -1)
            {
                cmbExpenseType.Focus();
                MessageBox.Show("Please Select Expense Type");
                return false;
            }
            else if (cmbPaymentMode.SelectedIndex == -1)
            {
                cmbPaymentMode.Focus();
                MessageBox.Show("Please Select Payment Type");
                return false;
            }
            else if (txtAmount.Text.Trim() == string.Empty)
            {
                txtAmount.Clear();
                txtAmount.Focus();
                MessageBox.Show("Please Enter Amount");
                return false;

            }
            else if (txtNotes.Text.Trim() == string.Empty)
            {
                txtNotes.Clear();
                txtNotes.Focus();
                MessageBox.Show("Please Enter Notes");
                return false;
            }
            return true;
        }

        private void ClearField()
        {
            dtpExpneseDate.Value = DateTime.Now;

        }
        private void createExpenseHeadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmExpenseHead eh = new frmExpenseHead();
            eh.ShowDialog();
            frmCreateExpense_Load(this, null);
        }

        private void frmCreateExpense_Load(object sender, EventArgs e)
        {
            dgvViewExpense.DataSource = dlExpense.ViewExpenseByDate(dtpExpenseSort.Value);
            dgvViewExpense.Columns[6].Visible = false;
            dgvViewExpense.Columns[7].Visible = false;
            dgvViewExpense.Columns[0].Width = 50; // Set width for the 1st column
            dgvViewExpense.Columns[1].Width = 120; // Set width for the 2nd column
            dgvViewExpense.Columns[2].Width = 200; // Set width for the 3rd column
            dgvViewExpense.Columns[3].Width = 200; // Set width for the 3rd column
            dgvViewExpense.Columns["Notes"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvViewExpense.Columns["Amount"].Width = 120;


            cmbExpenseType.SelectedValueChanged -= new EventHandler(cmbExpenseType_SelectedValueChanged);
            DataTable dtView = dlCoa.ViewChartOfAccountParenttID(2);
            DataRow[] filteredRows = dtView.Select("accountid <> 1041");

            // If you want to create a new DataTable with the filtered rows
            DataTable filteredDataTable = dtView.Clone(); // Clone the structure of the original DataTable

            foreach (DataRow row in filteredRows)
            {
                filteredDataTable.ImportRow(row); // Import the filtered rows into the new DataTable
            }

            cmbExpenseType.DataSource = filteredDataTable;
            cmbExpenseType.ValueMember = "AccountId";
            cmbExpenseType.DisplayMember = "AccountName";
            cmbExpenseType.SelectedIndex = -1;
            cmbExpenseType.SelectedValueChanged += new EventHandler(cmbExpenseType_SelectedValueChanged);


            DataTable dtViewPaymentMode = dlCoa.ViewChartOfAccountPaymentMode();
            cmbPaymentMode.DataSource = dtViewPaymentMode;
            cmbPaymentMode.ValueMember = "AccountId";
            cmbPaymentMode.DisplayMember = "AccountName";
            cmbPaymentMode.SelectedIndex = -1;

            GetTotal();
        }
        private void GetTotal()
        {
            int FinalCost = 0;
           
            for (int i = 0; i < dgvViewExpense.Rows.Count; i++)
            {
                FinalCost = FinalCost + Convert.ToInt32(dgvViewExpense.Rows[i].Cells["Amount"].Value);
                
            }

            txtTotalExp.Text = FinalCost.ToString();
        }

        private void cmbExpenseType_SelectedValueChanged(object sender, EventArgs e)
        {

        }

        private void cmbPaymentMode_SelectedValueChanged(object sender, EventArgs e)
        {
           
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (IsValidate())
            {
                blExpense.ExpenseDate = dtpExpneseDate.Value;
                blExpense.ExpenseTypeID = (int)cmbExpenseType.SelectedValue;
                blExpense.PaymentModeID = (int)cmbPaymentMode.SelectedValue;
                blExpense.Amount = Convert.ToDecimal(txtAmount.Text);
                blExpense.Notes = txtNotes.Text;
                if (dlExpense.Insert(blExpense) > 0)
                {


                    MessageBox.Show("Expense has been created...");
                    dgvViewExpense.DataSource = dlExpense.ViewExpenseByDate(dtpExpenseSort.Value);
                    dtpExpneseDate.Value = DateTime.Now;
                    cmbExpenseType.SelectedValue = -1;
                    cmbPaymentMode.SelectedValue = -1;
                    txtAmount.Text = string.Empty;
                    txtNotes.Text = string.Empty;
                    dtpExpneseDate.Focus();
                    GetTotal();

                }

            }
        }

        private void dtpExpenseSort_ValueChanged(object sender, EventArgs e)
        {
            dgvViewExpense.DataSource = dlExpense.ViewExpenseByDate(dtpExpenseSort.Value);
            GetTotal();
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            if (IsValidate())
            {
                blExpense.ExpenseDate = dtpExpneseDate.Value;
                blExpense.ExpenseTypeID = (int)cmbExpenseType.SelectedValue;
                blExpense.PaymentModeID = (int)cmbPaymentMode.SelectedValue;
                blExpense.Amount = Convert.ToDecimal(txtAmount.Text);
                blExpense.Notes = txtNotes.Text;
                if (dlExpense.Update(blExpense) > 0)
                {
                    MessageBox.Show("Expense has been created...");
                    dgvViewExpense.DataSource = dlExpense.ViewExpenseByDate(dtpExpenseSort.Value);
                    dtpExpneseDate.Value = DateTime.Now;
                    cmbExpenseType.SelectedValue = -1;
                    cmbPaymentMode.SelectedValue = -1;
                    txtAmount.Text = string.Empty;
                    txtNotes.Text = string.Empty;
                    GetTotal();

                }

            }
        }

        private void dgvViewExpense_DoubleClick(object sender, EventArgs e)
        {
            int GetRow = dgvViewExpense.Rows.GetFirstRow(DataGridViewElementStates.Selected);
            blExpense.ExpenseId = (int)dgvViewExpense.Rows[GetRow].Cells[0].Value;
            dtpExpneseDate.Value = (DateTime)dgvViewExpense.Rows[GetRow].Cells[1].Value;
            cmbPaymentMode.SelectedValue = (int)dgvViewExpense.Rows[GetRow].Cells[6].Value;
            cmbExpenseType.SelectedValue = (int)dgvViewExpense.Rows[GetRow].Cells[7].Value;
            txtAmount.Text = dgvViewExpense.Rows[GetRow].Cells[5].Value.ToString();
            txtNotes.Text = dgvViewExpense.Rows[GetRow].Cells[4].Value.ToString();

        }

        private void btnPrint_Click(object sender, EventArgs e)
        {

        }

        private void txtAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') )
            {
                e.Handled = true;
            }
            if (e.KeyChar == 45)
            {
                e.Handled = false;
            }
        }

        private void dtpExpneseDate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.SelectNextControl((Control)sender, true, true, true, true);
                e.SuppressKeyPress = true;
            }
        }
    }
}

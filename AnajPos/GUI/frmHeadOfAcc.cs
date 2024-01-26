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
    public partial class frmHeadOfAcc : frmTemplete
    {
        clsBlHeadOfAcc BlHead = new clsBlHeadOfAcc();
        clsHeadOfAcc DlHead = new clsHeadOfAcc();
        public frmHeadOfAcc()
        {
            InitializeComponent();
        }

        private void frmHeadOfAcc_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = DlHead.ViewHead();
        }

        private void btnCreateAccount_Click(object sender, EventArgs e)
        {
            if (btnCreateAccount.Text == "Modify")
            {
                if (IsValidated())
                {
                    BlHead.HeadId = int.Parse(txtId.Text);
                    BlHead.HeadAccName = txtAccountName.Text;
                    BlHead.HeadAccNameUrdu = txtAccountNameUrdu.Text;
                    BlHead.OpeningDate = dtpOpeningDate.Value;
                    BlHead.Remark = "Strarting Account";
                    BlHead.OpeningAmount = int.Parse(txtOpeningAmount.Text);
                    if (DlHead.Update(BlHead)>0)
                    {
                        MessageBox.Show("Your Account has been Modify...");
                        dataGridView1.DataSource = DlHead.ViewHead();
                        ClearField();
                        DisableField();
                        btnNewAccount.Enabled = true;
                        btnCreateAccount.Text = "Created";
                        btnCreateAccount.Enabled = false;
                        btnNewAccount.Focus();
                    }
                    
                }   
            }
            else
            {
                 if (IsValidated())
                 {
                     BlHead.HeadId = int.Parse(txtId.Text);   
                     BlHead.HeadAccName = txtAccountName.Text;
                     BlHead.HeadAccNameUrdu = txtAccountNameUrdu.Text;
                     BlHead.OpeningDate =dtpOpeningDate.Value;
                     BlHead.Remark = "Strarting Account";
                     BlHead.OpeningAmount = int.Parse(txtOpeningAmount.Text);
                     if (DlHead.Insert(BlHead)>0)                      
                     {
                         MessageBox.Show("Your Account has been Created...");   
                         dataGridView1.DataSource = DlHead.ViewHead();
                         ClearField();
                         DisableField();
                         btnNewAccount.Enabled = true;
                         btnCreateAccount.Enabled = false;
                         btnNewAccount.Focus();
                     }
                 }
            }
           
        }

        private bool IsValidated()
        {
            if (txtId.Text== string.Empty)
            {
                return false;
            }
            else if (txtAccountName.Text==string.Empty)
            {
                return false;
            }
            else if (txtAccountNameUrdu.Text== string.Empty)
            {
                return false;
            }
            else if (dtpOpeningDate.Value > DateTime.Now)
            {
                return false;
            }
            else if (txtOpeningAmount.Text== string.Empty)
            {
                return false;
            }
            return true;
        }

        private void btnNewAccount_Click(object sender, EventArgs e)
        {
            EnableField();
            txtId.Text = DlHead.NewId().ToString();
            btnCreateAccount.Enabled = true;
            btnNewAccount.Enabled = false;
            txtAccountName.Focus();
        }

       private void EnableField() 
       {
           txtAccountName.Enabled = true;
           txtAccountNameUrdu.Enabled = true;
           dtpOpeningDate.Enabled = true;
           txtOpeningAmount.Enabled = true;
       }
        private void DisableField()
       {
           txtAccountName.Enabled = false;
           txtAccountNameUrdu.Enabled = false;
           dtpOpeningDate.Enabled = false;
           txtOpeningAmount.Enabled = false;
       }
        private void ClearField()
        {
            txtId.Text = string.Empty;
            txtAccountName.Text = string.Empty;
            txtAccountNameUrdu.Text = string.Empty;
            dtpOpeningDate.Value = DateTime.Now;
            txtOpeningAmount.Text = string.Empty;
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            int GetRow = dataGridView1.Rows.GetFirstRow(DataGridViewElementStates.Selected);
            txtId.Text = dataGridView1.Rows[GetRow].Cells[0].Value.ToString();
            txtAccountName.Text = dataGridView1.Rows[GetRow].Cells[1].Value.ToString();
            dtpOpeningDate.Text = dataGridView1.Rows[GetRow].Cells[2].Value.ToString();
            txtOpeningAmount.Text = dataGridView1.Rows[GetRow].Cells[3].Value.ToString();
            txtAccountNameUrdu.Text = dataGridView1.Rows[GetRow].Cells[4].Value.ToString();
            btnNewAccount.Enabled = false;
            btnCreateAccount.Enabled = true;
            btnCreateAccount.Text = "Modify";
            EnableField();
            txtId.Enabled = false;
        }

        private void txtAccountName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.SelectNextControl((Control)sender, true, true, true, true);
                e.SuppressKeyPress = true;
            }
        }

        private void frmHeadOfAcc_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
            else if (e.KeyCode == Keys.F1)
            {
                if (btnNewAccount.Enabled == true)
                {
                    btnNewAccount_Click(null, new EventArgs());    
                }
            }
            else if (e.KeyCode == Keys.F2)
            {
                if (btnCreateAccount.Enabled== true)
                {
                    btnCreateAccount_Click(null, new EventArgs());
                }
            }
        }

        private void txtOpeningAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

    }
}

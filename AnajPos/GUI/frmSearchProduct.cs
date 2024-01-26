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

namespace AnajPos.GUI
{
    public partial class frmSearchProduct : frmTemplete
    {
        clsProduct DlProduct = new clsProduct();
        public bool IsOpenProduct { get; set; }
        public frmSearchProduct()
        {
            InitializeComponent();
            
        }

        private void frmSearchProduct_Load(object sender, EventArgs e)
        {
            dgViewProduct.DataSource = DlProduct.ViewProduct();

        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            dgViewProduct.DataSource = null;
            if (txtSearch.Text != string.Empty)
            {
                dgViewProduct.DataSource = DlProduct.ViewProductSearch(txtSearch.Text);    
            }
            else
            {
                dgViewProduct.DataSource = DlProduct.ViewProduct();
            }
            
        }

        private void dgViewProduct_DoubleClick(object sender, EventArgs e)
        {
            int rowIndex = dgViewProduct.Rows.GetFirstRow(DataGridViewElementStates.Selected);
            string Id = dgViewProduct.Rows[rowIndex].Cells[0].Value.ToString();
            //MessageBox.Show(Id.ToString());
            if (IsOpenProduct)
            {
                Properties.Settings.Default.ProductID = int.Parse(Id);
                this.Close();
            }
            else
            {
                Properties.Settings.Default.ProductID = int.Parse(Id);
                this.Close();
            }
            
        }

        private void frmSearchProduct_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                int rowIndex = dgViewProduct.Rows.GetFirstRow(DataGridViewElementStates.Selected);
                string Id = dgViewProduct.Rows[rowIndex].Cells[0].Value.ToString();
                //MessageBox.Show(Id.ToString());
                if (IsOpenProduct)
                {
                    Properties.Settings.Default.ProductID = int.Parse(Id);
                    this.Close();
                }
                else if (e.KeyCode == Keys.Escape)
                {
                    this.Close();
                }
                else
                {
                    Properties.Settings.Default.ProductID = int.Parse(Id);
                    this.Close();
                }

                
            }
        }

        private void frmSearchProduct_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void frmSearchProduct_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!IsOpenProduct && dgViewProduct.SelectedRows.Count >0)
            {
                int rowIndex = dgViewProduct.Rows.GetFirstRow(DataGridViewElementStates.Selected);
                string Id = dgViewProduct.Rows[rowIndex].Cells[0].Value.ToString();
                
                Properties.Settings.Default.ProductID = int.Parse(Id);
            

            }
            else
            {
                Properties.Settings.Default.ProductID = 1;
            }
        }
    }
}

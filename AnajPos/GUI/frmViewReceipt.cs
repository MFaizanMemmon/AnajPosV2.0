using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AnajPos.DAL;
using System.Globalization;

namespace AnajPos.GUI
{
    public partial class frmViewReceipt : frmTemplete
    {
        public DateTime Today { get; set; }
        clsReceipt cr = new clsReceipt();
        public frmViewReceipt()
        {
            InitializeComponent();
        }

        private void frmViewReceipt_Load(object sender, EventArgs e)
        {
           
            dataGridView1.DataSource = null;
            dataGridView1.DataSource= cr.ViewScreen(dateTimePicker1.Text);
            GetTotalAmount();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = cr.ViewScreen(dateTimePicker1.Text);
            GetTotalAmount();
        }

        private void GetTotalAmount()
        {
            int Amount = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
               
                Amount += int.Parse(dataGridView1.Rows[i].Cells[6].Value.ToString());
            }
            txtTotalAmount.Text = Amount.ToString();
        }

        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            printPreviewDialog1.Document = printDocument1;
            printPreviewDialog1.ShowDialog();
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawString("Reciving Details", new Font("Arial",16, FontStyle.Bold), Brushes.Blue, new Point(5, 20));
            e.Graphics.DrawString(dateTimePicker1.Value.ToShortDateString(), new Font("Arial", 16, FontStyle.Bold), Brushes.Blue, new PointF(600, 20));

            e.Graphics.DrawString("ID \t Customer Name \t\t Address \t\t Remark \t Amount", new Font("Arial", 12, FontStyle.Bold), Brushes.Black, new Point(15, 90));

            int YPos = 120;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
               
                string Id, Name, Address, Remark;
                int Amount = 0;
                Id = dataGridView1.Rows[i].Cells[0].Value.ToString();
                Name = dataGridView1.Rows[i].Cells[2].Value.ToString();
                Address = dataGridView1.Rows[i].Cells[3].Value.ToString();
                Remark = dataGridView1.Rows[i].Cells[5].Value.ToString();
                Amount = Convert.ToInt32(dataGridView1.Rows[i].Cells[6].Value.ToString());
          
                e.Graphics.DrawString(Id,new Font("Arial",11,FontStyle.Regular),Brushes.Black,new Point(15,YPos));
                e.Graphics.DrawString(Name,new Font("Arial", 11, FontStyle.Regular), Brushes.Black, new Point(85, YPos));
                e.Graphics.DrawString(Address, new Font("Arial", 11, FontStyle.Regular), Brushes.Black, new Point(350, YPos));
                e.Graphics.DrawString(Remark, new Font("Arial", 11, FontStyle.Regular), Brushes.Black, new Point(550, YPos));
                e.Graphics.DrawString(Amount.ToString("C0",CultureInfo.CreateSpecificCulture("ur-pk")), new Font("Arial", 11, FontStyle.Regular), Brushes.Black, new Point(680, YPos));
                YPos += 30;
            }
            YPos += 30;
         
            e.Graphics.DrawString("Total Amount is : " +txtTotalAmount.Text, new Font("Arial", 11, FontStyle.Regular), Brushes.Black, new Point(600, YPos));
        }

       
    }
}

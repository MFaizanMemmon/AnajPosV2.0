using AnajPos.DAL;
using AnajPos.GUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnajPos.GUI_Bank_Cash
{
    public partial class frmViewPendingCounterSale : frmTemplete
    {
        SqlConnection conn = new SqlConnection(clsConnectionString.cs);
        public frmViewPendingCounterSale()
        {
            InitializeComponent();
        }

        private void frmViewPendingCounterSale_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = ViewPendingOrder();
        }

        public DataTable ViewPendingOrder()
        {
            DataTable dt = new DataTable();

            using (SqlCommand cmd = new SqlCommand("usp_GetPendingCounterSale", conn))
            {
                SqlDataAdapter adpt = new SqlDataAdapter(cmd);
                adpt.Fill(dt);
            }
            return dt;
        }
    }
}

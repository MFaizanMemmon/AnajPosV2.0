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
using System.Data.SqlClient;
using AnajPos.DAL;

namespace AnajPos.GUI
{
    public partial class frmViewOrder : frmTemplete
    {
        public frmViewOrder()
        {
            InitializeComponent();
        }

        private void frmViewOrder_Load(object sender, EventArgs e)
        {
            using (SqlConnection conn=new SqlConnection(clsConnectionString.cs))
            {
                SqlDataAdapter adpt = new SqlDataAdapter("select distinct(tblInvoice.[Sid]) as 'ID', CONVERT(varchar(50),tblInvoice.SDate,106) as 'Date',tblZone.ZoneName as 'Area',tblAddress.AddressName as 'Address' ,tblCustomer.CustomerName as 'Name',tblInvoice.Remark,tblInvoice.OrderDate 'Order Date' from tblInvoice inner join tblZone on tblInvoice.ZoneId=tblZone.ZoneId inner join tblAddress on tblInvoice.AreaId=tblAddress.AddressID inner join tblCustomer on tblCustomer.CustomerID=tblInvoice.CustomerID where tblInvoice.PostingType ='Order'",conn);
                DataTable dt = new DataTable();
                adpt.Fill(dt);
                this.dataGridView1.DataSource = dt;
            }
        }
    }
}

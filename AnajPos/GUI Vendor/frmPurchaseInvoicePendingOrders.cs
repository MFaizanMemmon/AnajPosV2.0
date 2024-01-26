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

namespace AnajPos.GUI_Vendor
{
    public partial class frmPurchaseInvoicePendingOrders : frmTemplete
    {
        public frmPurchaseInvoicePendingOrders()
        {
            InitializeComponent();
        }

        private void frmPurchaseInvoicePendingOrders_Load(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionString.cs))
            {
                SqlDataAdapter adpt = new SqlDataAdapter("select tblPurchase.PId as 'ID',tblPurchase.PDate as 'Date',tblZone.ZoneName as 'Area',tblAddress.AddressName as 'Address',tblVendor.VendorName,tblPurchase.Remark,tblPurchase.OrderDate from tblPurchase inner join tblZone on tblZone.ZoneId=tblPurchase.ZoneId inner join tblAddress on tblAddress.AddressID=tblPurchase.AreaId inner join tblVendor on tblVendor.VendorID=tblPurchase.VendorId  where tblPurchase.PostingType = 'Order'", conn);
                DataTable dt = new DataTable();
                adpt.Fill(dt);
                this.dataGridView1.DataSource = dt;
            }
        }
    }
}

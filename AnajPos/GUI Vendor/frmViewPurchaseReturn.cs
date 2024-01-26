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

namespace AnajPos.GUI_Vendor
{
    public partial class frmViewPurchaseReturn : frmTemplete
    {
        public frmViewPurchaseReturn()
        {
            InitializeComponent();
        }

        private void frmViewPurchaseReturn_Load(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(DAL.clsConnectionString.cs))
            {
                SqlDataAdapter adpt = new SqlDataAdapter("select tblPurchaseR.ReturnId as 'ID',tblZone.ZoneName as 'Address',tblAddress.AddressName as 'Address',tblVendor.VendorName,convert(varchar(50),tblPurchaseR.ReturnDate) as 'Date',tblPurchaseR.Remark,tblPurchaseR.Amount from tblPurchaseR inner join tblVendor on tblVendor.VendorID=tblPurchaseR.VendorId inner join tblAddress on tblAddress.AddressID=tblVendor.VendorAddress inner join tblZone on tblZone.ZoneId=tblVendor.VendorID", conn);
                DataTable dt = new DataTable();
                adpt.Fill(dt);
                dataGridView1.DataSource = dt;
            }
        }
    }
}

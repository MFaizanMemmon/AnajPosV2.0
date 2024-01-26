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

namespace AnajPos.GUI
{
    public partial class frmViewSaleReturn : frmTemplete
    {
        public frmViewSaleReturn()
        {
            InitializeComponent();
        }

        private void frmViewSaleReturn_Load(object sender, EventArgs e)
        {
            using (SqlConnection conn=new SqlConnection(DAL.clsConnectionString.cs))
            {
                SqlDataAdapter adpt = new SqlDataAdapter("select tblSaleR.ReturnId as 'ID',tblZone.ZoneName as 'Zone',tblAddress.AddressName,tblCustomer.CustomerName, convert(varchar(50),tblSaleR.ReturnDate,106) as 'Date',tblSaleR.Remark,tblSaleR.Amount from tblSaleR inner join tblZone on tblZone.Zoneid=tblSaleR.ZoneId inner join tblAddress on tblAddress.AddressID=tblSaleR.Addressid inner join tblCustomer on tblCustomer.CustomerID=tblSaleR.CustomerId", conn);
                DataTable dt = new DataTable();
                adpt.Fill(dt);
                dataGridView1.DataSource = dt;
            }
        }
    }
}

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
using AnajPos.DalVendor;
using AnajPos.BALVendor;
using AnajPos.BAL_Cash_BANK;
using AnajPos.DAL_Cash_Bank;
using System.Data.SqlClient;

namespace AnajPos.GUI_Vendor
{
    public partial class frmPurchaseReturn : frmTemplete
    {
        public frmPurchaseReturn()
        {
            InitializeComponent();
        }
        clsZone cz = new clsZone();
        clsAddress DlAddress = new clsAddress();
        clsBlAddress BlAddress = new clsBlAddress();
        clsCustomer DlCustomer = new clsCustomer();
        clsBlCustomer BlCustomer = new clsBlCustomer();
        clsProduct DlProduct = new clsProduct();
        clsSaleReturn DlSaleReturn = new clsSaleReturn();
        clsBlSaleReturn BlSaleReturn = new clsBlSaleReturn();
        clsBlTransaction BlTransaction = new clsBlTransaction();
        clsTransaction DlTransaction = new clsTransaction();
        clsBlVendor BlVendor = new clsBlVendor();
        clsDlVendor DlVendor = new clsDlVendor();
        clsDlPurchaseReturn DlPurcharR = new clsDlPurchaseReturn();
        clsBlPurchaseReturn BlPurchaseR = new clsBlPurchaseReturn();
        clsBlReturnProcduct BlReturnProduct = new clsBlReturnProcduct();
        clsWarehouse DlWarehoues = new clsWarehouse();
        clsStock DlStock = new clsStock();
        clsBlStock BlStock = new clsBlStock();

        clsBLChartOfAccountLedger blCoaLedger = new clsBLChartOfAccountLedger();
        clsChartOfAccountLedger dlCoaLedger = new clsChartOfAccountLedger();



        private void frmPurchaseReturn_Load(object sender, EventArgs e)
        {
            cmbProduct.SelectedValueChanged -= new EventHandler(cmbProduct_SelectedValueChanged);
            DataTable dt2 = new DataTable();
            dt2 = DlProduct.ViewProduct();
            cmbProduct.DataSource = dt2;
            cmbProduct.ValueMember = "ProductId";
            cmbProduct.DisplayMember = "ProductName";
            cmbProduct.SelectedValueChanged += new EventHandler(cmbProduct_SelectedValueChanged);


            DataTable dt = new DataTable();
            cmbZone.SelectedIndexChanged -= new EventHandler(cmbZone_SelectedIndexChanged);
            cmbZone.SelectedValueChanged -= new EventHandler(cmbZone_SelectedValueChanged);
            DataTable dtView = cz.ViewZone();
            cmbZone.DataSource = dtView;
            cmbZone.ValueMember = "Id";
            cmbZone.DisplayMember = "Name of Zone";
            cmbZone.SelectedIndex = -1;
            // cmbZone.SelectedIndexChanged += new EventHandler(cmbZone_SelectedIndexChanged);
            cmbZone.SelectedValueChanged += new EventHandler(cmbZone_SelectedValueChanged);

            cmbLocation.SelectedValueChanged -= new EventHandler(cmbLocation_SelectedValueChanged);
            DataTable dtStockLcation = DlWarehoues.View();
            cmbLocation.DataSource = dtStockLcation;
            cmbLocation.ValueMember = "WarehouseId";
            cmbLocation.DisplayMember = "WarehouseName";
            cmbLocation.SelectedIndex = -1;
            cmbLocation.SelectedValueChanged += new EventHandler(cmbLocation_SelectedValueChanged);


            //________ Ids for Pagination ______
            lblFirstId.Text = MinId().ToString();
            lblLastId.Text = MaxId().ToString();
            lblCurrentId.Text = MinId().ToString();

            FillDataFieldInPurchase(int.Parse(lblFirstId.Text));


            FillGrid();
            lblCurrentId.Text = txtID.Text;
            btnNew.Focus();

        }

        private void cmbZone_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cmbZone.SelectedIndex != -1)
            {
                BlAddress.AddressId = (int)cmbZone.SelectedValue;
                cmbAddress.DataSource = DlAddress.ViewAddressByZone(BlAddress);
                cmbAddress.DisplayMember = "AddressName";
                cmbAddress.ValueMember = "AddressID";
            }
        }

        private void cmbAddress_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                BlVendor.AddressName = Convert.ToInt32(cmbAddress.SelectedValue);
                DataTable dt = DlVendor.ViewByAddress(BlVendor);
                cmbSelectVendor.DataSource = dt;
                cmbSelectVendor.DisplayMember = "VendorName";
                cmbSelectVendor.ValueMember = "VendorID";
                //  cmbCustomer.SelectedIndex = -1;
            }
            catch
            { }
        }


        private bool IsValidated()
        {
            if (cmbSelectVendor.SelectedIndex == -1)
            {
                MessageBox.Show("Please Select the Customer");
                return false;
            }
            else if (txtAmount.Text == string.Empty)
            {
                MessageBox.Show("Please Enter Amount");
                return false;
            }

            return true;
        }



        private void cmbProduct_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cmbSelectVendor.SelectedIndex == -1)
            {
                MessageBox.Show("Please select Account");
                return;
            }


            if (cmbProduct.SelectedValue != null)
            {
                using (SqlConnection conn = new SqlConnection(clsConnectionString.cs))
                {
                    string q = string.Format("select PurchaseDate,PurchaseId,Qty,ProductName,Kg,Price,Weight,Amount from tblProductTransactionVendor where Vendorid ={0} and ProductId ={1}", (int)cmbSelectVendor.SelectedValue, (int)cmbProduct.SelectedValue);
                    SqlDataAdapter adpt = new SqlDataAdapter(q, conn);
                    DataTable dt2 = new DataTable();
                    adpt.Fill(dt2);
                    dataGridView1.DataSource = dt2;
                }
            }


            DataTable dt = GetProductDetail((int)cmbSelectVendor.SelectedValue, (int)cmbProduct.SelectedValue);
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                string kg;
                kg = dr[6].ToString();
                //price = dr[""].ToString();
                if (kg == string.Empty)
                {
                    if (txtQty.Text == string.Empty)
                    {
                        txtQty.Text = "1";
                    }
                    chkIsKg.Checked = false;
                    //txtPrice.Text = price;
                    txtKg.Text = string.Empty;
                    txtWeight.Text = string.Empty;

                }
                else
                {
                    if (txtQty.Text == string.Empty)
                    {
                        txtQty.Text = "1";
                    }
                    chkIsKg.Checked = true;
                    txtKg.Text = kg;
                    //txtPrice.Text = price;
                }

            }
            else
            {
                //  MessageBox.Show("Old data found", "Inform", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DataTable dtt = GetProductDetail((int)cmbProduct.SelectedValue);
                if (dtt.Rows.Count > 0)
                {
                    DataRow drr = dtt.Rows[0];
                    string kg, price;
                    kg = drr[6].ToString();
                    price = drr[8].ToString();
                    if (kg == string.Empty)
                    {
                        if (txtQty.Text == string.Empty)
                        {
                            txtQty.Text = "1";
                        }
                        chkIsKg.Checked = false;
                        // txtPrice.Text = price;
                        txtKg.Text = string.Empty;
                        txtWeight.Text = string.Empty;

                    }
                    else
                    {
                        if (txtQty.Text == string.Empty)
                        {
                            txtQty.Text = "1";
                        }
                        chkIsKg.Checked = true;
                        txtKg.Text = kg;
                        txtPrice.Text = price;
                    }

                }
                else
                {

                    DataTable dtProductDetail = GetProductDetailByDefault((int)cmbProduct.SelectedValue);
                    if (dtProductDetail.Rows.Count > 0)
                    {
                        DataRow drProductDetailRow = dtProductDetail.Rows[0];
                        txtKg.Text = drProductDetailRow[5].ToString();
                        txtKg_TextChanged(txtKg, EventArgs.Empty);
                        //txtPrice.Text = drProductDetailRow[7].ToString();
                        txtPrice_TextChanged(txtPrice, EventArgs.Empty);
                    }
                    else
                    {
                        txtKg.TextChanged -= new EventHandler(txtKg_TextChanged);
                        txtKg.Text = string.Empty;
                        txtKg.TextChanged += new EventHandler(txtKg_TextChanged);
                        txtWeight.Text = string.Empty;
                        txtPrice.TextChanged -= new EventHandler(txtPrice_TextChanged);
                        //txtPrice.Text = string.Empty;
                        txtPrice.TextChanged += new EventHandler(txtPrice_TextChanged);
                    }

                }


            }
        }

        private void cmbAddress_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cmbLocation_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbProduct.SelectedValue != null && cmbLocation.SelectedValue != null)
                {
                    lblTotalStock.Text = DlStock.GetStockByProductIdAndWarehouseId((int)cmbProduct.SelectedValue, (int)cmbLocation.SelectedValue).ToString();
                }
            }
            catch
            {


            }
        }

        private void cmbZone_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private int MaxId()
        {
            int status = 0;
            using (SqlConnection conn = new SqlConnection(clsConnectionString.cs))
            {
                string Query = "select max([ReturnId]) from tblPurchaseR";

                using (SqlCommand cmd = new SqlCommand(Query, conn))
                {
                    if (conn.State != ConnectionState.Open)
                    {
                        conn.Open();
                    }
                    try
                    {
                        status = (int)cmd.ExecuteScalar();
                    }
                    catch
                    {
                        status = 0;
                    }
                    conn.Close();
                }
            }
            return status;

        }


        private int MinId()
        {
            int status = 0;
            using (SqlConnection conn = new SqlConnection(clsConnectionString.cs))
            {
                string Query = "select min([ReturnId]) from tblPurchaseR";

                using (SqlCommand cmd = new SqlCommand(Query, conn))
                {
                    if (conn.State != ConnectionState.Open)
                    {
                        conn.Open();
                    }
                    try
                    {
                        status = (int)cmd.ExecuteScalar();
                    }
                    catch
                    {
                        status = 0;
                    }
                    conn.Close();
                }
            }
            return status;

        }

        private void FillDataFieldInPurchase(int Id)
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionString.cs))
            {
                string q = string.Format("select ReturnId,Zoneid,Area,VendorId,ReturnDate,Remark,Amount,isnull(IsLedger,0) IsLedger from tblPurchaseR Where [returnId] ={0}", Id);
                SqlDataAdapter adpt = new SqlDataAdapter(q, conn);
                DataTable dt = new DataTable();
                adpt.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    txtID.Text = dr[0].ToString();
                    cmbZone.SelectedValue = (int)dr[1];
                    cmbAddress.SelectedValue = (int)dr[2];
                    cmbSelectVendor.SelectedValue = (int)dr[3];
                    dtpSaleR.Value = (DateTime)dr[4];
                    txtRemark.Text = dr[5].ToString();
                    txtAmount.Text = dr[6].ToString();
                    if ((bool)dr[7] == true)
                    {
                        btnPost.Enabled = false;

                    }
                    else
                    {
                        btnAddProduct.Enabled = true;
                        btnDeleteProduct.Enabled = true;
                        cmbProduct.Enabled = true;
                        txtKg.Enabled = true;
                        txtQty.Enabled = true;
                        txtWeight.Enabled = true;
                        txtPrice.Enabled = true;
                        cmbLocation.Enabled = true;
                        btnPost.Enabled = true;
                    }
                }
            }
        }

        public void FillGrid()
        {
            dataGridView2.DataSource = null;
            if (txtID.Text != string.Empty)
            {
                dataGridView2.DataSource = DlSaleReturn.GetReturndetail(int.Parse(txtID.Text), "PurchaseReturn");
                dataGridView2.Columns["ReturnId"].Visible = false;
                dataGridView2.Columns["TransactionId"].Visible = false;
                dataGridView2.Columns["ProductId"].Visible = false;
            }
        }
        private void GetTotal()
        {
            if (dataGridView2.Rows.Count > 0)
            {
                int FinalCost = 0;

                for (int i = 0; i < dataGridView2.Rows.Count; i++)
                {
                    FinalCost = FinalCost + Convert.ToInt32(dataGridView2.Rows[i].Cells[5].Value);

                }

                txtAmount.Text = FinalCost.ToString();
            }

        }
        private DataTable GetProductDetail(int cId, int pid)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(clsConnectionString.cs))
            {
                string qurery = string.Format("select * from tblProductTransaction where Customerid ={0} and ProductId = {1} and InvoiceDate=(select MAX(InvoiceDate) from tblProductTransaction where ProductId = {2} and CustomerId = {3})", cId, pid, pid, cId);
                SqlDataAdapter adpt = new SqlDataAdapter(qurery, conn);
                adpt.Fill(dt);
            }

            return dt;
        }

        private DataTable GetProductDetail(int Pid)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(clsConnectionString.cs))
            {
                string qurery = string.Format("select * from tblProductTransaction where ProductId = {0} and InvoiceDate =(select max(InvoiceDate) from tblProductTransaction where ProductId = {1})", Pid, Pid);
                SqlDataAdapter adpt = new SqlDataAdapter(qurery, conn);
                adpt.Fill(dt);
            }

            return dt;
        }
        private DataTable GetProductDetailByDefault(int Pid)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(clsConnectionString.cs))
            {
                string qurery = string.Format("select * from tblProduct where ProductId = {1}", Pid, Pid);
                SqlDataAdapter adpt = new SqlDataAdapter(qurery, conn);
                adpt.Fill(dt);
            }

            return dt;
        }

        private void txtKg_TextChanged(object sender, EventArgs e)
        {
            if (txtQty.Text != string.Empty && txtKg.Enabled == true && txtKg.Text != string.Empty)
            {
                txtWeight.Text = Convert.ToString(Convert.ToDouble(txtQty.Text) * Convert.ToDouble(txtKg.Text));
            }
        }

        private void txtPrice_TextChanged(object sender, EventArgs e)
        {
            if (txtQty.Text.Length > 9)
            {
                txtQty.Text = string.Empty;
            }
            if (chkIsKg.Checked)
            {
                if (txtPrice.Text != string.Empty && txtWeight.Text != string.Empty)
                {
                    lblTotalAmount.Text = Convert.ToString(Convert.ToDouble(txtWeight.Text) * Convert.ToDouble(txtPrice.Text));
                }
                else
                {

                }
            }
            else
            {
                if (txtPrice.Text != string.Empty)
                {
                    lblTotalAmount.Text = Convert.ToString(int.Parse(txtPrice.Text) * int.Parse(txtQty.Text));
                }
            }
        }

        private void txtQty_TextChanged(object sender, EventArgs e)
        {
            try
            {

                if (txtQty.Text.Length > 9)
                {
                    txtQty.Text = string.Empty;
                }
                if (txtQty.Text != string.Empty && txtKg.Enabled == true && txtKg.Text != string.Empty)
                {
                    txtWeight.Text = Convert.ToString(Convert.ToDouble(txtQty.Text) * Convert.ToDouble(txtKg.Text));
                }
            }
            catch
            {


            }
        }

        private void txtWeight_TextChanged(object sender, EventArgs e)
        {
            txtQty_TextChanged(txtQty, EventArgs.Empty);
            txtPrice_TextChanged(txtPrice, EventArgs.Empty);
        }

        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            if (txtQty.Text == string.Empty && txtPrice.Text == string.Empty && cmbProduct.SelectedIndex != -1)
            {
                MessageBox.Show("Please Enter Proper Data...");
                return;
            }


            if (IsProductAlreadyExists(cmbProduct.Text))
            {

                if (int.Parse(txtQty.Text) > int.Parse(lblTotalStock.Text))
                {
                    MessageBox.Show("Quantity would be less then Stock");
                    return;
                }


                BlReturnProduct.ProductId = (int)cmbProduct.SelectedValue;
                BlReturnProduct.ReturnId = int.Parse(txtID.Text);
                BlReturnProduct.ReturnDate = dtpSaleR.Value;
                BlReturnProduct.CustomerVendorAccountId = (int)cmbSelectVendor.SelectedValue;
                BlReturnProduct.Qty = int.Parse(txtQty.Text);
                BlReturnProduct.ProductName = cmbProduct.Text;
                BlReturnProduct.Kg = float.Parse(txtKg.Text);
                BlReturnProduct.Weight = float.Parse(txtWeight.Text);
                BlReturnProduct.Price = decimal.Parse(txtPrice.Text);
                BlReturnProduct.Amount = decimal.Parse(lblTotalAmount.Text);
                BlReturnProduct.Type = "PurchaseReturn";
                if (DlSaleReturn.InsertReturnProduct(BlReturnProduct) > 0)
                {
                    //BlStock.ProductTransDate = dtpSaleR.Value;
                    //BlStock.ProductTransId = int.Parse(txtID.Text);
                    //BlStock.TransactionType = "PurchaseReturn";
                    //BlStock.ProductId = (int)cmbProduct.SelectedValue;
                    //BlStock.UnitID = 1;
                    //BlStock.WarehouseId = (int)cmbLocation.SelectedValue;
                    //BlStock.StockIn = int.Parse(txtQty.Text);
                    //BlStock.StockOut = 0;
                    //BlStock.Amount = int.Parse(txtPrice.Text);
                    //BlStock.IsDeleted = false;
                    //if (DlStock.Insert(BlStock) > 0)
                    //{

                    FillGrid();
                    ClearFieldByProduct();
                    GetTotal();


                    using (SqlConnection conn = new SqlConnection(clsConnectionString.cs))
                    {
                        string query = string.Format("update tblPurchaseR set Amount ={0} where ReturnId ={1}", int.Parse(txtAmount.Text), int.Parse(txtID.Text));


                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            conn.Open();
                            cmd.ExecuteNonQuery();
                            cmbProduct.Focus();
                        }
                        conn.Close();
                    }
                    ClearProduct();


                    //}


                }

            }



        }
        private bool IsProductAlreadyExists(string ValidText)
        {
            if (dataGridView2.Rows.Count > 0)
            {
                for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                {
                    if (ValidText == dataGridView1.Rows[i].Cells[0].Value.ToString())
                    {
                        MessageBox.Show("this Product is Already exists in this Order Please Update this Product", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return false;
                    }
                }

            }
            return true;


        }
        private void ClearProduct()
        {
            cmbProduct.SelectedIndex = -1;
            txtKg.Text = string.Empty;
            txtQty.Text = string.Empty;
            txtPrice.Text = string.Empty;
            txtWeight.Text = string.Empty;
            lblTotalAmount.Text = string.Empty;
            cmbLocation.SelectedIndex = -1;
            lblTotalStock.Text = string.Empty;
            cmbProduct.Focus();
        }

        private void ClearFieldByProduct()
        {
            txtQty.Text = string.Empty;
            cmbProduct.SelectedValueChanged -= new EventHandler(cmbProduct_SelectedValueChanged);
            cmbProduct.SelectedIndex = -1;
            cmbProduct.SelectedValueChanged += new EventHandler(cmbProduct_SelectedValueChanged);
            txtKg.Text = string.Empty;
            txtWeight.Text = string.Empty;
            txtPrice.TextChanged -= new EventHandler(txtPrice_TextChanged);
            txtPrice.Text = string.Empty;
            txtPrice.TextChanged += new EventHandler(txtPrice_TextChanged);
            lblTotalAmount.Text = "0";
        }

        private void btnDeleteProduct_Click(object sender, EventArgs e)
        {
            DialogResult dg = MessageBox.Show("Are you sure you wana delete this Product ?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dg == DialogResult.Yes)
            {
                using (SqlConnection conn = new SqlConnection(clsConnectionString.cs))
                {
                    if (dataGridView2.SelectedRows.Count == 0)
                    {
                        MessageBox.Show("Please Select any Product");
                        return;
                    }
                    int rowindex = dataGridView2.CurrentCell.RowIndex;

                    string query = string.Format("delete from tblProductTransactionReturn where ReturnId = {0} and ProductName ='{1}' and TransactionType='{2}'", int.Parse(txtID.Text), dataGridView2.Rows[rowindex].Cells[1].Value.ToString(), "PurchaseReturn");
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        if (conn.State != ConnectionState.Open)
                        {
                            conn.Open();
                        }
                        int status = cmd.ExecuteNonQuery();
                        if (status > 0)
                        {
                            BlStock.ProductTransId = int.Parse(txtID.Text);
                            BlStock.ProductId = (int)dataGridView2.Rows[rowindex].Cells["ProductId"].Value;
                            BlStock.TransactionType = "Sale Invoice";
                            DlStock.Delete(BlStock);
                            MessageBox.Show("Your data has been perminently deleted...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            FillDataFieldInPurchase(int.Parse(txtID.Text));
                            GetTotal();

                            using (SqlConnection conn1 = new SqlConnection(clsConnectionString.cs))
                            {
                                string query1 = string.Format("update tblPurchaseR set Amount ={0} where ReturnId ={1}", int.Parse(txtAmount.Text), int.Parse(txtID.Text));


                                using (SqlCommand cmd1 = new SqlCommand(query, conn))
                                {
                                    conn1.Open();
                                    cmd1.ExecuteNonQuery();
                                    cmbProduct.Focus();
                                }
                                conn.Close();
                            }
                            FillGrid();


                        }
                    }
                }
            }
        }

        private void btnSaleR_Click(object sender, EventArgs e)
        {

            if (IsValidated())
            {
                BlPurchaseR.ReturnId = int.Parse(txtID.Text);
                BlPurchaseR.VendorId = (int)cmbSelectVendor.SelectedValue;
                BlPurchaseR.ReturnDate = dtpSaleR.Value;
                BlPurchaseR.Remark = txtRemark.Text;
                BlPurchaseR.Amount = int.Parse(txtAmount.Text);
                BlPurchaseR.ZoneId = (int)cmbZone.SelectedValue;
                BlPurchaseR.AreaId = (int)cmbAddress.SelectedValue;
                BlPurchaseR.IsLedger = false;
                if (DlPurcharR.InsertReturn(BlPurchaseR) > 0)
                {

                    MessageBox.Show("Your Purchase Return has been created..");
                    int NextId = int.Parse(txtID.Text);
                    NextId++;


                    //_______ SAQIB  1/1/2024 ___

                    //_______ disable Header ________
                    txtID.Enabled = false;
                    dtpSaleR.Enabled = false;
                    cmbZone.Enabled = false;
                    cmbAddress.Enabled = false;
                    cmbSelectVendor.Enabled = false;
                    txtRemark.Enabled = false;
                    btnSaleR.Enabled = false;
                    txtID.Enabled = false;

                    //_______ Enable Details ________
                    cmbProduct.Enabled = true;
                    txtKg.Enabled = true;
                    txtQty.Enabled = true;
                    txtKg.Enabled = true;
                    txtWeight.Enabled = true;
                    txtPrice.Enabled = true;
                    cmbLocation.Enabled = true;
                    btnAddProduct.Enabled = true;
                    btnDeleteProduct.Enabled = true;
                    btnSaleR.Enabled = false;
                    cmbProduct.Focus();
                }

            }


        }

        private void btnViewReturn_Click(object sender, EventArgs e)
        {
            frmViewPurchaseReturn vpr = new frmViewPurchaseReturn();
            vpr.ShowDialog();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            int NewId = 0;

            NewId = DlPurcharR.NewId();
            txtID.Text = NewId.ToString();

            //__________ SAQIB  1/1/2024 _____
            if (NewId > 0)
            {
                //1. Enable History
                txtID.Enabled = true;
                dtpSaleR.Enabled = true;
                cmbZone.Enabled = true;
                cmbAddress.Enabled = true;
                cmbSelectVendor.Enabled = true;
                txtRemark.Enabled = true;
                btnSaleR.Enabled = true;

                //2. Reset Data
                cmbZone.SelectedValue = 0;
                cmbAddress.SelectedValue = 0;
                cmbSelectVendor.SelectedValue = 0;
                dtpSaleR.Value = DateTime.Now;
                txtRemark.Text = "";

                //3. Disable Pagination
                btnFirst.Enabled = false;
                btnLast.Enabled = false;
                btnPrevious.Enabled = false;
                btnNext.Enabled = false;

                //btnViewReturn.Enabled = true;
                txtID.Text = NewId.ToString();
                dtpSaleR.Focus();
                btnPost.Enabled = true;
            }
            dataGridView2.DataSource = null;
            dataGridView1.DataSource = null;
        }

        private void btnPost_Click(object sender, EventArgs e)
        {
            if (dataGridView2.Rows.Count > 0)
            {
                //BlTransaction.TransactionDate = dtpSaleR.Value;
                //BlTransaction.TransactionType = "Purchase R";
                //BlTransaction.TransactionId = int.Parse(txtID.Text);
                //BlTransaction.CustomerId = (int)cmbSelectVendor.SelectedValue;
                //BlTransaction.Remark = txtRemark.Text;
                //BlTransaction.Dr = int.Parse(txtAmount.Text);
                //BlTransaction.Cr = 0;
                //BlTransaction.PostingDate = DateTime.Now;
                //BlTransaction.TranTypeUrdu = "پرچیس ریٹن";
                //if (DlTransaction.Insert(BlTransaction) > 0)
                //{

                DateTime TransactionDate = dtpSaleR.Value;
                string TransactionType = " Purchase Return";
                int TransactionId = int.Parse(txtAmount.Text);
                int VendorId = (int)cmbSelectVendor.SelectedValue;
                string Remark = txtRemark.Text;
                int Dr = 0;
                int Cr = int.Parse(txtAmount.Text);
                DateTime PostingDate = DateTime.Now;
                //string TranTypeUrdu = "خرید";
                //if (DlTransaction.Insert(BlTransaction) > 0)
                //{
                //    MessageBox.Show("Your ledger has been done...");
                //    lblIsPost.Text = "Invoice";

                //}
                int HowManyInsert = 0;
                using (SqlConnection conn = new SqlConnection(clsConnectionString.cs))
                {
                    string q = string.Format("insert into tblTransactionVendor values ('{0}','{1}',{2},{3},'{4}',{5},{6},'{7}')", TransactionDate.ToString("yyyy-MM-dd"), TransactionType, TransactionId, VendorId, Remark, Dr, Cr, PostingDate.ToString("yyyy-MM-dd"));
                    using (SqlCommand cmd1 = new SqlCommand(q, conn))
                    {

                        if (conn.State != ConnectionState.Open)
                        {

                            conn.Open();
                        }
                        HowManyInsert = cmd1.ExecuteNonQuery();

                    }
                }


                //Dr Entry
                blCoaLedger.TransactionDate = dtpSaleR.Value;
                blCoaLedger.TransactionType = "Create Purchase Return";
                blCoaLedger.TransactionId = int.Parse(txtID.Text);
                blCoaLedger.ChartOfAccountId = DlVendor.VendorAccountId((int)cmbSelectVendor.SelectedValue);
                blCoaLedger.Remark = txtRemark.Text;
                blCoaLedger.Dr = int.Parse(txtAmount.Text);
                blCoaLedger.Cr = 0;
                dlCoaLedger.Insert(blCoaLedger);
                //Cr Entry
                blCoaLedger.TransactionDate = dtpSaleR.Value;
                blCoaLedger.TransactionType = "Create Purchase Invoice";
                blCoaLedger.TransactionId = int.Parse(txtID.Text);
                // Chart of Account Credit Sale Id
                blCoaLedger.ChartOfAccountId = 3040;
                blCoaLedger.Remark = txtRemark.Text;
                blCoaLedger.Dr = 0;
                blCoaLedger.Cr = int.Parse(txtAmount.Text);
                dlCoaLedger.Insert(blCoaLedger);


                MessageBox.Show("Entry has been Post...");

                using (SqlConnection conn = new SqlConnection(clsConnectionString.cs))
                {
                    string query = string.Format("update tblPurchaseR set IsLedger =1 where ReturnId ={0}", txtID.Text);
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        cmbProduct.Focus();
                    }
                    conn.Close();
                }
                btnPost.Enabled = false;
                btnFirst.Enabled = true;
                btnNext.Enabled = true;
                btnPrevious.Enabled = true;
                btnLast.Enabled = true;
                btnAddProduct.Enabled = false;
                btnDeleteProduct.Enabled = false;
                cmbProduct.Enabled = false;
                txtKg.Enabled = false;
                txtQty.Enabled = false;
                txtWeight.Enabled = false;
                txtPrice.Enabled = false;
                cmbLocation.Enabled = false;

                //}

            }
            else
            {
                MessageBox.Show("Please add some product...");
            }


        }

        private void btnFirst_Click(object sender, EventArgs e)
        {


            FillDataFieldInPurchase(int.Parse(lblFirstId.Text));


            //GetTotal();
            lblCurrentId.Text = lblFirstId.Text;
            FillGrid();
        }

        private void btnLast_Click(object sender, EventArgs e)
        {

            FillDataFieldInPurchase(int.Parse(lblLastId.Text));

            lblCurrentId.Text = lblLastId.Text;
            FillGrid();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (int.Parse(lblCurrentId.Text) < int.Parse(lblLastId.Text))
            {
                int NextId = int.Parse(lblCurrentId.Text);
                NextId++;
                lblCurrentId.Text = NextId.ToString();

                FillDataFieldInPurchase(int.Parse(lblCurrentId.Text));

                //GetTotal();
                FillGrid();
            }
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            if (int.Parse(lblCurrentId.Text) > int.Parse(lblFirstId.Text))
            {
                int PreviousId = int.Parse(lblCurrentId.Text);
                PreviousId--;
                lblCurrentId.Text = PreviousId.ToString();


                FillDataFieldInPurchase(int.Parse(lblCurrentId.Text));

                FillGrid();
            }
        }

        private void dtpSaleR_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.SelectNextControl((Control)sender, true, true, true, true);
                e.SuppressKeyPress = true;
            }
        }


    }
}

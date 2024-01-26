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
using AnajPos.BAL;
using AnajPos.DAL;
using System.Data.SqlClient;
using AnajPos.DalVendor;
using AnajPos.BALVendor;
using AnajPos.GUI_Vendor;
using AnajPos.VendorReporting;
using AnajPos.BAL_Cash_BANK;
using AnajPos.DAL_Cash_Bank;

namespace AnajPos.GUI
{
    public partial class frmSaleReturn : frmTemplete
    {

        public frmSaleReturn()
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
      
        clsBlReturnProcduct BlReturnProduct = new clsBlReturnProcduct();
        clsWarehouse DlWarehoues = new clsWarehouse();
        clsStock DlStock = new clsStock();
        clsBlStock BlStock = new clsBlStock();

        clsBLChartOfAccountLedger blCoaLedger = new clsBLChartOfAccountLedger();
        clsChartOfAccountLedger dlCoaLedger = new clsChartOfAccountLedger();



        private void frmSaleReturn_Load(object sender, EventArgs e)
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

            lblVenderCustomer.Text = "Customer";
            FillDataFieldInSale(int.Parse(lblFirstId.Text));



            FillGrid();
            lblCurrentId.Text = txtID.Text;
            btnNew.Focus();

        }

        private void cmbZone_SelectedIndexChanged(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void cmbZone_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cmbZone.SelectedIndex != -1)
            {
                BlAddress.AddressId = (int)cmbZone.SelectedValue;
                cmbAddress.DataSource = DlAddress.ViewAddressByZone(BlAddress);
                cmbAddress.DisplayMember = "AddressName";
                cmbAddress.ValueMember = "AddressID";
                //    cmbAddress.SelectedIndex = -1;

            }
        }

        private void cmbAddress_SelectedValueChanged(object sender, EventArgs e)
        {

            try
            {
                BlCustomer.AddressId = Convert.ToInt32(cmbAddress.SelectedValue);
                DataTable dt = DlCustomer.ViewByAddress(BlCustomer);
                cmbSelectCustomer.DataSource = dt;
                cmbSelectCustomer.DisplayMember = "CustomerName";
                cmbSelectCustomer.ValueMember = "CustomerId";
                //  cmbCustomer.SelectedIndex = -1;
            }
            catch
            { }


        }


        private void btnLoad_Click(object sender, EventArgs e)
        {

        }

        private void btnSaleR_Click(object sender, EventArgs e)
        {

            if (IsValidated())
            {
                BlSaleReturn.ReturnId = int.Parse(txtID.Text);
                BlSaleReturn.ZoneId = (int)cmbZone.SelectedValue;
                BlSaleReturn.AddressId = (int)cmbAddress.SelectedValue;
                BlSaleReturn.CustomerId = (int)cmbSelectCustomer.SelectedValue;
                BlSaleReturn.ReturnDate = dtpSaleR.Value;
                BlSaleReturn.Remark = txtRemark.Text;
                BlSaleReturn.Amount = Convert.ToInt32(txtAmount.Text);
                if (DlSaleReturn.InsertReturn(BlSaleReturn) > 0)
                {
                    MessageBox.Show("Your Sale Return has been created..");

                    //BlTransaction.TransactionDate = dtpSaleR.Value;
                    //BlTransaction.TransactionType = "Sale R";
                    //BlTransaction.TransactionId = int.Parse(txtID.Text);
                    //BlTransaction.CustomerId = (int)cmbSelectCustomer.SelectedValue;
                    //BlTransaction.Remark = txtRemark.Text;
                    //BlTransaction.Dr = int.Parse(txtAmount.Text);
                    //BlTransaction.Cr = 0;
                    //BlTransaction.PostingDate = DateTime.Now;
                    //BlTransaction.TranTypeUrdu = "سیل ریٹن";
                    //if (DlTransaction.Insert(BlTransaction) > 0)
                    //{

                    //}

                    //_______ SAQIB  1/1/2024 ___

                    //_______ disable Header ________
                    txtID.Enabled = false;
                    dtpSaleR.Enabled = false;
                    cmbZone.Enabled = false;
                    cmbAddress.Enabled = false;
                    cmbSelectCustomer.Enabled = false;
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
        private bool IsValidated()
        {
            if (txtID.Text == "" || txtID.Text == null)
            {
                MessageBox.Show("Please Create History");
                return false;
            }
            else
            {
                if (cmbSelectCustomer.SelectedIndex == -1)
                {
                    MessageBox.Show("Please Select the Customer");
                    return false;
                }
                else if (txtAmount.Text == string.Empty)
                {
                    MessageBox.Show("Please Enter Amount");
                    return false;
                }
                else if (txtRemark.Text == string.Empty)
                {
                    MessageBox.Show("Please Enter Remark");
                    return false;
                }
            }
            return true;
        }

        private void cmbProduct_SelectedIndexChanged(object sender, EventArgs e)
        {


        }

        //private void cmbProduct_SelectedValueChanged(object sender, EventArgs e)
        //{



        //}

        private void viewAllReturnsToolStripMenuItem_Click(object sender, EventArgs e)
        {

            frmViewSaleReturn sr = new frmViewSaleReturn();
            sr.ShowDialog();

        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            int NewId = 0;

            NewId = DlSaleReturn.NewId();

            //__________ SAQIB  1/1/2024 _____
            if (NewId > 0)
            {
                //1. Enable History
                txtID.Enabled = true;
                dtpSaleR.Enabled = true;
                cmbZone.Enabled = true;
                cmbAddress.Enabled = true;
                cmbSelectCustomer.Enabled = true;
                txtRemark.Enabled = true;
                btnSaleR.Enabled = true;

                //2. Reset Data
                cmbZone.SelectedValue = 0;
                cmbAddress.SelectedValue = 0;
                cmbSelectCustomer.SelectedValue = 0;
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

        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            if (txtQty.Text == string.Empty && txtPrice.Text == string.Empty && cmbProduct.SelectedIndex != -1)
            {
                MessageBox.Show("Please Enter Proper Data...");
                return;
            }


            if (IsProductAlreadyExists(cmbProduct.Text))
            {


                BlReturnProduct.ProductId = (int)cmbProduct.SelectedValue;
                BlReturnProduct.ReturnId = int.Parse(txtID.Text);
                BlReturnProduct.ReturnDate = dtpSaleR.Value;
                BlReturnProduct.CustomerVendorAccountId = (int)cmbSelectCustomer.SelectedValue;
                BlReturnProduct.Qty = int.Parse(txtQty.Text);
                BlReturnProduct.ProductName = cmbProduct.Text;
                BlReturnProduct.Kg = float.Parse(txtKg.Text);
                BlReturnProduct.Weight = float.Parse(txtWeight.Text);
                BlReturnProduct.Price = decimal.Parse(txtPrice.Text);
                BlReturnProduct.Amount = decimal.Parse(lblTotalAmount.Text);
                BlReturnProduct.Type = "SaleReturn";
                if (DlSaleReturn.InsertReturnProduct(BlReturnProduct) > 0)
                {
                    //BlStock.ProductTransDate = dtpSaleR.Value;
                    //BlStock.ProductTransId = int.Parse(txtID.Text);
                    //BlStock.TransactionType = "SaleReturn";
                    //BlStock.ProductId = (int)cmbProduct.SelectedValue;
                    //BlStock.UnitID = 1;
                    //BlStock.WarehouseId = (int)cmbLocation.SelectedValue;

                    //BlStock.StockIn = 0;
                    //BlStock.StockOut = int.Parse(txtQty.Text);



                    //BlStock.Amount = int.Parse(txtPrice.Text);
                    //BlStock.IsDeleted = false;
                    //if (DlStock.Insert(BlStock) > 0)
                    //{

                    FillGrid();
                    ClearFieldByProduct();
                    GetTotal();


                    using (SqlConnection conn = new SqlConnection(clsConnectionString.cs))
                    {
                        string query = string.Format("update tblSaleR set Amount ={0} where ReturnId ={1}", int.Parse(txtAmount.Text), int.Parse(txtID.Text));


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

        public void FillGrid()
        {
            dataGridView2.DataSource = null;
            if (txtID.Text != string.Empty)
            {
                dataGridView2.DataSource = DlSaleReturn.GetReturndetail(int.Parse(txtID.Text), "SaleReturn");
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

        private void txtKg_TextChanged(object sender, EventArgs e)
        {
            if (txtQty.Text != string.Empty && txtKg.Enabled == true && txtKg.Text != string.Empty)
            {
                txtWeight.Text = Convert.ToString(Convert.ToDouble(txtQty.Text) * Convert.ToDouble(txtKg.Text));
            }
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

        private void cmbProduct_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cmbSelectCustomer.SelectedIndex == -1)
            {
                MessageBox.Show("Please select Account");
                return;
            }

            if (cmbProduct.SelectedValue != null)
            {
                using (SqlConnection conn = new SqlConnection(clsConnectionString.cs))
                {
                    string q = string.Format("select InvoiceDate,InvoiceId,Qty,ProductName,Kg,Price,Weight,Amount from tblProductTransaction where CustomerId ={0} and ProductId ={1}", (int)cmbSelectCustomer.SelectedValue, (int)cmbProduct.SelectedValue);
                    SqlDataAdapter adpt = new SqlDataAdapter(q, conn);
                    DataTable dt1 = new DataTable();
                    adpt.Fill(dt1);
                    dataGridView1.DataSource = dt1;
                }
            }



            DataTable dt = GetProductDetail((int)cmbSelectCustomer.SelectedValue, (int)cmbProduct.SelectedValue);
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



        private void txtWeight_TextChanged(object sender, EventArgs e)
        {
            txtQty_TextChanged(txtQty, EventArgs.Empty);
            txtPrice_TextChanged(txtPrice, EventArgs.Empty);

        }

        private void btnViewReturn_Click(object sender, EventArgs e)
        {
            frmViewSaleReturn sr = new frmViewSaleReturn();
            sr.ShowDialog();

        }

        /// <summary>
        /// __________________ Pagination _______________ 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region Pagination
        private int MinId()
        {
            int status = 0;
            using (SqlConnection conn = new SqlConnection(clsConnectionString.cs))
            {
                string Query = "select min([ReturnId]) from tblSaleR";

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
        private int MaxId()
        {
            int status = 0;
            using (SqlConnection conn = new SqlConnection(clsConnectionString.cs))
            {
                string Query = "select max([ReturnId]) from tblSaleR";

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

        //_____________________ Next _____________________
        private void btnNext_Click(object sender, EventArgs e)
        {
            if (int.Parse(lblCurrentId.Text) < int.Parse(lblLastId.Text))
            {
                int NextId = int.Parse(lblCurrentId.Text);
                NextId++;
                lblCurrentId.Text = NextId.ToString();

                FillDataFieldInSale(int.Parse(lblCurrentId.Text));


                //GetTotal();
                FillGrid();
            }
        }

        //_____________________ Previous _____________________
        private void btnPrevious_Click(object sender, EventArgs e)
        {
            if (int.Parse(lblCurrentId.Text) > int.Parse(lblFirstId.Text))
            {
                int PreviousId = int.Parse(lblCurrentId.Text);
                PreviousId--;
                lblCurrentId.Text = PreviousId.ToString();

                FillDataFieldInSale(int.Parse(lblCurrentId.Text));


                FillGrid();
            }
        }

        //_____________________ Last _____________________
        private void btnLast_Click(object sender, EventArgs e)
        {

            FillDataFieldInSale(int.Parse(lblLastId.Text));


            lblCurrentId.Text = lblLastId.Text;
            FillGrid();
        }

        //_____________________ First _____________________
        private void btnFirst_Click(object sender, EventArgs e)
        {

            FillDataFieldInSale(int.Parse(lblFirstId.Text));



            //GetTotal();
            lblCurrentId.Text = lblFirstId.Text;
            FillGrid();
        }

        private void FillDataFieldInSale(int Id)
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionString.cs))
            {
                string q = string.Format("select ReturnId,Zoneid,Addressid,CustomerId,ReturnDate,Remark,Amount,isnull(IsLedger,0) IsLedger from tblSaleR Where [returnId] ={0}", Id);
                SqlDataAdapter adpt = new SqlDataAdapter(q, conn);
                DataTable dt = new DataTable();
                adpt.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    txtID.Text = dr[0].ToString();
                    cmbZone.SelectedValue = (int)dr[1];
                    cmbAddress.SelectedValue = (int)dr[2];
                    cmbSelectCustomer.SelectedValue = (int)dr[3];
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

                    }
                }
            }
        }



        #endregion

        private void btnPost_Click(object sender, EventArgs e)
        {
            if (dataGridView2.Rows.Count > 0)
            {


                BlTransaction.TransactionDate = dtpSaleR.Value;
                BlTransaction.TransactionType = "Sale R";
                BlTransaction.TransactionId = int.Parse(txtID.Text);
                BlTransaction.CustomerId = (int)cmbSelectCustomer.SelectedValue;
                BlTransaction.Remark = txtRemark.Text;
                BlTransaction.Dr = int.Parse(txtAmount.Text);
                BlTransaction.Cr = 0;
                BlTransaction.PostingDate = DateTime.Now;
                BlTransaction.TranTypeUrdu = "سیل ریٹن";
                if (DlTransaction.Insert(BlTransaction) > 0)
                {
                    //Dr Entry
                    blCoaLedger.TransactionDate = dtpSaleR.Value;
                    blCoaLedger.TransactionType = "Create Sale Invoice";
                    blCoaLedger.TransactionId = int.Parse(txtID.Text);
                    blCoaLedger.ChartOfAccountId = DlCustomer.CustomerAccountId((int)cmbSelectCustomer.SelectedValue);
                    blCoaLedger.Remark = txtRemark.Text;
                    blCoaLedger.Dr = 0;
                    blCoaLedger.Cr = int.Parse(txtAmount.Text);
                    dlCoaLedger.Insert(blCoaLedger);
                    //Cr Entry
                    blCoaLedger.TransactionDate = dtpSaleR.Value;
                    blCoaLedger.TransactionType = "Create Sale Invoice";
                    blCoaLedger.TransactionId = int.Parse(txtID.Text);
                    // Chart of Account Credit Sale Id
                    blCoaLedger.ChartOfAccountId = 3040;
                    blCoaLedger.Remark = txtRemark.Text;
                    blCoaLedger.Dr = int.Parse(txtAmount.Text);
                    blCoaLedger.Cr = 0;
                    dlCoaLedger.Insert(blCoaLedger);


                    MessageBox.Show("Entry has been Post...");

                    using (SqlConnection conn = new SqlConnection(clsConnectionString.cs))
                    {
                        string query = string.Format("update tblSaleR set IsLedger =1 where ReturnId ={0}", txtID.Text);
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


                    btnAddProduct.Enabled = false;
                    btnDeleteProduct.Enabled = false;
                    cmbProduct.Enabled = false;
                    txtKg.Enabled = false;
                    txtQty.Enabled = false;
                    txtWeight.Enabled = false;
                    txtPrice.Enabled = false;
                    cmbLocation.Enabled = false;


                }


            }
            else
            {
                MessageBox.Show("Please add some product...");
            }


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

                    var returnId = int.Parse(txtID.Text);
                    var productName = dataGridView2.Rows[rowindex].Cells["ProductName"].Value.ToString();
                    var TransactionId = Convert.ToInt32(dataGridView2.Rows[rowindex].Cells["TransactionId"].Value);
                    string query = string.Format("delete from tblProductTransactionReturn where TransactionId = {0} and ReturnId = {1} and ProductName ='{2}' and TransactionType='{3}'", TransactionId, returnId, productName, "SaleReturn");
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
                            FillDataFieldInSale(int.Parse(txtID.Text));
                            GetTotal();
                            FillGrid();
                        }
                    }
                }
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

        private void customerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmViewCustomer vc = new frmViewCustomer();
            vc.ShowDialog();
        }

        private void vendorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmViewVendor vv = new frmViewVendor();
            vv.ShowDialog();

        }
    }
}

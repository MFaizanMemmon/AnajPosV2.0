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
using System.Data.SqlClient;
using AnajPos.BAL_Cash_BANK;
using AnajPos.DAL_Cash_Bank;

namespace AnajPos.GUI
{
    public partial class frmInvoice : frmTemplete
    {
        clsZone cz = new clsZone();
        clsCustomer DlCustomer = new clsCustomer();
        clsBlCustomer BlCustomer = new clsBlCustomer();
        clsAddress DlAddress = new clsAddress();
        clsBlAddress BlAddress = new clsBlAddress();
        clsTransaction DlTransaction = new clsTransaction();
        clsBlTransaction BlTransaction = new clsBlTransaction();
        clsHeadOfAcc DlHead = new clsHeadOfAcc();
        clsReceipt DlReceipt = new clsReceipt();
        clsBlReceipt BlReceipt = new clsBlReceipt();
        clsProduct DlProduct = new clsProduct();
        clsBlOrder BlOrder = new clsBlOrder();
        clsBlStock BlStock = new clsBlStock();
        clsStock DlStock = new clsStock();
        clsWarehouse DlWarehoues = new clsWarehouse();
        clsBLChartOfAccountLedger blCoaLedger = new clsBLChartOfAccountLedger();
        clsChartOfAccountLedger dlCoaLedger = new clsChartOfAccountLedger();

        public int ProductId { get; set; }

        public frmInvoice()
        {
            InitializeComponent();
        }

        private void frmInvoice_Load(object sender, EventArgs e)
        {
            cmbZone.SelectedValueChanged -= new EventHandler(cmbZone_SelectedValueChanged);
            DataTable dtView = cz.ViewZone();
            cmbZone.DataSource = dtView;
            cmbZone.ValueMember = "Id";
            cmbZone.DisplayMember = "Name of Zone";
            cmbZone.SelectedIndex = -1;
            cmbCustomer.Text = string.Empty;
            cmbZone.SelectedValueChanged += new EventHandler(cmbZone_SelectedValueChanged);

            cmbProduct.SelectedValueChanged -= new EventHandler(cmbProduct_SelectedValueChanged);
            cmbProduct.DataSource = DlProduct.GetAllProduct();
            cmbProduct.DisplayMember = "ProductName";
            cmbProduct.ValueMember = "ProductId";
            cmbProduct.SelectedValueChanged += new EventHandler(cmbProduct_SelectedValueChanged);


            cmbLocation.SelectedValueChanged -= new EventHandler(cmbLocation_SelectedValueChanged);
            DataTable dtStockLcation = DlWarehoues.View();
            cmbLocation.DataSource = dtStockLcation;
            cmbLocation.ValueMember = "WarehouseId";
            cmbLocation.DisplayMember = "WarehouseName";
            cmbLocation.SelectedIndex = -1;
            cmbLocation.SelectedValueChanged += new EventHandler(cmbLocation_SelectedValueChanged);

            lblFirstId.Text = MinId().ToString();
            lblLastId.Text = MaxId().ToString();
            FillDataFieldInOrder(int.Parse(lblFirstId.Text));
            FillDataFieldInProduct(int.Parse(lblFirstId.Text));
            lblCurrentId.Text = txtOrderID.Text;
          
            GetTotal();
        }

        private void FillDataFieldInOrder(int Id)
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionString.cs))
            {
                string q = string.Format("select * from tblInvoice Where [Sid] ={0}", Id);
                SqlDataAdapter adpt = new SqlDataAdapter(q, conn);
                DataTable dt = new DataTable();
                adpt.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    txtOrderID.Text = dr[0].ToString();
                    dtpOrderDate.Value = (DateTime)dr[1];
                    cmbZone.SelectedValue = (int)dr[2];
                    cmbAddress.SelectedValue = (int)dr[3];
                    cmbCustomer.SelectedValue = (int)dr[4];
                    txtPageNo.Text = dr[5].ToString();
                    txtBilty.Text = dr[6].ToString();
                    txtRemark.Text = dr[8].ToString();
                    cmbTransport.Text = dr[7].ToString();
                    lblPreviousAmount.Text = dr[12].ToString();
                    lblIsPost.Text = dr[14].ToString();
                    if (lblIsPost.Text == "Invoice")
                    {
                        txtGrandTotal.Text = dr[10].ToString();
                        txtRent.TextChanged -= new EventHandler(txtRent_TextChanged);
                        txtRent.Text = dr[15].ToString();
                        txtRent.TextChanged += new EventHandler(txtRent_TextChanged);
                        txtPerBag.TextChanged -= new EventHandler(txtPerBag_TextChanged);
                        txtPerBag.Text = dr[16].ToString();
                        txtPerBag.TextChanged += new EventHandler(txtPerBag_TextChanged);
                        txtLabourCharges.TextChanged -= new EventHandler(txtLabourCharges_TextChanged);
                        txtLabourCharges.Text = dr[17].ToString();
                        txtLabourCharges.TextChanged += new EventHandler(txtLabourCharges_TextChanged);

                    }
                    else
                    {
                        txtRent.Text = string.Empty;
                        txtPerBag.Text = string.Empty;
                        txtLabourCharges.Text = string.Empty;
                        txtGrandTotal.Text = string.Empty;
                    }
                }
            }

        }
        private void FillDataFieldInProduct(int Id)
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionString.cs))
            {
                string q = string.Format("select t.Qty,t.ProductName as Product, w.WarehouseName as 'Location',t.Kg,t.[Weight],t.Price,t.Amount,t.ProductId from tblProductTransaction t inner join tblStock s on s.ProductTransId = t.InvoiceId and TransactionType = 'Sale Invoice' and s.IsDeleted = 0 inner join tblWarehouse w on w.WarehouseId = s.WarehouseID where InvoiceId = {0}", Id);
                SqlDataAdapter adpt = new SqlDataAdapter(q, conn);
                DataTable dt = new DataTable();
                adpt.Fill(dt);
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = dt;
                dataGridView1.Columns["ProductId"].Visible = false;
            }
        }


        private void cmbZone_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cmbZone.SelectedIndex != -1)
            {
                BlAddress.AddressId = (int)cmbZone.SelectedValue;
                cmbAddress.DataSource = DlAddress.ViewAddressByZone(BlAddress);
                cmbAddress.DisplayMember = "AddressName";
                cmbAddress.ValueMember = "AddressID";
                cmbAddress.SelectedIndex = -1;
                cmbCustomer.Text = string.Empty;
            }

        }
        private int MinId()
        {
            int status = 0;
            using (SqlConnection conn = new SqlConnection(clsConnectionString.cs))
            {
                using (SqlCommand cmd = new SqlCommand("select min([Sid]) from tblInvoice", conn))
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
                using (SqlCommand cmd = new SqlCommand("select max([Sid]) from tblInvoice", conn))
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

        private void GetTotal()
        {
            int FinalCost = 0;
            int Qty = 0;
            int Count = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                FinalCost = FinalCost + Convert.ToInt32(dataGridView1.Rows[i].Cells[5].Value);
                Qty = Qty + Convert.ToInt32(dataGridView1.Rows[i].Cells[0].Value);
                Count++;
            }
            lblCount.Text = Count.ToString();
            lblQty.Text = Qty.ToString();
            txtTotal.Text = FinalCost.ToString();
        }

        private void cmbAddress_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                BlCustomer.AddressId = Convert.ToInt32(cmbAddress.SelectedValue);
                DataTable dt = DlCustomer.ViewByAddress(BlCustomer);
                cmbCustomer.DataSource = dt;
                cmbCustomer.DisplayMember = "CustomerName";
                cmbCustomer.ValueMember = "CustomerId";
                //  cmbCustomer.SelectedIndex = -1;
            }
            catch
            { }

        }

        private void cmbCustomer_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbCustomer.SelectedValue == null)
                {
                    return;
                }
                lblPreviousAmount.Text = DlCustomer.CustomerLastBalance((int)cmbCustomer.SelectedValue).ToString();
                GetTransport();

            }
            catch
            {


            }

        }
        private void GetTransport()
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionString.cs))
            {
                string q = string.Format("select * from tblInvoice where CustomerId={0}", (int)cmbCustomer.SelectedValue);
                using (SqlCommand cmd = new SqlCommand(q, conn))
                {
                    if (conn.State != ConnectionState.Open)
                    {
                        conn.Open();
                    }
                    SqlDataReader sdr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(sdr);
                    if (dt.Rows.Count > 0)
                    {
                        cmbTransport.Text = dt.Rows[dt.Rows.Count - 1][7].ToString();
                    }
                    else
                    {
                        cmbTransport.Text = string.Empty;
                    }

                }
            }
        }

        private void btnGenrateOrder_Click(object sender, EventArgs e)
        {
            DialogResult dg = MessageBox.Show("Are your sure you wana genrate this Order.. ?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dg == DialogResult.Yes)
            {

                if (IsValidation())
                {
                    if (btnGenrateOrder.Text == "Order")
                    {
                        using (SqlConnection conn = new SqlConnection(clsConnectionString.cs))
                        {
                            //                                                       id  date zone areacus  pa  bil     tran    rem   ord   gt   pd    pa   na    pt     r   pb   lc   
                            string q = string.Format("insert into tblInvoice values({0},'{1}',{2},{3},{4},'{5}','{6}',N'{7}',N'{8}','{9}',{10},'{11}',{12},{13},'{14}',{15},{16},{17},{18})", int.Parse(txtOrderID.Text), dtpOrderDate.Value.ToString("yyyy-MM-dd"), (int)cmbZone.SelectedValue, (int)cmbAddress.SelectedValue, (int)cmbCustomer.SelectedValue, txtPageNo.Text, txtBilty.Text, cmbTransport.Text, txtRemark.Text, DateTime.Now.ToString("yyyy-MM-dd"), 0, DateTime.Now.ToString("yyyy-MM-dd"), lblPreviousAmount.Text, 0, "Order", 0, 0, 0, 0);
                            using (SqlCommand cmd = new SqlCommand(q, conn))
                            {
                                if (conn.State != ConnectionState.Open)
                                {
                                    conn.Open();
                                }
                                int status = cmd.ExecuteNonQuery();
                                if (status > 0)
                                {
                                    MessageBox.Show("Your data has been saved.");
                                    DisableField();
                                    lblLastId.Text = MaxId().ToString();
                                    lblCurrentId.Text = lblLastId.Text;
                                    btnGenrateOrder.Enabled = false;
                                    button1.Enabled = true;
                                    DisableField();
                                    btnNext.Enabled = true;
                                    btnPrevious.Enabled = true;
                                    btnFirst.Enabled = true;
                                    btnLast.Enabled = true;
                                    btnAddProduct.Enabled = true;
                                    btnDeleteProduct.Enabled = true;

                                    txtQty.Enabled = true;
                                    cmbProduct.Enabled = true;
                                    chkIsKg.Enabled = true;
                                    txtKg.Enabled = true;
                                    txtWeight.Enabled = true;
                                    txtPrice.Enabled = true;
                                    txtRent.Enabled = true;
                                    txtPerBag.Enabled = true;

                                    cmbProduct.Focus();

                                }

                                conn.Close();

                            }

                        }
                    }
                    else
                    {
                        using (SqlConnection conn = new SqlConnection(clsConnectionString.cs))
                        {
                            string query = string.Format("update tblInvoice set SDate = '{0}',ZoneId = {1},AreaId ={2},CustomerId = {3},PageNo = '{4}',Bilty ='{5}',Remark = N'{6}',Transport=N'{7}' where [Sid]= {8}", dtpOrderDate.Value.ToString("yyyy-MM-dd"), (int)cmbZone.SelectedValue, (int)cmbAddress.SelectedValue, (int)cmbCustomer.SelectedValue, txtPageNo.Text, txtBilty.Text, txtRemark.Text, cmbTransport.Text, int.Parse(txtOrderID.Text));

                            using (SqlCommand cmd = new SqlCommand(query, conn))
                            {
                                if (conn.State != ConnectionState.Open)
                                {
                                    conn.Open();
                                }
                                int status = cmd.ExecuteNonQuery();
                                if (status > 0)
                                {
                                    MessageBox.Show("Your data has been Update.");
                                    btnGenrateOrder.Text = "Get Order";
                                    btnGenrateOrder.Enabled = false;
                                    button1.Enabled = true;

                                    btnAddProduct.Enabled = true;
                                    btnDeleteProduct.Enabled = true;
                                    cmbCustomer.Enabled = false;
                                    cmbZone.Enabled = false;
                                    cmbAddress.Enabled = false;
                                    txtBilty.Enabled = false;
                                    txtRemark.Enabled = true;
                                    cmbTransport.Enabled = false;
                                    txtPageNo.Enabled = false;
                                    dtpOrderDate.Enabled = false;
                                    txtRemark.Enabled = false;
                                    txtQty.Focus();
                                    btnAddProduct.Enabled = true;
                                    btnDeleteProduct.Enabled = true;

                                    txtQty.Enabled = true;
                                    cmbProduct.Enabled = true;
                                    chkIsKg.Enabled = true;
                                    txtKg.Enabled = true;
                                    txtWeight.Enabled = true;
                                    txtPrice.Enabled = true;

                                    txtQty.Focus();
                                }

                            }

                        }
                    }
                }
            }

        }

        private void DisableField()
        {
            txtOrderID.Enabled = false;
            dtpOrderDate.Enabled = false;
            cmbZone.Enabled = false;
            cmbAddress.Enabled = false;
            cmbCustomer.Enabled = false;
            txtPageNo.Enabled = false;
            txtBilty.Enabled = false;
            txtRemark.Enabled = false;
            cmbTransport.Enabled = false;
        }
        private void EnableField()
        {
            txtOrderID.Enabled = true;
            dtpOrderDate.Enabled = true;
            cmbZone.Enabled = true;
            cmbAddress.Enabled = true;
            cmbCustomer.Enabled = true;
            txtPageNo.Enabled = true;
            txtBilty.Enabled = true;
            txtRemark.Enabled = true;
            cmbTransport.Enabled = true;


        }
        private bool IsValidation()
        {
            if (txtOrderID.Text.Trim() == string.Empty)
            {
                return false;
            }
            else if (cmbCustomer.SelectedIndex == -1)
            {

                return false;
            }
            else if (txtPageNo.Text == string.Empty)
            {
                MessageBox.Show("Please Enter Valid Page Number Your Register...");
                return false;
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
            lblTotalStock.Text = string.Empty;
            cmbLocation.SelectedValueChanged -= new EventHandler(cmbLocation_SelectedValueChanged);
            cmbLocation.SelectedIndex = -1;
            cmbLocation.SelectedValueChanged += new EventHandler(cmbLocation_SelectedValueChanged);
        }
        private void InsertProduct()
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionString.cs))
            {
                string q = string.Empty;
                if (chkIsKg.Checked)
                {
                    q = string.Format("insert into tblProductTransaction values ({0},'{1}',{2},{3},{4},'{5}',{6},{7},{8},{9})", int.Parse(txtOrderID.Text), dtpOrderDate.Value.ToString("yyyy-MM-dd"), (int)cmbCustomer.SelectedValue, (int)cmbProduct.SelectedValue, int.Parse(txtQty.Text), cmbProduct.Text, Convert.ToDouble(txtKg.Text), Convert.ToDouble(txtWeight.Text), Convert.ToDouble(txtPrice.Text), Convert.ToDouble(lblTotalAmount.Text));
                }
                else
                {
                    q = string.Format("insert into tblProductTransaction (InvoiceId,InvoiceDate,CustomerId,ProductId,Qty,ProductName,Price,Amount) values ({0},'{1}',{2},{3},{4},'{5}',{6},{7})", int.Parse(txtOrderID.Text), dtpOrderDate.Value.ToString("yyyy-MM-dddd"), (int)cmbCustomer.SelectedValue, (int)cmbProduct.SelectedValue, int.Parse(txtQty.Text), cmbProduct.Text, int.Parse(txtPrice.Text), int.Parse(lblTotalAmount.Text));
                }
                using (SqlCommand cmd = new SqlCommand(q, conn))
                {
                    if (conn.State != ConnectionState.Open)
                    {
                        conn.Open();
                    }
                    int status = cmd.ExecuteNonQuery();
                    if (status > 0)
                    {
                        BlStock.ProductTransDate = dtpOrderDate.Value;
                        BlStock.ProductTransId = int.Parse(txtOrderID.Text);
                        BlStock.TransactionType = "Sale Invoice";
                        BlStock.ProductId = (int)cmbProduct.SelectedValue;
                        BlStock.UnitID = 1;
                        BlStock.WarehouseId = (int)cmbLocation.SelectedValue;
                        BlStock.StockIn = 0;
                        BlStock.StockOut = int.Parse(txtQty.Text);
                        BlStock.Amount = int.Parse(txtPrice.Text);
                        BlStock.IsDeleted = false;
                        if (DlStock.Insert(BlStock) > 0)
                        {
                            MessageBox.Show("your data has been done...");
                            FillDataFieldInProduct(int.Parse(txtOrderID.Text));
                            GetTotal();
                        }

                    }
                    conn.Close();
                }
            }
        }

        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            if (txtQty.Text == string.Empty && txtPrice.Text == string.Empty && cmbProduct.SelectedIndex != -1 && cmbLocation.SelectedIndex != -1)
            {
                MessageBox.Show("Please Enter Proper Data...");
                return;
            }
            if (int.Parse(txtQty.Text) > int.Parse(lblTotalStock.Text))
            {
                MessageBox.Show("Quantity should be less then Stock...");
                return;
            }
            else if (int.Parse(txtPrice.Text) < 0)
            {
                MessageBox.Show("Quantity should be grater then 0");
                return;
            }
            if (IsProductAlreadyExists(cmbProduct.Text))
            {
                InsertProduct();
                ClearFieldByProduct();
                using (SqlConnection conn = new SqlConnection(clsConnectionString.cs))
                {
                    string query = string.Format("update tblInvoice set TotalAmount ={0} where Sid ={1}", int.Parse(txtTotal.Text), int.Parse(txtOrderID.Text));
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        cmbProduct.Focus();
                    }
                    conn.Close();
                }


            }
        }
        private bool IsProductAlreadyExists(string ValidText)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (ValidText == dataGridView1.Rows[i].Cells[1].Value.ToString())
                {
                    MessageBox.Show("this Product is Already exists in this Order Please Update this Product", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }
            return true;

        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            FillDataFieldInOrder(int.Parse(lblFirstId.Text));
            FillDataFieldInProduct(int.Parse(lblFirstId.Text));
            GetTotal();
            lblCurrentId.Text = lblFirstId.Text;
        }

        private void btnGenrateNew_Click(object sender, EventArgs e)
        {
            int NewId = int.Parse(lblLastId.Text);
            NewId++;
            txtOrderID.Text = NewId.ToString();
            dataGridView1.DataSource = null;
            EnableField();
            cmbZone.SelectedIndex = -1;
            cmbAddress.DataSource = null;
            cmbCustomer.DataSource = null;
            cmbCustomer.Text = string.Empty;
            txtPageNo.Text = string.Empty;
            txtRemark.Text = string.Empty;
            cmbTransport.Text = string.Empty;
            txtBilty.Text = string.Empty;
            lblPreviousAmount.Text = "0";
            txtTotal.Text = string.Empty;
            btnGenrateOrder.Enabled = true;
            //txtQty.Enabled = true;
            //cmbProduct.Enabled = true;
            //chkIsKg.Enabled = true;
            //txtKg.Enabled = true;
            //txtWeight.Enabled = true;
            //txtPrice.Enabled = true;
            //dataGridView1.Enabled = true;
            //lblTotalAmount.Enabled = true;
            //txtTotal.Enabled = true;
            //txtRent.Enabled = true;
            //txtLabourCharges.Enabled = true;
            //txtGrandTotal.Enabled = true;
            //btnAddProduct.Enabled = true;
            // btnUpdateProduct.Enabled = true;
            //btnDeleteProduct.Enabled = true;
            btnNext.Enabled = false;
            btnPrevious.Enabled = false;
            btnFirst.Enabled = false;
            btnLast.Enabled = false;
            button1.Enabled = false;
            lblIsPost.Text = "Order";
            txtGrandTotal.Text = string.Empty;
            dtpOrderDate.Value = DateTime.Now;
            dtpOrderDate.Focus();
            txtRent.TextChanged -= new EventHandler(txtRent_TextChanged);
            txtRent.Text = string.Empty;
            txtRent.TextChanged += new EventHandler(txtRent_TextChanged);
            txtLabourCharges.TextChanged -= new EventHandler(txtLabourCharges_TextChanged);
            txtLabourCharges.Text = string.Empty;
            txtLabourCharges.TextChanged += new EventHandler(txtLabourCharges_TextChanged);
            txtPerBag.TextChanged -= new EventHandler(txtPerBag_TextChanged);
            txtPerBag.Text = string.Empty;
            txtPerBag.TextChanged += new EventHandler(txtPerBag_TextChanged);
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            FillDataFieldInOrder(int.Parse(lblLastId.Text));
            FillDataFieldInProduct(int.Parse(lblLastId.Text));
            GetTotal();
            lblCurrentId.Text = lblLastId.Text;

        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (int.Parse(lblCurrentId.Text) < int.Parse(lblLastId.Text))
            {
                int NextId = int.Parse(lblCurrentId.Text);
                NextId++;
                lblCurrentId.Text = NextId.ToString();
                FillDataFieldInOrder(NextId);
                FillDataFieldInProduct(NextId);
                GetTotal();
            }

        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            if (int.Parse(lblCurrentId.Text) > int.Parse(lblFirstId.Text))
            {
                int PreviousId = int.Parse(lblCurrentId.Text);
                PreviousId--;
                lblCurrentId.Text = PreviousId.ToString();
                FillDataFieldInOrder(PreviousId);
                FillDataFieldInProduct(PreviousId);
                GetTotal();
            }

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
                    //MessageBox.Show("Plese Enter Weight");
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

        private void txtRent_TextChanged(object sender, EventArgs e)
        {
            if (txtRent.Text.Length > 9)
            {
                txtRent.Text = string.Empty;
            }
            int TotalAmount = int.Parse(txtTotal.Text);
            int RentAmount = 0;
            int LabourAmount = 0;
            if (txtRent.Text != string.Empty && txtRent.Text != "-")
            {
                RentAmount = int.Parse(txtRent.Text);
            }
            else
            {
                RentAmount = 0;
            }
            if (txtLabourCharges.Text != string.Empty)
            {
                LabourAmount = int.Parse(txtLabourCharges.Text);
            }
            else
            {
                LabourAmount = 0;
            }
            txtGrandTotal.Text = Convert.ToString(TotalAmount + RentAmount + LabourAmount);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            dtpOrderDate.Enabled = true;
            cmbZone.Enabled = true;
            cmbAddress.Enabled = true;
            cmbCustomer.Enabled = true;
            txtPageNo.Enabled = true;
            txtBilty.Enabled = true;
            txtRemark.Enabled = true;
            cmbTransport.Enabled = true;
            btnGenrateOrder.Enabled = true;
            btnGenrateOrder.Text = "Update Order";
            btnAddProduct.Enabled = false;
            btnDeleteProduct.Enabled = false;

        }

        private void txtLabourCharges_TextChanged(object sender, EventArgs e)
        {
            int TotalAmount = int.Parse(txtTotal.Text);
            int RentAmount = 0;
            int LabourAmount = 0;
            if (txtRent.Text != string.Empty)
            {
                RentAmount = int.Parse(txtRent.Text);
            }
            else
            {
                RentAmount = 0;
            }
            if (txtLabourCharges.Text != string.Empty)
            {
                LabourAmount = int.Parse(txtLabourCharges.Text);
            }
            else
            {
                LabourAmount = 0;
            }
            txtGrandTotal.Text = Convert.ToString(TotalAmount + RentAmount + LabourAmount);


        }

        private void txtPerBag_TextChanged(object sender, EventArgs e)
        {
            if (txtPerBag.Text == string.Empty)
            {
                txtPerBag.Text = string.Empty;
            }
            if (txtPerBag.Text != string.Empty)
            {
                txtLabourCharges.Text = Convert.ToInt32(Convert.ToInt32(txtPerBag.Text) * Convert.ToInt32(lblQty.Text)).ToString();
            }
            else
            {
                txtLabourCharges.Text = string.Empty;
            }
        }

        private void btnDeleteProduct_Click(object sender, EventArgs e)
        {
            DialogResult dg = MessageBox.Show("Are you sure you wana delete this Product ?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dg == DialogResult.Yes)
            {
                using (SqlConnection conn = new SqlConnection(clsConnectionString.cs))
                {
                    if (dataGridView1.SelectedRows.Count == 0)
                    {
                        MessageBox.Show("Please Select any Product");
                        return;
                    }
                    int rowindex = dataGridView1.CurrentCell.RowIndex;

                    string query = string.Format("delete from tblProductTransaction where InvoiceId = {0} and ProductName ='{1}'", int.Parse(txtOrderID.Text), dataGridView1.Rows[rowindex].Cells[1].Value.ToString());
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        if (conn.State != ConnectionState.Open)
                        {
                            conn.Open();
                        }
                        int status = cmd.ExecuteNonQuery();
                        if (status > 0)
                        {
                            BlStock.ProductTransId = int.Parse(txtOrderID.Text);
                            BlStock.ProductId = (int)dataGridView1.Rows[rowindex].Cells["ProductId"].Value;
                            BlStock.TransactionType = "Sale Invoice";   
                            DlStock.Delete(BlStock);
                            MessageBox.Show("Your data has been perminently deleted...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            FillDataFieldInProduct(int.Parse(txtOrderID.Text));
                            GetTotal();
                        }
                    }
                }
            }
        }

        private void addProductToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmProduct fp = new frmProduct();
            fp.ShowDialog();

            cmbProduct.SelectedValueChanged -= new EventHandler(cmbProduct_SelectedValueChanged);
            cmbProduct.DataSource = null;
            cmbProduct.DataSource = DlProduct.GetAllProduct();
            cmbProduct.DisplayMember = "ProductName";
            cmbProduct.ValueMember = "ProductId";
            cmbProduct.SelectedIndex = -1;
            cmbProduct.SelectedValueChanged += new EventHandler(cmbProduct_SelectedValueChanged);

        }

        private void viewCustomerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmViewCustomer fvc = new frmViewCustomer();
            fvc.ShowDialog();

        }

        private void txtQty_KeyPress(object sender, KeyPressEventArgs e)
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
            if (e.KeyChar == 45)
            {
                e.Handled = false;
            }

        }

        private void btnGatePas_Click(object sender, EventArgs e)
        {
            Reports.frmGatePas fg = new Reports.frmGatePas();
            fg.Id = int.Parse(txtOrderID.Text);
            fg.ShowDialog();
        }

        private void btnGenrateInvoice_Click(object sender, EventArgs e)
        {
            if (txtGrandTotal.Text != string.Empty && dataGridView1.Rows.Count > 0 && txtPerBag.Text != string.Empty)
            {
                DialogResult dg = MessageBox.Show("Are your sure you wana Convet in Invoice...", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dg == DialogResult.Yes)
                {
                    using (SqlConnection conn = new SqlConnection(clsConnectionString.cs))
                    {
                        int NewAmount = int.Parse(lblPreviousAmount.Text) + int.Parse(txtGrandTotal.Text);

                        string query = string.Format("update tblInvoice set SDate = '{0}',ZoneId = {1},AreaId ={2},CustomerId = {3},PageNo = '{4}',Bilty ='{5}',Remark = N'{6}',Transport=N'{7}',GrandTotal ={8} ,PostingDate='{9}',PreviousAmount = {10},NewAmount={11},PostingType='{12}',Rent = {13},PerBagCharge = {14},LabourCharges = {15},TotalAmount={16} where Sid= {17}", dtpOrderDate.Value.ToString("yyyy-MM-dd"), (int)cmbZone.SelectedValue, (int)cmbAddress.SelectedValue, (int)cmbCustomer.SelectedValue, txtPageNo.Text, txtBilty.Text, txtRemark.Text, cmbTransport.Text, int.Parse(txtGrandTotal.Text), DateTime.Now.ToString("yyyy-MM-dd"), lblPreviousAmount.Text, NewAmount, "Invoice", int.Parse(txtRent.Text), int.Parse(txtPerBag.Text), int.Parse(txtLabourCharges.Text), int.Parse(txtTotal.Text), int.Parse(txtOrderID.Text));
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            if (conn.State != ConnectionState.Open)
                            {
                                conn.Open();
                            }
                            int status = (int)cmd.ExecuteNonQuery();
                            if (status > 0)
                            {
                                MessageBox.Show("Your Order Convert in Invoice ...", "Inform", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                BlTransaction.TransactionDate = dtpOrderDate.Value;
                                BlTransaction.TransactionType = "Invoice";
                                BlTransaction.TransactionId = int.Parse(txtOrderID.Text);
                                BlTransaction.CustomerId = (int)cmbCustomer.SelectedValue;
                                BlTransaction.Remark = txtRemark.Text;
                                BlTransaction.Dr = 0;
                                BlTransaction.Cr = int.Parse(txtGrandTotal.Text);
                                BlTransaction.PostingDate = DateTime.Now;
                                BlTransaction.TranTypeUrdu = "سیل";
                                if (DlTransaction.Insert(BlTransaction) > 0)
                                {
                                    //Dr Entry
                                    blCoaLedger.TransactionDate = dtpOrderDate.Value;
                                    blCoaLedger.TransactionType = "Create Sale Invoice";
                                    blCoaLedger.TransactionId = int.Parse(txtOrderID.Text);
                                    blCoaLedger.ChartOfAccountId = DlCustomer.CustomerAccountId((int)cmbCustomer.SelectedValue);
                                    blCoaLedger.Remark = txtRemark.Text;
                                    blCoaLedger.Dr = int.Parse(txtGrandTotal.Text);
                                    blCoaLedger.Cr = 0;
                                    dlCoaLedger.Insert(blCoaLedger);
                                    //Cr Entry
                                    blCoaLedger.TransactionDate = dtpOrderDate.Value;
                                    blCoaLedger.TransactionType = "Create Sale Invoice";
                                    blCoaLedger.TransactionId = int.Parse(txtOrderID.Text);
                                    // Chart of Account Credit Sale Id
                                    blCoaLedger.ChartOfAccountId = 1040;
                                    blCoaLedger.Remark = txtRemark.Text;
                                    blCoaLedger.Dr = 0;
                                    blCoaLedger.Cr = int.Parse(txtGrandTotal.Text);
                                    dlCoaLedger.Insert(blCoaLedger);

                                    MessageBox.Show("Your ledger has been done...");
                                    lblIsPost.Text = "Invoice";

                                }

                            }
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please Enter Labour Charges/Rent Charges and Product Entry is Required....", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lblIsPost_TextChanged(object sender, EventArgs e)
        {
            if (lblIsPost.Text == "Invoice")
            {
                btnGenrateInvoice.Enabled = false;
                txtQty.Enabled = false;
                cmbProduct.Enabled = false;
                chkIsKg.Enabled = false;
                txtKg.Enabled = false;
                txtWeight.Enabled = false;
                txtPrice.Enabled = false;
                //lblTotalAmount.Enabled = false;
                btnAddProduct.Enabled = false;
                // btnUpdateProduct.Enabled = false;
                btnDeleteProduct.Enabled = false;
                button1.Enabled = false;
                txtTotal.Enabled = false;
                txtRent.Enabled = false;
                txtLabourCharges.Enabled = false;
                txtPerBag.Enabled = false;
                txtGrandTotal.Enabled = false;
            }
            else if (lblIsPost.Text == "Order")
            {
                btnGenrateInvoice.Enabled = true;

                if (btnGenrateOrder.Enabled == false)
                {
                    txtQty.Enabled = true;
                    cmbProduct.Enabled = true;
                    chkIsKg.Enabled = true;
                    txtKg.Enabled = true;
                    txtWeight.Enabled = true;
                    txtPrice.Enabled = true;
                    //lblTotalAmount.Enabled = true;

                    btnAddProduct.Enabled = true;
                    ////btnUpdateProduct.Enabled = true;
                    btnDeleteProduct.Enabled = true;
                    button1.Enabled = true;
                    txtTotal.Enabled = true;
                    txtRent.Enabled = true;
                    txtLabourCharges.Enabled = true;
                    txtPerBag.Enabled = true;
                    txtGrandTotal.Enabled = true;
                }

                if (btnGenrateOrder.Enabled == false)
                {
                    txtQty.Focus();
                }

            }
        }

        private void chkIsKg_CheckedChanged(object sender, EventArgs e)
        {
            if (chkIsKg.Checked == false)
            {
                txtKg.Enabled = false;
                txtWeight.Enabled = false;
                txtWeight.Text = string.Empty;
            }
            else
            {
                txtKg.Enabled = true;
                txtWeight.Enabled = true;
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

        private void cmbProduct_SelectedValueChanged(object sender, EventArgs e)
        {

            DataTable dt = GetProductDetail((int)cmbCustomer.SelectedValue, (int)cmbProduct.SelectedValue);
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                string kg, price;
                kg = dr[6].ToString();
                price = dr[8].ToString();
                if (kg == string.Empty)
                {
                    if (txtQty.Text == string.Empty)
                    {
                        txtQty.Text = "1";
                    }
                    chkIsKg.Checked = false;
                    txtPrice.Text = price;
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
                        txtPrice.Text = price;
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
                    MessageBox.Show("This Product sale will be first time");
                    DataTable dtProductDetail = GetProductDetailByDefault((int)cmbProduct.SelectedValue);
                    if (dtProductDetail.Rows.Count > 0)
                    {
                        DataRow drProductDetailRow = dtProductDetail.Rows[0];
                        txtKg.Text = drProductDetailRow[5].ToString();
                        txtKg_TextChanged(txtKg, EventArgs.Empty);
                        txtPrice.Text = drProductDetailRow[7].ToString();
                        txtPrice_TextChanged(txtPrice, EventArgs.Empty);
                    }
                    else
                    {
                        txtKg.TextChanged -= new EventHandler(txtKg_TextChanged);
                        txtKg.Text = string.Empty;
                        txtKg.TextChanged += new EventHandler(txtKg_TextChanged);
                        txtWeight.Text = string.Empty;
                        txtPrice.TextChanged -= new EventHandler(txtPrice_TextChanged);
                        txtPrice.Text = string.Empty;
                        txtPrice.TextChanged += new EventHandler(txtPrice_TextChanged);
                    }

                }


            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtSearch.Text != string.Empty)
            {
                if (int.Parse(txtSearch.Text) >= int.Parse(lblFirstId.Text) && int.Parse(txtSearch.Text) <= int.Parse(lblLastId.Text))
                {
                    lblCurrentId.Text = txtSearch.Text;
                    FillDataFieldInOrder(int.Parse(lblCurrentId.Text));
                    FillDataFieldInProduct(int.Parse(lblCurrentId.Text));
                    GetTotal();
                }
                else
                {
                    MessageBox.Show("Yor data is not exists our Database", "Inform", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
        }

        private void btnPrintInvoice_Click(object sender, EventArgs e)
        {
            Reports.frmPrintInvoice inv = new Reports.frmPrintInvoice();
            inv.Id = int.Parse(txtOrderID.Text);
            inv.ShowDialog();
        }

        private void pandingOrderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmViewOrder pendingOrder = new frmViewOrder();
            pendingOrder.ShowDialog();
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

        private void frmInvoice_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                btnGenrateNew_Click(null, new EventArgs());
            }
            else if (e.KeyCode == Keys.F12)
            {
                btnLast_Click(null, new EventArgs());
            }
            else if (e.KeyCode == Keys.F11)
            {
                btnPrevious_Click(null, new EventArgs());
            }
            else if (e.KeyCode == Keys.F10)
            {
                btnNext_Click(null, new EventArgs());
            }
            else if (e.KeyCode == Keys.F9)
            {
                btnFirst_Click(null, new EventArgs());
            }
            else if (e.KeyCode == Keys.F8)
            {
                btnPrintInvoice_Click(null, new EventArgs());
            }
            else if (e.KeyCode == Keys.F7)
            {
                btnGatePas_Click(null, new EventArgs());
            }
            else if (e.KeyCode == Keys.F6)
            {

                frmProduct fp = new frmProduct();
                fp.ShowDialog();

                cmbProduct.SelectedValueChanged -= new EventHandler(cmbProduct_SelectedValueChanged);
                cmbProduct.DataSource = null;
                cmbProduct.DataSource = DlProduct.GetAllProduct();
                cmbProduct.DisplayMember = "ProductName";
                cmbProduct.ValueMember = "ProductId";
                cmbProduct.SelectedIndex = -1;
                cmbProduct.SelectedValueChanged += new EventHandler(cmbProduct_SelectedValueChanged);
            }

        }

        private void dtpOrderDate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.SelectNextControl((Control)sender, true, true, true, true);
                e.SuppressKeyPress = true;
            }
        }

        private void cmbProduct_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void cmbProduct_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                frmSearchProduct fs = new frmSearchProduct();
                fs.ShowDialog();
                cmbProduct.SelectedValue = Properties.Settings.Default.ProductID;

            }
            else if (e.KeyCode == Keys.Enter)
            {
                //chkIsKg.Focus();
                txtQty.Focus();
            }
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtSearch.Text != string.Empty)
                {
                    if (int.Parse(txtSearch.Text) >= int.Parse(lblFirstId.Text) && int.Parse(txtSearch.Text) <= int.Parse(lblLastId.Text))
                    {
                        lblCurrentId.Text = txtSearch.Text;
                        FillDataFieldInOrder(int.Parse(lblCurrentId.Text));
                        FillDataFieldInProduct(int.Parse(lblCurrentId.Text));
                        GetTotal();
                    }
                    else
                    {
                        MessageBox.Show("Yor data is not exists our Database", "Inform", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

            }

        }
        private void lastRent()
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionString.cs))
            {
                string q = string.Format("select Rent from tblInvoice where CustomerId={0} and SDate=(select max(SDate) from tblInvoice where CustomerID={1} and SDate <> getdate() and PostingType<>'Order')", (int)cmbCustomer.SelectedValue, (int)cmbCustomer.SelectedValue);
                using (SqlCommand cmd = new SqlCommand(q, conn))
                {
                    if (conn.State != ConnectionState.Open)
                    {
                        conn.Open();
                    }
                    int RentAmount = Convert.ToInt32(cmd.ExecuteScalar());
                    txtRent.Text = RentAmount.ToString();
                    conn.Close();
                }
            }
        }

        private void txtRent_Click(object sender, EventArgs e)
        {
            lastRent();
        }

        private void LastLabourCharges()
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionString.cs))
            {
                string q = string.Format("select PerBagCharge from tblInvoice where CustomerId={0} and SDate=(select max(SDate) from tblInvoice where CustomerID={1} and SDate <> getdate() and PostingType<>'Order') ", cmbCustomer.SelectedValue, cmbCustomer.SelectedValue);
                using (SqlCommand cmd = new SqlCommand(q, conn))
                {
                    if (conn.State != ConnectionState.Open)
                    {
                        conn.Open();
                    }
                    int LabourCharges = Convert.ToInt32(cmd.ExecuteScalar());
                    txtPerBag.Text = LabourCharges.ToString();
                    conn.Close();
                }
            }
        }

        private void txtPerBag_Click(object sender, EventArgs e)
        {
            LastLabourCharges();
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

        private void txtWeight_TextChanged(object sender, EventArgs e)
        {
            txtQty_TextChanged(txtQty, EventArgs.Empty);
            txtPrice_TextChanged(txtPrice, EventArgs.Empty);
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

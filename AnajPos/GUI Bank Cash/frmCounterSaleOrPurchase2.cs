using AnajPos.BAL;
using AnajPos.BAL_Cash_BANK;
using AnajPos.DAL;
using AnajPos.DAL_Cash_Bank;
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
    public partial class frmCounterSaleOrPurchase2 : frmTemplete
    {
        clsChartOfAccount dlCoa = new clsChartOfAccount();
        clsBLChartOfAccountLedger blCoaLedger = new clsBLChartOfAccountLedger();
        clsChartOfAccountLedger dlCoaLedger = new clsChartOfAccountLedger();
        clsProduct DlProduct = new clsProduct();
        clsWarehouse DlWarehoues = new clsWarehouse();
        clsStock DlStock = new clsStock();
        clsBlStock BlStock = new clsBlStock();
        clsBLChartOfAccountLedger clsBLChartOfAccountLedger = new clsBLChartOfAccountLedger();
        clsChartOfAccountLedger clsChartOfAccountLedger = new clsChartOfAccountLedger();


        public frmCounterSaleOrPurchase2()
        {
            InitializeComponent();
        }
        private void CounterSaleLoad()
        {

        }
        private void CounterzSalePurchaseLoad()
        {

        }



        private void frmCounterSaleOrPurchase2_Load(object sender, EventArgs e)
        {
            btnNew.Focus();
            DataTable dtViewPaymentMode = dlCoa.ViewChartOfAccountPaymentMode();
            cmbHead.DataSource = dtViewPaymentMode;
            cmbHead.ValueMember = "AccountId";
            cmbHead.DisplayMember = "AccountName";
            cmbHead.SelectedIndex = -1;

            DataTable dtCounterSaleMode = dlCoa.ViewChartOfAccountParenttID(1031);

            // Filter out rows with AccountId = 1040
            DataRow[] filteredRows = dtCounterSaleMode.Select("AccountId <> 1040");

            // Create a new DataTable with the filtered rows
            DataTable dtFilteredCounterSaleMode = dtCounterSaleMode.Clone();
            foreach (DataRow row in filteredRows)
            {
                dtFilteredCounterSaleMode.ImportRow(row);
            }

            cmbCustomer.DataSource = dtFilteredCounterSaleMode;
            cmbCustomer.ValueMember = "AccountId";
            cmbCustomer.DisplayMember = "AccountName";
            cmbCustomer.SelectedIndex = -1;



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

            disableField();


            btnAddProduct.Enabled = false;
            btnDeleteProduct.Enabled = false;
            btnGenrateInvoice.Enabled = false;

            lblFirstId.Text = MinId().ToString();
            lblLastId.Text = MaxId().ToString();

            FillDataFieldInOrder(int.Parse(lblFirstId.Text));
            FillDataFieldInProduct(int.Parse(lblFirstId.Text));
            lblCurrentId.Text = txtOrderID.Text;
            ClearFielProduct();
            GetTotal();
            txtPerBag.Text = "0";

        }

        #region Btn_New Wor
        private void btnNew_Click(object sender, EventArgs e)
        {

            string query = "select ISNULL(max(InvoiceID),0) as NewIvoiceID from tblCounterSaleOrPurchase";

            using (SqlConnection connection = new SqlConnection(clsConnectionString.cs))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();

                    // Execute the query and get the maximum ID
                    object result = command.ExecuteScalar();

                    // Check if the result is not null and is convertible to an integer
                    if (result != null && int.TryParse(result.ToString(), out int maxID))
                    {
                        // Display the maximum ID in the TextBox
                        maxID++;
                        txtOrderID.Text = maxID.ToString();
                    }
                    else
                    {
                        MessageBox.Show("Unable to fetch the maximum ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    dataGridView1.DataSource = null;
                    dtpOrderDate.Value = DateTime.Now;
                    cmbCustomer.SelectedIndex = -1;
                    txtTotal.Text = string.Empty;
                    btnAddProduct.Enabled = true;
                    btnDeleteProduct.Enabled = true;
                    btnGenrateInvoice.Enabled = true;
                    btnFirst.Enabled = true;
                    btnNew.Enabled = true;
                    btnLast.Enabled = true;
                    btnPrevious.Enabled = true;
                    btnNext.Enabled = true;
                    txtRent.Text = "0";
                    txtPerBag.Text = "0";
                    txtGrandTotal.Text = "0";
                    EnableField();
                    ClearFielProduct();
                    dtpOrderDate.Focus();

                    //GetTotal();
                }
            }
        }
        #endregion

        #region Btn_addProduct Work

        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            if (txtOrderID.Text == string.Empty)
            {
                MessageBox.Show("Please Add New Order.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (txtQty.Text == string.Empty)
            {
                MessageBox.Show("Please Enter Quantity.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (cmbLocation.SelectedIndex == -1)
            {
                MessageBox.Show("Please Select Location.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

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
            else if (Convert.ToDecimal(txtPrice.Text) < 0)
            {
                MessageBox.Show("Quantity should be grater then 0");
                return;
            }
            try
            {
                using (SqlConnection connection = new SqlConnection(clsConnectionString.cs))
                {
                    connection.Open();
                    if (IsProductAlreadyExists(cmbProduct.Text))
                    {
                        decimal weight = Convert.ToDecimal(txtWeight.Text);
                        string query = "insert into tblCounterSaleOrPurchase (CounterNo,InvoiceID,InvTime,InvDate,ProductId,Price,Qty,Remark,Weight,Kg,Discount,WarehouseId,IsDeleted,IsLedger,Rent) values (@CounterNo,@InvoiceID,@InvTime,@InvDate,@ProductId,@Price,@Qty,@Remark,@Weight,@kg,@Discount,@WarehouseId,@IsDeleted,@IsLedger,@Rent)";
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@InvoiceID", Convert.ToInt32(txtOrderID.Text));
                            command.Parameters.AddWithValue("@InvTime", DateTime.Now);
                            command.Parameters.AddWithValue("@InvDate", dtpOrderDate.Value);
                            command.Parameters.AddWithValue("@ProductId", cmbProduct.SelectedValue);
                            command.Parameters.AddWithValue("@CounterNo", cmbCustomer.SelectedValue);
                            command.Parameters.AddWithValue("@Price", txtPrice.Text);
                            command.Parameters.AddWithValue("@Qty", txtQty.Text);
                            command.Parameters.AddWithValue("@kg", txtKg.Text);
                            command.Parameters.AddWithValue("@Weight", weight);
                            command.Parameters.AddWithValue("@IsLedger", 0);
                            command.Parameters.AddWithValue("@Rent", txtRent.Text);
                            if (txtPerBag.Text == "")
                            {
                                command.Parameters.AddWithValue("@Discount", 0);
                            }
                            else
                            {
                                command.Parameters.AddWithValue("@Discount", txtPerBag.Text);
                            }
                            command.Parameters.AddWithValue("@WarehouseId", cmbLocation.SelectedValue);
                            command.Parameters.AddWithValue("@IsDeleted", 0);
                            command.Parameters.AddWithValue("@Remark", txtRemark.Text);
                            int status = command.ExecuteNonQuery();
                            if (status > 0)
                            {
                                BlStock.ProductTransDate = dtpOrderDate.Value;
                                BlStock.ProductTransId = int.Parse(txtOrderID.Text);
                                BlStock.TransactionType = "Counter Sale";
                                BlStock.ProductId = (int)cmbProduct.SelectedValue;
                                BlStock.UnitID = 1;
                                BlStock.WarehouseId = (int)cmbLocation.SelectedValue;
                                BlStock.StockIn = 0;
                                BlStock.StockOut = decimal.Parse(txtQty.Text);
                                BlStock.Amount = decimal.Parse(txtPrice.Text);
                                BlStock.IsDeleted = false;
                                if (DlStock.Insert(BlStock) > 0)
                                {
                                    MessageBox.Show("your data has been done...");
                                    FillDataFieldInProduct(int.Parse(txtOrderID.Text));
                                    //FillBilling();
                                    GetTotal();
                                    btnNew.Enabled = true;
                                    btnGenrateInvoice.Enabled = true;
                                    cmbHead.Enabled = true;
                                    ClearFielProduct();
                                    lblTotalAmount.Text = "0";
                                    cmbProduct.Focus();
                                }

                            }
                        }

                    }

                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }

        }



        #endregion


        #region For Invoice Work
        private void btnGenrateInvoice_Click(object sender, EventArgs e)
        {
            if (!IsValidateDispetch())
            {
                return;
            }

            DialogResult dg = MessageBox.Show("Are your sure you wana Convet in Invoice...", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dg == DialogResult.Yes)
            {
                using (SqlConnection conn = new SqlConnection(clsConnectionString.cs))
                {
                    string query = string.Format("Update tblCounterSaleOrPurchase set PaymentType = {0},Isledger = {1},Discount = {2},Rent = {3} where InvoiceId ={4}", (int)cmbHead.SelectedValue, 1, txtPerBag.Text, txtRent.Text, int.Parse(txtOrderID.Text));
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        if (conn.State != ConnectionState.Open)
                        {
                            conn.Open();
                        }
                        int status = (int)cmd.ExecuteNonQuery();
                        if (status > 0)
                        {
                            blCoaLedger.TransactionDate = dtpOrderDate.Value;
                            blCoaLedger.TransactionType = "Counter Sale";
                            blCoaLedger.TransactionId = int.Parse(txtOrderID.Text);
                            blCoaLedger.ChartOfAccountId = (int)cmbHead.SelectedValue;
                            blCoaLedger.Remark = txtRemark.Text;
                            blCoaLedger.Dr = int.Parse(txtGrandTotal.Text);
                            blCoaLedger.Cr = 0;
                            dlCoaLedger.Insert(blCoaLedger);
                            //Cr Entry
                            blCoaLedger.TransactionDate = dtpOrderDate.Value;
                            blCoaLedger.TransactionType = "Counter Sale";
                            blCoaLedger.TransactionId = int.Parse(txtOrderID.Text);
                            // Chart of Account Credit Sale Id
                            blCoaLedger.ChartOfAccountId = (int)cmbCustomer.SelectedValue;
                            blCoaLedger.Remark = txtRemark.Text;
                            blCoaLedger.Dr = 0;
                            blCoaLedger.Cr = int.Parse(txtGrandTotal.Text);
                            if (dlCoaLedger.Insert(blCoaLedger) > 0)
                            {

                                MessageBox.Show("your data has been done...");
                                FillDataFieldInProduct(int.Parse(txtOrderID.Text));
                                //FillBilling();
                                GetTotal();
                                btnNew.Enabled = true;
                                btnGenrateInvoice.Enabled = false;
                                cmbHead.Enabled = true;
                                ClearFielProduct();
                                lblTotalAmount.Text = "0";
                                disableField();
                                DisableProduct();
                            }




                        }
                    }
                }
            }
        }

        private bool IsValidateDispetch()
        {
            if (cmbHead.SelectedIndex == -1)
            {
                MessageBox.Show("Please Select Cash Account");
                cmbHead.Focus();
                return false;
            }
            else if (txtGrandTotal.Text == string.Empty)
            {
                MessageBox.Show("Please Enter Grand Total");
                cmbHead.Focus();
                return false;
            }
            else if (dataGridView1.Rows.Count < 0)
            {
                MessageBox.Show("Please add any Product");
                cmbProduct.Focus();
                return false;
            }
            if (txtRent.Text == string.Empty)
            {
                MessageBox.Show("Please Enter Rent...");
                txtRemark.Focus();
                return false;
            }
            else if (txtPerBag.Text == string.Empty)
            {

                MessageBox.Show("Please Enter Discount...");
                txtRemark.Focus();
                return false;
            }
            else if (txtGrandTotal.Text == string.Empty)
            {
                MessageBox.Show("Please Enter Grand Total");
                txtGrandTotal.Focus();
                return false;
            }
            return true;
        }
        #endregion

        #region Delete Work 

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
                    // Get the value of the "ProductId" column in the selected row
                    object selectedProductIdObject = dataGridView1.Rows[rowindex].Cells["ProductId"].Value;
                    int selectedProductId = 0;
                    if (dataGridView1.CurrentRow != null)
                    {


                        // Check if the value is not null
                        if (selectedProductIdObject != null)
                        {
                            selectedProductId = Convert.ToInt32(selectedProductIdObject);

                            // Now, you have the selected product ID
                            // You can use selectedProductId in your logic
                        }
                        else
                        {

                            MessageBox.Show("Please Select any row");
                            return;
                            // Handle the case where the value is null (if needed)
                        }
                    }

                    string query = string.Format("Update tblCounterSaleOrPurchase set IsDeleted = {0} where InvoiceId ={1} and ProductId = {2}", 1, int.Parse(txtOrderID.Text), selectedProductId);
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
                            BlStock.TransactionType = "Counter Sale";
                            BlStock.ProductId = (int)dataGridView1.Rows[rowindex].Cells["ProductId"].Value;
                            DlStock.Delete(BlStock);
                            MessageBox.Show("Your data has been perminently deleted...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            FillDataFieldInProduct(int.Parse(txtOrderID.Text));

                            GetTotal();

                        }
                    }
                }
            }
        }
        #endregion
        #region Work On chnage of Product Drop down
        private void cmbProduct_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cmbCustomer.SelectedValue != null)
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
            else
            {
                MessageBox.Show("Please Select The Customer");

            }
            cmbLocation.SelectedIndex = -1;
            cmbLocation.SelectedValue = 0;
            lblTotalStock.Text = string.Empty;

        }
        #endregion
        #region Get Product Details 
        private DataTable GetProductDetail(int cId, int pid)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(clsConnectionString.cs))
            {
                string qurery = string.Format("select * from tblCounterSaleOrPurchase where CounterNo = {0} and ProductId = {1} and InvDate=(select MAX(InvDate) from tblCounterSaleOrPurchase where ProductId = {2} and CounterNo ={3})", cId, pid, pid, cId);
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
                string qurery = string.Format("select * from tblCounterSaleOrPurchase where ProductId = {0} and InvDate =(select max(InvDate) from tblCounterSaleOrPurchase where ProductId = {1})", Pid, Pid);
                SqlDataAdapter adpt = new SqlDataAdapter(qurery, conn);
                adpt.Fill(dt);
            }

            return dt;
        }
        #endregion

        private bool IsProductAlreadyExists(string ValidText)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (ValidText == dataGridView1.Rows[i].Cells["ProductName"].Value.ToString())
                {
                    MessageBox.Show("this Product is Already exists in this Order Please Update this Product", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }
            return true;

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
            try
            {
                if (txtQty.Text != string.Empty && txtKg.Enabled == true && txtKg.Text != string.Empty)
                {
                    txtWeight.Text = Convert.ToString(Convert.ToDouble(txtQty.Text) * Convert.ToDouble(txtKg.Text));
                }
            }
            catch
            {


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

        #region Fill the Grid


        //private void FillBilling()
        //{

        //    string query = string.Format("select  i.CounterId,i.ProductId,i.Qty, p.ProductName,i.Kg,i.Weight,i.Price,i.Price * i.Qty as Total from tblCounterSaleOrPurchase  i inner join tblProduct p on p.ProductID = i.ProductId  where i.InvoiceID = {0}", Convert.ToInt32(txtOrderID.Text));

        //    using (SqlConnection connection = new SqlConnection(clsConnectionString.cs))
        //    {
        //        using (SqlCommand command = new SqlCommand(query, connection))
        //        {
        //            connection.Open();
        //            SqlDataAdapter adapter = new SqlDataAdapter(command);
        //            DataTable dataTable = new DataTable();
        //            adapter.Fill(dataTable);

        //            dataGridView1.DataSource = dataTable;
        //            dataGridView1.Columns[0].Width = 0;
        //            int Total = CalculateColumnTotal(dataGridView1, "Total");
        //            txtGrandTotal.Text = Total.ToString();
        //            dataGridView1.Columns["ProductId"].Visible = false;
        //            dataGridView1.Columns["CounterId"].Visible = false;
        //            dataGridView1.Columns["ProductName"].Width = 250;
        //            dataGridView1.Columns["Price"].Width = 100;
        //            dataGridView1.Columns["Qty"].Width = 100;
        //            dataGridView1.Columns["Total"].Width = 100;
        //            dataGridView1.Columns["Weight"].Width = 100;
        //            dataGridView1.Columns["Kg"].Width = 100;


        //        }
        //    }
        //}
        #endregion
        private int CalculateColumnTotal(DataGridView dataGridView, string columnName)
        {
            int total = 0;

            if (dataGridView.Columns.Contains(columnName))
            {
                foreach (DataGridViewRow row in dataGridView.Rows)
                {
                    if (row.Cells[columnName].Value != null &&
                        int.TryParse(row.Cells[columnName].Value.ToString(), out int value))
                    {
                        total += value;
                    }
                }
            }
            else
            {
                // Column not found, handle the error as needed (e.g., throw an exception, return a default value, etc.)
            }

            return total;
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

        private void GetTotal()
        {
            int FinalCost = 0;

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                FinalCost = FinalCost + Convert.ToInt32(dataGridView1.Rows[i].Cells["Total"].Value);

            }
            //lblCount.Text = Count.ToString();
            //lblQty.Text = Qty.ToString();
            txtTotal.Text = FinalCost.ToString();
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


        private void FillDataFieldInProduct(int Id)
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionString.cs))
            {
                string q = string.Format("select i.CounterId,i.ProductId,Cast(i.Qty as int) as 'Qty',p.ProductName,w.WarehouseName as 'Location',i.Kg,FORMAT(i.Price,'N2') as 'Price',i.Weight,Cast(i.Price * i.Weight as int) as Total from tblCounterSaleOrPurchase  i inner join tblProduct p on p.ProductID = i.ProductId inner join tblWarehouse w on w.WarehouseId = i.WarehouseId where i.InvoiceID ={0} and i.IsDeleted = 0", Id);
                SqlDataAdapter adpt = new SqlDataAdapter(q, conn);
                DataTable dt = new DataTable();
                adpt.Fill(dt);
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = dt;
                //dataGridView1.Columns["CounterId"].Width = 90;
                dataGridView1.Columns["CounterId"].Visible = false;
                dataGridView1.Columns["ProductId"].Visible = false;
                dataGridView1.Columns["ProductName"].Width = 430;
                dataGridView1.Columns["Price"].Width = 150;
                dataGridView1.Columns["Qty"].Width = 150;
                dataGridView1.Columns["Total"].Width = 150;
                dataGridView1.Columns["Weight"].Width = 150;

                //dataGridView1.Columns["ProductId"].Visible = false;
            }
        }

        private void txtRent_TextChanged(object sender, EventArgs e)
        {

            try
            {
                int rentAmount = string.IsNullOrEmpty(txtRent.Text) ? 0 : int.Parse(txtRent.Text);
                int totalAmount = string.IsNullOrEmpty(txtTotal.Text) ? 0 : int.Parse(txtTotal.Text);
                int perBagAmount = string.IsNullOrEmpty(txtPerBag.Text) ? 0 : int.Parse(txtPerBag.Text);

                txtGrandTotal.Text = (rentAmount + totalAmount - perBagAmount).ToString();
            }
            catch (FormatException)
            {
                // Handle format exception (non-integer input)
                //txtGrandTotal.Text = "Invalid input";
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                Console.WriteLine($"Error in calculating GrandTotal: {ex.Message}");
                txtGrandTotal.Text = "0";
            }
        }

        private void txtPerBag_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int rentAmount = string.IsNullOrEmpty(txtRent.Text) ? 0 : int.Parse(txtRent.Text);
                int totalAmount = string.IsNullOrEmpty(txtTotal.Text) ? 0 : int.Parse(txtTotal.Text);
                int perBagAmount = string.IsNullOrEmpty(txtPerBag.Text) ? 0 : int.Parse(txtPerBag.Text);

                txtGrandTotal.Text = (rentAmount + totalAmount - perBagAmount).ToString();
            }
            catch (FormatException)
            {
                // Handle format exception (non-integer input)
                txtGrandTotal.Text = "Invalid input";
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                Console.WriteLine($"Error in calculating GrandTotal: {ex.Message}");
                txtGrandTotal.Text = "0";
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

        private void txtWeight_TextChanged(object sender, EventArgs e)
        {

            txtQty_TextChanged(txtQty, EventArgs.Empty);
            txtPrice_TextChanged(txtPrice, EventArgs.Empty);

        }
        private void FillDataFieldInOrder(int Id)
        {

            using (SqlConnection conn = new SqlConnection(clsConnectionString.cs))
            {
                string q = string.Format("select CounterId,InvoiceId,InvDate,Remark,IsNull(PaymentType,0) as 'PaymentType',CounterNo,IsNull(Isledger,0) as 'Isledger',Cast(Discount as int),Rent from tblCounterSaleOrPurchase where InvoiceId={0}", Id);
                SqlDataAdapter adpt = new SqlDataAdapter(q, conn);
                DataTable dt = new DataTable();
                adpt.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    txtOrderID.Text = dr[1].ToString();
                    dtpOrderDate.Value = (DateTime)dr[2];


                    txtRemark.Text = (string)dr[3];
                    cmbHead.SelectedValue = (int)dr[4];
                    cmbCustomer.SelectedValue = (int)dr[5];

                    if ((bool)dr[6] == false)
                    {
                        btnGenrateInvoice.Enabled = true;
                        EnableProduct();
                    }
                    else
                    {
                        btnGenrateInvoice.Enabled = false;
                        DisableProduct();
                    }


                    txtRent.Text = dr[8].ToString();
                    txtPerBag.Text = dr[7].ToString();
                    //txtGrandTotal.Text = string.Empty;

                }
            }

        }

        #region For Btn Next,Last,First,Pervious
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

        private void btnFirst_Click(object sender, EventArgs e)
        {
            FillDataFieldInOrder(int.Parse(lblFirstId.Text));
            FillDataFieldInProduct(int.Parse(lblFirstId.Text));
            GetTotal();
            lblCurrentId.Text = lblFirstId.Text;
        }

        private void btnLast_Click(object sender, EventArgs e)
        {

            FillDataFieldInOrder(int.Parse(lblLastId.Text));
            FillDataFieldInProduct(int.Parse(lblLastId.Text));
            GetTotal();
            lblCurrentId.Text = lblLastId.Text;
        }

        private void btnPrevious_Click_1(object sender, EventArgs e)
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
        #endregion


        #region Max and Min For InvoiceID
        private int MinId()
        {
            int status = 0;
            using (SqlConnection conn = new SqlConnection(clsConnectionString.cs))
            {
                using (SqlCommand cmd = new SqlCommand("Select min(InvoiceId) as InvoiceId from tblCounterSaleOrPurchase", conn))
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
                using (SqlCommand cmd = new SqlCommand("Select max(InvoiceId) as InvoiceId from tblCounterSaleOrPurchase", conn))
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
        #endregion


        #region Disable and Enable

        public void EnableField()
        {
            dtpOrderDate.Enabled = true;
            cmbCustomer.Enabled = true;
            txtRemark.Enabled = true;
            cmbProduct.Enabled = true;
            txtKg.Enabled = true;
            txtQty.Enabled = true;
            txtWeight.Enabled = true;
            txtPrice.Enabled = true;
            cmbLocation.Enabled = true;
            txtRent.ReadOnly = false;
            txtPerBag.ReadOnly = false;

        }
        public void disableField()
        {
            dtpOrderDate.Enabled = false;
            cmbCustomer.Enabled = false;
            txtRemark.Enabled = false;

        }

        public void DisableProduct()
        {
            cmbProduct.Enabled = false;
            txtKg.Enabled = false;
            txtQty.Enabled = false;
            txtWeight.Enabled = false;
            txtPrice.Enabled = false;
            cmbLocation.Enabled = false;
            cmbHead.Enabled = false;
            btnAddProduct.Enabled = false;
            btnDeleteProduct.Enabled = false;
            txtRent.ReadOnly = true;
            txtPerBag.ReadOnly = true;

        }

        public void EnableProduct()
        {
            cmbProduct.Enabled = true;
            txtKg.Enabled = true;
            txtQty.Enabled = true;
            txtWeight.Enabled = true;
            txtPrice.Enabled = true;
            cmbLocation.Enabled = true;
            cmbHead.Enabled = true;
            btnAddProduct.Enabled = true;
            btnDeleteProduct.Enabled = true;
            txtRent.ReadOnly = true;
            txtPerBag.ReadOnly = true;

        }

        public void ClearField()
        {

            dtpOrderDate.Text = string.Empty;
            cmbCustomer.Text = string.Empty;
            txtRemark.Text = string.Empty;
            cmbProduct.Text = string.Empty;
            txtKg.Text = string.Empty;
            txtQty.Text = string.Empty;
            txtWeight.Text = string.Empty;
            txtPrice.Text = string.Empty;
            cmbLocation.Text = string.Empty;
            txtOrderID.Text = string.Empty;
            cmbCustomer.Text = string.Empty;
        }

        public void ClearFielProduct()
        {
            cmbProduct.Text = string.Empty;

            txtRemark.Text = string.Empty;
            lblTotalAmount.Text = string.Empty;
            lblTotalStock.Text = string.Empty;
            txtKg.Text = string.Empty;
            txtQty.Text = string.Empty;
            txtWeight.Text = string.Empty;
            txtPrice.Text = string.Empty;
           
            cmbLocation.SelectedValue = -1;
        }

        #endregion

        private void Ctrl_Keyup(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.SelectNextControl((Control)sender, true, true, true, true);
            }
        }

        private void txtTotal_TextChanged(object sender, EventArgs e)
        {
            txtRent_TextChanged(null, EventArgs.Empty);

            txtPerBag_TextChanged(null, EventArgs.Empty);
        }

        private void btnPrintInvoice_Click(object sender, EventArgs e)
        {
            if (txtOrderID.Text != string.Empty)
            {
                CashBankReporting.frmViewCounterReceipt inv = new CashBankReporting.frmViewCounterReceipt();
                inv.Id = int.Parse(txtOrderID.Text);
                inv.ShowDialog();
            }

        }

        private void addProductToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmProduct product = new frmProduct();
            product.ShowDialog();
            cmbProduct.SelectedValueChanged -= new EventHandler(cmbProduct_SelectedValueChanged);
            cmbProduct.DataSource = DlProduct.GetAllProduct();
            cmbProduct.DisplayMember = "ProductName";
            cmbProduct.ValueMember = "ProductId";
            cmbProduct.SelectedValueChanged += new EventHandler(cmbProduct_SelectedValueChanged);

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtSearch.Text != string.Empty)
            {
                if (int.Parse(txtSearch.Text) > int.Parse(lblLastId.Text))
                {
                    MessageBox.Show("Your Last Invoice ID is " + lblLastId.Text);
                    return;
                }
                else if (int.Parse(txtSearch.Text) < int.Parse(lblFirstId.Text))
                {
                    MessageBox.Show("Your First Invoice ID is " + lblLastId.Text);
                    return;
                }
                lblCurrentId.Text = txtSearch.Text;
                FillDataFieldInOrder(int.Parse(txtSearch.Text));
                FillDataFieldInProduct(int.Parse(txtSearch.Text));
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {

        }

        private void rollbackThisCounterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dg = MessageBox.Show("Are your sure you wana Rollback this Counter Sale!...", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dg == DialogResult.Yes)
            {
                if (RollbackCounter(int.Parse(txtOrderID.Text)) > 0)
                {
                    MessageBox.Show("Order has been Rollback");
                    FillDataFieldInOrder(int.Parse(txtOrderID.Text));
                    FillDataFieldInProduct(int.Parse(txtOrderID.Text));
                }
            }
        }
        public int RollbackCounter(int CounterId)
        {
            int HowManyInsert = 0;
            SqlConnection conn = new SqlConnection(clsConnectionString.cs);
            using (SqlCommand cmd = new SqlCommand("usp_RollbackCounter", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@InvoiceId", CounterId);
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                HowManyInsert = cmd.ExecuteNonQuery();

            }
            return HowManyInsert;

        }

        private void allPendingOrdersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmViewPendingCounterSale pc = new frmViewPendingCounterSale();
            pc.ShowDialog();
        }
    }
}

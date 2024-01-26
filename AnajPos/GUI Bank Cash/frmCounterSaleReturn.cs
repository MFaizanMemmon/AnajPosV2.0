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
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnajPos.GUI_Bank_Cash
{
    public partial class frmCounterSaleReturn : frmTemplete
    {
        
        clsChartOfAccount dlCoa = new clsChartOfAccount();
        clsBlStock BlStock = new clsBlStock();
        clsStock DlStock = new clsStock();
        clsWarehouse DlWarehoues = new clsWarehouse();
        clsBLChartOfAccountLedger blCoaLedger = new clsBLChartOfAccountLedger();
        clsChartOfAccountLedger dlCoaLedger = new clsChartOfAccountLedger();
        DataTable dt = new DataTable();
        public frmCounterSaleReturn()
        {
            InitializeComponent();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            string query = "select ISNULL(max(ReturnInvoiceId),0) as NewIvoiceID from tblCounterSaleReturn";

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
                        lblInvoiceID.Text = "00" + maxID.ToString();
                    }
                    else
                    {
                        MessageBox.Show("Unable to fetch the maximum ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (lblInvoiceID.Text == "000")
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
                MessageBox.Show("Please Select Location of Stock ... !");
                cmbLocation.Focus();
                cmbLocation.SelectedIndex = 0;
            }


            try
            {
                using (SqlConnection connection = new SqlConnection(clsConnectionString.cs))
                {
                    connection.Open();

                    string query = "insert into tblCounterSaleReturn (ReturnInvoiceId,CounterInvoiceId,ReturnDate,ProductId,Price,Qty,Weight,Remark,Warehouse,Kg,PaymentType,IsLedger) values " +
                        "(@ReturnInvoiceId,@CounterInvoiceId,@ReturnDate,@ProductId,@Price,@Qty,@Weight,@Remark,@Warehouse,@Kg,@PaymentType,@IsLedger)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ReturnInvoiceId", Convert.ToInt32(lblInvoiceID.Text));
                        command.Parameters.AddWithValue("@CounterInvoiceId", txtCounterSale.Text);
                        command.Parameters.AddWithValue("@ReturnDate", dtpOpeningDate.Value);
                        command.Parameters.AddWithValue("@ProductId", dgvProduct.SelectedRows[0].Cells["ProductId"].Value.ToString());
                        command.Parameters.AddWithValue("@Price", txtPrice.Text);
                        command.Parameters.AddWithValue("@Qty", txtQty.Text);
                        command.Parameters.AddWithValue("@Weight", txtWeight.Text);
                        command.Parameters.AddWithValue("@Remark", txtRemark.Text);
                        command.Parameters.AddWithValue("@Warehouse", cmbLocation.SelectedValue);
                        command.Parameters.AddWithValue("@Kg", txtPackaging.Text);


                        // Make sure that cmbPaymentMode.SelectedValue is providing a valid value
                        // and that the data type matches the expected type in the database.
                        command.Parameters.AddWithValue("@PaymentType", 0);

                        command.Parameters.AddWithValue("@IsLedger", false);

                        command.ExecuteNonQuery();
                    }
                }

                // Optionally, you can clear the input fields after successful insertion:
                //cmbGame.SelectedIndex = 0;
                //txtPrice.Text = "";
                txtQty.Text = "";
                txtTotal.Text = "";
                txtPrice.Text = "";
                //txtGameType.Text = "";
                FillBilling();
                txtPackaging.Text = string.Empty;
                txtWeight.Text = string.Empty;
                cmbLocation.SelectedIndex = -1;
                textBox1.Clear();
                textBox1.Focus();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void FillBilling()
        {

            string query = string.Format("select i.ReturnId,p.ProductName,i.Price,i.Qty,i.Price * i.Qty as Total from tblCounterSaleReturn i inner join tblProduct p on p.ProductID = i.ProductId where i.ReturnInvoiceId = {0} and ISNULL(i.IsDeleted,0) <> 1", Convert.ToInt32(lblInvoiceID.Text));

            using (SqlConnection connection = new SqlConnection(clsConnectionString.cs))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    dataGridView1.DataSource = dataTable;
                    dataGridView1.Columns[0].Visible = false;
                    int Total = CalculateColumnTotal(dataGridView1, "Total");
                    txtGrandTotal.Text = Total.ToString();

                }
            }
        }
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

        private void btnFirst_Click(object sender, EventArgs e)
        {
            string query = "select ISNULL(Min(ReturnId),0) as NewIvoiceID from tblCounterSaleReturn";

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

                        lblInvoiceID.Text = "00" + maxID.ToString();
                        FillBilling();
                    }
                    else
                    {
                        MessageBox.Show("Unable to fetch the maximum ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            int MaxID = 0;
            string query = "select ISNULL(Max(ReturnId),0) as NewIvoiceID from tblCounterSaleReturn";

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

                        MaxID = maxID;
                    }
                    else
                    {
                        MessageBox.Show("Unable to fetch the maximum ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            if (Convert.ToInt32(lblInvoiceID.Text) <= MaxID)
            {

                int NewID = Convert.ToInt32(lblInvoiceID.Text);
                NewID++;
                lblInvoiceID.Text = "00" + NewID.ToString();
                FillBilling();
            }
            else
            {
                MessageBox.Show("This is Last Bill");
            }
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            int Prev = Convert.ToInt32(lblInvoiceID.Text);
            if (Prev > 0)
            {
                Prev--;
                lblInvoiceID.Text = "00" + Prev.ToString();
                FillBilling();
            }
            else
            {
                MessageBox.Show("This is Last ID");
            }
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            string query = "select ISNULL(Max(ReturnId),0) as NewIvoiceID from tblCounterSaleReturn";

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

                        lblInvoiceID.Text = "00" + maxID.ToString();
                        FillBilling();
                    }
                    else
                    {
                        MessageBox.Show("Unable to fetch the maximum ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void CounterSaleOrPurchase_Load(object sender, EventArgs e)
        {
           
            DataTable dtViewPaymentMode = dlCoa.ViewChartOfAccountPaymentMode();
            cmbPaymentMode.DataSource = dtViewPaymentMode;
            cmbPaymentMode.ValueMember = "AccountId";
            cmbPaymentMode.DisplayMember = "AccountName";
            cmbPaymentMode.SelectedIndex = -1;

            cmbLocation.SelectedValueChanged -= new EventHandler(cmbLocation_SelectedValueChanged);
            DataTable dtStockLcation = DlWarehoues.View();
            cmbLocation.DataSource = dtStockLcation;
            cmbLocation.ValueMember = "WarehouseId";
            cmbLocation.DisplayMember = "WarehouseName";
            cmbLocation.SelectedIndex = -1;
            cmbLocation.SelectedValueChanged += new EventHandler(cmbLocation_SelectedValueChanged);

            //LoadProduct();
            //string query = "select ISNULL(max(InvoiceID),0) as NewIvoiceID from tblCounterSaleReturn";

            //using (SqlConnection connection = new SqlConnection(clsConnectionString.cs))
            //{
            //    using (SqlCommand command = new SqlCommand(query, connection))
            //    {
            //        connection.Open();

            //        // Execute the query and get the maximum ID
            //        object result = command.ExecuteScalar();

            //        // Check if the result is not null and is convertible to an integer
            //        if (result != null && int.TryParse(result.ToString(), out int maxID))
            //        {
            //            // Display the maximum ID in the TextBox
            //            maxID++;
            //            lblInvoiceID.Text = "00" + maxID.ToString();
            //            FillBilling();
            //        }
            //        else
            //        {
            //            MessageBox.Show("Unable to fetch the maximum ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        }
            //    }
            //}

        }
        private void LoadProduct(int InvoiceId)
        {
            try
            {

                // string query = "select ProductID, ProductName + ' ' + Cast(Pakaging as varchar(10))+ ' KG' as Product_Name,SaleRate from tblProduct";
                string query = string.Format("select i.CounterId,i.ProductId,Cast(i.Qty as int) as 'Qty',p.ProductName,i.Kg,i.Price,i.Weight,Cast(i.Price * i.Qty as int) as Total from tblCounterSaleOrPurchase  i inner join tblProduct p on p.ProductID = i.ProductId  where i.InvoiceID ={0} and i.IsDeleted = 0", InvoiceId);

                using (SqlConnection connection = new SqlConnection(clsConnectionString.cs))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        
                       
                        adapter.Fill(dt);



                        // Assuming you have a ComboBox control named "comboBox" in your form
                        dgvProduct.DataSource = dt;

                        dgvProduct.Columns[0].Visible = false; // 10% of the total width
                        dgvProduct.Columns[1].Visible = false; // 30% of the total width
                        dgvProduct.Columns[2].Width = (int)(dgvProduct.Width * 0.1); // 10% of the total width
                        dgvProduct.Columns[3].Width = (int)(dgvProduct.Width * 0.6); // 30% of the total width
                        dgvProduct.Columns[4].Visible = false;
                        dgvProduct.Columns[5].Width = (int)(dgvProduct.Width * 0.4);
                        dgvProduct.Columns[6].Visible = false;
                        dgvProduct.Columns[7].Visible = false;

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                // Handle exceptions or display an error message
                // ex.Message will contain the error message
            }
        }

        private void dgvProduct_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtPrice.Text = dgvProduct.SelectedRows[0].Cells["Price"].Value.ToString();
            txtPackaging.Text = dgvProduct.SelectedRows[0].Cells["Kg"].Value.ToString();
            txtQty.Focus();
        }

        private void txtQty_TextChanged(object sender, EventArgs e)
        {
            if (txtQty.Text != string.Empty)
            {
                CalculateTotalPrice();
            }
        }
        private void CalculateTotalPrice()
        {
            if (decimal.TryParse(txtPackaging.Text, out decimal kg) && decimal.TryParse(txtQty.Text, out decimal quantity))
            {
                decimal Weight = kg * quantity;
                txtWeight.Text = Weight.ToString();
                decimal TotalAmount = Weight * decimal.Parse(txtPrice.Text);
                txtTotal.Text = Convert.ToInt32(TotalAmount).ToString();
            }
            else
            {
                // If the entered values are not valid numbers, clear the total price TextBox
                txtTotal.Text = string.Empty;
            }
        }

        private void txtPrice_TextChanged(object sender, EventArgs e)
        {
            if (txtQty.Text != string.Empty)
            {
                CalculateTotalPrice();
            }
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // Perform your action here, for example, call a method or execute some code
                if (dgvProduct.Rows.Count > 0)
                {
                    dgvProduct.Rows[0].Selected = true;
                }

                // Optionally, you can suppress the Enter key to prevent a new line in the TextBox
                e.SuppressKeyPress = true;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
          
            DataView dvCleint = dt.DefaultView;
            dvCleint.RowFilter = "Product_Name like '%" + textBox1.Text + "%'";
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Down)
            {

                
                dgvProduct.Focus();
                
                // Prevent the Enter key from being processed further
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void dgvProduct_KeyDown(object sender, KeyEventArgs e)
        {
            if (dgvProduct.Rows.Count > 0)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtPrice.Text = dgvProduct.SelectedRows[0].Cells[2].Value.ToString();
                    txtQty.Focus();
                   
                }

            }
        }

        private void txtPrice_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtQty.Focus();
            }
        }

        private void txtQty_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnAdd_Click(btnAdd, null);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

        }

        private void btnFetch_Click(object sender, EventArgs e)
        {
            LoadProduct(int.Parse(txtCounterSale.Text));
        }

        private void cmbLocation_SelectedValueChanged(object sender, EventArgs e)
        {

        }

        private void txtPackaging_TextChanged(object sender, EventArgs e)
        {
            if (txtWeight.Text != string.Empty)
            {
                CalculateTotalPrice();
            }
        }
    }
}

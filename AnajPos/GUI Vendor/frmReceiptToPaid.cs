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
using AnajPos.BAL;
using AnajPos.DalVendor;
using AnajPos.BAL_Cash_BANK;
using AnajPos.DAL_Cash_Bank;

namespace AnajPos.GUI_Vendor
{
    public partial class frmReceiptToPaid : frmTemplete
    {
        public frmReceiptToPaid()
        {
            InitializeComponent();
        }
        clsZone cz = new clsZone();
        clsAddress DlAddress = new clsAddress();
        clsBlAddress BlAddress = new clsBlAddress();
        clsBlCustomer BlCustomer = new clsBlCustomer();
        clsCustomer DlCustomer = new clsCustomer();
        clsBlVendor BlVendor = new clsBlVendor();
        clsDlVendor DlVendor = new clsDlVendor();
        clsBLChartOfAccountLedger blCoaLedger = new clsBLChartOfAccountLedger();
        clsChartOfAccountLedger dlCoaLedger = new clsChartOfAccountLedger();

        private void Insert()
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionString.cs))
            {
                string query = string.Format("INSERT INTO tblReceiptToPaid (Id, Transdate, CZone, CArea, CustomerId, CRemark, CustomerPrevBalance, VZone, VArea, VendorId, VRemark, VendorPrevBalance, Amount) VALUES ({0}, '{1}', {2}, {3}, {4}, '{5}', {6}, {7}, {8}, {9}, '{10}', {11}, {12})",
                    int.Parse(txtId.Text),
                    dateTimePicker1.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                    (int)cmbCZone.SelectedValue,
                    (int)cmbCAddress.SelectedValue,
                    (int)cmbCustomer.SelectedValue,
                    txtCRemark.Text,
                    Convert.ToInt32(lblCustomerAmount.Text),
                    (int)cmbZone.SelectedValue,
                    (int)cmbAddress.SelectedValue,
                    (int)cmbVendor.SelectedValue,
                    txtVRemark.Text,
                    Convert.ToInt32(lblVendorAmount.Text),
                    Convert.ToInt32(txtAmount.Text)
                );

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    if (conn.State != ConnectionState.Open)
                    {
                        conn.Open();
                    }
                    int result = cmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        DateTime TransactionDate = dateTimePicker1.Value;
                        string TransactionType = "Receipt to Paid";
                        int TransactionId = int.Parse(txtId.Text);
                        int VendorId = (int)cmbVendor.SelectedValue;
                        string Remark = txtCRemark.Text;
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
                        using (SqlConnection conn1 = new SqlConnection(clsConnectionString.cs))
                        {
                            string q = string.Format("insert into tblTransactionVendor values ('{0}','{1}',{2},{3},'{4}',{5},{6},'{7}')", TransactionDate.ToString("yyyy-MM-dd"), TransactionType, TransactionId, VendorId, Remark, Dr, Cr, PostingDate.ToString("yyyy-MM-dd"));
                            using (SqlCommand cmd1 = new SqlCommand(q, conn1))
                            {

                                if (conn1.State != ConnectionState.Open)
                                {

                                    conn1.Open();
                                }
                                HowManyInsert = cmd1.ExecuteNonQuery();

                            }
                        }

                        TransactionType = "Receipt To Paid";
                        VendorId = (int)cmbCustomer.SelectedValue;

                        Dr = int.Parse(txtAmount.Text);
                        Cr = 0;
                        using (SqlConnection conn1 = new SqlConnection(clsConnectionString.cs))
                        {
                            string q = string.Format("insert into tblTransaction values ('{0}','{1}',{2},{3},'{4}',{5},{6},'{7}','{8}')", TransactionDate.ToString("yyyy-MM-dd"), TransactionType, TransactionId, VendorId, Remark, Dr, Cr, PostingDate.ToString("yyyy-MM-dd"),"آن لائن ٹرانفر");
                            using (SqlCommand cmd1 = new SqlCommand(q, conn1))
                            {

                                if (conn1.State != ConnectionState.Open)
                                {

                                    conn1.Open();
                                }
                                HowManyInsert = cmd1.ExecuteNonQuery();

                            }
                        }



                        // Dr Entry
                        blCoaLedger.TransactionDate = dateTimePicker1.Value;
                        blCoaLedger.TransactionType = "Create Receipt To Paid";
                        blCoaLedger.TransactionId = int.Parse(txtId.Text);
                        blCoaLedger.ChartOfAccountId = DlVendor.VendorAccountId((int)cmbVendor.SelectedValue);
                        blCoaLedger.Remark = txtVRemark.Text;
                        blCoaLedger.Dr = int.Parse(txtAmount.Text);
                        blCoaLedger.Cr = 0;
                        dlCoaLedger.Insert(blCoaLedger);

                        // Cr Entry
                        blCoaLedger.TransactionDate = dateTimePicker1.Value;
                        blCoaLedger.TransactionType = "Create Receipt To Paid";
                        blCoaLedger.TransactionId = int.Parse(txtId.Text);
                        blCoaLedger.ChartOfAccountId = DlCustomer.CustomerAccountId((int)cmbCustomer.SelectedValue);
                        blCoaLedger.Remark = txtCRemark.Text;
                        blCoaLedger.Dr = 0;
                        blCoaLedger.Cr = int.Parse(txtAmount.Text);
                        
                        dlCoaLedger.Insert(blCoaLedger);
                    }
                }
            }
        }
        private bool IsValidated()
        {
            if (cmbCustomer.Text == string.Empty && cmbCustomer.SelectedIndex == -1)
            {
                MessageBox.Show("Please Select Any Customer...");
                return false;
            }
            else if (cmbVendor.Text == string.Empty && cmbVendor.SelectedIndex == -1)
            {
                MessageBox.Show("Please Select Any Vendor...");
                return false;
            }
            else if (txtCRemark.Text == string.Empty)
            {
                MessageBox.Show("Please Enter Remark...");
                return false;
            }
            else if (txtVRemark.Text == string.Empty)
            {
                MessageBox.Show("Please Enter Reamrk");
                return false;
            }
            else if (txtAmount.Text == string.Empty)
            {
                MessageBox.Show("Please Enter Amount...");
                return false;
            }
            else if (txtId.Text == string.Empty)
            {
                MessageBox.Show("ID is Required...");
            }
            return true;
        }
        private void NewId()
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionString.cs))
            {
                using (SqlCommand cmd = new SqlCommand("select max(Id) from tblReceiptToPaid", conn))
                {
                    if (conn.State != ConnectionState.Open)
                    {
                        conn.Open();
                    }
                    try
                    {
                        int NewId = (int)cmd.ExecuteScalar();
                        NewId++;
                        txtId.Text = NewId.ToString();

                    }
                    catch
                    {
                        txtId.Text = "10001";

                    }

                }
            }
        }
        private void FillData(int Id)
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionString.cs))
            {
                string q = string.Format("select Id,Transdate,CZone,CArea,CustomerId,CRemark,CustomerPrevBalance,VZone,VArea,VendorId,VRemark,VendorPrevBalance,Amount from tblReceiptToPaid where Id ={0}", Id);
                SqlDataAdapter adpt = new SqlDataAdapter(q, conn);
                DataTable dt = new DataTable();
                adpt.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    txtId.Text = dr[0].ToString();
                    dateTimePicker1.Value = Convert.ToDateTime(dr[1]);
                    cmbCZone.SelectedValue = (int)dr[2];
                    cmbCAddress.SelectedValue = (int)dr[3];
                    cmbCustomer.SelectedValue = (int)dr[4];
                    txtCRemark.Text = dr[5].ToString();
                    lblCustomerAmount.Text = dr[6].ToString();
                    cmbZone.SelectedValue = (int)dr[7];
                    cmbAddress.SelectedValue = (int)dr[8];
                    cmbVendor.SelectedValue = (int)dr[9];
                    txtVRemark.Text = dr[10].ToString();
                    lblVendorAmount.Text = dr[11].ToString();
                    txtAmount.Text = dr[12].ToString();
                }
                else
                {
                    MessageBox.Show("This data is not exist in database");
                }
            }
        }
        private void UpdateRP()
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionString.cs))
            {
                using (SqlCommand cmd = new SqlCommand("usp_UpdateReceiptToPaid", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (conn.State != ConnectionState.Open)
                    {
                        conn.Open();
                    }
                    cmd.Parameters.AddWithValue("@Id", txtId.Text);
                    cmd.Parameters.AddWithValue("@EntryDate", dateTimePicker1.Value);
                    cmd.Parameters.AddWithValue("@CZone", (int)cmbCZone.SelectedValue);
                    cmd.Parameters.AddWithValue("@CAddress", (int)cmbCAddress.SelectedValue);
                    cmd.Parameters.AddWithValue("@CustomerId", (int)cmbCustomer.SelectedValue);
                    cmd.Parameters.AddWithValue("@CRemark", txtCRemark.Text);
                    cmd.Parameters.AddWithValue("@CLastBalance", lblCustomerAmount.Text);
                    cmd.Parameters.AddWithValue("@VZone", (int)cmbZone.SelectedValue);
                    cmd.Parameters.AddWithValue("@VAddress", (int)cmbAddress.SelectedValue);
                    cmd.Parameters.AddWithValue("@VendorId", (int)cmbVendor.SelectedValue);
                    cmd.Parameters.AddWithValue("@VRemark", txtVRemark.Text);
                    cmd.Parameters.AddWithValue("@VLastBalance", lblVendorAmount.Text);
                    cmd.Parameters.AddWithValue("@Amount", txtAmount.Text);
                    int res = cmd.ExecuteNonQuery();
                    if (res > 0)
                    {


                        DateTime TransactionDate = dateTimePicker1.Value;
                       // string TransactionType = "Receipt to Paid";
                        int TransactionId = int.Parse(txtId.Text);
                        int VendorId = (int)cmbVendor.SelectedValue;
                        string Remark = txtCRemark.Text;
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
                        using (SqlConnection conn1 = new SqlConnection(clsConnectionString.cs))
                        {
                            string q = string.Format("update tblTransactionVendor set TransactionDate	= '{0}', VendorId = {1},Remark = '{2}',Dr = {3},Cr = {4} where LedgerType = 'Receipt to Paid' and TranId = {5}", TransactionDate.ToString("yyyy-MM-dd"), VendorId, Remark, Dr, Cr, TransactionId);
                            using (SqlCommand cmd1 = new SqlCommand(q, conn1))
                            {

                                if (conn1.State != ConnectionState.Open)
                                {

                                    conn1.Open();
                                }
                                HowManyInsert = cmd1.ExecuteNonQuery();

                            }
                        }

                        //TransactionType = "Receipt To Paid";
                        VendorId = (int)cmbCustomer.SelectedValue;

                        Dr = int.Parse(txtAmount.Text);
                        Cr = 0;
                        using (SqlConnection conn1 = new SqlConnection(clsConnectionString.cs))
                        {
                            string q = string.Format("UPDATE tblTransaction SET TransactionDate = '{0}', CustomerName = {1}, Remark = '{2}', Dr = {3}, Cr = {4} WHERE TranType = 'Receipt to Paid' AND TranId = {5}",
                             TransactionDate.ToString("yyyy-MM-dd"), VendorId, Remark, Dr, Cr, TransactionId);

                            using (SqlCommand cmd1 = new SqlCommand(q, conn1))
                            {

                                if (conn1.State != ConnectionState.Open)
                                {

                                    conn1.Open();
                                }
                                HowManyInsert = cmd1.ExecuteNonQuery();

                            }
                        }




                        // Dr Entry
                        blCoaLedger.TransactionDate = dateTimePicker1.Value;
                        blCoaLedger.TransactionType = "Create Receipt To Paid";
                        blCoaLedger.TransactionId = int.Parse(txtId.Text);
                        blCoaLedger.ChartOfAccountId = DlVendor.VendorAccountId((int)cmbVendor.SelectedValue);
                        blCoaLedger.Remark = txtVRemark.Text;
                        blCoaLedger.Dr = int.Parse(txtAmount.Text);
                        blCoaLedger.Cr = 0;
                        dlCoaLedger.UpdateLedger(blCoaLedger);

                        // Cr Entry
                        blCoaLedger.TransactionDate = dateTimePicker1.Value;
                        blCoaLedger.TransactionType = "Create Receipt To Paid";
                        blCoaLedger.TransactionId = int.Parse(txtId.Text);
                        blCoaLedger.ChartOfAccountId = DlCustomer.CustomerAccountId((int)cmbCustomer.SelectedValue);
                        blCoaLedger.Remark = txtVRemark.Text;
                        blCoaLedger.Dr = 0;
                        blCoaLedger.Cr = int.Parse(txtAmount.Text);
                        dlCoaLedger.UpdateLedger(blCoaLedger);
                    }

                }
            }
        }

        private void FirstId()
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionString.cs))
            {
                using (SqlCommand cmd = new SqlCommand("select min(Id) from tblReceiptToPaid", conn))
                {
                    if (conn.State != ConnectionState.Open)
                    {
                        conn.Open();
                    }
                    int FirstId = 0;
                    try
                    {
                        FirstId = (int)cmd.ExecuteScalar();
                    }
                    catch
                    {

                    }

                    lblFirst.Text = FirstId.ToString();
                }
            }
        }
        private void LastId()
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionString.cs))
            {
                using (SqlCommand cmd = new SqlCommand("select max(Id) from tblReceiptToPaid", conn))
                {
                    if (conn.State != ConnectionState.Open)
                    {
                        conn.Open();
                    }
                    int LastId = 0;
                    try
                    {
                        LastId = (int)cmd.ExecuteScalar();
                    }
                    catch
                    {

                    }
                    lblLast.Text = LastId.ToString();
                }
            }
        }
        private void EnableField()
        {
            txtId.Enabled = true;
            dateTimePicker1.Enabled = true;
            cmbCZone.Enabled = true;
            cmbCAddress.Enabled = true;
            cmbCustomer.Enabled = true;
            txtCRemark.Enabled = true;
            cmbZone.Enabled = true;
            cmbAddress.Enabled = true;
            cmbVendor.Enabled = true;
            txtVRemark.Enabled = true;
            txtAmount.Enabled = true;
        }
        private void DisableField()
        {
            txtId.Enabled = false;
            dateTimePicker1.Enabled = false;
            cmbCZone.Enabled = false;
            cmbCAddress.Enabled = false;
            cmbCustomer.Enabled = false;
            txtCRemark.Enabled = false;
            cmbZone.Enabled = false;
            cmbAddress.Enabled = false;
            cmbVendor.Enabled = false;
            txtVRemark.Enabled = false;
            txtAmount.Enabled = false;
        }
        private void ClearField()
        {
            cmbZone.SelectedIndex = -1;
            cmbCZone.SelectedIndex = -1;
            txtId.Text = string.Empty;
            dateTimePicker1.Value = DateTime.Now;
            cmbCAddress.DataSource = null;
            cmbCAddress.Text = string.Empty;
            cmbCustomer.DataSource = null;
            cmbCustomer.Text = string.Empty;
            txtCRemark.Text = string.Empty;
            lblCustomerAmount.Text = string.Empty;
            cmbAddress.DataSource = null;
            cmbAddress.Text = string.Empty;
            cmbVendor.DataSource = null;
            cmbVendor.Text = string.Empty;
            txtVRemark.Text = string.Empty;
            lblVendorAmount.Text = string.Empty;
            txtAmount.Text = string.Empty;
        }
        private void frmReceiptToPaid_Load(object sender, EventArgs e)
        {
            cmbZone.SelectedValueChanged -= new EventHandler(cmbZone_SelectedValueChanged);
            DataTable dtView = cz.ViewZone();
            cmbZone.DataSource = dtView;
            cmbZone.ValueMember = "Id";
            cmbZone.DisplayMember = "Name of Zone";
            cmbZone.SelectedIndex = -1;
            cmbZone.SelectedValueChanged += new EventHandler(cmbZone_SelectedValueChanged);


            cmbCZone.SelectedValueChanged -= new EventHandler(cmbCZone_SelectedValueChanged);
            DataTable dtView1 = cz.ViewZone();
            cmbCZone.DataSource = dtView1;
            cmbCZone.ValueMember = "Id";
            cmbCZone.DisplayMember = "Name of Zone";
            cmbCZone.SelectedIndex = -1;
            cmbCZone.SelectedValueChanged += new EventHandler(cmbCZone_SelectedValueChanged);

            DisableField();
            FirstId();
            LastId();
            lblCurrent.Text = lblFirst.Text;
            FillData(int.Parse(lblFirst.Text));
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
                cmbVendor.Text = string.Empty;

            }
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            if (IsValidated())
            {
                UpdateRP();
                MessageBox.Show("Your Data has been Updated....");
                btnFirst.Enabled = btnNext.Enabled = btnPrevious.Enabled = btnLast.Enabled = true;
                btnModify.Enabled = false;
                DisableField();
                lblCurrent.Text = txtId.Text;
            }


        }

        private void cmbCZone_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cmbCZone.SelectedIndex != -1)
            {
                BlAddress.AddressId = (int)cmbCZone.SelectedValue;
                cmbCAddress.DataSource = DlAddress.ViewAddressByZone(BlAddress);
                cmbCAddress.DisplayMember = "AddressName";
                cmbCAddress.ValueMember = "AddressID";
                cmbCAddress.SelectedIndex = -1;
                cmbCustomer.Text = string.Empty;

            }
        }

        private void cmbCAddress_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                BlCustomer.AddressId = Convert.ToInt32(cmbCAddress.SelectedValue);
                DataTable dt = DlCustomer.ViewByAddress(BlCustomer);
                cmbCustomer.DataSource = dt;
                cmbCustomer.DisplayMember = "CustomerName";
                cmbCustomer.ValueMember = "CustomerId";
                //  cmbCustomer.SelectedIndex = -1;
            }
            catch
            { }
        }

        private void cmbAddress_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                BlVendor.AddressName = Convert.ToInt32(cmbAddress.SelectedValue);
                DataTable dt = DlVendor.ViewByAddress(BlVendor);
                cmbVendor.DataSource = dt;
                cmbVendor.DisplayMember = "VendorName";
                cmbVendor.ValueMember = "VendorID";
                //  cmbCustomer.SelectedIndex = -1;
            }
            catch
            { }
        }

        private void cmbCustomer_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbCustomer.SelectedValue != null)
                {
                    lblCustomerAmount.Text = DlCustomer.CustomerLastBalance((int)cmbCustomer.SelectedValue).ToString(); 
                }
            }
            catch
            {


            }
        }

        private void cmbVendor_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbVendor.SelectedValue != null)
                {
                    lblVendorAmount.Text = DlVendor.VendorLastBalance((int)cmbVendor.SelectedValue).ToString(); 
                }
            }
            catch
            {


            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            ClearField();
            NewId();
            btnNew.Enabled = false;
            btnSave.Enabled = true;
            EnableField();
            dateTimePicker1.Focus();

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (IsValidated())
            {
                Insert();
                btnSave.Enabled = false;
                btnNew.Enabled = true;
                LastId();
                lblCurrent.Text = lblLast.Text;
                DisableField();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FillData(int.Parse(txtSearch.Text));
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            FillData(int.Parse(lblLast.Text));
            lblCurrent.Text = lblLast.Text;
        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            FillData(int.Parse(lblFirst.Text));
            lblCurrent.Text = lblCurrent.Text;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (int.Parse(lblCurrent.Text) < int.Parse(lblLast.Text))
            {
                int NextId = int.Parse(lblCurrent.Text);
                NextId++;
                lblCurrent.Text = NextId.ToString();
                FillData(int.Parse(lblCurrent.Text));
            }
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            if (int.Parse(lblCurrent.Text) > int.Parse(lblFirst.Text))
            {
                int Previous = int.Parse(lblCurrent.Text);
                Previous--;
                lblCurrent.Text = Previous.ToString();
                FillData(int.Parse(lblCurrent.Text));
            }
        }

        private void alterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnModify.Enabled = true;
            EnableField();
            btnFirst.Enabled = btnNext.Enabled = btnPrevious.Enabled = btnLast.Enabled = false;
        }

        private void frmReceiptToPaid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                // MessageBox.Show("apka kam ho rha hai");
                //Coke.Click += new EventHandler(btnNew_Click);
                //Coke.PerformClick();
                if (btnNew.Enabled == true)
                {
                    btnNew_Click(null, new EventArgs());
                }

            }
            else if (e.KeyCode == Keys.F2)
            {
                if (btnSave.Enabled == true)
                {
                    btnSave_Click(null, new EventArgs());
                }
                else
                {
                    MessageBox.Show("Please Create the Customer");
                }
            }
            else if (e.KeyCode == Keys.F3)
            {
                if (btnModify.Enabled == true)
                {
                    btnModify_Click(null, new EventArgs());
                }
                else
                {
                    MessageBox.Show("Please Alteration Mode On");
                }
            }
            else if (e.KeyCode == Keys.F5)
            {
                if (btnView.Enabled == true)
                {
                    btnView_Click(null, new EventArgs());
                }

            }
            else if (e.KeyCode == Keys.F6)
            {
                if (btnFirst.Enabled == true)
                {
                    btnFirst_Click(null, new EventArgs());
                }
            }
            else if (e.KeyCode == Keys.F7)
            {
                if (btnNext.Enabled == true)
                {
                    btnNext_Click(null, new EventArgs());
                }

            }
            else if (e.KeyCode == Keys.F8)
            {
                if (btnPrevious.Enabled == true)
                {
                    btnPrevious_Click(null, new EventArgs());
                }

            }
            else if (e.KeyCode == Keys.F9)
            {
                if (btnLast.Enabled == true)
                {
                    btnLast_Click(null, new EventArgs());
                }
            }
            //else if (e.KeyCode == Keys.F10)
            //{
            //    if (pbCustomer.Enabled == true)
            //    {
            //        pbCustomer_Click(null, new EventArgs());
            //    }
            //}
            //else if (e.KeyCode == Keys.Escape)
            //{
            //    this.Close();
            //}
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            frmViewReceiptToPaid rp = new frmViewReceiptToPaid();
            rp.ShowDialog();
        }

        private void dateTimePicker1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                cmbCZone.Focus();
                e.SuppressKeyPress = true;
            }
        }

        private void cmbCZone_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.SelectNextControl((Control)sender, true, true, true, true);
                e.SuppressKeyPress = true;
            }
        }

        private void txtAmount_KeyPress(object sender, KeyPressEventArgs e)
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
        }

    }
}

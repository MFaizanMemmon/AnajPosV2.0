using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using AnajPos.DAL;

namespace AnajPos.GUI
{
    public partial class frmGridLedger : frmTemplete
    {
        public frmGridLedger()
        {
            InitializeComponent();
        }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        private void frmGridLedger_Load(object sender, EventArgs e)
        {
            label1.Text = CustomerName;
            dataGridView1.ColumnCount = 7;
            dataGridView1.Columns[0].Name = "Date";
            dataGridView1.Columns[1].Name = "Type";
            dataGridView1.Columns[2].Name = "Id";
            dataGridView1.Columns[3].Name = "Remark";
            dataGridView1.Columns[4].Name = "Dr";
            dataGridView1.Columns[5].Name = "Cr";
            dataGridView1.Columns[6].Name = "Balance";

            using (SqlConnection conn = new SqlConnection(clsConnectionString.cs))
            {
                string q = string.Format("select TransactionDate,TranType,TranID,Remark,Dr,Cr from tblTransaction where CustomerName = {0} order by TransactionDate", CustomerId);
                using (SqlCommand cmd = new SqlCommand(q, conn))
                {
                    //cmd.CommandType = CommandType.StoredProcedure;
                    if (conn.State != ConnectionState.Open)
                    {
                        conn.Open();
                    }
                    SqlDataReader sdr = cmd.ExecuteReader();
                    string[] arr = new string[7];
                    int Dr, Cr = 0;
                    int PreviousBalance = 0;
                    while (sdr.Read())
                    {
                        int Balance = 0;
                        Balance = PreviousBalance;
                        arr[0] = sdr[0].ToString();
                        arr[1] = sdr[1].ToString();
                        arr[2] = sdr[2].ToString();
                        arr[3] = sdr[3].ToString();
                        arr[4] = sdr[4].ToString();
                        arr[5] = sdr[5].ToString();

                        Dr = (sdr[4].ToString() == string.Empty) ? 0 : Convert.ToInt32(sdr[4]);
                        Cr = (sdr[5].ToString() == string.Empty) ? 0 : Convert.ToInt32(sdr[5]);
                        Balance += Cr - Dr;
                        PreviousBalance = Balance;
                        arr[6] = Balance.ToString();
                        dataGridView1.Rows.Add(arr);
                    }
                    //MessageBox.Show(arr[0]);
                }
            }
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            int getRow = dataGridView1.Rows.GetFirstRow(DataGridViewElementStates.Selected);
            if (dataGridView1.Rows[getRow].Cells[1].Value.ToString()=="Invoice")
            {
                string Id = dataGridView1.Rows[getRow].Cells[2].Value.ToString();
                Reports.frmPrintInvoice pi= new Reports.frmPrintInvoice();
                pi.Id = int.Parse(Id);
                pi.ShowDialog();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Reports.frmLedger fl = new Reports.frmLedger();
            fl.CustomerId = CustomerId;
            fl.ShowDialog();
        }
    }
}

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
    public partial class frmBackup : frmTemplete
    {
        public frmBackup()
        {
            InitializeComponent();
        }

        private void frmBackup_Load(object sender, EventArgs e)
        {
            this.Focus();
        }

      

        private void btnBrows_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                txtFilePath.Text = fbd.SelectedPath;
                btnUpload.Enabled = true;
            }
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn=new SqlConnection(DAL.clsConnectionString.cs))
            {
                string q = "backup database AnajPos to disk = '"+ txtFilePath.Text+  "\\AnajPos -" + DateTime.Now.Ticks.ToString() +".bak' ";
                using (SqlCommand cmd=new SqlCommand(q,conn))
                {
                    if (conn.State != ConnectionState.Open)
                    {
                        conn.Open();
                    }
                    cmd.CommandTimeout = 0;
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception)
                    {

                        MessageBox.Show("Please Change your Drive for backup data");
                    }
                    
                    MessageBox.Show("Your database backup has been taken successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            this.Close();
        }

        private void frmBackup_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                btnBrows_Click(null, new EventArgs());
            }
            else if (e.KeyCode == Keys.F2)
            {
                btnUpload_Click(null, new EventArgs());
            }
        }

      
    }
}

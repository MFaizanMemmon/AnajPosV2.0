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
    public partial class frmRestore : frmTemplete
    {
        public frmRestore()
        {
            InitializeComponent();
        }

        private void frmRestore_Load(object sender, EventArgs e)
        {

        }

        private void btnBrows_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                txtFilePath.Text = ofd.FileName;
                btnUpload.Enabled = true;
            }
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn=new SqlConnection(DAL.clsConnectionString.cs))
            {
                string q = "use master; alter database AnajPos set single_user with rollback immediate; ";
                q += "restore database AnajPos from disk = '"+txtFilePath.Text+"'With Replace";
                using (SqlCommand cmd=new SqlCommand(q,conn))
                {
                    if (conn.State != ConnectionState.Open)
                    {
                        conn.Open();
                    }
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch 
                    {

                        MessageBox.Show("Please backup file copy another folder again restore your work will be done...");
                    }
                    
                    MessageBox.Show("Your database Restore has been taken successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
             
                }
            }
            this.Close();
        }
    }
}

using AnajPos.BALUsers;
using AnajPos.DALUsers;
using AnajPos.GUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnajPos.Users
{
    public partial class frmRoles : frmTemplete
    {
        clsBLRoles bLRoles= new clsBLRoles();
        clsRoles dlRole = new clsRoles();
        public frmRoles()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (IsValid())
            {
                bLRoles.RoleName = txtRole.Text;
                if (dlRole.Insert(bLRoles) > 0)
                {
                    MessageBox.Show("Data Successful");
                    txtRole.Clear();
                    dgvUnit.DataSource = dlRole.View();
                }
            }
        }

        private bool IsValid()
        {

            if (txtRole.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Please Enter Role !! ");
                txtRole.Focus();
                return false;
            }
            return true;
        }

        private void frmRoles_Load(object sender, EventArgs e)
        {
            dgvUnit.DataSource = dlRole.View();
        }
    }
}

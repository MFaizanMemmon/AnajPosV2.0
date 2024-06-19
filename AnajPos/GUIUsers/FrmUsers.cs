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
using System.Web.Security;
using System.Windows.Forms;

namespace AnajPos.GUIUsers
{
    public partial class FrmUsers : frmTemplete
    {
        clsBLUsers bLUsers = new clsBLUsers();
        clsUsers dlUsers = new clsUsers();
        public FrmUsers()
        {
            InitializeComponent();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (IsValid())
            {
                bLUsers.UserName = txtUserName.Text;
                bLUsers.Phone = txtPhone.Text;
                bLUsers.UserEmail = txtEmail.Text;
                if (dlUsers.Insert(bLUsers) > 0)
                {
                    MessageBox.Show("Data Successful");
                    txtUserName.Clear();
                    txtPhone.Clear();
                    txtEmail.Clear();
                }
            }
        }
        private bool IsValid()
        {
            if (txtUserName.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Please Enter Role !! ");
                txtUserName.Focus();
                txtPhone.Focus();
                txtEmail.Focus();
                return false;
            }
            return true;
        }

    }
}

﻿using System;
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

namespace AnajPos.GUI
{
    public partial class frmViewCustomer : frmTemplete
    {
      
        clsZone cz = new clsZone();
        clsAddress DlAddress = new clsAddress();
        clsBlAddress BlAddress = new clsBlAddress();
        clsCustomer DlCustomer = new clsCustomer();
        clsBlCustomer BlCustomer = new clsBlCustomer();
        public frmViewCustomer()
        {
            InitializeComponent();
        }
        DataTable dt = new DataTable();
        private void frmViewCustomer_Load(object sender, EventArgs e)
        {
            cmbZone.SelectedIndexChanged -= new EventHandler(cmbZone_SelectedIndexChanged);
            cmbZone.SelectedValueChanged -= new EventHandler(cmbZone_SelectedValueChanged);
            DataTable dtView = cz.ViewZone();
            cmbZone.DataSource = dtView;
            cmbZone.ValueMember = "Id";
            cmbZone.DisplayMember = "Name of Zone";
            cmbZone.SelectedIndex = -1;
            cmbZone.SelectedValueChanged += new EventHandler(cmbZone_SelectedValueChanged);
            dt = DlCustomer.DisplayCustomer();
            dgViewCustomer.DataSource = dt;
            cmbZone.SelectedIndexChanged += new EventHandler(cmbZone_SelectedIndexChanged);
        }

        private void cmbZone_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cmbZone.SelectedIndex != -1)
            {
                BlAddress.AddressId = (int)cmbZone.SelectedValue;
                cmbAddress.DataSource = DlAddress.ViewAddressByZone(BlAddress);
                cmbAddress.DisplayMember = "AddressName";
                cmbAddress.ValueMember = "AddressID";
            }
        }

        private void cmbZone_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbAddress.SelectedIndexChanged -= new EventHandler(cmbAddress_SelectedIndexChanged);
            DataView dvZone = dt.DefaultView;
            dvZone.RowFilter = "Area like'%" + cmbZone.Text + "%'";
            cmbAddress.SelectedIndexChanged += new EventHandler(cmbAddress_SelectedIndexChanged);
        }

        private void cmbAddress_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataView dvAddress = dt.DefaultView;
            dvAddress.RowFilter = "Address like '%"+cmbAddress.Text+"'";
        }

        private void cmbCustomer_TextChanged(object sender, EventArgs e)
        {
            DataView dvCleint=dt.DefaultView;
            dvCleint.RowFilter = "Client like '%" + txtClient.Text + "%'";
        }
    }
}

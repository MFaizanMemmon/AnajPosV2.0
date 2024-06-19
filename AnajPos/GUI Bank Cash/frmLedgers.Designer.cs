
namespace AnajPos.GUI_Bank_Cash
{
    partial class frmLedgers
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLedgers));
            this.cmbChartOfAccounts = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dvgViewLedgers = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.dptStartDate = new System.Windows.Forms.DateTimePicker();
            this.dptEndDate = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.btnAddProduct = new System.Windows.Forms.Button();
            this.btnPrintInvoice = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dvgViewLedgers)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbChartOfAccounts
            // 
            this.cmbChartOfAccounts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbChartOfAccounts.FormattingEnabled = true;
            this.cmbChartOfAccounts.Location = new System.Drawing.Point(12, 38);
            this.cmbChartOfAccounts.Name = "cmbChartOfAccounts";
            this.cmbChartOfAccounts.Size = new System.Drawing.Size(280, 28);
            this.cmbChartOfAccounts.TabIndex = 7;
            this.cmbChartOfAccounts.SelectedValueChanged += new System.EventHandler(this.cmbChartOfAccounts_SelectedValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 20);
            this.label1.TabIndex = 6;
            this.label1.Text = "Accounts";
            // 
            // dvgViewLedgers
            // 
            this.dvgViewLedgers.AllowUserToAddRows = false;
            this.dvgViewLedgers.AllowUserToDeleteRows = false;
            this.dvgViewLedgers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dvgViewLedgers.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dvgViewLedgers.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.dvgViewLedgers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dvgViewLedgers.Location = new System.Drawing.Point(12, 72);
            this.dvgViewLedgers.Name = "dvgViewLedgers";
            this.dvgViewLedgers.ReadOnly = true;
            this.dvgViewLedgers.RowHeadersWidth = 62;
            this.dvgViewLedgers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dvgViewLedgers.Size = new System.Drawing.Size(1161, 399);
            this.dvgViewLedgers.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(325, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 20);
            this.label2.TabIndex = 8;
            this.label2.Text = "Start Date";
            // 
            // dptStartDate
            // 
            this.dptStartDate.CustomFormat = "dd-MMM-yyyy";
            this.dptStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dptStartDate.Location = new System.Drawing.Point(323, 39);
            this.dptStartDate.Name = "dptStartDate";
            this.dptStartDate.Size = new System.Drawing.Size(165, 27);
            this.dptStartDate.TabIndex = 27;
            // 
            // dptEndDate
            // 
            this.dptEndDate.CustomFormat = "dd-MMM-yyyy";
            this.dptEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dptEndDate.Location = new System.Drawing.Point(513, 39);
            this.dptEndDate.Name = "dptEndDate";
            this.dptEndDate.Size = new System.Drawing.Size(160, 27);
            this.dptEndDate.TabIndex = 29;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(516, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 20);
            this.label3.TabIndex = 28;
            this.label3.Text = "End Date";
            // 
            // btnAddProduct
            // 
            this.btnAddProduct.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddProduct.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnAddProduct.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddProduct.ForeColor = System.Drawing.Color.Black;
            this.btnAddProduct.Image = ((System.Drawing.Image)(resources.GetObject("btnAddProduct.Image")));
            this.btnAddProduct.Location = new System.Drawing.Point(933, 20);
            this.btnAddProduct.Name = "btnAddProduct";
            this.btnAddProduct.Size = new System.Drawing.Size(117, 43);
            this.btnAddProduct.TabIndex = 30;
            this.btnAddProduct.Text = "Apply";
            this.btnAddProduct.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAddProduct.UseVisualStyleBackColor = false;
            this.btnAddProduct.Click += new System.EventHandler(this.btnAddProduct_Click);
            // 
            // btnPrintInvoice
            // 
            this.btnPrintInvoice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrintInvoice.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnPrintInvoice.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrintInvoice.ForeColor = System.Drawing.Color.Black;
            this.btnPrintInvoice.Image = ((System.Drawing.Image)(resources.GetObject("btnPrintInvoice.Image")));
            this.btnPrintInvoice.Location = new System.Drawing.Point(1056, 20);
            this.btnPrintInvoice.Name = "btnPrintInvoice";
            this.btnPrintInvoice.Size = new System.Drawing.Size(117, 43);
            this.btnPrintInvoice.TabIndex = 87;
            this.btnPrintInvoice.Text = "Print";
            this.btnPrintInvoice.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnPrintInvoice.UseVisualStyleBackColor = false;
            this.btnPrintInvoice.Click += new System.EventHandler(this.btnPrintInvoice_Click);
            // 
            // frmLedgers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1182, 483);
            this.Controls.Add(this.btnPrintInvoice);
            this.Controls.Add(this.btnAddProduct);
            this.Controls.Add(this.dptEndDate);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dptStartDate);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbChartOfAccounts);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dvgViewLedgers);
            this.Name = "frmLedgers";
            this.Text = "frmLedgers";
            this.Load += new System.EventHandler(this.frmLedgers_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dvgViewLedgers)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbChartOfAccounts;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dvgViewLedgers;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dptStartDate;
        private System.Windows.Forms.DateTimePicker dptEndDate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnAddProduct;
        private System.Windows.Forms.Button btnPrintInvoice;
    }
}
namespace AnajPos.GUI
{
    partial class frmSearchProduct
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
            this.dgViewProduct = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgViewProduct)).BeginInit();
            this.SuspendLayout();
            // 
            // dgViewProduct
            // 
            this.dgViewProduct.AllowUserToAddRows = false;
            this.dgViewProduct.AllowUserToDeleteRows = false;
            this.dgViewProduct.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgViewProduct.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.dgViewProduct.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgViewProduct.Location = new System.Drawing.Point(12, 59);
            this.dgViewProduct.MultiSelect = false;
            this.dgViewProduct.Name = "dgViewProduct";
            this.dgViewProduct.ReadOnly = true;
            this.dgViewProduct.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgViewProduct.Size = new System.Drawing.Size(827, 202);
            this.dgViewProduct.TabIndex = 1;
            this.dgViewProduct.DoubleClick += new System.EventHandler(this.dgViewProduct_DoubleClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(12, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Search";
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(71, 16);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(416, 27);
            this.txtSearch.TabIndex = 0;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // frmSearchProduct
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(851, 273);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgViewProduct);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(5, 8, 5, 8);
            this.Name = "frmSearchProduct";
            this.Text = "Product Screen";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmSearchProduct_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmSearchProduct_FormClosed);
            this.Load += new System.EventHandler(this.frmSearchProduct_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmSearchProduct_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.dgViewProduct)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgViewProduct;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSearch;
    }
}
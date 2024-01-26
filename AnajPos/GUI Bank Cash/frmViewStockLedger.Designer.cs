namespace AnajPos.GUI_Bank_Cash
{
    partial class frmViewStockLedger
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
            this.dgViewStock = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgViewStock)).BeginInit();
            this.SuspendLayout();
            // 
            // dgViewStock
            // 
            this.dgViewStock.AllowUserToAddRows = false;
            this.dgViewStock.AllowUserToDeleteRows = false;
            this.dgViewStock.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgViewStock.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.dgViewStock.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgViewStock.Location = new System.Drawing.Point(12, 12);
            this.dgViewStock.Name = "dgViewStock";
            this.dgViewStock.ReadOnly = true;
            this.dgViewStock.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgViewStock.Size = new System.Drawing.Size(940, 441);
            this.dgViewStock.TabIndex = 2;
            // 
            // frmViewStockLedger
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(964, 482);
            this.Controls.Add(this.dgViewStock);
            this.Name = "frmViewStockLedger";
            this.Text = "Stock Ledger";
            this.Load += new System.EventHandler(this.frmViewStockLedger_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgViewStock)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgViewStock;
    }
}
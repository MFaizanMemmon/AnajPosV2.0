namespace AnajPos.GUI_Bank_Cash
{
    partial class frmViewBank
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
            this.dgViewCustomer = new System.Windows.Forms.DataGridView();
            this.label3 = new System.Windows.Forms.Label();
            this.txtClient = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgViewCustomer)).BeginInit();
            this.SuspendLayout();
            // 
            // dgViewCustomer
            // 
            this.dgViewCustomer.AllowUserToAddRows = false;
            this.dgViewCustomer.AllowUserToDeleteRows = false;
            this.dgViewCustomer.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgViewCustomer.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.dgViewCustomer.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgViewCustomer.Location = new System.Drawing.Point(24, 52);
            this.dgViewCustomer.Name = "dgViewCustomer";
            this.dgViewCustomer.ReadOnly = true;
            this.dgViewCustomer.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgViewCustomer.Size = new System.Drawing.Size(974, 437);
            this.dgViewCustomer.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(20, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 20);
            this.label3.TabIndex = 8;
            this.label3.Text = "Search";
            // 
            // txtClient
            // 
            this.txtClient.Location = new System.Drawing.Point(98, 19);
            this.txtClient.Name = "txtClient";
            this.txtClient.Size = new System.Drawing.Size(900, 27);
            this.txtClient.TabIndex = 7;
            this.txtClient.TextChanged += new System.EventHandler(this.txtClient_TextChanged);
            // 
            // frmViewBank
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1023, 501);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtClient);
            this.Controls.Add(this.dgViewCustomer);
            this.Name = "frmViewBank";
            this.Text = "frmViewBank";
            this.Load += new System.EventHandler(this.frmViewBank_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgViewCustomer)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgViewCustomer;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtClient;
    }
}
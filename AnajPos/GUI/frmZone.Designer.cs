namespace AnajPos.GUI
{
    partial class frmZone
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmZone));
            this.label1 = new System.Windows.Forms.Label();
            this.txtZone = new System.Windows.Forms.TextBox();
            this.dgViewZone = new System.Windows.Forms.DataGridView();
            this.btnSave = new System.Windows.Forms.Button();
            this.txtZoneUrdu = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgViewZone)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(25, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Zone";
            // 
            // txtZone
            // 
            this.txtZone.Location = new System.Drawing.Point(92, 24);
            this.txtZone.Name = "txtZone";
            this.txtZone.Size = new System.Drawing.Size(210, 27);
            this.txtZone.TabIndex = 0;
            this.txtZone.TextChanged += new System.EventHandler(this.txtZone_TextChanged);
            this.txtZone.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtZone_KeyDown);
            // 
            // dgViewZone
            // 
            this.dgViewZone.AllowUserToAddRows = false;
            this.dgViewZone.AllowUserToDeleteRows = false;
            this.dgViewZone.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgViewZone.BackgroundColor = System.Drawing.SystemColors.ActiveCaption;
            this.dgViewZone.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgViewZone.Location = new System.Drawing.Point(308, 24);
            this.dgViewZone.Name = "dgViewZone";
            this.dgViewZone.ReadOnly = true;
            this.dgViewZone.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgViewZone.Size = new System.Drawing.Size(312, 277);
            this.dgViewZone.TabIndex = 3;
            this.dgViewZone.DoubleClick += new System.EventHandler(this.dgViewZone_DoubleClick);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.ForeColor = System.Drawing.Color.Black;
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.Location = new System.Drawing.Point(162, 57);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(140, 41);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Save";
            this.btnSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            this.btnSave.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtZone_KeyDown);
            // 
            // txtZoneUrdu
            // 
            this.txtZoneUrdu.Font = new System.Drawing.Font("Jameel Noori Nastaleeq", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtZoneUrdu.Location = new System.Drawing.Point(279, 236);
            this.txtZoneUrdu.Name = "txtZoneUrdu";
            this.txtZoneUrdu.Size = new System.Drawing.Size(10, 43);
            this.txtZoneUrdu.TabIndex = 1;
            this.txtZoneUrdu.Visible = false;
            this.txtZoneUrdu.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtZone_KeyDown);
            // 
            // frmZone
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(632, 313);
            this.Controls.Add(this.txtZoneUrdu);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.dgViewZone);
            this.Controls.Add(this.txtZone);
            this.Controls.Add(this.label1);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(5, 8, 5, 8);
            this.Name = "frmZone";
            this.Text = "frmZone";
            this.Load += new System.EventHandler(this.frmZone_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmZone_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.dgViewZone)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtZone;
        private System.Windows.Forms.DataGridView dgViewZone;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox txtZoneUrdu;
    }
}
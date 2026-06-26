namespace Products
{
    partial class ProductRevisions
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProductRevisions));
            this.dgvRevision = new System.Windows.Forms.DataGridView();
            this.btnClose = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRevision)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvRevision
            // 
            this.dgvRevision.AllowUserToAddRows = false;
            this.dgvRevision.AllowUserToDeleteRows = false;
            this.dgvRevision.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRevision.Location = new System.Drawing.Point(0, 0);
            this.dgvRevision.Name = "dgvRevision";
            this.dgvRevision.ReadOnly = true;
            this.dgvRevision.RowHeadersVisible = false;
            this.dgvRevision.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.dgvRevision.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvRevision.Size = new System.Drawing.Size(605, 234);
            this.dgvRevision.TabIndex = 0;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(518, 240);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // ProductRevisions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(605, 268);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.dgvRevision);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ProductRevisions";
            this.Text = "Product Revisions";
            ((System.ComponentModel.ISupportInitialize)(this.dgvRevision)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvRevision;
        private System.Windows.Forms.Button btnClose;
    }
}
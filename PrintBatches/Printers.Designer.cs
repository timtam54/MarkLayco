namespace PrintBatches
{
    partial class Printers
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Printers));
            this.pnlPrinters = new System.Windows.Forms.Panel();
            this.clbPrinters = new System.Windows.Forms.CheckedListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // pnlPrinters
            // 
            this.pnlPrinters.BackColor = System.Drawing.Color.White;
            this.pnlPrinters.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlPrinters.Location = new System.Drawing.Point(109, 95);
            this.pnlPrinters.Name = "pnlPrinters";
            this.pnlPrinters.Size = new System.Drawing.Size(181, 79);
            this.pnlPrinters.TabIndex = 21;
            // 
            // clbPrinters
            // 
            this.clbPrinters.FormattingEnabled = true;
            this.clbPrinters.Location = new System.Drawing.Point(109, 9);
            this.clbPrinters.Name = "clbPrinters";
            this.clbPrinters.Size = new System.Drawing.Size(181, 79);
            this.clbPrinters.TabIndex = 18;
            this.clbPrinters.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.clbPrinters_ItemCheck);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(91, 13);
            this.label3.TabIndex = 19;
            this.label3.Text = "Printers Available:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 95);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(90, 13);
            this.label4.TabIndex = 20;
            this.label4.Text = "Selected Printers:";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(215, 180);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 22;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // Printers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(306, 214);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.pnlPrinters);
            this.Controls.Add(this.clbPrinters);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Printers";
            this.Text = "Printers";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlPrinters;
        private System.Windows.Forms.CheckedListBox clbPrinters;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnSave;
    }
}
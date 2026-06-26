namespace PrintBatches
{
    partial class PrintBatch
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
            this.pnlTicket = new System.Windows.Forms.Panel();
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.SuspendLayout();
            // 
            // pnlTicket
            // 
            this.pnlTicket.Location = new System.Drawing.Point(2, 1);
            this.pnlTicket.Name = "pnlTicket";
            this.pnlTicket.Size = new System.Drawing.Size(750, 382);
            this.pnlTicket.TabIndex = 16;
            // 
            // PrintBatch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(756, 389);
            this.Controls.Add(this.pnlTicket);
            this.Name = "PrintBatch";
            this.Text = "PrintBatch";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlTicket;
        private System.Drawing.Printing.PrintDocument printDocument1;
    }
}
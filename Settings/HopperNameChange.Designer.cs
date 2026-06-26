namespace Settings
{
    partial class HopperNameChange
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HopperNameChange));
            this.cbxProdNames = new System.Windows.Forms.ComboBox();
            this.lblCurrentName = new System.Windows.Forms.Label();
            this.btnChangeName = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cbxProdNames
            // 
            this.cbxProdNames.FormattingEnabled = true;
            this.cbxProdNames.Location = new System.Drawing.Point(91, 37);
            this.cbxProdNames.Name = "cbxProdNames";
            this.cbxProdNames.Size = new System.Drawing.Size(217, 21);
            this.cbxProdNames.TabIndex = 13;
            // 
            // lblCurrentName
            // 
            this.lblCurrentName.BackColor = System.Drawing.Color.White;
            this.lblCurrentName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblCurrentName.Location = new System.Drawing.Point(91, 5);
            this.lblCurrentName.Name = "lblCurrentName";
            this.lblCurrentName.Size = new System.Drawing.Size(217, 20);
            this.lblCurrentName.TabIndex = 12;
            this.lblCurrentName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnChangeName
            // 
            this.btnChangeName.Location = new System.Drawing.Point(186, 64);
            this.btnChangeName.Name = "btnChangeName";
            this.btnChangeName.Size = new System.Drawing.Size(123, 23);
            this.btnChangeName.TabIndex = 11;
            this.btnChangeName.Text = "Change Hopper Name";
            this.btnChangeName.UseVisualStyleBackColor = true;
            this.btnChangeName.Click += new System.EventHandler(this.btnChangeName_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "New name:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Current name:";
            // 
            // HopperNameChange
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(321, 94);
            this.Controls.Add(this.cbxProdNames);
            this.Controls.Add(this.lblCurrentName);
            this.Controls.Add(this.btnChangeName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "HopperNameChange";
            this.Text = "HopperNameChange";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbxProdNames;
        private System.Windows.Forms.Label lblCurrentName;
        private System.Windows.Forms.Button btnChangeName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}
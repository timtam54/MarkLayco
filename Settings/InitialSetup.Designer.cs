namespace Settings
{
    partial class InitialSetup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InitialSetup));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtIp = new System.Windows.Forms.TextBox();
            this.txtNumHoppers = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnVerify = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbtnKgs = new System.Windows.Forms.RadioButton();
            this.rbtnLbs = new System.Windows.Forms.RadioButton();
            this.label5 = new System.Windows.Forms.Label();
            this.lblConnection = new System.Windows.Forms.Label();
            this.btnMakeTicket = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(66, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "PLC IP:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(102, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Number of Hoppers:";
            // 
            // txtIp
            // 
            this.txtIp.Location = new System.Drawing.Point(115, 13);
            this.txtIp.Name = "txtIp";
            this.txtIp.Size = new System.Drawing.Size(100, 20);
            this.txtIp.TabIndex = 2;
            // 
            // txtNumHoppers
            // 
            this.txtNumHoppers.Location = new System.Drawing.Point(115, 43);
            this.txtNumHoppers.Name = "txtNumHoppers";
            this.txtNumHoppers.Size = new System.Drawing.Size(100, 20);
            this.txtNumHoppers.TabIndex = 3;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(221, 11);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 4;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnVerify
            // 
            this.btnVerify.Location = new System.Drawing.Point(221, 41);
            this.btnVerify.Name = "btnVerify";
            this.btnVerify.Size = new System.Drawing.Size(75, 23);
            this.btnVerify.TabIndex = 5;
            this.btnVerify.Text = "Verify";
            this.btnVerify.UseVisualStyleBackColor = true;
            this.btnVerify.Click += new System.EventHandler(this.btnVerify_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(233, 175);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 11;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbtnKgs);
            this.groupBox1.Controls.Add(this.rbtnLbs);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.lblConnection);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtIp);
            this.groupBox1.Controls.Add(this.txtNumHoppers);
            this.groupBox1.Controls.Add(this.btnSearch);
            this.groupBox1.Controls.Add(this.btnVerify);
            this.groupBox1.Location = new System.Drawing.Point(5, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(303, 136);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "System";
            // 
            // rbtnKgs
            // 
            this.rbtnKgs.AutoSize = true;
            this.rbtnKgs.Location = new System.Drawing.Point(115, 116);
            this.rbtnKgs.Name = "rbtnKgs";
            this.rbtnKgs.Size = new System.Drawing.Size(131, 17);
            this.rbtnKgs.TabIndex = 9;
            this.rbtnKgs.TabStop = true;
            this.rbtnKgs.Text = "Kilograms/Metric Tons";
            this.rbtnKgs.UseVisualStyleBackColor = true;
            // 
            // rbtnLbs
            // 
            this.rbtnLbs.AutoSize = true;
            this.rbtnLbs.Location = new System.Drawing.Point(115, 93);
            this.rbtnLbs.Name = "rbtnLbs";
            this.rbtnLbs.Size = new System.Drawing.Size(90, 17);
            this.rbtnLbs.TabIndex = 8;
            this.rbtnLbs.TabStop = true;
            this.rbtnLbs.Text = "Pounds/Tons";
            this.rbtnLbs.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(38, 95);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Weight Units:";
            // 
            // lblConnection
            // 
            this.lblConnection.ForeColor = System.Drawing.Color.Maroon;
            this.lblConnection.Location = new System.Drawing.Point(115, 67);
            this.lblConnection.Name = "lblConnection";
            this.lblConnection.Size = new System.Drawing.Size(100, 19);
            this.lblConnection.TabIndex = 6;
            this.lblConnection.Text = "No Connection";
            this.lblConnection.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnMakeTicket
            // 
            this.btnMakeTicket.Location = new System.Drawing.Point(208, 146);
            this.btnMakeTicket.Name = "btnMakeTicket";
            this.btnMakeTicket.Size = new System.Drawing.Size(100, 23);
            this.btnMakeTicket.TabIndex = 0;
            this.btnMakeTicket.Text = "Setup Ticket";
            this.btnMakeTicket.UseVisualStyleBackColor = true;
            this.btnMakeTicket.Click += new System.EventHandler(this.btnMakeTicket_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // InitialSetup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(315, 205);
            this.Controls.Add(this.btnMakeTicket);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnSave);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "InitialSetup";
            this.Text = "InitialSetup";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtIp;
        private System.Windows.Forms.TextBox txtNumHoppers;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnVerify;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblConnection;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btnMakeTicket;
        private System.Windows.Forms.RadioButton rbtnKgs;
        private System.Windows.Forms.RadioButton rbtnLbs;
        private System.Windows.Forms.Label label5;
    }
}
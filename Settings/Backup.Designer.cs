namespace Settings
{
    partial class Backup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Backup));
            this.btnStart = new System.Windows.Forms.Button();
            this.pbCurrent = new System.Windows.Forms.ProgressBar();
            this.pbTotal = new System.Windows.Forms.ProgressBar();
            this.lblCurrent = new System.Windows.Forms.Label();
            this.lblTotal = new System.Windows.Forms.Label();
            this.ckbxSettings = new System.Windows.Forms.CheckBox();
            this.ckbxDatabase = new System.Windows.Forms.CheckBox();
            this.txtSname = new System.Windows.Forms.TextBox();
            this.txtSpath = new System.Windows.Forms.TextBox();
            this.txtDname = new System.Windows.Forms.TextBox();
            this.txtDpath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnSbrowse = new System.Windows.Forms.Button();
            this.btnDbrowse = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Enabled = false;
            this.btnStart.Location = new System.Drawing.Point(253, 151);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(50, 23);
            this.btnStart.TabIndex = 4;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // pbCurrent
            // 
            this.pbCurrent.Location = new System.Drawing.Point(9, 193);
            this.pbCurrent.Name = "pbCurrent";
            this.pbCurrent.Size = new System.Drawing.Size(294, 23);
            this.pbCurrent.Step = 2;
            this.pbCurrent.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.pbCurrent.TabIndex = 6;
            // 
            // pbTotal
            // 
            this.pbTotal.Location = new System.Drawing.Point(9, 239);
            this.pbTotal.Name = "pbTotal";
            this.pbTotal.Size = new System.Drawing.Size(294, 23);
            this.pbTotal.Step = 3;
            this.pbTotal.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.pbTotal.TabIndex = 7;
            // 
            // lblCurrent
            // 
            this.lblCurrent.AutoSize = true;
            this.lblCurrent.Location = new System.Drawing.Point(8, 177);
            this.lblCurrent.Name = "lblCurrent";
            this.lblCurrent.Size = new System.Drawing.Size(41, 13);
            this.lblCurrent.TabIndex = 8;
            this.lblCurrent.Text = "Current";
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Location = new System.Drawing.Point(8, 224);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(31, 13);
            this.lblTotal.TabIndex = 9;
            this.lblTotal.Text = "Total";
            // 
            // ckbxSettings
            // 
            this.ckbxSettings.AutoSize = true;
            this.ckbxSettings.Location = new System.Drawing.Point(7, 9);
            this.ckbxSettings.Name = "ckbxSettings";
            this.ckbxSettings.Size = new System.Drawing.Size(64, 17);
            this.ckbxSettings.TabIndex = 12;
            this.ckbxSettings.Text = "Settings";
            this.ckbxSettings.UseVisualStyleBackColor = true;
            this.ckbxSettings.CheckedChanged += new System.EventHandler(this.ckbxSettings_CheckedChanged);
            // 
            // ckbxDatabase
            // 
            this.ckbxDatabase.AutoSize = true;
            this.ckbxDatabase.Location = new System.Drawing.Point(98, 9);
            this.ckbxDatabase.Name = "ckbxDatabase";
            this.ckbxDatabase.Size = new System.Drawing.Size(72, 17);
            this.ckbxDatabase.TabIndex = 16;
            this.ckbxDatabase.Text = "Database";
            this.ckbxDatabase.UseVisualStyleBackColor = true;
            this.ckbxDatabase.CheckedChanged += new System.EventHandler(this.ckbxDatabase_CheckedChanged);
            // 
            // txtSname
            // 
            this.txtSname.Enabled = false;
            this.txtSname.Location = new System.Drawing.Point(98, 39);
            this.txtSname.Name = "txtSname";
            this.txtSname.Size = new System.Drawing.Size(175, 20);
            this.txtSname.TabIndex = 28;
            // 
            // txtSpath
            // 
            this.txtSpath.Enabled = false;
            this.txtSpath.Location = new System.Drawing.Point(98, 65);
            this.txtSpath.Name = "txtSpath";
            this.txtSpath.Size = new System.Drawing.Size(175, 20);
            this.txtSpath.TabIndex = 29;
            // 
            // txtDname
            // 
            this.txtDname.Enabled = false;
            this.txtDname.Location = new System.Drawing.Point(98, 91);
            this.txtDname.Name = "txtDname";
            this.txtDname.Size = new System.Drawing.Size(175, 20);
            this.txtDname.TabIndex = 30;
            // 
            // txtDpath
            // 
            this.txtDpath.Enabled = false;
            this.txtDpath.Location = new System.Drawing.Point(98, 117);
            this.txtDpath.Name = "txtDpath";
            this.txtDpath.Size = new System.Drawing.Size(175, 20);
            this.txtDpath.TabIndex = 31;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 32;
            this.label1.Text = "Settings Name:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 13);
            this.label2.TabIndex = 33;
            this.label2.Text = "Settings Path:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 95);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(87, 13);
            this.label3.TabIndex = 34;
            this.label3.Text = "Database Name:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 121);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(81, 13);
            this.label4.TabIndex = 35;
            this.label4.Text = "Database Path:";
            // 
            // btnSbrowse
            // 
            this.btnSbrowse.Enabled = false;
            this.btnSbrowse.Location = new System.Drawing.Point(279, 63);
            this.btnSbrowse.Name = "btnSbrowse";
            this.btnSbrowse.Size = new System.Drawing.Size(24, 23);
            this.btnSbrowse.TabIndex = 36;
            this.btnSbrowse.Text = "...";
            this.btnSbrowse.UseVisualStyleBackColor = true;
            this.btnSbrowse.Click += new System.EventHandler(this.btnSbrowse_Click);
            // 
            // btnDbrowse
            // 
            this.btnDbrowse.Enabled = false;
            this.btnDbrowse.Location = new System.Drawing.Point(279, 116);
            this.btnDbrowse.Name = "btnDbrowse";
            this.btnDbrowse.Size = new System.Drawing.Size(24, 23);
            this.btnDbrowse.TabIndex = 37;
            this.btnDbrowse.Text = "...";
            this.btnDbrowse.UseVisualStyleBackColor = true;
            this.btnDbrowse.Click += new System.EventHandler(this.btnDbrowse_Click);
            // 
            // Backup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(310, 270);
            this.Controls.Add(this.btnDbrowse);
            this.Controls.Add(this.btnSbrowse);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtDpath);
            this.Controls.Add(this.txtDname);
            this.Controls.Add(this.txtSpath);
            this.Controls.Add(this.txtSname);
            this.Controls.Add(this.ckbxDatabase);
            this.Controls.Add(this.ckbxSettings);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.lblCurrent);
            this.Controls.Add(this.pbTotal);
            this.Controls.Add(this.pbCurrent);
            this.Controls.Add(this.btnStart);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Backup";
            this.Text = "Backup";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.ProgressBar pbCurrent;
        private System.Windows.Forms.ProgressBar pbTotal;
        private System.Windows.Forms.Label lblCurrent;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.CheckBox ckbxSettings;
        private System.Windows.Forms.CheckBox ckbxDatabase;
        private System.Windows.Forms.TextBox txtSname;
        private System.Windows.Forms.TextBox txtSpath;
        private System.Windows.Forms.TextBox txtDname;
        private System.Windows.Forms.TextBox txtDpath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnSbrowse;
        private System.Windows.Forms.Button btnDbrowse;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
    }
}
namespace PrintBatches
{
    partial class SetupTicket
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Date");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Time");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Total Requested");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Total Weight Ran");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Remove All Ticket Info");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Recipe Info", new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3,
            treeNode4,
            treeNode5});
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("Ratio");
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("Pounds");
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("Tons");
            System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode("Remove All Product Info");
            System.Windows.Forms.TreeNode treeNode11 = new System.Windows.Forms.TreeNode("Products", new System.Windows.Forms.TreeNode[] {
            treeNode7,
            treeNode8,
            treeNode9,
            treeNode10});
            System.Windows.Forms.TreeNode treeNode12 = new System.Windows.Forms.TreeNode("Signature");
            System.Windows.Forms.TreeNode treeNode13 = new System.Windows.Forms.TreeNode("Truck Firm");
            System.Windows.Forms.TreeNode treeNode14 = new System.Windows.Forms.TreeNode("Truck Number");
            System.Windows.Forms.TreeNode treeNode15 = new System.Windows.Forms.TreeNode("Remove All Signature Info");
            System.Windows.Forms.TreeNode treeNode16 = new System.Windows.Forms.TreeNode("Signature", new System.Windows.Forms.TreeNode[] {
            treeNode12,
            treeNode13,
            treeNode14,
            treeNode15});
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetupTicket));
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.pnlContainer = new System.Windows.Forms.Panel();
            this.btnSave = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.btnPrinters = new System.Windows.Forms.Button();
            this.splSign = new SizablePanel();
            this.splProds = new SizablePanel();
            this.prodIdInfo = new System.Windows.Forms.Label();
            this.prodNameInfo = new System.Windows.Forms.Label();
            this.prodId = new System.Windows.Forms.Label();
            this.prodName = new System.Windows.Forms.Label();
            this.splLogo = new SizablePanel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.splRecipe = new SizablePanel();
            this.recipeNameInfo = new System.Windows.Forms.Label();
            this.recipeName = new System.Windows.Forms.Label();
            this.splAddress = new SizablePanel();
            this.location = new System.Windows.Forms.TextBox();
            this.pnlContainer.SuspendLayout();
            this.splProds.SuspendLayout();
            this.splLogo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.splRecipe.SuspendLayout();
            this.splAddress.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(1, 1);
            this.treeView1.Name = "treeView1";
            treeNode1.Name = "cnDate";
            treeNode1.Tag = "date";
            treeNode1.Text = "Date";
            treeNode2.Name = "cnTime";
            treeNode2.Tag = "time";
            treeNode2.Text = "Time";
            treeNode3.Name = "cnTotalRequested";
            treeNode3.Tag = "totalRequested";
            treeNode3.Text = "Total Requested";
            treeNode4.Name = "cnTotalRan";
            treeNode4.Tag = "totalRan";
            treeNode4.Text = "Total Weight Ran";
            treeNode5.Name = "cnRemoveTicket";
            treeNode5.Tag = "removeTicket";
            treeNode5.Text = "Remove All Ticket Info";
            treeNode6.Name = "pnRecipe";
            treeNode6.Tag = "splRecipe";
            treeNode6.Text = "Recipe Info";
            treeNode7.Name = "cnRatio";
            treeNode7.Tag = "ratio";
            treeNode7.Text = "Ratio";
            treeNode8.Name = "cnPounds";
            treeNode8.Tag = "pounds";
            treeNode8.Text = "Pounds";
            treeNode9.Name = "cnTons";
            treeNode9.Tag = "tons";
            treeNode9.Text = "Tons";
            treeNode10.Name = "cnRemoveProducts";
            treeNode10.Tag = "removeProducts";
            treeNode10.Text = "Remove All Product Info";
            treeNode11.Name = "pnProducts";
            treeNode11.Tag = "splProds";
            treeNode11.Text = "Products";
            treeNode12.Name = "cnSignLine";
            treeNode12.Tag = "signLine";
            treeNode12.Text = "Signature";
            treeNode13.Name = "cnTruckFirm";
            treeNode13.Tag = "truckFirm";
            treeNode13.Text = "Truck Firm";
            treeNode14.Name = "cnTruckNum";
            treeNode14.Tag = "truckNum";
            treeNode14.Text = "Truck Number";
            treeNode15.Name = "cnRemoveSign";
            treeNode15.Tag = "removeSign";
            treeNode15.Text = "Remove All Signature Info";
            treeNode16.Name = "pnSignature";
            treeNode16.Tag = "splSign";
            treeNode16.Text = "Signature";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode6,
            treeNode11,
            treeNode16});
            this.treeView1.Size = new System.Drawing.Size(183, 283);
            this.treeView1.TabIndex = 14;
            this.treeView1.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.treeView1_ItemDrag);
            this.treeView1.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseDoubleClick);
            // 
            // pnlContainer
            // 
            this.pnlContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlContainer.Controls.Add(this.splSign);
            this.pnlContainer.Controls.Add(this.splProds);
            this.pnlContainer.Controls.Add(this.splLogo);
            this.pnlContainer.Controls.Add(this.splRecipe);
            this.pnlContainer.Controls.Add(this.splAddress);
            this.pnlContainer.Location = new System.Drawing.Point(190, 1);
            this.pnlContainer.Name = "pnlContainer";
            this.pnlContainer.Size = new System.Drawing.Size(750, 382);
            this.pnlContainer.TabIndex = 15;
            this.pnlContainer.DragDrop += new System.Windows.Forms.DragEventHandler(this.dragDrop);
            this.pnlContainer.DragEnter += new System.Windows.Forms.DragEventHandler(this.dragEnter);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(55, 350);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 16;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(47, 296);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(91, 23);
            this.btnBrowse.TabIndex = 17;
            this.btnBrowse.Text = "Browse for logo";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // btnPrinters
            // 
            this.btnPrinters.Location = new System.Drawing.Point(55, 323);
            this.btnPrinters.Name = "btnPrinters";
            this.btnPrinters.Size = new System.Drawing.Size(75, 23);
            this.btnPrinters.TabIndex = 18;
            this.btnPrinters.Text = "Printers";
            this.btnPrinters.UseVisualStyleBackColor = true;
            this.btnPrinters.Click += new System.EventHandler(this.btnPrinters_Click);
            // 
            // splSign
            // 
            this.splSign.AllowDrop = true;
            this.splSign.AutoSize = true;
            this.splSign.BackColor = System.Drawing.Color.White;
            this.splSign.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splSign.Location = new System.Drawing.Point(1, 279);
            this.splSign.Name = "splSign";
            this.splSign.Size = new System.Drawing.Size(744, 98);
            this.splSign.TabIndex = 12;
            // 
            // splProds
            // 
            this.splProds.AllowDrop = true;
            this.splProds.AutoSize = true;
            this.splProds.BackColor = System.Drawing.Color.White;
            this.splProds.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splProds.Controls.Add(this.prodIdInfo);
            this.splProds.Controls.Add(this.prodNameInfo);
            this.splProds.Controls.Add(this.prodId);
            this.splProds.Controls.Add(this.prodName);
            this.splProds.Location = new System.Drawing.Point(1, 173);
            this.splProds.Name = "splProds";
            this.splProds.Size = new System.Drawing.Size(744, 100);
            this.splProds.TabIndex = 10;
            // 
            // prodIdInfo
            // 
            this.prodIdInfo.Location = new System.Drawing.Point(132, 44);
            this.prodIdInfo.Name = "prodIdInfo";
            this.prodIdInfo.Size = new System.Drawing.Size(102, 13);
            this.prodIdInfo.TabIndex = 17;
            this.prodIdInfo.Text = "Dap";
            // 
            // prodNameInfo
            // 
            this.prodNameInfo.Location = new System.Drawing.Point(13, 44);
            this.prodNameInfo.Name = "prodNameInfo";
            this.prodNameInfo.Size = new System.Drawing.Size(102, 13);
            this.prodNameInfo.TabIndex = 16;
            this.prodNameInfo.Text = "Dap (14-86-00)";
            // 
            // prodId
            // 
            this.prodId.AutoSize = true;
            this.prodId.Location = new System.Drawing.Point(132, 17);
            this.prodId.Name = "prodId";
            this.prodId.Size = new System.Drawing.Size(56, 13);
            this.prodId.TabIndex = 14;
            this.prodId.Text = "Product Id";
            // 
            // prodName
            // 
            this.prodName.AutoSize = true;
            this.prodName.Location = new System.Drawing.Point(13, 17);
            this.prodName.Name = "prodName";
            this.prodName.Size = new System.Drawing.Size(75, 13);
            this.prodName.TabIndex = 13;
            this.prodName.Text = "Product Name";
            // 
            // splLogo
            // 
            this.splLogo.AllowDrop = true;
            this.splLogo.AutoSize = true;
            this.splLogo.BackColor = System.Drawing.Color.White;
            this.splLogo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splLogo.Controls.Add(this.pictureBox1);
            this.splLogo.Location = new System.Drawing.Point(230, 4);
            this.splLogo.Name = "splLogo";
            this.splLogo.Size = new System.Drawing.Size(244, 123);
            this.splLogo.TabIndex = 9;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(239, 104);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // splRecipe
            // 
            this.splRecipe.AllowDrop = true;
            this.splRecipe.AutoSize = true;
            this.splRecipe.BackColor = System.Drawing.Color.White;
            this.splRecipe.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splRecipe.Controls.Add(this.recipeNameInfo);
            this.splRecipe.Controls.Add(this.recipeName);
            this.splRecipe.Location = new System.Drawing.Point(480, 4);
            this.splRecipe.Name = "splRecipe";
            this.splRecipe.Size = new System.Drawing.Size(266, 163);
            this.splRecipe.TabIndex = 8;
            // 
            // recipeNameInfo
            // 
            this.recipeNameInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.recipeNameInfo.Location = new System.Drawing.Point(109, 3);
            this.recipeNameInfo.Name = "recipeNameInfo";
            this.recipeNameInfo.Size = new System.Drawing.Size(100, 18);
            this.recipeNameInfo.TabIndex = 8;
            this.recipeNameInfo.Text = "897";
            // 
            // recipeName
            // 
            this.recipeName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.recipeName.Location = new System.Drawing.Point(3, 3);
            this.recipeName.Name = "recipeName";
            this.recipeName.Size = new System.Drawing.Size(100, 18);
            this.recipeName.TabIndex = 7;
            this.recipeName.Text = "Recipe Name: ";
            // 
            // splAddress
            // 
            this.splAddress.AllowDrop = true;
            this.splAddress.AutoSize = true;
            this.splAddress.BackColor = System.Drawing.Color.White;
            this.splAddress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splAddress.Controls.Add(this.location);
            this.splAddress.Location = new System.Drawing.Point(2, 4);
            this.splAddress.Name = "splAddress";
            this.splAddress.Size = new System.Drawing.Size(222, 123);
            this.splAddress.TabIndex = 7;
            // 
            // location
            // 
            this.location.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.location.Location = new System.Drawing.Point(3, 3);
            this.location.Multiline = true;
            this.location.Name = "location";
            this.location.Size = new System.Drawing.Size(204, 87);
            this.location.TabIndex = 0;
            this.location.Text = "Location";
            // 
            // SetupTicket
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(945, 386);
            this.Controls.Add(this.btnPrinters);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.pnlContainer);
            this.Controls.Add(this.treeView1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SetupTicket";
            this.Text = "Setup Ticket";
            this.pnlContainer.ResumeLayout(false);
            this.pnlContainer.PerformLayout();
            this.splProds.ResumeLayout(false);
            this.splProds.PerformLayout();
            this.splLogo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.splRecipe.ResumeLayout(false);
            this.splAddress.ResumeLayout(false);
            this.splAddress.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Panel pnlContainer;
        private SizablePanel splSign;
        private SizablePanel splProds;
        private System.Windows.Forms.Label prodIdInfo;
        private System.Windows.Forms.Label prodNameInfo;
        private System.Windows.Forms.Label prodId;
        private System.Windows.Forms.Label prodName;
        private SizablePanel splLogo;
        private System.Windows.Forms.PictureBox pictureBox1;
        private SizablePanel splRecipe;
        private System.Windows.Forms.Label recipeNameInfo;
        private System.Windows.Forms.Label recipeName;
        private SizablePanel splAddress;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox location;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btnPrinters;
    }
}
namespace LaycoRecipeProgram
{
    partial class DbViewer
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DbViewer));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpgRecipes = new System.Windows.Forms.TabPage();
            this.txtRecipeContains = new System.Windows.Forms.TextBox();
            this.flpRecipeProds = new System.Windows.Forms.FlowLayoutPanel();
            this.lbxRecipeNames = new System.Windows.Forms.ListBox();
            this.txtRecipeName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnRecipeSort = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.tpgProducts = new System.Windows.Forms.TabPage();
            this.btnProdTotalRan = new System.Windows.Forms.Button();
            this.btnProdUsed = new System.Windows.Forms.Button();
            this.btnProductsRevisions = new System.Windows.Forms.Button();
            this.dgvProducts = new System.Windows.Forms.DataGridView();
            this.tpgHistory = new System.Windows.Forms.TabPage();
            this.btnViewPrinted = new System.Windows.Forms.Button();
            this.txtHrecipeContains = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.dtpToDate = new System.Windows.Forms.DateTimePicker();
            this.lbl2 = new System.Windows.Forms.Label();
            this.dtpFromDate = new System.Windows.Forms.DateTimePicker();
            this.lbl1 = new System.Windows.Forms.Label();
            this.cbxDateRange = new System.Windows.Forms.ComboBox();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.txtHrecipeName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnHistorySort = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.dgvHistory = new System.Windows.Forms.DataGridView();
            this.cbxDb = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblFrom = new System.Windows.Forms.Label();
            this.lblTo = new System.Windows.Forms.Label();
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.printPreviewDialog1 = new System.Windows.Forms.PrintPreviewDialog();
            this.tabControl1.SuspendLayout();
            this.tpgRecipes.SuspendLayout();
            this.tpgProducts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProducts)).BeginInit();
            this.tpgHistory.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistory)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tpgRecipes);
            this.tabControl1.Controls.Add(this.tpgProducts);
            this.tabControl1.Controls.Add(this.tpgHistory);
            this.tabControl1.Location = new System.Drawing.Point(1, 36);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(623, 464);
            this.tabControl1.TabIndex = 1;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tpgRecipes
            // 
            this.tpgRecipes.Controls.Add(this.txtRecipeContains);
            this.tpgRecipes.Controls.Add(this.flpRecipeProds);
            this.tpgRecipes.Controls.Add(this.lbxRecipeNames);
            this.tpgRecipes.Controls.Add(this.txtRecipeName);
            this.tpgRecipes.Controls.Add(this.label3);
            this.tpgRecipes.Controls.Add(this.btnRecipeSort);
            this.tpgRecipes.Controls.Add(this.label2);
            this.tpgRecipes.Location = new System.Drawing.Point(4, 22);
            this.tpgRecipes.Name = "tpgRecipes";
            this.tpgRecipes.Padding = new System.Windows.Forms.Padding(3);
            this.tpgRecipes.Size = new System.Drawing.Size(615, 438);
            this.tpgRecipes.TabIndex = 0;
            this.tpgRecipes.Text = "Recipes";
            this.tpgRecipes.UseVisualStyleBackColor = true;
            // 
            // txtRecipeContains
            // 
            this.txtRecipeContains.AutoCompleteCustomSource.AddRange(new string[] {
            "11-11-12",
            "22-59-62",
            "Aaron Edwards",
            "Aaron Short",
            "Red Onion",
            "Red Lobster",
            "Red Tamato "});
            this.txtRecipeContains.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.txtRecipeContains.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.txtRecipeContains.Location = new System.Drawing.Point(335, 10);
            this.txtRecipeContains.Name = "txtRecipeContains";
            this.txtRecipeContains.Size = new System.Drawing.Size(162, 20);
            this.txtRecipeContains.TabIndex = 15;
            // 
            // flpRecipeProds
            // 
            this.flpRecipeProds.AutoScroll = true;
            this.flpRecipeProds.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.flpRecipeProds.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpRecipeProds.Location = new System.Drawing.Point(196, 66);
            this.flpRecipeProds.Name = "flpRecipeProds";
            this.flpRecipeProds.Size = new System.Drawing.Size(411, 342);
            this.flpRecipeProds.TabIndex = 12;
            // 
            // lbxRecipeNames
            // 
            this.lbxRecipeNames.FormattingEnabled = true;
            this.lbxRecipeNames.Location = new System.Drawing.Point(8, 66);
            this.lbxRecipeNames.Name = "lbxRecipeNames";
            this.lbxRecipeNames.Size = new System.Drawing.Size(182, 342);
            this.lbxRecipeNames.TabIndex = 7;
            this.lbxRecipeNames.SelectedIndexChanged += new System.EventHandler(this.lbxRecipeNames_SelectedIndexChanged);
            // 
            // txtRecipeName
            // 
            this.txtRecipeName.AutoCompleteCustomSource.AddRange(new string[] {
            "11-11-12",
            "22-59-62",
            "Aaron Edwards",
            "Aaron Short",
            "Red Onion",
            "Red Lobster",
            "Red Tamato "});
            this.txtRecipeName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txtRecipeName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.txtRecipeName.Location = new System.Drawing.Point(99, 10);
            this.txtRecipeName.Name = "txtRecipeName";
            this.txtRecipeName.Size = new System.Drawing.Size(162, 20);
            this.txtRecipeName.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 14);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Recipe Name:";
            // 
            // btnRecipeSort
            // 
            this.btnRecipeSort.Location = new System.Drawing.Point(503, 9);
            this.btnRecipeSort.Name = "btnRecipeSort";
            this.btnRecipeSort.Size = new System.Drawing.Size(42, 23);
            this.btnRecipeSort.TabIndex = 4;
            this.btnRecipeSort.Text = "Sort";
            this.btnRecipeSort.UseVisualStyleBackColor = true;
            this.btnRecipeSort.Click += new System.EventHandler(this.btnRecipeSort_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(278, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Contains:";
            // 
            // tpgProducts
            // 
            this.tpgProducts.Controls.Add(this.btnProdTotalRan);
            this.tpgProducts.Controls.Add(this.btnProdUsed);
            this.tpgProducts.Controls.Add(this.btnProductsRevisions);
            this.tpgProducts.Controls.Add(this.dgvProducts);
            this.tpgProducts.Location = new System.Drawing.Point(4, 22);
            this.tpgProducts.Name = "tpgProducts";
            this.tpgProducts.Padding = new System.Windows.Forms.Padding(3);
            this.tpgProducts.Size = new System.Drawing.Size(615, 438);
            this.tpgProducts.TabIndex = 1;
            this.tpgProducts.Text = "Products";
            this.tpgProducts.UseVisualStyleBackColor = true;
            // 
            // btnProdTotalRan
            // 
            this.btnProdTotalRan.Location = new System.Drawing.Point(533, 98);
            this.btnProdTotalRan.Name = "btnProdTotalRan";
            this.btnProdTotalRan.Size = new System.Drawing.Size(75, 23);
            this.btnProdTotalRan.TabIndex = 5;
            this.btnProdTotalRan.Text = "Total Ran";
            this.btnProdTotalRan.UseVisualStyleBackColor = true;
            this.btnProdTotalRan.Click += new System.EventHandler(this.btnProdTotalRan_Click);
            // 
            // btnProdUsed
            // 
            this.btnProdUsed.Enabled = false;
            this.btnProdUsed.Location = new System.Drawing.Point(533, 35);
            this.btnProdUsed.Name = "btnProdUsed";
            this.btnProdUsed.Size = new System.Drawing.Size(75, 23);
            this.btnProdUsed.TabIndex = 4;
            this.btnProdUsed.Text = "Recipes";
            this.btnProdUsed.UseVisualStyleBackColor = true;
            this.btnProdUsed.Click += new System.EventHandler(this.btnProdUsed_Click);
            // 
            // btnProductsRevisions
            // 
            this.btnProductsRevisions.Enabled = false;
            this.btnProductsRevisions.Location = new System.Drawing.Point(533, 6);
            this.btnProductsRevisions.Name = "btnProductsRevisions";
            this.btnProductsRevisions.Size = new System.Drawing.Size(75, 23);
            this.btnProductsRevisions.TabIndex = 3;
            this.btnProductsRevisions.Text = "Revisions";
            this.btnProductsRevisions.UseVisualStyleBackColor = true;
            this.btnProductsRevisions.Click += new System.EventHandler(this.btnProductsRevisions_Click);
            // 
            // dgvProducts
            // 
            this.dgvProducts.AllowUserToAddRows = false;
            this.dgvProducts.AllowUserToDeleteRows = false;
            this.dgvProducts.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvProducts.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvProducts.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvProducts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvProducts.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvProducts.Location = new System.Drawing.Point(8, 6);
            this.dgvProducts.MultiSelect = false;
            this.dgvProducts.Name = "dgvProducts";
            this.dgvProducts.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvProducts.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvProducts.RowHeadersVisible = false;
            this.dgvProducts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvProducts.Size = new System.Drawing.Size(519, 426);
            this.dgvProducts.TabIndex = 0;
            // 
            // tpgHistory
            // 
            this.tpgHistory.Controls.Add(this.btnViewPrinted);
            this.tpgHistory.Controls.Add(this.txtHrecipeContains);
            this.tpgHistory.Controls.Add(this.label8);
            this.tpgHistory.Controls.Add(this.dtpToDate);
            this.tpgHistory.Controls.Add(this.lbl2);
            this.tpgHistory.Controls.Add(this.dtpFromDate);
            this.tpgHistory.Controls.Add(this.lbl1);
            this.tpgHistory.Controls.Add(this.cbxDateRange);
            this.tpgHistory.Controls.Add(this.btnExport);
            this.tpgHistory.Controls.Add(this.btnPrint);
            this.tpgHistory.Controls.Add(this.txtHrecipeName);
            this.tpgHistory.Controls.Add(this.label4);
            this.tpgHistory.Controls.Add(this.btnHistorySort);
            this.tpgHistory.Controls.Add(this.label5);
            this.tpgHistory.Controls.Add(this.dgvHistory);
            this.tpgHistory.Location = new System.Drawing.Point(4, 22);
            this.tpgHistory.Name = "tpgHistory";
            this.tpgHistory.Size = new System.Drawing.Size(615, 438);
            this.tpgHistory.TabIndex = 2;
            this.tpgHistory.Text = "History";
            this.tpgHistory.UseVisualStyleBackColor = true;
            // 
            // btnViewPrinted
            // 
            this.btnViewPrinted.Location = new System.Drawing.Point(532, 412);
            this.btnViewPrinted.Name = "btnViewPrinted";
            this.btnViewPrinted.Size = new System.Drawing.Size(75, 23);
            this.btnViewPrinted.TabIndex = 23;
            this.btnViewPrinted.Text = "View Ticket";
            this.btnViewPrinted.UseVisualStyleBackColor = true;
            this.btnViewPrinted.Click += new System.EventHandler(this.btnViewPrinted_Click);
            // 
            // txtHrecipeContains
            // 
            this.txtHrecipeContains.AutoCompleteCustomSource.AddRange(new string[] {
            "11-11-12",
            "22-59-62",
            "Aaron Edwards",
            "Aaron Short",
            "Red Onion",
            "Red Lobster",
            "Red Tamato "});
            this.txtHrecipeContains.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.txtHrecipeContains.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.txtHrecipeContains.Location = new System.Drawing.Point(293, 28);
            this.txtHrecipeContains.Name = "txtHrecipeContains";
            this.txtHrecipeContains.Size = new System.Drawing.Size(141, 20);
            this.txtHrecipeContains.TabIndex = 22;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(50, 8);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(33, 13);
            this.label8.TabIndex = 21;
            this.label8.Text = "Date:";
            // 
            // dtpToDate
            // 
            this.dtpToDate.Enabled = false;
            this.dtpToDate.Location = new System.Drawing.Point(428, 4);
            this.dtpToDate.Name = "dtpToDate";
            this.dtpToDate.Size = new System.Drawing.Size(110, 20);
            this.dtpToDate.TabIndex = 20;
            // 
            // lbl2
            // 
            this.lbl2.AutoSize = true;
            this.lbl2.Location = new System.Drawing.Point(402, 8);
            this.lbl2.Name = "lbl2";
            this.lbl2.Size = new System.Drawing.Size(20, 13);
            this.lbl2.TabIndex = 19;
            this.lbl2.Text = "To";
            this.lbl2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dtpFromDate
            // 
            this.dtpFromDate.Enabled = false;
            this.dtpFromDate.Location = new System.Drawing.Point(272, 4);
            this.dtpFromDate.Name = "dtpFromDate";
            this.dtpFromDate.Size = new System.Drawing.Size(110, 20);
            this.dtpFromDate.TabIndex = 18;
            // 
            // lbl1
            // 
            this.lbl1.AutoSize = true;
            this.lbl1.Location = new System.Drawing.Point(236, 8);
            this.lbl1.Name = "lbl1";
            this.lbl1.Size = new System.Drawing.Size(30, 13);
            this.lbl1.TabIndex = 17;
            this.lbl1.Text = "From";
            this.lbl1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cbxDateRange
            // 
            this.cbxDateRange.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxDateRange.FormattingEnabled = true;
            this.cbxDateRange.Items.AddRange(new object[] {
            "",
            "Today",
            "Last 7 Days",
            "Last 30 Days",
            "Last 90 Days",
            "Last 12 Months",
            "Specific Date Range"});
            this.cbxDateRange.Location = new System.Drawing.Point(89, 4);
            this.cbxDateRange.Name = "cbxDateRange";
            this.cbxDateRange.Size = new System.Drawing.Size(141, 21);
            this.cbxDateRange.TabIndex = 16;
            this.cbxDateRange.SelectedIndexChanged += new System.EventHandler(this.cbxDateRange_SelectedIndexChanged);
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(559, 27);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(48, 23);
            this.btnExport.TabIndex = 15;
            this.btnExport.Text = "Export";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Location = new System.Drawing.Point(505, 27);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(48, 23);
            this.btnPrint.TabIndex = 14;
            this.btnPrint.Text = "Print";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // txtHrecipeName
            // 
            this.txtHrecipeName.AutoCompleteCustomSource.AddRange(new string[] {
            "11-11-12",
            "22-59-62",
            "Aaron Edwards",
            "Aaron Short",
            "Red Onion",
            "Red Lobster",
            "Red Tamato "});
            this.txtHrecipeName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.txtHrecipeName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.txtHrecipeName.Location = new System.Drawing.Point(89, 28);
            this.txtHrecipeName.Name = "txtHrecipeName";
            this.txtHrecipeName.Size = new System.Drawing.Size(141, 20);
            this.txtHrecipeName.TabIndex = 13;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 32);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Recipe Name:";
            // 
            // btnHistorySort
            // 
            this.btnHistorySort.Location = new System.Drawing.Point(451, 27);
            this.btnHistorySort.Name = "btnHistorySort";
            this.btnHistorySort.Size = new System.Drawing.Size(48, 23);
            this.btnHistorySort.TabIndex = 11;
            this.btnHistorySort.Text = "Sort";
            this.btnHistorySort.UseVisualStyleBackColor = true;
            this.btnHistorySort.Click += new System.EventHandler(this.btnHistorySort_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(236, 32);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(51, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Contains:";
            // 
            // dgvHistory
            // 
            this.dgvHistory.AllowUserToAddRows = false;
            this.dgvHistory.AllowUserToDeleteRows = false;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvHistory.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvHistory.DefaultCellStyle = dataGridViewCellStyle5;
            this.dgvHistory.Location = new System.Drawing.Point(2, 51);
            this.dgvHistory.Name = "dgvHistory";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvHistory.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dgvHistory.RowHeadersVisible = false;
            this.dgvHistory.Size = new System.Drawing.Size(612, 359);
            this.dgvHistory.TabIndex = 0;
            // 
            // cbxDb
            // 
            this.cbxDb.FormattingEnabled = true;
            this.cbxDb.Location = new System.Drawing.Point(98, 5);
            this.cbxDb.Name = "cbxDb";
            this.cbxDb.Size = new System.Drawing.Size(121, 21);
            this.cbxDb.TabIndex = 24;
            this.cbxDb.SelectedIndexChanged += new System.EventHandler(this.cbxDb_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 13);
            this.label1.TabIndex = 25;
            this.label1.Text = "Chose Backup:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(225, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(68, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "History From:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(441, 9);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(23, 13);
            this.label7.TabIndex = 26;
            this.label7.Text = "To:";
            // 
            // lblFrom
            // 
            this.lblFrom.AutoSize = true;
            this.lblFrom.Location = new System.Drawing.Point(299, 9);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(35, 13);
            this.lblFrom.TabIndex = 27;
            this.lblFrom.Text = "label9";
            // 
            // lblTo
            // 
            this.lblTo.AutoSize = true;
            this.lblTo.Location = new System.Drawing.Point(470, 9);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(41, 13);
            this.lblTo.TabIndex = 28;
            this.lblTo.Text = "label10";
            // 
            // printPreviewDialog1
            // 
            this.printPreviewDialog1.AutoScrollMargin = new System.Drawing.Size(0, 0);
            this.printPreviewDialog1.AutoScrollMinSize = new System.Drawing.Size(0, 0);
            this.printPreviewDialog1.ClientSize = new System.Drawing.Size(400, 300);
            this.printPreviewDialog1.Enabled = true;
            this.printPreviewDialog1.Icon = ((System.Drawing.Icon)(resources.GetObject("printPreviewDialog1.Icon")));
            this.printPreviewDialog1.Name = "printPreviewDialog1";
            this.printPreviewDialog1.Visible = false;
            // 
            // DbViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(626, 502);
            this.Controls.Add(this.lblTo);
            this.Controls.Add(this.lblFrom);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbxDb);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DbViewer";
            this.Text = "Database Backup Viewer";
            this.tabControl1.ResumeLayout(false);
            this.tpgRecipes.ResumeLayout(false);
            this.tpgRecipes.PerformLayout();
            this.tpgProducts.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvProducts)).EndInit();
            this.tpgHistory.ResumeLayout(false);
            this.tpgHistory.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistory)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpgRecipes;
        private System.Windows.Forms.TextBox txtRecipeContains;
        private System.Windows.Forms.FlowLayoutPanel flpRecipeProds;
        private System.Windows.Forms.ListBox lbxRecipeNames;
        private System.Windows.Forms.TextBox txtRecipeName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnRecipeSort;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabPage tpgProducts;
        private System.Windows.Forms.Button btnProdTotalRan;
        private System.Windows.Forms.Button btnProdUsed;
        private System.Windows.Forms.Button btnProductsRevisions;
        private System.Windows.Forms.DataGridView dgvProducts;
        private System.Windows.Forms.TabPage tpgHistory;
        private System.Windows.Forms.TextBox txtHrecipeContains;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DateTimePicker dtpToDate;
        private System.Windows.Forms.Label lbl2;
        private System.Windows.Forms.DateTimePicker dtpFromDate;
        private System.Windows.Forms.Label lbl1;
        private System.Windows.Forms.ComboBox cbxDateRange;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.TextBox txtHrecipeName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnHistorySort;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGridView dgvHistory;
        private System.Windows.Forms.Button btnViewPrinted;
        private System.Windows.Forms.ComboBox cbxDb;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.Label lblTo;
        private System.Drawing.Printing.PrintDocument printDocument1;
        private System.Windows.Forms.PrintPreviewDialog printPreviewDialog1;
    }
}
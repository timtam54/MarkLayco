using Database;
using GeneralData;
using History;
using LaycoRecipeProgram;
using PlcComms;
using PlcComms.Master;
using PlcComms.Test;
using PrintBatches;
using Products;
using Recipes;
using Settings;
using SoftwareInteraction;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace LaycpRecipeProgram
{
    public partial class MainWindow : Form
    {
        string appData;
        public static string userSettingsFileName;
        Databases db;
        bool updatingRecipeList = false;

        PlcInteraction plc = new PlcInteraction(LRP.Default.PlcIp);
        List<Button> btns = new List<Button>();
        // Timer that will periodically check the PLC.
        private System.Threading.Timer pollingTimer;
        private object threadLockPlc = new object();
        // Ability to enable/disable communications to PLC.
        private bool plcCommsEnabled = true;
        // error tracking
        private int numErrors = 0;
        private string lastError = "";
        private static Software softwareInterface;

        public MainWindow()
        {
            InitializeComponent();
            checkSetup();
            softwareInterface = Software.Create();
            softwareInterface.RegisterWithMessageHandler(writeToLogFile);
            if (LRP.Default.useMetric)
            { rbtnKg.Checked = true; }
            db = new Databases();
            db.createDb();
            getRecipeNames();
            setContainsTxt();
            setRecipeNameTxt();
            setupSettings();
        }

        void tabControl1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (tabControl1.SelectedTab == tabControl1.TabPages["tpgProducts"])
            { getProducts(); }
            else if (tabControl1.SelectedTab == tabControl1.TabPages["tpgRecipe"])
            { getRecipeNames(); }
            else if (tabControl1.SelectedTab == tabControl1.TabPages["tpgHistory"])
            { setupHistory(); }
        }

        public void checkSetup()
        {
            //LRP.Default.setupDone = false;
            //LRP.Default.Save();
            appData = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"/Yargus/LRP/";
            if (!Directory.Exists(appData))
            { Directory.CreateDirectory(appData); }

            bool setup = LRP.Default.setupDone;
            if (!setup)
            {
                DialogResult dr = new DialogResult();
                InitialSetup initalSetup = new InitialSetup();
                initalSetup.ShowDialog();
                if (!LRP.Default.setupDone)
                {
                    dr = MessageBox.Show("The Layco Blending Program must have be setup. Setup now?", "No Settings", MessageBoxButtons.YesNo);
                    if (dr == DialogResult.No)
                    { Environment.Exit(0); }
                }
                checkSetup();
            }
            else
            {
                //if (LRP.Default.PlcIp != "0.0.0.0")
                //{
                    writeToLogFile("Layco Blending Interface started.");
                    // Start the timer that will periodically check the PLC.
                    TimerCallback timerTick = new TimerCallback(ElapsedTimer);
                    pollingTimer = new System.Threading.Timer(
                        timerTick, // delegate that is called each time the timer elapses
                        null, // object that is passed
                        3000, // amount of time for timmer to elapse the first time
                        Timeout.Infinite); // amount of time the timmer to elapse after the first time
                //}
            }
        }

        private void ElapsedTimer(object state)
        {
            if (plcCommsEnabled == true)
            {
                Thread checkingPlcThread = new Thread(new ThreadStart(checkPlc));
                checkingPlcThread.Start();
            }
            pollingTimer.Change(3000, Timeout.Infinite);
        }

        private void checkPlc()
        {
            // No PLC configured (demo / offline). Skip polling so an unhandled
            // socket error on this background thread can't crash the program.
            if (string.IsNullOrEmpty(LRP.Default.PlcIp) || LRP.Default.PlcIp == "0.0.0.0")
            { return; }
            lock (threadLockPlc)
            {
                try
                {
                    string ipAddress = LRP.Default.PlcIp;
                    softwareInterface.CheckPlc(ipAddress);
                }
                catch (TimeoutException ex)
                { writeToLogFile("Network timeout to the PLC. System error message :" + ex.Message, true); }
                catch (InvalidOperationException ex)
                { writeToLogFile("Invalid Operation. System error message: " + ex.Message, true); }
            }
        }

        /// <summary>
        /// Writes info to either the Log file or the Error Log file.
        /// </summary>
        /// <param name="message">The message that will be written to the log file.</param>
        /// <param name="isError">Indicates whether the message will be written to the error log.</param>
        private void writeToLogFile(string message, bool isError = false)
        {
            string logFile;
            displayNotificationBalloon(message);

            if (isError == false)
            { logFile = appData + "log.txt"; }// where the message will be written
            else // if the message is an error message
            {
                logFile = appData + "error_log.txt"; // where the message will be written
                // checks if the current error message is the
                // same as the last error message.
                if (lastError != message)
                {
                    lastError = message;
                    numErrors = 1;
                }
                else // if the current message is the same as the last error
                {
                    // This section minimizes the number of repeated error
                    // messages that are written to the log file.
                    numErrors++;
                    if (numErrors > 16)
                    { numErrors = 1; }
                    else if (numErrors > 3)
                    { return; }
                }
            }

            // write message to the appropriate location
            message = Convert.ToString(DateTime.Now) + " " + message;
            using (TextWriter tw = new StreamWriter(logFile, true))
            {
                try
                { tw.WriteLine(message); }
                catch (Exception ex)
                { MessageBox.Show("Problem writting to log file. " + ex.Message); }
                finally
                { tw.Close(); }
            }
        }

        private void displayNotificationBalloon(string message)
        {
            if (message.StartsWith("Network timeout to the PLC."))
            { notifyIcon.ShowBalloonTip(2000, "Connection Error", "Network timeout to the PLC", ToolTipIcon.Error); }
            else if (message.StartsWith("Error: Unable to establish a connection"))
            { notifyIcon.ShowBalloonTip(2000, "Connection Error", "Network timeout to the PLC", ToolTipIcon.Error); }
            else if (message.StartsWith("Invalid Operation. System error message: ExecuteReader: Connection property has not been initialized."))
            { notifyIcon.ShowBalloonTip(2000, "Error", "Could not connect to database.", ToolTipIcon.Error); }
            else if (message.StartsWith("Invalid Operation. System error message: ExecuteReader requires an open and available Connection. The connection's current state is Closed."))
            { notifyIcon.ShowBalloonTip(2000, "Error", "Could not connect to database.", ToolTipIcon.Error); }
            else if (message.StartsWith("Invalid Operation. System error message: The ConnectionString property has not been initialized."))
            { notifyIcon.ShowBalloonTip(2000, "Error", "Could not connect to database.", ToolTipIcon.Error); }
            else if (message.StartsWith("Requested recipe does not exist in the database."))
            { notifyIcon.ShowBalloonTip(2000, "Error", message, ToolTipIcon.Error); }
            else if (message.StartsWith("Product mismatch"))
            { notifyIcon.ShowBalloonTip(2000, "Error", message, ToolTipIcon.Error); }
        }

        #region Recipe
        public void setContainsTxt()
        {
            AutoCompleteStringCollection source = new AutoCompleteStringCollection();
            txtRecipeName.AutoCompleteCustomSource.Clear();
            txtHrecipeName.AutoCompleteCustomSource.Clear();
            List<string> names = db.getActiveProductNames();
            foreach (string name in names)
            { source.Add(name); }
            txtRecipeContains.AutoCompleteCustomSource = source;
            txtRecipeContains.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txtRecipeContains.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtHrecipeContains.AutoCompleteCustomSource = source;
            txtHrecipeContains.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txtHrecipeContains.AutoCompleteSource = AutoCompleteSource.CustomSource;
        }

        public void setRecipeNameTxt() 
        {
            AutoCompleteStringCollection source = new AutoCompleteStringCollection();
            txtRecipeName.AutoCompleteCustomSource.Clear(); 
            txtHrecipeName.AutoCompleteCustomSource.Clear();
            DataTable dt = db.getAllRecipeNames();
            foreach (DataRow dr in dt.Rows)
            { source.Add(dr[0].ToString()); }
            txtRecipeName.AutoCompleteCustomSource = source;
            txtRecipeName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txtRecipeName.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtHrecipeName.AutoCompleteCustomSource = source;
            txtHrecipeName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txtHrecipeName.AutoCompleteSource = AutoCompleteSource.CustomSource;
        }

        private void getRecipeNames()
        {
            lbxRecipeNames.Items.Clear();
            flpRecipeProds.Controls.Clear();
            DataTable dt = db.getAllRecipeNames();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                { lbxRecipeNames.Items.Add(dr[0].ToString()); }
                lbxRecipeNames.SelectedIndex = 0;
            }
            setContainsTxt();
            setRecipeNameTxt();
        }

        private void btnAddRecipe_Click(object sender, EventArgs e)
        {
            ModifyRecipes recipe = new ModifyRecipes(db);
            List<string> prods = db.getActiveProductNames();
            if (prods.Count > 0)
            { recipe.ShowDialog(); }
            else
            {MessageBox.Show("No products entered in Product Manager.");}

            getRecipeNames();
        }

        private void lbxRecipeNames_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!updatingRecipeList)
            {
                flpRecipeProds.Controls.Clear();

                int rId = db.getRid(lbxRecipeNames.SelectedItem.ToString());
                //get prods in recipe and then send to RecipeProdDisplay
                List<Product> prods = db.getRecipeProds(rId);
                foreach (Product prod in prods)
                {
                    RecipeProdDisplay rpd = new RecipeProdDisplay(prod);
                    flpRecipeProds.Controls.Add(rpd);
                }
            }
        }

        private void btnDeleteRecipe_Click(object sender, EventArgs e)
        {
            if (lbxRecipeNames.Items.Count > 0)
            {
                int rId = db.getRid(lbxRecipeNames.SelectedItem.ToString());
                ModifyRecipes delete = new ModifyRecipes(db, "Delete", rId, lbxRecipeNames.SelectedItem.ToString());
                delete.ShowDialog();
                getProducts();
            }
            getRecipeNames();
        }

        private void btnRecipeEdit_Click(object sender, EventArgs e)
        {
            if (lbxRecipeNames.Items.Count > 0)
            {
                string name = lbxRecipeNames.SelectedItem.ToString();
                int rId = db.getRid(name);
                if (!db.recipeRan(rId))
                {
                    ModifyRecipes edit = new ModifyRecipes(db, "Edit", rId, name);
                    edit.ShowDialog();
                    getProducts();
                }
                else
                { MessageBox.Show("The recipe, " + name +", has already been ran. Please make a new recipe."); }
            }
            getRecipeNames();
        }

        private void btnRecipeSort_Click(object sender, EventArgs e)
        {
            if (txtRecipeContains.Text != "")
            {
                updatingRecipeList = true;
                List<string> newNames = new List<string>();
                getRecipeNames();
                string id = db.getProdId(txtRecipeContains.Text);
                int yId = db.getYId(id);
                List<int> rIds = db.getRecipeForProd(yId);
                List<string> names = new List<string>();
                foreach (int rId in rIds)
                {
                    string name = db.getRecipeName(rId);
                    names.Add(name);
                }
                foreach (string recipe in lbxRecipeNames.Items)
                {
                    if (names.Contains(recipe))
                    { newNames.Add(recipe); }
                }
                lbxRecipeNames.Items.Clear();
                foreach (string name in newNames)
                { lbxRecipeNames.Items.Add(name); }
                if (lbxRecipeNames.Items.Count > 0)
                { lbxRecipeNames.SelectedIndex = 0; }
                updatingRecipeList = false;
            }
            else if (txtRecipeName.Text != "")
            {lbxRecipeNames.SelectedItem = txtRecipeName.Text;}
            else
            { getRecipeNames(); }
        }

        private void btnRecord_Click(object sender, EventArgs e)
        {
            int weight = 0;
            if (txtRunWeight.Text != "" && int.TryParse(txtRunWeight.Text, out weight))
            {
                int rId = db.getRid(lbxRecipeNames.SelectedItem.ToString());
                DateTime date = DateTime.Now;
                db.recordRun(rId, weight, date);
                List<Product> prods = new List<Product>();
                prods = db.getTicketProds(rId);
                MessageBox.Show("Recorded weight: " + txtRunWeight.Text + " \r\nRecipe: " + lbxRecipeNames.SelectedItem.ToString());
                if (LRP.Default.PrintSettings != "")
                {
                    PrintBatch print = new PrintBatch(lbxRecipeNames.SelectedItem.ToString(),
                        (float)Convert.ToDouble(txtRunWeight.Text), LRP.Default.PrintSettings, LRP.Default.useMetric, prods);
                    print.Show();
                    print.printTicket();
                }
                txtRunWeight.Text = "";
            }
            else
            { MessageBox.Show("Please enter a weight to record for the recipe."); }
        }
        #endregion Recipe

        #region Products
        void dgvProducts_SelectionChanged(object sender, System.EventArgs e)
        {
            if (dgvProducts.SelectedCells.Count > 0)
            {
                int selectedrowindex = dgvProducts.SelectedCells[0].RowIndex;
                DataGridViewRow selectedRow = dgvProducts.Rows[selectedrowindex];
                if (selectedRow.Cells[3].Value.ToString() != "0")
                { btnProductsRevisions.Enabled = true; }
                else
                { btnProductsRevisions.Enabled = false; }
                if (Convert.ToBoolean(selectedRow.Cells[4].Value))
                { btnProdUsed.Enabled = true; }
                else
                { btnProdUsed.Enabled = false; }
            }
        }

        private void getProducts()
        {
            dgvProducts.Controls.Clear();
            DataTable dt = db.getAllProds();
            if (dt.Rows.Count > 0)
            {dgvProducts.DataSource = dt;}
            setContainsTxt();
            setRecipeNameTxt();
        }

        private void btnProdAdd_Click(object sender, EventArgs e)
        {
            ModifyProducts add = new ModifyProducts(db);
            add.ShowDialog();
            getProducts();
        }

        private void btnProdEdit_Click(object sender, EventArgs e)
        {
            if (dgvProducts.Rows.Count > 0)
            {
                DataGridViewRow selectedRow = dgvProducts.Rows[dgvProducts.SelectedCells[0].RowIndex];
                string name = selectedRow.Cells[0].Value.ToString();
                ModifyProducts edit = new ModifyProducts(selectedRow.Cells[0].Value.ToString(), selectedRow.Cells[1].Value.ToString(),
                    selectedRow.Cells[2].Value.ToString(), "Edit", db);
                edit.ShowDialog();
                getProducts();
                getRecipeNames();
            }
        }

        private void btnProdDelete_Click(object sender, EventArgs e)
        {
            if (dgvProducts.Rows.Count > 0)
            {
                DataGridViewRow selectedRow = dgvProducts.Rows[dgvProducts.SelectedCells[0].RowIndex];
                string name = selectedRow.Cells[0].Value.ToString();
                ModifyProducts delete = new ModifyProducts(selectedRow.Cells[0].Value.ToString(), selectedRow.Cells[1].Value.ToString(),
                    selectedRow.Cells[2].Value.ToString(), "Delete", db);
                delete.ShowDialog();
                getProducts();
            }
        }

        private void btnProductRevisions_Click(object sender, EventArgs e)
        {
            DataGridViewRow selectedRow = dgvProducts.Rows[dgvProducts.SelectedCells[0].RowIndex];
            int yId = db.getYId(selectedRow.Cells[1].Value.ToString());
            ProductRevisions revision = new ProductRevisions(yId, db);
            revision.ShowDialog();
        }

        private void btnProdUsed_Click(object sender, EventArgs e)
        {
            DataGridViewRow selectedRow = dgvProducts.Rows[dgvProducts.SelectedCells[0].RowIndex];
            int yId = db.getYId(selectedRow.Cells[1].Value.ToString());
            InRecipes usedIn = new InRecipes(db, yId);
            usedIn.ShowDialog();
        }
        
        private void btnProdTotalRan_Click(object sender, EventArgs e)
        {
            DataGridViewRow selectedRow = dgvProducts.Rows[dgvProducts.SelectedCells[0].RowIndex];
            string name = selectedRow.Cells[0].Value.ToString();
            ProductTotalRan totalRan = new ProductTotalRan(db, name);
            totalRan.ShowDialog();
        }
        #endregion Products
                
        #region History
        private void setupHistory()
        {
            DataTable dt = db.getHistory();
            dgvHistory.DataSource = dt;
        }

        private void cbxDateRange_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxDateRange.SelectedText == "Specific Date Range")
            {
                dtpFromDate.Enabled = true;
                dtpToDate.Enabled = true;
            }
            else
            {
                dtpFromDate.Enabled = false;
                dtpToDate.Enabled = false;
            }
        }

        private void btnHistorySort_Click(object sender, EventArgs e)
        {
            DateTime fromDate = DateTime.Now;
            DateTime toDate = fromDate;
            string prod = "";
            string recipeName = "";
            bool useDate = false;
            bool useProd = false;
            bool useRecipe = false;

            if (cbxDateRange.SelectedItem == null)
            { cbxDateRange.SelectedIndex = 0; }

            TimeSpan timeSpan;

            // Get the date range
            if(cbxDateRange.SelectedItem.ToString() != "")
            {
                switch (cbxDateRange.SelectedItem.ToString())
                {
                    case ("Today"):
                        timeSpan = new TimeSpan(1, 0, 0, 0);
                        fromDate = fromDate.Subtract(timeSpan);
                        break;
                    case ("Last 7 Days"):
                        // Subtract 7 days from current time
                        timeSpan = new TimeSpan(7, 0, 0, 0);
                        fromDate = fromDate.Subtract(timeSpan);
                        break;
                    case ("Last 30 Days"):
                        // Subtract 30 days from current time
                        timeSpan = new TimeSpan(30, 0, 0, 0);
                        fromDate = fromDate.Subtract(timeSpan);
                        break;
                    case ("Last 90 Days"):
                        // Subtract 90 days from current time
                        timeSpan = new TimeSpan(90, 0, 0, 0);
                        fromDate = fromDate.Subtract(timeSpan);
                        break;
                    case ("Last 12 Months"):
                        // Subtract 365 days from current time
                        timeSpan = new TimeSpan(365, 0, 0, 0);
                        fromDate = fromDate.Subtract(timeSpan);
                        break;
                    case ("Specific Date Range"):
                        // Get toDate and fromDate from the dateTimePickers
                        fromDate = dtpFromDate.Value;
                        toDate = dtpToDate.Value;

                        // Make sure that the fromDate is BEFORE the toDate
                        if (fromDate > toDate)
                        {
                            MessageBox.Show("The From Date must be BEFORE the To Date.");
                            return;
                        }
                        break;
                }
                useDate = true;
            }

            if (txtHrecipeContains.Text != "")
            {
                useProd = true;
                prod = txtHrecipeContains.Text;
            }

            if (txtHrecipeName.Text != "")
            {
                useRecipe = true;
                recipeName = txtHrecipeName.Text;
            }

            DataTable dt = new DataTable();
            if (!useProd && !useRecipe && !useDate)
            { dt = db.getHistory(); }
            else
            { dt = db.getHistory(useProd, useRecipe, useDate, prod, recipeName, fromDate, toDate); }

            if (dt.Rows.Count > 0)
            {
                dgvHistory.DataSource = null;
                dgvHistory.Refresh();
                dgvHistory.DataSource = dt;
            }
            else
            { MessageBox.Show("No results returned. Please modify your search."); }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            SaveFileDialog exportFileDialog = new SaveFileDialog();
            exportFileDialog.Filter = "CSV | *.csv";
            exportFileDialog.Title = "Export Blend Run list to File";

            if (exportFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (exportFileDialog.FileName != "")
                {
                    //pull info from datagridview and put into excel (export in BlendTrackerDbSqlCe)
                    int cols;
                    //open file 
                    using (StreamWriter wr = new StreamWriter(exportFileDialog.FileName))
                    {
                        //determine the number of columns and write columns to file 
                        cols = dgvHistory.Columns.Count;
                        for (int i = 0; i < cols; i++)
                        { wr.Write(dgvHistory.Columns[i].Name.ToString().ToUpper() + ","); }
                        wr.WriteLine();

                        //write rows to excel file
                        for (int i = 0; i < (dgvHistory.Rows.Count); i++)
                        {
                            for (int j = 0; j < cols; j++)
                            {
                                if (dgvHistory.Rows[i].Cells[j].Value != null)
                                { wr.Write(dgvHistory.Rows[i].Cells[j].Value + ","); }
                                else
                                { wr.Write(","); }
                            }
                            wr.WriteLine();
                        }
                    }
                }
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            printPreviewDialog1.Document = printDocument1;
            printDocument1.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.PrintPage);
            Margins margin = new Margins(50, 25, 25, 25);
            printDocument1.Print();
        }

        private void PrintPage(object sender, PrintPageEventArgs e)
        {
            int rowPosition = 25;

            // draw headers
            DrawHeader(e.Graphics, ref rowPosition);
            rowPosition += 40;

            // draw each row
            DrawGridBody(e.Graphics, rowPosition, e);
        }

        private int row = 0;

        private void DrawHeader(Graphics g, ref int y_value)
        {
            int x_value = 0;
            Font bold = new Font(this.Font, FontStyle.Bold);

            foreach (DataGridViewColumn dc in dgvHistory.Columns)
            {
                if (dgvHistory.Columns[2] == dc)
                { x_value += 30; }
                g.DrawString(dc.HeaderText, bold, Brushes.Black, (float)x_value, (float)y_value);
                x_value += dc.Width + 5;
            }
        }

        private void DrawGridBody(Graphics g, int y_value, PrintPageEventArgs e)
        {
            int x_value;
            int rowCount = 0;
            while (dgvHistory.Rows.Count > row)
            {
                DataRow dr = ((DataTable)dgvHistory.DataSource).Rows[row];
                x_value = 0;

                // draw a solid line
                g.DrawLine(Pens.Black, new Point(0, y_value), new Point(this.Width, y_value));

                foreach (DataGridViewColumn dc in dgvHistory.Columns)
                {
                    if (dgvHistory.Columns[2] == dc)
                    { x_value += 30; }
                    string text = dr[dc.DataPropertyName].ToString();
                    g.DrawString(text, this.Font, Brushes.Black, (float)x_value, (float)y_value + 10f);
                    x_value += dc.Width + 5;
                }
                y_value += 40;

                if (rowCount >= 24)
                {
                    e.HasMorePages = true;
                    rowCount = 0;
                    return;
                }
                else if (rowCount < dgvHistory.Rows.Count)
                { e.HasMorePages = false; }

                row++;
                rowCount++;
            }
        }
        
        private void btnViewPrinted_Click(object sender, EventArgs e)
        {
            if (dgvHistory.Rows.Count > 0)
            {
                DataGridViewRow selectedRow = dgvHistory.Rows[dgvHistory.SelectedCells[0].RowIndex];
                string name = selectedRow.Cells[0].Value.ToString();
                DateTime date =Convert.ToDateTime(selectedRow.Cells[1].Value);

                string day = date.ToShortDateString();
                string time = date.ToShortTimeString();
                day = day.Replace("/", ".");
                time=time.Replace(":", ".");
                string path = LRP.Default.AppData + "\\PrintTickets\\" + name + "_" + day +"_" +time + ".jpg";
                if (File.Exists(path))
                {
                    ViewTicket view = new ViewTicket(path);
                    view.Show();
                }
                else
                {
                    MessageBox.Show("Ticket not found.");
                    this.Close();
                }
            }
        }
        #endregion History

        #region settings
        private void setupSettings()
        {
            btns.Clear();
            foreach (Button b in pnlHoppers.Controls)
            { btns.Add(b); }

            txtIp.Text = LRP.Default.PlcIp;
            lblNumHoppers.Text = LRP.Default.NumHoppers.ToString();

            if (LRP.Default.useMetric)
            { rbtnKg.Checked = true; }

            if (!string.IsNullOrWhiteSpace(LRP.Default.PlcIp) && LRP.Default.PlcIp != "0.0.0.0")
            {
                int len = btns.Count;
                for (int i = 1; i <= LRP.Default.NumHoppers; i++)
                {
                    Button btn = btns[len - i];
                    if (!btn.Visible)
                    {
                        btn.Visible = true;
                        btn.Enabled = true;
                        btn.MouseClick += new MouseEventHandler(b_MouseClick);
                    }
                    btn.Text = "";
                    string name = plc.GetHopperNames(i);
                    btn.Text = "Hopper " + i + ":\r\n" + name;
                    if (name.Length > 10)
                    { toolTip1.SetToolTip(btn, name); }
                }
                for (int j = Convert.ToInt16(lblNumHoppers.Text) + 1; j < btns.Count; j++)
                {
                    Button btn = btns[len - j];
                    if (btn.Visible)
                    {
                        btn.Visible = false;
                        btn.Enabled = false;
                        btn.MouseClick -= new MouseEventHandler(b_MouseClick);
                    }
                }
            }
            else
            { checkPlcConn();}

            if (Directory.Exists(LRP.Default.AppData + "\\Backup"))
            {
                DirectoryInfo dirInfo = new DirectoryInfo(LRP.Default.AppData + "\\Backup");
                FileInfo[] savedDbList = dirInfo.GetFiles("*.sdf");
                if (savedDbList.Count() > 0)
                { btnViewDbBu.Enabled = true; }
                else
                { btnViewDbBu.Enabled = false; }
            }
            else
            { btnViewDbBu.Enabled = false; }
        }

        public void checkPlcConn()
        {
            if (LRP.Default.PlcIp != "0.0.0.0" && TcpConnection.TestConnection(txtIp.Text, 502, 300))
            {
                lblConnect.Text = "Connected";
                lblConnect.ForeColor = Color.DarkGreen;
                btnVerify.Enabled = true;
            }
            else
            {
                lblConnect.Text = "Not Connected";
                lblConnect.ForeColor = Color.DarkRed;
                btnVerify.Enabled = false;
            }
        }

        void b_MouseClick(object sender, MouseEventArgs e)
        {
            Button btn = (Button)sender;
            string bName = btn.Name;
            int i = Convert.ToInt16(bName.Remove(0, 6));
            HopperNameChange hnc = new HopperNameChange(i);
            hnc.ShowDialog();
            setupSettings();
        }

        private void btnVerify_Click(object sender, EventArgs e)
        {
            int numberOfHoppers = 0;
            string plcIpAddress = txtIp.Text;
            int minimumNumberOfHoppers = 1;
            int maximumNumberOfHoppers = 32;

            try
            {
                //.TestConnection(ip, modbus, timeout)
                if (LRP.Default.PlcIp != "0.0.0.0" && TcpConnection.TestConnection(plcIpAddress, 502, 300))
                {
                    PlcTcpMaster plcConnection = new PlcTcpMaster(plcIpAddress);
                    numberOfHoppers = Convert.ToInt32(plcConnection.readFloat(LRP.Default.MLNumHoppers));
                    // Check to make sure that the value is greater than 0 and less than 32.
                    if ((numberOfHoppers >= minimumNumberOfHoppers) && (numberOfHoppers <= maximumNumberOfHoppers))
                    {
                        lblNumHoppers.Text = Convert.ToString(numberOfHoppers);
                        lblConnect.Text = "Connected";
                        lblConnect.ForeColor = Color.DarkGreen;
                    }
                    LRP.Default.PlcIp = txtIp.Text;
                    LRP.Default.NumHoppers = numberOfHoppers;
                    LRP.Default.Save();
                    setupSettings();
                }
                else
                {
                    lblConnect.Text = "No Connection";
                    lblConnect.ForeColor = Color.DarkRed;
                }
            }
            catch (TimeoutException ex)
            { }
        }

        private void btnTicket_Click(object sender, EventArgs e)
        {
            SetupTicket ticket = new SetupTicket(LRP.Default.PrintSettings, LRP.Default.useMetric);
            ticket.ShowDialog();
            string path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)
                + @"\Yargus\BatchTicket.txt";
            if (File.Exists(path))
            { 
                LRP.Default.PrintSettings = path;
                LRP.Default.Save();
            }
        }

        private void rbtnLbs_CheckedChanged(object sender, EventArgs e)
        {
            if (LRP.Default.useMetric)
            {
                LRP.Default.useMetric = false;
                LRP.Default.Save();
            }
        }

        private void rbtnKg_CheckedChanged(object sender, EventArgs e)
        {
            if (!LRP.Default.useMetric)
            {
                LRP.Default.useMetric = true;
                LRP.Default.Save();
            }
        }

        private void btnBackupDb_Click(object sender, EventArgs e)
        {
            Backup manage = new Backup(db);
            manage.ShowDialog();
            setupSettings();
        }

        private void btnImportDb_Click(object sender, EventArgs e)
        {
            Restore manage = new Restore();
            manage.ShowDialog();
            setupSettings();
        }

        private void btnViewDbBu_Click(object sender, EventArgs e)
        {
            DbViewer view = new DbViewer();
            view.Show();
        }

        private void btnOpenFolder_Click(object sender, EventArgs e)
        {
            string folderPath = LRP.Default.AppData + "\\PrintTickets";
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = folderPath;
            p.Start();
        }
        #endregion Settings
    }
}

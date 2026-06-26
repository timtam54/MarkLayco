using Database;
using GeneralData;
using History;
using Products;
using Recipes;
using Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LaycoRecipeProgram
{
    public partial class DbViewer : Form
    {
        DatabaseViewer db;
        bool updatingRecipeList = false;

        public DbViewer()
        {
            InitializeComponent();

            DirectoryInfo dirInfo = new DirectoryInfo(LRP.Default.AppData +"\\Backup");
            FileInfo[] savedDbList = dirInfo.GetFiles("*.sdf");
            List<string> dbList = new List<string>();
            List<string> rlbTicketList = new List<string>();

            foreach (FileInfo ticket in savedDbList)
            { dbList.Add(Path.GetFileNameWithoutExtension(ticket.Name)); }

            foreach (string data in dbList)
            { cbxDb.Items.Add(data); }

            cbxDb.SelectedIndex = 0;
            getRecipeNames();
            setContainsTxt();
            setRecipeNameTxt();
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

        private void cbxDb_SelectedIndexChanged(object sender, EventArgs e)
        { 
            db = new DatabaseViewer(LRP.Default.AppData +"\\Backup\\" + cbxDb.SelectedItem.ToString() +".sdf");
            lblFrom.Text = db.getFromDate().ToString();
            lblTo.Text = db.getToDate().ToString();
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
            DataTable dt = db.getAllRecipeNames();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                { lbxRecipeNames.Items.Add(dr[0].ToString()); }
                lbxRecipeNames.SelectedIndex = 0;
            }
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
            { lbxRecipeNames.SelectedItem = txtRecipeName.Text; }
            else
            { getRecipeNames(); }
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
            { dgvProducts.DataSource = dt; }
        }
        
        private void btnProductsRevisions_Click(object sender, EventArgs e)
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
            if (cbxDateRange.SelectedItem.ToString() != "")
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
            if (!useProd && !useRecipe && !useProd)
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
                DateTime date = Convert.ToDateTime(selectedRow.Cells[1].Value);

                string day = date.ToShortDateString();
                string time = date.ToShortTimeString();
                day = day.Replace("/", ".");
                time = time.Replace(":", ".");
                string path = LRP.Default.AppData + "\\PrintTickets\\" + name + "_" + day + "_" + time + ".jpg";
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
    }
}

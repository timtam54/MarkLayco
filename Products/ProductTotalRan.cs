using Database;
using Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Products
{
    public partial class ProductTotalRan : Form
    {
        Databases db;
        DatabaseViewer dbv;
        string prod;
        public ProductTotalRan(Databases db, string prod)
        {
            InitializeComponent();
            this.db = db;
            this.prod = prod;
            if(LRP.Default.useMetric)
            {lblUom.Text = "Kilograms:";}
            else
            { lblUom.Text = "Pounds:"; }
            label1.Text = prod;
        }

        public ProductTotalRan(DatabaseViewer dbv, string prod)
        {
            InitializeComponent();
            this.dbv = dbv;
            this.prod = prod;
            if (LRP.Default.useMetric)
            { lblUom.Text = "Kilograms:"; }
            else
            { lblUom.Text = "Pounds:"; }
            label1.Text = prod;
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

        private void btnFind_Click(object sender, EventArgs e)
        {
            DateTime fromDate = DateTime.Now;
            DateTime toDate = fromDate;

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
            }

            DataTable dt = new DataTable();
            if (db != null)
            { dt = db.getHistory(prod, fromDate, toDate); }
            else
            { dt = dbv.getHistory(prod, fromDate, toDate); }

            if (dt.Rows.Count > 0)
            {
                float total = 0;
                float prodWeight = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    if (float.TryParse(dr[prod].ToString(), out prodWeight))
                    {total += prodWeight;}
                }
                lblWeight.Text = total.ToString();
                if (LRP.Default.useMetric)
                { lblTons.Text = (total / 1000).ToString(); }
                else
                { lblTons.Text = (total / 2000).ToString(); }
            }
            else
            { MessageBox.Show("No results returned. Please modify your search."); }

        }
    }
}

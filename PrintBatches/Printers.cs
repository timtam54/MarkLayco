using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace PrintBatches
{
    public partial class Printers : Form
    {
        List<string> printers = new List<string>();
        int printNum = 0;
        int locY = 0;
        string printSettings;

        public Printers(List<string>printers, string printSettings)
        {
            InitializeComponent();
            foreach (String printer in PrinterSettings.InstalledPrinters)
            { clbPrinters.Items.Add(printer.ToString()); }
            this.printers = printers;
            this.printSettings = printSettings;
            if (printers.Count > 0)
            { setupPrint(); }
        }

        private void setupPrint()
        {
            List<string> names = new List<string>();
            int count = 0;
            foreach (string p in printers)
            {
                if (!names.Contains(p))
                { names.Add(p); }
            }

            foreach (string n in names)
            {
                count = printers.Count(i => i == n);
                if (clbPrinters.Items.Contains(n))
                {
                    int index = clbPrinters.Items.IndexOf(n);
                    clbPrinters.SetItemChecked(index, true);
                }

                pnlPrinters.Controls.Add(new Label()
                {
                    Name = n,
                    Text = n,
                    Location = new Point(10, locY),
                    Size = new Size(100, 15),
                    TextAlign = ContentAlignment.MiddleLeft
                });
                pnlPrinters.Controls.Add(new TextBox()
                {
                    Name = n,
                    Text = count.ToString(),
                    Location = new Point(125, locY),
                    Size = new Size(25, 15)
                });
                count = 0;
            }
        }

        private void clbPrinters_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            int num = clbPrinters.Items.Count;
            if (e.NewValue == CheckState.Checked)
            {
                printNum++;
                int i = 1;
                foreach (Control c in pnlPrinters.Controls)
                {
                    if (c is Label)
                    {
                        i++;
                        locY = c.Location.Y + 20;
                    }
                }

                if (i == 1)
                { locY = 5; }

                if (clbPrinters.SelectedItem != null)
                {
                    pnlPrinters.Controls.Add(new Label()
                    {
                        Name = clbPrinters.SelectedItem.ToString(),
                        Text = clbPrinters.SelectedItem.ToString(),
                        Location = new Point(10, locY),
                        Size = new Size(100, 15),
                        TextAlign = ContentAlignment.MiddleLeft
                    });
                    pnlPrinters.Controls.Add(new TextBox()
                    {
                        Name = clbPrinters.SelectedItem.ToString(),
                        Text = "1",
                        Location = new Point(125, locY),
                        Size = new Size(25, 15)
                    });
                }
            }
            else
            {
                int i = 0;
                foreach (Control c in pnlPrinters.Controls)
                {
                    if (c is Label)
                    {
                        i++;
                        if (clbPrinters.SelectedItem.ToString() == c.Text)
                        {
                            pnlPrinters.Controls.Remove(c);
                            foreach (Control t in pnlPrinters.Controls)
                            {
                                if (t is TextBox)
                                {
                                    if (t.Name == clbPrinters.SelectedItem.ToString())
                                    { pnlPrinters.Controls.Remove(t); }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string settings = "";
            if (File.Exists(printSettings))
            {
                using (StreamReader ticketFile = new StreamReader(printSettings))
                {
                    string line;
                    while ((line = ticketFile.ReadLine()) != null)
                    {
                        string[] items = line.Split('\t');
                        if (items.Length > 2)
                        { settings += line + "\r\n"; }
                    }
                    foreach (Control c in pnlPrinters.Controls)
                    {
                        if (c is TextBox)
                        {
                            string printer = c.Name.ToString();
                            int numCopies = 0;
                            if (int.TryParse(c.Text.ToString(), out numCopies))
                            {
                                for (int i = 0; i < numCopies; i++)
                                { settings += printer + "\r\n"; }
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (Control c in pnlPrinters.Controls)
                {
                    if (c is TextBox)
                    {
                        string printer = c.Name.ToString();
                        int numCopies = 0;
                        if (int.TryParse(c.Text.ToString(), out numCopies))
                        {
                            for (int i = 0; i < numCopies; i++)
                            { settings += printer + "\r\n"; }
                        }
                    }
                }
            }

            using (StreamWriter sw = new StreamWriter(printSettings))
            { sw.WriteLine(settings); }
            this.Close();
        }
    }
}

using GeneralData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PrintBatches
{
    public partial class PrintBatch : Form
    {
        string recipe;
        float ranWeight;
        string printSettings;
        string ticketInPath;
        int batch;
        string logoPath;
        List<string> printerList = new List<string>();
        List<Product> products = new List<Product>();
        float reqWeight = 0;
        string picPath;
        bool metric;
        
        public PrintBatch(string recipe, float ranWeight, string printSettings, float reqWeight, bool metric, List<Product> prods)
        {
            InitializeComponent();
            this.recipe = recipe;
            this.ranWeight = ranWeight;
            this.reqWeight = reqWeight;
            this.printSettings = printSettings;
            this.metric = metric;
            this.products = prods;
            setupTicket();
        }

        public PrintBatch(string recipe, float ranWeight, string printSettings, bool metric, List<Product> prods)
        {
            InitializeComponent();
            this.recipe = recipe;
            this.ranWeight = ranWeight;
            this.printSettings = printSettings;
            this.products = prods;
            setupTicket();
        }

        public void setupTicket()
        {
            string line = "";
            string filePath = printSettings;
            List<Panel> spl = new List<Panel>();
            List<string> spnl = new List<string>();
            if (File.Exists(filePath))
            {
                if (File.Exists(printSettings))
                {
                    using (StreamReader sr = new StreamReader(printSettings))
                    {
                        while ((line = sr.ReadLine()) != null)
                        {
                            string[] items = line.Split('\t');
                            if (items[0] == "SizablePanel")
                            {
                                Panel p = new Panel();
                                p.Name = items[1];
                                p.Location = new Point(Convert.ToInt16(items[3]), Convert.ToInt16(items[4]));
                                p.Size = new Size(Convert.ToInt16(items[5]), Convert.ToInt16(items[6]));
                                pnlTicket.Controls.Add(p);
                                spl.Add(p);
                                spnl.Add(p.Name);
                            }
                            else if (items[0].Contains("Label"))
                            {
                                Label l = new Label();
                                l.Name = items[1];
                                l.Location = new Point(Convert.ToInt16(items[3]), Convert.ToInt16(items[4]));
                                l.Size = new Size(Convert.ToInt16(items[5]), Convert.ToInt16(items[6]));
                                if (!items[1].Contains("Info"))
                                { l.Text = items[2]; }
                                foreach (Panel p in pnlTicket.Controls)
                                {
                                    if (p.Name == items[7])
                                    {
                                        if (l.Name.Contains("Info"))
                                        { fillInTicket(l, p); }
                                        l.Font = new Font(l.Font.FontFamily, 8);
                                        p.Controls.Add(l);
                                        if (p.Name == "splSign" && l.Name.Contains("Info"))
                                        { l.BorderStyle = BorderStyle.FixedSingle; }
                                        break;
                                    }
                                }
                            }
                            else if (items[0].Contains("PictureBox"))
                            {
                                foreach (Panel p in pnlTicket.Controls)
                                {
                                    if (p.Name == items[7])
                                    {
                                        PictureBox pic = new PictureBox();
                                        if (File.Exists(items[2]))
                                        { pic.Image = Image.FromFile(items[2]); }
                                        pic.Name = items[1];
                                        pic.Location = new Point(Convert.ToInt16(items[3]), Convert.ToInt16(items[4]));
                                        pic.Size = new Size(Convert.ToInt16(items[5]), Convert.ToInt16(items[6]));
                                        p.Controls.Add(pic);
                                        break;
                                    }
                                }
                            }
                            else if (items[0].Contains("TextBox"))
                            {
                                foreach (Panel p in pnlTicket.Controls)
                                {
                                    if (p.Name == items[7])
                                    {
                                        Label l = new Label();
                                        string address = items[2];
                                        if (address.Contains("~"))
                                        { address = address.Replace("~", "\r\n"); }
                                        l.Text = address;
                                        l.AutoSize = true;
                                        l.Font = new Font(l.Font.FontFamily, 12);
                                        p.Controls.Add(l);
                                    }
                                }
                            }
                            else if (items.Length < 2 && items[0] != "")
                            { printerList.Add(items[0]); }
                        }
                    }
                }
            }
        }

        public void fillInTicket(Label l, Panel p)
        {
            if (l.Name.Contains("prod") || l.Name.Contains("ratio") ||l.Name.Contains("Id")||
                l.Name.Contains("pounds") ||l.Name.Contains("kilograms") || l.Name.Contains("tons"))
            { getProdFields(l, p); }
            else if (l.Name.Contains("date") || l.Name.Contains("time") || l.Name.Contains("Requested") ||
                l.Name.Contains("Ran") || l.Name.Contains("recipe"))
            { getSystemFields(l, p); }
        }

        public void getProdFields(Label l, Panel p)
        {
            int x = 0;
            int y = 0;
            int w = 0;
            int h = 0;
            List<string> names = new List<string>();
            List<string> ratios = new List<string>();
            List<string> ids = new List<string>();

            List<string> use = new List<string>();
            foreach (Product prod in products)
            {
                names.Add(prod.Name);
                ids.Add(prod.Id);
                ratios.Add(Convert.ToString(prod.Ratio));
            }

            if (l.Name.Contains("Name"))
            { use = names; }
            else if (l.Name.Contains("Id"))
            { use = ids; }
            else if (l.Name.Contains("ratio"))
            { use = ratios; }
            else if (l.Name.Contains("pounds"))
            {
                foreach (string ratio in ratios)
                {
                    float ranPounds = ranWeight * (float)Convert.ToDouble(ratio);
                    use.Add(ranPounds.ToString());
                }
            }
            else if (l.Name.Contains("tons"))
            {
                foreach (string ratio in ratios)
                {
                    float ranTons = 0;
                    float prodWeight = ranWeight * (float)Convert.ToDouble(ratio);
                    if (!metric)
                    { ranTons = (prodWeight/ 2000); }
                    else
                    { ranTons = (prodWeight / 1000); }
                    use.Add(ranTons.ToString());
                }
            }
            for (int i = 0; i < use.Count(); i++)
            {
                if (i == 0)
                {
                    l.Text = use[i];
                    x = l.Location.X;
                    y = l.Location.Y;
                    w = l.Size.Width;
                    h = l.Size.Height;

                    l.Font = new Font(l.Font.FontFamily, 8);
                }
                else
                {
                    Label label = new Label();
                    label.Text = use[i];
                    label.Location = new Point(x, y + 25);
                    label.Size = new Size(w, h);
                    p.Controls.Add(label);
                    if (p.Height < label.Location.Y + label.Height)
                    {p.Height = label.Location.Y + label.Height + 5;}
                    x = label.Location.X;
                    y = label.Location.Y;
                    l.Font = new Font(l.Font.FontFamily, 10);
                }
            }
            use.Clear();
            foreach (Control c in pnlTicket.Controls)
            {
                if (c.Name == "splSign")
                {
                    if (c.Location.Y - 10 < p.Location.Y + p.Size.Height)
                    {
                        c.Location = new Point(c.Location.X, p.Location.Y + p.Size.Height + 5);
                        pnlTicket.Size = new Size(pnlTicket.Size.Width, c.Location.Y + c.Size.Height + 50); 
                        this.Size = new Size(this.Size.Width, pnlTicket.Size.Height +10);
                    }
                }
            }
        }

        public void getSystemFields(Label l, Panel p)
        {
            if (l.Name.Contains("time"))
            { l.Text = DateTime.Now.ToString("h:mm:ss"); }
            else if (l.Name.Contains("date"))
            { l.Text = DateTime.Now.ToShortDateString(); }
            else if (l.Name.Contains("Requested"))
            {
                if (reqWeight != 0)
                { l.Text = reqWeight.ToString(); }
            }
            else if (l.Name.Contains("Ran"))
            { l.Text = ranWeight.ToString(); }
            else if (l.Name.Contains("recipe"))
            { l.Text = recipe.ToString(); }
        }

        public void printTicket()
        {
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            int x = SystemInformation.WorkingArea.X;
            int y = SystemInformation.WorkingArea.Y;
            int width = this.ClientSize.Width;
            int height = this.ClientSize.Height;

            Rectangle bounds = new Rectangle(x, y, width, height);

            Bitmap img = new Bitmap(width, height);

            this.DrawToBitmap(img, bounds);

            string path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) 
                    + @"\Yargus\LRP\PrintTickets\";
            if (!Directory.Exists(path))
            { Directory.CreateDirectory(path); }
            string day = DateTime.Now.ToShortDateString();
            day = day.Replace("/", "-");
            string time = DateTime.Now.ToShortTimeString();
            time = time.Replace(":", "-");
            string name = recipe.Replace("/", "").Replace("\\", "").Replace("|", "").Replace("*", "").Replace("<", "").Replace(">", "").Replace(":", "").Replace("\"", "").Replace("?", "");
            picPath = Path.Combine(path, name + "_" + day + "_" + time + ".jpg");
            img.Save(picPath, ImageFormat.Jpeg);
            print();
        }

        private void print()
        {
            if (printerList.Count > 0)
            {
                foreach (string printer in printerList)
                {
                    printDocument1.PrintPage += PrintPage;
                    printDocument1.PrinterSettings.PrinterName = printer;
                    printDocument1.Print();
                }
            }
            this.Close();
        }

        private void PrintPage(object o, PrintPageEventArgs e)
        {
            Image img = Image.FromFile(picPath);
            e.Graphics.DrawImage(img, 15, 25);
        }
    }
}


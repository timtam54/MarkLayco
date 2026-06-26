using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PrintBatches
{
    public partial class SetupTicket : Form
    {
        #region variables
        List<string> printerList = new List<string>();
        List<string> fieldNames = new List<string>();
        List<Control> ticketC = new List<Control>();
        List<Control> prodC= new List<Control>();
        List<Control> customerC= new List<Control>();
        List<Control> signC= new List<Control>();
        List<Control> prodDefault= new List<Control>();
        private Control activeControl;
        private Point previousLocation;
        private bool resize = false;
        string logoPath;
        string printSettings;
        #endregion variables
        
        public SetupTicket(string printSettings, bool useMetric)
        {
            InitializeComponent();
            this.printSettings = printSettings;
            if (useMetric)
            {
                TreeNode pn = treeView1.Nodes["pnProducts"];
                TreeNode tn = pn.Nodes["cnPounds"];
                tn.Text = "Kilograms";
            }
            if (File.Exists(printSettings))
            { setupTicket(); }
            else
            { setupRequiredPanels(); }
        }

        public void setupTicket()
        {
            string line = "";
            if (File.Exists(printSettings))
            {
                using (StreamReader sr = new StreamReader(printSettings))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] items = line.Split('\t');
                        if (items[0] == "SizablePanel")
                        {
                            foreach (SizablePanel p in pnlContainer.Controls)
                            {
                                if (p.Name == items[1])
                                {
                                    p.Location = new Point(Convert.ToInt16(items[3]), Convert.ToInt16(items[4]));
                                    p.Size = new Size(Convert.ToInt16(items[5]), Convert.ToInt16(items[6]));
                                    break;
                                }
                            }
                        }
                        else if (items[0].Contains("Label"))
                        {
                            Label l = new Label();
                            l.Name = items[1];
                            l.Location = new Point(Convert.ToInt16(items[3]), Convert.ToInt16(items[4]));
                            l.Size = new Size(Convert.ToInt16(items[5]), Convert.ToInt16(items[6]));
                            l.Text = items[2];
                            fieldNames.Add(l.Name);
                            foreach (SizablePanel p in pnlContainer.Controls)
                            {
                                if (p.Name == items[7])
                                {
                                    if (p.Name == "splSign" && l.Name.Contains("Info"))
                                    { l.BorderStyle = BorderStyle.FixedSingle; }
                                    p.Controls.Add(l);
                                    addToList(p, l);
                                    break;
                                }
                            }
                        }
                        else if (items[0].Contains("PictureBox"))
                        {
                            foreach (SizablePanel p in pnlContainer.Controls)
                            {
                                if (p.Name == items[7])
                                {
                                    if (File.Exists(items[2]))
                                    { pictureBox1.Image = Image.FromFile(items[2]); }
                                    pictureBox1.Name = items[1];
                                    pictureBox1.Location = new Point(Convert.ToInt16(items[3]), Convert.ToInt16(items[4]));
                                    pictureBox1.Size = new Size(Convert.ToInt16(items[5]), Convert.ToInt16(items[6]));
                                    break;
                                }
                            }
                        }
                        else if (items[0].Contains("TextBox"))
                        {
                            string address = items[2];
                            if (address.Contains("~"))
                            { address = address.Replace("~", "\r\n"); }
                            location.Text = address;
                        }
                        else if (items.Length < 2 && items[0] != "")
                        { printerList.Add(items[0]); }
                    }
                }
                foreach (SizablePanel p in pnlContainer.Controls)
                {
                    p.MouseDown += new MouseEventHandler(mouseDown);
                    p.MouseMove += new MouseEventHandler(mouseMove);
                    p.MouseUp += new MouseEventHandler(mouseUp);
                    p.DragEnter += new DragEventHandler(dragEnter);
                    p.DragDrop += new DragEventHandler(dragDrop);
                }
            }
        }

        #region methods
        public void setupRequiredPanels()
        {
            foreach (SizablePanel p in pnlContainer.Controls)
            {
                p.MouseDown += new MouseEventHandler(mouseDown);
                p.MouseMove += new MouseEventHandler(mouseMove);
                p.MouseUp += new MouseEventHandler(mouseUp);
                p.DragEnter += new DragEventHandler(dragEnter);
                p.DragDrop += new DragEventHandler(dragDrop);

                foreach (Control c in p.Controls)
                { setupRequiredFields(c, p); }
            }
        }

        public void setupRequiredFields(Control c, SizablePanel p)
        {
            c.MouseDown += new MouseEventHandler(mouseDown);
            c.MouseMove += new MouseEventHandler(mouseMove);
            c.MouseUp += new MouseEventHandler(mouseUp);
            if (c is Label)
            {
                Label c1 = (Label)c;
                addToList(p, c1);
            }
        }

        void addFields(TreeNode tn)
        {
            foreach (Control c in pnlContainer.Controls)
            {
                if (c is SizablePanel && tn.Parent.Tag.ToString() == c.Name)
                {
                    c.AllowDrop = true;
                    addControls(tn, (SizablePanel)c);
                    break;
                }
            }
        }

        void setLblText(Label c2)
        {
            //available to all
            if (c2.Text == "Date")
            { c2.Text = DateTime.Today.ToShortDateString(); }
            if (c2.Text == "Time")
            { c2.Text = DateTime.Today.ToShortTimeString(); }
            if (c2.Text == "Total Requested")
            { c2.Text = "9900"; }
            if (c2.Text == "Total Weight Ran")
            { c2.Text = "9950"; }
            if (c2.Text == "Pounds")
            { c2.Text = "5800"; }
            if (c2.Text == "Kilograms")
            { c2.Text = "4975"; }
            if (c2.Text == "Product Description")
            { c2.Text = "11-52-10"; }
            if (c2.Text == "Ratio")
            { c2.Text = "1"; }
            if (c2.Text == "Tons")
            { c2.Text = "4.975"; }
        }

        List<Control> getControlList(SizablePanel pnl)
        {
            List<Control> controls = new List<Control>();
            if (pnl.Name == "splRecipe")
            { controls = ticketC; }
            if (pnl.Name == "splProds")
            { controls = prodC; }
            if (pnl.Name == "splSign")
            { controls = signC; }
            return controls;
        }

        void addToList(SizablePanel pnl, Label c1)
        {
            if (pnl.Name == "splRecipe")
            {ticketC.Add(c1); }
            else if (pnl.Name == "splProds")
            { prodC.Add(c1); }
            else if (pnl.Name == "splSign")
            { signC.Add(c1); }
        }

        void addControls(TreeNode tn, SizablePanel pnl)
        {
            List<Control> c = getControlList(pnl);
            if (!fieldNames.Contains(tn.Tag.ToString()))
            {
                string name = pnl.Name;
                Label c1 = new Label();
                c1.Text = tn.Text;
                c1.Name = tn.Tag.ToString();
                c1.MouseDown += new MouseEventHandler(mouseDown);
                c1.MouseMove += new MouseEventHandler(mouseMove);
                c1.MouseUp += new MouseEventHandler(mouseUp);
                pnl.Controls.Add(c1); 
                fieldNames.Add(c1.Name);
                Label c2 = new Label();
                c2.Name = tn.Tag.ToString() + "Info";
                c2.Text = tn.Text.ToString();
                c2.MouseDown += new MouseEventHandler(mouseDown);
                c2.MouseMove += new MouseEventHandler(mouseMove);
                c2.MouseUp += new MouseEventHandler(mouseUp);
                if(tn.Parent.Name.Contains("Sign"))
                {
                    c2.Size = new Size(c2.Size.Width * 2, 2);
                    c2.BorderStyle = BorderStyle.FixedSingle;
                }
                pnl.Controls.Add(c2);
                fieldNames.Add(c2.Name);
                addToList(pnl, c1);
                addToList(pnl, c2);

                setLblText(c2);

                //own method?
                int i = c.Count - 1;
                if (pnl.Name != "splProds" && pnl.Name != "splSign")
                {
                    if (c.Count > 2)
                    { c1.Location = new Point(c[i].Location.X, (c.Count / 2) * 25 - 25); }
                    c2.Location = new Point(c1.Location.X + 100, c1.Location.Y);
                }
                else if (pnl.Name == "splSign")
                {
                    if (c.Count > 4)
                    {c1.Location = new Point(c[i].Location.X + 350, ((c.Count -2) /2) * 25 + 25); }
                    else if (c.Count > 2)
                    { c1.Location = new Point(c[i].Location.X + 15, (c.Count / 2) * 25 + 25); }
                    else
                    { c1.Location = new Point(c[i].Location.X + 15, 25); }
                    c2.Location = new Point(c1.Location.X + 100, c1.Location.Y + c1.Size.Height-10);
                    
                }
                else
                {
                    if (c.Count == 6)
                    {
                        c1.Location = new Point(260, 17);
                        c2.Location = new Point(260, 44);
                    }
                    else if (c.Count == 8)
                    {
                        c1.Location = new Point(380, 17);
                        c2.Location = new Point(380, 44);

                    }
                    else if (c.Count == 10)
                    {
                        c1.Location = new Point(500, 17);
                        c2.Location = new Point(500, 44);
                    }
                    else
                    {
                        c1.Location = new Point(620, 17);
                        c2.Location = new Point(620, 44);
                    }
                }

                if (pnl.Size.Height - 5 < (c2.Location.Y + c2.Size.Height))
                { pnl.Size = new Size(pnl.Size.Width, pnl.Size.Height + 30); }
                //end own method
            }
            else
            { MessageBox.Show("Field already added to ticket"); }
        }

        void removeLabels(Control con, SizablePanel pnl)
        {
            bool delete = true;
            if (pnl.Name == "splRecipe")
            {
                if (!con.Name.Contains("RecipeName"))
                { ticketC.Remove(con); }
                else
                { delete = false; }
            }
            else if (pnl.Name == "splProds")
            {
                if (con.Name.Contains("productName") || con.Name.Contains("productId"))
                { delete = false; }
                else
                { prodC.Remove(con); }
            }
            else if (pnl.Name == "splSign")
            { signC.Remove(con); }
            if (delete)
            { con.Dispose(); }

            fieldNames.Remove(con.Name);
        }
        #endregion methods

        #region events
        void treeView1_ItemDrag(object sender, ItemDragEventArgs e)
        {
            TreeNode tn = (TreeNode)e.Item;
            if (!tn.Name.Contains("remove"))
            { treeView1.DoDragDrop(e.Item, DragDropEffects.Copy); }
        }

        void dragDrop(object sender, DragEventArgs e)
        {
            TreeNode tn = (TreeNode)e.Data.GetData(typeof(TreeNode));
            if (tn.Parent != null)
            { addFields(tn); }
        }

        void dragEnter(object sender, DragEventArgs e)
        { e.Effect = DragDropEffects.Copy; }

        void mouseDown(object sender, MouseEventArgs e)
        {
            activeControl = sender as Control;
            if (activeControl.GetType() != typeof(SizablePanel))
            {
                previousLocation = e.Location;
                Cursor = Cursors.Hand;
            }
            else
            {
                int x = activeControl.Location.X;
                int y = activeControl.Location.Y;
                int wGrip = activeControl.Width - 20;
                int hGrip = activeControl.Height - 20;
                if (e.Location.X <= activeControl.Width && e.Location.X >= wGrip && e.Location.Y <= activeControl.Height && e.Location.Y >= hGrip)
                { resize = true; }
                else
                {
                    previousLocation = e.Location;
                    Cursor = Cursors.Hand;
                }
            }
        }

        void mouseMove(object sender, MouseEventArgs e)
        {
            if (!resize)
            {
                if (activeControl == null || activeControl != sender)
                { return; }

                if (activeControl.GetType() == typeof(Label))
                {
                    SizablePanel pnl = (SizablePanel)activeControl.Parent;
                    List<Control> cList = new List<Control>();
                    cList = getControlList(pnl);

                    foreach (Control c in cList)
                    {
                        if (c.Name == activeControl.Name)
                        { moveItem(e); }
                    }
                }
                else
                { moveItem(e); }
            }
        }

        void mouseUp(object sender, MouseEventArgs e)
        {
            resize = false;
            if (activeControl != null)
            {
                if (activeControl.GetType() == typeof(Label))
                {
                    SizablePanel pnl = (SizablePanel)activeControl.Parent;
                    List<Control> cList = new List<Control>();
                    cList = getControlList(pnl);

                    foreach (Control c in cList)
                    {
                        //moves 2nd label so still lines up correctly
                        if (c.Name.Contains(activeControl.Name) && c.Name != activeControl.Name)
                        {
                            if (activeControl.Parent.Name.Contains("splSign") && c.Name.Contains("Info"))
                            { c.Location = new Point(activeControl.Location.X + 100, activeControl.Location.Y +15); }
                            else
                            {c.Location = new Point(activeControl.Location.X + 100, activeControl.Location.Y);}
                            snap(c, c.Location);
                        }
                    }
                }
            }
            activeControl = null;
            Cursor = Cursors.Default;
        }

        Point moveItem(MouseEventArgs e)
        {
            var location = activeControl.Location;
            location.Offset(e.Location.X - previousLocation.X, e.Location.Y - previousLocation.Y);
            activeControl.Location = location;
            snap(activeControl, activeControl.Location);
            return location;
        }

        void snap(Control c, Point p)
        {
            decimal newX = 0;
            decimal newY = 0;
            newX = Math.Round(Convert.ToDecimal(p.X / 10)) * 10;
            newY = Math.Round(Convert.ToDecimal(p.Y / 10)) * 10;

            var location = new Point(Convert.ToInt16(newX), Convert.ToInt16(newY));
            c.Location = location;
        }

        void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode tn = (TreeNode)e.Node;
            if (tn.Parent != null)
            {
                if (fieldNames.Contains(tn.Tag.ToString()) || tn.Tag.ToString().Contains("remove"))
                {
                    string name = e.Node.Tag.ToString();
                    foreach (Control c in pnlContainer.Controls)
                    {
                        if (c.Name == tn.Parent.Tag.ToString())
                        {
                            if (!tn.Tag.ToString().Contains("remove"))
                            {
                                for (int i = c.Controls.Count - 1; i >= 0; i--)
                                {
                                    Control con = c.Controls[i];
                                    if (con.Name.Contains(e.Node.Tag.ToString()))
                                    { removeLabels(con, (SizablePanel)c); }
                                }
                                break;
                            }
                            else
                            {
                                for (int i = c.Controls.Count - 1; i >= 0; i--)
                                {
                                    Control con = c.Controls[i];
                                    removeLabels(con, (SizablePanel)c);
                                }
                                if (c.Name == "pnlSign")
                                { c.Dispose(); }
                                break;
                            }
                        }
                    }
                }
                else
                { addFields(tn); }
            }
        }
        #endregion events

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Image Files (*.bmp, *.jpg, *.png, *.gif, *.tif)|*.bmp;*.jpg;*.png;*.gif;*.tif";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            { logoPath = openFileDialog1.FileName; }
            if (File.Exists(logoPath))
            {pictureBox1.Image = Image.FromFile(logoPath);}
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            getPrinterList();
            List<TicketSettings> tsl = new List<TicketSettings>();
            foreach (Control p in pnlContainer.Controls)
            {
                string type = p.GetType().ToString();
                TicketSettings ts = new TicketSettings(type, p.Name, "", p.Location.X, p.Location.Y, p.Size.Width, p.Size.Height, "");
                tsl.Add(ts);
                foreach (Control c in p.Controls)
                {
                    string cType = c.GetType().ToString();
                    if (c.Name == "location")
                    {
                        if (c.Text.Contains("\r\n"))
                        { c.Text = c.Text.Replace("\r\n", "~"); }
                    }
                    if(cType.Contains("PictureBox"))
                    {
                        if (File.Exists(logoPath))
                        {c.Text = logoPath;}
                    }
                    TicketSettings cts = new TicketSettings(cType, c.Name, c.Text, c.Location.X, c.Location.Y, c.Size.Width, c.Size.Height, p.Name);
                    tsl.Add(cts);
                }
            }

            string info = "";
            foreach (TicketSettings ts in tsl)
            {
                info += ts.Type + "\t" + ts.Name + "\t" + ts.Text + "\t" + ts.X + "\t" + ts.Y + "\t" + ts.W +
                "\t" + ts.H + "\t" + ts.Parent + "\r\n";
            }

            foreach (string p in printerList)
            { info += p + "\r\n"; }

            string partialPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)
                + @"\Yargus";
            string filePath = partialPath + @"\BatchTicket.txt";
            if (!Directory.Exists(partialPath))
            { Directory.CreateDirectory(partialPath); }

            using (StreamWriter writeOutTicket = new StreamWriter(filePath))
            { writeOutTicket.WriteLine(info); }

            this.Close();
        }

        private void btnPrinters_Click(object sender, EventArgs e)
        {
            Printers printer = new Printers(printerList, printSettings);
            printer.ShowDialog();
            getPrinterList();
        }

        private void getPrinterList()
        {
            printerList.Clear();
            string line = "";
            if (File.Exists(printSettings))
            {
                using (StreamReader sr = new StreamReader(printSettings))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] items = line.Split('\t');
                        if (items.Length < 2 && items[0] != "")
                        { printerList.Add(items[0]); }
                    }
                }
            }
        }
    }

    public class TicketSettings
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int W { get; set; }
        public int H { get; set; }
        public string Parent { get; set; }

        public TicketSettings(string type, string name, string text, int x, int y, int w, int h, string parent)
        {
            this.Type = type;
            this.Name = name;
            this.Text = text;
            this.X = x;
            this.Y = y;
            this.W = w;
            this.H = h;
            this.Parent = parent;
        }
    }
}

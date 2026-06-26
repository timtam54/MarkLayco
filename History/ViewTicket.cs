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

namespace History
{
    public partial class ViewTicket : Form
    {
        string path;
        public ViewTicket(string path)
        {
            InitializeComponent();
            this.path = path;
            if (File.Exists(path))
            { pictureBox1.Image = Image.FromFile(path); }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            printDocument1.PrintPage += PrintPage;
            if (printDialog1.ShowDialog() == DialogResult.OK)
            { printDocument1.Print(); }
        }

        private void PrintPage(object o, PrintPageEventArgs e)
        {
            Image img = Image.FromFile(path);
            e.Graphics.DrawImage(img, 15, 25);
        }
    }
}

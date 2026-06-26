using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Settings
{
    public partial class Restore : Form
    {
        public Restore()
        {
            InitializeComponent();
            FileInfo file = getNewestFile(new DirectoryInfo(LRP.Default.AppData + "\\Backup"));
            txtPath.Text = file.FullName;
        }

        public static FileInfo getNewestFile(DirectoryInfo d)
        {
            return d.GetFiles("*.txt")
                .Union(d.GetDirectories().Select(dir => getNewestFile(d)))
                .OrderByDescending(f => (f == null ? DateTime.MinValue : f.LastWriteTime))
                .FirstOrDefault();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            using (StreamReader sr = new StreamReader(txtPath.Text))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] items = line.Split('=');
                    if (items[0] == "DBPath")
                    { LRP.Default.DBPath = items[1]; }
                    if (items[0] == "ErrorPath")
                    { LRP.Default.ErrorPath = items[1]; }
                    if (items[0] == "LogPath")
                    { LRP.Default.LogPath = items[1]; }
                    if (items[0] == "NumHoppers")
                    { LRP.Default.NumHoppers = Convert.ToInt16(items[1]); }
                    if (items[0] == "PlcIp")
                    { LRP.Default.PlcIp = items[1]; }
                    if (items[0] == "PrintSettings")
                    { LRP.Default.PrintSettings = items[1]; }
                    if (items[0] == "Rlb")
                    { LRP.Default.Rlb = items[1]; }
                    if (items[0] == "setupDone")
                    { LRP.Default.setupDone = Convert.ToBoolean(items[1]); }
                    if (items[0] == "useMetric")
                    { LRP.Default.useMetric = Convert.ToBoolean(items[1]); }
                    pbCurrent.Value += 5;
                }
            }
            LRP.Default.Save();
            pbCurrent.Value = 100;
            MessageBox.Show("Backup successful.");
            this.Close();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            { txtPath.Text = folderBrowserDialog1.SelectedPath; }
        }
    }
}

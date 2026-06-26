using Database;
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
    public partial class Backup : Form
    {
        Databases db;
        bool settings = false;
        bool database = false;
        bool cleanup = false;

        public Backup(Databases db)
        {
            InitializeComponent();
            this.db = db;
            string date = DateTime.Now.ToShortDateString();
            date = date.Replace("/", "-");
            txtSname.Text = "Settings-" + date;
            txtSpath.Text = LRP.Default.AppData + "\\Backup\\";
            txtDname.Text = "Database-" + date;
            txtDpath.Text = LRP.Default.AppData + "\\Backup\\";
        }

        private void checkCkbx()
        {
            if (ckbxDatabase.Checked || ckbxSettings.Checked)
            { btnStart.Enabled = true; }
            else
            { btnStart.Enabled = false; }
        }

        private void ckbxSettings_CheckedChanged(object sender, EventArgs e)
        {
            checkCkbx();
            if (ckbxSettings.Checked)
            {
                txtSname.Enabled = true;
                txtSpath.Enabled = true;
                btnSbrowse.Enabled = true;
            }
            else
            {
                txtSname.Enabled = false;
                txtSpath.Enabled = false;
                btnSbrowse.Enabled = false;
            }
        }

        private void ckbxDatabase_CheckedChanged(object sender, EventArgs e)
        {
            checkCkbx();
            if (ckbxDatabase.Checked)
            {
                txtDname.Enabled = true;
                txtDpath.Enabled = true;
                btnDbrowse.Enabled = true;
            }
            else
            {
                txtDname.Enabled = false;
                txtDpath.Enabled = false;
                btnDbrowse.Enabled = false;
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (ckbxSettings.Checked)
            {
                if (txtSname.Text != "" && txtSpath.Text != "")
                {
                    checkPath();
                    if (checkOverwrite(txtSpath.Text + txtSname.Text + ".txt", "Settings"))
                    { settings = true; }
                }
                else
                { MessageBox.Show("Please enter a name and path for the Settings backup."); }
            }
            if (ckbxDatabase.Checked)
            {
                if (txtDname.Text != "" && txtDpath.Text != "")
                {
                    checkPath();
                    if (checkOverwrite(txtDpath.Text + txtDname.Text + ".sdf", "Database"))
                    { database = true; }
                }
                else
                { MessageBox.Show("Please enter a name and path for the Database backup."); }
                
                DialogResult dr = MessageBox.Show("Would you like to delete the history after backing it up? This is recommended yearly.",
                    "Cleanup Database?", MessageBoxButtons.YesNo);
                if (dr == DialogResult.Yes)
                {cleanup = true;}
            }
            if (settings)
            { backupSettings(); }
            if (database)
            { backupDatabase(); }
            if (cleanup)
            { 
                db.deleteAllHistory();
                pbCurrent.Value = 100;
                pbTotal.Value = 100;
                this.Close();
            }
        }

        private bool checkOverwrite(string path, string type)
        {
            bool overwrite = true;
            if (File.Exists(path))
            {
                DialogResult dr = MessageBox.Show("File name already in use. Do you want to overwrite the existing " 
                    + type + " file?","Overwrite " + type, MessageBoxButtons.YesNo);
                if (dr == DialogResult.No)
                { 
                    overwrite = false;
                    if (type == "Settings")
                    { settings = false; }
                    else
                    {
                        database = false;
                        cleanup = false;
                    }
                }
            }
            return overwrite;
        }

        private void checkPath()
        {
            if (!Directory.Exists(txtSpath.Text))
            {Directory.CreateDirectory(txtSpath.Text);}
        }

        private void backupSettings()
        {
            lblCurrent.Text += ": Backing up settings";
            string settings = "AppData=" + LRP.Default.AppData +"\r\n";
            pbCurrent.Value = 5;
            settings += "DBPath=" + LRP.Default.DBPath + "\r\n";
            pbCurrent.Value += 5;
            settings += "ErrorPath=" + LRP.Default.ErrorPath + "\r\n";
            pbCurrent.Value += 5;
            settings += "LogPath=" + LRP.Default.LogPath + "\r\n";
            pbCurrent.Value += 5;
            settings += "NumHoppers=" + LRP.Default.NumHoppers + "\r\n";
            pbCurrent.Value += 5;
            settings += "PlcIp=" + LRP.Default.PlcIp + "\r\n";
            pbCurrent.Value += 5;
            settings += "PrintSettings=" + LRP.Default.PrintSettings + "\r\n";
            pbCurrent.Value += 5;
            settings += "Rlb=" + LRP.Default.Rlb + "\r\n";
            pbCurrent.Value += 5;
            settings += "setupDone=" + LRP.Default.setupDone + "\r\n";
            pbCurrent.Value += 5;
            settings += "useMetric=" + LRP.Default.useMetric + "\r\n";
            pbCurrent.Value += 5;

            using (StreamWriter sw = new StreamWriter(txtSpath.Text + txtSname.Text + ".txt"))
            { sw.WriteLine(settings); }
            pbCurrent.Value = 100;

            if (!database && !cleanup)
            {
                pbTotal.Value = 100;
                closeForm();
            }
            else if (database && cleanup)
            { pbTotal.Value = 33; }
            else
            { pbTotal.Value =50;}
        }

        private void backupDatabase()
        {
            if(File.Exists(LRP.Default.DBPath))
            {
                pbCurrent.Value = 50;
                File.Copy(LRP.Default.DBPath, txtDpath.Text + txtDname.Text + ".sdf",true);
                pbCurrent.Value = 100;
            }

            if (!settings && !cleanup)
            {
                pbTotal.Value = 100;
                closeForm();
            }
            else if (settings && cleanup)
            { pbTotal.Value += 33; }
            else
            { 
                pbTotal.Value += 50;
                if (pbTotal.Value == 100)
                { closeForm(); }
            }
        }

        private void closeForm()
        {
            MessageBox.Show("Backup successful.");
            this.Close();
        }

        private void btnSbrowse_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            { txtSpath.Text = folderBrowserDialog1.SelectedPath; }
        }

        private void btnDbrowse_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            { txtDpath.Text = folderBrowserDialog1.SelectedPath; }
        }
        //backup settings
        //backup databasea
    }
}

using PlcComms.Master;
using PlcComms.Test;
using PrintBatches;
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

namespace Settings
{
    public partial class InitialSetup : Form
    {
        #region fields
        // Modbus port number. Industry standard is port 502.
        private int modbusPort = 502;
        // Timeout value (in msec) for TCP connection.
        private int timeOutValue = 50;
        // Timeout value (in msec) for testing TCP connection.
        private int timeOutValueLong = 300;
        private string defaultIp = "0.0.0.0";
        public bool settingsFileCreated = false;
        #endregion fields

        public InitialSetup()
        {
            InitializeComponent();
        }
        
        private void btnSearch_Click(object sender, EventArgs e)
        {
            // display hourglass/spinning cursor
            this.Cursor = Cursors.WaitCursor;
            bool addressFound = false;
            try
            {
                // Get the ip address of the local computer and store it
                //  into the string ipAddr
                string ipAddr = TcpConnection.GetIpAddress();

                // Separate the ip address octets into an array.
                string[] octets;
                string[] stringSeparators = new string[] { "." };
                octets = ipAddr.Split(stringSeparators, StringSplitOptions.None);
                // Store the first three octets into a string.
                string ipAddressThreeOctets = octets[0] + "." + octets[1] + "." + octets[2] + ".";

                string ipAddressFull = "";
                int maxIpSearch = 254;
                
                // This loop cycles through ip addresses. For example,
                //  if the first three octets were 192.168.1, this will
                //  cycle through 192.168.1.1 to 192.168.1.maxIpSearch.
                for (int i = 1; i <= maxIpSearch; i++)
                {
                    // Combine the first three octets with a number for the fourth octet
                    //  to create a full ip address and displays it in text box.
                    ipAddressFull = ipAddressThreeOctets + Convert.ToString(i);
                    txtIp.Text = ipAddressFull;
                    // Performs a connection test to the current ip address. If the test
                    //  passes, it updates the status to valid and stops performing 
                    //  tests on any other addresses.
                    if (TcpConnection.TestConnection(ipAddressFull, modbusPort, timeOutValue))
                    {
                        addressFound = true;
                        lblConnection.Text = "Connected";
                        lblConnection.ForeColor = Color.DarkGreen;
                        btnVerify.Enabled = true;
                        break;
                    }
                }
            }
            catch
            {
                // exception caught
                // ..could log exception message here.
            }
            finally
            {
                // If no device was found, the default ip address will be
                //  displayed in the text box
                if (addressFound == false)
                {txtIp.Text = defaultIp;}
                // display normal cursor
                this.Cursor = Cursors.Default;
            }
        }

        private void btnVerify_Click(object sender, EventArgs e)
        {
            int numberOfHoppers = 0;
            string plcIpAddress = txtIp.Text;
            int minimumNumberOfHoppers = 1;
            int maximumNumberOfHoppers = 32;

            if (plcIpAddress != "0.0.0.0")
            {
                try
                {
                    if (TcpConnection.TestConnection(plcIpAddress, modbusPort, timeOutValueLong))
                    {
                        PlcTcpMaster plcConnection = new PlcTcpMaster(plcIpAddress);
                        numberOfHoppers = Convert.ToInt32(plcConnection.readFloat(LRP.Default.MLNumHoppers));
                        // Check to make sure that the value is greater than 0 and less than 32.
                        if ((numberOfHoppers >= minimumNumberOfHoppers) && (numberOfHoppers <= maximumNumberOfHoppers))
                        {
                            txtNumHoppers.Text = Convert.ToString(numberOfHoppers);
                            lblConnection.Text = "Connected";
                            lblConnection.ForeColor = Color.DarkGreen;
                        }
                    }
                    else
                    {
                        lblConnection.Text = "No Connection";
                        lblConnection.ForeColor = Color.DarkRed;
                    }
                }
                catch (TimeoutException ex)
                { btnVerify.Enabled = false; }
            }
            else
            {
                txtNumHoppers.Text = "8";
                lblConnection.Text = "No Connection";
                lblConnection.ForeColor = Color.DarkRed;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string printSettingsPath = "";
            try
            {
                LRP.Default.setupDone = true;
                List<string> printerList = new List<string>();
                // Save settings to the settingsInfo class which is created
                //  in the main window.
                // An empty IP must be normalised to the "no PLC" sentinel
                // 0.0.0.0, otherwise the main window treats it as a real PLC
                // and crashes trying to read hopper names.
                LRP.Default.PlcIp = string.IsNullOrWhiteSpace(txtIp.Text) ? "0.0.0.0" : txtIp.Text;
                // Guard against an empty/zero hopper count (e.g. Save pressed
                // without Verify, or no PLC connected). A 0 here breaks the
                // Recipe Maker, so default to the standard 8 hoppers.
                int parsedHoppers;
                if (!int.TryParse(txtNumHoppers.Text, out parsedHoppers) || parsedHoppers <= 0)
                { parsedHoppers = 8; }
                LRP.Default.NumHoppers = parsedHoppers;
                
                string appData = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\Yargus\LRP";
                LRP.Default.Rlb = appData + @"\RLB";
                LRP.Default.LogPath = appData + @"\LogFile";
                LRP.Default.ErrorPath = appData + @"\ErrorFile";
                LRP.Default.AppData = appData;
                LRP.Default.DBPath = appData + @"\LRP.sdf";

                if (rbtnKgs.Checked)
                { LRP.Default.useMetric = true; }
                else
                { LRP.Default.useMetric = false; }

                printSettingsPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) 
                    + @"\Yargus\BatchTicket.txt";
                LRP.Default.PrintSettings = printSettingsPath;
                LRP.Default.Save();
                if (!Directory.Exists(appData))
                { Directory.CreateDirectory(appData); }
                if (!Directory.Exists(LRP.Default.Rlb))
                { Directory.CreateDirectory(LRP.Default.Rlb); }
                if (!File.Exists(LRP.Default.LogPath))
                { File.Create(LRP.Default.LogPath); }
                if (!File.Exists(LRP.Default.ErrorPath))
                { File.Create(LRP.Default.ErrorPath); }
                if (!Directory.Exists(appData +@"\PrintTickets"))
                { Directory.CreateDirectory(appData + @"\PrintTickets"); }

            }
            catch (Exception ex)
            { ex.ToString(); }
            this.Close();
        }

        private void btnMakeTicket_Click(object sender, EventArgs e)
        {
            bool useMetric = false;
            if (rbtnKgs.Checked)
            { useMetric = true; }
            string path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)
                    + @"\Yargus\BatchTicket.txt";
            SetupTicket ticket = new SetupTicket(path, useMetric);
            ticket.ShowDialog();
        }
    }
}

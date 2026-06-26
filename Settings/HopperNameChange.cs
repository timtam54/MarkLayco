using Database;
using PlcComms;
using PlcComms.Master;
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
    public partial class HopperNameChange : Form
    {
        PlcInteraction plc = new PlcInteraction(LRP.Default.PlcIp);
        List<string> names = new List<string>();
        int hopperNum;
        Databases db = new Databases();

        public HopperNameChange(int hopperNum)
        {
            InitializeComponent();
            this.hopperNum = hopperNum;
            lblCurrentName.Text = plc.GetHopperNames(hopperNum);
            getProdNames();
        }

        private void getProdNames()
        {
            names = db.getActiveProductNames();
            foreach (string n in names)
            { cbxProdNames.Items.Add(n); }
        }

        private void btnChangeName_Click(object sender, EventArgs e)
        {
            string name = cbxProdNames.SelectedItem.ToString();
            plc.SetHopperName(hopperNum, name);
            this.Close();
        }
    }
}

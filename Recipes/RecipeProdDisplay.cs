using GeneralData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Recipes
{
    public partial class RecipeProdDisplay : UserControl
    {
        public RecipeProdDisplay(Product prod)
        {
            InitializeComponent();
            lblName.Text = prod.Name;
            lblRatio.Text = prod.Ratio.ToString("#.####") + "%";
            label1.Text = prod.Yid.ToString() ;
        }
    }
}

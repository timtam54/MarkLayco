using Database;
using GeneralData;
using Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Recipes
{
    public partial class ModifyRecipes : Form
    {
        Databases db;
        string type;
        int rId;
        string recipeName;
        List<Product> prods = new List<Product>();
        
        public ModifyRecipes(Databases db)
        {
            InitializeComponent();
            this.db = db;
            type = "Add";
            makeRecipe();
        }

        public ModifyRecipes(Databases db, string type, int rId, string recipeName)
        {
            InitializeComponent();
            this.db = db;
            this.type = type;
            this.rId = rId;
            this.recipeName = recipeName;
            prods = db.getRecipeProds(rId);
            makeRecipe();
            setupInfo();

            if (type == "Delete")
            { btnNext.Text = "Delete"; }
            else
            { btnNext.Text = "Edit"; }
        }

        public void makeRecipe()
        {
            // Without a connected PLC the hopper count can be saved as 0, which
            // leaves the Recipe Maker with no Product/Ratio rows and makes Save
            // impossible to enable. Fall back to the program's default hopper
            // count so a recipe can still be entered.
            if (LRP.Default.NumHoppers <= 0)
            {
                LRP.Default.NumHoppers = 8;
                LRP.Default.Save();
            }
            int numHoppers = LRP.Default.NumHoppers;

            for (int i = 0; i < numHoppers; i++)
            {
                tlpBlendInfo.Controls.Add(new ComboBox() { Name = "cbxProd" + i }, 0, i);
                TextBox text = new TextBox();
                text.Name = "txtRatio" + i;
                tlpBlendInfo.Controls.Add(text, 1, i);
                text.TextChanged += new EventHandler(text_TextChanged);
            }

            List<string> prods = db.getActiveProductNames();
            if (prods.Count > 0)
            {
                for (int i = 0; i < numHoppers; i++)
                {

                    AutoCompleteStringCollection source = new AutoCompleteStringCollection();
                    ComboBox cbx = (ComboBox)tlpBlendInfo.GetControlFromPosition(0, i);
                    foreach (string prod in prods)
                    { 
                        cbx.Items.Add(prod);
                        source.Add(prod);
                    }
                    cbx.AutoCompleteCustomSource.Clear();
                    cbx.AutoCompleteCustomSource = source;
                    cbx.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    cbx.AutoCompleteSource = AutoCompleteSource.CustomSource;
                }
            }
            int sizeTLP = tlpBlendInfo.Location.Y + tlpBlendInfo.Height + 10;
            if (sizeTLP > pnlBtns.Location.Y)
            { pnlBtns.Location = new Point(pnlBtns.Location.X, sizeTLP); }

            this.Cursor = Cursors.Default;
        }

        private void setupInfo()
        {
            txtRecipeName.Text = recipeName;
            for (int j = 0; j < prods.Count; j++)
            {
                foreach (Product prod in prods)
                {
                    Control c = this.tlpBlendInfo.GetControlFromPosition(0, j);
                    ComboBox cbx = (ComboBox)c;
                    cbx.SelectedItem = prod.Name;
                    Control con = this.tlpBlendInfo.GetControlFromPosition(1, j);
                    con.Text = prod.Ratio.ToString();
                    j++;
                }
            }
        }

        void text_TextChanged(object sender, EventArgs e)
        {
            float totalRatio = new float();
            for (int j = 0; j < LRP.Default.NumHoppers; j++)
            {
                Control c = this.tlpBlendInfo.GetControlFromPosition(0, j);

                if (c != null)
                {
                    Control txt = tlpBlendInfo.GetControlFromPosition(1, j);
                    float num = new float();
                    if (Single.TryParse(txt.Text, out num))
                    {
                        totalRatio += Convert.ToSingle(num);
                        txtTotalRatio.Text = totalRatio.ToString();
                    }
                }
                else
                { break; }
            }

            if (totalRatio > 99.5 && totalRatio < 100)
            { 
                txtTotalRatio.Text = "100";
                totalRatio = 100;
            }

            if (totalRatio == 100)
            { btnNext.Enabled = true; }
            else
            { btnNext.Enabled = false; }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            bool done = true;

            List<Product> products = new List<Product>();
            List<float> ratios = new List<float>();
            List<int> yIdList = new List<int>();
            List<float> ratioList = new List<float>();
            List<string> prodList = new List<string>();

            for (int i = 0; i < LRP.Default.NumHoppers; i++)
            {
                //get prod
                ComboBox cbx = (ComboBox)tlpBlendInfo.GetControlFromPosition(0, i);

                if(cbx.SelectedItem != null)
                {
                    string prod = cbx.SelectedItem.ToString();
                    TextBox txt = (TextBox)tlpBlendInfo.GetControlFromPosition(1, i);
                    float ratioFloat = (float)Convert.ToDouble(txt.Text);
                    string ratioRound = ratioFloat.ToString("#.######");
                    float ratio = (float)Convert.ToDouble(ratioRound);

                    if (prod != "")
                    {
                        string id = db.getProdId(prod);
                        int yId = db.getYId(id);
                        yIdList.Add(yId);
                        ratioList.Add(ratio);
                        prodList.Add(prod);
                    }
                }
            }

            if (type == "Add")
            {
                if (txtRecipeName.Text != "")
                {
                    if (!db.checkRecipeExists(txtRecipeName.Text))
                    {db.addRecipe(txtRecipeName.Text, yIdList, ratioList);}
                    else
                    {
                        done = false;
                        MessageBox.Show("Recipe name already in use. Please enter a different name.");
                    }
                }
                else
                {
                    done = false;
                    MessageBox.Show("Please enter a name for the recipe");
                }
            }
            else if (type == "Delete")
            {
                if (!db.recipeRan(rId))
                {db.completeRecipeDelete(rId);}
                done = true;
            }
            else if (type == "Edit")
            {
                if (txtRecipeName.Text == "")
                { 
                    MessageBox.Show("Please enter a name for the recipe");
                    done = false;
                }
                else
                {
                    DialogResult result = MessageBox.Show("Are you sure you want to save edits to the Recipe?",
                        "Edit Recipe", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        if (!db.recipeRan(rId))
                        {
                            db.completeRecipeDelete(rId);
                            db.addRecipe(txtRecipeName.Text, yIdList, ratioList);
                        }
                        done = true;
                    }
                    else
                    { done = false; }
                }
            }
            if (done)
            { this.Close(); }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {this.Close();}

        private void ModifyRecipes_Load(object sender, EventArgs e)
        {

        }
    }
}

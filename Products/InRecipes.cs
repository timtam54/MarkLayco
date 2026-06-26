using Database;
using GeneralData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Products
{
    public partial class InRecipes : Form
    {
        Databases db;
        DatabaseViewer dbv;

        public InRecipes(Databases db, int yId)
        {
            InitializeComponent();
            this.db = db;
            getRecipeInfo(yId);
        }

        public InRecipes(DatabaseViewer dbv, int yId)
        {
            InitializeComponent();
            this.dbv = dbv;
            getRecipeInfo(yId);
        }

        public void getRecipeInfo(int yId)
        {
            int i = 0;
            tlpRecipes.RowCount = 1;
            tlpRecipes.Controls.Clear();
            tlpRecipes.RowStyles.Clear();
            List<int> recipeIds = new List<int>();
            if (db != null)
            { recipeIds = db.getRecipeForProd(yId); }
            else
            { recipeIds = dbv.getRecipeForProd(yId); }
            foreach (int rId in recipeIds)
            {
                string name = "";
                List<Product> prods = new List<Product>();
                if (db != null)
                {
                    prods = db.getRecipeProds(rId);
                    name = db.getRecipeName(rId);
                }
                else
                {
                    prods = dbv.getRecipeProds(rId);
                    name = dbv.getRecipeName(rId);
                }
                Label l = new Label();
                l.Text = name;
                tlpRecipes.Controls.Add(l, 0, i);
                i++;
                foreach (Product p in prods)
                {
                    Label l1 = new Label();
                    l1.Text = p.Name;
                    tlpRecipes.Controls.Add(l1, 1, i);
                    Label l2 = new Label();
                    l2.Text = p.Ratio.ToString();
                    tlpRecipes.Controls.Add(l2, 2, i);
                    i++;
                }

            }
        }
    }
}

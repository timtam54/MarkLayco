using Database;
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
    public partial class ModifyProducts : Form
    {
        string type;
        Databases db;
        string name;
        string id;
        string state;

        public ModifyProducts(Databases db)
        {
            InitializeComponent();
            type = "Add";
            this.db = db;
        }

        public ModifyProducts(string name, string id, string state, string type, Databases db)
        {
            InitializeComponent();
            this.type = type;
            this.name = name;
            this.id = id;
            this.state = state;
            this.db = db;
            txtName.Text = name;
            txtId.Text = id;
            cbxState.SelectedItem = state;

            if (type == "Delete")
            { btnFinish.Text = "Delete"; }
        }

        private void btnFinish_Click(object sender, EventArgs e)
        {
            bool done = false;
            List<string> prodNames = db.getAllProdNames();
            List<string> prodIds = db.getAllProdIds();

            if (type == "Add")
            {
                if (prodIds.Contains(txtId.Text))
                { MessageBox.Show("Product id already exists. Please either enter a different id or press the Set Id button."); }
                else
                {
                    if (!prodNames.Contains(txtName.Text))
                    {
                        db.addProduct(txtName.Text, txtId.Text, cbxState.SelectedItem.ToString());
                        done = true;
                    }
                    else
                    { MessageBox.Show("Product name already exists. Please enter a different product name."); }
                }
            }
            else if (type == "Edit")
            {
                if (prodIds.Contains(txtId.Text))
                { MessageBox.Show("Product id already exists. Please either enter a different id or press the Set Id button."); }
                else
                {
                    DialogResult result = MessageBox.Show("Are you sure you want to save edits to the product?",
                        "Edit Product", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        if (db.productInRecipe(id))
                        {
                            int yId = db.getYId(id);
                            db.editProduct(txtName.Text, txtId.Text, cbxState.SelectedItem.ToString(), yId);
                            done = true;
                        }
                        else
                        {
                            db.completeProdDelete(id);
                            db.addProduct(txtName.Text, txtId.Text, cbxState.SelectedItem.ToString());
                            done = true;
                        }
                    }
                }
            }
            else if (type == "Delete")
            {
                if (db.productInRecipe(id))
                {
                    int yId = db.getYId(id);
                    db.markProdInactive(yId);
                    done = true;
                }
                else
                {
                    db.completeProdDelete(id);
                    done = true;
                }
            }

            if (done)
            { this.Close(); }
        }


    }
}

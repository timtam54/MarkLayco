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
    public partial class ProductRevisions : Form
    {
        int yId;
        Databases db;
        DatabaseViewer dbv;
        public ProductRevisions(int yId, Databases db)
        {
            InitializeComponent();
            this.yId = yId;
            this.db = db;

            DataTable dt = db.getProdRevisions(yId);
            if (dt.Rows.Count > 0)
            { dgvRevision.DataSource = dt; }
            dgvRevision.Columns[3].HeaderText = "Revision";
            dgvRevision.Columns[4].HeaderText = "Date Changed";

            foreach (DataGridViewColumn col in dgvRevision.Columns)
            {col.Width = 120; }
        }

        public ProductRevisions(int yId, DatabaseViewer dbv)
        {
            InitializeComponent();
            this.yId = yId;
            this.dbv = dbv;

            DataTable dt = db.getProdRevisions(yId);
            if (dt.Rows.Count > 0)
            { dgvRevision.DataSource = dt; }
            dgvRevision.Columns[3].HeaderText = "Revision";
            dgvRevision.Columns[4].HeaderText = "Date Changed";

            foreach (DataGridViewColumn col in dgvRevision.Columns)
            { col.Width = 120; }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

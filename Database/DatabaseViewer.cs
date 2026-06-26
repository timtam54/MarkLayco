using GeneralData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlServerCe;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Database
{
    public class DatabaseViewer
    {
        #region Fields
        private string sqlString;
        string commonAppData;
        private string fullPath;
        SqlCeConnection conn = new SqlCeConnection();
        private string conString;
        #endregion Fields

         #region Constructors
        /// <summary>
        /// pass userSetting YargusDB
        /// </summary>
        /// <param name="dbPath"></param>
        public DatabaseViewer(string dbName)
        {
            commonAppData = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\Yargus\LRP\";
            fullPath = Path.Combine(commonAppData, dbName);
            conString = @"Data Source = " + fullPath + "; Password = 'yargus'; ";
        }
        #endregion Constructors

        public bool OpenConnection()
        {
            conn.ConnectionString = conString;
            conn.Open();
            if (conn.State == ConnectionState.Open)
            { return true; }
            else
            { return false; }
        }

        #region ProductTable
        /// <summary>
        /// Gets all active prod
        /// </summary>
        public DataTable getAllProds()
        {
            SqlCeCommand cmd;
            string sql;
            DataTable dt = new DataTable();

            try
            {
                conn = new SqlCeConnection(conString);
                if (conn.State == ConnectionState.Closed) { conn.Open(); }
                sql = "Select ProductName, ProductId, ProductState, revision, inRecipe" +
                    " from Products Where prodActive = 1 Order by ProductName asc";
                cmd = new SqlCeCommand(sql, conn);

                using (SqlCeDataAdapter a = new SqlCeDataAdapter(cmd))
                { a.Fill(dt); }
            }
            finally
            { conn.Close(); }
            return dt;
        }

        /// <summary>
        /// Gets all products to make sure name isn't used more than once
        /// </summary>
        public List<string> getAllProdNames()
        {
            conn = new SqlCeConnection(conString);
            if (conn.State == ConnectionState.Closed) { conn.Open(); }

            List<string> prodNames = new List<string>();
            try
            {
                string sql = "SELECT ProductName FROM Products";
                SqlCeCommand command = new SqlCeCommand(sql, conn);

                using (SqlCeDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    { prodNames.Add(dataReader["ProductName"].ToString()); }
                }
            }
            finally
            { conn.Close(); }
            return prodNames;
        }

        public List<string> getActiveProductNames()
        {
            conn = new SqlCeConnection(conString);
            if (conn.State == ConnectionState.Closed) { conn.Open(); }

            List<string> prodNames = new List<string>();
            try
            {
                string sql = "SELECT ProductName FROM Products Where prodActive =1";
                SqlCeCommand command = new SqlCeCommand(sql, conn);

                using (SqlCeDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    { prodNames.Add(dataReader["ProductName"].ToString()); }
                }
            }
            finally
            { conn.Close(); }
            return prodNames;
        }

        public List<string> getAllProdIds()
        {
            conn = new SqlCeConnection(conString);
            if (conn.State == ConnectionState.Closed) { conn.Open(); }

            List<string> prodIds = new List<string>();
            try
            {
                string sql = "SELECT ProductId FROM Products";
                SqlCeCommand command = new SqlCeCommand(sql, conn);

                using (SqlCeDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    { prodIds.Add(dataReader["ProductId"].ToString()); }
                }
            }
            finally
            { conn.Close(); }
            return prodIds;
        }

        /// <summary>
        /// send product name to find prod Id
        /// </summary>
        /// <param name="prodName"></param>
        public string getProdId(string prodName)
        {
            string productId;
            try
            {
                conn = new SqlCeConnection(conString);
                if (conn.State == ConnectionState.Closed) { conn.Open(); }

                string sql = "SELECT ProductId FROM Products where ProductName = @productName";
                SqlCeCommand cmd = new SqlCeCommand(sql, conn);
                cmd.Parameters.AddWithValue("@productName", prodName);
                productId = cmd.ExecuteScalar().ToString();
            }
            finally
            { conn.Close(); }

            return productId;
        }

        public string getProdName(int yId)
        {
            string prodName = "";
            try
            {
                conn = new SqlCeConnection(conString);
                if (conn.State == ConnectionState.Closed) { conn.Open(); }

                string sql = "SELECT ProductName FROM Products where yId = @yId and prodActive = 1";
                SqlCeCommand cmd = new SqlCeCommand(sql, conn);
                cmd.Parameters.AddWithValue("@yId", yId);
                prodName = cmd.ExecuteScalar().ToString();
            }
            finally
            { conn.Close(); }

            return prodName;
        }

        /// <summary>
        /// sets the yargus id - id that is used in the background to track products regardless of changes
        /// </summary>
        private int setYId()
        {
            int id;
            try
            {
                conn = new SqlCeConnection(conString);
                if (conn.State == ConnectionState.Closed) { conn.Open(); }

                string sql = "SELECT yId FROM Products Order by yId desc";
                SqlCeCommand cmd = new SqlCeCommand(sql, conn);
                id = Convert.ToInt16(cmd.ExecuteScalar()) + 1;
            }
            finally
            { conn.Close(); }

            return id;
        }

        public int getYId(string prodId)
        {
            int id;
            try
            {
                conn = new SqlCeConnection(conString);
                if (conn.State == ConnectionState.Closed) { conn.Open(); }

                string sql = "SELECT yId FROM Products Where ProductId = @prodId";
                SqlCeCommand cmd = new SqlCeCommand(sql, conn);
                cmd.Parameters.AddWithValue("@prodId", prodId);
                id = Convert.ToInt16(cmd.ExecuteScalar());
            }
            finally
            { conn.Close(); }

            return id;
        }

        /// <summary>
        /// checks to see if product is in Recipe before deleting or updating
        /// </summary>
        /// <param name="id"></param>
        public bool productInRecipe(string id)
        {
            int yId = getYId(id);
            bool exists = false;
            SqlCeCommand cmd;
            string sql;
            try
            {
                conn = new SqlCeConnection(conString);
                if (conn.State == ConnectionState.Closed) { conn.Open(); }

                sql = "Select * from RecipeInfo Where yId = @yId";
                cmd = new SqlCeCommand(sql, conn);
                cmd.Parameters.AddWithValue("@yId", yId);
                int rowsReturned = Convert.ToInt32(cmd.ExecuteScalar());
                if (rowsReturned > 0)
                { exists = true; }
            }
            finally
            { conn.Close(); }
            return exists;
        }

        public int getProdRevisionNum(int yId)
        {
            int revision;
            try
            {
                conn = new SqlCeConnection(conString);
                if (conn.State == ConnectionState.Closed) { conn.Open(); }

                string sql = "SELECT revision FROM Products Where yId = @yId Order by revision desc";
                SqlCeCommand cmd = new SqlCeCommand(sql, conn);
                cmd.Parameters.AddWithValue("@yId", yId);
                revision = Convert.ToInt16(cmd.ExecuteScalar());
            }
            finally
            { conn.Close(); }

            return revision;
        }

        public DataTable getProdRevisions(int yId)
        {
            SqlCeCommand cmd;
            string sql;
            DataTable dt = new DataTable();

            try
            {
                conn = new SqlCeConnection(conString);
                if (conn.State == ConnectionState.Closed) { conn.Open(); }
                sql = "Select ProductName, ProductId, ProductState, revision, changeDate" +
                    " from Products Where yId= @yId Order by revision desc";
                cmd = new SqlCeCommand(sql, conn);
                cmd.Parameters.AddWithValue("@yId", yId);

                using (SqlCeDataAdapter a = new SqlCeDataAdapter(cmd))
                { a.Fill(dt); }
            }
            finally
            { conn.Close(); }
            return dt;
        }
        
        public List<Product> getRecipeProds(int rId)
        {
            conn = new SqlCeConnection(conString);
            if (conn.State == ConnectionState.Closed) { conn.Open(); }
            List<Product> prods = new List<Product>();
            try
            {
                string sql = "SELECT * FROM RecipeInfo Where rId = @rId";
                SqlCeCommand cmd = new SqlCeCommand(sql, conn);
                cmd.Parameters.AddWithValue("@rId", rId);
                using (SqlCeDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        int yId = Convert.ToInt16(dataReader[0]);
                        double ratio = Convert.ToDouble(dataReader[1]);
                        string name = getProdName(yId);
                        Product prod = new Product(name, yId, ratio);
                        prods.Add(prod);
                    }
                }
            }
            finally
            { conn.Close(); }
            return prods;
        }
        #endregion ProductTable

        #region RecipeTable
        private int setRecipeId()
        {
            int id;
            try
            {
                conn = new SqlCeConnection(conString);
                if (conn.State == ConnectionState.Closed) { conn.Open(); }

                string sql = "SELECT rId FROM RecipeList Order by rId desc";
                SqlCeCommand cmd = new SqlCeCommand(sql, conn);
                id = Convert.ToInt16(cmd.ExecuteScalar()) + 1;
            }
            finally
            { conn.Close(); }

            return id;
        }

        public int getRid(string recipeName)
        {
            int id;
            try
            {
                conn = new SqlCeConnection(conString);
                if (conn.State == ConnectionState.Closed) { conn.Open(); }

                string sql = "SELECT rId FROM RecipeList Where RecipeName = @recipeName";
                SqlCeCommand cmd = new SqlCeCommand(sql, conn);
                cmd.Parameters.AddWithValue("@recipeName", recipeName);
                id = Convert.ToInt16(cmd.ExecuteScalar());
            }
            finally
            { conn.Close(); }

            return id;
        }

        public DataTable getAllRecipeNames()
        {
            SqlCeCommand cmd;
            string sql;
            DataTable dt = new DataTable();

            try
            {
                conn = new SqlCeConnection(conString);
                if (conn.State == ConnectionState.Closed) { conn.Open(); }

                sql = "Select RecipeName from RecipeList Where recipeActive = 1 Order by RecipeName desc";
                cmd = new SqlCeCommand(sql, conn);

                using (SqlCeDataAdapter a = new SqlCeDataAdapter(cmd))
                { a.Fill(dt); }
            }
            finally
            { conn.Close(); }
            return dt;
        }

        public string getRecipeName(int rId)
        {
            string recipeName = "";
            try
            {
                conn = new SqlCeConnection(conString);
                if (conn.State == ConnectionState.Closed) { conn.Open(); }

                string sql = "SELECT RecipeName FROM RecipeList where rId = @rId";
                SqlCeCommand cmd = new SqlCeCommand(sql, conn);
                cmd.Parameters.AddWithValue("@rId", rId);
                recipeName = cmd.ExecuteScalar().ToString();
            }
            finally
            { conn.Close(); }

            return recipeName;
        }

        public bool checkRecipeExists(string name)
        {
            bool exists = false;
            SqlCeCommand cmd;
            string sql;
            try
            {
                conn = new SqlCeConnection(conString);
                if (conn.State == ConnectionState.Closed) { conn.Open(); }

                sql = "SELECT COUNT(*) FROM RecipeList where RecipeName = @name";
                cmd = new SqlCeCommand(sql, conn);
                cmd.Parameters.AddWithValue("@name", name);
                int rowsReturned = Convert.ToInt32(cmd.ExecuteScalar());
                if (rowsReturned > 0)
                { exists = true; }
            }
            finally
            { conn.Close(); }
            return exists;
        }
        
        public List<int> getRecipeForProd(int yId)
        {
            conn = new SqlCeConnection(conString);
            if (conn.State == ConnectionState.Closed) { conn.Open(); }

            List<int> rIds = new List<int>();
            List<int> recipeIds = new List<int>();
            try
            {
                string sql = "SELECT rId FROM RecipeList Where recipeActive =1";
                SqlCeCommand cmd = new SqlCeCommand(sql, conn);
                using (SqlCeDataReader dR = cmd.ExecuteReader())
                {
                    while (dR.Read())
                    { rIds.Add(Convert.ToInt16(dR["rId"])); }
                }

                sql = "SELECT rId FROM RecipeInfo Where yId = @yId";
                cmd = new SqlCeCommand(sql, conn);
                cmd.Parameters.AddWithValue("@yId", yId);
                using (SqlCeDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        if (rIds.Contains(Convert.ToInt16(dataReader["rId"])))
                        { recipeIds.Add(Convert.ToInt16(dataReader["rId"])); }
                    }
                }

            }
            finally
            { conn.Close(); }
            return recipeIds;
        }

        public bool recipeRan(int rId)
        {
            bool exists = false;
            SqlCeCommand cmd;
            string sql;
            try
            {
                conn = new SqlCeConnection(conString);
                if (conn.State == ConnectionState.Closed) { conn.Open(); }

                sql = "Select ran from RecipeList Where rId = @rId";
                cmd = new SqlCeCommand(sql, conn);
                cmd.Parameters.AddWithValue("@rId", rId);
                int rowsReturned = Convert.ToInt32(cmd.ExecuteScalar());
                if (rowsReturned > 0)
                { exists = true; }
            }
            finally
            { conn.Close(); }
            return exists;
        }
        
        public int getRecipeRevisionNum(int rId)
        {
            int revision;
            try
            {
                conn = new SqlCeConnection(conString);
                if (conn.State == ConnectionState.Closed) { conn.Open(); }

                string sql = "SELECT revision FROM RecipeList Where rId = @rId Order by revision desc";
                SqlCeCommand cmd = new SqlCeCommand(sql, conn);
                cmd.Parameters.AddWithValue("@rId", rId);
                revision = Convert.ToInt16(cmd.ExecuteScalar());
            }
            finally
            { conn.Close(); }

            return revision;
        }
        #endregion RecipeTable

        #region HistoryTable
        public DataTable getHistory()
        {
            DataTable results = new DataTable();
            string sql = "";
            SqlCeCommand cmd = new SqlCeCommand();
            sql = "Select * from History order by RanDate desc";
            cmd = new SqlCeCommand(sql, conn);

            DataTable dt = setupHistoryDt();
            try
            {
                conn.Open();
                using (SqlCeDataAdapter a = new SqlCeDataAdapter(cmd))
                { a.Fill(results); }
                dt = fillInHistoryDt(results, dt);
            }
            finally
            { conn.Close(); }
            return dt;
        }

        public DataTable getHistory(bool useProd, bool useRecipe, bool useDate, string prod,
            string recipe, DateTime from, DateTime to)
        {
            DataTable dt = getHistory();
            DataTable final = setupHistoryDt();

            foreach (DataRow dr in dt.Rows)
            {
                if (useDate)
                {
                    if (Convert.ToDateTime(dr[1]) < to && Convert.ToDateTime(dr[1]) > from)
                    { final.Rows.Add(dr.ItemArray); }
                }
                if (useRecipe)
                {
                    if (dr[0].ToString().ToLower() == recipe.ToLower())
                    { final.Rows.Add(dr.ItemArray); }
                }
                if (useProd)
                {
                    if (dr[prod].ToString() != "")
                    { final.Rows.Add(dr.ItemArray); }
                }
            }
            return final;
        }

        public DataTable getHistory(string prod, DateTime from, DateTime to)
        {
            DataTable dt = getHistory();
            DataTable final = setupHistoryDt();

            foreach (DataRow dr in dt.Rows)
            {
                if (Convert.ToDateTime(dr[1]) < to && Convert.ToDateTime(dr[1]) > from)
                { final.Rows.Add(dr.ItemArray); }

                if (dr[prod].ToString() != "")
                { final.Rows.Add(dr.ItemArray); }
            }
            return final;
        }

        private DataTable setupHistoryDt()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Recipe Name");
            dt.Columns.Add("Date/Time");
            dt.Columns.Add("Total Weight");
            List<string> prodNames = getActiveProductNames();

            foreach (string prod in prodNames)
            {
                if (!dt.Columns.Contains(prod))
                { dt.Columns.Add(prod); }
            }
            return dt;
        }

        private DataTable fillInHistoryDt(DataTable results, DataTable dt)
        {

            int row = 0;
            foreach (DataRow dataRow in results.Rows)
            {
                int rId = Convert.ToInt32(dataRow[0]);
                string name = getRecipeName(rId);


                DateTime date = Convert.ToDateTime(dataRow[2]);
                dt.Rows.Add(name, date, dataRow[1]);

                List<Product> productList = getRecipeProds(rId);

                foreach (Product prod in productList)
                {
                    int column = 3;
                    int dtColCount = dt.Columns.Count;
                    int count = productList.Count;
                    while (column < dtColCount)
                    {
                        if (dt.Columns[column].ToString() == prod.Name)
                        {
                            dt.Rows[row][column] = Convert.ToDouble(dataRow[1]) * (prod.Ratio / 100);
                            break;
                        }
                        column++;
                    }
                }
                row++;
            }

            if (dt.Rows.Count == 0)
            { MessageBox.Show("No results from search"); }

            return dt;
        }

        public DateTime getFromDate()
        {
            DateTime from;
            try
            {
                conn = new SqlCeConnection(conString);
                if (conn.State == ConnectionState.Closed) { conn.Open(); }

                string sql = "SELECT RanDate FROM History Order by RanDate asc";
                SqlCeCommand cmd = new SqlCeCommand(sql, conn);
                from = Convert.ToDateTime(cmd.ExecuteScalar());
            }
            finally
            { conn.Close(); }

            return from;
        }

        public DateTime getToDate()
        {
            DateTime to;
            try
            {
                conn = new SqlCeConnection(conString);
                if (conn.State == ConnectionState.Closed) { conn.Open(); }

                string sql = "SELECT RanDate FROM History Order by RanDate desc";
                SqlCeCommand cmd = new SqlCeCommand(sql, conn);
                to = Convert.ToDateTime(cmd.ExecuteScalar());
            }
            finally
            { conn.Close(); }

            return to;
        }
        #endregion HistoryTable
    }
}

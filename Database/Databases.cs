using GeneralData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Database
{
    public class Databases
    {
        #region Fields
        private const string dbName = "LRP.db";
        private string sqlString;
        string commonAppData;
        private string fullPath;
        SQLiteConnection conn = new SQLiteConnection();
        // Table creation strings
        protected string createProductsTable = @"CREATE TABLE Products (ProductID NVARCHAR(75) Primary Key, prodActive bit," +
            " ProductName NVARCHAR(75), ProductState NVARCHAR(75), yId int, revision Int, changeDate DateTime," +
            "inRecipe bit)";
        protected string createHistoryTable = @"CREATE TABLE History (rId NVARCHAR(75), " + 
            "RanWeight float NOT NULL, RanDate DateTime)";
        protected string createRecipeListTable = @"CREATE TABLE RecipeList (RecipeName NVARCHAR(75) Primary Key," +
            " rId int, revision int, recipeActive bit, changeDate DateTime, ran bit)";
        protected string createRecipeInfoTable = @"CREATE TABLE RecipeInfo (yId int, Ratio float NOT NULL, rId int, revision int)";
        
        private string conString;
        #endregion Fields
        
        #region Constructors
        /// <summary>
        /// pass userSetting YargusDB
        /// </summary>
        /// <param name="dbPath"></param>
        public Databases()
        {
            commonAppData = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\Yargus\LRP\";
            fullPath = Path.Combine(commonAppData, dbName);
            conString = @"Data Source=" + fullPath + ";Version=3;";
        }
        #endregion

        public bool DbExist()
        {
            if (File.Exists(fullPath))
            { return true; }
            else
            { return false; }
        }

        public void createDb()
        {
            if (!DbExist())
            {
                SQLiteConnection.CreateFile(fullPath);
                createTables();
            }
        }

        public void createTables()
        {
            try
            {
                conn = new SQLiteConnection(conString);
                if (conn.State == ConnectionState.Closed)
                { conn.Open(); }
                if (!TableExist("Products"))
                {
                    using (SQLiteCommand cmd = new SQLiteCommand(createProductsTable, conn))
                    { cmd.ExecuteNonQuery(); }
                }
                if (!TableExist("RecipeList"))
                {
                    using (SQLiteCommand cmd = new SQLiteCommand(createRecipeListTable, conn))
                    { cmd.ExecuteNonQuery(); }
                }
                if (!TableExist("RecipeInfo"))
                {
                    using (SQLiteCommand cmd = new SQLiteCommand(createRecipeInfoTable, conn))
                    { cmd.ExecuteNonQuery(); }
                }
                if (!TableExist("History"))
                {
                    using (SQLiteCommand cmd = new SQLiteCommand(createHistoryTable, conn))
                    { cmd.ExecuteNonQuery(); }
                }
            }
            finally
            { conn.Close(); }
        }

        public bool OpenConnection()
        {
            conn.ConnectionString = conString;
            conn.Open();
            if (conn.State == ConnectionState.Open)
            { return true; }
            else
            { return false; }
        }

        public bool TableExist(string tableName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(tableName))
                { throw new ArgumentException("Invalid table name"); }

                using (SQLiteCommand command = conn.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = "SELECT 1 FROM sqlite_master WHERE type = 'table' AND name = @tableName";
                    command.Parameters.AddWithValue("@tableName", tableName);
                    object result = command.ExecuteScalar();

                    return result != null;
                }
            }
            finally
            { }
        }

        #region ProductTable
        /// <summary>
        /// Gets all active prod
        /// </summary>
        public DataTable getAllProds()
        {
            SQLiteCommand cmd;
            string sql;
            DataTable dt = new DataTable();

            try
            {
                conn = new SQLiteConnection(conString);
                if (conn.State == ConnectionState.Closed) { conn.Open(); }
                sql = "Select ProductName, ProductId, ProductState, revision, inRecipe" +
                    " from Products Where prodActive = 1 Order by ProductName asc";
                cmd = new SQLiteCommand(sql, conn);

                using (SQLiteDataAdapter a = new SQLiteDataAdapter(cmd))
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
            conn = new SQLiteConnection(conString);
            if (conn.State == ConnectionState.Closed) { conn.Open(); }

            List<string> prodNames = new List<string>();
            try
            {
                string sql = "SELECT ProductName FROM Products";
                SQLiteCommand command = new SQLiteCommand(sql, conn);

                using (SQLiteDataReader dataReader = command.ExecuteReader())
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
            conn = new SQLiteConnection(conString);
            if (conn.State == ConnectionState.Closed) { conn.Open(); }

            List<string> prodNames = new List<string>();
            try
            {
                string sql = "SELECT ProductName FROM Products Where prodActive =1";
                SQLiteCommand command = new SQLiteCommand(sql, conn);

                using (SQLiteDataReader dataReader = command.ExecuteReader())
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
            conn = new SQLiteConnection(conString);
            if (conn.State == ConnectionState.Closed) { conn.Open(); }

            List<string> prodIds = new List<string>();
            try
            {
                string sql = "SELECT ProductId FROM Products";
                SQLiteCommand command = new SQLiteCommand(sql, conn);

                using (SQLiteDataReader dataReader = command.ExecuteReader())
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
                conn = new SQLiteConnection(conString);
                if (conn.State == ConnectionState.Closed) { conn.Open(); }

                string sql = "SELECT ProductId FROM Products where ProductName = @productName";
                SQLiteCommand cmd = new SQLiteCommand(sql, conn);
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
                conn = new SQLiteConnection(conString);
                if (conn.State == ConnectionState.Closed) { conn.Open(); }

                string sql = "SELECT ProductName FROM Products where yId = @yId and prodActive = 1";
                SQLiteCommand cmd = new SQLiteCommand(sql, conn);
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
                conn = new SQLiteConnection(conString);
                if (conn.State == ConnectionState.Closed) { conn.Open(); }

                string sql = "SELECT yId FROM Products Order by yId desc";
                SQLiteCommand cmd = new SQLiteCommand(sql, conn);
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
                conn = new SQLiteConnection(conString);
                if (conn.State == ConnectionState.Closed) { conn.Open(); }

                string sql = "SELECT yId FROM Products Where ProductId = @prodId";
                SQLiteCommand cmd = new SQLiteCommand(sql, conn);
                cmd.Parameters.AddWithValue("@prodId", prodId);
                id = Convert.ToInt16(cmd.ExecuteScalar());
            }
            finally
            { conn.Close(); }

            return id;
        }

        public void addProduct(string name, string id, string state)
        {
            SQLiteCommand cmd;
            string sql;

            int yId = setYId();
            try
            {
                conn = new SQLiteConnection(conString);
                if (conn.State == ConnectionState.Closed) { conn.Open(); }

                sql = "INSERT INTO Products (ProductID, ProductName, ProductState,yId, prodActive,revision, inRecipe)" +
                    " VALUES (@productid, @productname, @producttype,@yId, 1,0,0)";
                cmd = new SQLiteCommand(sql, conn);
                cmd.Parameters.AddWithValue("@productid", id);
                cmd.Parameters.AddWithValue("@productname", name);
                cmd.Parameters.AddWithValue("@producttype", state);
                cmd.Parameters.AddWithValue("@yId", yId);
                cmd.ExecuteNonQuery();
            }
            finally
            { conn.Close(); }
        }

        /// <summary>
        /// checks to see if product is in Recipe before deleting or updating
        /// </summary>
        /// <param name="id"></param>
        public bool productInRecipe(string id)
        {
            int yId = getYId(id);
            bool exists = false;
            SQLiteCommand cmd;
            string sql;
            try
            {
                conn = new SQLiteConnection(conString);
                if (conn.State == ConnectionState.Closed) { conn.Open(); }

                sql = "Select * from RecipeInfo Where yId = @yId";
                cmd = new SQLiteCommand(sql, conn);
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
                conn = new SQLiteConnection(conString);
                if (conn.State == ConnectionState.Closed) { conn.Open(); }

                string sql = "SELECT revision FROM Products Where yId = @yId Order by revision desc";
                SQLiteCommand cmd = new SQLiteCommand(sql, conn);
                cmd.Parameters.AddWithValue("@yId", yId);
                revision = Convert.ToInt16(cmd.ExecuteScalar());
            }
            finally
            { conn.Close(); }

            return revision;
        }

        public DataTable getProdRevisions(int yId)
        {
            SQLiteCommand cmd;
            string sql;
            DataTable dt = new DataTable();

            try
            {
                conn = new SQLiteConnection(conString);
                if (conn.State == ConnectionState.Closed) { conn.Open(); }
                sql = "Select ProductName, ProductId, ProductState, revision, changeDate" +
                    " from Products Where yId= @yId Order by revision desc";
                cmd = new SQLiteCommand(sql, conn);
                cmd.Parameters.AddWithValue("@yId", yId);

                using (SQLiteDataAdapter a = new SQLiteDataAdapter(cmd))
                { a.Fill(dt); }
            }
            finally
            { conn.Close(); }
            return dt;
        }

        /// <summary>
        /// if in recipe-updates old version of the product and makes new version - all changes except yId
        /// </summary>
        /// <param name="name"></param><param name="id"></param><param name="state"></param><param name="yId"></param>
        public void editProduct(string name, string id, string state,int yId)
        {
            SQLiteCommand cmdNew;
            string sqlNew;
            SQLiteCommand cmdUpdate;
            string sqlUpdate;
            int revision = getProdRevisionNum(yId) +1;

            try
            {
                conn = new SQLiteConnection(conString);
                if (conn.State == ConnectionState.Closed) 
                { conn.Open(); }

                sqlUpdate = "UPDATE Products SET prodActive = 0, changeDate = @date "
                    + "where prodActive = 1 and yId = @yId";
                cmdUpdate = new SQLiteCommand(sqlUpdate, conn);
                cmdUpdate.Parameters.AddWithValue("@date", DateTime.Now);
                cmdUpdate.Parameters.AddWithValue("@yId", yId);
                cmdUpdate.ExecuteNonQuery();

                sqlNew = "INSERT INTO Products (ProductID, ProductName, ProductState,yId, prodActive, revision, inRecipe)" +
                    " VALUES (@productid, @productname, @producttype,@yId,1, @revision,1)";
                cmdNew = new SQLiteCommand(sqlNew, conn);
                cmdNew.Parameters.AddWithValue("@productid", id);
                cmdNew.Parameters.AddWithValue("@productname", name);
                cmdNew.Parameters.AddWithValue("@producttype", state);
                cmdNew.Parameters.AddWithValue("@revision", revision);
                cmdNew.Parameters.AddWithValue("@yId", yId);
                cmdNew.ExecuteNonQuery();
            }
            finally
            { conn.Close(); }
        }
        
        /// <summary>
        /// if product is edited/deleted before used in Recipe then delete. if edit then treat as new prod
        /// </summary>
        /// <param name="prodId"></param>
        public void completeProdDelete(string id)
        {
            int yId = getYId(id);
            SQLiteCommand cmd;
            string sql;
            try
            {
                conn = new SQLiteConnection(conString);
                if (conn.State == ConnectionState.Closed) { conn.Open(); }

                sql = "Delete from Products Where yId = @yId";
                cmd = new SQLiteCommand(sql, conn);
                cmd.Parameters.AddWithValue("@yId", yId);
                cmd.ExecuteScalar();
            }
            finally
            { conn.Close(); }
        }
        
        public void markProdInactive(int yId)
        { 
            SQLiteCommand cmdUpdate;
            string sqlUpdate;

            try
            {
                conn = new SQLiteConnection(conString);
                if (conn.State == ConnectionState.Closed) 
                { conn.Open(); }

                sqlUpdate = "UPDATE Products SET prodActive = 0 "
                    + "where prodActive = 1 and yId = @yId";
                cmdUpdate = new SQLiteCommand(sqlUpdate, conn);
                cmdUpdate.Parameters.AddWithValue("@yId", yId);
                cmdUpdate.ExecuteNonQuery();
            }
            finally
            { conn.Close(); }
        }

        public List<Product> getRecipeProds(int rId)
        {
            conn = new SQLiteConnection(conString);
            if (conn.State == ConnectionState.Closed) { conn.Open(); }
            List<Product> prods = new List<Product>();
            try
            {
                string sql = "SELECT * FROM RecipeInfo Where rId = @rId";
                SQLiteCommand cmd = new SQLiteCommand(sql, conn);
                cmd.Parameters.AddWithValue("@rId", rId);
                using (SQLiteDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    { 
                        int yId = Convert.ToInt16(dataReader[0]);
                        double ratio = Convert.ToDouble(dataReader[1]);
                        string name = getProdName(yId);
                        Product prod = new Product(name,yId, ratio);
                        prods.Add(prod);
                    }
                }
            }
            finally
            { conn.Close(); }
            return prods;
        }

        public List<Product> getTicketProds(int rId)
        {
            conn = new SQLiteConnection(conString);
            if (conn.State == ConnectionState.Closed) { conn.Open(); }
            List<Product> prods = new List<Product>();
            try
            {
                string sql = "SELECT * FROM RecipeInfo Where rId = @rId";
                SQLiteCommand cmd = new SQLiteCommand(sql, conn);
                cmd.Parameters.AddWithValue("@rId", rId);
                using (SQLiteDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        int yId = Convert.ToInt16(dataReader[0]);
                        double ratio = Convert.ToDouble(dataReader[1]);
                        string name = getProdName(yId);
                        string id = getProdId(name);
                        Product prod = new Product(name, id, ratio/100); ;
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
                conn = new SQLiteConnection(conString);
                if (conn.State == ConnectionState.Closed) { conn.Open(); }

                string sql = "SELECT rId FROM RecipeList Order by rId desc";
                SQLiteCommand cmd = new SQLiteCommand(sql, conn);
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
                conn = new SQLiteConnection(conString);
                if (conn.State == ConnectionState.Closed) { conn.Open(); }

                string sql = "SELECT rId FROM RecipeList Where RecipeName = @recipeName";
                SQLiteCommand cmd = new SQLiteCommand(sql, conn);
                cmd.Parameters.AddWithValue("@recipeName", recipeName);
                id = Convert.ToInt16(cmd.ExecuteScalar());
            }
            finally
            { conn.Close(); }

            return id;
        }

        public DataTable getAllRecipeNames()
        {
            SQLiteCommand cmd;
            string sql;
            DataTable dt = new DataTable();

            try
            {
                conn = new SQLiteConnection(conString);
                if (conn.State == ConnectionState.Closed) { conn.Open(); }

                sql = "Select RecipeName from RecipeList Where recipeActive = 1 Order by RecipeName desc";
                cmd = new SQLiteCommand(sql, conn);

                using (SQLiteDataAdapter a = new SQLiteDataAdapter(cmd))
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
                conn = new SQLiteConnection(conString);
                if (conn.State == ConnectionState.Closed) { conn.Open(); }

                string sql = "SELECT RecipeName FROM RecipeList where rId = @rId";
                SQLiteCommand cmd = new SQLiteCommand(sql, conn);
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
            SQLiteCommand cmd;
            string sql;
            try
            {
                conn = new SQLiteConnection(conString);
                if (conn.State == ConnectionState.Closed) { conn.Open(); }

                sql = "SELECT COUNT(*) FROM RecipeList where RecipeName = @name";
                cmd = new SQLiteCommand(sql, conn);
                cmd.Parameters.AddWithValue("@name", name);
                int rowsReturned = Convert.ToInt32(cmd.ExecuteScalar());
                if (rowsReturned > 0)
                { exists = true; }
            }
            finally
            { conn.Close(); }
            return exists;
        }

        public void addRecipe(string name, List<int> yIdList, List<float> ratioList)
        {
            int i = 0;
            SQLiteCommand cmd;
            string sql;
            int rId = setRecipeId();
            try
            {
                conn = new SQLiteConnection(conString);
                if (conn.State == ConnectionState.Closed)
                { conn.Open(); }

                sql = "INSERT INTO RecipeList (RecipeName, rId, revision, recipeActive, ran)" +
                        " VALUES (@name, @id, 0, 1, 0)";
                cmd = new SQLiteCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Id", rId);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.ExecuteNonQuery();
                                
                foreach (int id in yIdList)
                {
                    if (conn.State == ConnectionState.Closed)
                    { conn.Open(); }
                    float ratio = ratioList[i];
                    sql = "INSERT INTO RecipeInfo (rId, yId, ratio,revision)"+
                        " VALUES (@rId, @yId,@ratio,0)";
                    cmd = new SQLiteCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@rId", rId);
                    cmd.Parameters.AddWithValue("@yId", id);
                    cmd.Parameters.AddWithValue("@ratio", ratio);
                    cmd.ExecuteNonQuery();

                    sql = "UPDATE Products SET inRecipe = 1 where prodActive = 1 and yId = @yId";
                    cmd = new SQLiteCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@yId", id);
                    cmd.ExecuteNonQuery();
                    i++;
                }
            }
            finally
            { conn.Close(); }
        }

        public void deleteRecipe(string recipeName)
        {
            int rId = getRid(recipeName);
            SQLiteCommand cmdInfo;
            string sqlInfo;
            SQLiteCommand cmdList;
            string sqlList;
            try
            {
                conn = new SQLiteConnection(conString);
                if (conn.State == ConnectionState.Closed) { conn.Open(); }

                sqlList= "Delete from RecipeInfo Where recipeId = @rId";
                cmdList= new SQLiteCommand(sqlList, conn);
                cmdList.Parameters.AddWithValue("@rId", rId);
                cmdList.ExecuteScalar();

                sqlInfo = "Delete from RecipeList Where recipeId = @rId";
                cmdInfo = new SQLiteCommand(sqlInfo, conn);
                cmdInfo.Parameters.AddWithValue("@rId", rId);
                cmdInfo.ExecuteScalar();
            }
            finally
            { conn.Close(); }
        }

        public List<int> getRecipeForProd(int yId)
        {
            conn = new SQLiteConnection(conString);
            if (conn.State == ConnectionState.Closed) { conn.Open(); }

            List<int> rIds = new List<int>();
            List<int> recipeIds = new List<int>();
            try
            {
                string sql = "SELECT rId FROM RecipeList Where recipeActive =1";
                SQLiteCommand cmd = new SQLiteCommand(sql, conn);
                using (SQLiteDataReader dR = cmd.ExecuteReader())
                {
                    while (dR.Read())
                    { rIds.Add(Convert.ToInt16(dR["rId"])); }
                }

                sql = "SELECT rId FROM RecipeInfo Where yId = @yId";
                cmd = new SQLiteCommand(sql, conn);
                cmd.Parameters.AddWithValue("@yId", yId);
                using (SQLiteDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        if (rIds.Contains(Convert.ToInt16(dataReader["rId"])))
                        { recipeIds.Add(Convert.ToInt16(dataReader["rId"]));}
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
            SQLiteCommand cmd;
            string sql;
            try
            {
                conn = new SQLiteConnection(conString);
                if (conn.State == ConnectionState.Closed) { conn.Open(); }

                sql = "Select ran from RecipeList Where rId = @rId";
                cmd = new SQLiteCommand(sql, conn);
                cmd.Parameters.AddWithValue("@rId", rId);
                int rowsReturned = Convert.ToInt32(cmd.ExecuteScalar());
                if (rowsReturned > 0)
                { exists = true; }
            }
            finally
            { conn.Close(); }
            return exists;
        }

        public void markRecipeInactive(int rId)
        {
            SQLiteCommand cmdUpdate;
            string sqlUpdate;

            try
            {
                conn = new SQLiteConnection(conString);
                if (conn.State == ConnectionState.Closed)
                { conn.Open(); }

                sqlUpdate = "UPDATE RecipeList SET recipeActive = 0 "
                    + "where recipeActive = 1 and rId = @rId";
                cmdUpdate = new SQLiteCommand(sqlUpdate, conn);
                cmdUpdate.Parameters.AddWithValue("@rId", rId);
                cmdUpdate.ExecuteNonQuery();
            }
            finally
            { conn.Close(); }
        }

        /// <summary>
        /// if recipe is edited/deleted before used then delete. if edit then treat as new recipe
        /// </summary>
        /// <param name="prodId"></param>
        public void completeRecipeDelete(int rId)
        {
            SQLiteCommand cmdList;
            string sqlList;
            SQLiteCommand cmdInfo;
            string sqlInfo;
            try
            {
                conn = new SQLiteConnection(conString);
                if (conn.State == ConnectionState.Closed) { conn.Open(); }

                sqlList = "Delete from RecipeList Where rId = @rId";
                cmdList = new SQLiteCommand(sqlList, conn);
                cmdList.Parameters.AddWithValue("@rId", rId);
                cmdList.ExecuteScalar();
                sqlInfo = "Delete from RecipeInfo Where rId = @rId";
                cmdInfo = new SQLiteCommand(sqlInfo, conn);
                cmdInfo.Parameters.AddWithValue("@rId", rId);
                cmdInfo.ExecuteScalar();
            }
            finally
            { conn.Close(); }
        }

        public int getRecipeRevisionNum(int rId)
        {
            int revision;
            try
            {
                conn = new SQLiteConnection(conString);
                if (conn.State == ConnectionState.Closed) { conn.Open(); }

                string sql = "SELECT revision FROM RecipeList Where rId = @rId Order by revision desc";
                SQLiteCommand cmd = new SQLiteCommand(sql, conn);
                cmd.Parameters.AddWithValue("@rId", rId);
                revision = Convert.ToInt16(cmd.ExecuteScalar());
            }
            finally
            { conn.Close(); }

            return revision;
        }

        /// <summary>
        /// if in history-updates old version of the recipe and makes new version
        /// </summary>
        /// <param name="name"></param><param name="id"></param><param name="state"></param><param name="yId"></param>
        public void editRecipeList(string name, int rId)
        {
            SQLiteCommand cmd;
            string sql;
            int revision = getRecipeRevisionNum(rId) + 1;

            try
            {
                conn = new SQLiteConnection(conString);
                if (conn.State == ConnectionState.Closed)
                { conn.Open(); }

                sql= "UPDATE RecipeInfo SET recipeActive = 0, changeDate = @date "
                    + "where recipeActive = 1 and recipeId = @rId";
                cmd= new SQLiteCommand(sql, conn);
                cmd.Parameters.AddWithValue("@date", DateTime.Now);
                cmd.Parameters.AddWithValue("@rId", rId);
                cmd.ExecuteNonQuery();

                sql = "INSERT INTO RecipeList (RecipeName,recipeId, revision,recipeActive,ran)" +
                    " VALUES (@name, @rId, @revision,1, 1)";
                cmd = new SQLiteCommand(sql, conn);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@rId", rId);
                cmd.Parameters.AddWithValue("@revision", revision);
                cmd.ExecuteNonQuery();
            }
            finally
            { conn.Close(); }
        }

        public void editRecipeInfo(int rId, List<int> yIdList, List<float> ratioList)
        {
            int i = 0;
            SQLiteCommand cmd;
            string sql;
            int revision = getRecipeRevisionNum(rId);
            try
            {
                conn = new SQLiteConnection(conString);
                if (conn.State == ConnectionState.Closed)
                { conn.Open(); }
                
                foreach (int id in yIdList)
                {
                    if (conn.State == ConnectionState.Closed)
                    { conn.Open(); }
                    float ratio = ratioList[i];
                    sql = "INSERT INTO RecipeInfo (recipeId, yId, ratio,revision)" +
                        " VALUES (@rId, @yId,@ratio,@revision)";
                    cmd = new SQLiteCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@rId", rId);
                    cmd.Parameters.AddWithValue("@yId", id);
                    cmd.Parameters.AddWithValue("@ratio", ratio);
                    cmd.Parameters.AddWithValue("@revision", revision);
                    cmd.ExecuteNonQuery();

                    sql = "UPDATE Products SET inRecipe = 1 where prodActive = 1 and yId = @yId";
                    cmd = new SQLiteCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@yId", id);
                    cmd.ExecuteNonQuery();
                    i++;
                }
            }
            finally
            { conn.Close(); }
        }
        #endregion RecipeTable

        #region HistoryTable
        public DataTable getHistory()
        {
            DataTable results = new DataTable();
            string sql = "";
            SQLiteCommand cmd = new SQLiteCommand();
            sql = "Select * from History Order by RanDate desc";
            cmd = new SQLiteCommand(sql, conn);

            DataTable dt = setupHistoryDt();
            try
            {
                conn.Open();
                using (SQLiteDataAdapter a = new SQLiteDataAdapter(cmd))
                { a.Fill(results); }
                dt = fillInHistoryDt(results, dt);
            }
            finally
            { conn.Close(); }
            return dt;
        }

        public DataTable getHistory(bool useProd, bool useRecipe,bool useDate, string prod,
            string recipe, DateTime from, DateTime to)
        {
            DataTable dt = new DataTable();
            dt = getHistory();
            DataTable final = new DataTable();
            final = setupHistoryDt();
                        
            foreach (DataRow dr in dt.Rows)
            {
                bool added = false;
                if (useDate)
                {
                    if (Convert.ToDateTime(dr[1]) < to && Convert.ToDateTime(dr[1]) > from)
                    { 
                        final.Rows.Add(dr.ItemArray);
                        added = true;
                    }
                }
                if (useRecipe && !added)
                {
                    if (dr[0].ToString().ToLower() == recipe.ToLower())
                    {
                        final.Rows.Add(dr.ItemArray);
                        added = true;
                    }
                }
                if (useProd && !added)
                {
                    if (dr[prod].ToString() != "")
                    { 
                        final.Rows.Add(dr.ItemArray);
                        added = true;
                    }
                }
            }
            return final;
        }

        public DataTable getHistory(string prod, DateTime from, DateTime to)
        {
            DataTable dt = getHistory();
            DataTable final = setupHistoryDt();
            bool entered = false;

            foreach (DataRow dr in dt.Rows)
            {
                if (Convert.ToDateTime(dr[1]) < to && Convert.ToDateTime(dr[1]) > from)
                { 
                    final.Rows.Add(dr.ItemArray);
                    entered = true;
                }
                
                if (!entered && dr[prod].ToString() != "")
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

        public void recordRun(int rId, float ranWeight, DateTime date)
        {
            SQLiteCommand cmd;
            string sql;

            try
            {
                conn = new SQLiteConnection(conString);
                if (conn.State == ConnectionState.Closed) { conn.Open(); }

                sql = "INSERT INTO History (rId, RanWeight, RanDate)"+
                    " VALUES (@rId, @weight, @date)";
                cmd = new SQLiteCommand(sql, conn);
                cmd.Parameters.AddWithValue("@rId", rId);
                cmd.Parameters.AddWithValue("@weight", ranWeight);
                cmd.Parameters.AddWithValue("@date", date);
                cmd.ExecuteNonQuery();

                sql= "UPDATE RecipeList SET ran = 1 where recipeActive = 1 and rId = @rId";
                cmd= new SQLiteCommand(sql, conn);
                cmd.Parameters.AddWithValue("@rId", rId);
                cmd.ExecuteNonQuery();
            }
            finally
            { conn.Close(); }
        }

        public void deleteAllHistory()
        {
            SQLiteCommand cmd;
            string sql;
            try
            {
                conn = new SQLiteConnection(conString);
                if (conn.State == ConnectionState.Closed) { conn.Open(); }

                sql = "Delete from History";
                cmd = new SQLiteCommand(sql, conn);
                cmd.ExecuteScalar();

                sql = "UPDATE RecipeList SET ran = 0 "
                    + "where recipeActive = 1";
                cmd = new SQLiteCommand(sql, conn);
                cmd.ExecuteNonQuery();
            }
            finally
            { conn.Close(); }
        }
        #endregion HistoryTable
    }
}

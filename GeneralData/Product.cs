using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeneralData
{
    public enum ProductType
    {
        Solid,
        LiquidMicroNutrient,
        LiquidCoating,
        LiquidPesticideHerbicide
    }

    /// <summary>
    /// The Product class is similar to a chemical, but has amounts associated with it.
    /// </summary>
    public class Product : IComparable
    {
        /// <summary>
        /// The type of chemical that the Product is comprised of.
        /// </summary>
        private Chemical chemical;

        public double Ratio { get; set; }

        public double Yid { get; set; }

        /// <summary>
        /// The real-world amount of the product that was used in the blend.
        /// </summary>
        public float ActualAmount { get; set; }

        public string Name
        { get { return chemical.Name; } }

        public string Id
        { get { return chemical.Uid; } }

        public ProductType Type { get; set; }

        #region Constructors
        public Product()
        {
            chemical = new Chemical();
            ActualAmount = new float();
        }

        public Product(string name, double ratio)
        {
            chemical = new Chemical(name);
            Ratio = ratio;
        }

        public Product(string name, string id, double ratio)
        {
            chemical = new Chemical(name,id);
            Ratio = ratio;
        }

        public Product(string name, int yId, double ratio)
        {
            chemical = new Chemical(name);
            Yid = yId;
            Ratio = ratio;
        }

        public Product(int yId, double ratio)
        {
            Yid = yId;
            Ratio = ratio;
        }

        public Product(string name, string id, int yId, double ratio)
        {
            chemical = new Chemical(name, id);
            Ratio = ratio;
            Yid = yId;
        }

        public Product(string name, string id, int yId, double ratio, float actualWeight)
        {
            chemical = new Chemical(name, id);
            Ratio = ratio;
            Yid = yId;
            ActualAmount = actualWeight;
        }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// Allows a collection of products to be sorted based on the product's name.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        int IComparable.CompareTo(object obj)
        {
            Product temp = obj as Product;
            if (temp != null)
            { return string.Compare(this.Name, temp.Name); }
            else
            { throw new ArgumentException("Parameter is not a valid object (Product)."); }
        }

        public override bool Equals(object obj)
        {
            if ((obj is Product) && (obj != null))
            {
                Product temp;
                temp = (Product)obj;
                if (temp.Id == this.Id)
                { return true; }
                else
                { return false; }
            }
            else
            { return false; }
        }

        public static bool operator ==(Product prod1, Product prod2)
        { return prod1.Equals(prod2); }

        public static bool operator !=(Product prod1, Product prod2)
        { return !(prod1 == prod2); }
        #endregion Methods

    }
}

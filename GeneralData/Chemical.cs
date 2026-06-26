using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeneralData
{
    [Serializable]
    public class Chemical : IComparable
    {
        #region Fields
        private string name;
        private string uid;
        #endregion Fields

        #region Properties
        public string Name
        {
            get { return name; }

            protected set
            {
                if (!string.IsNullOrWhiteSpace(value))
                { name = value; }
            }
        }

        public string Uid
        {
            get { return uid; }

            protected set
            {
                if (value != null)
                { uid = value; }
            }
        }

        public string ChemicalType { get; protected set; }
        #endregion Properties

        #region Constructors
        public Chemical()
        {
            // an empty chemical
            name = "none";
            uid = "";
        }

        public Chemical(string _name)
        {
            name = _name;
            uid = Math.Abs(GetHashCode()).ToString();
        }

        public Chemical(string _name, string _uid)
        {
            name = _name;
            Uid = _uid;
        }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// Allows a collection of chemicals to be sorted based on the chemicals name.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        int IComparable.CompareTo(object obj)
        {
            Chemical temp = obj as Chemical;
            if (temp != null)
            { return string.Compare(this.Name, temp.Name); }
            else
            { throw new ArgumentException("Parameter is not a valid object (Chemical)."); }
        }

        // Compare two chemicals. If two chemicals are 
        // the same then true is returned.
        public override bool Equals(object obj)
        {
            if ((obj is Chemical) && (obj != null))
            {
                Chemical temp;
                temp = (Chemical)obj;
                if (temp.Uid == this.Uid)
                { return true; }
                else
                { return false; }
            }
            else
            { return false; }
        }

        // Overloading the == operator.
        public static bool operator ==(Chemical chem1, Chemical chem2)
        {
            if (System.Object.ReferenceEquals(chem1, chem2))
            { return true; }

            if (((object)chem1 == null || ((object)chem2 == null)))
            { return false; }
            return chem1.Uid == chem2.Uid;
        }

        public static bool operator !=(Chemical chem1, Chemical chem2)
        { return !(chem1 == chem2); }

        public override int GetHashCode()
        { return name.ToLower().GetHashCode(); }
        #endregion Methods
    }
}

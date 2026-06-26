using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeneralData
{
    public class Hoppers
    {
        private float amount;

        public string Name { get; set; }
        public int Counter { get; set; }
        public float TotalAmount { get; set; }
        public float Amount
        {
            get
            {
                if (Counter > 0)
                { return amount / Counter; }
                else
                { return 0; }
            }
            set
            { amount = value; }
        }

        public Hoppers(string name)
        {
            Name = name;
            Counter = 0;
            Amount = 0;
        }
    }
}

using Database;
using GeneralData;
using PlcComms;
using PrintBatches;
using Recipes;
using Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SoftwareInteraction
{
    public class RecipeInteraction
    {
        public RequestType Type { get { return RequestType.Ticket; } }
        private PlcInteraction plc;
        public PlcInteraction Plc { get { return plc; } set { plc = value; } }
        private string recipeName;
        private List<float> amounts;
        private List<Product> prodList;
        string recipe;
        Databases db;

        #region Constructor
        public RecipeInteraction()
        {
            this.amounts = new List<float>();
            db = new Databases();
        }
        #endregion Constructor

        /// <summary>
        /// Verifies that Recipe exists and can be read in.
        /// </summary>
        /// <returns>Returns true if file can be found and read; else a notification is sent to the PLC and false is returned.</returns>
        public bool verifyRequest()
        {
            if (plc != null)
            {
                recipeName = plc.GetRequest();
                recipeName = recipeName.Trim();
                recipeName = recipeName.Replace("\0", "");

                if (verifyTicketExists())
                {
                    getRequestedTicketFile();
                    if (verifyProductHopperMatch())
                    { return true; }
                }
                else
                { plc.ticketDNE(); }
            }
            return false;
        }

        public bool verifyProcess()
        {
            if (plc != null)
            {
                recipeName = plc.GetProcessTicketNumber();
                recipeName = recipeName.Trim();
                recipeName = recipeName.Replace("\0", "");

                if (verifyTicketExists())
                {return true;}
            }
            return false;
        }

        /// <summary>
        /// This checks if the product names match the names given to the hoppers on a blend system.
        /// </summary>
        /// <returns>Returns true if all hopper and product names match for a blend. Else false is returned.</returns>
        private bool verifyProductHopperMatch()
        {
            // Get a list of the names that are currently assigned to hoppers.
            List<string> hopperNames = new List<string>();
            float numOfHoppers = plc.GetNumOfHoppers();

            for (int i = 1; i <= numOfHoppers; i++)
            {
                string tempName = plc.GetHopperNames(i);
                hopperNames.Add(tempName);
            }

            List<string> productsNotAssigned = new List<string>();
            // Cycle through the list and make sure each product in the recipe matches a hopper.
            foreach (Product product in prodList)
            {
                bool isOk = false;
                foreach (string hopperName in hopperNames)
                {
                    if (hopperName.ToLower() == product.Name.ToLower())
                    {
                        isOk = true;
                        break;
                    }
                }
                // If the product isn't assigned to a hopper, add it to the list so it can be reported.
                if (!isOk)
                {
                    //put name here so they know what product isn't there
                    productsNotAssigned.Add(product.Name);
                }
            }

            if (productsNotAssigned.Count > 0)
            {
                // send error to PLC.
                plc.SendPcBufferData(productsNotAssigned);
                plc.SendError(ErrorCode.ProductHopperMismatch);
                // display error
                string productsMismatched = "";
                foreach (string value in productsNotAssigned)
                { productsMismatched += value + " "; }

                throw new ArgumentException("Product mismatch with recipe listing and hopper names. " + productsMismatched);
                // return false;
            }
            return true;
        }

        /// <summary>
        /// When a ticket request is sent from the PLC, this method checks to see if the ticket file exist in the file system.
        /// </summary>
        /// <returns>Returns true if the ticket is found; else returns false;</returns>
        private bool verifyTicketExists()
        {
            bool ticketExist = db.checkRecipeExists(recipeName);
            return ticketExist;
        }

        private void getRequestedTicketFile()
        {
            int rId = db.getRid(recipeName);
            prodList = db.getRecipeProds(rId); 
        }

        /// <summary>
        /// processRequest() sorts through the list of products that are in the requested ticket and the list of hoppers on the blend system.
        /// It creates a list of amounts that match with the appropriate hopper. This list is the object which is sent in plc.SendBufferData.
        /// </summary>
        public void processRequest()
        {
            // Get a list of the names that are currently assigned to hoppers.
            List<string> hopperNames = new List<string>();
            float numOfHoppers = plc.GetNumOfHoppers();
            for (int i = 1; i <= numOfHoppers; i++)
            {
                // remove any whitespace/extra null characters
                //  at the end of the string.
                string tempName = plc.GetHopperNames(i);
                tempName = tempName.Trim();
                tempName = tempName.Replace("\0", "");
                hopperNames.Add(tempName);
            }

            List<float> amounts = new List<float>();

            // Check if there are any products assigned to more than one hopper.
            List<Hoppers> hoppers = getHoppers(hopperNames);

            if (hoppers.Count < hopperNames.Count)
            {
                // count how many hoppers per product
                foreach (Hoppers h in hoppers)
                {
                    foreach (string hopper in hopperNames)
                    {
                        if (h.Name.ToLower() == hopper.ToLower())
                        { h.Counter++; }
                    }
                }

                // assign amounts based on the ticket
                foreach (Hoppers h in hoppers)
                {
                    foreach (Product prod in prodList)
                    {
                        if (h.Name.ToLower() == prod.Name.ToLower())
                        { h.Amount = (float)prod.Ratio /100; }
                    }
                }

                // create a list of amounts per hopper
                foreach (string name in hopperNames)
                {
                    foreach (Hoppers h in hoppers)
                    {
                        if (h.Name.ToLower() == name.ToLower())
                        {
                            amounts.Add(h.Amount);
                            break;
                        }
                    }
                }
                this.amounts = amounts;
            }
            else // there are no products that are assigned to more than one hopper
            {
                // Calculate the amount of each product. 
                foreach (string HopperName in hopperNames)
                {
                    bool amountAddedToHopper = false;
                    foreach (Product product in prodList)
                    {
                        if (HopperName.ToLower() == product.Name.ToLower())
                        {
                            amounts.Add((float)product.Ratio / 100);
                            amountAddedToHopper = true;
                            break;
                        }
                    }
                    if (amountAddedToHopper == false)
                    { amounts.Add((float)0); }
                }
            }
            this.amounts = amounts;
        }

        public List<Hoppers> getHoppers(List<string> hopperNames)
        {
            List<Hoppers> listOfProducts = new List<Hoppers>();
            bool inList = false;
            foreach (string hopper in hopperNames)
            {
                inList = false;
                foreach (Hoppers h in listOfProducts)
                {
                    if (h.Name.ToLower() == hopper.ToLower())
                    { inList = true; }
                }
                if (inList == false)
                { listOfProducts.Add(new Hoppers(hopper)); }
            }
            return listOfProducts;
        }

        /// <summary>
        /// Sends a list of floating point numbers to the PLC.
        /// These numbers represent the weights for each product to be ran in a blend.
        /// This code should remain unchanged among future versions unless there are dramatic system changes.
        /// </summary>
        public void sendData()
        {plc.SendPcBufferData(amounts);}

        /// <summary>
        /// Writes back the ACT file
        /// </summary>
        public void storeData()
        {
            int overheadHopperNum = plc.GetOverheadNumber();
            float overheadTare = plc.GetOverheadTare();
            float customerEnteredWeight = plc.GetCustomerRequestedWeight();

            recipe = plc.GetProcessTicketNumber();
            recipe = recipe.Trim();
            recipe = recipe.Replace("\0", "");

            int rId = db.getRid(recipe);
            DateTime date = DateTime.Now;
            db.recordRun(rId, overheadTare, date);

            if (LRP.Default.PrintSettings != "")
            {
                List<Product> prods = new List<Product>();
                prods = db.getTicketProds(rId);
                PrintBatch print = new PrintBatch(recipe, overheadTare, LRP.Default.PrintSettings, customerEnteredWeight,
                    LRP.Default.useMetric, prods);
                print.Show();
                print.printTicket();
            }
        }
    }
}

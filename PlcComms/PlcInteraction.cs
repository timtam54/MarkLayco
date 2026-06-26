using PlcComms.Master;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PlcComms
{
    /// <summary>
    /// Status of the communication process between the PC and PLC.
    /// </summary>
    public enum CommStatus
    {
        Null = 0,
        Request = 1,
        Updated = 2,
        Finished = 3,
        Downloaded = 4
    }

    /// <summary>
    /// The type of request that the PLC is making to the PC.
    /// </summary>
    public enum RequestType
    {
        ProductName = 12,
        Recipe = 13,
        Ticket = 14
    }

    /// <summary>
    /// Type of error message.
    /// </summary>
    public enum ErrorCode
    {
        ProductHopperMismatch,
        InvalidRequest
    }

    public class PlcInteraction
    {
        #region Fields
        //V memory address
        private ushort statusFieldAddress = 1087;           //2077   //previously 6170
        private ushort requestFieldAddress = 4672;          //11100  //previously 6260
        private ushort requestTicketNumAddress = 4672;      //11100  //previously 6260
        private ushort processTicketNumAddress = 4688;      //11120
        private ushort firstAmountFieldAddress = 4737;      //11201  //previously 6200
        private ushort finalBlendWeight = 4737;             //11201  //previously 6200
        private ushort numberOfHoppersFieldAddress = 416;   //640    //perviously 1472
        private ushort firstHopperNameFieldAddress = 4160;  //10100  //previously 10000
        private ushort errorFieldAddress = 1151;            //2177   //previously 6171
        private ushort impregFieldAddress1 = 16788;         //40624  //previously 40735
        private ushort bulkDensityAddress = 1089;           //2101   //previously 1555
        private ushort fullAmountFieldAddress = 3198;       //6176
        private ushort manualOrAutoAddress = 3263;          //6277
        private ushort manualProductNameAddress = 3255;     //6267
        private ushort overheadHopperNumAddress = 3262;     //6276
        private ushort sendRecieveTotalWeight = 3816;       //7350
        private ushort getRequestedWeight = 4739;           //11203

        // Status Field bit configuration
        //  bit 14: Request Type - Ticket
        //  bit 13: Request Type - Recipe
        //  bit 12: Request Type - Product Name
        //  bit 3: Ticket Downloaded
        //  bit 2: Ticket Finished
        //  bit 1: Hopper Values Updated
        //  bit 0: Ticket Request
        private ushort bitTicketRequest = 14;
        private ushort bitRecipeRequest = 13;
        private ushort bitProductNameRequest = 12;
        private ushort bitDownloaded = 3;
        private ushort bitFinished = 2;
        private ushort bitUpdated = 1;
        private ushort bitRequest = 0;

        private ushort bitAuto = 0;
        private ushort bitManual = 1;

        // PLC's ip address
        private string ipAddress;
        private string defaultIpAddress = "0.0.0.0";

        public string fileName = @"C:\ProgramData\Yargus\debug.txt";
        #endregion Fields

        #region Properties
        public ushort StatusFieldAddress
        {
            get { return statusFieldAddress; }
            set { statusFieldAddress = value; }
        }

        public ushort RequestFieldAddress
        {
            get { return requestFieldAddress; }
            set { requestFieldAddress = value; }
        }

        public ushort FirstAmountFieldAddress
        {
            get { return firstAmountFieldAddress; }
            set { firstAmountFieldAddress = value; }
        }

        public ushort NumberOfHoppersFieldAddress
        {
            get { return numberOfHoppersFieldAddress; }
            set { numberOfHoppersFieldAddress = value; }
        }

        public ushort ErrorFieldAddress
        {
            get { return errorFieldAddress; }
            set { errorFieldAddress = value; }
        }

        public bool InProgressBit { get; set; }

        public bool MoreBit { get; set; }

        public string IpAddress
        {
            get { return ipAddress; }
            set { ipAddress = value; }
        }
        #endregion Properties

        #region Constructors
        public PlcInteraction()
        { ipAddress = defaultIpAddress; }

        public PlcInteraction(string ipAddress)
        { this.ipAddress = ipAddress; }
        #endregion Constructors

        /// <summary>
        /// Sends string to debug.txt
        /// contains info about variable values and methods ran
        /// </summary>
        /// <param name="_strToDebug">values of variables to display in text file</param>
        /// <param name="_callingFunction">display what method was called</param>
        public void writeToDebug(string _strToDebug, string _callingFunction)
        {
            string curTime = Convert.ToString(DateTime.Now);
            FileStream fs = new FileStream(fileName, FileMode.Append, FileAccess.Write);
            byte[] buffer = Encoding.ASCII.GetBytes(curTime + "\t" + _callingFunction + "\t" + _strToDebug + "\r\n");
            fs.Write(buffer, 0, buffer.Length);
            fs.Flush();
            fs.Close();
        }

        public CommStatus CheckStatusField()
        {
            ushort readStatusField = new ushort();
            bool downloadedBitSet = false;
            bool finishedBitSet = false;
            bool updatedBitSet = false;
            bool requestBitSet = false;

            // throw an error is ipAddress is blank/null
            if (string.IsNullOrWhiteSpace(ipAddress))
            { throw new ArgumentNullException(); }

            // set up new connection to PLC and read the status field
            PlcTcpMaster conn = new PlcTcpMaster(ipAddress);
            readStatusField = conn.readInt16(statusFieldAddress);

            downloadedBitSet = IsBitSet(readStatusField, bitDownloaded);
            finishedBitSet = IsBitSet(readStatusField, bitFinished);
            updatedBitSet = IsBitSet(readStatusField, bitUpdated);
            requestBitSet = IsBitSet(readStatusField, bitRequest);

            if (!downloadedBitSet && !finishedBitSet && !updatedBitSet && !requestBitSet)
            { return CommStatus.Null; }
            else if (downloadedBitSet && !finishedBitSet && !updatedBitSet && !requestBitSet)
            { return CommStatus.Downloaded; }
            else if (!downloadedBitSet && finishedBitSet && !updatedBitSet && !requestBitSet)
            { return CommStatus.Finished; }
            else if (!downloadedBitSet && !finishedBitSet && updatedBitSet && !requestBitSet)
            { return CommStatus.Updated; }
            else if (!downloadedBitSet && !finishedBitSet && !updatedBitSet && requestBitSet)
            { return CommStatus.Request; }
            else
            { throw new InvalidOperationException("More than one status bit is set within the PLC at memory location " + statusFieldAddress + ". The read data is : " + readStatusField); }
        }

        /// <summary>
        /// Writes amounts of a product to add to mem locations of the PLC
        /// </summary>
        /// <param name="_listOfAmounts">A list of weight for products</param>
        /// <returns>Returns a boolean if complete or not</returns>
        public bool SendPcBufferData(List<float> _listOfAmounts)
        {
            // first memory location to write to
            int memLocation = firstAmountFieldAddress;
            int sizeOfAmountFields = 2;

            try
            {
                // new connection to PLC
                PlcTcpMaster conn = new PlcTcpMaster(ipAddress);

                foreach (float amount in _listOfAmounts)
                {
                    conn.writeFloat(amount, memLocation);
                    memLocation += sizeOfAmountFields;
                }
            }
            catch
            {
                // error
                return false;
            }

            return true; // everything went ok
        }

        public bool SendPcBufferData(float totalWeight)
        {
            // first memory location to write to
            int memWeightLocation = sendRecieveTotalWeight;

            try
            {
                // new connection to PLC
                PlcTcpMaster conn = new PlcTcpMaster(ipAddress);
                conn.writeFloat(totalWeight, memWeightLocation);
            }
            catch
            { return false; }
            return true; // everything went ok
        }

        /// <summary>
        /// Writes names of a products to add to mem locations of the PLC
        /// </summary>
        /// <param name="_listOfText">List of Product Names</param>
        /// <returns>Returns a boolean if complete or not</returns>
        public bool SendPcBufferData(List<string> _listOfText)
        {
            // first memory location to write to
            int memLocation = firstAmountFieldAddress;
            int sizeOfAmountFields = 7;

            try
            {
                // new connection to PLC
                PlcTcpMaster conn = new PlcTcpMaster(ipAddress);

                foreach (string text in _listOfText)
                {
                    conn.writeText(text, 14, memLocation);
                    memLocation += sizeOfAmountFields;
                }
            }
            catch
            {
                // error
                return false;
            }

            return true; // everything went ok
        }

        /// <summary>
        /// Gets the requested ticket or blend number from PLC when Request bit is set.
        /// </summary>
        /// <returns>Returns a String representing the ticket number</returns>
        public string GetRequest()
        {
            string requestedNumber;

            // throw an error if ipAddress is blank/null
            if (string.IsNullOrWhiteSpace(ipAddress))
                throw new ArgumentNullException();

            // set up new connection to PLC and read the status field
            PlcTcpMaster conn = new PlcTcpMaster(ipAddress);
            requestedNumber = conn.readText(requestFieldAddress, 32, false);
            return requestedNumber;
        }

        public bool SetValuesUpdatedField()
        {
            // bit 1 // previously also set 4
            int setData = 2; //18;

            // If the More Bit is set, set bit 8 (by adding 256).
            if (MoreBit == true)
            {
                setData += 256;
                MoreBit = false;
            }

            // throw an error is ipAddress is blank/null
            if (string.IsNullOrWhiteSpace(ipAddress))
            { throw new ArgumentNullException(); }

            try
            {
                // set up new connection to PLC and read the status field
                PlcTcpMaster conn = new PlcTcpMaster(ipAddress);
                // Set the Hopper Values Updated bit
                conn.writeInt(setData, statusFieldAddress);
            }
            catch
            { return false; }
            return true;
        }

        public bool SetDownloadedBit()
        {
            // bit 3
            int setData = 24;

            // throw an error is ipAddress is blank/null
            if (string.IsNullOrWhiteSpace(ipAddress))
            { throw new ArgumentNullException(); }

            try
            {
                // set up new connection to PLC and read the status field
                PlcTcpMaster conn = new PlcTcpMaster(ipAddress);
                // set the Ticket Downloadedbit
                conn.writeInt(setData, statusFieldAddress);
                ushort readStatusField = conn.readInt16(statusFieldAddress);
            }
            catch (Exception ex)
            { return false; }
            return true;
        }

        public float DownloadActualAmounts(int hopperNumber)
        {
            float actualAmount = new float();
            // size of amount field is 2
            int memLocation = firstAmountFieldAddress + (2 * (hopperNumber - 1));

            // throw an error is ipAddress is blank/null
            if (string.IsNullOrWhiteSpace(ipAddress))
            { throw new ArgumentNullException(); }

            // set up new connection to PLC and read the status field
            PlcTcpMaster conn = new PlcTcpMaster(ipAddress);
            actualAmount = conn.readFloat(memLocation);

            return actualAmount;
        }

        public bool ClearStatus()
        {
            int writeData = 0;

            // throw an error is ipAddress is blank/null
            if (string.IsNullOrWhiteSpace(ipAddress))
            { throw new ArgumentNullException(); }
            try
            {
                // set up new connection to PLC and read the status field
                PlcTcpMaster conn = new PlcTcpMaster(ipAddress);
                // Clear the status field
                conn.writeInt(writeData, statusFieldAddress);
            }
            catch
            { return false; }
            return true;
        }

        public RequestType CheckRequestType()
        {
            // throw an error is ipAddress is blank/null
            if (string.IsNullOrWhiteSpace(ipAddress))
            { throw new ArgumentNullException(); }

            // set up new connection to PLC and read the status field
            ushort readStatusField = new ushort();
            PlcTcpMaster conn = new PlcTcpMaster(ipAddress);
            readStatusField = conn.readInt16(statusFieldAddress);

            bool ticketRequested = IsBitSet(readStatusField, bitTicketRequest);

            // Make sure only one bit is set. If not, an exception is thrown.
            if (ticketRequested)
            { return RequestType.Ticket; }
            else if (!ticketRequested)
            {
                throw new InvalidOperationException("None of the Request Type bits are set. Memory location "
                + statusFieldAddress + ". The read data is : " + readStatusField);
            }
            else
            {
                throw new InvalidOperationException("More than one status bit is set within the PLC at memory location "
                  + statusFieldAddress + ". The read data is : " + readStatusField);
            }
        }

        public bool ManualCheck()
        {
            ushort modeCheck = new ushort();

            // throw an error if ipAddress is blank/null
            if (string.IsNullOrWhiteSpace(ipAddress))
            { throw new ArgumentNullException(); }

            // set up new connection to PLC and read the status field
            PlcTcpMaster conn = new PlcTcpMaster(ipAddress);
            modeCheck = conn.readInt16(manualOrAutoAddress, false);

            bool autoRun = false;
            bool manualRun = false;

            autoRun = IsBitSet(modeCheck, bitAuto);
            manualRun = IsBitSet(modeCheck, bitManual);

            if (autoRun && !manualRun)
            { return false; }
            else if (!autoRun && manualRun)
            { return true; }
            else
            {
                throw new InvalidOperationException("More than one status bit is set within the PLC at memory location "
                + manualOrAutoAddress + ". The read data is : " + modeCheck);
            }
        }

        /// <summary>
        ///  Gets the whole sale product name from the PLC for a manual run.
        /// </summary>
        /// <returns>Returns a String containing the product name of a wholesale item</returns>
        public string GetManualProductName()
        {
            string manualProdName;

            // throw an error if ipAddress is blank/null
            if (string.IsNullOrWhiteSpace(ipAddress))
            { throw new ArgumentNullException(); }

            // set up new connection to PLC and read the status field
            PlcTcpMaster conn = new PlcTcpMaster(ipAddress);
            manualProdName = conn.readText(manualProductNameAddress, 14, false);

            return manualProdName;
        }

        public string GetRequestedTicketNumber()
        {
            string ticketNumber = "";

            if (string.IsNullOrWhiteSpace(ipAddress))
            { throw new ArgumentNullException(); }

            // set up new connection to PLC and read the status field
            PlcTcpMaster conn = new PlcTcpMaster(ipAddress);
            ticketNumber = conn.readText(requestTicketNumAddress, 14, false);

            return ticketNumber;
        }

        public string GetProcessTicketNumber()
        {
            string ticketNumber = "";

            if (string.IsNullOrWhiteSpace(ipAddress))
            { throw new ArgumentNullException(); }

            // set up new connection to PLC and read the status field
            PlcTcpMaster conn = new PlcTcpMaster(ipAddress);
            ticketNumber = conn.readText(processTicketNumAddress, 14, false);

            return ticketNumber;
        }

        public int GetOverheadNumber()
        {
            int overheadHopperNum = 0;

            // throw an error if ipAddress is blank/null
            if (string.IsNullOrWhiteSpace(ipAddress))
            { throw new ArgumentNullException(); }

            // set up new connection to PLC and read the status field
            PlcTcpMaster conn = new PlcTcpMaster(ipAddress);
            overheadHopperNum = conn.readInt(overheadHopperNumAddress, false);

            return overheadHopperNum;
        }

        public float GetOverheadTare()
        {
            float overheadHopperTare = 0;

            // size of float field is 72 memory locations
            /* Removed since tare weight goes to only one address 
             * int otaMemAddress = overheadTareAddress +(72 * (_overHeadHopperNum - 1));   
             */

            // throw an error if ipAddress is blank/null
            if (string.IsNullOrWhiteSpace(ipAddress))
            { throw new ArgumentNullException(); }

            // set up new connection to PLC and read the status field
            PlcTcpMaster conn = new PlcTcpMaster(ipAddress);
            overheadHopperTare = conn.readFloat(finalBlendWeight, false);

            return overheadHopperTare;
        }

        public float GetCustomerRequestedWeight()
        {
            float requestedWeight = 0;

            // size of float field is 72 memory locations
            /* Removed since tare weight goes to only one address 
             * int otaMemAddress = overheadTareAddress +(72 * (_overHeadHopperNum - 1));   
             */

            // throw an error if ipAddress is blank/null
            if (string.IsNullOrWhiteSpace(ipAddress))
            { throw new ArgumentNullException(); }

            // set up new connection to PLC and read the status field
            PlcTcpMaster conn = new PlcTcpMaster(ipAddress);
            requestedWeight = conn.readFloat(getRequestedWeight, false);

            return requestedWeight;
        }

        public float GetFullAmount()
        {
            float fullAmount = new float();

            // throw an error is ipAddress is blank/null
            if (string.IsNullOrWhiteSpace(ipAddress))
            { throw new ArgumentNullException(); }

            // set up new connection to PLC and read the status field
            PlcTcpMaster conn = new PlcTcpMaster(ipAddress);
            fullAmount = conn.readFloat(fullAmountFieldAddress);

            return fullAmount;
        }

        public string GetHopperNames(int hopperNumber)
        {
            string hopperName = "";
            // size of text field is 16 memory locations
            int memLocation = firstHopperNameFieldAddress + (16 * (hopperNumber - 1));

            // throw an error is ipAddress is blank/null
            if (string.IsNullOrWhiteSpace(ipAddress))
            { throw new ArgumentNullException(); }

            // set up new connection to PLC and read the hopper name
            PlcTcpMaster conn = new PlcTcpMaster(ipAddress);
            hopperName = conn.readText(memLocation, 32);

            // remove any whitespace/extra null characters
            //  at the end of the string.
            hopperName = hopperName.Trim();
            hopperName = hopperName.Replace("\0", "");

            return hopperName;
        }

        public void SetHopperName(int hopperNum, string name)
        {
            int memLocation = firstHopperNameFieldAddress + (16 * (hopperNum - 1));
            PlcTcpMaster conn = new PlcTcpMaster(ipAddress);
            conn.writeText(name, 32, memLocation);
        }

        public float getBulkDensity(int hopperNumber)
        {
            float bulkD = new float();
            int memLocation = bulkDensityAddress + (2 * (hopperNumber - 1));

            // throw an error is ipAddress is blank/null
            if (string.IsNullOrWhiteSpace(ipAddress))
            { throw new ArgumentNullException(); }

            // set up new connection to PLC and read the status field
            PlcTcpMaster conn = new PlcTcpMaster(ipAddress);
            bulkD = conn.readFloat(memLocation);

            return bulkD;
        }

        public void ticketDNE()
        {
            PlcTcpMaster conn = new PlcTcpMaster(ipAddress);
            conn.writeInt(16384, errorFieldAddress);
        }

        public bool SendError(ErrorCode error)
        {
            try
            {
                // new connection to PLC
                PlcTcpMaster conn = new PlcTcpMaster(ipAddress);
                // Product/Hopper mismatch error bit = bit0 (1).
                if (error == ErrorCode.ProductHopperMismatch)
                { conn.writeInt(1, errorFieldAddress); }
                // Invalid Request error bit = bit1 (2).
                else if (error == ErrorCode.InvalidRequest)
                { conn.writeInt(2, errorFieldAddress); }
            }
            catch
            { return false; }

            return true;
        }

        public float GetNumOfHoppers()
        {
            PlcTcpMaster plcConnection = new PlcTcpMaster(ipAddress);
            float num = plcConnection.readFloat(numberOfHoppersFieldAddress);
            // num = checkCase(num);
            return num;
        }

        public ushort GetImpregs()
        {
            PlcTcpMaster plcConnection = new PlcTcpMaster(ipAddress);
            ushort num = plcConnection.readInt16(impregFieldAddress1);
            return num;
        }

        // checks if a specific bit is set.
        private bool IsBitSet(ushort _numberToCheck, ushort _positionOfBit)
        { return (_numberToCheck & (1 << _positionOfBit)) != 0; }
    }
}
using PlcComms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoftwareInteraction
{
    public class Software
    {
        public delegate void MessageHandler(string message, bool isError);
        private MessageHandler listOfHanders;
        private PlcInteraction plc;
        private CommStatus status;
        private RequestType requestType;
        private bool isError;
        private RecipeInteraction interact;
        private static Software software;

        /// <summary>
        /// The constructor is set as private.
        /// The private modifier helps ensure that only a single instance of the class is created.
        /// This is called from the Create() method.
        /// </summary>
        private Software()
        {
            plc = new PlcInteraction();
            interact = new RecipeInteraction();
            isError = false;
        }

        public static Software Create()
        {
            if (software == null)
            { return software = new Software(); }
            return software;
        }

        public void CheckPlc(string ipAddress)
        {
            try
            {
                // Create a new PLC messaging object
                plc = new PlcInteraction(ipAddress);
                // Get the status field
                status = plc.CheckStatusField();

                if (status == CommStatus.Null)
                {
                    plc.ClearStatus();
                    return;
                }
                else if (status == CommStatus.Request)
                {
                    plc.InProgressBit = true;
                    checkRequestType();
                    requestProcedure();
                }
                else if (status == CommStatus.Finished)
                {
                    checkRequestType();
                    storeProcedure();
                }
                else if (status == CommStatus.Downloaded)
                {
                    //software only loops once
                }
            }
            catch (ArgumentException ex)
            {
                // write to log file
                if (listOfHanders != null)
                { listOfHanders(ex.Message, true); }
            }
            catch (Exception ex)
            {
                // write to log file
                if (listOfHanders != null)
                { listOfHanders("Error: " + ex.Message, true); }
            }
        }

        private void requestProcedure()
        {
            isError = false;
            interact.Plc = plc;
            if (interact.verifyRequest())
            {
                interact.processRequest();
                interact.sendData();
            }
            else
            { isError = true; }
            setUpdatedBit();
        }

        private void storeProcedure()
        {
            interact.Plc = plc;
            if (interact.verifyProcess())
            {
                interact.storeData();
                setDownloadedBit();
            }
            else
            { plc.ticketDNE(); }
        }
        
        private void setDownloadedBit()
        {
            if (isError == false)
            { plc.SetDownloadedBit(); }
        }

        private void checkRequestType()
        {requestType = plc.CheckRequestType();}
        
        private void setUpdatedBit()
        {
            if (isError == false)
            { plc.SetValuesUpdatedField(); }
        }
        
        #region MessageHandling
        public void RegisterWithMessageHandler(MessageHandler methodToCall)
        { listOfHanders += methodToCall; }

        public void UnRegisterWithMessageHandler(MessageHandler methodToCall)
        { listOfHanders -= methodToCall; }
        #endregion MessageHandling
    }
}

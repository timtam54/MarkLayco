//  PlcComms.dll
//  Version 1.0
//  Last modified on 8/18/2011
//  Created 6/8/2011 by Tim Morgan
//  Yargus Manufacturing, Inc
//  12285 E. Main Street
//  Marshall, IL 62441

//The PlcTcpComms class allows communication between a PC and a PLC over
//Modbus TCP/IP connection. It is based off of the NModbus drivers that can
//be found at http://code.google.com/p/nmodbus/.

//Capability to Read from PLC devices to PC:
//-Integer values into Int32 and UInt16
//-Text strings into string
//-Real numbers into float values (32bit)

//Capability to Write to PLC devices from PC:
//-Integer values (unsigned 16bit) from Int32 and UInt16
//-Text strings from string
//-Real numbers from float values (32bit)

//Creating a PlcTcpComms object.
//    A few constructors are provided to allow ease of integration of this
//    class into a main program. To create a PlcTcpComms object, only
//    passing a parameter for the IP address of the PLC device is needed.
//    The option to pass the PLC's memory address (as an Int or a ushort)
//    to the constructor is also available. The default format for the
//    memory address is in decimal form, but if necessary, a third boolean
//    parameter can be passed to the constructor to indicate Octal format.

//Reading values from the PLC.
//    This class provides two methods to read integer values from the PLC.
//    The first method returns and Int32 and the second method returns
//    a ushort (UInt16). Both of these methods provide several overload
//    options to allow flexibility. The possibility to pass memory locations
//    in Int32 or ushort format is also available.
//    This class also provides a method that returns text in string format
//    and a method that returns a real number in a float data type. Both
//    of these methods have several overload possibilites as well to allow
//    flexibility for the main program.

//Writing values to the PLC.
//    It is possible to write integer values into a single memory location,
//    strings of text characters into continuous memory locations (two
//    characters in each memory location), and to write 32bit floating
//    point values (real numbers) into two continuous memory locations.

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Web;
using System.Text;
using Modbus.Data;
using Modbus.Device;
using Modbus.Utility;

namespace PlcComms
{
    // Use when the PC is acting as the modbus master.
    namespace Master
    {
        /// <summary>
        /// Used to establish a connection to a modbus device
        /// over TCP/IP.
        /// </summary>
        public class PlcTcpMaster
        {
            #region Fields
            // IP Address of PLC.
            public string plcAddress { get; private set; }
            // Memory address of PLC to read/write. This is in decimal
            //  format. If the memory address is inputted as octal, it
            //  is converted before being stored in this value.
            public ushort memAddress { get; private set; }
            // Port number for modbus. 
            public int modBusPortNumber { get; private set; }
            // Default modbus port number. Standard is port 502.
            const int DEFAULT_MODBUS_PORT_NUMBER = 502;
            // Timeout period (in msec) for TCP connection test. If
            //  the program doesn't establish a connection to the
            //  PLC's ethernet port within this many miliseconds,
            //  a timeout error will occur.
            public int tcpTimeOutPeriod { get; private set; }
            // Default tcp time out period (in msec).
            //  1/4 of a second.
            const int DEFAULT_TCP_TIMEOUT = 250;
            #endregion Fields

            #region Properties
            /// <summary>
            /// IP Address of the PLC device.
            /// </summary>
            /// <exception cref="ArgumentNullException"></exception>
            /// <exception cref="ArgumentException"></exception>
            public string IpAddress
            {
                get { return plcAddress; }
                set { if (verifyIpAddress(value)) plcAddress = value; }
            }

            public int ModbusPort
            {
                get { return modBusPortNumber; }
                set { if (verifyPortNumber(value))  modBusPortNumber = value; }
            }

            /// <summary>
            /// The timeout period (in msec) when connecting to a PLC device.
            /// </summary>
            /// <exception cref="ArgumentOutOfRangeException"></exception>
            public int Timeout
            {
                get { return tcpTimeOutPeriod; }
                set { if (verifyTcpTimeOutPeriod(value)) tcpTimeOutPeriod = value; }
            }
            #endregion Properties

            #region Constructors
            /// <summary>
            /// Capable of establishing a TCP/IP connection to a modbus device.
            /// </summary>
            /// <param name="_plcAddress">IP address of the remote modbus device.</param>
            public PlcTcpMaster(string _plcAddress)
            {
                // Checks for empty PLC IP Address.
                if (string.IsNullOrWhiteSpace(_plcAddress))
                {
                    // Error message for an empty IP Address.
                    string exceptionErrorMessage = "IP address for PLC device cannot be empty.";
                    throw new ArgumentNullException(exceptionErrorMessage);
                }
                else
                { plcAddress = _plcAddress; }
                // Sets the modbus port number to the default 502;
                modBusPortNumber = DEFAULT_MODBUS_PORT_NUMBER;
                // Sets the default TCP timeout period for connection tests.
                tcpTimeOutPeriod = DEFAULT_TCP_TIMEOUT;
                // Sets the Memory Address to 0 which will require
                //  the main program to set the memory address or to
                //  use the methods which accept a memory address.
                memAddress = 0;
            }
            #endregion Constructors

            #region Read Integers
            /// <summary>
            /// Read an integer value from a PLC using modbus TCP/IP.
            /// </summary>
            /// <remarks>
            /// readInt() is called to read an integer (whole number)
            /// from the PLC. It creates a TCP connection to the PLC then
            /// returns an integer from the memory location value already
            /// set.
            /// NOTE: This method reads from a memory location already established
            /// previously in the code. If the memory location needs to be
            /// specified, another overloaded readInt() method should be used.
            /// </remarks>
            /// <returns></returns>
            /// <exception cref="TimeoutException">
            /// Thrown if a connection to the PLC can't be established within the allotted time.
            /// </exception>
            private int readInt()
            {
                // Since it is reading an Integer, only one memory location is read.
                ushort numOfPoints = 1;
                ushort[] readData = new ushort[numOfPoints];
                // The readData will be converted to int and stored in dataValue.
                int dataValue;
                // Connection objects that will be created.
                TcpClient client = null;
                ModbusIpMaster master = null;

                try
                {
                    // Check to make sure that all values are valid before anything
                    //  operations are performed.
                    readWriteEnabled();
                    // Creates a TCP connection to the PLC.
                    client = new TcpClient(plcAddress, modBusPortNumber);
                    // Creates the modbus connection to the PLC.
                    master = ModbusIpMaster.CreateIp(client);
                    // Read the data from the device and store it into readData. Since only
                    //  one memory location is read, the readData array is length of 1.
                    readData = master.ReadHoldingRegisters(memAddress, numOfPoints);
                    // Convert the 16bit value that was read from the device into a 
                    //  normal 32bit Integer.
                    dataValue = Convert.ToInt32(readData[0]);
                    // Returns the read data to the main program.
                    return dataValue;
                }
                catch (SocketException)
                { throw new TimeoutException("Was not able to open a TCP connection to the PLC within the allotted amount of time."); }
                finally
                {
                    // Closes the TCP connection.
                    if (client != null)
                    { client.Close(); }
                }
            }

            /// <summary>
            /// Read an integer value from a PLC using modbus TCP/IP.
            /// </summary>
            /// <remarks>
            /// readInt16() is called to read an integer (whole number)
            ///  from the PLC. It creates a TCP connection to the PLC then 
            ///  returns a ushort, UInt16, from the memory location.
            /// NOTE: This method reads from a memory location already established
            ///  previously in the code. If the memory location needs to be 
            ///  specified, another overloaded readInt16() method should be used.
            /// </remarks>
            /// <returns></returns>
            /// <exception cref="TimeoutException">
            /// Thrown if a connection to the PLC can't be established within the allotted time.
            /// </exception>
            private ushort readInt16()
            {
                // Since it is reading an Integer, only one memory location is read.
                ushort numOfPoints = 1;
                ushort[] readData = new ushort[numOfPoints];
                // Connection objects.
                TcpClient client = null;
                ModbusIpMaster master = null;

                try
                {
                    // Check to make sure that all values are valid before anything
                    //  operations are performed.
                    readWriteEnabled();
                    // Creates a TCP connection to the PLC.
                    client = new TcpClient(plcAddress, modBusPortNumber);
                    // Creates the modbus connection to the PLC.
                    master = ModbusIpMaster.CreateIp(client);
                    // Read the data from the device and store it into readData. Since only
                    //  one memory location is read, the readData array is length of 1.
                    readData = master.ReadHoldingRegisters(memAddress, numOfPoints);
                    // Returns the read data to the main program.
                    return readData[0];
                }
                catch (SocketException)
                { throw new TimeoutException("Was not able to open a TCP connection to the PLC within the allotted amount of time."); }
                finally
                {
                    // Closes the TCP connection.
                    if (client != null)
                    { client.Close(); }
                }
            }

            /// <summary>
            /// Read an integer value from a PLC using modbus TCP/IP.
            /// </summary>
            /// <remarks>
            /// readInt() is called to read an integer (whole number)
            /// from the PLC. It creates a TCP connection to the PLC then
            /// returns an integer from the memory location value already
            /// set.
            /// </remarks>
            /// <returns></returns>
            /// <exception cref="TimeoutException">
            /// Thrown if a connection to the PLC can't be established within the allotted time.
            /// </exception>
            public int readInt(int _memLocation, bool _isOctal = false)
            {
                setMemoryAddress(_memLocation, _isOctal);
                // Reads the memory location into dataValue.
                int dataValue = readInt();
                // Returns the read data to the main program.
                return dataValue;
            }

            /// <summary>
            /// Read an integer value from a PLC using modbus TCP/IP.
            /// </summary>
            /// <remarks>
            /// readInt() is called to read an integer (whole number)
            /// from the PLC. It creates a TCP connection to the PLC then
            /// returns an integer from the memory location value already
            /// set.
            /// </remarks>
            /// <returns></returns>
            /// <exception cref="TimeoutException">
            /// Thrown if a connection to the PLC can't be established within the allotted time.
            /// </exception>
            public int readInt(ushort _memLocation, bool _isOctal = false)
            {
                setMemoryAddress(_memLocation, _isOctal);
                // Reads the memory location into dataValue.
                int dataValue = readInt();
                // Returns the read data to the main program.
                return dataValue;
            }

            /// <summary>
            /// Read an integer value from a PLC using modbus TCP/IP.
            /// </summary>
            /// <remarks>
            /// readInt() is called to read an integer (whole number)
            /// from the PLC. It creates a TCP connection to the PLC then
            /// returns an integer from the memory location value already
            /// set.
            /// </remarks>
            /// <returns></returns>
            /// <exception cref="TimeoutException">
            /// Thrown if a connection to the PLC can't be established within the allotted time.
            /// </exception>
            public ushort readInt16(int _memLocation, bool _isOctal = false)
            {
                setMemoryAddress(_memLocation, _isOctal);
                ushort readData = readInt16();
                // Returns the read data to the main program.
                return readData;
            }

            /// <summary>
            /// Read an integer value from a PLC using modbus TCP/IP.
            /// </summary>
            /// <remarks>
            /// readInt() is called to read an integer (whole number)
            /// from the PLC. It creates a TCP connection to the PLC then
            /// returns an integer from the memory location value already
            /// set.
            /// </remarks>
            /// <returns></returns>
            /// <exception cref="TimeoutException">
            /// Thrown if a connection to the PLC can't be established within the allotted time.
            /// </exception>
            public ushort readInt16(ushort _memLocation, bool _isOctal = false)
            {
                setMemoryAddress(_memLocation, _isOctal);

                ushort readData = readInt16();
                // Returns the read data to the main program.
                return readData;
            }
            #endregion Read Integers

            #region Read Text
            /// <summary>
            /// Read a string of characters from a PLC device using modbus TCP/IP.
            /// </summary>
            /// <remarks>
            /// readText() is called to read a string (text characters)
            ///  from the PLC. It establishes a TCP connection to the PLC and
            ///  then reads a specified number of characters from the device. 
            ///  These characters are then combined into a string and then returned
            ///  to the main program.
            /// </remarks>
            /// <param name="_numChars"></param>
            /// <returns></returns>
            /// <exception cref="ArgumentOutOfRangeException">
            /// This exception is thrown when _numChars is less than 1 or greater than the max UInt16 value.
            /// </exception>
            /// <exception cref="TimeoutException">
            /// Thrown if a connection to the PLC can't be established within the allotted time.
            /// </exception>
            private string readText(int _numChars)
            {
                // Check to make sure that all values are valid before anything
                //  operations are performed.
                if (verifyNumCharacters(_numChars))
                {
                    if (readWriteEnabled())
                    {
                        // The number of characters to be read from the device.
                        int numChars = _numChars;
                        // Each memory location in the PLC can hold two characters so
                        //  the number of memory locations needed to be read is the
                        //  number of characters divided by two. Then that value is 
                        //  placed into the 16bit variable numOfPoints.
                        // If the number of characters is odd, a 1 is added to it so
                        //  all memory locations can be read.
                        if (numChars % 2 != 0)
                        {
                            // number of characters is odd, so add 1 to it.
                            numChars++;
                        }
                        int numMemoryLocations = numChars / 2;
                        ushort numOfPoints = Convert.ToUInt16(numMemoryLocations);
                        // Connection objects.
                        TcpClient client = null;
                        ModbusIpMaster master = null;

                        try
                        {
                            // Creates a TCP connection to the PLC.
                            client = new TcpClient(plcAddress, modBusPortNumber);
                            // Creates the modbus connection to the PLC.
                            master = ModbusIpMaster.CreateIp(client);

                            // Read data from the PLC starting at memAddress and reads the
                            //  number of memory locations equal to numOfPoints. Each memory location
                            //  is stored in the array readData[] as 16bit values (each 16bit value
                            //  contains two characters).
                            ushort[] readData = master.ReadHoldingRegisters(memAddress, numOfPoints);

                            // Each 16bit readData[] value holds two characters. This next section
                            //  splits each readData[] value and converts it into an ASCII
                            //  character. Because of the format that the PLC stores in text, the
                            //  two characters need to be swapped around. For example, if the values
                            //  in one memory location is 'VA', the PLC stores it as 'AV'.
                            //  After the foreach loop is completed, the full string is stored
                            //  in fullText and returned to the main program.
                            string fullText = "";
                            foreach (ushort value in readData)
                            {
                                // Split one ushort value into two bytes.
                                byte[] byteText = BitConverter.GetBytes(value);
                                // The first character is the second byte.
                                string firstCharacter = Encoding.ASCII.GetString(byteText, 1, 1);
                                // The second character is the first byte.
                                string secondCharacter = Encoding.ASCII.GetString(byteText, 0, 1);
                                // Append both characters to the full text.
                                fullText += firstCharacter + secondCharacter;
                            }
                            return fullText;
                        }
                        catch (SocketException)
                        {
                            throw new TimeoutException("Was not able to open a TCP connection to the PLC within the allotted amount of time.");
                        }
                        finally
                        {
                            // Close the TCP connection.
                            if (client != null)
                            { client.Close(); }
                        }
                    }
                }
                // verifyNumCharacters(_numChars) returned false. number of characters invalid
                string exceptionErrorMessage = "Number of characters for text string is outside of the capable range. Value: " + _numChars;
                throw new ArgumentOutOfRangeException(exceptionErrorMessage);
            }

            /// <summary>
            /// Read a string of characters from a PLC device using modbus TCP/IP.
            /// </summary>
            /// <remarks>
            /// readText() is called to read a string (text characters)
            ///  from the PLC. It establishes a TCP connection to the PLC and
            ///  then reads a specified number of characters from the device. 
            ///  These characters are then combined into a string and then returned
            ///  to the main program. 
            /// </remarks>
            /// <param name="_memLocation"></param>
            /// <param name="_numChars"></param>
            /// <param name="_isOctal"></param>
            /// <returns></returns>
            public string readText(int _memLocation, int _numChars, bool _isOctal = false)
            {
                setMemoryAddress(_memLocation, _isOctal);
                verifyNumCharacters(_numChars);
                string readData = readText(_numChars);
                return readData;
            }

            /// <summary>
            /// Read a string of characters from a PLC device using modbus TCP/IP.
            /// </summary>
            /// <remarks>
            /// readText() is called to read a string (text characters)
            ///  from the PLC. It establishes a TCP connection to the PLC and
            ///  then reads a specified number of characters from the device. 
            ///  These characters are then combined into a string and then returned
            ///  to the main program. 
            /// </remarks>
            /// <param name="_memLocation"></param>
            /// <param name="_numChars"></param>
            /// <param name="_isOctal"></param>
            /// <returns></returns>
            public string readText(ushort _memLocation, int _numChars, bool _isOctal = false)
            {
                setMemoryAddress(_memLocation, _isOctal);
                verifyNumCharacters(_numChars);
                string readData = readText(_numChars);
                return readData;
            }
            #endregion Read Text

            #region Read Float
            // readFloat method is called to read a Real number (Floating point
            //  number) from the PLC. It establishes a TCP connection to the PLC
            //  and then reads two memory locations (a float requires two 16bit
            //  locations. The real (float) number is then returned to the main program.
            private float readFloat()
            {
                // Check to make sure that all values are valid before anything
                //  operations are performed.
                readWriteEnabled();
                // Creates a TCP connection to the PLC.
                TcpClient client = new TcpClient(plcAddress, modBusPortNumber);
                // Creates the modbus connection to the PLC.
                ModbusIpMaster master = ModbusIpMaster.CreateIp(client);

                // Floating points require 2 memory locations (4 bytes) so
                //  we need to read two memory locations.
                ushort numOfPoints = 2;
                // Read two memory locations from the PLC. It then stores it into
                //  the readData array. Each value in the array is a 16bit integer.
                //  The array is two values long.
                ushort[] readData = master.ReadHoldingRegisters(memAddress, numOfPoints);
                // Close the TCP connection.
                client.Close();

                // Two arrays of byte's are needed for the next part.
                // BitConverter.GetBytes takes the first 16bit readData value (which
                //  is the first half of the 32bit float value), converts it into 
                //  two bytes and places them into byteText[0-1]. byteText[0-1] is
                //  then placed into the first two positions of byteTextFull[].
                // BitConverter.GetBytes is performed again, but on the second value
                //  of readData. This is converted into two bytes and placed into
                //  byteText[0-1] which is then placed into the third and fourth
                //  locations of byteTextFull.
                byte[] byteTextFull = new byte[4];
                byte[] byteText = new byte[2];
                // Get the first 16 bits of the floating point number and place into 
                //  the first half of byteTextFull.
                byteText = BitConverter.GetBytes(readData[0]);
                byteTextFull[0] = byteText[0];
                byteTextFull[1] = byteText[1];
                // Get the second 16 bits of the floating point number and place into 
                //  the second half of byteTextFull.
                byteText = BitConverter.GetBytes(readData[1]);
                byteTextFull[2] = byteText[0];
                byteTextFull[3] = byteText[1];

                // Convert the four bytes of byteTextFull to a 32 bit floating point 
                //  value and return to the main program.
                float floatValue;
                floatValue = BitConverter.ToSingle(byteTextFull, 0);
                return floatValue;
            }

            public float readFloat(int _memLocation, bool _isOctal = false)
            {
                setMemoryAddress(_memLocation, _isOctal);
                float readData = readFloat();
                return readData;
            }

            public float readFloat(ushort _memLocation, bool _isOctal = false)
            {
                setMemoryAddress(_memLocation, _isOctal);
                float readData = readFloat();
                return readData;
            }
            #endregion Read Float

            #region Write Integers
            // writeInt is a method that can write an integer number to the
            //  PLC. It is recommended to call the readInt() method directly
            //  after to verify that the data was written correctly.
            // NOTE: writeInt is an overloaded method. One accepts an Int 
            //  value and one accepts a ushort value.
            private void writeInt(int _writeData)
            {
                // Check to make sure that all values are valid before anything
                //  operations are performed.
                readWriteEnabled();
                // Check to make sure that the integer value is small enough to
                //  be converted to a 16bit unsigned digit.
                if ((_writeData >= 0) && (_writeData <= UInt16.MaxValue))
                {
                    // Creates a TCP connection to the PLC.
                    TcpClient client = new TcpClient(plcAddress, modBusPortNumber);
                    // Creates the modbus connection to the PLC.
                    ModbusIpMaster master = ModbusIpMaster.CreateIp(client);
                    // Convert the input integer (_writeData) to a 16bit ushort
                    //  value and then write to the PLC.
                    ushort writeData = Convert.ToUInt16(_writeData);
                    master.WriteSingleRegister(memAddress, writeData);
                    // Close the TCP connection.
                    client.Close();
                }
                else
                {
                    string exceptionErrorMessage = "Integer value is too large to write to PLC's 16bit memory location. Value: " + _writeData;
                    throw new ArgumentOutOfRangeException(exceptionErrorMessage);
                }
            }

            // writeInt is a method that can write an integer number to the
            //  PLC. It is recommended to call the readInt() method directly
            //  after to verify that the data was written correctly.
            // NOTE: writeInt is an overloaded method. One accepts an Int 
            //  value and one accepts a ushort value.
            private void writeInt(ushort _writeData)
            {
                // Check to make sure that all values are valid before anything
                //  operations are performed.
                readWriteEnabled();
                // Creates a TCP connection to the PLC.
                TcpClient client = new TcpClient(plcAddress, modBusPortNumber);
                // Creates the modbus connection to the PLC.
                ModbusIpMaster master = ModbusIpMaster.CreateIp(client);
                // Write the value to the PLC memory location.
                master.WriteSingleRegister(memAddress, _writeData);
                // Close the TCP connection.
                client.Close();
            }

            public void writeInt(int _writeData, int _memLocation, bool _isOctal = false)
            {
                setMemoryAddress(_memLocation, _isOctal);
                writeInt(_writeData);
            }

            public void writeInt(int _writeData, ushort _memLocation, bool _isOctal = false)
            {
                setMemoryAddress(_memLocation, _isOctal);
                writeInt(_writeData);
            }

            public void writeInt(ushort _writeData, int _memLocation, bool _isOctal = false)
            {
                setMemoryAddress(_memLocation, _isOctal);
                writeInt(_writeData);
            }

            public void writeInt(ushort _writeData, ushort _memLocation, bool _isOctal = false)
            {
                setMemoryAddress(_memLocation, _isOctal);
                writeInt(_writeData);
            }
            #endregion Write Integers

            #region Write Text
            // writeString is a method that can write a string of a specified
            //  number of characters to the PLC. The string and the number of
            //  characters is required for this method. The number of characters
            //  does not have to be the same as the writeString, but if it shorter
            //  all of the text will not be written.
            // Note: Each memory location in the PLC can hold two characters.
            private void writeText(string _writeString, int _numChars)
            {
                // Check to make sure that all values are valid before anything
                //  operations are performed.
                readWriteEnabled();
                verifyNumCharacters(_numChars);
                // The string that will be written to the device.
                string writeString = _writeString;
                // The number of characters to be written. Can be different
                //  from the number of characters in writeString.
                int numChars = _numChars;

                // Creates a TCP connection to the PLC.
                TcpClient client = new TcpClient(plcAddress, modBusPortNumber);
                // Creates the modbus connection to the PLC.
                ModbusIpMaster master = ModbusIpMaster.CreateIp(client);

                // Split the string into an array of bytes.
                byte[] byteText = Encoding.ASCII.GetBytes(writeString);
                // Create an empty array of ushorts (16bit values) with a length
                //  of the total number of characters to be written. If the number
                //  of characters is odd, 1 must be added to it since each memory
                //  location holds two characters.
                if (numChars % 2 != 0)
                {
                    // number of characters is odd, so add 1 to it.
                    numChars++;
                }
                ushort[] writeData = new ushort[Convert.ToUInt16(numChars / 2)];
                // Check to see if the number of characters are even or odd.
                //  If it is odd, it adds an extra value blank value to the
                //  end of the array. If this isn't done, the last character
                //  won't be read if it contains an odd number of characters.
                // Each memory location in the PLC can hold two text characters.
                //  Because of this, the number of data to be read is half of
                //  the total number of characters.
                int numOfData = byteText.GetLength(0);
                if (numOfData % 2 != 0)
                {
                    // Odd number of characters. Make the array length even.
                    Array.Resize<byte>(ref byteText, numOfData + 1);
                    numOfData = (numOfData / 2) + 1;
                }
                else
                {
                    // Even number of characters.
                    numOfData = numOfData / 2;
                }

                // The next section of code loops through each value in the
                //  byteText array. Because of the way the PLC stores text data,
                //  each pair of characters needs to be inverted. For example, if
                //  you want to write 'VA' to the PLC, the PLC needs to store it
                //  as 'AV'. The invertByteTemp value is used to perform this 
                //  inverting action. All of the characters are stored into the
                //  writeData array as 16bit ushorts.
                for (int i = 0; i < numOfData; i++)
                {
                    // Invert pairs of characters.
                    byte invertByteTemp = byteText[(i * 2)];
                    byteText[(i * 2)] = byteText[(i * 2) + 1];
                    byteText[(i * 2) + 1] = invertByteTemp;
                    // Write two bytes into one element of the writeData array.
                    writeData[(i)] = BitConverter.ToUInt16(byteText, (i * 2));
                }
                // Write the full string to the PLC.
                master.WriteMultipleRegisters(Convert.ToUInt16(memAddress), writeData);
                // Close the TCP connection.
                client.Close();
            }

            public void writeText(string _writeString, int _numChars, int _memLocation, bool _isOctal = false)
            {
                setMemoryAddress(_memLocation, _isOctal);
                verifyNumCharacters(_numChars);
                writeText(_writeString, _numChars);
            }

            public void writeText(string _writeString, int _numChars, ushort _memLocation, bool _isOctal = false)
            {
                setMemoryAddress(_memLocation, _isOctal);
                verifyNumCharacters(_numChars);
                writeText(_writeString, _numChars);
            }
            #endregion Write Text

            #region Write Float
            // writeFloat is a method that can write a float number (real number)
            //  to the PLC. It is recommended to call the readFloat method after
            //  writing to the device to verify a successful write.
            // Note: Real numbers (Floats) take up two memory locations in the PLC.
            private void writeFloat(float _writeFloat)
            {
                // Check to make sure that all values are valid before anything
                //  operations are performed.
                readWriteEnabled();
                // The number that will be written to the device.
                float writeFloat = _writeFloat;
                // Convert the float (32bit) into an array of four bytes.
                byte[] byteData = BitConverter.GetBytes(writeFloat);
                // Take the first two bytes and combine them into the first
                //  16bit ushort location of the writeData array. Then take
                //  the last two bytes and put them into the second ushort
                //  location of writeData.
                ushort[] writeData = new ushort[2];
                writeData[0] = BitConverter.ToUInt16(byteData, 0);
                writeData[1] = BitConverter.ToUInt16(byteData, 2);

                // Creates a TCP connection to the PLC.
                TcpClient client = new TcpClient(plcAddress, modBusPortNumber);
                // Creates the modbus connection to the PLC.
                ModbusIpMaster master = ModbusIpMaster.CreateIp(client);
                // Write the data to the PLC.
                master.WriteMultipleRegisters(Convert.ToUInt16(memAddress), writeData);
                // Close the TCP connection.
                client.Close();
            }

            public void writeFloat(float _writeFloat, int _memLocation, bool _isOctal = false)
            {
                setMemoryAddress(_memLocation, _isOctal);
                writeFloat(_writeFloat);
            }

            public void writeFloat(float _writeFloat, ushort _memLocation, bool _isOctal = false)
            {
                setMemoryAddress(_memLocation, _isOctal);
                writeFloat(_writeFloat);
            }
            #endregion Write Float

            #region Setters
            // setMemoryAddress() is a method that allows the main program to
            //  modify the PLC memory location directly, after a new instance 
            //  of the class has  already been created. This method checks to make 
            //  sure that the memory address is valid and returns true if the method
            //  is successful at modifiying the address, otherwise it returns
            //  false.
            // NOTE: This method has two overloads, one that accepts an integer
            //  and one that accepts a ushort.
            private bool setMemoryAddress(int _memAddress, bool _isOctal = false)
            {
                if (verifyMemoryAddress(_memAddress, _isOctal))
                {
                    if (_isOctal == false)
                    {
                        // Convert input from 32bit signed number to a 16bit unsigned
                        //  number.
                        memAddress = Convert.ToUInt16(_memAddress);
                        return true;
                    }
                    else // if _isOctal == true
                    {
                        // memory address input is in Octal format. Must 
                        //  convert it to decimal format so other methods can use it.
                        memAddress = Convert.ToUInt16(Convert.ToString(_memAddress), 8);
                        return true;
                    }
                }
                else
                { return false; }
            }

            // setMemoryAddress() is a method that allows the main program to
            //  modify the PLC memory location directly, after a new instance 
            //  of the class has  already been created. This method checks to make 
            //  sure that the memory address is valid and returns true if the method
            //  is successful at modifiying the address, otherwise it returns
            //  false.
            // NOTE: This method has two overloads, one that accepts an integer
            //  and one that accepts a ushort.
            private bool setMemoryAddress(ushort _memAddress, bool _isOctal = false)
            {
                if (verifyMemoryAddress(_memAddress, _isOctal))
                {
                    if (_isOctal == false)
                    {
                        // Memory Address is already in decimal formate.
                        memAddress = _memAddress;
                        return true;
                    }
                    else // if _isOctal == true
                    {
                        // memory address input is in Octal format. Must 
                        //  convert it to decimal format so other methods can use it.
                        memAddress = Convert.ToUInt16(Convert.ToString(_memAddress), 8);
                        return true;
                    }
                }
                else
                { return false; }
            }
            #endregion Setters

            #region Error Checking
            // readWriteEnabled() is an error checking method that
            //  verifies that all values are valid before allowing
            //  any read/write functions.
            private bool readWriteEnabled()
            {
                // Verifies that the PLC IP Address is valid.
                verifyIpAddress(plcAddress);
                // Verifies that the Memory Address is valid.
                verifyMemoryAddress(memAddress);
                // Verifies that the port number is valid.
                verifyPortNumber(modBusPortNumber);
                // Verifies that a connection can be established 
                //  to the IP Address of the PLC.
                verifyConnection(plcAddress, modBusPortNumber, tcpTimeOutPeriod);

                return true;
            }

            // verifyIpAddress() checks for a valid IP Address for the PLC.
            //  It first makes sure that the IP field is not blank (or null)
            //  and then it verifies that it is in an IP format. If these 
            //  checks fail, an exception will be thrown.
            private bool verifyIpAddress(string _ipAddress)
            {
                if (string.IsNullOrWhiteSpace(_ipAddress))
                {
                    // Check for empty IP Address. If it is empty, throw an exception.
                    string exceptionErrorMessage = "IP address for PLC device cannot be empty.";
                    throw new ArgumentNullException(exceptionErrorMessage);
                }
                else
                {
                    // If IP address is not empty, check for a valid format.
                    System.Net.IPAddress result;
                    if (System.Net.IPAddress.TryParse(_ipAddress, out result))
                    {
                        // Valid format.
                        return true;
                    }
                    else
                    {
                        // Not a valid format. Throw an exception.
                        string exceptionErrorMessage = "IP address for PLC device is not in correct format.";
                        throw new ArgumentException(exceptionErrorMessage);
                    }
                }
            }

            // verifyMemoryAddress() checks for a valid Memory Address location.
            // NOTE: This method checks a ushort data type.
            private bool verifyMemoryAddress(ushort _memoryAddress)
            {
                if (_memoryAddress == 0)
                { throw new ArgumentOutOfRangeException("Memory Address cannot be 0."); }
                // Valid.
                return true;
            }

            // verifyMemoryAddress() checks for a valid Memory Address location.
            // NOTE: This method checks for an int data type.
            private bool verifyMemoryAddress(int _memoryAddress, bool _isOctal = false)
            {
                if ((_memoryAddress < 1) || (_memoryAddress > UInt16.MaxValue))
                {
                    string exceptionErrorMessage = "Memory Address is outside of the available range. Value: " + _memoryAddress;
                    throw new ArgumentOutOfRangeException(exceptionErrorMessage);
                }
                // Valid.
                return true;
            }

            // verifyNumCharacters() checks for a valid number of text characters
            //  to read/write. numChars must be a positive number within the limit
            //  of UInt16 (ushort).
            private bool verifyNumCharacters(int _numChars)
            {
                if ((_numChars < 1) || (_numChars > UInt16.MaxValue))
                {
                    // not valid
                    return false;
                }
                // Valid.
                return true;
            }

            // verifyPortNumber() checks for a valid port number. It tests 
            //  for a positive integer value. If it fails, it throws an exception.
            private bool verifyPortNumber(int _portNumber)
            {
                if (_portNumber < 1)
                {
                    // Port numbers cannot have a negative value.
                    string exceptionErrorMessage = "Port number must be a non-negative number. Value: " + _portNumber;
                    throw new ArgumentOutOfRangeException(exceptionErrorMessage);
                }
                return true;
            }

            // verifyConnection() checks to make sure that the PLC's IP address
            //  makes a TCP connection within a specified timeout period.
            private bool verifyConnection(string strIpAddress, int intPort, int nTimeoutMsec)
            {
                bool connectionEstablished = false;
                Socket socket = null;
                try
                {
                    socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontLinger, false);

                    IAsyncResult result = socket.BeginConnect(strIpAddress, intPort, null, null);
                    bool success = result.AsyncWaitHandle.WaitOne(nTimeoutMsec, true);
                    if (socket.Connected == true)
                    { connectionEstablished = true; }
                    else
                    {
                        // Not able to make a connection.
                        string exceptionErrorMessage = "Unable to establish a connection to " + strIpAddress + " within " + nTimeoutMsec + " mSeconds.";
                        throw new TimeoutException(exceptionErrorMessage);
                    }
                }
                catch
                {
                    // Not able to make a connection.
                    string exceptionErrorMessage = "Unable to establish a connection to " + strIpAddress + " within " + nTimeoutMsec + " mSeconds.";
                    throw new TimeoutException(exceptionErrorMessage);
                }
                finally
                {
                    if (null != socket)
                    { socket.Close(); }
                }
                return connectionEstablished;
            }

            private bool verifyTcpTimeOutPeriod(int _timeOutPeriod)
            {
                if (_timeOutPeriod < 1)
                {
                    // Timeout period cannot be less than one.
                    string exceptionErrorMessage = "TCP Timeout period cannot be less than 1. Value: " + _timeOutPeriod;
                    throw new ArgumentOutOfRangeException(exceptionErrorMessage);
                }
                return true;
            }
            #endregion Error Checking
        }

    }

    // Use when the PC is acting as the modbus slave.
    namespace Slave
    {
        // not yet implemented
    }

    // Static classes used for testing.
    namespace Test
    {
        /// <summary>
        /// Provides testing and generical static methods for TCP connections over modbus.
        /// </summary>
        public static class TcpConnection
        {
            /// <summary>
            /// Tests a network connection to a modbus device.
            /// </summary>
            /// <remarks>
            /// Specify the device's ip address, the port and the 
            /// timeout (in msec). This method returns true if the connection
            /// was established within the timeout period. Otherwise it
            /// returns false if the connection was not established.
            /// </remarks>
            /// <param name="strIpAddress">IP address of the remote device.</param>
            /// <param name="intPort">Port number that the modbus is using.</param>
            /// <param name="nTimeoutMsec">Timeout for the connection to be established. In milliseconds.</param>
            /// <returns>
            /// True is returned if a connection to the device was
            /// established within the timeout period. If a connection
            /// was not established, false is returned.
            /// </returns>
            /// <exception cref=""></exception>
            public static bool TestConnection(string strIpAddress, int intPort, int nTimeoutMsec)
            {
                bool connectionEstablished = false;
                Socket socket = null;
                try
                {
                    socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontLinger, false);

                    IAsyncResult result = socket.BeginConnect(strIpAddress, intPort, null, null);
                    bool success = result.AsyncWaitHandle.WaitOne(nTimeoutMsec, true);
                    if (socket.Connected == true)
                    { connectionEstablished = true; }
                    else
                    {
                        // Not able to make a connection.
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    // Not able to make a connection.
                    throw new TimeoutException(ex.Message);
                }
                finally
                {
                    if (null != socket)
                    { socket.Close(); }
                }
                return connectionEstablished;
            }

            /// <summary>
            /// Returns the IPv4 address of the local computer.
            /// </summary>
            /// <returns>
            /// Returns the IP address of the local computer as a string.
            /// </returns> 
            /// <exception cref="System.Net.Sockets.SocketException"></exception>
            public static string GetIpAddress()
            {
                // Get the ip address of the local computer and store it
                //  into the string ipAddr
                IPAddress[] addr = Dns.GetHostAddresses(Dns.GetHostName());
                string ipAddr = "";
                // finds the IPv4 address if there are IPv6 addresses
                foreach (IPAddress value in addr)
                {
                    if (value.AddressFamily == AddressFamily.InterNetwork)
                    {
                        // If value is in the InterNetwork family, then we keep it.
                        ipAddr = Convert.ToString(value);
                        break;
                    }
                }
                return ipAddr;
            }
        }
    }
}

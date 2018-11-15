using System;
using System.IO.Ports;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DAQDevices
{
    public class Arduino : DAQDevice
    {
        private SerialPort port;

        /// <summary>
        /// Initializes a new instance of the arduino DAQ class with the specified parameters
        /// </summary>
        /// <param name="portname">Serial port</param>
        /// <param name="baudrate">Baud Rate defaults to 19200</param>
        public Arduino(string portname, int baudrate = 19200)
        {
            port = new SerialPort(portname, baudrate);
        }

        /// <summary>
        /// Initializes a new instance of the arduino DAQ class and tries to auto-identify the device
        /// </summary>
        /// <param name="baudrate">Baud Rate defaults to 19200</param>
        public Arduino(int baudrate = 19200)
        {
            string[] allports = SerialPort.GetPortNames();
            foreach (string item in allports)
            {
                try
                {
                    port = new SerialPort(item, baudrate);
                    port.Open();
                    string response;
                    response = Task.Run(() => port.ReadExisting()).Result;
                    if(response == "sens")
                    {
                        break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        public override Task<string[]> ReadAllParamsAsync()
        {
            throw new NotImplementedException();
        }

        public override Task<string> ReadParamAsync(string paramName)
        {
            throw new NotImplementedException();
        }

        public override Task SendAsync()
        {
            throw new NotImplementedException();
        }
    }
}

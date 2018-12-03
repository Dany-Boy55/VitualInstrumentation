using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAQDevices
{
    public class ESPWiFi
    {
        // Private fields and objects
        private int port;
        private IPAddress deviceIpAddress;
        private TcpClient tcpClient;
        private CancellationTokenSource cancellation;
        // Events
        public event EventHandler<DAQDataArgs> dataAvaileable;
        // Public Properties
        public int Port { get => port; set => port = value; }
        public IPAddress DeviceIP { get => deviceIpAddress; set => deviceIpAddress = value; }
        public bool IsConnected { get => tcpClient.Connected; }

        // Constructors
        /// <summary>
        /// Initializes a new instance of the ESPWiFi class
        /// </summary>
        /// <param name="port">Communication port</param>
        /// <param name="adquisition">Adquisition mode: continuous async or on demand</param>
        public ESPWiFi(IPAddress deviceip, int Port = 23, bool connect = false)
        {
            deviceIpAddress = deviceip;
            port = Port;
            tcpClient = new TcpClient();
            if (connect)
                StartConnection();
        }
        
        // Public memebers
        /// <summary>
        /// Initializes an empty instance of the class
        /// </summary>
        public ESPWiFi()
        {
            deviceIpAddress = IPAddress.Any;
            port = 23;
            tcpClient = new TcpClient();
        }

        /// <summary>
        /// Begins connection with the device
        /// </summary>
        public void StartConnection()
        {
            if (!IsConnected)
            {                
                tcpClient = new TcpClient();
                // Connecto to the tcp server
                tcpClient.Connect(deviceIpAddress, port);
                // Instantiate a callback to trigger an event when there is data
                EventHandler<DAQDataArgs> handler = dataAvaileable;
                cancellation = new CancellationTokenSource();
                // Fire up the asynchronous data checking task with all its parameters
                CheckData(tcpClient, handler, cancellation.Token);
            }
        }

        /// <summary>
        /// Ends the connection with the device
        /// </summary>
        public void StopConnection()
        {
            if (IsConnected)
            {
                // Cancel the asynchronous data checking task via a token and close the tcp connection
                cancellation.Cancel();
                tcpClient.Close();
                tcpClient = new TcpClient();
            }
        }
        
        /// <summary>
        /// Send data to the device
        /// </summary>
        public void SendData(byte[] buffer)
        {
            if (IsConnected)
            {
                NetworkStream stream = tcpClient.GetStream();
                stream.Write(buffer, 0, buffer.Length);
                stream.Dispose();
            }
            else
            {
                throw new InvalidOperationException("The device is not connected");
            }
        }

        /// <summary>
        /// Send data to the device
        /// </summary>
        public void SendData(string data)
        {
            byte[] b = Encoding.ASCII.GetBytes(data);
            SendData(b);
        }

        /// <summary>
        /// Send data asynchronously to the device
        /// </summary>
        public async Task SendDataAsync(byte[] buffer)
        {
            if (IsConnected)
            {
                NetworkStream stream = tcpClient.GetStream();
                await stream.WriteAsync(buffer, 0, buffer.Length);
            }
            else
            {
                throw new InvalidOperationException("The device is not connected");
            }
        }

        /// <summary>
        /// Send data asynchronously to the device
        /// </summary>
        /// <returns></returns>
        public async Task SendDataAsync(string data)
        {
            byte[] b =Encoding.ASCII.GetBytes(data);
            await SendDataAsync(b);
        }

        // Private memebers
        /// <summary>
        /// constantly checks for data
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        private async Task CheckData(TcpClient client, EventHandler<DAQDataArgs> callback, CancellationToken can)
        {
            // Create the necesary objects once, run the loop until cancelled
            DAQDataArgs args = new DAQDataArgs();
            NetworkStream stream = client.GetStream();
            // Run forever (until cancelled)
            while (true)
            {
                if (can.IsCancellationRequested)
                {
                    Console.WriteLine("Connection Stopped");
                    stream.Dispose();
                    break;
                }
                //Console.WriteLine("Checking for data");
                //Console.WriteLine("buffer {0}", client.Available);
                if (client.Available > 0)
                {
                    int leng = client.Available;
                    byte[] buff = new byte[leng];
                    stream.Read(buff, 0, leng);
                    args.bufferSize = leng;
                    args.data = Encoding.ASCII.GetString(buff);
                    //Console.WriteLine("data: " + args.data);
                    callback(this, args);
                 
                }                
                await Task.Delay(200);
            }
        }
    }

    /// <summary>
    /// EventArgs used when there is new data
    /// </summary>
    public class DAQDataArgs : EventArgs
    {
        public int bufferSize { get; set; }
        public string data { get; set; }
    }
}

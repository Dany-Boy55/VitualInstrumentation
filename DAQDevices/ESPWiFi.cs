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
        private int port;
        private IPAddress deviceIP;
        private TcpClient tcpClient;
        private bool isConnected;
        private CancellationTokenSource cancellation;

        public event EventHandler<DAQDataArgs> dataAvaileable;

        public int UdpPort { get => port; set => port = value; }
        public IPAddress DeviceIP { get => deviceIP; set => deviceIP = value; }
        public bool IsConnected { get => isConnected; }

        /// <summary>
        /// Initializes a new instance of the ESPWiFi class
        /// </summary>
        /// <param name="port">Communication port</param>
        /// <param name="adquisition">Adquisition mode: continuous async or on demand</param>
        public ESPWiFi(IPAddress deviceip, int Port = 23, bool connect = false)
        {
            isConnected = false;
            deviceIP = deviceip;
            port = Port;
            tcpClient = new TcpClient();
            if (connect)
                StartConnection();
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
                tcpClient.ConnectAsync(deviceIP, port);
                // Instantiate a callback to trigger an event when there is data
                EventHandler<DAQDataArgs> h = dataAvaileable;
                cancellation = new CancellationTokenSource();
                // Fire up the asynchronous data checking task
                CheckData(tcpClient, h, cancellation.Token);
                isConnected = true;
            }
        }

        /// <summary>
        /// Ends the connection with the device
        /// </summary>
        public void StopConnection()
        {
            if (IsConnected)
            {
                // Cancel the asynchronous checking task and close the tcp connection
                cancellation.Cancel();
                tcpClient.Close();
                isConnected = false;
            }
        }

        /// <summary>
        /// constantly checks for data
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        private async Task CheckData(TcpClient client, EventHandler<DAQDataArgs> callback, CancellationToken can)
        {
            // Run forever (until cancelled by cancellationtoken)
            while (true)
            {
                if (can.IsCancellationRequested)
                {
                    Console.WriteLine("Connection Stopped");
                    break;
                }
                //Console.WriteLine("Checking for data");
                //Console.WriteLine("buffer {0}", client.Available);
                if (client.Available > 0)
                {
                    DAQDataArgs args = new DAQDataArgs();
                    NetworkStream s = client.GetStream();
                    int leng = client.Available;
                    byte[] buff = new byte[leng];
                    s.Read(buff, 0, leng);
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

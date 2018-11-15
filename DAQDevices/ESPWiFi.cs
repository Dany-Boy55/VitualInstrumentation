using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAQDevices
{
    public class ESPWiFi : DAQDevice
    {
        private int udpPort;
        private EventHandler adquistionModeChanged;
        private IPAddress deviceIP;
        private Socket sender;
        private UdpClient listener;
        private AdquisitionMode mode;

        public int UdpPort { get => udpPort; set => udpPort = value; }
        public IPAddress DeviceIP { get => deviceIP; }
        public AdquisitionMode Mode
        {
            get => mode;
            set
            {
                OnAdquisitionModeChanged();
                mode = value;
            }

        }

        private void OnAdquisitionModeChanged()
        {
            adquistionModeChanged(this, new EventArgs());
        }

        /// <summary>
        /// Initializes a new instance of the ESPWiFi class
        /// </summary>
        /// <param name="port">Communication port</param>
        /// <param name="adquisition">Adquisition mode: continuous async or on demand</param>
        public ESPWiFi(IPAddress deviceip, int port = 1234, AdquisitionMode adquisition = AdquisitionMode.OnDemand)
        {
            deviceIP = deviceip;
            mode = adquisition;
            udpPort = port;
            listener = new UdpClient(udpPort);
        }

        public override Task<string[]> ReadAllParamsAsync()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sends a data request to the remote device
        /// </summary>
        /// <returns></returns>
        private bool SendRequest()
        {
            sender = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            byte[] buffer = { 0x25, 0x25, 0x00 };
            return true;
        }

        public override Task<string> ReadParamAsync(string paramName)
        {
            string value = "";
            listener.ReceiveAsync();
            return Task.Run(() => { return "asdf"; });
        }

        public override Task SendAsync()
        {
            throw new NotImplementedException();
        }

      
    }
}

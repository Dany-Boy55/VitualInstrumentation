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
        private int port;
        private EventHandler adquistionModeChanged;
        private IPAddress deviceIP;
        private TcpClient tcpClient;
        private AdquisitionMode mode;

        public int UdpPort { get => port; set => port = value; }
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
        public ESPWiFi(IPAddress deviceip, int Port = 1234, AdquisitionMode adquisition = AdquisitionMode.OnDemand)
        {
            deviceIP = deviceip;
            mode = adquisition;
            port = Port;
            tcpClient = new TcpClient();
        }

        public override Task<string[]> ReadAllParamsAsync()
        {
            throw new NotImplementedException();
        }
        
        public override Task<string> ReadParamAsync(string paramName)
        {
            string mssg = "";
            return Task.Run(() => { return mssg; });
        }

        public override Task SendAsync()
        {
            throw new NotImplementedException();
        }

      
    }
}

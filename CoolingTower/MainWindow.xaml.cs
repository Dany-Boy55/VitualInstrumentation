using System;
using System.Timers;
using DataPlotter;
using DAQDevices;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CoolingTower
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ESPWiFi adquisitionDevice;
        private static IPAddress defaultIp = new IPAddress(new byte[]{192,168,100,100});
        private double[] variables = new double[8];
        int samples;

        public MainWindow()
        {
            InitializeComponent();
            adquisitionDevice = new ESPWiFi(IPAddress.Parse("192.168.100.2"));
            adquisitionDevice.dataAvaileable += AdquisitionDevice_dataAvaileable;
        }

        /// <summary>
        /// When there is new data from the adquisition device, show it in the UI
        /// </summary>
        private void AdquisitionDevice_dataAvaileable(object sender, DAQDataArgs e)
        {
            statusBar.Foreground = new SolidColorBrush(Colors.LimeGreen);
            statusText.Text = "Conectado: Recibiendo datos";
            string[] data = e.data.Split('\n');
            samples++;
            Console.WriteLine("Read {0} lines", data.Length);
            foreach (string item in data)
            {
                try
                {
                    int varIndex = int.Parse(item.Split(',')[0]);
                    variables[varIndex] = double.Parse(item.Split(',')[1]);
                    Console.WriteLine(item);
                    switch (varIndex)
                    {
                        case 0:
                            temp1graph.AddNewDataPoint(samples, variables[0]);
                            Temp1.Value = variables[0];
                            break;
                        case 1:
                            Temp2.Value = variables[1];
                            temp2graph.AddNewDataPoint(samples, variables[1]);
                            break;
                        case 2:
                            flow1.Value = variables[2];
                            flowgraph.AddNewDataPoint(samples, variables[2]);
                            break;
                        case 3:
                            airTemp1.Value = variables[3];
                            airtemp1graph.AddNewDataPoint(samples, variables[3]);
                            break;
                        case 4:
                            airTemp2.Value = variables[4];
                            airtemp2graph.AddNewDataPoint(samples, variables[4]);
                            break;
                        case 5:
                            airHumi1.Value = variables[5];
                            humi1graph.AddNewDataPoint(samples, variables[5]);
                            break;
                        case 6:
                            airHumi2.Value = variables[6];
                            humi2graph.AddNewDataPoint(samples, variables[6]);
                            break;
                        case 7:
                            press.Value = variables[7];
                            pressgraph.AddNewDataPoint(samples, variables[7]);
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception)
                {
                    
                }
            }
        }
        
        
        /// <summary>
        /// Change the content that the user can interact with
        /// </summary>
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem item = sender as MenuItem;
            switch (item.Header.ToString())
            {
                case "Tiempo Real":
                    if (mainPanel.Visibility == Visibility.Visible)
                        mainPanel.Visibility = Visibility.Collapsed;
                    else
                        mainPanel.Visibility = Visibility.Visible;
                    break;
                case "Histogramas":
                    if (histogramsPanel.Visibility == Visibility.Visible)
                        histogramsPanel.Visibility = Visibility.Collapsed;
                    else
                        histogramsPanel.Visibility = Visibility.Visible;
                    break;
                case "Configuracion":
                    if (configurationPanel.Visibility == Visibility.Visible)
                        configurationPanel.Visibility = Visibility.Collapsed;
                    else
                        configurationPanel.Visibility = Visibility.Visible;
                    break;
                case "Acerca de":
                    if (aboutPanel.Visibility == Visibility.Visible)
                        aboutPanel.Visibility = Visibility.Collapsed;
                    else
                        aboutPanel.Visibility = Visibility.Visible;
                    break;
                default:
                    mainPanel.Visibility = Visibility.Collapsed;
                    histogramsPanel.Visibility = Visibility.Collapsed;
                    configurationPanel.Visibility = Visibility.Collapsed;
                    aboutPanel.Visibility = Visibility.Collapsed;
                    break;
            }
            Console.WriteLine(MasterWindow.Width);
        }

        /// <summary>
        /// Establish a tcp connection to the data adquisition device
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConnectToDevice(object sender, RoutedEventArgs e)
        {
            if (!adquisitionDevice.IsConnected)
            {
                try
                {
                    IPAddress iP = IPAddress.Parse(ipTextbox.Text);
                    adquisitionDevice.DeviceIP = iP;
                    adquisitionDevice.Port = int.Parse(portTextbox.Text);
                    this.Cursor = Cursors.Wait;
                    statusText.Text = "Conectando...";
                    portTextbox.IsEnabled = false;
                    ipTextbox.IsEnabled = false;
                    statusBar.Foreground = new SolidColorBrush(Colors.Yellow);
                    adquisitionDevice.StartConnection();
                    while(!adquisitionDevice.IsConnected){ }
                    this.Cursor = null;
                    statusBar.Foreground = new SolidColorBrush(Colors.DodgerBlue);
                    statusText.Text = "Conectado: Sin Datos";
                    (sender as Button).Content = "Detener Conexion";
                }
                catch (FormatException)
                {
                    ipTextbox.Text = defaultIp.ToString();
                    portTextbox.Text = "23";
                    MessageBox.Show("Direccion IP o puerto TCP no validos");
                }
                catch (InvalidOperationException)
                {
                    MessageBox.Show("Error al abrir la conexion TCP espeficicada");
                }
            }
            else
            {
                portTextbox.IsEnabled = true;
                ipTextbox.IsEnabled = true;
                adquisitionDevice.StopConnection();
                statusBar.Foreground = new SolidColorBrush(Colors.Red);
                statusText.Text = "Desconectado";
                (sender as Button).Content = "Establecer Conexion";
            }
        }

        /// <summary>
        /// Errases all the content in the plots
        /// </summary>
        private void ResetGraphs_Click(object sender, RoutedEventArgs e)
        {
            samples = 0;
            foreach (var item in graphContainer.Children)
            {
                (item as BasicGraph).ClearPlot();
            }
        }
    }
}

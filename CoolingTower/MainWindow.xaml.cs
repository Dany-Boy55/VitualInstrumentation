using System;
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
        private DAQDevice adquisitionDevice;

        public MainWindow()
        {
            InitializeComponent();
            adquisitionDevice = new ESPWiFi(IPAddress.Parse("192.168.0.5"));
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
        }

        private void ConnectToDevice(object sender, RoutedEventArgs e)
        {
            
        }
    }
}

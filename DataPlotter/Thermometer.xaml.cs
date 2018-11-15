using System;
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

namespace DataPlotter
{

    /// <summary>
    /// Interaction logic for Thermometer.xaml
    /// </summary>
    public partial class Thermometer : UserControl
    {
        private string title;
        internal float temperature;
        internal float minTemperature;
        internal float maxTemperature;
        int totalUpdates;

        public float Temperature { get => temperature; set { temperature = value; totalUpdates++; } }
        public string Title { get => title; set => title = value; }

        public Thermometer()
        {
            InitializeComponent();
        }

    }
}

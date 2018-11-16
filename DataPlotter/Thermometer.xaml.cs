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
        private string units;
        internal double temperature;
        internal double minRange;
        internal double maxRange;

        public double Temperature { get => temperature; set { temperature = value; UpdateDrawing(); } }
        public string Title { get => title; set { nameLabel.Text = value; title = value; } }
        public string Units { get => units; set => units = value; }

        public Thermometer()
        {
            InitializeComponent();
            maxRange = 100;
            minRange = 0;
            units = "";
        }

        void UpdateDrawing()
        {
            if (temperature > maxRange)
                maxRange = temperature;
            if (temperature < minRange)
                minRange = temperature;
            double conversionFactor = 130 / (maxRange - minRange);
            ThermoRectangle.Height = conversionFactor * temperature;
            valueLabel.Text = temperature.ToString() + units;
            double percent = ThermoRectangle.Height / 130;
            SolidColorBrush b = new SolidColorBrush(MixColors(Colors.Red, Colors.Cyan, percent));
            ThermoRectangle.Fill = b;
            thermoCircle.Fill = b;
        }

        /// <summary>
        /// Mix 2 colors with a chomatically acurate method
        /// </summary>
        /// <param name="baseColor"></param>
        /// <param name="addedColor"></param>
        /// <param name="ratio"></param>
        /// <returns></returns>
        Color MixColors(Color baseColor, Color addedColor, double ratio)
        {
            byte r, g, b;
            // Computer colors are represented with a sqrt method, so we do the wheighted RMS
            r = (byte)Math.Sqrt(baseColor.R * baseColor.R * ratio + addedColor.R * addedColor.R * (1 - ratio));
            g = (byte)Math.Sqrt(baseColor.G * baseColor.G * ratio + addedColor.G * addedColor.G * (1 - ratio));
            b = (byte)Math.Sqrt(baseColor.B * baseColor.B * ratio + addedColor.B * addedColor.B * (1 - ratio));
            return Color.FromRgb(r, g, b);
        }
    }
}

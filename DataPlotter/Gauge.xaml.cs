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
    /// Interaction logic for Gauge.xaml
    /// </summary>
    public partial class Gauge : UserControl
    {
        private string title;
        internal double pressure;
        internal double minRange;
        internal double maxRange;

        public double Pressure { get => pressure; set { pressure = value; UpdateDrawing(); } }
        public string Title { get => title; set { nameLabel.Text = value; title = value; } }


        public Gauge()
        {
            InitializeComponent();
            minRange = 0;
            maxRange = 1000;
        }

        void UpdateDrawing()
        {
            if (pressure > maxRange)
                maxRange = pressure;
            if (pressure < minRange)
                minRange = pressure;
            double conversionFactor = (3 * Math.PI / 2) / (maxRange - minRange);
            double angle = (5*Math.PI/4) - conversionFactor * pressure;
            valueLabel.Text = pressure.ToString() + " KPa";
            gaugeIndicator.X2 = 100 + 80 * Math.Cos(angle);
            gaugeIndicator.Y2 = 100 - 80 * Math.Sin(angle);
        }

    }
}

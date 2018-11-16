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
        private string units;
        internal double _value;
        private double minRange;
        private double maxRange;

        public double Value { get => _value; set { _value = value; UpdateDrawing(); } }
        public string Title { get => title; set { nameLabel.Text = value; title = value; } }
        public string Units { get => units; set => units = value; }
        internal double MinValue { get => minRange; set => minRange = value; }
        internal double MaxValue { get => maxRange; set => maxRange = value; }

        public Gauge()
        {
            InitializeComponent();
            minRange = 0;
            maxRange = 100;
            units = "";
        }

        void UpdateDrawing()
        {
            if (_value > maxRange)
                maxRange = _value;
            if (_value < minRange)
                minRange = _value;
            double conversionFactor = (3 * Math.PI / 2) / (maxRange - minRange);
            double angle = (5*Math.PI/4) - conversionFactor * _value;
            valueLabel.Text = _value.ToString() + units;
            minLabel.Content = minRange.ToString() + units;
            maxLabel.Content = maxRange.ToString() + units;
            gaugeIndicator.X2 = 100 + 80 * Math.Cos(angle);
            gaugeIndicator.Y2 = 100 - 80 * Math.Sin(angle);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
        private double maxval, minval, avgval;

        public double Value {
            get => _value;
            set { _value = value; UpdateDrawing(); } }
        public string Title { get => title; set { nameLabel.Text = value; title = value; } }
        public string Units { get => units; set => units = value; }
        public double MinValue { get => minRange; set => minRange = value; }
        public double MaxValue { get => maxRange; set => maxRange = value; }

        public Gauge()
        {
            InitializeComponent();
            minRange = 0;
            maxRange = 100;
            minval = double.MaxValue;
            maxval = double.MinValue;
        }

        void UpdateDrawing()
        {
            // update the minimums and maximums in case a new values falls outside current boundries
            if (_value > maxRange)
            {
                maxRange = _value;
                maxRangeLabel.Content = maxRange.ToString() + units;
            }
            if (_value < minRange)
            {
                minRange = _value;
                minRangeLabel.Content = minRange.ToString() + units;
            }
            if (_value > maxval)
            {
                maxval = _value;
                maxLabel.Text = maxval.ToString() + units;
            }
            if (_value < minval)
            {
                minval = _value;
                minLabel.Text = minval.ToString() + units;
            }
            // Update the control drawing programatically (later will be done with bindings and converters)
            avgval = (avgval + _value) / 2;
            avgLabel.Text = avgval.ToString() + units;
            // Perform the calculations for the positions of the drawing elements
            double conversionFactor = (3 * Math.PI / 2) / (maxRange - minRange);
            double angle = (5 * Math.PI / 4) - conversionFactor * _value;
            valueLabel.Text = _value.ToString() + units;
            gaugeIndicator.X2 = 100 + 80 * Math.Cos(angle);
            gaugeIndicator.Y2 = 100 - 80 * Math.Sin(angle);
        }

    }
}

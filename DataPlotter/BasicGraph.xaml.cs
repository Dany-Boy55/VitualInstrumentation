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
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class BasicGraph : UserControl
    {

        private List<double> xValues;
        private List<double> yValues;
        private string xLabel;
        private string yLabel;
        private string title;

        /// <summary>
        /// Initalizes a new empty instance of the control
        /// </summary>
        public BasicGraph()
        {
            InitializeComponent();
            xValues = new List<double>();
            yValues = new List<double>();
            DrawGraph();
        }
        
        public List<double> XValues { get => xValues; set => xValues = value; }
        public List<double> YValues { get => yValues; set => yValues = value; }
        public string XLabel { get => xLabel; set {xLabel = value; xAxisLabel.Text = value; } }
        public string YLabel { get => yLabel; set { yLabel = value; yAxisLabel.Text = value; } }
        public string Title { get => title; set { title = value; titleLabel.Text = value; } }
        public Color foreColor { get; set; }

        /// <summary>
        /// Adds a new data point to the graph 
        /// </summary>
        /// <param name="x">Value of the independant variable</param>
        /// <param name="y">Value of the dependent variable</param>
        public void AddNewDataPoint(double x, double y)
        {
            if (xValues == null)
                xValues = new List<double>();
            if (yValues == null)
                yValues = new List<double>();
            xValues.Add(x);
            yValues.Add(y);
            DrawGraph();
        }

        /// <summary>
        /// Clear the plot
        /// </summary>
        public void ClearPlot()
        {
            Plot.Points.Clear();
            xValues = new List<double>();
            yValues = new List<double>();
        }

        /// <summary>
        /// Performs all the calculations to draw the graph
        /// </summary>
        internal bool DrawGraph()
        {
            // Do no try to render if there are no values
            if (!xValues.Any() || !yValues.Any())
                return false;
            // Width and Height of the drawing area
            // During first render the elements have 0 width and height
            // We use designed width and height instead
            double h, w;
            if (MainCanvas.Width == 0)
                w = 400;
            else
                w = MainCanvas.Width - 40;
            if (MainCanvas.Height == 0)
                h = 200;
            else
                h = MainCanvas.Height - 40;
            // Calculate scaling factors so the given values can fit within the graph area
            double xDrawFactor = (w) / (xValues.Max() - xValues.Min());
            double yDrawFactor = (h) / (yValues.Max() - YValues.Min());
            // Clear the existing plot
            Plot.Points.Clear();
            // Draw all the points in the polyline
            for (int i = 0; i < xValues.Count; i++)
            {
                int tempx = (int)(xValues.ElementAt(i) * xDrawFactor) + 20;
                int tempy = (int)(h + 20 - yValues.ElementAt(i) * yDrawFactor);
                Plot.Points.Add(new Point(tempx, tempy));
            }
            // Success
            return true;
        }
        
        /// <summary>
        /// Triggered when a context menu item is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem item = sender as MenuItem;
        }        
    }

}

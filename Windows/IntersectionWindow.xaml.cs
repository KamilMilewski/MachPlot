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
using System.Windows.Shapes;

namespace MachPlotNamespace
{
    /// <summary>
    /// Interaction logic for IntersectionWindow.xaml
    /// </summary>
    public partial class IntersectionWindow : Window
    {
        public IntersectionWindow(List<SeriesFunPair> r_tempList)
        {
            InitializeComponent();

            intersectionListBox.Items.Clear();

            IDLabelSeries0.Content = r_tempList[0].ID;
            IDLabelSeries1.Content = r_tempList[1].ID;

            intersectionPoints.Clear();
            intersectionPoints = MachVectorIntersection.findIntrsectionAll(r_tempList[0].corelatedFun.dataPointList, r_tempList[1].corelatedFun.dataPointList);
            foreach (Point point in intersectionPoints)
            {
                intersectionListBox.Items.Add(
                    "< " + Math.Round(point.X, 2) + "  ;  " + Math.Round(point.Y, 2) + " >"
                    );
            }
        }

        private List<Point> intersectionPoints = new List<Point>();
    }
}

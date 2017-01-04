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
    /// Interaction logic for additionalRenderWindow.xaml
    /// </summary>
    public partial class additionalRenderWindow : Window
    {
        public additionalRenderWindow(MainWindow r_window)
        {
            InitializeComponent();
            chart = new MachChart(containerCanvas);
            senderWindow = r_window;
            isActive = true;
        }

        //Mouse move code:
        private Point currentPoint;
        private List<Line> drawingsList = new List<Line>();
        private void canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
                currentPoint = e.GetPosition(machCanvas);
        }
        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            Point p0 = chart.YO_Get_CursorPosition(0);

            Point p1 = chart.YO_Get_CursorPosition(1);

            Point p2 = chart.YO_Get_CursorPosition(2);

            Point p3 = chart.YO_Get_CursorPosition(3);


            statusBarXCursorPosition.Text = "X: " + Math.Round(p1.X, 2);

            statusBarY0CursorPosition.Text = chart.YO_Get_Description(0) + ": " + Math.Round(p0.Y, 2);
            statusBarY1CursorPosition.Text = chart.YO_Get_Description(1) + ": " + Math.Round(p1.Y, 2);
            statusBarY2CursorPosition.Text = chart.YO_Get_Description(2) + ": " + Math.Round(p2.Y, 2);
            statusBarY3CursorPosition.Text = chart.YO_Get_Description(3) + ": " + Math.Round(p3.Y, 2);


            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Line drawing = new Line();
                drawingsList.Add(drawing);
                drawing.Stroke = Brushes.Red;
                drawing.X1 = currentPoint.X;
                drawing.Y1 = currentPoint.Y;
                drawing.X2 = e.GetPosition(machCanvas).X;
                drawing.Y2 = e.GetPosition(machCanvas).Y;

                currentPoint = e.GetPosition(machCanvas);

                machCanvas.Children.Add(drawing);
            }
        }
        private void canvas_MouseDown2(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
                currentPoint = e.GetPosition(machCanvas);
        }
        private void canvas_MouseMove2(object sender, MouseEventArgs e)
        {
            Point p0 = chart.YO_Get_CursorPosition(0);

            Point p1 = chart.YO_Get_CursorPosition(1);

            Point p2 = chart.YO_Get_CursorPosition(2);

            Point p3 = chart.YO_Get_CursorPosition(3);


            statusBarXCursorPosition.Text = "X: " + Math.Round(p1.X, 2);

            statusBarY0CursorPosition.Text = chart.YO_Get_Description(0) + ": " + Math.Round(p0.Y, 2);
            statusBarY1CursorPosition.Text = chart.YO_Get_Description(1) + ": " + Math.Round(p0.Y, 2);
            statusBarY2CursorPosition.Text = chart.YO_Get_Description(2) + ": " + Math.Round(p0.Y, 2);
            statusBarY3CursorPosition.Text = chart.YO_Get_Description(3) + ": " + Math.Round(p0.Y, 2);


            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Line drawing = new Line();
                drawingsList.Add(drawing);
                drawing.Stroke = Brushes.Red;
                drawing.X1 = currentPoint.X;
                drawing.Y1 = currentPoint.Y;
                drawing.X2 = e.GetPosition(machCanvas).X;
                drawing.Y2 = e.GetPosition(machCanvas).Y;

                currentPoint = e.GetPosition(machCanvas);

                machCanvas.Children.Add(drawing);
            }
        }


        private void axisSettings_Click(object sender, RoutedEventArgs e)
        {
            AxisScopeWindow axisScopeWindow = new AxisScopeWindow(chart);
            axisScopeWindow.Show();
        }
        private void cleanDrawing_Click(object sender, RoutedEventArgs e)
        {
            foreach (Line drawing in drawingsList)
            {
                machCanvas.Children.Remove(drawing);
                machCanvas.Children.Remove(drawing);
            }
            drawingsList.Clear();
        }
        public MachChart chart;
        public MainWindow senderWindow;
        public bool isActive;
        private void window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            List<SeriesFunPair> tempList = new List<SeriesFunPair>();

            foreach(SeriesFunPair pair in senderWindow.seriesFunPairs)
            {
                if(chart.seriesList.Contains(pair.series)) tempList.Add(pair);
            }



            foreach (SeriesFunPair pair in tempList)
            {
                foreach (SeriesMatrix matrix in SeriesMatrix.collection)
                {
                    if (matrix.name == pair.ID)
                    {
                        matrix.isUsed = false;
                        int index = SeriesMatrix.collection.IndexOf(matrix) + 1;
                        matrix.name = index.ToString();
                        break;
                    }
                }
                pair.series.dispose();
            }

            foreach (SeriesFunPair pair in tempList) senderWindow.seriesFunPairs.Remove(pair);

            senderWindow.listBox.SelectedIndex = senderWindow.listBox.Items.Count - 1;
            if (senderWindow.listBox.Items.Count <= 0) senderWindow.suspendSliders = true;
            if (senderWindow.listBox.Items.Count != 0) senderWindow.addButton.IsEnabled = true;

            isActive = false;
        }
    }
}

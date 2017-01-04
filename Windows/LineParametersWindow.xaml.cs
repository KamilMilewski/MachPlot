using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for LineParametersWindow.xaml
    /// </summary>
    public partial class LineParametersWindow : Window
    {
        public MainWindow senderWindow;
        public Brush color;

        public LineParametersWindow(MainWindow r_Window)
        {
            InitializeComponent();
            senderWindow = r_Window as MainWindow;
        }

        private void ClrPcker_Background_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {            
            var converter = new System.Windows.Media.BrushConverter();
            color = (Brush)converter.ConvertFromString(ClrPcker_Background.SelectedColor.ToString());
        }

        private void acceptButton_Click(object sender, RoutedEventArgs e)
        {
            List<SeriesFunPair> tempList = new List<SeriesFunPair>();
            SeriesFunPair tempPair;
            foreach(SeriesFunPair pair in senderWindow.listBox.SelectedItems)
            {
                tempPair = pair as SeriesFunPair;
                tempList.Add(pair);
            }

            foreach (SeriesFunPair pair in tempList)
            {
                senderWindow.seriesFunPairs.Remove(pair);
                pair.series.polyline.StrokeThickness = textBoxValue;
                pair.series.polyline.Stroke = color;
                pair.series.color = color;
                senderWindow.seriesFunPairs.Add(new SeriesFunPair(pair.series, pair.corelatedFun));
            }


            this.Close();
        }

        double textBoxValue;
        private void textBox_handling(TextBox textBox)
        {
            double result;
            if (Double.TryParse(textBox.Text, out result))
            {
                textBox.Background = Brushes.SpringGreen;
                textBoxValue = result;
                acceptButton.IsEnabled = true;               
            }
            else
            {
                textBox.Background = Brushes.Salmon;
                acceptButton.IsEnabled = false;
            }
        }
        private void strokeWeightTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            textBox_handling(sender as TextBox);
        }
        private void strokeWeightTextBox_Loaded(object sender, RoutedEventArgs e)
        {
            strokeWeightTextBox.Text = Convert.ToString(senderWindow.parameters.strokeThickness);
        }
    }
}

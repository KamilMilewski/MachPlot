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
    /// Interaction logic for AxisScopeAxis.xaml
    /// </summary>
    public partial class AxisScopeWindow : Window
    {
        private MachChart chart;

        public AxisScopeWindow(MachChart r_chart)
        {
            InitializeComponent();
            chart = r_chart;

            minY0TextBox.Text = Math.Round(chart.YO_Get_GlobalMin(0), 3).ToString();
            maxY0TextBox.Text = Math.Round(chart.YO_Get_GlobalMax(0), 3).ToString();
            minY1TextBox.Text = Math.Round(chart.YO_Get_GlobalMin(1), 3).ToString();
            maxY1TextBox.Text = Math.Round(chart.YO_Get_GlobalMax(1), 3).ToString();
            minY2TextBox.Text = Math.Round(chart.YO_Get_GlobalMin(2), 3).ToString();
            maxY2TextBox.Text = Math.Round(chart.YO_Get_GlobalMax(2), 3).ToString();
            minY3TextBox.Text = Math.Round(chart.YO_Get_GlobalMin(3), 3).ToString();
            maxY3TextBox.Text = Math.Round(chart.YO_Get_GlobalMax(3), 3).ToString();

            disableEventFlag = true;
            Y0checkBox.IsChecked = chart.YO_Get_ManualScope(0);
            Y1checkBox.IsChecked = chart.YO_Get_ManualScope(1);
            Y2checkBox.IsChecked = chart.YO_Get_ManualScope(2);
            Y3checkBox.IsChecked = chart.YO_Get_ManualScope(3);

            Y00PointcheckBox.IsChecked = chart.YO_Get_ZeroPoint(0);
            Y10PointcheckBox.IsChecked = chart.YO_Get_ZeroPoint(1);
            Y20PointcheckBox.IsChecked = chart.YO_Get_ZeroPoint(2);
            Y30PointcheckBox.IsChecked = chart.YO_Get_ZeroPoint(3);
            disableEventFlag = false;

            minY0TextBox.IsEnabled = chart.YO_Get_ManualScope(0);
            maxY0TextBox.IsEnabled = chart.YO_Get_ManualScope(0);
            minY1TextBox.IsEnabled = chart.YO_Get_ManualScope(1);
            maxY1TextBox.IsEnabled = chart.YO_Get_ManualScope(1);
            minY2TextBox.IsEnabled = chart.YO_Get_ManualScope(2);
            maxY2TextBox.IsEnabled = chart.YO_Get_ManualScope(2);
            minY3TextBox.IsEnabled = chart.YO_Get_ManualScope(3);
            maxY3TextBox.IsEnabled = chart.YO_Get_ManualScope(3);
        }

        private void acceptButton_Click(object sender, RoutedEventArgs e)
        {
            if (chart.YO_Get_ManualScope(0))
            {
                chart.YO_Set_GlobalMin(0, Convert.ToDouble(minY0TextBox.Text));
                chart.YO_Set_GlobalMax(0, Convert.ToDouble(maxY0TextBox.Text));
            }

            if (chart.YO_Get_ManualScope(1))
            {
                chart.YO_Set_GlobalMin(1, Convert.ToDouble(minY1TextBox.Text));
                chart.YO_Set_GlobalMax(1, Convert.ToDouble(maxY1TextBox.Text));
            }

            if (chart.YO_Get_ManualScope(2))
            {
                chart.YO_Set_GlobalMin(2, Convert.ToDouble(minY2TextBox.Text));
                chart.YO_Set_GlobalMax(2, Convert.ToDouble(maxY2TextBox.Text));
            }

            if (chart.YO_Get_ManualScope(3))
            {
                chart.YO_Set_GlobalMin(3, Convert.ToDouble(minY3TextBox.Text));
                chart.YO_Set_GlobalMax(3, Convert.ToDouble(maxY3TextBox.Text));
            }

            //chart.updateAllSeries();
            this.Close();
        }
        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void textBoxChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            double result;
            if (Double.TryParse(textBox.Text, out result))
            {
                textBox.Background = Brushes.SpringGreen;
                acceptButton.IsEnabled = true;
            }
            else
            {
                textBox.Background = Brushes.Salmon;
                acceptButton.IsEnabled = false;
            }
        }

        public bool disableEventFlag;
        private void Y0checkBox_Toggled(object sender, RoutedEventArgs e)
        {
            if(!disableEventFlag)
            {
                minY0TextBox.IsEnabled = !minY0TextBox.IsEnabled;
                maxY0TextBox.IsEnabled = !maxY0TextBox.IsEnabled;
                chart.YO_Toggle_ManualScope(0);

            }
            
        }
        private void Y1checkBox_Toggled(object sender, RoutedEventArgs e)
        {
            if (!disableEventFlag)
            {
                minY1TextBox.IsEnabled = !minY1TextBox.IsEnabled;
                maxY1TextBox.IsEnabled = !maxY1TextBox.IsEnabled;
                chart.YO_Toggle_ManualScope(1);
            }         
        }
        private void Y2checkBox_Toggled(object sender, RoutedEventArgs e)
        {
            if (!disableEventFlag)
            {
                minY2TextBox.IsEnabled = !minY2TextBox.IsEnabled;
                maxY2TextBox.IsEnabled = !maxY2TextBox.IsEnabled;
                chart.YO_Toggle_ManualScope(2);
            }           
        }
        private void Y3checkBox_Toggled(object sender, RoutedEventArgs e)
        {
            if (!disableEventFlag)
            {
                minY3TextBox.IsEnabled = !minY3TextBox.IsEnabled;
                maxY3TextBox.IsEnabled = !maxY3TextBox.IsEnabled;
                chart.YO_Toggle_ManualScope(3);
            }           
        }

        private void Y0_ZeroPointcheckBox_Toggled(object sender, RoutedEventArgs e)
        {
            if (!disableEventFlag)
            {
                chart.YO_Toggle_ZeroPoint(0);
            }
        }
        private void Y1_ZeroPointcheckBox_Toggled(object sender, RoutedEventArgs e)
        {
            if (!disableEventFlag)
            {
                chart.YO_Toggle_ZeroPoint(1);
            }
        }
        private void Y2_ZeroPointcheckBox_Toggled(object sender, RoutedEventArgs e)
        {
            if (!disableEventFlag)
            {
                chart.YO_Toggle_ZeroPoint(2);
            }
        }
        private void Y3_ZeroPointcheckBox_Toggled(object sender, RoutedEventArgs e)
        {
            if (!disableEventFlag)
            {
                chart.YO_Toggle_ZeroPoint(3);
            }
        }
    }
}

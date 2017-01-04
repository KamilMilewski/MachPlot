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
    /// Interaction logic for DefaultParametersWindow.xaml
    /// </summary>
    /// 
    public partial class DefaultParametersWindow : Window
    {
        Parameters parameters = Parameters.ReadXml();

        public DefaultParametersWindow(MainWindow r_mainWindow)
        {
            InitializeComponent();

            mainWindow = r_mainWindow;

            U1TextBox.Text = Convert.ToString(parameters.U1);
            fTextBox.Text = Convert.ToString(parameters.f);
            R1TextBox.Text = Convert.ToString(parameters.R1);
            R2TextBox.Text = Convert.ToString(parameters.R2);
            L1TextBox.Text = Convert.ToString(parameters.L1);
            L2TextBox.Text = Convert.ToString(parameters.L2);
            LmTextBox.Text = Convert.ToString(parameters.Lm);
            RfeTextBox.Text = Convert.ToString(parameters.Rfe);

            U1maxTextBox.Text = Convert.ToString(parameters.U1max);
            U1minTextBox.Text = Convert.ToString(parameters.U1min);
            fmaxTextBox.Text = Convert.ToString(parameters.fmax);
            fminTextBox.Text = Convert.ToString(parameters.fmin);
            R1maxTextBox.Text = Convert.ToString(parameters.R1max);
            R1minTextBox.Text = Convert.ToString(parameters.R1min);
            R2maxTextBox.Text = Convert.ToString(parameters.R2max);
            R2minTextBox.Text = Convert.ToString(parameters.R2min);
            L1maxTextBox.Text = Convert.ToString(parameters.L1max);
            L1minTextBox.Text = Convert.ToString(parameters.L1min);
            L2maxTextBox.Text = Convert.ToString(parameters.L2max);
            L2minTextBox.Text = Convert.ToString(parameters.L2min);
            LmmaxTextBox.Text = Convert.ToString(parameters.Lmmax);
            LmminTextBox.Text = Convert.ToString(parameters.Lmmin);
            RfemaxTextBox.Text = Convert.ToString(parameters.Rfemax);
            RfeminTextBox.Text = Convert.ToString(parameters.Rfemin);

            startXTextBox.Text = Convert.ToString(parameters.startX);
            endXTextBox.Text = Convert.ToString(parameters.endX);
            stepTextBox.Text = Convert.ToString(parameters.step);

            aFanTextBox.Text = Convert.ToString(parameters.aFan);
            a2FanTextBox.Text = Convert.ToString(parameters.a2Fan);
            bFanTextBox.Text = Convert.ToString(parameters.bFan);
            cFanTextBox.Text = Convert.ToString(parameters.cFan);

            cCraneTextBox.Text = Convert.ToString(parameters.cCrane);

            aConveyorTextBox.Text = Convert.ToString(parameters.aConveyor);
            bConveyorTextBox.Text = Convert.ToString(parameters.bConveyor);
            cConveyorTextBox.Text = Convert.ToString(parameters.cConveyor);

            aGeneratorTextBox.Text = Convert.ToString(parameters.aGenerator);
            bGeneratorTextBox.Text = Convert.ToString(parameters.bGenerator);
            cGeneratorTextBox.Text = Convert.ToString(parameters.cGenerator);

            strokeThicknessTextBox.Text = Convert.ToString(parameters.strokeThickness);
            separateWindows_checkBox.IsChecked = parameters.separateWindows;
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.suspendSliders = true;

            //aktualizacja parametrow domyslnych
            parameters.U1 = Convert.ToDouble(U1TextBox.Text);
            parameters.f = Convert.ToDouble(fTextBox.Text);
            parameters.R1 = Convert.ToDouble(R1TextBox.Text);
            parameters.R2 = Convert.ToDouble(R2TextBox.Text);
            parameters.L1 = Convert.ToDouble(L1TextBox.Text);
            parameters.L2 = Convert.ToDouble(L2TextBox.Text);
            parameters.Lm = Convert.ToDouble(LmTextBox.Text);
            parameters.Rfe = Convert.ToDouble(RfeTextBox.Text);

            parameters.U1max = Convert.ToDouble(U1maxTextBox.Text);
            parameters.U1min = Convert.ToDouble(U1minTextBox.Text);
            parameters.fmax = Convert.ToDouble(fmaxTextBox.Text);
            parameters.fmin = Convert.ToDouble(fminTextBox.Text);
            parameters.R1max = Convert.ToDouble(R1maxTextBox.Text);
            parameters.R1min = Convert.ToDouble(R1minTextBox.Text);
            parameters.R2max = Convert.ToDouble(R2maxTextBox.Text);
            parameters.R2min = Convert.ToDouble(R2minTextBox.Text);
            parameters.L1max = Convert.ToDouble(L1maxTextBox.Text);
            parameters.L1min = Convert.ToDouble(L1minTextBox.Text);
            parameters.L2max = Convert.ToDouble(L2maxTextBox.Text);
            parameters.L2min = Convert.ToDouble(L2minTextBox.Text);
            parameters.Lmmax = Convert.ToDouble(LmmaxTextBox.Text);
            parameters.Lmmin = Convert.ToDouble(LmminTextBox.Text);
            parameters.Rfemax = Convert.ToDouble(RfemaxTextBox.Text);
            parameters.Rfemin = Convert.ToDouble(RfeminTextBox.Text);

            parameters.startX = Convert.ToDouble(startXTextBox.Text);
            parameters.endX = Convert.ToDouble(endXTextBox.Text);
            parameters.step = Convert.ToDouble(stepTextBox.Text);

            parameters.aFan = Convert.ToDouble(aFanTextBox.Text);
            parameters.a2Fan = Convert.ToDouble(a2FanTextBox.Text);
            parameters.bFan = Convert.ToDouble(bFanTextBox.Text);
            parameters.cFan = Convert.ToDouble(cFanTextBox.Text);

            parameters.cCrane = Convert.ToDouble(cCraneTextBox.Text);

            parameters.aConveyor = Convert.ToDouble(aConveyorTextBox.Text);
            parameters.bConveyor = Convert.ToDouble(bConveyorTextBox.Text);
            parameters.cConveyor = Convert.ToDouble(cConveyorTextBox.Text);

            parameters.aGenerator = Convert.ToDouble(aGeneratorTextBox.Text);
            parameters.bGenerator = Convert.ToDouble(bGeneratorTextBox.Text);
            parameters.cGenerator = Convert.ToDouble(cGeneratorTextBox.Text);

            parameters.strokeThickness = Convert.ToDouble(strokeThicknessTextBox.Text);
            parameters.separateWindows = Convert.ToBoolean(separateWindows_checkBox.IsChecked);

            parameters.SaveXml();

            mainWindow.suspendSliders = false;
            mainWindow.reset();
            this.Close();          
        }

        private void U1TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            textBoxHandling(U1TextBox, U1minTextBox, U1maxTextBox);
        }
        private void U1minTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            textBoxHandling(U1TextBox, U1minTextBox, U1maxTextBox);
        }
        private void U1maxTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            textBoxHandling(U1TextBox, U1minTextBox, U1maxTextBox);
        }
        private void fTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            textBoxHandling(fTextBox, fminTextBox, fmaxTextBox);
        }
        private void fminTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            textBoxHandling(fTextBox, fminTextBox, fmaxTextBox);
        }
        private void fmaxTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            textBoxHandling(fTextBox, fminTextBox, fmaxTextBox);
        }
        private void R1TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            textBoxHandling(R1TextBox, R1minTextBox, R1maxTextBox);
        }
        private void R1minTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            textBoxHandling(R1TextBox, R1minTextBox, R1maxTextBox);
        }
        private void R1maxTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            textBoxHandling(R1TextBox, R1minTextBox, R1maxTextBox);
        }
        private void R2TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            textBoxHandling(R2TextBox, R2minTextBox, R2maxTextBox);
        }
        private void R2minTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            textBoxHandling(R2TextBox, R2minTextBox, R2maxTextBox);
        }
        private void R2maxTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            textBoxHandling(R2TextBox, R2minTextBox, R2maxTextBox);
        }
        private void L1TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            textBoxHandling(L1TextBox, L1minTextBox, L1maxTextBox);
        }
        private void L1minTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            textBoxHandling(L1TextBox, L1minTextBox, L1maxTextBox);
        }
        private void L1maxTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            textBoxHandling(L1TextBox, L1minTextBox, L1maxTextBox);
        }
        private void L2TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            textBoxHandling(L2TextBox, L2minTextBox, L2maxTextBox);
        }
        private void L2minTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            textBoxHandling(L2TextBox, L2minTextBox, L2maxTextBox);
        }
        private void L2maxTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            textBoxHandling(L2TextBox, L2minTextBox, L2maxTextBox);
        }
        private void LmTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            textBoxHandling(LmTextBox, LmminTextBox, LmmaxTextBox);
        }
        private void LmminTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            textBoxHandling(LmTextBox, LmminTextBox, LmmaxTextBox);
        }
        private void LmmaxTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            textBoxHandling(LmTextBox, LmminTextBox, LmmaxTextBox);
        }
        private void RfeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            textBoxHandling(RfeTextBox, RfeminTextBox, RfemaxTextBox);
        }
        private void RfeminTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            textBoxHandling(RfeTextBox, RfeminTextBox, RfemaxTextBox);
        }                    
        private void RfemaxTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            textBoxHandling(RfeTextBox, RfeminTextBox, RfemaxTextBox);
        }
             
        private void aConveyorTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            textBoxHandling(sender as TextBox);
        }
        private void bConveyorTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            textBoxHandling(sender as TextBox);
        }
        private void cConveyorTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            textBoxHandling(sender as TextBox);
        }
        private void aGeneratorTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            textBoxHandling(sender as TextBox);
        }
        private void bGeneratorTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            textBoxHandling(sender as TextBox);
        }
        private void cGeneratorTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            textBoxHandling(sender as TextBox);
        }
        private void cCraneTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            textBoxHandling(sender as TextBox);
        }
        private void aFanTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            textBoxHandling(sender as TextBox);
        }
        private void bFanTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            textBoxHandling(sender as TextBox);
        }
        private void cFanTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            textBoxHandling(sender as TextBox);
        }
        private void a2FanTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            textBoxHandling(sender as TextBox);
        }

        private void startXTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            textBoxHandling(sender as TextBox);
        }
        private void endXTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            textBoxHandling(sender as TextBox);
        }
        private void stepTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            textBoxHandling(sender as TextBox);
        }

        private bool textBoxHandling(TextBox textBox)
        {
            double result;
            if (Double.TryParse(textBox.Text, out result))
            {
                textBox.Background = Brushes.SpringGreen;
                saveButton.IsEnabled = true;
                return true;
            }
            else
            {
                textBox.Background = Brushes.Salmon;
                saveButton.IsEnabled = false;
                return false;
            }
        }
        private bool textBoxHandling(TextBox textBoxDef, TextBox textBoxMin, TextBox textBoxMax)
        {
            double resultDef;
            double resultMin;
            double resultMax;

            if (Double.TryParse(textBoxDef.Text, out resultDef) &&
                Double.TryParse(textBoxMin.Text, out resultMin) &&
                Double.TryParse(textBoxMax.Text, out resultMax))
            {
                if (resultMax > resultMin && resultMin != 0)
                {
                    if(resultDef >= resultMin && resultDef <= resultMax)
                    {
                        textBoxDef.Background = Brushes.SpringGreen;
                        textBoxMin.Background = Brushes.SpringGreen;
                        textBoxMax.Background = Brushes.SpringGreen;
                        saveButton.IsEnabled = true;
                        return true;
                    }
                    else
                    {
                        textBoxDef.Background = Brushes.Salmon;
                        textBoxMin.Background = Brushes.Salmon;
                        textBoxMax.Background = Brushes.Salmon;
                        saveButton.IsEnabled = false;
                        return false;
                    }
                }
                else
                {
                    textBoxDef.Background = Brushes.Salmon;
                    textBoxMin.Background = Brushes.Salmon;
                    textBoxMax.Background = Brushes.Salmon;
                    saveButton.IsEnabled = false;
                    return false;
                }
            }
            else
            {
                textBoxDef.Background = Brushes.Salmon;
                textBoxMin.Background = Brushes.Salmon;
                textBoxMax.Background = Brushes.Salmon;
                saveButton.IsEnabled = false;
                return false;
            }
        }

        private MainWindow mainWindow;

        private void strokeThicknessTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            textBoxHandling(sender as TextBox);
        }        
    }
}

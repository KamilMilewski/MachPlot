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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.IO;

namespace MachPlotNamespace
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Declarations:
        public MachChart chart0;
        public MachChart chart1;
        public Parameters parameters;
        public ObservableCollection<SeriesFunPair> seriesFunPairs = new ObservableCollection<SeriesFunPair>();

        public MainWindow()
        {
            InitializeComponent();

            chart0 = new MachChart(containerCanvas0);
            chart1 = new MachChart(containerCanvas1);

            SeriesMatrix.refresh();
            parameters = Parameters.ReadXml();                       
        }

        //Sliders:
        private void sliderU1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            sliderHandling(sender);
        }
        private void sliderf_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            sliderHandling(sender);
        }
        private void sliderR1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            sliderHandling(sender);
        }
        private void sliderR2_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            sliderHandling(sender);
        }
        private void sliderL1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            sliderHandling(sender);
        }
        private void sliderL2_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            sliderHandling(sender);
        }
        private void sliderLm_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            sliderHandling(sender);
        }
        private void sliderRfe_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            sliderHandling(sender);
        }
        public void setSliders()
        {
            //setting starting point for sliders:
                SeriesFunPair pair;
                if (listBox.SelectedIndex != -1 && 
                    !(listBox.SelectedItem as SeriesFunPair).ID.Contains("maszyna robocza")
                    )
                {
                    pair = listBox.SelectedItem as SeriesFunPair;
                    sliderU1.Value = (pair.corelatedFun as AsynchronousMotor).U1;
                    sliderf.Value = (pair.corelatedFun as AsynchronousMotor).f;
                    sliderR1.Value = (pair.corelatedFun as AsynchronousMotor).R1;
                    sliderR2.Value = (pair.corelatedFun as AsynchronousMotor).R2;
                    sliderL1.Value = (pair.corelatedFun as AsynchronousMotor).L1;
                    sliderL2.Value = (pair.corelatedFun as AsynchronousMotor).L2;
                    sliderLm.Value = (pair.corelatedFun as AsynchronousMotor).Lm;
                    sliderRfe.Value = (pair.corelatedFun as AsynchronousMotor).Rfe;
                }
                else//for reset button
                {
                    sliderU1.Value = 0;
                    sliderf.Value = 0;
                    sliderR1.Value = 0;
                    sliderR2.Value = 0;
                    sliderL1.Value = 0;
                    sliderL2.Value = 0;
                    sliderLm.Value = 0;
                    sliderRfe.Value = 0;
                }

                sliderU1.Maximum = parameters.U1max;
                sliderU1.Minimum = parameters.U1min;
                sliderf.Maximum = parameters.fmax;
                sliderf.Minimum = parameters.fmin;
                sliderR1.Maximum = parameters.R1max;
                sliderR1.Minimum = parameters.R1min;
                sliderR2.Maximum = parameters.R2max;
                sliderR2.Minimum = parameters.R2min;
                sliderL1.Maximum = parameters.L1max;
                sliderL1.Minimum = parameters.L1min;
                sliderL2.Maximum = parameters.L2max;
                sliderL2.Minimum = parameters.L2min;
                sliderLm.Maximum = parameters.Lmmax;
                sliderLm.Minimum = parameters.Lmmin;
                sliderRfe.Maximum = parameters.Rfemax;
                sliderRfe.Minimum = parameters.Rfemin;      
        }
        public bool suspendSliders = true; //flag for sliderHandling()
        private void sliderHandling(object r_sender)
        {
            if (suspendSliders == false && listBox.SelectedIndex != -1)
            {
                SeriesFunPair selectedPair;

                for (int i = 0; i < listBox.SelectedItems.Count; i++)
                {                    
                    selectedPair = listBox.SelectedItems[i] as SeriesFunPair;

                    if(selectedPair.corelatedFun is AsynchronousMotor)
                    {
                        if (r_sender == sliderU1) (selectedPair.corelatedFun as AsynchronousMotor).U1 = (r_sender as Slider).Value;
                        else if (r_sender == sliderf) (selectedPair.corelatedFun as AsynchronousMotor).f = (r_sender as Slider).Value;
                        else if (r_sender == sliderR1) (selectedPair.corelatedFun as AsynchronousMotor).R1 = (r_sender as Slider).Value;
                        else if (r_sender == sliderR2) (selectedPair.corelatedFun as AsynchronousMotor).R2 = (r_sender as Slider).Value;
                        else if (r_sender == sliderL1) (selectedPair.corelatedFun as AsynchronousMotor).L1 = (r_sender as Slider).Value;
                        else if (r_sender == sliderL2) (selectedPair.corelatedFun as AsynchronousMotor).L2 = (r_sender as Slider).Value;
                        else if (r_sender == sliderLm) (selectedPair.corelatedFun as AsynchronousMotor).Lm = (r_sender as Slider).Value;
                        else if (r_sender == sliderRfe) (selectedPair.corelatedFun as AsynchronousMotor).Rfe = (r_sender as Slider).Value;

                        selectedPair.corelatedFun.calculate();
                        selectedPair.series.update(selectedPair.corelatedFun.dataPointList);
                    }
                }
                statusBarValues.Text = (listBox.SelectedItem as SeriesFunPair).corelatedFun.dataString;
                statusBarValues2.Text = (listBox.SelectedItem as SeriesFunPair).corelatedFun.dataString;

                if(U1tofRatio != null)
                {
                    if (r_sender == sliderU1)
                    (sliderf as Slider).Value = sliderU1.Value / Convert.ToDouble(U1tofRatio);
                    else if (r_sender == sliderf)
                    (sliderU1 as Slider).Value = sliderf.Value * Convert.ToDouble(U1tofRatio);

                    U1tofActualValuetextBox.Text = Convert.ToString(sliderU1.Value / sliderf.Value);
                }

            }
        } //for slider events

        //ListBox:
        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (listBox.SelectedIndex != -1)
            {
                setSliders();

                foreach(SeriesFunPair pair in seriesFunPairs)
                {
                    pair.series.polyline.StrokeDashArray = new DoubleCollection(new double[] { });
                }

                //MachSeries selectedItem;
                SeriesFunPair selectedPair;
                for (int i = 0; i < listBox.SelectedItems.Count; i++)
                {
                    selectedPair = listBox.SelectedItems[i] as SeriesFunPair;
                    selectedPair.series.polyline.StrokeDashArray = new DoubleCollection(new double[] { 5, 5 });
                }
            }
        }
        private void listBox_Loaded(object sender, RoutedEventArgs e)
        {
            listBox.ItemsSource = seriesFunPairs;
        }

        //Events for adding new series.
        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            addNewSeries(chart0);            
        }
        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            addNewSeries(chart0);
        }   
        //Function for creating new series.
        private void addNewSeries(MachChart r_plot)
        {
            if (comboBox.SelectedIndex != -1)
            {
                switch (comboBox.SelectedItem.ToString())
            {
                case "elektryczny moment obrotowy: model 1":
                    createSeries(comboBox.SelectedItem.ToString(), "n[obr/min]", "Me [Nm]", r_plot);
                    break;
                case "elektryczny moment obrotowy: model 2":
                    createSeries(comboBox.SelectedItem.ToString(), "n[obr/min]", "Me [Nm]", r_plot);
                    break;
                case "elektryczny moment obrotowy: model 3":
                    createSeries(comboBox.SelectedItem.ToString(), "n[obr/min]", "Me [Nm]", r_plot);
                    break;


                case "maszyna robocza: obciążenie wentylatorowe":
                    createSeries(comboBox.SelectedItem.ToString(), "n[obr/min]", "Me [Nm]", r_plot);
                    break;
                case "maszyna robocza: obciążenie taśmowe":
                    createSeries(comboBox.SelectedItem.ToString(), "n[obr/min]", "Me [Nm]", r_plot);
                    break;
                case "maszyna robocza: obciążenie dźwigowe":
                    createSeries(comboBox.SelectedItem.ToString(), "n[obr/min]", "Me [Nm]", r_plot);
                    break;
                case "maszyna robocza: obciążenie generatorowe":
                    createSeries(comboBox.SelectedItem.ToString(), "n[obr/min]", "Me [Nm]", r_plot);
                    break;


                case "prąd wirnika: model 1":
                    createSeries(comboBox.SelectedItem.ToString(), "n[obr/min]", "I [A]", r_plot);
                    break;
                case "prąd wirnika: model 2":
                    createSeries(comboBox.SelectedItem.ToString(), "n[obr/min]", "I [A]", r_plot);
                    break;
                case "prąd wirnika: model 3":
                    createSeries(comboBox.SelectedItem.ToString(), "n[obr/min]", "I [A]", r_plot);
                    break;


                case "prąd stojana: model 1":
                    createSeries(comboBox.SelectedItem.ToString(), "n[obr/min]", "I [A]", r_plot);
                    break;
                case "prąd stojana: model 2":
                    createSeries(comboBox.SelectedItem.ToString(), "n[obr/min]", "I [A]", r_plot);
                    break;
                case "prąd stojana: model 3":
                    createSeries(comboBox.SelectedItem.ToString(), "n[obr/min]", "I [A]", r_plot);
                    break;


                case "prąd gałęzi poprzecznej: model 1":
                    createSeries(comboBox.SelectedItem.ToString(), "n[obr/min]", "I [A]", r_plot);
                    break;
                case "prąd gałęzi poprzecznej: model 2":
                    createSeries(comboBox.SelectedItem.ToString(), "n[obr/min]", "I [A]", r_plot);
                    break;
                case "prąd gałęzi poprzecznej: model 3":
                    createSeries(comboBox.SelectedItem.ToString(), "n[obr/min]", "I [A]", r_plot);
                    break;


                case "moc czynna wejściowa: model 1":
                    createSeries(comboBox.SelectedItem.ToString(), "n[obr/min]", "P [W]", r_plot);
                    break;
                case "moc czynna wejściowa: model 2":
                    createSeries(comboBox.SelectedItem.ToString(), "n[obr/min]", "P [W]", r_plot);
                    break;
                case "moc czynna wejściowa: model 3":
                    createSeries(comboBox.SelectedItem.ToString(), "n[obr/min]", "P [W]", r_plot);
                    break;


                case "współczynnik mocy: model 1":
                    createSeries(comboBox.SelectedItem.ToString(), "n[obr/min]", "PF [-]", r_plot);
                    break;
                case "współczynnik mocy: model 2":
                    createSeries(comboBox.SelectedItem.ToString(), "n[obr/min]", "PF [-]", r_plot);
                    break;
                case "współczynnik mocy: model 3":
                    createSeries(comboBox.SelectedItem.ToString(), "n[obr/min]", "PF [-]", r_plot);
                    break;


                case "impedancja wirnika: model 1":
                    createSeries(comboBox.SelectedItem.ToString(), "n[obr/min]", "Z2 [Ohm]", r_plot);
                    break;
                case "impedancja wirnika: model 2":
                    createSeries(comboBox.SelectedItem.ToString(), "n[obr/min]", "Z2 [Ohm]", r_plot);
                    break;
                case "impedancja wirnika: model 3":
                    createSeries(comboBox.SelectedItem.ToString(), "n[obr/min]", "Z2 [Ohm]", r_plot);
                    break;
            }
            }


            if (SeriesMatrix.collection.Count == 0) addButton.IsEnabled = false;

            startXtextBox.Text = Convert.ToString(chart0.XO_Get_GlobalMin());
            endXtextBox.Text = Convert.ToString(chart0.XO_Get_GlobalMax());
        }//------
        //Helper function for addNewSeries().
        private void createSeries(string r_choosenFunctionType, string r_xAxisLabel, string r_yAxisLabel, MachChart r_plot)
        {
            foreach (SeriesMatrix matrix in SeriesMatrix.collection)
            {
                if(matrix.isUsed == false)
                {
                    MachModel newFun;
                    if (r_choosenFunctionType.Contains("maszyna robocza"))
                    {
                        newFun = new LoadMachine(r_choosenFunctionType);
                        (newFun as LoadMachine).aFan = parameters.aFan;
                        (newFun as LoadMachine).a2Fan = parameters.a2Fan;
                        (newFun as LoadMachine).bFan = parameters.bFan;
                        (newFun as LoadMachine).cFan = parameters.cFan;

                        (newFun as LoadMachine).cCrane = parameters.cCrane;

                        (newFun as LoadMachine).aConveyor = parameters.aConveyor;
                        (newFun as LoadMachine).bConveyor = parameters.bConveyor;
                        (newFun as LoadMachine).cConveyor = parameters.cConveyor;

                        (newFun as LoadMachine).aGenerator = parameters.aGenerator;
                        (newFun as LoadMachine).bGenerator = parameters.bGenerator;
                        (newFun as LoadMachine).cGenerator = parameters.cGenerator;
                    }
                    else
                    {
                        newFun = new AsynchronousMotor(r_choosenFunctionType);
                        (newFun as AsynchronousMotor).U1 = parameters.U1;
                        (newFun as AsynchronousMotor).f = parameters.f;
                        (newFun as AsynchronousMotor).R1 = parameters.R1;
                        (newFun as AsynchronousMotor).R2 = parameters.R2;
                        (newFun as AsynchronousMotor).L1 = parameters.L1;
                        (newFun as AsynchronousMotor).L2 = parameters.L2;
                        (newFun as AsynchronousMotor).Lm = parameters.Lm;
                        (newFun as AsynchronousMotor).Rfe = parameters.Rfe;
                    }

                    newFun.step = parameters.step;
                    newFun.startX = SeriesMatrix.startX;
                    newFun.endX = SeriesMatrix.endX;

                    newFun.calculate();

                    string seriesID = matrix.name + ". " + r_choosenFunctionType;
                    MachChart.MachSeries newSeries = new MachChart.MachSeries(
                                                    seriesID,
                                                    r_xAxisLabel,
                                                    r_yAxisLabel,
                                                    matrix.color,
                                                    r_plot,
                                                    newFun.dataPointList
                                                    );

                    seriesFunPairs.Add(new SeriesFunPair(newSeries, newFun));         

                    listBox.SelectedIndex = listBox.Items.Count - 1;
                    suspendSliders = false;
                    matrix.isUsed = true;
                    matrix.name = seriesID;
                    break;
                }
            }
            statusBarValues.Text = (listBox.SelectedItem as SeriesFunPair).corelatedFun.dataString;
        }

        //Function ComboBox:
        private void comboBox_Loaded(object sender, RoutedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            comboBox.Items.Add("elektryczny moment obrotowy: model 1");
            comboBox.Items.Add("elektryczny moment obrotowy: model 2");
            comboBox.Items.Add("elektryczny moment obrotowy: model 3");
            comboBox.Items.Add(new Separator());
            comboBox.Items.Add("maszyna robocza: obciążenie wentylatorowe");
            comboBox.Items.Add("maszyna robocza: obciążenie taśmowe");
            comboBox.Items.Add("maszyna robocza: obciążenie dźwigowe");
            comboBox.Items.Add("maszyna robocza: obciążenie generatorowe");
            comboBox.Items.Add(new Separator());
            comboBox.Items.Add("prąd wirnika: model 1");
            comboBox.Items.Add("prąd wirnika: model 2");
            comboBox.Items.Add("prąd wirnika: model 3");
            comboBox.Items.Add(new Separator());
            comboBox.Items.Add("prąd stojana: model 1");
            comboBox.Items.Add("prąd stojana: model 2");
            comboBox.Items.Add("prąd stojana: model 3");
            comboBox.Items.Add(new Separator());
            comboBox.Items.Add("prąd gałęzi poprzecznej: model 1");
            comboBox.Items.Add("prąd gałęzi poprzecznej: model 2");
            comboBox.Items.Add("prąd gałęzi poprzecznej: model 3");
            comboBox.Items.Add(new Separator());
            comboBox.Items.Add("moc czynna wejściowa: model 1");
            comboBox.Items.Add("moc czynna wejściowa: model 2");
            comboBox.Items.Add("moc czynna wejściowa: model 3");
            comboBox.Items.Add(new Separator());
            comboBox.Items.Add("współczynnik mocy: model 1");
            comboBox.Items.Add("współczynnik mocy: model 2");
            comboBox.Items.Add("współczynnik mocy: model 3");
            comboBox.Items.Add(new Separator());
            comboBox.Items.Add("impedancja wirnika: model 1");
            comboBox.Items.Add("impedancja wirnika: model 2");
            comboBox.Items.Add("impedancja wirnika: model 3");
        }//------
        
        //Handling changes of x range:
        private void startXtextBox_OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) calculateNewXRange(sender);
        }
        private void endXtextBox_OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) calculateNewXRange(sender);
        }
        private void startXtextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            calculateNewXRange(sender);
        }
        private void endXtextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            calculateNewXRange(sender);
        }
        private void calculateNewXRange(object sender)
        {
            double result;
            if (Double.TryParse(startXtextBox.Text, out result) && Double.TryParse(endXtextBox.Text, out result) &&
                Convert.ToDouble(startXtextBox.Text) < chart0.XO_Get_GlobalMax() && Convert.ToDouble(endXtextBox.Text) > chart0.XO_Get_GlobalMin())
            {
                SeriesMatrix.startX = Convert.ToDouble(startXtextBox.Text);
                SeriesMatrix.endX = Convert.ToDouble(endXtextBox.Text);

                startXtextBox.Background = Brushes.SpringGreen;
                endXtextBox.Background = Brushes.SpringGreen;

                if (sender == startXtextBox)
                {
                    foreach (SeriesFunPair pair in seriesFunPairs)
                    {
                        pair.corelatedFun.startX = SeriesMatrix.startX;
                        pair.corelatedFun.endX = SeriesMatrix.endX;
                        pair.corelatedFun.calculate();
                        pair.series.update(pair.corelatedFun.dataPointList);
                        pair.corelatedFun.generateDataString();
                    }
                }
                else if (sender == endXtextBox)
                {
                    foreach (SeriesFunPair pair in seriesFunPairs)
                    {
                        pair.corelatedFun.startX = SeriesMatrix.startX;
                        pair.corelatedFun.endX = SeriesMatrix.endX;
                        pair.corelatedFun.calculate();
                        pair.series.update(pair.corelatedFun.dataPointList);
                        pair.corelatedFun.generateDataString();
                    }
                }
            }
            else
            {
                startXtextBox.Background = Brushes.Salmon;
                endXtextBox.Background = Brushes.Salmon;
            }
            if (listBox.SelectedIndex != -1)
            statusBarValues.Text = (listBox.SelectedItem as SeriesFunPair).corelatedFun.dataString;
        } //Function invoked by four X range textBox handlers

        //Handling changes of parameters textBoxes:
        private void U1textBox_OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                double result;
                SeriesFunPair selectedSeriesFunPair;
                if (Double.TryParse((sender as TextBox).Text, out result))
                {
                    foreach (SeriesFunPair selectedItem in listBox.SelectedItems)
                    {
                        selectedSeriesFunPair = selectedItem as SeriesFunPair;
                        (selectedSeriesFunPair.corelatedFun as AsynchronousMotor).U1 = result;
                        (selectedSeriesFunPair.corelatedFun as AsynchronousMotor).calculate();
                        selectedSeriesFunPair.series.update(selectedSeriesFunPair.corelatedFun.dataPointList);
                        sliderU1.Value = result;
                    }
                }
            }
        }
        private void ftextBox_OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                double result;
                SeriesFunPair selectedSeriesFunPair;
                if (Double.TryParse((sender as TextBox).Text, out result))
                {
                    foreach (SeriesFunPair selectedItem in listBox.SelectedItems)
                    {
                        selectedSeriesFunPair = selectedItem as SeriesFunPair;
                        (selectedSeriesFunPair.corelatedFun as AsynchronousMotor).f = result;
                        (selectedSeriesFunPair.corelatedFun as AsynchronousMotor).calculate();
                        selectedSeriesFunPair.series.update(selectedSeriesFunPair.corelatedFun.dataPointList);
                        sliderf.Value = result;
                    }
                }
            }
        }
        private void R1textBox_OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                double result;
                SeriesFunPair selectedSeriesFunPair;
                if (Double.TryParse((sender as TextBox).Text, out result))
                {
                    foreach (SeriesFunPair selectedItem in listBox.SelectedItems)
                    {
                        selectedSeriesFunPair = selectedItem as SeriesFunPair;
                        (selectedSeriesFunPair.corelatedFun as AsynchronousMotor).R1 = result;
                        (selectedSeriesFunPair.corelatedFun as AsynchronousMotor).calculate();
                        selectedSeriesFunPair.series.update(selectedSeriesFunPair.corelatedFun.dataPointList);
                        sliderR1.Value = result;
                    }
                }
            }
        }
        private void R2textBox_OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                double result;
                SeriesFunPair selectedSeriesFunPair;
                if (Double.TryParse((sender as TextBox).Text, out result))
                {
                    foreach (SeriesFunPair selectedItem in listBox.SelectedItems)
                    {
                        selectedSeriesFunPair = selectedItem as SeriesFunPair;
                        (selectedSeriesFunPair.corelatedFun as AsynchronousMotor).R2 = result;
                        (selectedSeriesFunPair.corelatedFun as AsynchronousMotor).calculate();
                        selectedSeriesFunPair.series.update(selectedSeriesFunPair.corelatedFun.dataPointList);
                        sliderR2.Value = result;
                    }
                }
            }
        }
        private void L1textBox_OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                double result;
                SeriesFunPair selectedSeriesFunPair;
                if (Double.TryParse((sender as TextBox).Text, out result))
                {
                    foreach (SeriesFunPair selectedItem in listBox.SelectedItems)
                    {
                        selectedSeriesFunPair = selectedItem as SeriesFunPair;
                        (selectedSeriesFunPair.corelatedFun as AsynchronousMotor).L1 = result;
                        (selectedSeriesFunPair.corelatedFun as AsynchronousMotor).calculate();
                        selectedSeriesFunPair.series.update(selectedSeriesFunPair.corelatedFun.dataPointList);
                        sliderL1.Value = result;
                    }
                }
            }
        }
        private void L2textBox_OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                double result;
                SeriesFunPair selectedSeriesFunPair;
                if (Double.TryParse((sender as TextBox).Text, out result))
                {
                    foreach (SeriesFunPair selectedItem in listBox.SelectedItems)
                    {
                        selectedSeriesFunPair = selectedItem as SeriesFunPair;
                        (selectedSeriesFunPair.corelatedFun as AsynchronousMotor).L2 = result;
                        (selectedSeriesFunPair.corelatedFun as AsynchronousMotor).calculate();
                        selectedSeriesFunPair.series.update(selectedSeriesFunPair.corelatedFun.dataPointList);
                        sliderL2.Value = result;
                    }
                }
            }
        }
        private void LmtextBox_OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                double result;
                SeriesFunPair selectedSeriesFunPair;
                if (Double.TryParse((sender as TextBox).Text, out result))
                {
                    foreach (SeriesFunPair selectedItem in listBox.SelectedItems)
                    {
                        selectedSeriesFunPair = selectedItem as SeriesFunPair;
                        (selectedSeriesFunPair.corelatedFun as AsynchronousMotor).Lm = result;
                        (selectedSeriesFunPair.corelatedFun as AsynchronousMotor).calculate();
                        selectedSeriesFunPair.series.update(selectedSeriesFunPair.corelatedFun.dataPointList);
                        sliderLm.Value = result;
                    }
                }
            }
        }
        private void RfetextBox_OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                double result;
                SeriesFunPair selectedSeriesFunPair;
                if (Double.TryParse((sender as TextBox).Text, out result))
                {
                    foreach (SeriesFunPair selectedItem in listBox.SelectedItems)
                    {
                        selectedSeriesFunPair = selectedItem as SeriesFunPair;
                        (selectedSeriesFunPair.corelatedFun as AsynchronousMotor).Rfe = result;
                        (selectedSeriesFunPair.corelatedFun as AsynchronousMotor).calculate();
                        selectedSeriesFunPair.series.update(selectedSeriesFunPair.corelatedFun.dataPointList);
                        sliderRfe.Value = result;
                    }
                }
            }
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
            Point p0 = chart0.YO_Get_CursorPosition(0);

            Point p1 = chart0.YO_Get_CursorPosition(1);

            Point p2 = chart0.YO_Get_CursorPosition(2);

            Point p3 = chart0.YO_Get_CursorPosition(3);


            statusBarXCursorPosition.Text = "X: " + Math.Round(p1.X, 2);

            statusBarY0CursorPosition.Text = chart0.YO_Get_Description(0) + ": " + Math.Round(p0.Y, 2);
            statusBarY1CursorPosition.Text = chart0.YO_Get_Description(1) + ": " + Math.Round(p1.Y, 2);
            statusBarY2CursorPosition.Text = chart0.YO_Get_Description(2) + ": " + Math.Round(p2.Y, 2);
            statusBarY3CursorPosition.Text = chart0.YO_Get_Description(3) + ": " + Math.Round(p3.Y, 2);


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
                currentPoint = e.GetPosition(machCanvas1);
        }
        private void canvas_MouseMove2(object sender, MouseEventArgs e)
        {
            Point p0 = chart1.YO_Get_CursorPosition(0);

            Point p1 = chart1.YO_Get_CursorPosition(1);

            Point p2 = chart1.YO_Get_CursorPosition(2);

            Point p3 = chart1.YO_Get_CursorPosition(3);


            statusBarXCursorPosition2.Text = "X: " + Math.Round(p1.X, 2);

            statusBarY0CursorPosition2.Text = chart1.YO_Get_Description(0) + ": " + Math.Round(p0.Y, 2);
            statusBarY1CursorPosition2.Text = chart1.YO_Get_Description(1) + ": " + Math.Round(p0.Y, 2);
            statusBarY2CursorPosition2.Text = chart1.YO_Get_Description(2) + ": " + Math.Round(p0.Y, 2);
            statusBarY3CursorPosition2.Text = chart1.YO_Get_Description(3) + ": " + Math.Round(p0.Y, 2);


            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Line drawing = new Line();
                drawingsList.Add(drawing);
                drawing.Stroke = Brushes.Red;
                drawing.X1 = currentPoint.X;
                drawing.Y1 = currentPoint.Y;
                drawing.X2 = e.GetPosition(machCanvas1).X;
                drawing.Y2 = e.GetPosition(machCanvas1).Y;

                currentPoint = e.GetPosition(machCanvas1);

                machCanvas1.Children.Add(drawing);
            }
        }

        //Others
        private void deleteSeries_Click(object sender, RoutedEventArgs e)
        {

            SeriesFunPair selectedPair;
            List<SeriesFunPair> tempList = new List<SeriesFunPair>();
            for (int i = 0; i < listBox.SelectedItems.Count; i++)
            {
                selectedPair = listBox.SelectedItems[i] as SeriesFunPair;
                tempList.Add(selectedPair);
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

            foreach (SeriesFunPair pair in tempList) seriesFunPairs.Remove(pair);

            listBox.SelectedIndex = listBox.Items.Count - 1;
            if (listBox.Items.Count <= 0) suspendSliders = true;
            if (listBox.Items.Count != 0) addButton.IsEnabled = true;
        }
        public Brush selectedLineColor;//for lineOptions
        public double selectedLineWeight;//for lineOptions
        private void lineOptions_Click(object sender, RoutedEventArgs e)
        {
            LineParametersWindow lineParametersWindow = new LineParametersWindow(this);
            lineParametersWindow.Show();
        }
        private void changeChart_Click(object sender, RoutedEventArgs e)
        {
            SeriesFunPair selectedPair;
            List<SeriesFunPair> tempList = new List<SeriesFunPair>();
            for (int i = 0; i < listBox.SelectedItems.Count; i++)
            {
                selectedPair = listBox.SelectedItems[i] as SeriesFunPair;
                tempList.Add(selectedPair);
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

            foreach (SeriesFunPair pair in tempList) seriesFunPairs.Remove(pair);

            listBox.SelectedIndex = listBox.Items.Count - 1;
            if (listBox.Items.Count <= 0) suspendSliders = true;
            if (listBox.Items.Count != 0) addButton.IsEnabled = true;


            foreach (SeriesFunPair pair in tempList)
            {
                if (pair.series.chart == chart0)
                {
                    if (pair.corelatedFun is LoadMachine)
                    {
                        createSeries((pair.corelatedFun as LoadMachine).functionChoice,
                        pair.series.axisXDescription,
                        pair.series.axisYDescription,
                        chart1);
                    }
                    else
                    {
                        createSeries((pair.corelatedFun as AsynchronousMotor).functionChoice,
                        pair.series.axisXDescription,
                        pair.series.axisYDescription,
                        chart1);
                    }
                }
                else if (pair.series.chart == chart1)
                {
                    if (pair.corelatedFun is LoadMachine)
                    {
                        createSeries((pair.corelatedFun as LoadMachine).functionChoice,
                        pair.series.axisXDescription,
                        pair.series.axisYDescription,
                        chart0);
                    }
                    else
                    {
                        createSeries((pair.corelatedFun as AsynchronousMotor).functionChoice,
                        pair.series.axisXDescription,
                        pair.series.axisYDescription,
                        chart0);
                    }
                }
            }
            RowDefinition rowDef = new RowDefinition();
            rowDef.Height = GridLength.Auto;
            firstColumnGrid.RowDefinitions[2] = rowDef;

        }
        private void selectAll_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < listBox.Items.Count; i++)
            {
                listBox.SelectAll();
            }
        } 
        private void cleanDrawing_Click(object sender, RoutedEventArgs e)
        {
            foreach (Line drawing in drawingsList)
            {
                machCanvas.Children.Remove(drawing);
                machCanvas1.Children.Remove(drawing);
            }
            drawingsList.Clear();
        }
        private void axisSettings0_Click(object sender, RoutedEventArgs e)
        {
            AxisScopeWindow axisScopeWindow = new AxisScopeWindow(chart0);
            axisScopeWindow.Show();
        }
        private void axisSettings1_Click(object sender, RoutedEventArgs e)
        {
            AxisScopeWindow axisScopeWindow = new AxisScopeWindow(chart1);
            axisScopeWindow.Show();
        }

        //Menu items:
        private void defaultParameters_Click(object sender, RoutedEventArgs e)
        {
            DefaultParametersWindow defaultParametersWindow = new DefaultParametersWindow(this);
            defaultParametersWindow.Show();
        }
        private void about_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow aboutWindow = new AboutWindow();
            aboutWindow.Show();
        }
        private void help_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(@"MachPlotAsynchroHelp.pdf");
            }
            catch
            {
                MessageBox.Show("Nie odnaleziono pliku pomocy");
            }
        }
        private void saveTo_Click(object sender, RoutedEventArgs e)
        {
            chart0.saveToPNG();            
        }
        private void reset_Click(object sender, RoutedEventArgs e)
        {
            reset();
        }
        public void reset()
        {
            seriesFunPairs.Clear();
            parameters = Parameters.ReadXml();
            chart0.reset();
            chart1.reset();
            setSliders();

            startXtextBox.Text = Convert.ToString(parameters.startX);
            endXtextBox.Text = Convert.ToString(parameters.endX);

            addButton.IsEnabled = true;
            suspendSliders = true;

            startXtextBox.Text = "";
            endXtextBox.Text = "";

            statusBarXCursorPosition.Text = "";
            statusBarY0CursorPosition.Text = "";
            statusBarY1CursorPosition.Text = "";
            statusBarY2CursorPosition.Text = "";
            statusBarY3CursorPosition.Text = "";
            statusBarValues.Text = "";

            statusBarXCursorPosition2.Text = "";
            statusBarY0CursorPosition2.Text = "";
            statusBarY1CursorPosition2.Text = "";
            statusBarY2CursorPosition2.Text = "";
            statusBarY3CursorPosition2.Text = "";
            statusBarValues2.Text = "";

            comboBox.SelectedIndex = -1;

            U1tofRatio      =   null;
            U1tofRatiomin   =   null;
            U1tofRatiomax   =   null;

            U1tofcheckBox.IsChecked = false;
            U1tofSetValuetextBox.Text = "";
            U1tofActualValuetextBox.Text = "";

            SeriesMatrix.refresh();
        }
        private void run_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(@"MachPlotStart.exe");
            }
            catch
            {
                MessageBox.Show("Nie odnaleziono modułu startowego");
            }
        }
        private void exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }              
        private void generateInSecond_Click(object sender, RoutedEventArgs e)
        {
            addNewSeries(chart1);
        }       
        private void intersection_Click(object sender, RoutedEventArgs e)
        {   
            if(listBox.SelectedItems.Count >= 2)
            {
                SeriesFunPair selectedPair;
                List<SeriesFunPair> tempList = new List<SeriesFunPair>();
                for (int i = 0; i < 2; i++)
                {
                    selectedPair = listBox.SelectedItems[i] as SeriesFunPair;
                    tempList.Add(selectedPair);
                }

                IntersectionWindow intersectionWindow = new IntersectionWindow(tempList);
                intersectionWindow.Show();
            }
            else
            {
                MessageBox.Show("Należy zaznaczyć dwa przebiegi!");
            }
        }

        //U1 to f = const
        private double? U1tofRatio      =   null;
        private double? U1tofRatiomin   =   null;
        private double? U1tofRatiomax   =   null;
        private void U1tofcheckBoxChecked(object sender, RoutedEventArgs e)
        {
            U1tofRatio = sliderU1.Value / sliderf.Value;
            U1tofRatiomin = parameters.U1min/parameters.fmax;
            U1tofRatiomax = parameters.U1max / parameters.fmin;
            U1tofSetValuetextBox.IsEnabled = true;
            U1tofSetValuetextBox.Text = Convert.ToString(U1tofRatio);
        }
        private void U1tofcheckBoxUnChecked(object sender, RoutedEventArgs e)
        {
            U1tofRatio = null;
            U1tofSetValuetextBox.IsEnabled = false;
            U1tofSetValuetextBox.Text = "";
        }
        private void U1tof_Click(object sender, RoutedEventArgs e)
        {
            if (U1tofStackPanel.Visibility == System.Windows.Visibility.Visible)
                U1tofStackPanel.Visibility = System.Windows.Visibility.Collapsed;
            else if (U1tofStackPanel.Visibility == System.Windows.Visibility.Collapsed)
                U1tofStackPanel.Visibility = System.Windows.Visibility.Visible;
        }
        private void U1toftextBox_OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                double result;
                if (Double.TryParse((sender as TextBox).Text, out result))
                {
                    U1tofSetValuetextBox.Background = Brushes.SpringGreen;

                    if (result < U1tofRatiomin)
                    {
                        U1tofRatio = U1tofRatiomin;
                        U1tofSetValuetextBox.Text = U1tofRatio.ToString();
                    }
                    else if (result > U1tofRatiomax)
                    {
                        U1tofRatio = U1tofRatiomax;
                        U1tofSetValuetextBox.Text = U1tofRatio.ToString();
                    }
                    else
                    {
                        U1tofRatio = result;
                    }
                }
                else
                {
                    U1tofSetValuetextBox.Background = Brushes.Salmon;
                    U1tofRatio = null;
                }
            }
        }
        private void U1toftextBox_TextChangedHandler(object sender, TextChangedEventArgs e)
        {
            double result;
            if (Double.TryParse((sender as TextBox).Text, out result))
            {
                U1tofSetValuetextBox.Background = Brushes.SpringGreen;

                if (result < U1tofRatiomin)
                {
                    U1tofRatio = U1tofRatiomin;
                    U1tofSetValuetextBox.Text = U1tofRatio.ToString();
                }
                else if (result > U1tofRatiomax)
                {
                    U1tofRatio = U1tofRatiomax;
                    U1tofSetValuetextBox.Text = U1tofRatio.ToString();
                }
                else
                {
                    U1tofRatio = result;
                }
            }
            else
            {
                U1tofSetValuetextBox.Background = Brushes.Salmon;
                U1tofRatio = null;
            }
        }

        private void sliderTextBox_textChanged(object sender, TextChangedEventArgs e)
        {
            double result;
            if (Double.TryParse((sender as TextBox).Text, out result))
            {
                (sender as TextBox).Background = Brushes.SpringGreen;
            }
            else
            {
                (sender as TextBox).Background = Brushes.Salmon;
            }
        }

    }

    public class SeriesMatrix
    {
        public SeriesMatrix(string r_name, Brush r_color, bool r_isUsed)
        {
            color = r_color;
            name = r_name;
            isUsed = r_isUsed;
        }

        public Brush color;
        public string name;
        public bool isUsed;

        public static double startX;
        public static double endX;
        public static List<SeriesMatrix> collection = new List<SeriesMatrix>();
        public static void refresh()
        {
            collection.Clear();
            collection.Add(new SeriesMatrix("1", Brushes.Black, false));
            collection.Add(new SeriesMatrix("2", Brushes.Green, false));
            collection.Add(new SeriesMatrix("3", Brushes.Orange, false));
            collection.Add(new SeriesMatrix("4", Brushes.Purple, false));
            collection.Add(new SeriesMatrix("5", Brushes.Red, false));
            collection.Add(new SeriesMatrix("6", Brushes.Blue, false));
            collection.Add(new SeriesMatrix("7", Brushes.DarkCyan, false));
            collection.Add(new SeriesMatrix("8", Brushes.MediumVioletRed, false));

            Parameters parameters = Parameters.ReadXml();
            SeriesMatrix.startX = parameters.startX;
            SeriesMatrix.endX = parameters.endX;
        }
    }

    public class SeriesFunPair
    {
        public SeriesFunPair(MachChart.MachSeries r_series, MachModel r_model)
        {
            series = r_series;
            corelatedFun = r_model;

            //for listBox databinding:
            ID = series.ID;
            color = series.color;
        }

        public MachChart.MachSeries series { get; set; }
        public MachModel corelatedFun { get; set; }

        public string ID { get; set; }
        public Brush color { get; set; }
    }

    public class Parameters
    {
        public static string filePath = @"asynchro.xml";

        //Parameters for load machines:
        public double aFan { get; set; }
        public double a2Fan { get; set; }
        public double bFan { get; set; }
        public double cFan { get; set; }

        public double cCrane { get; set; }

        public double aConveyor { get; set; }
        public double bConveyor { get; set; }
        public double cConveyor { get; set; }

        public double aGenerator { get; set; }
        public double bGenerator { get; set; }
        public double cGenerator { get; set; }

        //Parameters for asynchronous machine:
        public double U1 { get; set; }
        public double f { get; set; }
        public double polePairs { get; set; }
        public double R1 { get; set; }
        public double R2 { get; set; }
        public double L1 { get; set; }
        public double L2 { get; set; }
        public double Lm { get; set; }
        public double Rfe { get; set; }
        public double turnsRatio { get; set; }

        public double U1max { get; set; }
        public double U1min { get; set; }
        public double fmax { get; set; }
        public double fmin { get; set; }
        public double R1max { get; set; }
        public double R1min { get; set; }
        public double R2max { get; set; }
        public double R2min { get; set; }
        public double L1max { get; set; }
        public double L1min { get; set; }
        public double L2max { get; set; }
        public double L2min { get; set; }
        public double Lmmax { get; set; }
        public double Lmmin { get; set; }
        public double Rfemax { get; set; }
        public double Rfemin { get; set; }

        //Basics calculation parameters:
        public double step { get; set; } //calculation step
        public double startX { get; set; }//here calculation start
        public double endX { get; set; } //here calculation end

        public void SaveXml()
        {
            System.Xml.Serialization.XmlSerializer writer =
                new System.Xml.Serialization.XmlSerializer(typeof(Parameters));
            System.IO.StreamWriter file = new System.IO.StreamWriter(
                filePath);
            writer.Serialize(file, this);
            file.Close();
        }
        public static Parameters ReadXml()
        {
            System.Xml.Serialization.XmlSerializer reader =
                new System.Xml.Serialization.XmlSerializer(typeof(Parameters));
            using (System.IO.StreamReader file = new System.IO.StreamReader(filePath))
            {
                return (Parameters)reader.Deserialize(file);
            }
        }
    }
}

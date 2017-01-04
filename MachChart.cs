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
using System.IO;
using Microsoft.Win32;

namespace MachPlotNamespace
{
    public class MachChart
    {   

        //INTERFACE:
        public MachChart(Canvas r_containerCanvas)
        {
            containerCanvas = r_containerCanvas;
            machCanvas = containerCanvas.Children[0] as Canvas;
            machCanvasBehind = containerCanvas.Children[1] as Canvas;

            axisYOList.Add(new MachAxisYO(this, true,  "axisY0LabelStyle", "axisY0DescriptionStyle"));
            axisYOList.Add(new MachAxisYO(this, false, "axisY1LabelStyle", "axisY1DescriptionStyle"));
            axisYOList.Add(new MachAxisYO(this, true, "axisY2LabelStyle", "axisY2DescriptionStyle"));
            axisYOList.Add(new MachAxisYO(this, false, "axisY3LabelStyle", "axisY3DescriptionStyle"));

            axisXO = new MachAxisXO(this, "axisXLabelStyle", "axisY0DescriptionStyle");


            setAdjustToDefault();
            reset();
        }
       
        public void reset()
        {
            machCanvas.Children.Clear();
            machCanvasBehind.Children.Clear();
            seriesList.Clear();

            //setAdjustToDefault();

            renderLines();
            axisXO.renderLabels();
            axisXO.renderDescription();

            for(int i=0; i<axisYOList.Count; i++)
            {
                axisYOList[i].isUsed = null;
                axisYOList[i].renderLabels();
                axisYOList[i].renderDescription();
                axisYOList[i].visibility = Visibility.Hidden;
                axisYOList[i].manualScope = false;
            }
        }
        public void saveToPNG()
        {
            RenderTargetBitmap renderBitmap = new RenderTargetBitmap(
             (int)(1.2 * containerCanvas.ActualWidth), (int)(1.2 * containerCanvas.ActualHeight),
             96d, 96d, PixelFormats.Pbgra32);

            containerCanvas.Measure(new Size((int)containerCanvas.Width, (int)containerCanvas.Height));
            containerCanvas.Arrange(new Rect(new Size((int)containerCanvas.Width, (int)containerCanvas.Height)));

            renderBitmap.Render(containerCanvas);

            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(renderBitmap));

            SaveFileDialog dialog = new SaveFileDialog()
            {
                Filter = "Graphic Files(*.png)|*.png|All(*.*)|*"
            };

            if (dialog.ShowDialog() == true)
            {
                using (FileStream file = File.Create(dialog.FileName))
                {
                    encoder.Save(file);
                }
            }
        }

        //Canvas used by this Plot:
        public Canvas containerCanvas   { get; private set; } //read only
        public Canvas machCanvas        { get; private set; } //read only
        public Canvas machCanvasBehind  { get; private set; } //read only        

        public uint horizontalDivisionCount = 6;
        public uint verticalDivisionCount = 6;

        //Axis specific methods:
        public bool     YO_Get_ZeroPoint(int r_index)
        {
            return axisYOList[r_index].isZeroPointActive;
        }
        public void     YO_Set_ZeroPoint(int r_index, bool r_value)
        {
            if(axisYOList[r_index].isUsed != null)
            axisYOList[r_index].isZeroPointActive = r_value;
        }
        public void     YO_Toggle_ZeroPoint(int r_index)
        {
            if (axisYOList[r_index].isUsed != null)
            {
                if (axisYOList[r_index].isZeroPointActive == true) axisYOList[r_index].isZeroPointActive = false;
                else axisYOList[r_index].isZeroPointActive = true;
            }
        }
        public string   YO_Get_Description(int r_index)
        {
            return axisYOList[r_index].description.Text;
        }//read only
        public Point    YO_Get_CursorPosition(int r_index)
        {
            return axisYOList[r_index].cursorPosition;
        }//read only
        public double   YO_Get_GlobalMin(int r_index)
        {
            return axisYOList[r_index].globalMin;
        }
        public void     YO_Set_GlobalMin(int r_index, double r_value)
        {
            axisYOList[r_index].globalMin = r_value;
            axisYOList[r_index].globalRange = axisYOList[r_index].globalMax - axisYOList[r_index].globalMin;
            foreach (MachSeries series in seriesList) series.update();
        }
        public double   YO_Get_GlobalMax(int r_index)
        {
            return axisYOList[r_index].globalMax;
        }
        public void     YO_Set_GlobalMax(int r_index, double r_value)
        {
            axisYOList[r_index].globalMax = r_value;
            axisYOList[r_index].globalRange = axisYOList[r_index].globalMax - axisYOList[r_index].globalMin;
            foreach (MachSeries series in seriesList) series.update();
        }
        public double   YO_Get_GlobalRange(int r_index)
        {
            return axisYOList[r_index].globalRange;
        }
        public int      YO_Get_RoundTo(int r_index, int r_value)
        {
            return axisYOList[r_index].roundTo;
        }
        public void     YO_Set_RoundTo(int r_index, int r_value)
        {
            axisYOList[r_index].roundTo = r_value;
        }
        public bool     YO_Get_ManualScope(int r_index)
        {
            return axisYOList[r_index].manualScope;
        }
        public void     YO_Set_ManualScope(int r_index, bool r_value)
        {
             axisYOList[r_index].manualScope = r_value;
        }
        public void     YO_Toggle_ManualScope(int r_index)
        {
            axisYOList[r_index].manualScope = !axisYOList[r_index].manualScope;
        }
        public bool     YO_Get_Visibility(int r_index)
        {
                if (axisYOList[r_index].visibility == Visibility.Visible) return true;
                else return false;
        }
        public void     YO_Set_Visibility(int r_index, bool r_value)
        {
            if(r_value == true) axisYOList[r_index].visibility = Visibility.Visible;
            else axisYOList[r_index].visibility = Visibility.Hidden;
        }
        public void     YO_Toggle_Visibility(int r_index)
        {
            if (axisYOList[r_index].visibility == Visibility.Visible) axisYOList[r_index].visibility = Visibility.Hidden;
            else axisYOList[r_index].visibility = Visibility.Visible;
        }
        public double   YO_Get_LabelsVerticalAdjust(int r_index)
        {
            return axisYOList[r_index].labelsVerticalAdjust;
        }
        public void     YO_Set_LabelsVerticalAdjust(int r_index, double r_value)
        {
            try
            {
                axisYOList[r_index].labelsVerticalAdjust = r_value;
            }
            catch { }
        }
        public double   YO_Get_LabelsHorizontalAdjust(int r_index)
       {
           return axisYOList[r_index].labelsHorizontalAdjust;
       }
        public void     YO_Set_LabelsHorizontalAdjust(int r_index, double r_value)
        {
            axisYOList[r_index].labelsHorizontalAdjust = r_value;
        }
        public double   YO_Get_LabelsRotateAngle(int r_index)
       {
           return axisYOList[r_index].labelsRotateAngle;
       }
        public void     YO_Set_LabelsRotateAngle(int r_index, double r_value)
        {
            axisYOList[r_index].labelsRotateAngle = r_value;
        }
        public double   YO_Get_DescriptionVerticalAdjust(int r_index)
       {
           return axisYOList[r_index].descriptionVerticalAdjust;
       }
        public void     YO_Set_DescriptionVerticalAdjust(int r_index, double r_value)
        {
            axisYOList[r_index].descriptionVerticalAdjust = r_value;
        }
        public double   YO_Get_DescriptionHorizontalAdjust(int r_index)
       {
           return axisYOList[r_index].descriptionHorizontalAdjust;
       }
        public void     YO_Set_DescriptionHorizontalAdjust(int r_index, double r_value)
        {
            axisYOList[r_index].descriptionHorizontalAdjust = r_value;
        }
        public double   YO_Get_DescriptionRotateAngle(int r_index)
       {
           return axisYOList[r_index].descriptionRotateAngle;
       }
        public void     YO_Set_DescriptionRotateAngle(int r_index, double r_value)
        {
            axisYOList[r_index].descriptionRotateAngle = r_value;
        }

        public string   XO_Get_Description()
        {
                return axisXO.description.Text;
        }//read only
        public double   XO_Get_GlobalMin()
        {
                return axisXO.globalMin;
        }
        public void     XO_Set_GlobalMin(double r_value)
        {
            axisXO.globalMin = r_value;
            axisXO.globalRange = axisXO.globalMax - axisXO.globalMin;
        }
        public double   XO_Get_GlobalMax()
        {
                return axisXO.globalMax;
        }
        public void     XO_Set_GlobalMax(double r_value)
        {
            axisXO.globalMax = r_value;
            axisXO.globalRange = axisXO.globalMax - axisXO.globalMin;
        }
        public double   XO_Get_GlobalRange()
        {
                return axisXO.globalRange;
        }
        public int      XO_Get_RoundTo(int r_value)
        {
                return axisXO.roundTo;
        }
        public void     XO_Set_RoundTo(int r_value)
        {
            axisXO.roundTo = r_value;
        }
        public bool     XO_Get_Visibility()
        {
            if (axisXO.visibility == Visibility.Visible) return true;
            else return false;
        }
        public void     XO_Set_Visibility(bool r_value)
        {
            if (r_value == true) axisXO.visibility = Visibility.Visible;
            else axisXO.visibility = Visibility.Hidden;
        }
        public void     XO_Toggle_Visibility()
        {
            if (axisXO.visibility == Visibility.Visible)
            {
                axisXO.visibility = Visibility.Hidden;
                System.Console.WriteLine("ukryte");
            }
            else if (axisXO.visibility == Visibility.Hidden)
            {
                axisXO.visibility = Visibility.Visible;
                System.Console.WriteLine("widialne");
            }
        }
        public double   XO_Get_LabelsVerticalAdjust()
       {
           return axisXO.labelsVerticalAdjust;
       }
        public void     XO_Set_LabelsVerticalAdjust(double r_value)
        {
            axisXO.labelsVerticalAdjust = r_value;
        }
        public double   XO_Get_LabelsHorizontalAdjust()
       {
           return axisXO.labelsHorizontalAdjust;
       }
        public void     XO_Set_LabelsHorizontalAdjust(double r_value)
        {
            axisXO.labelsHorizontalAdjust = r_value;
        }
        public double   XO_Get_LabelsRotateAngle()
       {
           return axisXO.labelsRotateAngle;
       }
        public void     XO_Set_LabelsRotateAngle(double r_value)
        {
            axisXO.labelsRotateAngle = r_value;
        }
        public double   XO_Get_DescriptionVerticalAdjust()
       {
           return axisXO.descriptionVerticalAdjust;
       }
        public void     XO_Set_DescriptionVerticalAdjust(double r_value)
        {
            axisXO.descriptionVerticalAdjust = r_value;
            Canvas.SetTop(axisXO.description, Canvas.GetTop(axisXO.description) + r_value);
        }
        public double   XO_Get_DescriptionHorizontalAdjust()
       {
           return axisXO.descriptionHorizontalAdjust;
       }
        public void     XO_Set_DescriptionHorizontalAdjust(double r_value)
        {
            axisXO.descriptionHorizontalAdjust = r_value;
            Canvas.SetLeft(axisXO.description, Canvas.GetLeft(axisXO.description) + r_value);
        }
        public double   XO_Get_DescriptionRotateAngle()
       {
           return axisXO.descriptionRotateAngle;
       }
        public void     XO_Set_DescriptionRotateAngle(double r_value)
        {
            axisXO.descriptionRotateAngle = r_value;
        }


        private void setAdjustToDefault()
        {
            axisYOList[0].labelsVerticalAdjust = 0;
            axisYOList[0].labelsHorizontalAdjust = -35;
            axisYOList[0].labelsRotateAngle = 180;
            axisYOList[0].descriptionVerticalAdjust = 20;
            axisYOList[0].descriptionHorizontalAdjust = -50;
            axisYOList[0].descriptionRotateAngle = 180;

            axisYOList[1].labelsVerticalAdjust = 0;
            axisYOList[1].labelsHorizontalAdjust = 0;
            axisYOList[1].labelsRotateAngle = 180;
            axisYOList[1].descriptionVerticalAdjust = 20;
            axisYOList[1].descriptionHorizontalAdjust = 10;
            axisYOList[1].descriptionRotateAngle = 180;

            axisYOList[2].labelsVerticalAdjust = 13;
            axisYOList[2].labelsHorizontalAdjust = 0;
            axisYOList[2].labelsRotateAngle = 180;
            axisYOList[2].descriptionVerticalAdjust = 30;
            axisYOList[2].descriptionHorizontalAdjust = -10;
            axisYOList[2].descriptionRotateAngle = 180;

            axisYOList[3].labelsVerticalAdjust = 10;
            axisYOList[3].labelsHorizontalAdjust = -30;
            axisYOList[3].labelsRotateAngle = 180;
            axisYOList[3].descriptionVerticalAdjust = 30;
            axisYOList[3].descriptionHorizontalAdjust = -30;
            axisYOList[3].descriptionRotateAngle = 180;



            axisXO.labelsVerticalAdjust = -21;
            axisXO.labelsHorizontalAdjust = -10;
            axisXO.labelsRotateAngle = 180;
            axisXO.descriptionVerticalAdjust = -30;
            axisXO.descriptionHorizontalAdjust = -40;
            axisXO.descriptionRotateAngle = 180;
        }
        private void renderLines()
        {
            //Horizontal Lines:
            Style horizontalLineStyle = Application.Current.FindResource("horizontalLineStyle") as Style;
            Line horizontalLine;
            for (int i = 1; i <= horizontalDivisionCount; i++)
            {
                horizontalLine = new Line();
                horizontalLine.Style = horizontalLineStyle;
                horizontalLine.X1 = machCanvas.Width;
                horizontalLine.X2 = 0;
                horizontalLine.Y1 = i * (machCanvas.Height / horizontalDivisionCount);
                horizontalLine.Y2 = i * (machCanvas.Height / horizontalDivisionCount);
                machCanvas.Children.Add(horizontalLine);
            }

            //Vertical Lines:
            Style verticallLineStyle = Application.Current.FindResource("verticalLineStyle") as Style;
            Line verticalLine;
            for (int i = 1; i <= verticalDivisionCount; i++)
            {
                verticalLine = new Line();
                verticalLine.Style = verticallLineStyle;
                verticalLine.X1 = i * (machCanvas.Width / verticalDivisionCount);
                verticalLine.X2 = i * (machCanvas.Width / verticalDivisionCount);
                verticalLine.Y1 = machCanvas.Height;
                verticalLine.Y2 = 0;
                machCanvas.Children.Add(verticalLine);
            }

            //Axis X Line:
            Style axisXlLineStyle = Application.Current.FindResource("axisXLineStyle") as Style;
            Line axisXLine = new Line();
            axisXLine.Style = axisXlLineStyle;
            axisXLine.X1 = machCanvas.Width;
            axisXLine.X2 = 0;
            axisXLine.Y1 = 0;
            axisXLine.Y2 = 0;
            machCanvas.Children.Add(axisXLine);

            //Axis Y Lines:
            Style axisYLineStyle = Application.Current.FindResource("axisYLineStyle") as Style;
            Line axisYLine1 = new Line();
            axisYLine1.Style = axisYLineStyle;
            axisYLine1.X1 = 0;
            axisYLine1.X2 = 0;
            axisYLine1.Y1 = machCanvas.Height;
            axisYLine1.Y2 = 0;
            machCanvas.Children.Add(axisYLine1);

            Line axisYLine2 = new Line();
            axisYLine2.Style = axisYLineStyle;
            axisYLine2.X1 = machCanvas.Width;
            axisYLine2.X2 = machCanvas.Width;
            axisYLine2.Y1 = machCanvas.Height;
            axisYLine2.Y2 = 0;
            machCanvas.Children.Add(axisYLine2);
        }
        private int whichAxis;//helper variable for setValuesToAxisLabels()
        private void setValuesToAxisLabels(string r_axisXDescription, string r_axisYDescription)
        {
            double axisXStep = (axisXO.globalMin - axisXO.globalMax) / verticalDivisionCount;
            for (int i = 0; i <= verticalDivisionCount; i++)
            {
                axisXO.labelCollection[i].Content = Convert.ToString(Math.Round(axisXO.globalMax + i * axisXStep, axisXO.roundTo));
            }
            axisXO.description.Text = r_axisXDescription;

            whichAxis = axisYOList.FindIndex(element => element.isUsed == r_axisYDescription);
            double axisYStep = (axisYOList[whichAxis].globalMax - axisYOList[whichAxis].globalMin) / horizontalDivisionCount;
            for (int i = 0; i <= horizontalDivisionCount; i++)
            {
                axisYOList[whichAxis].labelCollection[i].Content =
                Convert.ToString(Math.Round(axisYOList[whichAxis].globalMin + i * axisYStep, axisYOList[whichAxis].roundTo));
                axisYOList[whichAxis].description.Text = r_axisYDescription;
            }
        }
        private Point calculateDisplayCords(Point r_DataPoint, int r_whichAxis)
        {
            return new Point(
                    ((r_DataPoint.X - axisXO.globalMin) / axisXO.globalRange) * machCanvas.Width,
                    ((r_DataPoint.Y - axisYOList[r_whichAxis].globalMin) /
                      axisYOList[r_whichAxis].globalRange) * machCanvas.Height
                    );
        }
        //calculating global values - helper function for updateSeries():

        public List<MachSeries> seriesList = new List<MachSeries>();
        private List<MachAxisYO> axisYOList = new List<MachAxisYO>();
        private MachAxisXO axisXO;


        //
        //NESTES CLASS: MachSeries
        //
        public class MachSeries
        {
            public MachSeries(string r_id,
                              string r_axisXDescription,
                              string r_axisYDescription,
                              Brush r_colour,
                              MachChart r_plot,
                              List<Point> r_dataPointList)
            {
                ID = r_id;
                axisXDescription = r_axisXDescription;
                axisYDescription = r_axisYDescription;
                color = r_colour;
                chart = r_plot;
                canvas = chart.machCanvas;
                dataPointList = r_dataPointList;

                polyline = new Polyline();
                displayPointCollection = new PointCollection();
                polyline.Stroke = color;
                polyline.StrokeThickness = 1.5;
                polyline.FillRule = FillRule.EvenOdd;
                polyline.Points = displayPointCollection;
                chart.machCanvas.Children.Add(polyline);

                //associating created series with its axis (Y0,Y1,Y2,Y3)
                if (!chart.axisYOList.Any(element => element.isUsed == axisYDescription))
                {
                    foreach (MachAxisYO axis in chart.axisYOList)
                    {
                        if (axis.isUsed == null)
                        {
                            axis.isUsed = axisYDescription;
                            break;
                        }
                    }
                }

                chart.seriesList.Add(this);

                update(r_dataPointList);
            }

            public void update(List<Point> r_dataPointList)
            {
                dataPointList = r_dataPointList;
                updateBase();
            } 
            public void update()
            {
                updateBase();
            }
            private void updateBase()
            {
                if (chart.seriesList.Count > 0)
                {
                    setGlobals();
                    foreach (MachSeries series in chart.seriesList)
                    {
                        series.displayPointCollection.Clear();
                        whichAxis = chart.axisYOList.FindIndex(element => element.isUsed == series.axisYDescription);
                        foreach (Point dataPoint in series.dataPointList)
                        {
                            series.displayPointCollection.Add(chart.calculateDisplayCords(dataPoint, whichAxis));
                        }

                        if (chart.axisYOList[whichAxis].isZeroPointActive) chart.axisYOList[whichAxis].createZeroPoint();

                    }
                    chart.setValuesToAxisLabels(axisXDescription, axisYDescription);  
                }               
            }

            //Proper and the only way of removing series:
            public void dispose()
            {
                int i = 0;
                foreach (MachSeries series in chart.seriesList)
                    if (series.axisYDescription == axisYDescription) i++;
                if (i < 2) chart.axisYOList[chart.axisYOList.FindIndex(element => element.isUsed == axisYDescription)].isUsed = null;

                canvas.Children.Remove(polyline);
                chart.seriesList.Remove(this);
            }

            public string ID { get; set; }
            public string axisXDescription { get; set; }
            public string axisYDescription { get; set; }
            public Brush color { get; set; }
            public MachChart chart { get; private set; } //read only
            public Canvas canvas { get; private set; } //read only
            public List<Point> dataPointList { get; private set; } //read only
            public Polyline polyline { get; private set; } //read only
            public PointCollection displayPointCollection { get; private set; } //read only

            //calculates canvas position for a DataPoint - helper function for updateSeries():

            private int whichAxis;
            private void setGlobals()
            {               
                chart.axisXO.globalMin = findGlobalMinX();
                chart.axisXO.globalMax = findGlobalMaxX();
                chart.axisXO.globalRange = Math.Abs(chart.axisXO.globalMax - chart.axisXO.globalMin);
                
                whichAxis = chart.axisYOList.FindIndex(element => element.isUsed == axisYDescription);
                if (chart.axisYOList[whichAxis].manualScope == false)
                {
                    chart.axisYOList[whichAxis].globalMin = findGlobalMinY();
                    chart.axisYOList[whichAxis].globalMax = findGlobalMaxY();
                    chart.axisYOList[whichAxis].globalRange = Math.Abs(chart.axisYOList[whichAxis].globalMax - chart.axisYOList[whichAxis].globalMin);
                }
            }
            private List<MachSeries> temporarySeriesList = new List<MachSeries>();
            private double findGlobalMinX()
            {
                temporarySeriesList.Clear();
                foreach (MachSeries series in chart.seriesList)
                {
                    if (series.axisXDescription == axisXDescription) temporarySeriesList.Add(series);
                }
                return temporarySeriesList.Min(element => element.dataPointList.Min(element2 => element2.X));
            } //helper function for setGlobals()
            private double findGlobalMaxX()
            {
                temporarySeriesList.Clear();
                foreach (MachSeries series in chart.seriesList)
                {
                    if (series.axisXDescription == axisXDescription) temporarySeriesList.Add(series);
                }
                return temporarySeriesList.Max(element => element.dataPointList.Max(element2 => element2.X));
            } //helper function for setGlobals()
            private double findGlobalMinY()
            {
                temporarySeriesList.Clear();
                foreach (MachSeries series in chart.seriesList)
                {
                    if (series.axisYDescription == axisYDescription) temporarySeriesList.Add(series);
                }
                return temporarySeriesList.Min(element => element.dataPointList.Min(element2 => element2.Y));
            } //helper function for setGlobals()
            private double findGlobalMaxY()
            {
                temporarySeriesList.Clear();
                foreach (MachSeries series in chart.seriesList)
                {
                    if (series.axisYDescription == axisYDescription) temporarySeriesList.Add(series);
                }
                return temporarySeriesList.Max(element => element.dataPointList.Max(element2 => element2.Y));
            } //helper function for setGlobals()
        }

        //
        //NESTES CLASS: MachAxisYO
        //
        public class MachAxisYO : MachAxis
        {
            public MachAxisYO(MachChart r_chart, bool r_isLeftSide, string r_LabelStyle, string r_DescriptionStyle)
            {
                chart = r_chart;
                machCanvasBehind = chart.machCanvasBehind;
                isLeftSide = r_isLeftSide;
                labelStyle = r_LabelStyle;
                descriptionStyle = r_DescriptionStyle;

                visibility = Visibility.Visible;
            }

            public bool manualScope = false;
            public bool isLeftSide;
            public string isUsed
            {
                get { return _isUsed; }
                set
                {
                    _isUsed = value;
                    if (_isUsed != null) visibility = Visibility.Visible;
                    else visibility = Visibility.Hidden;
                }
            }

            public void renderLabels()
            {
                description.Text = "";
                labelCollection.Clear();

                //loading style and performing necessary transform:
                Style axisLabelStyle = Application.Current.FindResource(labelStyle) as Style;
                TransformGroup transformGroup = transformAxisElement(labelsRotateAngle);

                double baseCanvasWidth;
                if (isLeftSide == true) baseCanvasWidth = machCanvasBehind.Width;
                else baseCanvasWidth = 0;

                Label axisYLabel;
                for (int i = 0; i <= chart.horizontalDivisionCount; i++)
                {
                    axisYLabel = new Label();
                    axisYLabel.Style = axisLabelStyle;
                    axisYLabel.SetValue(Canvas.RightProperty, baseCanvasWidth + labelsHorizontalAdjust);
                    axisYLabel.SetValue(Canvas.TopProperty, i * machCanvasBehind.Height / chart.horizontalDivisionCount + labelsVerticalAdjust);
                    axisYLabel.RenderTransformOrigin = new Point(0.5, 0.5);
                    axisYLabel.RenderTransform = transformGroup;
                    labelCollection.Add(axisYLabel);
                    machCanvasBehind.Children.Add(axisYLabel);
                }
            }
            public void renderDescription()
            {
                Style style = Application.Current.FindResource(descriptionStyle) as Style;
                TransformGroup transformGroup = transformAxisElement(descriptionRotateAngle);

                double baseCanvasWidth;
                if (isLeftSide == true) baseCanvasWidth = machCanvasBehind.Width;
                else baseCanvasWidth = 0;

                description.Style = style;
                description.SetValue(Canvas.RightProperty, baseCanvasWidth + descriptionHorizontalAdjust);
                description.SetValue(Canvas.TopProperty, machCanvasBehind.Height + descriptionVerticalAdjust);
                description.RenderTransformOrigin = new Point(0.5, 0.5);

                description.RenderTransform = transformGroup;
                machCanvasBehind.Children.Add(description);
            }

            public Point cursorPosition
            {
                get
                {
                    return new Point(
                    ((chart.axisXO.globalRange * Mouse.GetPosition(chart.machCanvas).X) / chart.machCanvas.Width) + chart.axisXO.globalMin,
                    ((globalRange * Mouse.GetPosition(chart.machCanvas).Y) / chart.machCanvas.Height) + globalMin
                    );
                }
                private set { }
            } //read only


            public bool isZeroPointActive
            {
                get { return _isZeroPointActive; }
                set 
                { 
                    if(value == true)
                    {
                        _isZeroPointActive = value;
                        createZeroPoint();
                    }
                    else
                    {
                        _isZeroPointActive = value;
                        removeZeroPoint();
                    }
                }
            }
            public Line zeroPointHorizontalLine;
            public Line zeroPointVerticalLine;
            public void createZeroPoint()
            {
                //isZeroPointActive = true;

                chart.machCanvas.Children.Remove(zeroPointHorizontalLine);
                chart.machCanvas.Children.Remove(zeroPointVerticalLine);

                zeroPointHorizontalLine = new Line();
                zeroPointVerticalLine = new Line();
                zeroPointHorizontalLine.Stroke = Brushes.Black;
                zeroPointHorizontalLine.StrokeThickness = 2;
                zeroPointVerticalLine.Stroke = Brushes.Black;
                zeroPointVerticalLine.StrokeThickness = 2;

                int whichAxis = chart.axisYOList.IndexOf(this);

                zeroPointHorizontalLine.X1 = 0;
                zeroPointHorizontalLine.X2 = machCanvasBehind.Width;
                zeroPointHorizontalLine.Y1 = chart.calculateDisplayCords(new Point(0, 0), whichAxis).Y;
                zeroPointHorizontalLine.Y2 = chart.calculateDisplayCords(new Point(0, 0), whichAxis).Y;

                zeroPointVerticalLine.X1 = chart.calculateDisplayCords(new Point(0, 0), whichAxis).X;
                zeroPointVerticalLine.X2 = chart.calculateDisplayCords(new Point(0, 0), whichAxis).X;
                zeroPointVerticalLine.Y1 = 0;
                zeroPointVerticalLine.Y2 = machCanvasBehind.Height;

                chart.machCanvas.Children.Add(zeroPointHorizontalLine);
                chart.machCanvas.Children.Add(zeroPointVerticalLine);
                //updateZeroPoint();
            }
            public void removeZeroPoint()
            {
                //isZeroPointActive = false;

                chart.machCanvas.Children.Remove(zeroPointHorizontalLine);
                chart.machCanvas.Children.Remove(zeroPointVerticalLine);
                zeroPointHorizontalLine = null;
                zeroPointVerticalLine = null;
            }
            public void updateZeroPoint()
            {
                int whichAxis = chart.axisYOList.IndexOf(this);

                Canvas.SetTop(zeroPointHorizontalLine, chart.calculateDisplayCords(new Point(0, 0), whichAxis).Y);

                Canvas.SetLeft(zeroPointVerticalLine, chart.calculateDisplayCords(new Point(0, 0), whichAxis).X);

            }

            private string _isUsed = null;
            private bool _isZeroPointActive = false;
        }

        //
        //NESTES CLASS: MachAxisXO
        //
        public class MachAxisXO : MachAxis
        {
            public MachAxisXO(MachChart r_chart, string r_LabelStyle, string r_DescriptionStyle)
            {
                chart = r_chart;
                machCanvasBehind = chart.machCanvasBehind;
                labelStyle = r_LabelStyle;
                descriptionStyle = r_DescriptionStyle;

                visibility = Visibility.Visible;
            }

            public void renderLabels()
            {
                description.Text = null;
                labelCollection.Clear();
                baseLabelsPosition.Clear();

                //loading style and performing necessary transform:
                Style axisLabelStyle = Application.Current.FindResource(labelStyle) as Style;
                TransformGroup transformGroup = transformAxisElement(labelsRotateAngle);

                Label axisLabel;
                for (int i = 0; i <= chart.verticalDivisionCount; i++)
                {
                    axisLabel = new Label();
                    axisLabel.Style = axisLabelStyle;
                    baseLabelsPosition.Add((chart.verticalDivisionCount - i) * machCanvasBehind.Width / chart.verticalDivisionCount);
                    axisLabel.SetValue(Canvas.LeftProperty, (baseLabelsPosition[i] + labelsHorizontalAdjust));
                    axisLabel.SetValue(Canvas.TopProperty, labelsVerticalAdjust);
                    axisLabel.RenderTransformOrigin = new Point(0.5, 0.5);
                    axisLabel.RenderTransform = transformGroup;
                    labelCollection.Add(axisLabel);
                    machCanvasBehind.Children.Add(axisLabel);
                }
            }
            public void renderDescription()
            {
                Style style = Application.Current.FindResource(descriptionStyle) as Style;
                TransformGroup transformGroup = transformAxisElement(descriptionRotateAngle);

                description.Style = style;

                description.SetValue(Canvas.LeftProperty, machCanvasBehind.Width + descriptionHorizontalAdjust);
                description.SetValue(Canvas.TopProperty, descriptionVerticalAdjust);

                description.RenderTransformOrigin = new Point(0.5, 0.5);

                description.RenderTransform = transformGroup;
                machCanvasBehind.Children.Add(description);
            }
            public List<double> baseLabelsPosition = new List<double>();
        }

        //
        //NESTES CLASS: MachAxis
        //
        public abstract class MachAxis
        {
            public double globalMin;
            public double globalMax;
            public double globalRange;

            public int roundTo = 2;
            public TextBlock description = new TextBlock();
            public List<Label> labelCollection
            {
                get { return _labelCollection; }
                private set { }
            }
            
            public Visibility visibility
            {
                get
                {
                    return _visibility;
                }
                set
                {
                    _visibility = value;
                    foreach (FrameworkElement element in labelCollection)
                    {
                        element.Visibility = _visibility;
                    }
                    description.Visibility = _visibility;
                }
            }

            public double labelsVerticalAdjust;
            public double labelsHorizontalAdjust;
            public double labelsRotateAngle;

            public double descriptionVerticalAdjust;
            public double descriptionHorizontalAdjust;
            public double descriptionRotateAngle;
                     
            protected string labelStyle;
            protected string descriptionStyle;
            protected Canvas machCanvasBehind;
            protected MachChart chart;
            protected TransformGroup transformAxisElement(double r_labelRotateAngle)
            {
                RotateTransform myRotateTransform = new RotateTransform();
                myRotateTransform.Angle = r_labelRotateAngle;
                SkewTransform mySkewTransform = new SkewTransform();
                mySkewTransform.AngleX = 0;
                mySkewTransform.AngleY = 0;
                ScaleTransform myScaleTransform = new ScaleTransform();
                myScaleTransform.ScaleX = -1;
                myScaleTransform.ScaleY = 1;
                TransformGroup transformGroup = new TransformGroup();
                transformGroup.Children.Add(myRotateTransform);
                transformGroup.Children.Add(mySkewTransform);
                transformGroup.Children.Add(myScaleTransform);
                return transformGroup;
            }

            protected Visibility _visibility = Visibility.Hidden;
            protected List<Label> _labelCollection = new List<Label>();
        }
    }
}
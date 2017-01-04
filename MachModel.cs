using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Windows;

namespace MachPlotNamespace
{
    public abstract class MachModel
    {
        public abstract void calculate();
        public List<Point> dataPointList = new List<Point>();

        public abstract void generateDataString();
        public string dataString;

        public double step = 1; //calculation step
        public double startX = 0;//here calculation start
        public double endX = 1000; //here calculation end
    }
}

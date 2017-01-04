using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Windows;

namespace MachPlotNamespace
{
    public class LoadMachine : MachModel
    {
        public LoadMachine(string r_functionChoice = "default")
        {
            functionChoice = r_functionChoice;
        }
        public override void calculate()
        {
            dataPointList.Clear();

            switch (functionChoice)
            {
                case "maszyna robocza: obciążenie wentylatorowe":
                    fanLoad();
                    break;
                case "maszyna robocza: obciążenie taśmowe":
                    conveyorLoad();
                    break;
                case "maszyna robocza: obciążenie dźwigowe":
                    craneLoad();
                    break;
                case "maszyna robocza: obciążenie generatorowe":
                    generatorLoad();
                    break;

                default:
                    fanLoad();
                    break;
            }  
        }        
        public override void generateDataString()
        {
            //empty
        }

        public string functionChoice { get; private set; }

        //Default Parameters:
        public double aFan = 0.1;
        public double a2Fan = 0.1;
        public double bFan = 0.1; 
        public double cFan = 0.1;

        public double aConveyor = 0.1;
        public double bConveyor = 0.1;
        public double cConveyor = 0.1;

        public double cCrane = 0.1;

        public double aGenerator = 0.1;
        public double bGenerator = 0.1;
        public double cGenerator = 0.1;



        private void fanLoad()
        {
            for (double n = startX; n < endX; n += step)
            {
                dataPointList.Add(new Point(n,
                    aFan * Math.Pow(n + bFan, 2) + a2Fan * (n + bFan) + cFan
                    ));
            }
        }
        private void conveyorLoad()
        {
            for (double n = startX; n < endX; n += step)
            {
                dataPointList.Add(new Point(n,
                    (aConveyor / (n + bConveyor)) + cConveyor
                    ));
            }
        }
        private void craneLoad()
        {
            for (double n = startX; n < endX; n += step)
            {
                dataPointList.Add(new Point(n, cCrane));
            }
        }
        private void generatorLoad()
        {
            for (double n = startX; n < endX; n += step)
            {
                dataPointList.Add(new Point(n, aGenerator * (n + bGenerator) + cGenerator));
            }
        }
    }   
}

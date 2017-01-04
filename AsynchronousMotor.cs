using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Windows;

namespace MachPlotNamespace
{
    public class AsynchronousMotor : MachModel
    {
        public AsynchronousMotor(string r_functionChoice = "default")
        {
            functionChoice = r_functionChoice;
        }
        
        public override void calculate()
        {
            dataPointList.Clear();

            switch (functionChoice)
            {
                case "elektryczny moment obrotowy: model 1":
                    model1("Me");
                    break;
                case "elektryczny moment obrotowy: model 2":
                    model2("Me");
                    break;
                case "elektryczny moment obrotowy: model 3":
                    model3("Me");
                    break;


                case "prąd wirnika: model 1":
                    model1("I2");
                    break;
                case "prąd wirnika: model 2":
                    model2("I2");
                    break;
                case "prąd wirnika: model 3":
                    model3("I2");
                    break;


                case "prąd stojana: model 1":
                    model1("I1");
                    break;
                case "prąd stojana: model 2":
                    model2("I1");
                    break;
                case "prąd stojana: model 3":
                    model3("I1");
                    break;


                case "prąd gałęzi poprzecznej: model 1":
                    model1("I3");
                    break;
                case "prąd gałęzi poprzecznej: model 2":
                    model2("I3");
                    break;
                case "prąd gałęzi poprzecznej: model 3":
                    model3("I3");
                    break;


                case "moc czynna wejściowa: model 1":
                    model1("P1");
                    break;
                case "moc czynna wejściowa: model 2":
                    model2("P1");
                    break;
                case "moc czynna wejściowa: model 3":
                    model3("P1");
                    break;


                case "współczynnik mocy: model 1":
                    model1("PF");
                    break;
                case "współczynnik mocy: model 2":
                    model2("PF");
                    break;
                case "współczynnik mocy: model 3":
                    model3("PF");
                    break;

                case "impedancja wirnika: model 1":
                    model1("Z2");
                    break;
                case "impedancja wirnika: model 2":
                    model2("Z2");
                    break;
                case "impedancja wirnika: model 3":
                    model3("Z2");
                    break;

                default:
                    model1("Me");
                    break;
            }
        }
        public override void generateDataString()
        {
            //Parameters for display:
            //Mmax:
            double maxY = dataPointList.Max(element => element.Y);
            double minY = dataPointList.Min(element => element.Y);
            double maxX = 0;
            double minX = 0;

            //Max:
            foreach (Point point in dataPointList)
            {
                if (point.Y == maxY) maxX = point.X;
            }

            //Mmin:
            foreach (Point point in dataPointList)
            {
                if (point.Y == minY) minX = point.X;
            }

            maxY = Math.Round(maxY, 2);
            minY = Math.Round(minY, 2);
            maxX = Math.Round(maxX, 2);
            minX = Math.Round(minX, 2);

            dataString = "Ymax:" + maxY + "(X:" + maxX + ")" +
                              "|" +
                              "Ymin:" + minY + "(X:" + minX + ")" +
                              "|" +
                              "Nsyn:" + Math.Round(nSyn, 2);
        }              

        //Basic parameters for asynchronous machine with defaults values

        public double U1 = 230;         
        public double f = 50;           
        public double polePairs = 3;    
        public double R1 = 6;           
        public double R2 = 6;           
        public double L1 = 0.08;        
        public double L2 = 0.03;        
        public double Lm = 3;           
        public double Rfe = 1000;       
        public double turnsRatio = 2;   

        public string functionChoice { get; private set; }//helper variable for calculate()

        //More complex parameters for asynchronous machine:
        private double X1;
        private double X2;
        private double R2refToPrim;
        private double X2refToPrim;
        private double Xm;

        private Complex E;
        private Complex Z1;
        private Complex Z2RefToPrim;
        private Complex Z2;
        private Complex Z3;
        private Complex I1;
        private Complex I2;
        private Complex I2RefToPrim;
        private Complex I3;
        private Complex Im;

        private double P1;
        private double Q1;
        private Complex S1;
        private double PF;

        private double nSyn;
        private double wSyn;
        private double n;
        private double s;
        private double Me;

        private void model1(string r_ValuePick)
        {
            evaluateParameters();

            double Mk = (3 / wSyn) * (Math.Pow(U1, 2) / (2 * (X1 + X2refToPrim)));
            double sk = R2refToPrim / (X1 + X2refToPrim);


            for (n = startX; n < endX; n += step)
            {
                s = (nSyn - n) / nSyn;
                if (s == 0) s = 0.00001;

                Z2RefToPrim = new Complex(R2refToPrim / s, X2refToPrim);
                Z2 = Z2RefToPrim / Math.Pow(turnsRatio, 2);
                I2RefToPrim = (U1 / (new Complex(R2refToPrim / s, X1 + X2refToPrim)));
                I3 = 0;
                I2 = I2RefToPrim * turnsRatio;
                I1 = I2RefToPrim;

                S1 = U1 * Complex.Conjugate(I1);
                P1 = S1.Real;
                Q1 = S1.Imaginary;
                PF = Math.Abs(Math.Cos(S1.Phase));

                Me = (2 * Mk) / ((s / sk) + (sk / s));

                chooseRequestedValue(r_ValuePick);

            }
            generateDataString();
        }
        private void model2(string r_ValuePick)
        {
            evaluateParameters();
            Z1 = new Complex(R1, X1);
            Z3 = new Complex(0, Rfe * Xm) / new Complex(Rfe, Xm);
            //Z3 = new Complex(0, Xm);

            for (n = startX; n < endX; n += step)
            {
                s = (nSyn - n) / nSyn;
                if (s == 0) s = 0.00001;
                Z2RefToPrim = new Complex(R2refToPrim / s, X2refToPrim);
                Z2 = Z2RefToPrim / Math.Pow(turnsRatio, 2);

                I2RefToPrim = (Z3 * U1) /
                    (Z2RefToPrim * Z1 + Z3 * (Z2RefToPrim + Z1));
                I2 = I2RefToPrim * turnsRatio;

                E = Z2RefToPrim * I2RefToPrim;
                I3 = E / Z3;

                I1 = I2RefToPrim + I3;

                S1 = U1 * Complex.Conjugate(I1);
                P1 = S1.Real;
                Q1 = S1.Imaginary;
                PF = Math.Abs(Math.Cos(S1.Phase));

                Me = (3 * 1 / wSyn) *
                     Math.Pow(Complex.Abs(I2RefToPrim), 2) *
                     R2refToPrim / s;
                chooseRequestedValue(r_ValuePick);


            }

            generateDataString();
        }
        private void model3(string r_ValuePick)
        {
            evaluateParameters();

            double _EvaluatedLm; //evaluated Lm
            double _EvaluatedPreviousXm = Xm; //evaluated Xm from last step
            double _EvaluatedCurrentXm = Xm; //current evaluated Xm
            double Qm;

            Z1 = new Complex(R1, X1);

            for (n = startX; n < endX; n += step)
            {
                s = (nSyn - n) / nSyn;
                if (s == 0) s = 0.00001;

                Z2RefToPrim = new Complex(R2refToPrim / s, X2refToPrim);
                Z2 = Z2RefToPrim / Math.Pow(turnsRatio, 2);

                //---Numeric calculations for nonlinear core:
                do
                {
                    _EvaluatedPreviousXm = _EvaluatedCurrentXm;

                    Z3 = new Complex(0, Rfe * _EvaluatedPreviousXm) / new Complex(Rfe, _EvaluatedPreviousXm);

                    I2RefToPrim = (Z3 * U1) /
                        (Z2RefToPrim * Z1 + Z3 * (Z2RefToPrim + Z1));

                    E = Z2RefToPrim * I2RefToPrim;
                    Im = E / (new Complex(0, Xm));
                    Qm = magnetizationCurve(Complex.Abs(Im)); //flux
                    _EvaluatedLm = Qm / Complex.Abs(Im);
                    _EvaluatedCurrentXm = 2 * 3.14 * f * _EvaluatedLm;

                } while (Math.Abs(_EvaluatedPreviousXm - _EvaluatedCurrentXm) >= 0.001);
                _EvaluatedPreviousXm = _EvaluatedCurrentXm;
                _EvaluatedCurrentXm = Xm;
                //---

                I2 = I2RefToPrim * turnsRatio;
                I3 = E / Z3;
                I1 = I2RefToPrim + I3;

                S1 = U1 * Complex.Conjugate(I1);
                P1 = S1.Real;
                Q1 = S1.Imaginary;
                PF = Math.Abs(Math.Cos(S1.Phase));

                Me = (3 * 1 / wSyn) *
                     Math.Pow(Complex.Abs(I2RefToPrim), 2) *
                     R2refToPrim / s;

                chooseRequestedValue(r_ValuePick);
            }

            generateDataString();
        }
        //support functions for model functions:
        private void chooseRequestedValue(string r_ValuePick)
        {
            switch (r_ValuePick)
            {
                case "I1":
                    dataPointList.Add(new Point(n, Complex.Abs(I1)));
                    break;
                case "I2":
                    dataPointList.Add(new Point(n, Complex.Abs(I2)));
                    break;
                case "I3":
                    dataPointList.Add(new Point(n, Complex.Abs(I3)));
                    break;
                case "Me":
                    dataPointList.Add(new Point(n, Me));
                    break;
                case "P1":
                    dataPointList.Add(new Point(n, 3 * P1));
                    break;
                case "PF":
                    dataPointList.Add(new Point(n, PF));
                    break;
                case "Z2":
                    dataPointList.Add(new Point(n, Complex.Abs(Z2)));
                    break;
            }
        }
        private double magnetizationCurve(double r_Im)//returns flux - helper function for nonlinearCore model
        {
            double a = 0.55; // a = 0,55 to 0,65
            double Im = r_Im; //magnetization current
            double Qm; //flux
            Qm = Im / (a * Math.Abs(Im) + (1 - a));
            return Qm;
        }
        private void evaluateParameters()
        {
            nSyn = (60 * f) / polePairs;
            wSyn = (2 * 3.14 * nSyn) / 60;
            X1 = 2 * 3.14 * f * L1;
            X2 = 2 * 3.14 * f * L2;
            R2refToPrim = R2 * Math.Pow(turnsRatio, 2);
            X2refToPrim = X2 * Math.Pow(turnsRatio, 2);
            Xm = 2 * 3.14 * f * Lm;
        }
        
    }
}

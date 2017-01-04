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

namespace MachPlotNamespace
{
    public class MachSegment
    {
        public MachSegment(Point p1, Point p2)
        {
            X1 = p1.X;
            X2 = p2.X;
            Y1 = p1.Y;
            Y2 = p2.Y;
        }
        //Segment start:
        public double X1;
        public double Y1;
        //Segment end:
        public double X2;        
        public double Y2;
    }

    public abstract class MachVectorIntersection
    {
        //helper variables to simplify equation:
        private static double X;
        private static double Y;
        private static double t1;
        private static double t2;
        private static double t3;
        private static double t4;

        private static double a;
        private static double b;
        private static double c;
        private static double d;
        private static double denominator;


        public static MachIntersectionPoint findIntersectionSingle(MachSegment segment1, MachSegment segment2)
        {
            //Console.WriteLine("segment1(received):   " + segment1.X1 + "   " + segment1.Y1 + "   " + segment1.X2 + "   " + segment1.Y2);
            //Console.WriteLine("segment2(received):   " + segment2.X1 + "   " + segment2.Y1 + "   " + segment2.X2 + "   " + segment2.Y2);
            //Console.WriteLine("----------");

            a = (segment1.X1 * segment1.Y2 - segment1.Y1 * segment1.X2) * (segment2.X1 - segment2.X2);
            b = (segment1.X1 - segment1.X2) * (segment2.X1 * segment2.Y2 - segment2.Y1 * segment2.X2);
            c = (segment1.X1 - segment1.X2) * (segment2.Y1 - segment2.Y2);
            d = (segment1.Y1 - segment1.Y2) * (segment2.X1 - segment2.X2);

            denominator = c - d;

            if (denominator == 0)
            {
                return new MachIntersectionPoint() { point = new Point(), isIntersect = false, isParalel = true };
            }


            X = (a - b) / denominator;

            a = (segment1.X1 * segment1.Y2 - segment1.Y1 * segment1.X2) * (segment2.Y1 - segment2.Y2);
            b = (segment1.Y1 - segment1.Y2) * (segment2.X1 * segment2.Y2 - segment2.Y1 * segment2.X2);

            Y = (a - b) / denominator;


            if ((X - segment1.X1) == 0) t1 = 0;
            else t1 = (X - segment1.X1) / (segment1.X2 - segment1.X1);

            if ((Y - segment1.Y1) == 0) t2 = 0;
            else t2 = (Y - segment1.Y1) / (segment1.Y2 - segment1.Y1);

            if ((X - segment2.X1) == 0) t3 = 0;
            else t3 = (X - segment2.X1) / (segment2.X2 - segment2.X1);

            if ((Y - segment2.Y1) == 0) t4 = 0;
            else t4 = (Y - segment2.Y1) / (segment2.Y2 - segment2.Y1);

            if (t1 <= 1 && t1 >= 0 &&
                t2 <= 1 && t2 >= 0 &&
                t3 <= 1 && t3 >= 0 &&
                t4 <= 1 && t4 >= 0)
            {
                //Console.WriteLine(t1);
                return new MachIntersectionPoint() { point = new Point(X, Y), isIntersect = true, isParalel = false };
            }
            else
            {
                 //Console.WriteLine("denominator not zero");
                return new MachIntersectionPoint() { point = new Point(), isIntersect = false, isParalel = false };
            }
            //return new IntersectionPoint() { point = new Point(X, Y), isIntersect = true, isParalel = false };
        }
        public static List<Point> findIntrsectionAll(List<Point> r_dataCollection1, List<Point> r_dataCollection2)
        {

            intersectionPoints.Clear();
            segmentCollection1 = MachVectorIntersection.convertPointsToSegments(r_dataCollection1);
            segmentCollection2 = MachVectorIntersection.convertPointsToSegments(r_dataCollection2);


            foreach (MachSegment outerSegment in segmentCollection1)
            {
                foreach (MachSegment innerSegment in segmentCollection2)
                {
                    point = findIntersectionSingle(outerSegment, innerSegment);
                    if (point.isIntersect == true) intersectionPoints.Add(point.point);
                    //intersectionPoints.Add(point.point);
                }
            }
            return intersectionPoints;
        }


        private static List<MachSegment> convertPointsToSegments(List<Point> r_dataCollection)
        {
            List<MachSegment> returnSegments = new List<MachSegment>();
            for (int i = 0; i < r_dataCollection.Count - 1; i++)
            {
                returnSegments.Add(new MachSegment(r_dataCollection[i], r_dataCollection[i + 1]));
                //Console.WriteLine(i + ": ");
                //Console.WriteLine(returnSegments[i].X1 + "   " + returnSegments[i].Y1 + "   " + returnSegments[i].X2 + "   " + returnSegments[i].Y2);
            }
            return returnSegments;
        }
        private static List<MachSegment> segmentCollection1;
        private static List<MachSegment> segmentCollection2;
        private static List<Point> intersectionPoints = new List<Point>();
        private static MachIntersectionPoint point = new MachIntersectionPoint();
        
    }

    public class MachIntersectionPoint
    {
        public Point point;
        public bool isParalel;
        public bool isIntersect;
    }
}

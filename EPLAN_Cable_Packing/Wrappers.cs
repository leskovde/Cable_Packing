using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace EPLAN_Cable_Packing
{
    internal struct Point
    {
        public long X;
        public long Y;

        public Point(long x, long y)
        {
            X = x;
            Y = y;
        }

        public IEnumerable<Point> GetInterval(Point from, Point to, int step)
        {
            var tempPoint = new Point(from.X, from.Y);

            while (tempPoint.Y <= to.Y)
            {
                yield return tempPoint;

                if (tempPoint.X < to.X)
                {
                    tempPoint.X += step;
                }
                else
                {
                    tempPoint.X = from.X;
                    tempPoint.Y += step;
                }
            }
        }
    }

    internal struct Circle
    {
        public int Diameter;
        public Point Center;

        public Circle(int diameter, Point center)
        {
            Diameter = diameter;
            Center = center;
        }
    }

    internal struct PackingResultWrapper
    {
        public Circle Bundle;
        public List<Circle> InnerCircles;

        public PackingResultWrapper(Circle bundle, List<Circle> innerCircles)
        {
            Bundle = bundle;
            InnerCircles = innerCircles;
        }
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
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
        public int Radius;
        public Color Color;
        public Point Center;

        public Circle(int radius, Point center)
        {
            Radius = radius;
            Center = center;

            var rnd = new Random();

            Color = default;

            while (Color.R < 30 && Color.G < 30 && Color.B < 30)
                Color = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
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

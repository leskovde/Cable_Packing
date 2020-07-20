using System;
using System.Collections.Generic;
using System.Drawing;

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

        public IEnumerable<Point> GetInterval(Point from, Point to, long step)
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

        public IEnumerable<Point> GetSpiralInterval(Point from, Point to, long step)
        {
            var radius = to.X - from.X / 2;
            var centerPoint = new Point(from.X + radius, from.Y + radius);

            // Value of Theta corresponding to end of last coil
            var thetaMax = radius * 2 * Math.PI;

            double distanceBetweenPoints = step;

            yield return centerPoint;

            // For every side, step around and away from center. Start at the angle corresponding to a distance
            // of chord away from centre.
            for (var theta = distanceBetweenPoints / step; theta <= thetaMax;)
            {
                var awayFromCenter = step * theta;

                var x = centerPoint.X + Math.Cos(theta) * awayFromCenter;
                var y = centerPoint.Y + Math.Sin(theta) * awayFromCenter;

                yield return new Point((long) x, (long) y);

                // To a first approximation, the points are on a circle
                // so the angle between them is chord/radius
                theta += distanceBetweenPoints / awayFromCenter;
            }
        }
    }

    internal struct Circle
    {
        public long Radius;
        public Color Color;
        public Point Center;

        public Circle(long radius, Point center)
        {
            Radius = radius;
            Center = center;

            var rnd = new Random();

            Color = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));

            while (Color.R > 200 && Color.G > 200 && Color.B > 200)
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
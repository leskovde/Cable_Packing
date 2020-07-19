using System;
using System.Collections.Generic;
using System.Text;

namespace EPLAN_Cable_Packing
{
    internal struct Point
    {
        public int X;
        public int Y;
    }

    internal struct Circle
    {
        public int Diameter;
        public Point Center;
    }

    internal struct PackingResultWrapper
    {
        public Circle Bundle;
        public List<Circle> InnerCircles;
    }

}

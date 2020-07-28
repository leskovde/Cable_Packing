using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace EPLAN_Cable_Packing
{
    internal static class DecimalExtensions
    {
        public static int Precision(this decimal decimalValue)
        {
            return decimalValue.ToString(CultureInfo.InvariantCulture)
                .SkipWhile(c => c != '.')
                .Skip(1)
                .Count();
        }

        public static int Digits(this decimal decimalValue)
        {
            return decimalValue.ToString(CultureInfo.InvariantCulture)
                .Count(c => c != '.');
        }
    }

    internal static class ListExtensions
    {
        public static int NormalizedDigitCountLowerBound = 5;
        public static int NormalizedDigitCountUpperBound = 7;

        public static (List<long>, int maxPrecision) Normalize(this List<decimal> decimalList)
        {
            var maxPrecision = 0;

            foreach (var @decimal in decimalList)
            {
                if (@decimal.Precision() > maxPrecision)
                    maxPrecision = @decimal.Precision();
            }

            var integerList = decimalList.Select(radius => (long) (radius * (decimal) Math.Pow(10, maxPrecision))).ToList();

            while (integerList.Sum() > Math.Pow(10, NormalizedDigitCountUpperBound))
            {
                for (var i = 0; i < integerList.Count; i++)
                {
                    integerList[i] /= 10;
                }

                maxPrecision--;
            }

            while (integerList.Sum() < Math.Pow(10, NormalizedDigitCountLowerBound))
            {
                for (var i = 0; i < integerList.Count; i++)
                {
                    integerList[i] *= 10;
                }

                maxPrecision++;
            }

            return (integerList, maxPrecision);
        }
    }

    internal static class GraphicsExtensions
    {
        public static void DrawCircle(this Graphics g, Pen pen,
            Circle circle, float divisor, float xAxisOffset, float yAxisOffset, decimal originalRadius)
        {
            pen.Color = circle.Color;

            var multipliedRadius = circle.Radius / divisor;
            var multipliedCenterX = circle.Center.X / divisor + xAxisOffset - multipliedRadius;
            var multipliedCenterY = circle.Center.Y / divisor + yAxisOffset - multipliedRadius;

            g.DrawEllipse(pen, multipliedCenterX, multipliedCenterY,
                2 * multipliedRadius, 2 * multipliedRadius);

            g.DrawString(originalRadius.ToString(CultureInfo.InvariantCulture), new Font("Arial", 8),
                new SolidBrush(pen.Color),
                multipliedCenterX + multipliedRadius, multipliedCenterY + multipliedRadius);
        }
    }

    internal static class Program
    {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new OutputVisualization());
        }
    }
}
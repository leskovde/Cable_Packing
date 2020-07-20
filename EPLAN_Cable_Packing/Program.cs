using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EPLAN_Cable_Packing
{
    /**
     * Initial idea:
     * Most likely an NP-hard problem.
     *
     * Solve a linear program to obtain the radius of the containing circle and all the radii of contained circles.
     * This should be possible due to the restriction to rational numbers (those can be easily converted to integers
     * via multiplication by a single factor) and creating a relaxation of the linear program. Thus getting a polynomial
     * running time.
     *
     * Research:
     *
     * Theorem: To decide if a set of circles can be packed into a square is NP-hard.
     * => approximation (time X quality)
     *
     * Greedy approach: Items appear one after the other, item must be packed when it
     * arrives, i.e. without the knowledge of further items. Items cannot be repacked.
     *
     * Area based algorithms: Best possible density is Pi/sqrt(12) ~ 1.10266, result is Area(L), L is the list of circles
     * Using square packing:
     *          (1) Round each circle into a square (side length of 2*r)
     *          (2) Use square packing algorithm
     *          => area increase: at best 4/Pi ~ 1.27324 => at best 4/Pi * Area(L), at worst 5.1 * OPT(L) + 3
     *          => shelf packing: better than 2.548 * OPT(L)
     *
     * Packing equal circles: better than 1.654 * Area(L) + 2*r
     *
     * Rounding circles:
     *          (1) Round circles with close radii to the same radius.
     *          (2) Use equal circle packing algorithm on those circles
     *          (3) Concatenate results of (2)
     *          => better than 1.654 * Area(L) + c
     *
     */

    internal static class DecimalExtensions
    {
        public static int Precision(this decimal decimalValue)
        {
            return decimalValue.ToString(CultureInfo.InvariantCulture)
                .TrimEnd('0')
                .SkipWhile(c => c != '.')
                .Skip(1)
                .Count();
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

            g.DrawString(originalRadius.ToString(CultureInfo.InvariantCulture), new Font("Arial", 8), new SolidBrush(pen.Color),
                multipliedCenterX + multipliedRadius, multipliedCenterY + multipliedRadius);
        }

        public static void FillCircle(this Graphics g, Brush brush,
            float centerX, float centerY, float radius)
        {
            g.FillEllipse(brush, centerX - radius, centerY - radius,
                radius + radius, radius + radius);
        }
    }

    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EPLAN_Cable_Packing
{

    public partial class OutputVisualization : Form
    {
        public OutputVisualization()
        {
            InitializeComponent();

            //Canvas.Refresh();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
        }

        private void Canvas_Paint(object sender, PaintEventArgs e)
        {
            var factories = new Dictionary<Algorithms, AlgorithmFactory>
            {
                {Algorithms.Greedy, new GreedyAlgorithmFactory()},
                {Algorithms.LinearProgramming, new LinearProgrammingAlgorithmFactory()},
            };

            const string inputFile = "input.txt";
            const Algorithms algorithmType = Algorithms.Greedy;

            var maxPrecision = 0;
            var diameters = new List<decimal>();

            using (var inputProcessor = new InputProcessor(inputFile))
            {
                decimal? entry;

                while ((entry = inputProcessor.ReadEntry()) != null)
                {
                    diameters.Add(entry.Value);

                    if (entry.Value.Precision() > maxPrecision)
                        maxPrecision = entry.Value.Precision();
                }
            }

            var integerDiameters = diameters.Select(diameter => (int)(diameter * (decimal)Math.Pow(10, maxPrecision))).ToList();

            var cablePacking = new CablePacking(factories[algorithmType].Create());

            var result = cablePacking.GetCablePacking(integerDiameters);

            var multiplier = 3 * (float)result.Bundle.Diameter / (Canvas.Width);
            var xAxisOffset = (float)Canvas.Width / 2 - result.Bundle.Center.X / multiplier;
            var yAxisOffset = (float)Canvas.Height / 2 - result.Bundle.Center.Y / multiplier;


            Pen pen = new Pen(Color.Blue, 1);
            e.Graphics.DrawCircle(pen, result.Bundle.Center.X / multiplier + xAxisOffset,
                result.Bundle.Center.Y / multiplier + yAxisOffset, result.Bundle.Diameter / multiplier);

            foreach (var circle in result.InnerCircles)
            {
                e.Graphics.DrawCircle(pen, circle.Center.X / multiplier + xAxisOffset,
                    circle.Center.Y / multiplier + yAxisOffset, circle.Diameter / multiplier);
            }
        }
    }
}

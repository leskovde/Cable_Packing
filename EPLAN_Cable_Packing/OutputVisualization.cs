using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EPLAN_Cable_Packing
{
    internal partial class OutputVisualization : Form
    {
        public static Algorithms AlgorithmType = Algorithms.LinearProgramming;

        public OutputVisualization()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
        }

        private void Canvas_Paint(object sender, PaintEventArgs e) { }


        private void Canvas_Paint(object sender, PaintEventArgs e, PackingResultWrapper result, int maxPrecision)
        {

            var multiplier = 3 * (float)result.Bundle.Radius / (Canvas.Width);
            var xAxisOffset = (float)Canvas.Width / 2 - result.Bundle.Center.X / multiplier;
            var yAxisOffset = (float)Canvas.Height / 2 - result.Bundle.Center.Y / multiplier;

            var outerCirclePen = new Pen(Color.LimeGreen, 3);
            var innerCirclePen = new Pen(Color.Aqua, 1);

            //e.Graphics.DrawCircle(outerCirclePen, result.Bundle.Center.X / multiplier + xAxisOffset,
            //    result.Bundle.Center.Y / multiplier + yAxisOffset, result.Bundle.Radius / multiplier, result.Bundle.Radius / (decimal) Math.Pow(10, maxPrecision));

            e.Graphics.DrawCircle(outerCirclePen, result.Bundle, multiplier, xAxisOffset, yAxisOffset, result.Bundle.Radius / (decimal)Math.Pow(10, maxPrecision));


            foreach (var circle in result.InnerCircles)
            {
                e.Graphics.DrawCircle(outerCirclePen, circle, multiplier, xAxisOffset, yAxisOffset, circle.Radius / (decimal)Math.Pow(10, maxPrecision));
                //e.Graphics.DrawCircle(innerCirclePen, circle.Center.X / multiplier + xAxisOffset,
                //    circle.Center.Y / multiplier + yAxisOffset, circle.Radius / multiplier, circle.Radius / (decimal) Math.Pow(10, maxPrecision));
            }

            OutputDiameterText.Text = (2 * result.Bundle.Radius / (decimal) Math.Pow(10, maxPrecision)).ToString(CultureInfo.InvariantCulture) + " mm";
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                InitialDirectory = @"C:\",
                Title = "Browse Text Files",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "txt",
                Filter = "txt files (*.txt)|*.txt",
                FilterIndex = 2,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                FileName.Text = openFileDialog.FileName;
            }
        }

        private void GreedySwitch_CheckedChanged(object sender, EventArgs e)
        {
            AlgorithmType = Algorithms.Greedy;
        }

        private void LinearSwitch_CheckedChanged(object sender, EventArgs e)
        {
            AlgorithmType = Algorithms.LinearProgramming;
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            Status.Text = "Running";

            var factories = new Dictionary<Algorithms, AlgorithmFactory>
            {
                {Algorithms.Greedy, new GreedyAlgorithmFactory()},
                {Algorithms.LinearProgramming, new LinearProgrammingAlgorithmFactory()},
            };

            //var inputFile = FileName.Text;
            var inputFile = "input.txt";

            var maxPrecision = 0;
            var radii = new List<decimal>();

            using (var inputProcessor = new InputProcessor(inputFile))
            {
                decimal? entry;

                while ((entry = inputProcessor.ReadEntry()) != null)
                {
                    radii.Add(entry.Value);

                    if (entry.Value.Precision() > maxPrecision)
                        maxPrecision = entry.Value.Precision();
                }
            }

            var integerRadii = radii.Select(radius => (int)(radius * (decimal)Math.Pow(10, maxPrecision))).ToList();

            var cablePacking = new CablePacking(factories[AlgorithmType].Create());

            var result = cablePacking.GetCablePacking(integerRadii);

            Canvas.Paint += (senderObj, paintEventArgs) => { Canvas_Paint(sender, paintEventArgs, result, maxPrecision); };

            Canvas.Refresh();

            Status.Text = "Finished";
        }
    }
}

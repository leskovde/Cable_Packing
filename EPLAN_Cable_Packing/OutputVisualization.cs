﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EPLAN_Cable_Packing
{
    internal partial class OutputVisualization : Form
    {
        public static Algorithms AlgorithmType = Algorithms.Greedy;

        public OutputVisualization()
        {
            InitializeComponent();
        }

        private void Canvas_Paint(object sender, PaintEventArgs e)
        {
        }


        private void Canvas_Paint(PaintEventArgs e, PackingResultWrapper result, int maxPrecision)
        {
            e.Graphics.Clear(Canvas.BackColor);

            var ratioDivisor = 3 * (float) result.Bundle.Radius / Canvas.Width;
            var xAxisOffset = (float) Canvas.Width / 2 - result.Bundle.Center.X / ratioDivisor;
            var yAxisOffset = (float) Canvas.Height / 2 - result.Bundle.Center.Y / ratioDivisor;

            var outerCirclePen = new Pen(Color.LimeGreen, 3);
            var innerCirclePen = new Pen(Color.Aqua, 1);

            e.Graphics.DrawCircle(outerCirclePen, result.Bundle, ratioDivisor, xAxisOffset, yAxisOffset,
                result.Bundle.Radius / (decimal) Math.Pow(10, maxPrecision));


            foreach (var circle in result.InnerCircles)
                e.Graphics.DrawCircle(innerCirclePen, circle, ratioDivisor, xAxisOffset, yAxisOffset,
                    circle.Radius / (decimal) Math.Pow(10, maxPrecision));

            OutputDiameterText.Text =
                (2 * result.Bundle.Radius / (decimal) Math.Pow(10, maxPrecision))
                .ToString(CultureInfo.InvariantCulture) + " mm";
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

            if (openFileDialog.ShowDialog() == DialogResult.OK) FileName.Text = openFileDialog.FileName;
        }

        private void GreedySwitch_CheckedChanged(object sender, EventArgs e)
        {
            AlgorithmType = Algorithms.Greedy;
        }

        private void LinearSwitch_CheckedChanged(object sender, EventArgs e)
        {
            AlgorithmType = Algorithms.IntegerProgramming;
        }

        private void SinglePassSwitch_CheckedChanged(object sender, EventArgs e)
        {
            AlgorithmType = Algorithms.SinglePass;
        }

        private async void StartButton_Click(object sender, EventArgs e)
        {
            Status.Text = "Running";

            var factories = new Dictionary<Algorithms, AlgorithmFactory>
            {
                {Algorithms.Greedy, new GreedyAlgorithmFactory()},
                {Algorithms.SinglePass, new SinglePassAlgorithmFactory()},
                {Algorithms.IntegerProgramming, new IntegerProgrammingAlgorithmFactory()}
            };

            var inputFile = FileName.Text;

            var radii = new List<decimal>();

            using (var inputProcessor = new InputProcessor(inputFile))
            {
                decimal? entry;

                while ((entry = inputProcessor.ReadEntry()) != null)
                {
                    radii.Add(entry.Value);
                }
            }

            var (integerRadii, maxPrecision) = radii.Normalize();
            
            await Task.Run(() =>
            {
                var cablePacking = new CablePacking(factories[AlgorithmType].Create());
                var result = cablePacking.GetCablePacking(integerRadii);

                Canvas.Paint += (senderObj, paintEventArgs) =>
                {
                    Canvas_Paint(paintEventArgs, result, maxPrecision);
                };
            });

            Canvas.Refresh();
            
            Status.Text = "Finished";
        }
    }
}
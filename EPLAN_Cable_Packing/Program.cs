using System;
using System.Collections.Generic;
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


    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Add factory pattern
            var factories = new Dictionary<Algorithms, AlgorithmFactory>
            {
                {Algorithms.Greedy, new GreedyAlgorithmFactory()},
                {Algorithms.LinearProgramming, new LinearProgrammingAlgorithmFactory()},
            };

            const string inputFile = "input.txt";
            const Algorithms algorithmType = Algorithms.Greedy;

            var diameters = new List<decimal>();

            using (var inputProcessor = new InputProcessor(inputFile))
            {
                decimal? entry;
                var maxPrecision = 0;

                while ((entry = inputProcessor.ReadEntry()) != null)
                {
                    diameters.Add(entry.Value);

                    if (entry.Value.Precision() > maxPrecision)
                        maxPrecision = entry.Value.Precision();
                }
            }

            var algorithm = factories[algorithmType].Create();
            
            var result = algorithm.Run();

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new OutputVisualization());
        }
    }
}

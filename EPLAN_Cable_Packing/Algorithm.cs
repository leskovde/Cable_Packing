using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Google.OrTools.Sat;
using static EPLAN_Cable_Packing.AlgorithmExtensions;

namespace EPLAN_Cable_Packing
{
    internal static class AlgorithmExtensions
    {
        // Avoid the float operations used in Math.Pow
        public static long Power(long number, long power)
        {
            return power == 1 ? number : number * Power(number, power - 1);
        }

        public static long SquareDistance(Point point1, Point point2)
        {
            return Power(point1.X - point2.X, 2) + Power(point1.Y - point2.Y, 2);
        }

        public static double GetRoughnessMultiplier(long inputSize)
        {
            return Math.Min(Math.Min(1, 1.25 * Math.Pow(inputSize, -0.5)), 1000.0 / inputSize);
        }

        public static (Point newBundleCenter, long actualBundleRadius) PlaceBundle(long lowestBundleRadius,
            Point bundleCenter, List<Circle> circlePlacements, long gridSize, long roughnessMultiplier)
        {
            var leftMostInterval = bundleCenter.X - lowestBundleRadius / 4;
            var rightMostInterval = bundleCenter.X + lowestBundleRadius / 4;
            var lowerMostInterval = bundleCenter.Y - lowestBundleRadius / 4;
            var upperMostInterval = bundleCenter.Y + lowestBundleRadius / 4;

            var newBundleCenter = bundleCenter;
            var actualBundleRadius = (long) lowestBundleRadius;

            for (; actualBundleRadius < gridSize; actualBundleRadius += roughnessMultiplier)
            {
                var intersectsAnotherCircle = false;

                foreach (var point in bundleCenter.GetInterval(new Point(leftMostInterval, lowerMostInterval),
                    new Point(rightMostInterval, upperMostInterval), Math.Max(roughnessMultiplier / 2, 1)))
                {
                    intersectsAnotherCircle = false;

                    foreach (var circle in circlePlacements)
                    {
                        if (SquareDistance(circle.Center, point) <= Power(actualBundleRadius - circle.Radius, 2))
                            continue;

                        intersectsAnotherCircle = true;
                        break;
                    }

                    if (intersectsAnotherCircle) continue;

                    newBundleCenter = point;
                    break;
                }

                if (!intersectsAnotherCircle) break;
            }

            return (newBundleCenter, actualBundleRadius);
        }

        public static (long leftMostCoordinate, long rightMostCoordinate, long lowerMostCoordinate, long
            upperMostCoordinate)
            GetPackingBoundaries(List<Circle> circlePlacements)
        {
            var leftMostCoordinate = long.MaxValue;
            var rightMostCoordinate = long.MinValue;
            var lowerMostCoordinate = long.MaxValue;
            var upperMostCoordinate = long.MinValue;

            foreach (var circle in circlePlacements)
            {
                if (circle.Center.X - circle.Radius < leftMostCoordinate)
                    leftMostCoordinate = circle.Center.X - circle.Radius;

                if (circle.Center.X + circle.Radius > rightMostCoordinate)
                    rightMostCoordinate = circle.Center.X + circle.Radius;

                if (circle.Center.Y - circle.Radius < lowerMostCoordinate)
                    lowerMostCoordinate = circle.Center.Y - circle.Radius;

                if (circle.Center.Y + circle.Radius > upperMostCoordinate)
                    upperMostCoordinate = circle.Center.Y + circle.Radius;
            }

            return (leftMostCoordinate, rightMostCoordinate, lowerMostCoordinate, upperMostCoordinate);
        }
    }

    /**
     * Place circles onto a spiral pattern. Switch between large and small circles while preferring the larger ones.
     */
    internal class SinglePassPackingAlgorithm : IPackingAlgorithm
    {
        public PackingResultWrapper Run(List<long> radii)
        {
            var gridSize = radii.Sum();
            var roughnessMultiplier = (long) (1.0 / GetRoughnessMultiplier(gridSize));
            var bundleCenter = new Point(gridSize / 2, gridSize / 2);

            radii.Sort((x, y) => y.CompareTo(x));
            var circlePlacements = new List<Circle>();

            var smallRadiiQueue = new Queue<long>();
            var largeRadiiQueue = new Queue<long>();

            for (var i = 0; i < radii.Count / 2; i++) largeRadiiQueue.Enqueue(radii[i]);

            for (var i = radii.Count / 2; i < radii.Count; i++) smallRadiiQueue.Enqueue(radii[i]);

            // Slowly spiral out of the center
            foreach (var point in bundleCenter.GetSpiralInterval(new Point(0, 0),
                new Point(gridSize, gridSize), Math.Max(roughnessMultiplier / 2, 1)))
            {
                if (largeRadiiQueue.Count == 0 && smallRadiiQueue.Count == 0) break;

                var currentRadius = largeRadiiQueue.Count > 0 ? largeRadiiQueue.Peek() : smallRadiiQueue.Peek();

                    // Attempt to place a large circle, upon failure attempt to place a small circle
                for (var i = 0; i < 2; i++)
                {
                    var intersectsAnotherCircle = false;

                    foreach (var circle in circlePlacements)
                    {
                        if (SquareDistance(circle.Center, point) >= Power(currentRadius + circle.Radius, 2))
                            continue;

                        intersectsAnotherCircle = true;

                        if (smallRadiiQueue.Count > 0)
                            currentRadius = smallRadiiQueue.Peek();

                        break;
                    }

                    if (intersectsAnotherCircle) continue;

                    if (largeRadiiQueue.Count != 0 && largeRadiiQueue.Peek() == currentRadius)
                        largeRadiiQueue.Dequeue();
                    else
                        smallRadiiQueue.Dequeue();

                    circlePlacements.Add(new Circle(currentRadius, point));

                    break;
                }
            }

            // Find the bundle center and radius
            var (leftMostCoordinate, rightMostCoordinate, lowerMostCoordinate, upperMostCoordinate) =
                GetPackingBoundaries(circlePlacements);
            var lowestBundleRadius = Math.Max(rightMostCoordinate - leftMostCoordinate,
                                         upperMostCoordinate - lowerMostCoordinate) / 2;

            bundleCenter.X = leftMostCoordinate + (rightMostCoordinate - leftMostCoordinate) / 2;
            bundleCenter.Y = lowerMostCoordinate + (upperMostCoordinate - lowerMostCoordinate) / 2;

            long actualBundleRadius;
            (bundleCenter, actualBundleRadius) = PlaceBundle(lowestBundleRadius, bundleCenter, circlePlacements,
                gridSize, roughnessMultiplier);

            return new PackingResultWrapper(new Circle(actualBundleRadius, bundleCenter) {Color = Color.Lime},
                circlePlacements);
        }
    }

    /**
     * Place the circle closest to the center of the containing circle while not touching
     * any other circles.
     */
    internal class GreedyPackingAlgorithm : IPackingAlgorithm
    {
        private List<Circle> _circlePlacements;
        private long _gridSize;
        private long _roughnessMultiplier;

        public PackingResultWrapper Run(List<long> radii)
        {
            _gridSize = radii.Sum();
            _roughnessMultiplier = (long) (1.0 / GetRoughnessMultiplier(_gridSize));
            var bundleCenter = new Point(_gridSize / 2, _gridSize / 2);

            radii.Sort((x, y) => y.CompareTo(x));
            _circlePlacements = new List<Circle>();

            PlaceCircles(radii, bundleCenter);

            var (leftMostCoordinate, rightMostCoordinate, lowerMostCoordinate, upperMostCoordinate) =
                GetPackingBoundaries(_circlePlacements);
            var lowestBundleRadius = Math.Max(rightMostCoordinate - leftMostCoordinate,
                                         upperMostCoordinate - lowerMostCoordinate) / 2;

            bundleCenter.X = leftMostCoordinate + (rightMostCoordinate - leftMostCoordinate) / 2;
            bundleCenter.Y = lowerMostCoordinate + (upperMostCoordinate - lowerMostCoordinate) / 2;

            long actualBundleRadius;
            (bundleCenter, actualBundleRadius) = PlaceBundle(lowestBundleRadius, bundleCenter, _circlePlacements,
                _gridSize, _roughnessMultiplier);

            return new PackingResultWrapper(new Circle(actualBundleRadius, bundleCenter) {Color = Color.Lime},
                _circlePlacements);
        }

        private void PlaceCircles(List<long> radii, Point bundleCenter)
        {
            foreach (var radius in radii)
            {
                var bestSquareDistance = long.MaxValue;
                var bestCenter = new Point(long.MaxValue, long.MaxValue);

                // X axis
                for (long i = 0; i < _gridSize; i += _roughnessMultiplier)
                    // Y axis
                    for (long j = 0; j < _gridSize; j += _roughnessMultiplier)
                    {
                        // This point in the grid is already occupied
                        if (CoordinateContainsCircle(i, j)) continue;

                        // There is an already better point (i.e. closer to the center) that has been discovered
                        if (!(SquareDistance(new Point(i, j), bundleCenter) < bestSquareDistance)) continue;

                        var intersectsAnotherCircle = CircleContainsAnotherCircle(radius, i, j);

                        // The circle would intersect another circle if it were to be placed here
                        if (intersectsAnotherCircle) continue;

                        // This is the best location for this circle yet
                        bestCenter = new Point(i, j);
                        bestSquareDistance = SquareDistance(new Point(i, j), bundleCenter);
                    }

                if (!(bestSquareDistance < long.MaxValue)) throw new NotImplementedException();

                _circlePlacements.Add(new Circle(radius, bestCenter));
            }
        }

        private bool CoordinateContainsCircle(long coordinateX, long coordinateY)
        {
            foreach (var circle in _circlePlacements)
                if (SquareDistance(new Point(coordinateX, coordinateY), circle.Center) < Power(circle.Radius, 2))
                    return true;

            return false;
        }

        private bool CircleContainsAnotherCircle(long radius, long circleCenterX, long circleCenterY)
        {
            foreach (var circle in _circlePlacements)
                if (SquareDistance(new Point(circleCenterX, circleCenterY), circle.Center) <
                    Power(circle.Radius + radius, 2))
                    return true;

            return false;
        }
    }

    /**
     * Solve a mixed integer program to achieve an approximation in polynomial time - O(n^2) constraints and variables.
     * CURRENTLY FAILS - the model is not feasible
     */
    internal class IntegerProgrammingPackingAlgorithm : IPackingAlgorithm
    {
        public PackingResultWrapper Run(List<long> radii)
        {
            var model = new CpModel();

            const int dimension = 2;
            var domainConstraint = 2 * radii.Sum();

            // ----------------------------------------------------------------
            // Variable declaration
            // ----------------------------------------------------------------

            // Integer bundle radius, 0 <= x <= 2 * the sum of radii
            var bundleRadius = model.NewIntVar(0, domainConstraint, "r");

            // Coordinates (x, y) of the centers of inner circles, -2 * the sum of radii <= x, y <= 2 * the sum of radii
            var circleCenters = new IntVar[radii.Count, dimension];
            for (var i = 0; i < radii.Count; i++)
            for (var j = 0; j < dimension; j++)
                circleCenters[i, j] = model.NewIntVar(-domainConstraint, domainConstraint, $"alpha_{i},{j}");

            // Slack variable - Coordinates (x, y) of the centers of inner circles squared, 0 <= x, y < infinity
            var circleCentersSquared = new IntVar[radii.Count, dimension];
            for (var i = 0; i < radii.Count; i++)
            for (var j = 0; j < dimension; j++)
                circleCentersSquared[i, j] = model.NewIntVar(0, uint.MaxValue, $"s_alpha_{i},{j}");

            // Inner circle radii - input
            var circleRadii = new IntVar[radii.Count];
            for (var i = 0; i < radii.Count; i++) circleRadii[i] = model.NewIntVar(radii[i], radii[i], $"r_{i}");

            // Slack variable - bundle radius minus the inner circle radius, -2 * the sum of radii <= x <= 2 * the sum of radii
            var circleRadiiSlack = new IntVar[radii.Count];
            for (var i = 0; i < radii.Count; i++)
                circleRadiiSlack[i] = model.NewIntVar(-domainConstraint, domainConstraint, $"s_r_{i}");

            // Slack variable - bundle radius minus the inner circle radius, 0 <= x < infinity
            var circleRadiiSlackSquared = new IntVar[radii.Count];
            for (var i = 0; i < radii.Count; i++)
                circleRadiiSlackSquared[i] = model.NewIntVar(0, uint.MaxValue, $"s_r_s_{i}");

            // Slack variable - The difference of the centers of two inner circles on a single axis,
            // -2 * the sum of radii <= x <= 2 * the sum of radii
            var circleDistances = new IntVar[radii.Count, radii.Count, dimension];
            for (var i = 0; i < radii.Count; i++)
            for (var j = 0; j < radii.Count; j++)
            for (var k = 0; k < dimension; k++)
                circleDistances[i, j, k] = model.NewIntVar(-domainConstraint, domainConstraint, $"s_d_{i}_{j}_{k}");

            // The square distance of the centers of two inner circles on a single axis, 0 <= x < infinity
            var circleDistancesSquared = new IntVar[radii.Count, radii.Count, dimension];
            for (var i = 0; i < radii.Count; i++)
            for (var j = 0; j < radii.Count; j++)
            for (var k = 0; k < dimension; k++)
                circleDistancesSquared[i, j, k] = model.NewIntVar(0, uint.MaxValue, $"s_d_s_{i}_{j}_{k}");

            // The sum of radii of two inner circles, 0 <= x <= 2 * the sum of radii
            var radiiSum = new IntVar[radii.Count, radii.Count];
            for (var i = 0; i < radii.Count; i++)
            for (var j = 0; j < radii.Count; j++)
                radiiSum[i, j] = model.NewIntVar(0, domainConstraint, $"s_rs_{i}_{j}");

            // Slack variable - The square of the sum of radii of two inner circles, 0 <= x <= infinity
            var radiiSumSquared = new IntVar[radii.Count, radii.Count];
            for (var i = 0; i < radii.Count; i++)
            for (var j = 0; j < radii.Count; j++)
                radiiSumSquared[i, j] = model.NewIntVar(0, uint.MaxValue, $"s_rs_s_{i}_{j}");

            // ----------------------------------------------------------------
            // Constraint declaration
            // ----------------------------------------------------------------

            // (1) Bundle radius lower bound
            model.AddMaxEquality(bundleRadius, circleRadii);


            // (2) All inner circles are fully contained in the bundle
            for (var i = 0; i < radii.Count; i++)
            {
                // The solver doesn't support the multiplication of negative variables! Further constraints or domain limitation is needed.
                /*
                model.Add(circleCenters[i, 0] >= 0);
                model.Add(circleCenters[i, 1] >= 0);
                model.Add(circleRadiiSlack[i] >= 0);
                */

                model.AddProdEquality(circleCentersSquared[i, 0],
                    new List<IntVar> {circleCenters[i, 0], circleCenters[i, 0]});

                model.AddProdEquality(circleCentersSquared[i, 1],
                    new List<IntVar> {circleCenters[i, 1], circleCenters[i, 1]});

                // NOT FEASIBLE with this constraint
                model.Add(bundleRadius == circleRadiiSlack[i] + circleRadii[i]);

                model.AddProdEquality(circleRadiiSlackSquared[i], new[] {circleRadiiSlack[i], circleRadiiSlack[i]});

                // NOT FEASIBLE with this constraint
                model.Add(circleCentersSquared[i, 0] + circleCentersSquared[i, 1] <= circleRadiiSlackSquared[i]);
            }


            // (3) No two inner circles overlap
            for (var i = 0; i < radii.Count; i++)
            for (var j = 0; j < radii.Count; j++)
            {
                if (i == j) continue;

                // The solver doesn't support the multiplication of negative variables! Further constraints or domain limitation is needed.
                /*
                    model.Add(circleDistances[i, j, 0] >= 0);
                    model.Add(circleDistances[i, j, 1] >= 0);
                    */

                model.Add(circleDistances[i, j, 0] == circleCenters[i, 0] - circleCenters[j, 0]);
                model.Add(circleDistances[i, j, 1] == circleCenters[i, 1] - circleCenters[j, 1]);

                model.AddProdEquality(circleDistancesSquared[i, j, 0],
                    new List<IntVar> {circleDistances[i, j, 0], circleDistances[i, j, 0]});

                model.AddProdEquality(circleDistancesSquared[i, j, 1],
                    new List<IntVar> {circleDistances[i, j, 1], circleDistances[i, j, 1]});

                model.Add(radiiSum[i, j] == circleRadii[i] + circleRadii[j]);

                // NOT FEASIBLE with this constraint
                model.AddProdEquality(radiiSumSquared[i, j], new List<IntVar> {radiiSum[i, j], radiiSum[i, j]});

                model.Add(circleDistancesSquared[i, j, 0] + circleDistancesSquared[i, j, 1] >= radiiSumSquared[i, j]);
            }


            // ----------------------------------------------------------------
            // Target declaration
            // ----------------------------------------------------------------

            // Minimize the radius of the bundle
            model.Minimize(bundleRadius);

            var solver = new CpSolver();
            var status = solver.Solve(model);

            if (status != CpSolverStatus.Feasible && status != CpSolverStatus.Optimal)
                throw new NotImplementedException();

            var bundle = new Circle((long) solver.Value(bundleRadius), new Point(0, 0));
            var innerCircles = new List<Circle>();

            for (var i = 0; i < radii.Count; i++)
                innerCircles.Add(new Circle((long) solver.Value(circleRadii[i]),
                    new Point(solver.Value(circleCenters[i, 0]), solver.Value(circleCenters[i, 1]))));

            return new PackingResultWrapper(bundle, innerCircles);
        }
    }

    internal interface IPackingAlgorithm
    {
        PackingResultWrapper Run(List<long> radii);
    }

    internal class CablePacking
    {
        private readonly IPackingAlgorithm _packingAlgorithm;

        public CablePacking(IPackingAlgorithm packingAlgorithm)
        {
            _packingAlgorithm = packingAlgorithm;
        }

        public PackingResultWrapper GetCablePacking(List<long> diameters)
        {
            return _packingAlgorithm.Run(diameters);
        }
    }
}
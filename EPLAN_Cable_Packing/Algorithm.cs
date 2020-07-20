﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace EPLAN_Cable_Packing
{
    /**
     * Place the circle closest to the center of the containing circle while not touching
     * any other circles.
     */
    internal class GreedyPackingAlgorithm : IPackingAlgorithm
    {
        private long _gridSize;
        private int _roughnessMultiplier;
        private List<Circle> _circlePlacements;

        // Avoid the float operations used in Math.Pow
        private static long Power(long number, long power)
        {
            return power == 1 ? number : number * Power(number, power - 1);
        }

        private static long SquareDistance(Point point1, Point point2)
        {
            return (Power(point1.X - point2.X, 2) + Power(point1.Y - point2.Y, 2));
        }

        private static long GetRoughnessMultiplier(long inputSize)
        {
            return inputSize / 1000;
            //return inputSize / (300 + 7000 / (1 + Power(inputSize / 40000, 40)));
        }

        public PackingResultWrapper Run(List<int> radii)
        {
            _gridSize = radii.Sum() * 2;
            _roughnessMultiplier = (int) GetRoughnessMultiplier(_gridSize);
            var bundleCenter = new Point(_gridSize / 2, _gridSize / 2);

            radii.Sort((x, y) => y.CompareTo(x));
            _circlePlacements = new List<Circle>();

            PlaceCircles(radii, bundleCenter);

            var (leftMostCoordinate, rightMostCoordinate, lowerMostCoordinate, upperMostCoordinate) = GetPackingBoundaries();
            var lowestBundleRadius = Math.Max(rightMostCoordinate - leftMostCoordinate,
                upperMostCoordinate - lowerMostCoordinate) / 2;

            bundleCenter.X = leftMostCoordinate + (rightMostCoordinate - leftMostCoordinate) / 2;
            bundleCenter.Y = lowerMostCoordinate + (upperMostCoordinate - lowerMostCoordinate) / 2;

            int actualBundleRadius;
            (bundleCenter, actualBundleRadius) = PlaceBundle(lowestBundleRadius, bundleCenter);

            //bundleCenter.X = leftMostCoordinate + actualBundleDiameter;
            //bundleCenter.Y = lowerMostCoordinate + actualBundleDiameter;

            return new PackingResultWrapper(new Circle(actualBundleRadius, bundleCenter) {Color =  Color.Lime}, _circlePlacements);
        }

        private (Point newBundleCenter, int actualBundleRadius) PlaceBundle(long lowestBundleRadius, Point bundleCenter)
        {
            var leftMostInterval = bundleCenter.X - lowestBundleRadius / 4;
            var rightMostInterval = bundleCenter.X + lowestBundleRadius / 4;
            var lowerMostInterval = bundleCenter.Y - lowestBundleRadius / 4;
            var upperMostInterval = bundleCenter.Y + lowestBundleRadius / 4;

            var newBundleCenter = bundleCenter;
            var actualBundleRadius = (int) lowestBundleRadius;

            for (; actualBundleRadius < _gridSize / 2; actualBundleRadius += _roughnessMultiplier)
            {
                var intersectsAnotherCircle = false;

                foreach (var point in bundleCenter.GetInterval(new Point(leftMostInterval, lowerMostInterval),
                    new Point(rightMostInterval, upperMostInterval), _roughnessMultiplier / 2))
                {
                    intersectsAnotherCircle = false;

                    foreach (var circle in _circlePlacements)
                    {
                        if (SquareDistance(circle.Center, point) >
                            Power(actualBundleRadius - circle.Radius, 2))
                        {
                            intersectsAnotherCircle = true;
                            break;
                        }
                    }

                    if (!intersectsAnotherCircle)
                    {
                        newBundleCenter = point;
                        break;
                    }
                }

                if (!intersectsAnotherCircle) break;
            }

            return (newBundleCenter, actualBundleRadius);
        }

        /*
        private int GetActualBundleDiameter(long lowestBundleDiameter, Point bundleCenter)
        {
            var actualBundleDiameter = (int) lowestBundleDiameter;

            for (; actualBundleDiameter < _gridSize / 2; actualBundleDiameter += _roughnessMultiplier)
            {
                var intersectsAnotherCircle = false;

                foreach (var circle in _circlePlacements)
                {
                    if (SquareDistance(circle.Center, bundleCenter) > Power(actualBundleDiameter - circle.Diameter, 2))
                    {
                        intersectsAnotherCircle = true;
                        break;
                    }
                }

                if (!intersectsAnotherCircle) break;
            }

            return actualBundleDiameter;
        }
        */

        private (long leftMostCoordinate, long rightMostCoordinate, long lowerMostCoordinate, long upperMostCoordinate)
            GetPackingBoundaries()
        {
            var leftMostCoordinate = long.MaxValue;
            var rightMostCoordinate = long.MinValue;
            var lowerMostCoordinate = long.MaxValue;
            var upperMostCoordinate = long.MinValue;

            foreach (var circle in _circlePlacements)
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

            /*
            for (var i = 0; i < _gridSize; i += _roughnessMultiplier)
            {
                for (var j = 0; j < _gridSize; j += _roughnessMultiplier)
                {
                    if (!CoordinateContainsCircle(i, j)) continue;

                    if (i < leftMostCoordinate)
                        leftMostCoordinate = i;

                    if (i > rightMostCoordinate)
                        rightMostCoordinate = i;

                    if (j < lowerMostCoordinate)
                        lowerMostCoordinate = j;

                    if (j > upperMostCoordinate)
                        upperMostCoordinate = j;
                }
            }
            */

            return (leftMostCoordinate, rightMostCoordinate, lowerMostCoordinate, upperMostCoordinate);
        }

        private void PlaceCircles(List<int> radii, Point bundleCenter)
        {
            foreach (var radius in radii)
            {
                var bestSquareDistance = long.MaxValue;
                var bestCenter = new Point(int.MaxValue, int.MaxValue);

                // x axis
                for (var i = 0; i < _gridSize; i += _roughnessMultiplier)
                {
                    // y axis
                    for (var j = 0; j < _gridSize; j += _roughnessMultiplier)
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
                }

                if (!(bestSquareDistance < long.MaxValue)) throw new NotImplementedException();

                _circlePlacements.Add(new Circle(radius, bestCenter));
            }
        }

        private bool CoordinateContainsCircle(long coordinateX, long coordinateY)
        {
            //var containsAnotherCircle = false;

            foreach (var circle in _circlePlacements)
            {
                /*
                for (var currentDiameter = 1; currentDiameter <= circle.Diameter; currentDiameter += _multiplier)
                {
                    for (var currentAngle = 0; currentAngle < 360; currentAngle += 10)
                    {
                        var x = (int)(currentDiameter * Math.Cos(currentAngle * Math.PI / 180));
                        var y = (int)(currentDiameter * Math.Sin(currentAngle * Math.PI / 180));

                        if (circle.Center.X + x != coordinateX && circle.Center.Y + y != coordinateY) continue;
                        
                        containsAnotherCircle = true;
                        break;
                    }

                    if (containsAnotherCircle)
                        break;
                }

                if (containsAnotherCircle)
                    break;
                */

                if (SquareDistance(new Point(coordinateX, coordinateY), circle.Center) < Power(circle.Radius, 2))
                    return true;
            }

            return false;
        }

        private bool CircleContainsAnotherCircle(int radius, long circleCenterX, long circleCenterY)
        {
            foreach (var circle in _circlePlacements)
            {
                if (SquareDistance(new Point(circleCenterX, circleCenterY), circle.Center) <
                    Power(circle.Radius + radius, 2))
                    return true;
            }

            return false;
            /*
            var intersectsAnotherCircle = false;

            for (var currentDiameter = 1; currentDiameter <= diameter; currentDiameter += _multiplier)
            {
                for (var currentAngle = 0; currentAngle < 360; currentAngle += 10)
                {
                    var x = (int) (currentDiameter * Math.Cos(currentAngle * Math.PI / 180));
                    var y = (int) (currentDiameter * Math.Sin(currentAngle * Math.PI / 180));

                    // The circle reaches beyond the bounds of the grid
                    if (circleCenterX + x < 0 || circleCenterX + x >= _gridSize)
                    {
                        intersectsAnotherCircle = true;
                        break;
                    }

                    // The circle reaches beyond the bounds of the grid
                    if (circleCenterY + y < 0 || circleCenterY + y >= _gridSize)
                    {
                        intersectsAnotherCircle = true;
                        break;
                    }

                    // The circle intersects another circle
                    if (!CoordinateContainsCircle(circleCenterX + x, circleCenterY + y)) continue;

                    intersectsAnotherCircle = true;
                    break;
                }

                if (intersectsAnotherCircle)
                    break;
            }

            return intersectsAnotherCircle;
            */
        }
    }

    internal class LinearProgrammingPackingAlgorithm : IPackingAlgorithm
    {
        public PackingResultWrapper Run(List<int> radii)
        {
            var returnValue = new PackingResultWrapper { Bundle = { Center = new Point(6, 2), Radius = 2 } };

            return returnValue;
        }
    }

    internal interface IPackingAlgorithm
    {
        PackingResultWrapper Run(List<int> radii);
    }

    internal class CablePacking
    {
        private readonly IPackingAlgorithm _packingAlgorithm;

        public CablePacking(IPackingAlgorithm packingAlgorithm)
        {
            _packingAlgorithm = packingAlgorithm;
        }

        public PackingResultWrapper GetCablePacking(List<int> diameters)
        {
            return _packingAlgorithm.Run(diameters);
        }
    }
}

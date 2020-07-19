using System;
using System.Collections.Generic;
using System.Text;

namespace EPLAN_Cable_Packing
{
    internal class GreedyPackingAlgorithm : IPackingAlgorithm
    {
        public PackingResultWrapper Run(List<int> diameters)
        {

        }
    }

    internal class LinearProgrammingPackingAlgorithm : IPackingAlgorithm
    {
        public PackingResultWrapper Run(List<int> diameters)
        {

        }
    }

    internal interface IPackingAlgorithm
    {
        PackingResultWrapper Run(List<int> diameters);
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

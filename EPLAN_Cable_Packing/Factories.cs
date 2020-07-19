using System;
using System.Collections.Generic;
using System.Text;

namespace EPLAN_Cable_Packing
{
    internal enum Algorithms
    {
        Greedy,
        LinearProgramming
    }

    internal abstract class AlgorithmFactory
    {
        public abstract IPackingAlgorithm Create();
    }

    internal class GreedyAlgorithmFactory : AlgorithmFactory
    {
        public override IPackingAlgorithm Create()
        {
            return new GreedyPackingAlgorithm();
        }
    }

    internal class LinearProgrammingAlgorithmFactory : AlgorithmFactory
    {
        public override IPackingAlgorithm Create()
        {
            return new LinearProgrammingPackingAlgorithm();
        }
    }
}

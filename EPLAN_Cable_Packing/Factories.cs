using System;
using System.Collections.Generic;
using System.Text;

namespace EPLAN_Cable_Packing
{
    internal enum Algorithms
    {
        Greedy,
        SinglePass,
        IntegerProgramming
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

    internal class SinglePassAlgorithmFactory : AlgorithmFactory
    {
        public override IPackingAlgorithm Create()
        {
            return new SinglePassPackingAlgorithm();
        }
    }

    internal class IntegerProgrammingAlgorithmFactory : AlgorithmFactory
    {
        public override IPackingAlgorithm Create()
        {
            return new IntegerProgrammingPackingAlgorithm();
        }
    }
}

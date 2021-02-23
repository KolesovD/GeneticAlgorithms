using System.Collections.Generic;
using System.Numerics;

namespace GeneticAlgorithms
{
    public interface IPopulation
    {
        List<AbstractIndividual> CurrentGeneration { get; }
    }
}
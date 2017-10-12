using System.Collections.Generic;

namespace GeneticAlgorithms
{
    public interface IPopulation
    {
        List<AbstractIndividual> CurrentGeneration { get; }
    }
}
using System.Collections.Generic;

namespace GeneticAlgorithms
{
    public interface IPopulation
    {
        List<Iindividual> CurrentGeneration { get; }
    }
}
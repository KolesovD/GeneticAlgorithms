using System.Collections.Generic;

namespace GeneticAlgorithms.Delegates
{
    public delegate AbstractIndividual Crossover(AbstractIndividual parent1, AbstractIndividual parent2);
    public delegate void Mutator(AbstractIndividual target);
    public delegate void MutationFunction(List<Segment> segmentList);
}

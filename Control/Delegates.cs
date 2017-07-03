using System.Collections.Generic;

namespace GeneticAlgorithms.Delegates
{
    public delegate AbstractIndividual Crossover(AbstractIndividual parent1, AbstractIndividual parent2, AbstractIndividual child);
    public delegate void Mutator(List<Segment> segmentList);
}

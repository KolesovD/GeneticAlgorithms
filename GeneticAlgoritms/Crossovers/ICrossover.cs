namespace GeneticAlgorithms.Crossovers
{
    public interface ICrossover
    {
        void PerformCrossover(AbstractIndividual parent1, AbstractIndividual parent2, AbstractIndividual child);
    }
}

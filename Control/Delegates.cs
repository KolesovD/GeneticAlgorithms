namespace GeneticAlgorithms.Delegates
{
    public delegate Plate Crossover(Plate parent1, Plate parent2);
    public delegate void Mutator(Plate target);
}

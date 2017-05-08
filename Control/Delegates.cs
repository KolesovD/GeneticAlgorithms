namespace GeneticAlgorithms.Delegates
{
    public delegate Iindividual Crossover(Iindividual parent1, Iindividual parent2);
    public delegate void Mutator(Iindividual target);
	public delegate Iindividual CreateNewIindividual();
	public delegate Iindividual CopyFromIindividual(Iindividual _individual);
}

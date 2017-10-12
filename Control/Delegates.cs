namespace GeneticAlgorithms.Delegates
{
    public delegate void Crossover(Iindividual parent1, Iindividual parent2, Iindividual child);
    public delegate void Mutatordel(Iindividual target);
	public delegate Iindividual CreateNewIindividual();
	public delegate Iindividual CopyFromIindividual(Iindividual _individual);
}

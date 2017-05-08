using System;
namespace GeneticAlgorithms
{
	public abstract class Loader
	{
		public abstract Iindividual CreateNewIndividual();
		public abstract Iindividual CreateNewIndividualEmpty();
		public abstract Iindividual CopyFrom(Iindividual _individual);
		public abstract Iindividual Crossover(Iindividual parent1, Iindividual parent2);
		public abstract void Mutator(Iindividual target);
	}
}

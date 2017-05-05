using System;

namespace GeneticAlgorithms
{
	public class LoaderPlate : Loader
	{
		public override Iindividual CreateNewIndividual()
		{
			return new Plate();
		}

		public override Iindividual CopyFrom(Iindividual _individual)
		{
			return new Plate(_individual as Plate);
		}


	}
}

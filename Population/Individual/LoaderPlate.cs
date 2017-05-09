using System;

namespace GeneticAlgorithms
{
	public class LoaderPlate : Loader
	{
		public override Iindividual CreateNewIndividualEmpty()
		{
			return new Plate(true);
		}
		public override Iindividual CreateNewIndividual()
		{
			return new Plate(false);
		}

		public override Iindividual CopyFrom(Iindividual _individual)
		{
			return new Plate(_individual as Plate);
		}

		//public override Iindividual Crossover(Iindividual parent1, Iindividual parent2)
		//{
		//	//тестовая версия с логом
		//	//здесь пишем код кроссовера
		//	Console.WriteLine("Crossover is work");
		//	return parent1;
		//}

		//public override void Mutator(Iindividual target)
		//{
		//	//тестовая версия с логом
		//	//здесь пишем код мутатора
		//	Console.WriteLine("Mutator is work");
		//}
	}
}
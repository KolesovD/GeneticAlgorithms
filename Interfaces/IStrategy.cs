using System;
namespace GeneticAlgorithms
{
	public interface IStrategy {
		Func<Loader> Load { get; set; }
		Func<int> populationSize { get; set; }
		Func<double> fractionOfNewIndividuals { get; set; }
		Func<double> mutationProbability { get; set; }
		Action ProgrammRun { get; set; }
	}
}

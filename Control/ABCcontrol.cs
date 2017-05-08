using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneticAlgorithms.Delegates;

namespace GeneticAlgorithms
{
    public class ABCcontrol
    {
        private int populationSize;
		private Roulette _Roulete;
		private Population _Population;

        //private Population population;

        //public bool AllowParentsIntoNewGenerations = true;
        private double _FractionOfNewIndividuals = 0.25; //изменяется от 0 до 1
        private double _MutationProbability = 0.01; //изменяется от 0 до 1

		public double FractionOfNewIndividuals {
			get { return _FractionOfNewIndividuals; }
			set {
				if (value < 0) { _FractionOfNewIndividuals = 0; }
				else if (value > 1) { _FractionOfNewIndividuals = 1; }
				else { _FractionOfNewIndividuals = value; }
			}
		}

		public double MutationProbability {
			get { return _MutationProbability; }
			set { 
				if (value < 0) { _MutationProbability = 0; }
				else if (value > 1) { _MutationProbability = 1; }
				else { _MutationProbability = value; }
			}
		}
		public Roulette GetRoulette {
			get { return _Roulete; }
		}
		public int GetPopulationSize {
			get { return populationSize; }
		}
		public Population GetPopulation {
			get { return _Population; }
		}

		public ABCcontrol(Loader Load, int populationSize = 50, double fractionOfNewIndividuals = 0.25, double mutationProbability = 0.01)
		{
			this.populationSize = populationSize;
			this.FractionOfNewIndividuals = fractionOfNewIndividuals;
			this.MutationProbability = mutationProbability;

			_Population = new Population(Load, this);
			_Roulete = new Roulette(this.populationSize);
		}

		public virtual void ProgramRuning() {}

		//public void Optimize(Crossover crossover, Mutator mutator)
		//{
		//    population.Crossover = crossover;
		//    population.Mutator = mutator;
		//    population.PerformSelection();
		//    population.PerformCrossing(crossover);
		//    population.PerformMutation(mutator);
		//    population.SwitchGenerations();
		//    CurrentGenerationNumber++;
		//}
	}
}

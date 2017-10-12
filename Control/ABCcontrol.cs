using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GeneticAlgorithms.Delegates;

namespace GeneticAlgorithms
{
    public class ABCcontrol
    {
		private Roulette _Roulete;
		private Population _Population;

		public Roulette GetRoulette {
			get { return _Roulete; }
		}
		public Population GetPopulation {
			get { return _Population; }
		}

		public Func<Loader> Load { get; set; }
		public Func<int> SizeOfPopulation { get; set; }
		public Func<double> fractionOfNewIndividuals { get; set; }
		public Func<double> mutationProbability { get; set; }
		public Action ProgrammRun { get; set; }

		public ABCcontrol() {
			
		}

		//public void ProgrammRun(ABCcontrol controll) { }

		public void Configurate(Action<ABCcontrol> conf) {
            conf(this);
			_Population = new Population(Load.Invoke(), this);
			_Roulete = new Roulette(SizeOfPopulation.Invoke());
			Thread tr = new Thread(new ThreadStart(ProgrammRun));
			tr.Start();
			//ProgrammRun.BeginInvoke(null, null);
		}

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithms
{
	class Program {
		//static ABCcontrol Controll;
		static void Main(string[] args) {
			LoaderPlate load = new LoaderPlate("../../Population/Individual/Picture.xml");
			ABCcontrol contr = new ABCcontrol();
			contr.Configurate((obj) => {
				obj.Load = () => load;
				obj.SizeOfPopulation = () => 50;
				obj.fractionOfNewIndividuals = () => 0.25;
				obj.mutationProbability = () => 0.01;
				obj.ProgrammRun = () => {
					Console.WriteLine("Size Of Population = {0}", obj.SizeOfPopulation());
					Console.WriteLine("fraction Of New Individuals = {0}", obj.fractionOfNewIndividuals());
					Console.WriteLine("mutation Probability = {0}", obj.mutationProbability());
				};
			});
		}
	}
}
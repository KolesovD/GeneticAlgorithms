using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneticAlgorithms.Delegates;
using Assets.MyRandoms;

namespace GeneticAlgorithms
{
    public class Population : IPopulation
    {
		private ABCcontrol _core;
		private List<Iindividual> firstGeneration;
		private List<Iindividual> secondGeneration;

		//private Crossover Cross;
		//private Mutator Mutate;
		private CreateNewIindividual CreateIindividual;
		private CopyFromIindividual CopyIindividual;
		private CreateNewIindividual CreateIindividualEmpty;

        public bool currentGenerationFlag = true; //true = первая популяция является текущей

		public void LoadDelegates(Loader _Load) {
			//Cross = _Load.Crossover;
			//Mutate = _Load.Mutator;
			CreateIindividual = _Load.CreateNewIndividual;
			CopyIindividual = _Load.CopyFrom;
			CreateIindividualEmpty = _Load.CreateNewIndividualEmpty;
		}

        public Population(Loader _Load, ABCcontrol core, List<Iindividual> firstGeneration, List<Iindividual> secondGeneration)
        {
			LoadDelegates(_Load);
			_core = core;
            this.firstGeneration = firstGeneration;
            this.secondGeneration = secondGeneration;
        }

        public Population(Loader _Load, ABCcontrol core)
        {
            LoadDelegates(_Load);
            _core = core;
			firstGeneration = new List<Iindividual>();
			secondGeneration = new List<Iindividual>();
			Iindividual perfectPlate = CreateIindividual();
			for (int i = 0; i<_core.GetPopulationSize; i++)
            {
				Iindividual plate = CopyIindividual(perfectPlate);
				plate.Shuffle();
                firstGeneration.Add(plate);
				secondGeneration.Add(CreateIindividualEmpty());
            }
            //secondGeneration = new List<Iindividual>(firstGeneration);
        }

        //Вернуть ссылку на текущее поколение
        public List<Iindividual> CurrentGeneration
        {
            get
            {
                return currentGenerationFlag ? firstGeneration : secondGeneration;
            }

        }

        //Вернуть ссылку на не текущее поколение
        public List<Iindividual> AnotherGeneration
        {
            get
            {
                return currentGenerationFlag ? secondGeneration : firstGeneration;
            }

        }

        //Вернуть ссылку на особь по индексу из текущей популяции
        public Iindividual GetPlateFromCurrentPopulation(int index)
        {
            return CurrentGeneration[index];
        }

        //Вернуть ссылку на особь по индексу из не текущей популяции
        public Iindividual GetIndividualFromAnotherPopulation(int index)
        {
            return AnotherGeneration[index];
        }

		//Возвращает среднее значение фитнес функции в текущем поколении
		public double СalculationFitnessFunctionOfGeneration() {
			double result = 0;
			for (int i = 0; i < CurrentGeneration.Count; i++) {
				result += CurrentGeneration[i].GetFitnessFunction;
			}
			return result/CurrentGeneration.Count;
		}

        public void Mutation(Mutator MutateDelegate) //мутирует текущее поколение в соответствии с _core.MutationProbability
        {
			int count = (int)((Convert.ToDouble(CurrentGeneration.Count) / 100.0d) * Convert.ToDouble(_core.MutationProbability));
			for (int i = 0; i < count; i++) {
				MutateDelegate(CurrentGeneration[MyRandom.rnd.Next(CurrentGeneration.Count)]);
			}
        }

        public void Crossover(Crossover crossover) //получает из рулетки особи из текущего поколения и формирует неактивное поколение
        {
			_core.GetRoulette.LoadByPopulation(this);

			for (int i = 0; i < AnotherGeneration.Count; i++) {
				crossover(GetIndividualFromAnotherPopulation(_core.GetRoulette.PickIndividualIndex()), GetIndividualFromAnotherPopulation(_core.GetRoulette.PickIndividualIndex()), AnotherGeneration[i]);
			}

            //for (int i = 1; i < firstGeneration.Count; i++)
            //{
            //    Population p = new Population();
            //    crossover(p.GetPlateFromCurrentPopulation(i), p.GetPlateFromCurrentPopulation(i - 1));
            //}
            //делегат возвращает AbstractIndividual
            //использовать для создания новых особей
            //создание из 2 особей 1 или нескольких особей по команде
        }

    }
}

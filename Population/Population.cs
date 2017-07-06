using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.MyRandoms;

namespace GeneticAlgorithms
{
    class Population : IPopulation
    {
        private List<AbstractIndividual> firstGeneration = new List<AbstractIndividual>();
        private List<AbstractIndividual> secondGeneration = new List<AbstractIndividual>();
        private bool currentGenerationFlag = true; //true = первая популяция является текущей
        private int currentGenerationNumber = 0;

        //Заполнение популяций особями
        //ВНИМАНИЕ!!! Отладочный метод, использующий объекты класса Plate, а не интерфейсы
        public Population(int count = 50)
        {
            Plate perfectPlate = new Plate();

            Console.WriteLine("Эталон: ");

            for (int i = 0; i < 10; i++)
            {
                perfectPlate.AddSegment(new Segment(i, i, i, i + 1, i + 1, true));
                Console.WriteLine($"New segment ID: {i}, [{i},{i}];[{i+1},{i+1}], direction: {true}");
            }

            Console.ReadKey();
            Console.WriteLine("\nСтартовая популяция: ");

            for (int i = 0; i < count; i++)
            {
                Plate plate = new Plate(perfectPlate);
                plate.ShuffleSegments();
                firstGeneration.Add(plate);
                Console.WriteLine(plate.ToString());
            }

            Console.ReadKey();

            secondGeneration = new List<AbstractIndividual>(firstGeneration);

        }

        //Вернуть ссылку на текущее поколение
        public List<AbstractIndividual> CurrentGeneration
        {
            get
            {
                return currentGenerationFlag ? firstGeneration : secondGeneration;
            }

        }

        //Вернуть ссылку на не текущее поколение
        public List<AbstractIndividual> AnotherGeneration
        {
            get
            {
                return currentGenerationFlag ? secondGeneration : firstGeneration;
            }
        }

        public void SwitchGenerations()
        {
            currentGenerationFlag = !currentGenerationFlag;
            currentGenerationNumber++;
        }

        //Вернуть ссылку на особь по индексу из текущей популяции
        public AbstractIndividual GetPlateFromCurrentPopulation(int index)
        {
            return CurrentGeneration[index];
        }

        //Вернуть ссылку на особь по индексу из не текущей популяции
        public AbstractIndividual GetIndividualFromAnotherPopulation(int index)
        {
            return AnotherGeneration[index];
        }

        public void PerformMutation(Delegates.Mutator mutator, double mutationProbability) //Использовать mutationProbability
        {
            foreach (AbstractIndividual individual in CurrentGeneration)
            {
                individual.Mutate(mutator); //Добавить вероятность как параметр в Mutate
            }
        }

        public void PerformCrossingover(Delegates.Crossover crossover, int[] indexesForCrossover)
        {
            //for (int i = 0, j = 0; i < indexesForCrossover.Length/2 - 1; i++, j++)
            //{
            //    crossover(CurrentGeneration[indexesForCrossover[i]], CurrentGeneration[indexesForCrossover[i + 1]], AnotherGeneration[j]);
            //    crossover(CurrentGeneration[indexesForCrossover[i+1]], CurrentGeneration[indexesForCrossover[i]], AnotherGeneration[++j]);
            //    //j++;
            //}

            for (int i = 0; i < indexesForCrossover.Count() / 2; i++) 
            {
                crossover(CurrentGeneration[indexesForCrossover[i]], CurrentGeneration[indexesForCrossover[i + 1]], AnotherGeneration[j]);
                crossover(CurrentGeneration[indexesForCrossover[i + 1]], CurrentGeneration[indexesForCrossover[i]], AnotherGeneration[++j]);

            }
        }

        public AbstractIndividual GetBestIndividual()
        {
            double maxFitness = CurrentGeneration[0].FitnessFunction;

            int maxIndex = 0;
            for (int i = 1; i < CurrentGeneration.Count; i++)
            {
                if (CurrentGeneration[i].FitnessFunction > maxFitness)
                {
                    maxFitness = CurrentGeneration[i].FitnessFunction;
                    maxIndex = i;
                }
            }
            Console.WriteLine("Лучший в поколении №" + currentGenerationNumber + "\n"  + CurrentGeneration[maxIndex]);
            return CurrentGeneration[maxIndex];
        }

        public override String ToString()
        {

            string sas = "Поколение №" + currentGenerationNumber + "\n";

            foreach (var item in CurrentGeneration)
            {
                sas += item.ToString() + "\n";
            }

            return sas; 
        }

    }
}

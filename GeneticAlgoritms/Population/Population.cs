using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Assets.MyRandoms;
using GeneticAlgorithms.Information;

namespace GeneticAlgorithms
{
    class Population : IPopulation
    {
        private List<AbstractIndividual> firstGeneration = new List<AbstractIndividual>();
        private List<AbstractIndividual> secondGeneration = new List<AbstractIndividual>();
        private bool currentGenerationFlag = true; //true = первая популяция является текущей
        
        public int currentGenerationNumber { get; private set; }

        public Population()
        {

        }

        public void RemoveAt(int index) {
            firstGeneration.RemoveAt(index);
            secondGeneration.RemoveAt(index);
        }

        public void Add(AbstractIndividual item) {
            CurrentGeneration.Add(item);
            AnotherGeneration.Add(item.GetCopy());//возможно можно создавать пустую
        }

        //Заполнение популяций особями
        //ВНИМАНИЕ!!! Отладочный метод, использующий объекты класса Plate, а не интерфейсы
        public void CreateStartingPopulation(string XML_path, int count = 50)
        {
            XMLLoader load = new XMLLoader(XML_path);
            Plate perfectPlate = load.Parse();
            //Создание стартовой популяции
            for (int i = 0; i < count; i++)
            {
                Plate plate = new Plate(perfectPlate);
                plate.ShuffleSegments();
                firstGeneration.Add(plate);
            }

            secondGeneration = new List<AbstractIndividual>(firstGeneration);
            currentGenerationNumber = 0;
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

        public int GenerationSize
        {
            get { return CurrentGeneration.Count; }
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

        public void PerformMutation(Delegates.Mutator mutator)
        {
            AbstractIndividual bestOne = GetBestIndividual();
            Parallel.ForEach<AbstractIndividual>(CurrentGeneration, (individual) =>
            {
                if (individual != bestOne)
                {
                    individual.Mutate(mutator);
                }
            });
            //foreach (AbstractIndividual individual in CurrentGeneration)
            //{
            //    if (individual != bestOne)
            //        individual.Mutate(mutator);
            //}
        }

        public void PerformCrossingover(Delegates.Crossover crossover, int[] indexesForCrossover)
        {
            //AnotherGeneration.Sort(AnotherGeneration[AnotherGeneration.Count-1]);//в конце лучшие

            //double average = AnotherGeneration.AsParallel().Sum((i) => i.FitnessFunction); //Среднее значение фитнес-функции
            //average = average / AnotherGeneration.Count();

            //int[] badIndexes = new int[indexesForCrossover.Count()];
            //int k = 0;
            //for (int i = 0; i < AnotherGeneration.Count; i++)
            //{
            //    if (AnotherGeneration[i].FitnessFunction < average)
            //    {
            //        if (k == indexesForCrossover.Count())
            //        {
            //            break;
            //        }
            //        badIndexes[k] = i;
            //        k++;
            //    }
            //}

            //if (k < indexesForCrossover.Length/3)
            //{
            //    Console.WriteLine("true");
            //    for (int i = k + 1; i < badIndexes.Length; i++)
            //    {
            //        badIndexes[i] = MyRandom.rnd.Next(0, AnotherGeneration.Count() - 1);
            //    }
            //}

            //int another_gen_count = AnotherGeneration.Count;
            for (int i = 0; i < indexesForCrossover.Length - indexesForCrossover.Length % 2; i += 2)
            {
                //double child_individual, parent_01_individual, parent_02_individual;
                //Console.WriteLine("i = {0}",i);
                //Для создания двух разных потомков
                //Скрещиваем 1 особь со 2 особъю
                crossover(
                    CurrentGeneration[indexesForCrossover[i]],
                    CurrentGeneration[indexesForCrossover[i + 1]],
                    AnotherGeneration[i]
                    );
                //child_individual = AnotherGeneration[i].FitnessFunction;
                //parent_01_individual = CurrentGeneration[indexesForCrossover[i]].FitnessFunction;
                //parent_02_individual = CurrentGeneration[indexesForCrossover[i + 1]].FitnessFunction;
                //if ((child_individual < parent_01_individual) && (child_individual < parent_02_individual))
                //{
                //    AnotherGeneration[i] = CurrentGeneration[indexesForCrossover[i]].GetCopy(CurrentGeneration[indexesForCrossover[i]]);
                //}
                //Скрещиваем 2 особь с 1 особъю
                crossover(
                    CurrentGeneration[indexesForCrossover[i + 1]],
                    CurrentGeneration[indexesForCrossover[i]],
                    AnotherGeneration[i+1]
                    );
                //child_individual = AnotherGeneration[i+1].FitnessFunction;
                //parent_01_individual = CurrentGeneration[indexesForCrossover[i+1]].FitnessFunction;
                //parent_02_individual = CurrentGeneration[indexesForCrossover[i]].FitnessFunction;
                //if ((child_individual < parent_01_individual) && (child_individual < parent_02_individual))
                //{
                //    AnotherGeneration[i+1] = CurrentGeneration[indexesForCrossover[i+1]].GetCopy(CurrentGeneration[indexesForCrossover[i+1]]);
                //}
            }

            //if (indexesForCrossover.Length % 2 != 0)
            //{
            //    crossover(
            //        CurrentGeneration[indexesForCrossover[indexesForCrossover.Count() - 2]],
            //        CurrentGeneration[indexesForCrossover[indexesForCrossover.Count() - 1]],
            //        AnotherGeneration[indexesForCrossover.Length - indexesForCrossover.Length % 2]
            //        );
            //}
        }

        private int get_best_in_bit(int start, int count) {
            double maxFitness = CurrentGeneration[start].FitnessFunction;

            int index = start;
            for (int i = start + 1; i < start + count; i++) {
                double _prom = CurrentGeneration[i].FitnessFunction;
                if (_prom > maxFitness) {
                    maxFitness = _prom;
                    index = i;
                }
            }
            return index;
        }

        public AbstractIndividual GetBestIndividual()
        {
            int lenght = Environment.ProcessorCount;
            Task<int>[] tasks = new Task<int>[lenght];
            int start = 0;
            int count = CurrentGeneration.Count() / lenght;
            for (int i = 0; i < lenght; i++)
            {
                if (i != lenght - 1)
                {
                    tasks[i] = new Task<int>(() => get_best_in_bit(start, count));
                    start += count;
                }
                else {
                    tasks[i] = new Task<int>(() => get_best_in_bit(start, CurrentGeneration.Count() - start));
                }
                tasks[i].Start();
            }
            Task.WaitAll(tasks);
            double maxFitness = CurrentGeneration[tasks[0].Result].FitnessFunction;

            int maxIndex = tasks[0].Result;
            for (int i = 1; i < lenght; i++)
            {
                double _prom = CurrentGeneration[tasks[i].Result].FitnessFunction;
                if (_prom > maxFitness)
                {
                    maxFitness = _prom;
                    maxIndex = tasks[i].Result;
                }
            }
            return CurrentGeneration[maxIndex];
        }

        public override String ToString()
        {
            string sas = string.Format("Поколение № {0}", currentGenerationNumber);
            return sas;
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
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

            double average = AnotherGeneration.AsParallel().Sum((i) => i.FitnessFunction); //Среднее значение фитнес-функции
            average = average / AnotherGeneration.Count();

            int[] badIndexes = new int[indexesForCrossover.Count()];
            int k = 0;
            for (int i = 0; i < AnotherGeneration.Count; i++)
            {
                if (AnotherGeneration[i].FitnessFunction < average)
                {
                    if (k == indexesForCrossover.Count())
                    {
                        break;
                    }
                    badIndexes[k] = i;
                    k++;
                }
            }
            for (int i = k + 1; i < AnotherGeneration.Count; i++)
            {
                badIndexes[k] = MyRandom.rnd.Next(0, AnotherGeneration.Count() - 1);
            }

            //Parallel.For(0, indexesForCrossover.Count() / 2, i =>
            //{
            //    int ind = (2 * i);
            //    //Для создания двух разных потомков
            //    //Скрещиваем 1 особь со 2 особъю
            //    crossover(
            //        CurrentGeneration[indexesForCrossover[ind]],
            //        CurrentGeneration[indexesForCrossover[ind + 1]],
            //        AnotherGeneration[badIndexes[ind]]
            //        );
            //    //Скрещиваем 2 особь с 1 особъю
            //    crossover(
            //        CurrentGeneration[indexesForCrossover[ind + 1]],
            //        CurrentGeneration[indexesForCrossover[ind]],
            //        AnotherGeneration[badIndexes[ind + 1]]
            //        );
            //});

            int j = 0;
            for (int i = 0; i < indexesForCrossover.Count() / 2; i += 2, j++)
            {
                //Для создания двух разных потомков
                //Скрещиваем 1 особь со 2 особъю
                crossover(
                    CurrentGeneration[indexesForCrossover[i]],
                    CurrentGeneration[indexesForCrossover[i + 1]],
                    AnotherGeneration[badIndexes[j]]
                    );
                //Скрещиваем 2 особь с 1 особъю
                crossover(
                    CurrentGeneration[indexesForCrossover[i + 1]],
                    CurrentGeneration[indexesForCrossover[i]],
                    AnotherGeneration[badIndexes[++j]]
                    );
            }

            if (badIndexes.Length % 2 != 0)
            {
                crossover(
                    CurrentGeneration[indexesForCrossover[indexesForCrossover.Count() - 2]],
                    CurrentGeneration[indexesForCrossover[indexesForCrossover.Count() - 1]],
                    AnotherGeneration[badIndexes[badIndexes.Length - 1]]
                    );
            }
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

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
        public void CreateStartingPopulation(string XML_path ,int count = 50)
        {
            XMLLoader load = new XMLLoader(XML_path);
            Plate perfectPlate = load.Parse();


            //Console.WriteLine("Эталон: ");
            //for (int i = 0; i < 10; i++)
            //{
            //    perfectPlate.AddSegment(new Segment(i, i, i , i + 1 , i + 1, true));
            //    //Вывод сегментов эталонного варианта
            //    //Console.WriteLine($"New segment ID: {i}, [{i},{i}];[{i + 1},{i + 1}], direction: {true}");
            //}

            //Остановка для просмотра сформированного эталона
            //Console.ReadKey();

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
            foreach (AbstractIndividual individual in CurrentGeneration)
            {
                if (individual != bestOne)
                    individual.Mutate(mutator);
            }
        }

        //private int AwerageByPie(List<AbstractIndividual> gen, int index_start, int index_end) {
            
        //}

        public void PerformCrossingover(Delegates.Crossover crossover, int[] indexesForCrossover)
        {

            double average = 0; //Среднее значение фитнес-функции
            for (int i = 0; i < AnotherGeneration.Count; i++)
            {
                average += AnotherGeneration[i].FitnessFunction;
            }

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

            int j = 0;
            for (int i = 0; i < indexesForCrossover.Count() / 2; i+=2, j++) 
            {
                //Для создания двух разных потомков
                //Скрещиваем 1 особь со 2 особъю
                if (k == -1) { break; }
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
                k--;
            }

            //if (indexesForCrossover.Count() % 2 != 0)
            //{
            //    crossover(
            //        CurrentGeneration[indexesForCrossover[indexesForCrossover.Count() - 2]],
            //        CurrentGeneration[indexesForCrossover[indexesForCrossover.Count() - 1]],
            //        AnotherGeneration[badIndexes[++j]]
            //        );
            //}
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
            return CurrentGeneration[maxIndex];
        }

        public override String ToString()
        {
            string sas = string.Format("Поколение № {0}", currentGenerationNumber);
            return sas; 
        }

    }
}

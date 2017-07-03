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
        private List<AbstractIndividual> firstGeneration;
        private List<AbstractIndividual> secondGeneration;
        private bool currentGenerationFlag = true; //true = первая популяция является текущей

        //Заполнение популяций особями
        //ВНИМАНИЕ!!! Отладочный метод, использующий объекты класса Plate, а не интерфейсы
        public Population(int count = 50)
        {
            Plate perfectPlate = new Plate();

            for (int i = 0; i < 10; i++)
            {
                perfectPlate.AddSegment(new Segment(i, i, i, i + 1, i + 1, true));
            }

            for (int i = 0; i < count; i++)
            {
                Plate plate = new Plate(perfectPlate);
                plate.ShuffleSegments();
                firstGeneration.Add(plate);
            }
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
            
            for (int i = 0, j = 0; i < indexesForCrossover.Length/2 - 1; i++, j++)
            {
                crossover(CurrentGeneration[indexesForCrossover[i]], CurrentGeneration[indexesForCrossover[i + 1]], AnotherGeneration[j]);
                crossover(CurrentGeneration[indexesForCrossover[i+1]], CurrentGeneration[indexesForCrossover[i]], AnotherGeneration[++j]);
                j++;
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithms
{
    class Population : IPopulation
    {
        private List<AbstractIndividual> firstGeneration;
        private List<AbstractIndividual> secondGeneration;
        public bool currentGenerationFlag = true; //true = первая популяция является текущей

        public Population(List<AbstractIndividual> firstGeneration, List<AbstractIndividual> secondGeneration)
        {
            this.firstGeneration = firstGeneration;
            this.secondGeneration = secondGeneration;
        }

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

        public void Mutation(Delegates.Mutator target)
        {
            
        }

        public void Crossover(Delegates.Crossover crossover)
        {
            for (int i = 1; i < firstGeneration.Count; i++)
            {
                Population p = new Population();
                crossover(p.GetPlateFromCurrentPopulation(i), p.GetPlateFromCurrentPopulation(i - 1));
            }
            //делегат возвращает AbstractIndividual
            //использовать для создания новых особей
            //создание из 2 особей 1 или нескольких особей по команде
        }

    }
}

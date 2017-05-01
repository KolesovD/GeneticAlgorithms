using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithms
{
    class Population : IPopulation
    {
        private List<Iindividual> firstGeneration;
        private List<Iindividual> secondGeneration;
        public bool currentGenerationFlag = true; //true = first is current

        public Population(List<Iindividual> firstGeneration, List<Iindividual> secondGeneration)
        {
            this.firstGeneration = firstGeneration;
            this.secondGeneration = secondGeneration;
        }

        //fill both populations using plates (debug method)
        public Population(int count = 50)
        {
            Iindividual perfectPlate = new Iindividual();

            for (int i = 0; i < 10; i++)
            {
                perfectPlate.AddSegment(new Segment(i, i, i, i + 1, i + 1, true));
            }

            for (int i = 0; i < count; i++)
            {
                Iindividual plate = new Iindividual(perfectPlate);
                plate.ShuffleSegments();
                firstGeneration.Add(plate);
            }
            secondGeneration = new List<Iindividual>(firstGeneration);
        }

        public List<Iindividual> CurrentGeneration
        {
            get
            {
                return currentGenerationFlag ? firstGeneration : secondGeneration;
            }

        }

        //return element by index in current population
        public Iindividual GetPlateFromCurrentPopulation(int index)
        {
            return CurrentGeneration[index];
        }

        public void Mutation(Delegate mutator)
        {

        }

        public void Crossover(Delegate del)
        {

        }


    }
}

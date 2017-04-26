using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaserLineOpt;

namespace GeneticAlgorithms.Generation.Individual
{
    class Population
    {
        private List<Plate> firstPopulation { get; set; }
        private List<Plate> secondPopulation { get; set; }
        public Boolean currentPopulation; //true = first is current

        public Population(List<Plate> firstPopulation, List<Plate> secondPopulation)
        {
            this.firstPopulation = firstPopulation;
            this.secondPopulation = secondPopulation;
        }

        //fill both populations
        public Population()
        {
            int count = 50; //Individuals count in population
            Plate perfectPlate = new Plate();
            perfectPlate.CreatePerfectIndividual();
            for (int i = 0; i < count; i++)
            {
                Plate plate = new Plate(perfectPlate);
                plate.ShuffleSegments();
                firstPopulation.Add(plate);
            }
            secondPopulation = firstPopulation;
        }

        public List<Plate> GetCurrentPopulation()
        {
            return currentPopulation ? firstPopulation : secondPopulation;
        }

        //return element by index in current population
        public Plate GetPlateFromCurrentPopulation(int index)
        {
            return GetCurrentPopulation()[index];
        }

        public void Mutation(Delegate mutator)
        {

        }

        public void Crossingover(Delegate del)
        {

        }


    }
}

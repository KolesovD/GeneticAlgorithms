using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithms
{
    class Control
    {
        private int populationSize;
        private Plate startingPlate;
        //private Population population;

        public bool AllowParentsIntoNewGenerations = true;
        public int CurrentGenerationNumber = 0;
        public double FractionOfNewIndividuals = 0.25;
        public double MutationProbability = 0.01;

        public Control(int populationSize, List<Segment> segmentList, double fractionOfNewIndividuals = 0.25, double mutationProbability = 0.01)
        {
            this.populationSize = populationSize;
            this.FractionOfNewIndividuals = fractionOfNewIndividuals;
            this.MutationProbability = mutationProbability;

            startingPlate = new Plate(segmentList);
            //population = new Population(populationCount, startingPlate)
        }

        public void Optimize(Crossover crossover, Mutator mutator)
        {
            //population.Crossover = crossover;
            //population.Mutator = mutator;
            //population.PerformSelection();
            //population.PerformCrossing(crossover);
            //population.PerformMutation(mutator);
            //population.SwitchGenerations();
            CurrentGenerationNumber++;
        }
    }
}

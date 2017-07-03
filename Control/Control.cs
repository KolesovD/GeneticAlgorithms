using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithms
{
    class Control
    {
        private int generationSize;
        private Plate startingPlate;
        private Population population;
        private Roulette roulette;


        public bool AllowParentsIntoNewGenerations = true;
        public int CurrentGenerationNumber = 0;
        public double FractionOfNewIndividuals = 0.25;
        public double MutationProbability = 0.01;

        public Control(int generationSize, double fractionOfNewIndividuals = 0.25, double mutationProbability = 0.01)
        {
            this.generationSize = generationSize;
            this.FractionOfNewIndividuals = fractionOfNewIndividuals;
            this.MutationProbability = mutationProbability;
            population = new Population(generationSize);
            roulette = new Roulette(this.generationSize);
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

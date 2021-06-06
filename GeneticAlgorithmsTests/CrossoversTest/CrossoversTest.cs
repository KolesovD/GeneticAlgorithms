using System.Linq;
using System.Collections.Generic;
using Xunit;
using GeneticAlgorithms;
using GeneticAlgorithms.Crossovers;
using GeneticAlgorithms.Mutations;
using GeneticAlgorithms.Information;

namespace GeneticAlgorithmsTests.CrossoversTest
{
    public class CrossoversTest
    {
        private const string FILE_PATH = "../../../../picture.xml";

        private ICrossover GetCrossover(int crossoverID)
        {
            ICrossover crossover;
            switch (crossoverID)
            {
                case 1:
                default:
                    crossover = new CyclicCrossover();
                    break;
                case 2:
                    crossover = new OrderedCrossover();
                    break;
            }
            return crossover;
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void Crossover_UnrepeatableData(int testNum)
        {
            ICrossover crossover = GetCrossover(testNum);            
            Population population = new Population();
            population.CreateStartingPopulation(new XMLLoader(FILE_PATH));
            population.SetCurrentCrossingover(crossover);
            population.PerformCrossover(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 });

            List<int> foundedIds = new List<int>();
            foreach (AbstractIndividual individual in population.CurrentGeneration)
            {
                foundedIds.Clear();
                foreach (Segment segment in individual.Segments)
                {
                    Assert.DoesNotContain(segment.ID, foundedIds);
                    foundedIds.Add(segment.ID);
                }
            }
        }
    }
}

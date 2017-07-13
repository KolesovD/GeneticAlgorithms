using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithms
{
    class Program
    {
        static void Main(string[] args)
        {
            int generationSize = 5000;

            Console.WriteLine("Start with generation size {0}", generationSize);
            Control control = new Control(generationSize, fractionOfNewIndividuals:0.9);
            Mutator mutator = new Mutator(segmentFlipProbability: 0.01, mutationProbability: 0.05);

            for (int i = 0; i <=3000; i++)
            {
                control.OptimizeStep(Crosser.CyclicCrossover, mutator.ReverseSegmentMutation);

                if (i%10 == 0)
                {
                    Console.ReadKey();
                }
            }
        }
    }
}

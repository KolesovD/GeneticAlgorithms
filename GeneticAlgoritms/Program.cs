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
            //App.Main();
            int generationSize = 10;

            Console.WriteLine("Start with generation size {0}", generationSize);
            Control control = new Control("../../../Lines.xml", generationSize, 0.9);
            Mutator mutator = new Mutator(segmentFlipProbability: 0.01, mutationProbability: 0.05);


            for (int i = 0; i <=6000; i++)
            {
                //Console.WriteLine($"Поколение №{control.currentGenerationNumber}");
                control.OptimizeStep(Crosser.CyclicCrossover, mutator.ReverseSegmentMutation);
                Console.WriteLine("Лучший в поколении №" + control.currentGenerationNumber + "\n" + control.bestIndividual);
                if (i%10 == 0)
                {
                    Console.ReadKey();
                }
            }

        }
    }
}

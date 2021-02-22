using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GeneticAlgorithms
{
    class Program
    {
        private static MasterControl GA;
        static void Main(string[] args)
        {
            //App.Main();
            int generationSize = 2000;
            int island_count = 4;
            int migration_count = (int)(generationSize * 0.1f);

            Console.WriteLine("Start with generation size {0}", generationSize);
            Mutator mutator = new Mutator(segmentFlipProbability: 0.01, mutationProbability: 0.05);
            int k = 10;
            int g = k;
            GA = new MasterControl(migration_count, island_count, "../../../Lines.xml", generationSize,
                (i) =>
                {
                    return Crosser.CyclicCrossover;
                },
                (i) =>
                {
                    return mutator.ReverseSegmentMutation;
                },
                (i) =>
                {
                    return (c) =>
                    {
                        if (i != 0) { return; }
                        //if (k > 0) { k--; return; }
                        Console.WriteLine("Лучший в поколении №" + c.currentGenerationNumber + " на острове " + i + "\n" + c.bestIndividual);
                        //k = g;
                    };
                },
                CancellationToken.None
            );
            GA.Start();
            Console.ReadKey();
            GA.Pause();
            Console.ReadKey();
            //for (int i = 0; i <=6000; i++)
            //{
            //    //Console.WriteLine($"Поколение №{control.currentGenerationNumber}");
            //    control.OptimizeStep(Crosser.CyclicCrossover, mutator.ReverseSegmentMutation);
            //    Console.WriteLine("Лучший в поколении №" + control.currentGenerationNumber + "\n" + control.bestIndividual);
            //    if (i%200 == 0)
            //    {
            //        Console.ReadKey();
            //    }
            //}

        }
    }
}

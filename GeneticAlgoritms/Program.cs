using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithms
{
    class Program
    {
        private static MasterControll GA;
        static void Main(string[] args)
        {
            //App.Main();
            int generationSize = 5000;
            int island_count = 4;

            Console.WriteLine("Start with generation size {0}", generationSize);
            //Control control = new Control("../../../Lines.xml", generationSize);
            Mutator mutator = new Mutator(segmentFlipProbability: 0.01, mutationProbability: 0.05);

            GA = new MasterControll(island_count, "../../../Lines.xml", generationSize,
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
                        Console.WriteLine("Лучший в поколении №" + c.currentGenerationNumber + " на острове "+i+"\n" + c.bestIndividual);
                    };
                }
            );
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

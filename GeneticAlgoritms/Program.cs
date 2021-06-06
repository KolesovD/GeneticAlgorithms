using GeneticAlgorithms.Information;
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
            int generationSize = 10;
            int island_count = 1;
            int migration_count = (int)(generationSize * 0.3f);
            double migrationProbability = 0d;
            int k = 20;
            int g = k * island_count;
            Mutator mutator = new Mutator(segmentFlipProbability: 0.01, mutationProbability: 0.01);
            Console.WriteLine("Start with generation size {0}", generationSize);
            GA = new MasterControl(
                migration_count, 
                island_count, 
                new GerberLoader("../../../КНБТ.100.610.GBL") 
                //new XMLLoader("../../../Lines.xml")
                , 
                generationSize, migrationProbability,
                (i) => {
                    return Crosser.CyclicCrossover;
                },
                (i) => {
                    return mutator.ReverseSegmentMutation;
                },
                (i) => {
                    return (c) =>
                    {
                        //if (g > 0) { g--; return; }

                        Console.WriteLine(
                            string.Format(
                                "Поколение № {0} остров № {1} количество миграций {2}\nbest: {3}", 
                                c.currentGenerationNumber, 
                                i, 
                                c.MigrationCount,
                                c.bestIndividual.FitnessFunction
                                )
                            );
                        //g = k * island_count;
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

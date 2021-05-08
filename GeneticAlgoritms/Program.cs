﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GeneticAlgorithms.Crossovers;
using GeneticAlgorithms.Mutations;

namespace GeneticAlgorithms
{
    class Program
    {
        private static MasterControl GA;
        
        static void Main(string[] args)
        {
            int generationSize = 2000;
            int island_count = 4;
            int migration_count = (int)(generationSize * 0.3f);
            double migrationProbability = 1d;
            int k = 20;
            int g = k * island_count;

            IMutation mutator = new ReverseSegmentMutation(mutationProbability: 0.01);
            ICrossover crossover = new CyclicCrossover();

            Console.WriteLine("Start with generation size {0}", generationSize);
            GA = new MasterControl(migration_count, island_count, "../../../Lines.xml", generationSize, migrationProbability,
                (i) => {
                    List<(float, ICrossover)> crossoverList = new List<(float, ICrossover)>();
                    switch (i)
                    {
                        case 1:
                            crossoverList.Add((0.75f, new CyclicCrossover()));
                            crossoverList.Add((0.25f, new OrderedCrossover()));
                            break;
                        /*case 4:
                            crossoverList.Add((1f, new OrderedCrossover()));
                            break;*/
                        default:
                            crossoverList.Add((1f, new CyclicCrossover()));
                            break;
                    }
                    
                    return crossoverList;
                },
                (i) => {
                    List<(float, IMutation)> mutationList = new List<(float, IMutation)>();
                    switch (i)
                    {
                        case 2:
                            mutationList.Add((0.1f, new ReverseSegmentMutation(mutationProbability: 0.1f)));
                            break;
                        /*case 5:
                            mutationList.Add((1f, new InvertDirectionMutation(mutationProbability: 0.25f)));
                            break;*/
                        default:
                            mutationList.Add((0.95f, new ReverseSegmentMutation(mutationProbability: 0.01f)));
                            mutationList.Add((0.05f, new InvertDirectionMutation(mutationProbability: 0.1f)));
                            break;
                    }

                    return mutationList;
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

using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Assets.MyRandoms;

namespace GeneticAlgorithms
{
    public class Control
    {
        private int _id;
        private int generationSize;
        private Population population;
        private Roulette roulette;

        private ConcurrentBag<AbstractIndividual> Write;
        private ConcurrentBag<AbstractIndividual> Read;
        //private ChangeBag<AbstractIndividual> Write;
        //private ChangeBag<AbstractIndividual> Read;
        private MasterControl Repository;
        public int MigrationCount { get; private set; }

        public bool allowParentsIntoNewGenerations = true;
        public int currentGenerationNumber { get; private set; } //Нулевое поколение - сгенерированное случайно
        //public double fractionOfNewIndividuals;

        public AbstractIndividual bestIndividual
        {
            get
            {
                return population.GetBestIndividual();
            }
        }

        public Control(
            int i,
            MasterControl master, 
            string xml_path, 
            int generationSize,
            ConcurrentBag<AbstractIndividual> Write,
            ConcurrentBag<AbstractIndividual> Read
            )
        {
            _id = i;
            MigrationCount = 0;
            Repository = master;
            currentGenerationNumber = 0;
            this.generationSize = generationSize;
            //this.fractionOfNewIndividuals = fractionOfNewIndividuals;
            population = new Population();
            population.CreateStartingPopulation(xml_path, generationSize);
            roulette = new Roulette(this.generationSize);
            this.Read = Read;
            this.Write = Write;
        }

        //private void roullete_task(int start, int count, int[] target)
        //{
        //    for (int k = start; k < start+count; k++)
        //    {
        //        target[k] = roulette.PickIndividualIndex();
        //    }
        //}

        private int[] RouletteSelection()
        {
            ////Отбор---
            //Загрузить в рулетку популяцию
            //Достать определённое количество индексов
            ////---
            //Загружаем данные о популяции в рулетку
            roulette.LoadByPopulation(population);

            int[] selectedIndexes = new int[population.GenerationSize];
            //Console.WriteLine("use roulette thread №{0}", num);
            for (int i = 0; i < selectedIndexes.Length; i++) 
            {
                selectedIndexes[i] = roulette.PickIndividualIndex();
            }

            //Console.WriteLine("end use roulette thread №{0}", num);
            //int proc_count = Environment.ProcessorCount;
            //Task[] t = new Task[proc_count];
            //int delta = individualsToSelect / proc_count;
            //int start = 0;
            //for (int i = 0; i < proc_count; i++)
            //{
            //    if (i == proc_count - 1)
            //    {
            //        t[i] = new Task(() => roullete_task(start, individualsToSelect - start, selectedIndexes));
            //    }
            //    else
            //    {
            //        t[i] = new Task((s) => roullete_task((int)s, delta, selectedIndexes), start);
            //        start += delta;
            //    }
            //    t[i].Start();
            //}
            //Task.WaitAll(t);
            //Parallel.For(0, individualsToSelect - 1, (i) => selectedIndexes[i] = roulette.PickIndividualIndex());
            return selectedIndexes;
        }

        public void Optimize(Delegates.Crossover crossover, Delegates.Mutator mutator, int maxPopulationNumber)
        {
            //К этому моменту начальная случайно сгенерированная популяция уже создана, далее выполняется отбор
            for (int i = 0; i < maxPopulationNumber; i++)
            {
                int[] selectedIndexes = RouletteSelection();
                population.PerformCrossingover(crossover, selectedIndexes); //Кроссинговер
                population.PerformMutation(mutator); //Мутация 
                population.SwitchGenerations(); //Поменять поколения в популяции местами
                currentGenerationNumber++;
            }
            Console.ReadLine();
            //Выполнять определённое количество раз
        }

        public void ReadOperate() 
        {
            //Read.SaveOperate((bag) =>
            //{
            //    for (int i = 0; i < bag.Count; i++)
            //    {
            //        population.Add(bag[i]);
            //    }
            //    bag.Clear();
            //});

            while (!Read.IsEmpty)
            {
                Read.TryTake(out AbstractIndividual instance);
                population.Add(instance);
            }

        }
        public void WriteOperate() 
        {
            int count_of_population = population.GenerationSize;

            for (int i = 0; i < Repository.MigrationCount; i++)
            {
                int migrate_index = MyRandom.rnd.Next(count_of_population);
                Write.Add(population.CurrentGeneration[migrate_index]);
                population.RemoveAt(migrate_index);
                count_of_population--;
            }

            //int count_of_population = population.GenerationSize;
            //Write.SaveOperate((bag) =>
            //{
            //    for (int i = 0; i < Repository.MigrationCount; i++)
            //    {
            //        int migrate_index = MyRandom.rnd.Next(count_of_population);
            //        bag.Add(population.CurrentGeneration[migrate_index]);
            //        population.RemoveAt(migrate_index);
            //        count_of_population--;
            //    }
            //});
        }

        public void OptimizeStep(Delegates.Crossover crossover, Delegates.Mutator mutator)
        {
            //Console.WriteLine($"Поколение №{population.currentGenerationNumber}");
            //К этому моменту начальная случайно сгенерированная популяция уже создана, далее выполняется отбор

            Console.WriteLine($"read migrate {_id} IsEmpty = {Read.IsEmpty}");
            if (!Read.IsEmpty) 
            {
                if (population.GenerationSize < generationSize) 
                {
                    //условие окончания кольца 
                    ReadOperate();
                    Repository.Reset();
                    MigrationCount++;
                    Console.WriteLine($"end migrate {_id}");
                }
                else 
                {
                    //условие прихода мигрантов
                    WriteOperate();
                    ReadOperate();
                    Console.WriteLine($"get migrate {_id}");
                }
            }

            if (Repository.MigrationProbability >= MyRandom.GetRandomDouble(1) && Repository.Set()) 
            {
                //условие запуска миграции
                
                WriteOperate();
                Console.WriteLine($"start migrate {_id}");
            }

            int[] selectedIndexes = RouletteSelection();
            population.PerformCrossingover(crossover, selectedIndexes); //Кроссинговер
            population.PerformMutation(mutator); //Мутация 
            population.SwitchGenerations(); //Поменять поколения в популяции местами
            //Console.WriteLine("Лучший в поколении №" + currentGenerationNumber + "\n" + population.GetBestIndividual());
            currentGenerationNumber++;
        }
    }
}

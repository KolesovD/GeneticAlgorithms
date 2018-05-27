using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.MyRandoms;

namespace GeneticAlgorithms
{
    public class Control
    {
        private int generationSize;
        private Population population;
        private Roulette roulette;
        private ChangeBag<AbstractIndividual> Write;
        private ChangeBag<AbstractIndividual> Read;
        private MasterControll Repository;

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

        public Control(MasterControll master, string xml_path, int generationSize, ChangeBag<AbstractIndividual> Read, ChangeBag<AbstractIndividual> Write)
        {
            Repository = master;
            currentGenerationNumber = 0;
            this.generationSize = generationSize;
            //this.fractionOfNewIndividuals = fractionOfNewIndividuals;
            population = new Population();
            population.CreateStartingPopulation(xml_path, generationSize); //Порождается отладочная популяция из Plate (единичные сегменты по диагонали с расстоянием 0)
            roulette = new Roulette(this.generationSize);
            this.Read = Read;
            this.Write = Write;
        }

        private void roullete_task(int start, int count, int[] target)
        {
            for (int k = start; k < start+count; k++)
            {
                target[k] = roulette.PickIndividualIndex();
            }
        }

        public int[] RouletteSelection()
        {
            ////Отбор---
            //Загрузить в рулетку популяцию
            //Достать определённое количество индексов
            ////---
            //Загружаем данные о популяции в рулетку
            roulette.LoadByPopulation(population);

            int individualsToSelect = generationSize;

            int[] selectedIndexes = new int[individualsToSelect];
            int proc_count = Environment.ProcessorCount;
            Task[] t = new Task[proc_count];
            int delta = individualsToSelect / proc_count;
            int start = 0;
            for (int i = 0; i < proc_count; i++)
            {
                if (i == proc_count - 1)
                {
                    t[i] = new Task(() => roullete_task(start, individualsToSelect - start, selectedIndexes));
                }
                else
                {
                    t[i] = new Task((s) => roullete_task((int)s, delta, selectedIndexes), start);
                    start += delta;
                }
                t[i].Start();
            }
            Task.WaitAll(t);
             //Parallel.For(0, individualsToSelect - 1, (i) => selectedIndexes[i] = roulette.PickIndividualIndex());
                //for (int i = 0; i < individualsToSelect; i++)
                //{
                //    selectedIndexes[i] = roulette.PickIndividualIndex();
                //}
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

        public void OptimizeStep(Delegates.Crossover crossover, Delegates.Mutator mutator)
        {
            //Console.WriteLine($"Поколение №{population.currentGenerationNumber}");
            //К этому моменту начальная случайно сгенерированная популяция уже создана, далее выполняется отбор
            if (Read.Migrate.Count != 0) {
                //условие окончания кольца
                Repository.Reset();
            }
            try {
                if (Repository.IsAvaliableChange & Repository.MigrationProbability >= MyRandom.GetRandomDouble(1)) {
                    //условие запуска миграции
                    Repository.Set();
                    int count_of_population = population.GenerationSize;
                    Write.SaveOperate((bag) => {
                        for (int i = 0; i < Repository.MigrationCount; i++)
                        {
                            int migrate_index = MyRandom.rnd.Next(count_of_population);

                            population.RemoveAt(migrate_index);
                            count_of_population--;
                        }
                    });
                }
            }
            catch (FlagException err) {}
            int[] selectedIndexes = RouletteSelection();
            population.PerformCrossingover(crossover, selectedIndexes); //Кроссинговер
            population.PerformMutation(mutator); //Мутация 
            population.SwitchGenerations(); //Поменять поколения в популяции местами
            //Console.WriteLine("Лучший в поколении №" + currentGenerationNumber + "\n" + population.GetBestIndividual());
            currentGenerationNumber++;
        }
    }
}

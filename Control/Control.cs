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
        private Population population;
        private Roulette roulette;

        public bool allowParentsIntoNewGenerations = true;
        public int currentGenerationNumber = 0; //Нулевое поколение - сгенерированное случайно
        public double fractionOfNewIndividuals;

        public Control(int generationSize, double fractionOfNewIndividuals = 0.5)
        {
            this.generationSize = generationSize;
            this.fractionOfNewIndividuals = fractionOfNewIndividuals;
            population = new Population();
            population.CreateStartingPopulation(generationSize); //Порождается отладочная популяция из Plate (единичные сегменты по диагонали с расстоянием 0)
            roulette = new Roulette(this.generationSize);
        }

        public int[] RouletteSelection()
        {
            ////Отбор---
            //Загрузить в рулетку популяцию
            //Достать определённое количество индексов
            ////---
            //Загружаем данные о популяции в рулетку
            roulette.LoadByPopulation(population);

            int individualsToSelect = (int)((generationSize * fractionOfNewIndividuals));

            int[] selectedIndexes = new int[individualsToSelect];
            for (int i = 0; i < individualsToSelect; i++)
            {
                selectedIndexes[i] = roulette.PickIndividualIndex();
            }
            return selectedIndexes;
        }

        public void Optimize(Delegates.Crossover crossover, Delegates.Mutator mutator, int maxPopulationNumber) //Перегрузить!
        {
            //К этому моменту начальная случайно сгенерированная популяция уже создана, далее выполняется отбор
            for (int i = 0; i < maxPopulationNumber; i++)
            {
                int[] selectedIndexes = RouletteSelection();

                population.PerformCrossingover(crossover, selectedIndexes); //Кроссинговер
                population.PerformMutation(mutator); //Мутация 
                population.SwitchGenerations(); //Поменять поколения в популяции местами
                currentGenerationNumber++;
                population.GetBestIndividual();

            }
            Console.ReadLine();
            //Выполнять определённое количество раз
        }

        public void OptimizeStep(Delegates.Crossover crossover, Delegates.Mutator mutator)
        {
            Console.WriteLine($"Поколение №{population.currentGenerationNumber}");
            //К этому моменту начальная случайно сгенерированная популяция уже создана, далее выполняется отбор
            int[] selectedIndexes = RouletteSelection();
            population.PerformCrossingover(crossover, selectedIndexes); //Кроссинговер
            population.PerformMutation(mutator); //Мутация 
            population.SwitchGenerations(); //Поменять поколения в популяции местами
            Console.WriteLine("Лучший в поколении №" + currentGenerationNumber + "\n" + population.GetBestIndividual());
            currentGenerationNumber++;
        }
    }
}

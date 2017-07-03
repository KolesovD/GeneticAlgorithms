﻿using System;
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
        public double fractionOfNewIndividuals = 0.25;
        public double mutationProbability = 0.01;

        public Control(int generationSize, double fractionOfNewIndividuals = 0.25, double mutationProbability = 0.01)
        {
            this.generationSize = generationSize;
            this.fractionOfNewIndividuals = fractionOfNewIndividuals;
            this.mutationProbability = mutationProbability;
            population = new Population(generationSize); //В конструкторе автоматически порождается отладочная популяция из Plate (единичные сегменты по диагонали с расстоянием 0) 
            roulette = new Roulette(this.generationSize);
        }

        public int[] RouletteSelection()
        {
            ////Отбор---
            //Загрузить в рулетку популяцию
            //Достать определённое количество индексов
            ////---
            roulette.LoadByPopulation(population);
            int individualsToSelect = (int)((generationSize * fractionOfNewIndividuals)/2); //Деление на 2 из-за того, что кроссинговером будет произведено в 2 раза больше потомков (спорно)
            int[] selectedIndexes = new int[individualsToSelect];
            for (int i = 0; i < generationSize; i++)
            {
                selectedIndexes[i] = roulette.PickIndividualIndex();
            }
            return selectedIndexes;
        }

        public void Optimize(Delegates.Crossover crossover, Delegates.Mutator mutator)
        {
            //К этому моменту начальная случайно сгенерированная популяция уже создана, далее выполняется отбор
            int[] selectedIndexes = RouletteSelection();
            population.PerformCrossingover(crossover, selectedIndexes); //Кроссинговер
            population.PerformMutation(mutator, mutationProbability); //Мутация 
            //Использовать вероятность

            population.SwitchGenerations(); //Поменять поколения в популяции местами

            currentGenerationNumber++;
            //Выполнять определённое количество раз
        }
    }
}

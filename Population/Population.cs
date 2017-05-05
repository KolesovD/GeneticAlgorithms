﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneticAlgorithms.Delegates;

namespace GeneticAlgorithms
{
    class Population : IPopulation
    {
		private ABCcontrol _core;
		private List<Iindividual> firstGeneration;
		private List<Iindividual> secondGeneration;

		private Crossover Cross;
		private Mutator Mutate;
		private CreateNewIindividual CreateIindividual;
		private CopyFromIindividual CopyIindividual;

        public bool currentGenerationFlag = true; //true = первая популяция является текущей

		public void LoadDelegates(Loader _Load) {
			Cross = _Load.Crossover;
			Mutate = _Load.Mutator;
			CreateIindividual = _Load.CreateNewIndividual;
			CopyIindividual = _Load.CopyFrom;
		}

        public Population(Loader _Load, ABCcontrol core, List<Iindividual> firstGeneration, List<Iindividual> secondGeneration)
        {
			LoadDelegates(_Load);
			_core = core;
            this.firstGeneration = firstGeneration;
            this.secondGeneration = secondGeneration;
        }


        public Population(Loader _Load, ABCcontrol core)
        {
            LoadDelegates(_Load);
            _core = core;
			Iindividual perfectPlate = CreateIindividual();
			for (int i = 0; i<_core.GetPopulationSize; i++)
            {
				Iindividual plate = CopyIindividual(perfectPlate);
				plate.Shuffle();
                firstGeneration.Add(plate);
            }
            secondGeneration = new List<Iindividual>(firstGeneration);
        }

        //Вернуть ссылку на текущее поколение
        public List<Iindividual> CurrentGeneration
        {
            get
            {
                return currentGenerationFlag ? firstGeneration : secondGeneration;
            }

        }

        //Вернуть ссылку на не текущее поколение
        public List<Iindividual> AnotherGeneration
        {
            get
            {
                return currentGenerationFlag ? secondGeneration : firstGeneration;
            }

        }

        //Вернуть ссылку на особь по индексу из текущей популяции
        public Iindividual GetPlateFromCurrentPopulation(int index)
        {
            return CurrentGeneration[index];
        }

        //Вернуть ссылку на особь по индексу из не текущей популяции
        public Iindividual GetIndividualFromAnotherPopulation(int index)
        {
            return AnotherGeneration[index];
        }

        public void Mutation(Delegates.Mutator target)
        {
            
        }

        public void Crossover(Delegates.Crossover crossover)
        {
            for (int i = 1; i < firstGeneration.Count; i++)
            {
                Population p = new Population();
                crossover(p.GetPlateFromCurrentPopulation(i), p.GetPlateFromCurrentPopulation(i - 1));
            }
            //делегат возвращает AbstractIndividual
            //использовать для создания новых особей
            //создание из 2 особей 1 или нескольких особей по команде
        }

    }
}

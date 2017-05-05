using System;
using System.Collections.Generic;
using Assets.MyRandoms;

namespace GeneticAlgorithms
{
	//класс сектора на рулетке
    public class Sector
    {
        public double Start { get; set; }
        public double Value
        {
            get { return End - Start; }
        }
        public double End { get; set; }

        public void ConfigSector(double prevEnd, double _value)
        {
            Start = prevEnd;
            End = Start + _value;
        }
    }

    public class Roulette
    {
        public List<Sector> GenerationAxis;
        private double get_end_of_gen //возвращает конечное значение на оси рулетки
        {
            get { return GenerationAxis[GenerationAxis.Count - 1].End; }
        }

        public Roulette(int gen_lenght) //создаёт рулетку с пустыми секторами
        {
            GenerationAxis = new List<Sector>();
            for (int i = 0; i < gen_lenght; i++)
            {
                GenerationAxis.Add(new Sector());
            }
        }

        public void LoadByPopulation(IPopulation population) //загружает в себя индивидуальности из текущего поколения
        {
            double start = 0;
            //string res = "0";
            for (int i = 0; i < GenerationAxis.Count; i++)
            {
                GenerationAxis[i].ConfigSector(start, population.CurrentGeneration[i].GetFitnessFunction);
                start = GenerationAxis[i].End;
                //res += " - " + start;
            }
            //Console.WriteLine(res);
        }
		public int[] PickIndividualsIndex(int Count) //возвращает индексы особей в кличестве Count в текущем поколении
		{
			int[] result = new int[Count];
			for (int i = 0; i < Count; i++) {
				result[i] = PickIndividualIndex();
			}
			return result;
		}
        public int PickIndividualIndex() //возвращает индекс особи в текущем поколении
        {
            double random = MyRandom.GetRandomDouble(get_end_of_gen);
            //Console.WriteLine("случайное значение {0}", random);
            int step = 0;

            int start = 0;
            int end = GenerationAxis.Count - 1;

            int index;
            double res_start;
            double res_end;

            do
            {
                step += 1;

                index = start + ((end - start) / 2);
                res_start = GenerationAxis[index].Start;
                res_end = GenerationAxis[index].End;
                if (res_start > random)
                {
                    end = index - 1;
                }
                else if (res_end < random)
                {
                    start = index + 1;
                }

                //Console.WriteLine("шаг № {0}, диапазон сушности по индексу № {3}: {1} - {2}\n" +
                //                  "диапазон текущего массива {4} - {5}", step, res_start, res_end, index, start, end);
            } while (!(res_start < random && res_end > random));
            return index;
        }
    }
}
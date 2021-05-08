using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.MyRandoms;

namespace GeneticAlgorithms.Crossovers
{
    public class OrderedCrossover : ICrossover
    {
        public static void PerformCrossover(AbstractIndividual _parent1, AbstractIndividual _parent2, AbstractIndividual child)
        {

            List<Segment> parent1 = new List<Segment>(_parent1.Segments);
            List<Segment> parent2 = new List<Segment>(_parent2.Segments);

            //1 шаг - выбираем сегменты 
            int start = MyRandom.rnd.Next(0, parent1.Count);
            int end = MyRandom.rnd.Next(0, parent1.Count);
            start = Math.Min(start, end);
            end = Math.Max(start, end);
            ////коллекции для хранения выбранных сегментов
            //HashSet<Segment> parent1_segments = new HashSet<Segment>();
            //HashSet<Segment> parent2_segments = new HashSet<Segment>();


            //2 шаг - добавляем сегменты в соответствующую коллекцию и меняем их местами
            Segment helper;
            for (int i = start; i <= end; i++)
            {
                ////кладем сегменты в коллекцию
                //parent1_segments.Add(parent1.Segments[i]);
                //parent2_segments.Add(parent2.Segments[i]);
                //меняем местами
                helper = parent1[i];
                parent1[i] = parent2[i];
                parent2[i] = helper;
            }


            //3 шаг - ищем зависимости
            List<int[]> dependencies = new List<int[]>(end - start);

            for (int i = start; i <= end; i++)
            {
                int id1 = parent1[i].ID;

                for (int j = 0; j < parent1.Count; j++)
                {
                    if ((j < start || j > end) && id1 == parent1[j].ID)
                    {
                        int id2 = parent2[i].ID;
                        //dependencies.Add(new int[2] {parent1[i].ID, parent2[i].ID });
                    }
                    else
                    {
                        continue;
                    }
                }

            }
        }

        void ICrossover.PerformCrossover(AbstractIndividual parent1, AbstractIndividual parent2, AbstractIndividual child)
        {
            PerformCrossover(parent1, parent2, child);
        }
    }
}

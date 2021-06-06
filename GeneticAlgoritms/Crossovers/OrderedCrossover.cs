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

            //1 шаг - выбираем сегменты 
            int fisrtRandom = MyRandom.rnd.Next(0, _parent1.Segments.Count);
            int secondRandom = MyRandom.rnd.Next(0, _parent1.Segments.Count);
            int start = Math.Min(fisrtRandom, secondRandom);
            int end = Math.Max(fisrtRandom, secondRandom);
            ////коллекции для хранения выбранных сегментов
            //HashSet<Segment> parent1_segments = new HashSet<Segment>();
            //HashSet<Segment> parent2_segments = new HashSet<Segment>();


            //2 шаг - добавляем сегменты из первого родителя в потомка и меняем направление относительно второго родителя
            for (int i = 0; i < start; i++)
                child.Segments[i].SetDataFromSegment(_parent1.Segments[i]);

            for (int i = start; i < end; i++)
            {
                child.Segments[i].SetDataFromSegment(_parent1.Segments[i]);
                child.Segments[i].Direction = _parent2.Segments[i].Direction;
            }

            for (int i = end; i < _parent1.Segments.Count; i++)
                child.Segments[i].SetDataFromSegment(_parent1.Segments[i]);
        }

        void ICrossover.PerformCrossover(AbstractIndividual parent1, AbstractIndividual parent2, AbstractIndividual child)
        {
            PerformCrossover(parent1, parent2, child);
        }
    }
}

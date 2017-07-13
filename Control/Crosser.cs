using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.MyRandoms;

namespace GeneticAlgorithms
{
    public class Crosser
    {
        public static void OrderedCrossover(AbstractIndividual parent1, AbstractIndividual parent2, AbstractIndividual child)
        {
            List<Segment> childSegments = new List<Segment>();

            for (int i = 0; i < parent1.Size(); i++)
            {
                childSegments.Add(null);
            }

            int start = MyRandom.rnd.Next(0, parent1.Size());
            int end = MyRandom.rnd.Next(0, parent1.Size());

            start = Math.Min(start, end);
            end = Math.Max(start, end);

            HashSet<Segment> set = new HashSet<Segment>();

            for (int i = start; i <= end; i++)
            {
                childSegments[i] = parent1.Segments[i];
                set.Add(childSegments[i]);
            }

            int ci = end + 1;
            ci %= parent1.Size();

            for (int i = ci; i < 2 * parent2.Size(); i++)
            {
                int ri = i % parent2.Size();

                if (!set.Contains(parent2.Segments[ri]))
                {
                    childSegments[ci] = parent2.Segments[ri];
                    set.Add(parent2.Segments[ri]);
                    ci++;

                    ci %= parent1.Size();
                }
            }

            child.Rewrite(childSegments);
        }

        public static void CyclicCrossover(AbstractIndividual parent1, AbstractIndividual parent2, AbstractIndividual child)
        {
            HashSet<int> cycleSet = new HashSet<int>();

            int startIndex = parent1.Segments.FindIndex(s => s.ID == 0); //Индекс сегмента с ID = 0
            cycleSet.Add(startIndex);
            int currentIndex = parent1.Segments.FindIndex(s => s.ID == parent2.Segments[startIndex].ID); //Индекс в parent1 сегмента с ID, стоящим "напротив" начального элемента
            while (currentIndex != startIndex)
            {
                cycleSet.Add(currentIndex);
                currentIndex = parent1.Segments.FindIndex(s => s.ID == parent2.Segments[currentIndex].ID);
            }

            for (int i = 0; i < child.Size(); i++)
            {
                if (cycleSet.Contains(i))
                    child.Segments[i].SetDataFromSegment(parent1.Segments[i]);
                else
                    child.Segments[i].SetDataFromSegment(parent2.Segments[i]);
            }
        }
    }
}

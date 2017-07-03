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
        //static Random rng = new Random();

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

            int size = parent1.Size();
            List<Segment> childSegments = new List<Segment>();

            for (int i = 0; i < size; i++)
            {
                childSegments.Add(null);
            }

            HashSet<int> seenIndexes = new HashSet<int>();

            int currentIndex = 0;
            while (!seenIndexes.Contains(currentIndex))
            {
                seenIndexes.Add(currentIndex);
                int IDToFind = parent2.Segments[currentIndex].ID;
                currentIndex = parent1.Segments.FindIndex(a =>
                {
                    return a.ID == IDToFind;
                });
            }

            foreach (int index in seenIndexes)
            {
                childSegments[index] = new Segment(parent1.Segments[index]);  //Не создавать новые а перезаписывать
            }

            for (int i = 0; i < size; i++)
            {
                if (childSegments[i] == null)
                {
                    childSegments[i] = new Segment(parent2.Segments[i]);
                }
            }

            child.Rewrite(childSegments);
        }

    }
}

using System.Collections.Generic;

namespace GeneticAlgorithms.Crossovers
{
    public class CyclicCrossover : ICrossover
    {
        public static void PerformCrossover(AbstractIndividual parent1, AbstractIndividual parent2, AbstractIndividual child)
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

        void ICrossover.PerformCrossover(AbstractIndividual parent1, AbstractIndividual parent2, AbstractIndividual child)
        {
            PerformCrossover(parent1, parent2, child);
        }
    }
}

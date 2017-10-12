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

        public static void OrderedCrossover(AbstractIndividual _parent1, AbstractIndividual _parent2, AbstractIndividual child)
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










            //создаем лист массивов для хранения зависимостей
            //List<int[]> sas = new List<int[]>(end - start);

            //for (int i = 0; i < sas.Count; i++)
            //{
            //    //добавляем зависимости в лист.
            //    sas[i] = new int[2] { parent1.Segments[i].ID, parent2.Segments[i].ID };
            //}
            ////проходимся по всему листу
            //for (int i = 0; i < sas.Count; i++)
            //{
            //    //индекс из первой особи
            //    int index = sas.ElementAt(i)[0];
            //    //пробегаем по листу
            //    for (int j = 0; j < sas.Count; j++)
            //    {
            //        //Сравниваем индекс из первой особи со всеми индексами из второй особи.
            //        if (index == sas.ElementAt(i)[1])
            //        {
            //            sas.ElementAt(i)[0] = sas.ElementAt(j)[0];
            //            sas.RemoveAt(j);
            //            i--;
            //            break;
            //        }
            //    }
            //}
    

            //List<Segment> childSegments = new List<Segment>();

            //for (int i = 0; i < parent1.Size(); i++)
            //{
            //    childSegments.Add(null);
            //}

            //int start = MyRandom.rnd.Next(0, parent1.Size());
            //int end = MyRandom.rnd.Next(0, parent1.Size());

            //start = Math.Min(start, end);
            //end = Math.Max(start, end);

            //HashSet<Segment> set = new HashSet<Segment>();

            //for (int i = start; i <= end; i++)
            //{
            //    childSegments[i] = parent1.Segments[i];
            //    set.Add(childSegments[i]);
            //}

            //int ci = end + 1;
            //ci %= parent1.Size();

            //for (int i = ci; i < 2 * parent2.Size(); i++)
            //{
            //    int ri = i % parent2.Size();

            //    if (!set.Contains(parent2.Segments[ri]))
            //    {
            //        childSegments[ci] = parent2.Segments[ri];
            //        set.Add(parent2.Segments[ri]);
            //        ci++;

            //        ci %= parent1.Size();
            //    }
            //}

            //child.Rewrite(childSegments);
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

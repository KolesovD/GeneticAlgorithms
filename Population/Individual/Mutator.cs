using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.MyRandoms;

namespace GeneticAlgorithms
{
    public class Mutator
    {
        static void InsertRange(List<Segment> original, List<Segment> ToAdd, int position)
        {
            for (int i = position; i < position + ToAdd.Count; i++)
            {
                original[i] = ToAdd[i - position];
                original[i].ReverseDirection();
            }

        }

        static void ReverseRange(List<Segment> segments, int start, int length)
        {
            List<Segment> Range;
            if (start + length >= segments.Count)
            {
                Range = segments.GetRange(start - length, length);
                start -= length;

            }
            else if (start - length < 0)
            {
                Range = segments.GetRange(start, length);

            }
            else if (MyRandom.rnd.NextDouble() < 0.5)
            {
                Range = segments.GetRange(start - length, length);
                start -= length;

            }
            else
            {
                Range = segments.GetRange(start, length);
            }

            Range.Reverse();
            InsertRange(segments, Range, start);
        }

        public static void FlipRandomSegments(List<Segment> segmentListToFlip, double mutationProbability)
        {
            foreach (Segment segment in segmentListToFlip)
            {
                if (MyRandom.rnd.NextDouble() < mutationProbability)
                {
                    segment.ReverseDirection();
                }
            }
        }

        public static void ReverseSegmentMutation(List<Segment> segmentListToMutate)
        {
            double mutationProbability = 0.01; //Передавать как параметр или убрать

            FlipRandomSegments(segmentListToMutate, mutationProbability);

            double max_part_len = 0.2; //Константа - нехорошо
            int max_part_elems = (int)(segmentListToMutate.Count * max_part_len);
            int part_elems;

            for (int i = 0; i < segmentListToMutate.Count; i++)
            {
                if (MyRandom.rnd.NextDouble() <= mutationProbability)
                {
                    part_elems = (int)(max_part_elems * MyRandom.rnd.NextDouble());

                    if (part_elems < 2) part_elems = 2;

                    ReverseRange(segmentListToMutate, i, part_elems);
                }
            }

        }
    }
}

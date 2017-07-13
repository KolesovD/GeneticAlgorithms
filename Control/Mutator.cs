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
        public double mutationProbability { get; set; }
        public double segmentFlipProbability { get; set; }

        public Mutator(double mutationProbability, double segmentFlipProbability)
        {
            this.mutationProbability = mutationProbability;
            this.segmentFlipProbability = segmentFlipProbability;
        }

        private void InsertRange(List<Segment> original, List<Segment> ToAdd, int position)
        {
            for (int i = position; i < position + ToAdd.Count; i++)
            {
                original[i] = ToAdd[i - position];
                original[i].ReverseDirection();
            }

        }

        private void ReverseRange(List<Segment> segments, int start, int length)
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

        private void FlipRandomSegments(List<Segment> segmentListToFlip)
        {
            foreach (Segment segment in segmentListToFlip)
            {
                if (MyRandom.rnd.NextDouble() < segmentFlipProbability)
                {
                    segment.ReverseDirection();
                }
            }
        }

        /*
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
        */

        /*
        public static void ReverseSegmentMutation(List<Segment> segmentListToMutate, double mutationProbability = 0.01)
        {
            //Для передачи дополнительных параметров необходимо обернуть метод мутации в класс
            FlipRandomSegments(segmentListToMutate, mutationProbability); //Не использовать mutationProbability для флипа сегментов

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

        }*/

        public void ReverseSegmentMutation(List<Segment> segmentListToMutate)
        {
            //Для передачи дополнительных параметров необходимо обернуть метод мутации в класс
            FlipRandomSegments(segmentListToMutate);

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

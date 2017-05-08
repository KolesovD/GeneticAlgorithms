using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaserLineOpt
{
    public class Mutator
    {
        static Random rng = new Random();

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
            else if (rng.NextDouble() < 0.5)
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

        public static void FlipRandomSegments(Plate plate, double mutationProbability)
        {
            foreach (Segment segment in plate.Segments)
            {
                if (rng.NextDouble() < mutationProbability)
                {
                    segment.ReverseDirection();
                }
            }
        }

        public static void ReverseSegmentMutation(Plate plate, double mutationProbability)
        {

            FlipRandomSegments(plate, mutationProbability);

            double max_part_len = 0.2;
            int max_part_elems = (int)(plate.Segments.Count * max_part_len);
            int part_elems;

            for (int i = 0; i < plate.Segments.Count; i++)
            {
                if (rng.NextDouble() < mutationProbability)
                {
                    part_elems = (int)(max_part_elems * rng.NextDouble());

                    if (part_elems < 2) part_elems = 2;

                    ReverseRange(plate.Segments, i, part_elems);
                }
            }

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.MyRandoms;

namespace GeneticAlgorithms.Mutations
{
    public class ReverseSegmentMutation : IMutation
    {
        public double mutationProbability { get; set; }
        public double maxCutoutPartLength;

        public ReverseSegmentMutation(double mutationProbability, double maxCutoutPartLength = 0.2)
        {
            this.mutationProbability = mutationProbability;
            this.maxCutoutPartLength = maxCutoutPartLength;
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

        public void PerformMutation(List<Segment> segmentListToMutate)
        {
            int max_part_elems = (int)(segmentListToMutate.Count * maxCutoutPartLength);
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

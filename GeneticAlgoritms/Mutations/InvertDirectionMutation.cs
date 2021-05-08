using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.MyRandoms;

namespace GeneticAlgorithms.Mutations
{
    public class InvertDirectionMutation : IMutation
    {
        public double mutationProbability { get; set; }

        public InvertDirectionMutation(double mutationProbability)
        {
            this.mutationProbability = mutationProbability;
        }

        private void FlipRandomSegments(List<Segment> segmentListToFlip)
        {
            foreach (Segment segment in segmentListToFlip)
            {
                if (MyRandom.rnd.NextDouble() < mutationProbability)
                {
                    segment.ReverseDirection();
                    return;
                }
            }
        }

        public void PerformMutation(List<Segment> segmentListToMutate)
        {
            FlipRandomSegments(segmentListToMutate);
        }
    }
}

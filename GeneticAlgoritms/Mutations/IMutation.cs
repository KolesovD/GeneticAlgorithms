using System.Collections.Generic;

namespace GeneticAlgorithms.Mutations
{
    public interface IMutation
    {
        double MutationProbability { get; set; }

        void PerformMutation(List<Segment> segmentListToMutate);
    }
}

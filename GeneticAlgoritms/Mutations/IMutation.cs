using System.Collections.Generic;

namespace GeneticAlgorithms.Mutations
{
    public interface IMutation
    {
        void PerformMutation(List<Segment> segmentListToMutate);
    }
}

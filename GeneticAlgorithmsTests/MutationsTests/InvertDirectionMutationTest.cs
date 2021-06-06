using System.Collections.Generic;
using Xunit;
using GeneticAlgorithms;
using GeneticAlgorithms.Mutations;

namespace GeneticAlgorithmsTests.MutationsTests
{
    public class InvertDirectionMutationTest
    {
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void InvertDirectionMutation_UnrepeatableData(int testNum)
        {
            List<Segment> segments = SegmentFactory.GetSegments(testNum);            
            IMutation mutation = new InvertDirectionMutation(1f);

            mutation.PerformMutation(segments);

            List<int> foundedIds = new List<int>();
            foreach (Segment segment in segments)
            {
                Assert.DoesNotContain(segment.ID, foundedIds);
                foundedIds.Add(segment.ID);
            }
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void InvertDirectionMutation_MutationPerformed(int testNum)
        {
            List<Segment> segments = SegmentFactory.GetSegments(testNum);
            IMutation mutation = new InvertDirectionMutation(1f);
            bool firstSegmentDirection = segments[0].Direction;

            mutation.PerformMutation(segments);

            Assert.NotEqual(firstSegmentDirection, segments[0].Direction);
        }
    }
}

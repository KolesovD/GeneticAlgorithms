using System.Linq;
using System.Collections.Generic;
using Xunit;
using GeneticAlgorithms;
using GeneticAlgorithms.Mutations;

namespace GeneticAlgorithmsTests.MutationsTests
{
    public class ReverseSegmentMutationTest
    {
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void ReverseSegmentMutation_UnrepeatableData(int testNum)
        {
            List<Segment> segments = SegmentFactory.GetSegments(testNum);            
            IMutation mutation = new ReverseSegmentMutation(1f);

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
        public void InvertDirectionMutation_ReverseNotChangeOtherData(int testNum)
        {
            List<Segment> segments = SegmentFactory.GetSegments(testNum);
            List<Segment> segmentsCopy = SegmentFactory.GetSegments(testNum);
            IMutation mutation = new ReverseSegmentMutation(1f);

            mutation.PerformMutation(segments);

            foreach (Segment segment in segments)
            {
                Segment segmentOld = segmentsCopy.First(seg => seg.ID == segment.ID);
                Assert.Equal(segment.Point1.X, segmentOld.Point1.X);
                Assert.Equal(segment.Point1.Y, segmentOld.Point1.Y);
                Assert.Equal(segment.Point2.X, segmentOld.Point2.X);
                Assert.Equal(segment.Point2.Y, segmentOld.Point2.Y);
            }
        }
    }
}

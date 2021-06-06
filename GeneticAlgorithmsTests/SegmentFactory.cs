using System.Collections.Generic;
using GeneticAlgorithms;

namespace GeneticAlgorithmsTests
{
    public class SegmentFactory
    {
        public static List<Segment> GetSegments(int id)
        {
            List<Segment> segments = new List<Segment>(5);

            switch (id)
            {
                case 1:
                default:
                    segments.Add(new Segment(1, 1f, 1f, 2f, 2f, true));
                    segments.Add(new Segment(2, 2f, 2f, 3f, 3f, false));
                    segments.Add(new Segment(3, 3f, 3f, 4f, 4f, true));
                    segments.Add(new Segment(4, 4f, 4f, 5f, 5f, false));
                    segments.Add(new Segment(5, 5f, 5f, 6f, 6f, true));
                    break;
                case 2:
                    segments.Add(new Segment(3, 4f, 1f, 2f, 3f, false));
                    segments.Add(new Segment(5, 1f, 8f, 2f, 6f, false));
                    segments.Add(new Segment(1, 3f, 7f, 1f, 2f, true));
                    segments.Add(new Segment(2, 9f, 3f, 2f, 1f, true));
                    segments.Add(new Segment(4, 2f, 2f, 1f, 7f, false));
                    break;
                case 3:
                    segments.Add(new Segment(2, 9f, 5f, 2f, 12f, false));
                    segments.Add(new Segment(5, 12f, 9f, 7f, 4f, true));
                    segments.Add(new Segment(1, 22f, 7f, 8f, 1f, true));
                    segments.Add(new Segment(4, 5f, 6f, 3f, 3f, true));
                    segments.Add(new Segment(3, 1f, 2f, 7f, 8f, false));
                    break;
            }

            return segments;
        }
    }
}

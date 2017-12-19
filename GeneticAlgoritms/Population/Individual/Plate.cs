using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.MyRandoms;

namespace GeneticAlgorithms
{
    public class Plate : AbstractIndividual
    {
        public Plate() : base() { }

        public Plate(List<Segment> segments) : base(segments) { }

        override public double FitnessFunction
        {
            get
            {
                return 1.0 / (1 + CalcSumIdlingLine());
            }
        }

        public Plate(Plate copy)
        {
            _Segments = new List<Segment>();
            foreach (Segment segment in copy.Segments)
            {
                AddSegment(new Segment(segment));
            }
        }

        public void ShuffleSegments()
        {
            FisherYatesShuffle();
            SetRandomDirectionsToSegments();
        }

        private void FisherYatesShuffle()
        {
            int n = Segments.Count;

            while (n > 1)
            {
                n--;
                int k = MyRandom.rnd.Next(n + 1);
                Segment value = new Segment(Segments[k]);
                Segments[k] = new Segment(Segments[n]);
                Segments[n] = value;
            }
        }

        public void SetRandomDirectionsToSegments()
        {

            foreach (Segment segment in Segments)
            {
                int n = MyRandom.rnd.Next(2);

                segment.Direction = (n == 1); 
            }
        }

        public double CalcSumIdlingLine()
        {
            double length = 0;
            int size = Segments.Count;

            for (int i = 0; i < size - 1; i++)
            {
                length += (_Segments[i].End - _Segments[i + 1].Start).Length();
            }
            return length;
        }
    }
}

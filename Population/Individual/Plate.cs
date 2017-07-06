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
                this.AddSegment(new Segment(segment));
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

        /*private double CalcIdling(Segment segment1, Segment segment2)
        {

        
            if (segment1.Direction && segment2.Direction)        //True и True
            {
                return CalcIdlingLine(segment1.X2, segment1.Y2, segment2.X1, segment2.Y1);
            }
            else if (!segment1.Direction && segment2.Direction)  //False и True
            {
                return CalcIdlingLine(segment1.X1, segment1.Y1, segment2.X1, segment2.Y1);
            }
            else if (segment1.Direction && !segment2.Direction) //True и False
            {
                return CalcIdlingLine(segment1.X2, segment1.Y2, segment2.X2, segment2.Y2);
            }
            else                                                //False и False
            {
                return CalcIdlingLine(segment1.X1, segment1.Y1, segment2.X2, segment2.Y2);
            }
        }*/

        /*
        private double CalcIdlingLine(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow((x1 - x2), 2) + Math.Pow((y1 - y2), 2));
        }*/
    }
}

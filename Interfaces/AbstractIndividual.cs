using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithms
{
    abstract public class AbstractIndividual
    {
        protected List<Segment> _Segments;

        public List<Segment> Segments
        {
            get { return _Segments; }
        }

        public AbstractIndividual()
        {
            _Segments = new List<Segment>();
        }

        public AbstractIndividual(List<Segment> segments)
        {
            _Segments = segments;
        }

        public abstract double GetFitnessFunction { get; }

        public void AddSegment(Segment segment)
        {
            Segments.Add(segment);
        }

        public int Size()
        {
            return Segments.Count();
        }

        public void Rewrite(List<Segment> newSegmentList)
        {
            for (int i = 0; i < newSegmentList.Count; i++)
            {
                _Segments[i].SetDataFromSegment(newSegmentList[i]);
            }
        }

        public void Mutate(Delegates.MutationFunction Mutator)
        {
            Mutator(_Segments);
        }
    }
}

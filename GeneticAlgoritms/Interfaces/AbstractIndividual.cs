using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneticAlgorithms.Mutations;

namespace GeneticAlgorithms
{
    abstract public class AbstractIndividual //: IComparer<AbstractIndividual>
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
        public abstract AbstractIndividual GetCopy();
        public abstract double FitnessFunction { get; }

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

        public void Mutate(IMutation mutator)
        {
            mutator.PerformMutation(_Segments);
        }

        public override string ToString()
        {
            StringBuilder bd = new StringBuilder();
            bd.AppendFormat("Fitness function: {0}\n", FitnessFunction.ToString());

            for (int i = 0; i < _Segments.Count; i++)
            {
                bd.Append(_Segments[i].ToString());
                
            }
            bd.Append("\n");

            return bd.ToString();
        }

        public string GetParsedSegmets()
        {
            return ToString();
        }

        public List<OuterSegment> GetSegmentsToSerialize()
        {
            List<OuterSegment> outerSegments = new List<OuterSegment>(_Segments.Count);
            foreach (Segment segment in _Segments)
                outerSegments.Add(new OuterSegment(segment));
            return outerSegments;
        }
    }
}

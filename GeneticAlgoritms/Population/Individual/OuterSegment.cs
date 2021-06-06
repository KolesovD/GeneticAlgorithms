using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace GeneticAlgorithms
{
    public class OuterSegment
    {
        public int ID;
        public bool Direction;

        public OuterSegment(Segment copy)
        {
            SetDataFromSegment(copy);
        }

        public OuterSegment(int ID, bool direction)
        {
            this.ID = ID;
            Direction = direction;
        }

        public void SetDataFromSegment(Segment newSegment)
        {
            ID = newSegment.ID;
            Direction = newSegment.Direction;
        }
    } 
}

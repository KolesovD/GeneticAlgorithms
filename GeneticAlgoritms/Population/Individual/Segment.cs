using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace GeneticAlgorithms
{
    public class Segment
    {
        public int ID { get; set; }
        public Vector2 Point1;
        public Vector2 Point2;

        public Vector2 Start
        {
            get
            {
                if (Direction)
                {
                    return Point1;
                }
                else {
                    return Point2;
                }
            }
        }
        public Vector2 End
        {
            get
            {
                if (Direction)
                {
                    return Point2;
                }
                else {
                    return Point1;
                }
            }
        }

        public float Length
        {
            get
            {
                return (Point1 - Point2).Length();
            }
        }

        // True означает направление от 1 точки ко 2
        public bool Direction { get; set; }

        public Segment(Segment copy)
        {
            Point1 = new Vector2(copy.Point1.X, copy.Point1.Y);
            Point2 = new Vector2(copy.Point2.X, copy.Point2.Y);
            Direction = copy.Direction;
            ID = copy.ID;
        }

        public Segment(int ID, int X1, int Y1, int X2, int Y2, bool _direction)
        {
            this.ID = ID;
            Point1 = new Vector2(X1, Y1);
            Point2 = new Vector2(X2, Y2);
            Direction = _direction;
        }

        public void SetDataFromSegment(Segment newSegment)
        {
            this.ID = newSegment.ID;
            Point1 = newSegment.Point1;
            Point2 = newSegment.Point2;
            Direction = newSegment.Direction;
        }

        public override string ToString()
        {
            if (Direction)
            {
                return "[" + Point1.X + ", " + Point1.Y + ";" + Point2.X + ", " + Point2.Y + "]";
            }
            else
            {
                return "[" + Point2.X + ", " + Point2.Y + ";" + Point1.X + ", " + Point1.Y + "]";
            }
        }

        public void ReverseDirection()
        {
            Direction = !Direction;
        }
    } 
}

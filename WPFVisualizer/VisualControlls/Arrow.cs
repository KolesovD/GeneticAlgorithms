using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows;
using System.Numerics;

namespace WPFVisualizer
{
    class Arrow : Shape
    {
        public Point start { get; set; }
        public Point end { get; set; }
        public bool direction = true;
        GeometryGroup lineGroup;

        public Arrow (Point start, Point end, float thickness = 1)
        {
            Create(start, end, thickness);
        }

        public Arrow(Vector2 start, Vector2 end, float thickness = 1) {
            Create(new Point(start.X, start.Y), new Point(end.X, end.Y), thickness);
        }

        private void Create(Point start, Point end, float thickness = 1) {
            this.start = start;
            this.end = end;
            lineGroup = new GeometryGroup();
            double x_l = Math.Abs(end.X - start.X);
            double y_l = Math.Abs(end.Y - start.Y);
            if (Math.Sqrt(Math.Pow(x_l, 2) + Math.Pow(y_l, 2)) <= 0.5d)
            {
                return;
            }

            Stroke = Brushes.Black;
            StrokeThickness = thickness;
            Fill = Brushes.Black;

            double theta = Math.Atan2((end.Y - start.Y), (end.X - start.X)) * 180 / Math.PI;

            PathGeometry pathGeometry = new PathGeometry();
            PathFigure pathFigure = new PathFigure();
            Point p = new Point(end.X, end.Y);
            pathFigure.StartPoint = p;

            Point lpoint = new Point(p.X + 1* thickness, p.Y + 2f* thickness);
            Point rpoint = new Point(p.X - 1* thickness, p.Y + 2f* thickness);
            LineSegment seg1 = new LineSegment();
            seg1.Point = lpoint;
            pathFigure.Segments.Add(seg1);

            LineSegment seg2 = new LineSegment();
            seg2.Point = rpoint;
            pathFigure.Segments.Add(seg2);

            LineSegment seg3 = new LineSegment();
            seg3.Point = p;
            pathFigure.Segments.Add(seg3);

            pathGeometry.Figures.Add(pathFigure);
            RotateTransform transform = new RotateTransform();
            transform.Angle = theta + 90;
            transform.CenterX = p.X;
            transform.CenterY = p.Y;
            pathGeometry.Transform = transform;
            lineGroup.Children.Add(pathGeometry);

            LineGeometry connectorGeometry = new LineGeometry();
            connectorGeometry.StartPoint = start;
            connectorGeometry.EndPoint = end;
            lineGroup.Children.Add(connectorGeometry);
        }

        public void SetColor(SolidColorBrush color) {
            Stroke = color;
            Fill = color;
        }


        protected override Geometry DefiningGeometry
        {
            get
            {
                return lineGroup;
            }
        }
    }
}
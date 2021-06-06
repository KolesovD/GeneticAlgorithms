using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Controls;
using GeneticAlgorithms;
using WPFVisualizer.Extensions;
using System.Windows.Media.Media3D;
using System.Numerics;
using Quaternion = System.Windows.Media.Media3D.Quaternion;

namespace WPFVisualizer.VisualControlls
{
    public class PlateShape : Shape
    {
        private Geometry shapeInternal;

        public void SetSegments(IEnumerable<Segment> segments, float thickness = 1) 
        {
            Matrix3D d45Plus = Matrix3D.Identity;
            d45Plus.Rotate(new Quaternion(new Vector3D(0f, 0f, 1f), 30f));

            Matrix3D d45Minus = Matrix3D.Identity;
            d45Minus.Rotate(new Quaternion(new Vector3D(0f, 0f, 1f), -30f));


            StreamGeometry streamGeometry = new StreamGeometry();
            shapeInternal = streamGeometry;

            streamGeometry.FillRule = FillRule.EvenOdd;

            StrokeThickness = thickness;

            using (StreamGeometryContext ctx = streamGeometry.Open())
            {
                foreach (var item in segments)
                {
                    ctx.BeginFigure(item.Start.ToPoint(), false /* is filled */, false /* is closed */);
                    ctx.LineTo(item.End.ToPoint(), true /* is stroked */, false /* is smooth join */);

                    ctx.BeginFigure(item.End.ToPoint(), false /* is filled */, true /* is closed */);


                    Vector3D _vec = (item.Start - item.End).ToVector3D();
                    _vec.Normalize();


                    Vector3D left = d45Plus.Transform(_vec);
                    left *= thickness;


                    Vector3D right = d45Minus.Transform(_vec);
                    right *= thickness;

                    ctx.LineTo((item.End + left.ToVector2()).ToPoint(), true /* is stroked */, false /* is smooth join */);
                    ctx.LineTo((item.End + right.ToVector2()).ToPoint(), true /* is stroked */, false /* is smooth join */);
                }
            }

            streamGeometry.Freeze();
        }

        public static Path SetPath(IEnumerable<Segment> segments, float thickness = 1)
        {
            // Create a path to draw a geometry with.
            Path myPath = new Path();
            myPath.Stroke = Brushes.Black;
            myPath.StrokeThickness = thickness;

            // Create a StreamGeometry to use to specify myPath.
            StreamGeometry geometry = new StreamGeometry();
            geometry.FillRule = FillRule.EvenOdd;

            using (StreamGeometryContext ctx = geometry.Open())
            {
                ctx.BeginFigure(segments.First().Start.ToPoint(), false /* is filled */, false /* is closed */);
                ctx.LineTo(segments.First().End.ToPoint(), true /* is stroked */, false /* is smooth join */);

                foreach (var item in segments.Skip(1))
                {
                    ctx.LineTo(item.Start.ToPoint(), false /* is filled */, false /* is closed */);
                    ctx.LineTo(item.End.ToPoint(), true /* is stroked */, false /* is smooth join */);
                }
            }

            // Freeze the geometry (make it unmodifiable)
            // for additional performance benefits.
            geometry.Freeze();

            // Specify the shape (triangle) of the Path using the StreamGeometry.
            myPath.Data = geometry;

            return myPath;
        }

        public void SetColor(SolidColorBrush color)
        {
            Stroke = color;
            Fill = color;
        }

        protected override Geometry DefiningGeometry
        {
            get
            {
                return shapeInternal;
            }
        }
    }
}

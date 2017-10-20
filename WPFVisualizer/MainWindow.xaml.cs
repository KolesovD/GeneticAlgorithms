using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFVisualizer
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Line ordinat = new Line();
            ordinat.Stretch = Stretch.Uniform;
            ordinat.Stroke = System.Windows.Media.Brushes.Black;
            ordinat.X1 = 100;
            ordinat.Y1 = 0;
            ordinat.X2 = 0;  // 150 too far
            ordinat.Y2 = 0;
            ordinat.SnapsToDevicePixels = true;
            ordinat.SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Aliased);
            ordinat.StrokeThickness = 2;

            Line absciss = new Line();
            absciss.Stroke = System.Windows.Media.Brushes.Black;
            absciss.X1 = 0;
            absciss.Y1 = 0;
            absciss.X2 = 0;  // 150 too far
            absciss.Y2 = 100;
            absciss.SnapsToDevicePixels = true;
            absciss.SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Aliased);
            absciss.StrokeThickness = 2;

            CanvasDraw.Children.Add(new ArrowLine(new Point(0, 0), new Point(100, 0)).getShape());
            CanvasDraw.Children.Add(new ArrowLine(new Point(0, 0), new Point(0, 100)).getShape());
        }

    }

    public class ArrowLine : Shape
    {
        private System.Windows.Shapes.Path path = new System.Windows.Shapes.Path();

        private Point start, end;


        public ArrowLine(Point start, Point end)
        {
            this.start = start;
            this.end = end;

            GeometryGroup lineGroup = new GeometryGroup();
            double theta = Math.Atan2((end.Y - start.Y), (end.X - start.X)) * 180 / Math.PI;

            PathGeometry pathGeometry = new PathGeometry();
            PathFigure pathFigure = new PathFigure();
            Point p = new Point(end.X, end.Y);
            pathFigure.StartPoint = p;

            Point lpoint = new Point(p.X + 6, p.Y + 15);
            Point rpoint = new Point(p.X - 6, p.Y + 15);
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
            path.Data = lineGroup;
            path.StrokeThickness = 2;
            path.Stroke = path.Fill = Brushes.Black;
        }

        protected override Geometry DefiningGeometry
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Shape getShape()
        {
            return path;
        }
    }
}

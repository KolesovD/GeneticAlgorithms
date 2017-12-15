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
using GeneticAlgorithms;
using System.Threading;
using System.Numerics;
using System.Windows.Threading;

namespace WPFVisualizer
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Queue<AbstractIndividual> _Queue = new Queue<AbstractIndividual>();
        Timer t;
        private DispatcherTimer dispatcherTimer;

        void someFunc()
        {
            int generationSize = 5000;

            GeneticAlgorithms.Control control = new GeneticAlgorithms.Control("../../../picture.xml", generationSize, fractionOfNewIndividuals: 0.9);
            Mutator mutator = new Mutator(segmentFlipProbability: 0.01, mutationProbability: 0.05);

            while (true)
            {
                control.OptimizeStep(Crosser.CyclicCrossover, mutator.ReverseSegmentMutation);
                _Queue.Enqueue(control.bestIndividual);
            }

        }

        Point fromVector(Vector2 vec, float scale) {
            return new Point(vec.X * scale, vec.Y * scale);
        }

        private void visualize_timer_new(object sender, EventArgs e) {
            textBox.AppendText("tik!!!");
            CanvasDraw.Children.Clear();
            AbstractIndividual deq = _Queue.Peek();
            foreach (Segment seg in deq.Segments)
            {
                Arrow visual = null;
                if (seg.Direction)
                {
                    visual = new Arrow(fromVector(seg.Point1, 100), fromVector(seg.Point2, 100));
                }
                else {
                    visual = new Arrow(fromVector(seg.Point2, 100), fromVector(seg.Point1, 100));
                }
                //дописать создание радуги
                int i = 1023 / deq.Segments.Count * seg.ID;
                //byte d = Convert.ToByte(255/deq.Segments.Count*seg.ID);
                //byte r = Convert.ToByte(255 - d);
                SolidColorBrush br = new SolidColorBrush(GetRainbow(i)); //Color.FromRgb(255, 33, 225);
                visual.SetColor(br);
                CanvasDraw.Children.Add(visual);
            }
        }

        private Color GetRainbow(int index) {
            int _index = index % 1023;
            byte r = 0;
            byte g = 0;
            byte b = 0;

            if (_index >= 0 && _index < 170)
            {
                g = Convert.ToByte(1.5 * (_index-0));
            }
            else if (_index >= 170 && _index < 511)
            {
                g = 255;
            }
            else if (_index >= 511 && _index < 682)
            {
                g = Convert.ToByte(1.5 * (682 - _index));
            }
            else if (_index >= 682 && _index < 1023)
            {
                g = 0;
            }

            if (_index >= 0 && _index < 341)
            {
                b = 0;
            }
            else if (_index >= 341 && _index < 511)
            {
                b = Convert.ToByte(1.5 * (_index - 341));
            }
            else if (_index >= 511 && _index < 852)
            {
                b = 255;
            }
            else if (_index >= 852 && _index < 1023)
            {
                b = Convert.ToByte(1.5 * (1023 - _index));
            }

            if (_index >= 0 && _index < 170)
            {
                r = 255;
            }
            else if (_index >= 170 && _index < 341)
            {
                r = Convert.ToByte(1.5 * (341 - _index));
            }
            else if (_index >= 341 && _index < 682)
            {
                r = 0;
            }
            else if (_index >= 682 && _index < 852)
            {
                r = Convert.ToByte(1.5 * (_index - 682));
            }
            else if (_index >= 852 && _index < 1023)
            {
                r = 255;
            }
            return Color.FromRgb(r, g, b);
        }

        public MainWindow()
        {
            
            InitializeComponent();
            Task optTask = new Task(someFunc);
            optTask.Start();
            //timer
            //TimerCallback time = new TimerCallback(visualize);
            //SynchronizationContext uiContext = SynchronizationContext.Current;
            //t = new Timer(time, uiContext, 0, 1000);

            //new timer
            dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(visualize_timer_new);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();

            //Line ordinat = new Line();
            //ordinat.Stretch = Stretch.Uniform;
            //ordinat.Stroke = System.Windows.Media.Brushes.Black;
            //ordinat.X1 = 100;
            //ordinat.Y1 = 0;
            //ordinat.X2 = 0;  // 150 too far
            //ordinat.Y2 = 0;
            //ordinat.SnapsToDevicePixels = true;
            //ordinat.SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Aliased);
            //ordinat.StrokeThickness = 2;

            //Line absciss = new Line();
            //absciss.Stroke = System.Windows.Media.Brushes.Black;
            //absciss.X1 = 0;
            //absciss.Y1 = 0;
            //absciss.X2 = 0;  // 150 too far
            //absciss.Y2 = 100;
            //absciss.SnapsToDevicePixels = true;
            //absciss.SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Aliased);
            //absciss.StrokeThickness = 2;

            //Arrow r1 = new Arrow(new Point(0, 0), new Point(0, 100));
            //CanvasDraw.Children.Add(r1);
            //CanvasDraw.Children.Add(new Arrow(new Point(0, 0), new Point(100, 0)));
            //r1.SetColor(Brushes.Red);

        }

    }
}

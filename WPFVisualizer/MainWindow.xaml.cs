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
        private float scale = 80;
        private Queue<AbstractIndividual> _Queue = new Queue<AbstractIndividual>();
        private DispatcherTimer dispatcherTimer;

        void someFunc()
        {
            int generationSize = 5000;

            GeneticAlgorithms.Control control = new GeneticAlgorithms.Control("../../../picture.xml", generationSize, fractionOfNewIndividuals: 0.9);
            Mutator mutator = new Mutator(segmentFlipProbability: 0.01, mutationProbability: 0.05);

            while (true)
            {
                control.OptimizeStep(Crosser.CyclicCrossover, mutator.ReverseSegmentMutation);
                _Queue.Enqueue(new Plate((Plate)control.bestIndividual));
            }

        }

        Point fromVector(Vector2 vec, float scale) {
            return new Point(vec.X * scale, vec.Y * scale);
        }

        private void visualize_timer_new(object sender, EventArgs e) {
            if (_Queue.Count == 0) { return; }
            textBox.Clear();
            CanvasDraw.Children.Clear();
            AbstractIndividual deq = _Queue.Dequeue();
            textBox.AppendText("Лучший в поколении № " + deq.ToString());
            textBox.AppendText("Queue count: "+_Queue.Count);
            bool b = true;
            foreach (Segment seg in deq.Segments.ToArray())
            {
                Arrow segmentArrow = new Arrow(fromVector(seg.End, scale), fromVector(seg.Start, scale), 5);
                //дописать создание радуги
                SolidColorBrush br = null;
                if (b == true)
                {
                    br = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                    b = false;
                }
                else
                {
                    br = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                }
                segmentArrow.SetColor(br);
                CanvasDraw.Children.Add(segmentArrow);
                
            }

            for (int i = 0; i < deq.Segments.Count - 1; i++)
            {
                Arrow spareArrow = new Arrow(fromVector(deq.Segments[i].Start, scale), fromVector(deq.Segments[i + 1].End, scale));
                SolidColorBrush br = new SolidColorBrush(GetRainbow(1023/deq.Size() * i));
                spareArrow.SetColor(br);
                CanvasDraw.Children.Add(spareArrow);
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
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 1000);
            dispatcherTimer.Start();
        }

    }
}

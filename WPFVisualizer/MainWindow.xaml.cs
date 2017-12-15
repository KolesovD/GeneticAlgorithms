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

            //Console.WriteLine("Start with generation size {0}", generationSize);
            GeneticAlgorithms.Control control = new GeneticAlgorithms.Control("../../../picture.xml", generationSize, fractionOfNewIndividuals: 0.9);
            Mutator mutator = new Mutator(segmentFlipProbability: 0.01, mutationProbability: 0.05);

            while (true)
            {
                //Thread.Sleep(100);
                //Console.WriteLine($"Поколение №{control.currentGenerationNumber}");
                control.OptimizeStep(Crosser.CyclicCrossover, mutator.ReverseSegmentMutation);
                _Queue.Enqueue(control.bestIndividual);    
                //Console.WriteLine("Лучший в поколении №" + control.currentGenerationNumber + "\n" + control.bestIndividual);
            }

        }

        //void visualize(object obj) {
        //        //Thread.Sleep(100);
        //        SynchronizationContext _uiContext = obj as SynchronizationContext;
        //        _uiContext.Post((objs) =>
        //        {
        //            textBox.AppendText("tik!!!");
        //        }, null);
        //        try
        //        {
        //            AbstractIndividual deq = _Queue.Peek();

        //            if (deq != null)
        //            {
                        
        //                _uiContext.Post(visualizeDraw, deq);
        //            }
        //        }
        //        catch (InvalidOperationException err) { }
        //}

        //void visualizeDraw(object data) {
        //    AbstractIndividual deq = data as AbstractIndividual;
        //    foreach (Segment seg in deq.Segments) {
        //        if (seg.Direction) {
        //            CanvasDraw.Children.Add(new Arrow(fromVector(seg.Point1, 100), fromVector(seg.Point2, 100)));
        //        }
        //        else {
        //            CanvasDraw.Children.Add(new Arrow(fromVector(seg.Point2, 100), fromVector(seg.Point1, 100)));
        //        }
        //    }
        //}

        Point fromVector(Vector2 vec, float scale) {
            return new Point(vec.X * scale, vec.Y * scale);
        }

        private void visualize_timer_new(object sender, EventArgs e) {
            textBox.AppendText("tik!!!");
            CanvasDraw.Children.Clear();
            AbstractIndividual deq = _Queue.Peek();
            foreach (Segment seg in deq.Segments.ToArray())
            {
                Arrow segmentArrow = new Arrow(fromVector(seg.Start, 100), fromVector(seg.End, 100), 5);
                //дописать создание радуги
                SolidColorBrush br = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                segmentArrow.SetColor(br);
                CanvasDraw.Children.Add(segmentArrow);
            }

            for (int i = 0; i < deq.Segments.Count-1; i++)
            {               
                Arrow spareArrow = new Arrow(fromVector(deq.Segments[i].Start, 100), fromVector(deq.Segments[i+1].End, 100));
                byte d = Convert.ToByte(255 / deq.Segments.Count * i);
                byte r = Convert.ToByte(255 - d);
                SolidColorBrush br = new SolidColorBrush(Color.FromRgb(r, d, 255));
                spareArrow.SetColor(br);
                CanvasDraw.Children.Add(spareArrow);
            }
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
        }

    }
}

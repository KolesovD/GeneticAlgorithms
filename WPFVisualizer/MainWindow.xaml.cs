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

namespace WPFVisualizer
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Queue<AbstractIndividual> _Queue = new Queue<AbstractIndividual>();

        void someFunc()
        {
            int generationSize = 5000;

            //Console.WriteLine("Start with generation size {0}", generationSize);
            GeneticAlgorithms.Control control = new GeneticAlgorithms.Control(generationSize, fractionOfNewIndividuals: 0.9);
            Mutator mutator = new Mutator(segmentFlipProbability: 0.01, mutationProbability: 0.05);

            while (true)
            {
                Thread.Sleep(100);
                //Console.WriteLine($"Поколение №{control.currentGenerationNumber}");
                control.OptimizeStep(Crosser.CyclicCrossover, mutator.ReverseSegmentMutation);
                _Queue.Enqueue(control.bestIndividual);
                
                //Console.WriteLine("Лучший в поколении №" + control.currentGenerationNumber + "\n" + control.bestIndividual);
            }

        }

        void visualize(object obj) {
            while (true)
            {
                Thread.Sleep(100);
                SynchronizationContext _uiContext = obj as SynchronizationContext;
                try
                {
                    AbstractIndividual deq = _Queue.Peek();

                    if (deq != null)
                    {
                        //textBox.AppendText(deq.ToString());
                        _uiContext.Post((objs) =>
                        {
                            textBox.AppendText(deq.ToString());
                        }, null);
                    }
                }
                catch (InvalidOperationException err) { }
            }
        }

        public MainWindow()
        {
            
            InitializeComponent();
            Task optTask = new Task(someFunc);
            optTask.Start();
            TimerCallback time = new TimerCallback(visualize);
            SynchronizationContext uiContext = SynchronizationContext.Current;
            Timer t = new Timer(time, uiContext, 0, 1000);

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

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
using WPFVisualizer.Code;
using System.Collections.Concurrent;
using WPFVisualizer.Extensions;

namespace WPFVisualizer
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private VisualiserFuncs Code;
        private ConcurrentQueue<Info> Queue = new ConcurrentQueue<Info>();
        private DispatcherTimer dispatcherTimer;
        private Point last_pos;
        private Info DequeueIndidvidual;
        private MasterControl GA;
        private float _thickness = 0.1f;

        private void GeneticAlgotithmFunc() {
            int generationSize = 2000;
            int island_count = 4;
            int migration_count = (int)(generationSize * 0.4f);
            int k = 10;
            int g = k * island_count;
            Mutator mutator = new Mutator(segmentFlipProbability: 0.01, mutationProbability: 0.01);

            GA = new MasterControl(migration_count, island_count, "../../../Lines.xml", generationSize, 
                (i) => {
                    return Crosser.CyclicCrossover;
                }, 
                (i) => {
                    return mutator.ReverseSegmentMutation;
                }, 
                (i) => {
                    return (c) => {
                        //if (i != 0) { return; }
                        if (g > 0) { g--; return; }
                        AbstractIndividual best = c.bestIndividual;
                        Queue.Enqueue(new Info(best.GetCopy(), string.Format("Поколение № {0} остров № {1} количество миграций {2}", c.currentGenerationNumber, i, c.MigrationCount)));
                        g = k * island_count;
                    }; 
                }
            );
            GA.Start();
            //GeneticAlgorithms.Control control = new GeneticAlgorithms.Control("../../../Lines.xml", generationSize);
            

            //while (true) {
            //    stopper.WaitOne();
            //    control.OptimizeStep(Crosser.CyclicCrossover, mutator.ReverseSegmentMutation);
            //    Queue.Enqueue(new Info(new Plate((Plate)control.bestIndividual), string.Format("Поколение № {0}", control.currentGenerationNumber)));
            //}

        }

        private void VisualizeTimer(object sender, EventArgs e) {
            if (Queue.Count == 0) {
                return;
            }
            else {
                textBox.Clear();
                CanvasDraw.Children.Clear();
                if (!Queue.TryDequeue(out DequeueIndidvidual)) { return; }
            }
            textBox.AppendText(DequeueIndidvidual.GenInfo);
            textBox.AppendText("\n");
            textBox.AppendText("Лучший в поколении: " + DequeueIndidvidual.Individual.ToString());
            textBox.AppendText("Queue count: "+ Queue.Count);

            SolidColorBrush segment_color = new SolidColorBrush(Color.FromRgb(255, 0, 0));
            Segment[] segment_array = DequeueIndidvidual.Individual.Segments.ToArray();
            SolidColorBrush link_color = null;
            Arrow segmentArrow = null;

            for (int i = 0; i < segment_array.Length-1; i++) {
                segmentArrow = new Arrow(segment_array[i].Start, segment_array[i].End, _thickness);
                segmentArrow.SetColor(segment_color);
                CanvasDraw.Children.Add(segmentArrow);

                Arrow LinkArrow = new Arrow(segment_array[i].End, segment_array[i+1].Start, _thickness/3f);
                link_color = new SolidColorBrush(Code.GetRainbow(1023 / DequeueIndidvidual.Individual.Size() * i));
                LinkArrow.SetColor(link_color);
                CanvasDraw.Children.Add(LinkArrow);

                if (segment_color.Color.R == 255) {
                    segment_color = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                }
            }
            segmentArrow = new Arrow(segment_array[segment_array.Length-1].Start, segment_array[segment_array.Length - 1].End, _thickness);
            segmentArrow.SetColor(segment_color);
            CanvasDraw.Children.Add(segmentArrow);
        }

        public MainWindow()
        {
            Code = new VisualiserFuncs();
            InitializeComponent();

            last_pos = new Point();

            this.MouseWheel += Box_MouseWheel;
            this.MouseMove += MainWindow_MouseMove;
            this.MouseLeftButtonDown += MainWindow_MouseLeftButtonDown;
            this.MouseLeftButtonUp += MainWindow_MouseLeftButtonUp;

            GeneticAlgotithmFunc();

            dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(VisualizeTimer);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            dispatcherTimer.Start();
        }

        private void MainWindow_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            CanvasDraw.ReleaseMouseCapture();
        }

        void MainWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 1)
            {
                CanvasDraw.Focus();
                last_pos = e.GetPosition(CanvasDraw);
                CanvasDraw.CaptureMouse();
            }
            e.Handled = true;
        }

        void MainWindow_MouseMove(object sender, MouseEventArgs e)
        {
            if (CanvasDraw.IsMouseCaptured)
            {
                Point point = e.GetPosition(CanvasDraw);
                CanvasDraw.RenderTransform = new MatrixTransform(Matrix.Multiply(new TranslateTransform(point.X - last_pos.X, point.Y - last_pos.Y).Value, CanvasDraw.RenderTransform.Value));
                
            }
        }

        void Box_MouseWheel(object sender, MouseWheelEventArgs e) 
        {
            Point mousePoint = e.GetPosition(CanvasDraw);

            double _scale =  MathExtensions.Clamp(e.Delta/100f + 1f, 0, float.PositiveInfinity);

            CanvasDraw.RenderTransform = new MatrixTransform(Matrix.Multiply(new ScaleTransform(_scale, _scale, mousePoint.X, mousePoint.Y).Value, CanvasDraw.RenderTransform.Value));
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            if (GA.StopState)
            {
                GA.ResetStopper();//осановка
                stop.Content = "continue";
            }
            else
            {
                GA.SetStopper();//запуск
                stop.Content = "stop";
            }
        }
    }
}

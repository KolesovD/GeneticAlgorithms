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

namespace WPFVisualizer
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private VisualiserFuncs Code;
        private Queue<Info> Queue = new Queue<Info>();
        private DispatcherTimer dispatcherTimer;
        private Vector2 last_pos;
        private Info DequeueIndidvidual;
        private float scale = 100;
        float ScaleRate = 1.1f;
        private bool stop_state = true;
        private ManualResetEvent stopper;

        private void GeneticAlgotithmFunc() {
            int generationSize = 5000;

            GeneticAlgorithms.Control control = new GeneticAlgorithms.Control("../../../Lines.xml", generationSize, fractionOfNewIndividuals: 0.9);
            Mutator mutator = new Mutator(segmentFlipProbability: 0.01, mutationProbability: 0.01);

            while (true) {
                stopper.WaitOne();
                control.OptimizeStep(Crosser.CyclicCrossover, mutator.ReverseSegmentMutation);
                Queue.Enqueue(new Info(new Plate((Plate)control.bestIndividual), string.Format("Поколение № {0}", control.currentGenerationNumber)));
            }

        }

        private void VisualizeTimer(object sender, EventArgs e) {
            if (Queue.Count == 0) {
                return;
            }
            else {
                textBox.Clear();
                CanvasDraw.Children.Clear();
                DequeueIndidvidual = Queue.Dequeue();
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
                segmentArrow = new Arrow(segment_array[i].Start*scale, segment_array[i].End * scale, 5);
                segmentArrow.SetColor(segment_color);
                CanvasDraw.Children.Add(segmentArrow);

                Arrow LinkArrow = new Arrow(segment_array[i].End * scale, segment_array[i+1].Start * scale);
                link_color = new SolidColorBrush(Code.GetRainbow(1023 / DequeueIndidvidual.Individual.Size() * i));
                LinkArrow.SetColor(link_color);
                CanvasDraw.Children.Add(LinkArrow);

                if (segment_color.Color.R == 255) {
                    segment_color = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                }
            }
            segmentArrow = new Arrow(segment_array[segment_array.Length-1].Start * scale, segment_array[segment_array.Length - 1].End * scale, 5);
            segmentArrow.SetColor(segment_color);
            CanvasDraw.Children.Add(segmentArrow);
        }

        public MainWindow()
        {
            stopper = new ManualResetEvent(stop_state);
            Code = new VisualiserFuncs();
            InitializeComponent();

            last_pos = new Vector2(0, 0);

            this.MouseWheel += Box_MouseWheel;
            this.MouseMove += MainWindow_MouseMove;
            this.MouseLeftButtonDown += MainWindow_MouseLeftButtonDown;

            Task optTask = new Task(GeneticAlgotithmFunc);
            optTask.Start();

            dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(VisualizeTimer);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            dispatcherTimer.Start();
        }

        void MainWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            last_pos.X = (float)e.GetPosition(CanvasDraw).X;
            last_pos.Y = (float)e.GetPosition(CanvasDraw).Y;
        }

        void MainWindow_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) {
                position.X += (e.GetPosition(CanvasDraw).X - last_pos.X);
                position.Y += (e.GetPosition(CanvasDraw).Y - last_pos.Y);
            }
        }

        void Box_MouseWheel(object sender, MouseWheelEventArgs e) {
            if (e.Delta > 0)
            {
                CanvasScale.CenterX = e.GetPosition(CanvasMove).X;
                CanvasScale.CenterY = e.GetPosition(CanvasMove).Y;
                CanvasScale.ScaleX *= ScaleRate;
                CanvasScale.ScaleY *= ScaleRate;
            }
            else
            {
                CanvasScale.ScaleX /= ScaleRate;
                CanvasScale.ScaleY /= ScaleRate;
                //c.Width *= ScaleRate;
                //c.Height *= ScaleRate;
                CanvasScale.CenterX = e.GetPosition(CanvasMove).X;
                CanvasScale.CenterY = e.GetPosition(CanvasMove).Y;
            }

            //CanvasScale.CenterX = e.GetPosition(CanvasSkale).X;
            //CanvasScale.CenterY = e.GetPosition(CanvasSkale).Y;
            //CanvasScale.ScaleX += e.Delta * 0.0005f;
            //CanvasScale.ScaleY += e.Delta * 0.0005f;
            //if (CanvasScale.ScaleX <= 0.1) {
            //    CanvasScale.ScaleX = 0.1;
            //    CanvasScale.ScaleY = 0.1;
            //}
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            if (stop_state)
            {
                stopper.Reset();//осановка
                stop.Content = "continue";
            }
            else
            {
                stopper.Set();//запуск
                stop.Content = "stop";
            }
            stop_state = !stop_state;
        }
    }
}

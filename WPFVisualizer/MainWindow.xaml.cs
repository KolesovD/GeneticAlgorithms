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
        private Queue<AbstractIndividual> Queue = new Queue<AbstractIndividual>();
        private DispatcherTimer dispatcherTimer;
        private Vector2 last_pos;
        private AbstractIndividual DequeueIndidvidual;
        private float scale = 50;

        private void GeneticAlgotithmFunc() {
            int generationSize = 5000;

            GeneticAlgorithms.Control control = new GeneticAlgorithms.Control("../../../picture_3.xml", generationSize, fractionOfNewIndividuals: 0.9);
            Mutator mutator = new Mutator(segmentFlipProbability: 0.01, mutationProbability: 0.05);

            while (true) {
                control.OptimizeStep(Crosser.CyclicCrossover, mutator.ReverseSegmentMutation);
                Queue.Enqueue(new Plate((Plate)control.bestIndividual));
            }

        }

        private void VisualizeTimer(object sender, EventArgs e) {
            if (Queue.Count == 0)
            {
                return;
            }
            else {
                textBox.Clear();
                CanvasDraw.Children.Clear();
                DequeueIndidvidual = Queue.Dequeue();
            }

            textBox.AppendText("Лучший в поколении № " + DequeueIndidvidual.ToString());
            textBox.AppendText("Queue count: "+ Queue.Count);

            SolidColorBrush segment_color = new SolidColorBrush(Color.FromRgb(255, 0, 0));
            Segment[] segment_array = DequeueIndidvidual.Segments.ToArray();
            SolidColorBrush link_color = null;
            Arrow segmentArrow = null;

            for (int i = 0; i < segment_array.Length-1; i++) {
                segmentArrow = new Arrow(segment_array[i].Start*scale, segment_array[i].End * scale, 5);
                segmentArrow.SetColor(segment_color);
                CanvasDraw.Children.Add(segmentArrow);

                Arrow LinkArrow = new Arrow(segment_array[i].End * scale, segment_array[i+1].Start * scale);
                link_color = new SolidColorBrush(Code.GetRainbow(1023 / DequeueIndidvidual.Size() * i));
                LinkArrow.SetColor(link_color);
                CanvasDraw.Children.Add(LinkArrow);

                if (segment_color.Color.R == 255) {
                    segment_color = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                }
            }
            segmentArrow = new Arrow(segment_array[segment_array.Length-1].Start * scale, segment_array[segment_array.Length - 1].End * scale, 5);
            segmentArrow.SetColor(segment_color);
            CanvasDraw.Children.Add(segmentArrow);

            //foreach (Segment seg in DequeueIndidvidual.Segments) {
            //    Arrow segmentArrow = new Arrow(, , 5);
            //    segmentArrow.SetColor(br);
            //    CanvasDraw.Children.Add(segmentArrow);
            //    if (br.Color.R == 255) {
            //        br = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            //    }
            //}

            //for (int i = 0; i < DequeueIndidvidual.Segments.Count - 1; i++)
            //{
            //    Arrow spareArrow = new Arrow(fromVector(DequeueIndidvidual.Segments[i].Start, scale), fromVector(DequeueIndidvidual.Segments[i + 1].End, scale));
            //    br = new SolidColorBrush(Code.GetRainbow(1023/ DequeueIndidvidual.Size() * i));
            //    spareArrow.SetColor(br);
            //    CanvasDraw.Children.Add(spareArrow);
            //}
        }

        public MainWindow()
        {
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
            CanvasScale.CenterX = e.GetPosition(CanvasDraw).X;
            CanvasScale.CenterY = e.GetPosition(CanvasDraw).Y;
            CanvasScale.ScaleX += e.Delta * 0.001f;
            CanvasScale.ScaleY += e.Delta * 0.001f;
            if (CanvasScale.ScaleX <= 0.1) {
                CanvasScale.ScaleX = 0.1;
                CanvasScale.ScaleY = 0.1;
            }
        }
    }
}

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
using GeneticAlgorithms.Crossovers;
using GeneticAlgorithms.Mutations;
using System.Threading;
using System.Numerics;
using System.Windows.Threading;
using System.Collections.Concurrent;
using WPFVisualizer.Code;
using WPFVisualizer.Extensions;

namespace WPFVisualizer
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CancellationTokenSource CancellationTokenSource;

        private VisualiserFunctions Code;
        private ConcurrentQueue<Info> Queue = new ConcurrentQueue<Info>();
        private DispatcherTimer dispatcherTimer;
        private Canvas[] islandsCanvases;

        private Point last_pos;
        private MasterControl GA;
        private float _thickness = 0.1f;
        private float space = 1;

        private void GeneticAlgotithmFunc(string path = "../../../Lines.xml") 
        {
            CancellationTokenSource?.Cancel();
            CancellationTokenSource?.Dispose();
            CancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = CancellationTokenSource.Token;

            int generationSize = 2500;
            int island_count = 6;
            int migration_count = (int)(generationSize * 0.3f);
            double migrationProbability = 0.1f;
            int k = 5;
            int g = k * island_count;

            //IMutation mutator = new ReverseSegmentMutation(mutationProbability: 0.01);
            //ICrossover crossover = new CyclicCrossover();

            GA = new MasterControl(migration_count, island_count, path, generationSize, migrationProbability,
                (i) => {
                    List<(float, ICrossover)> crossoverList = new List<(float, ICrossover)>();
                    switch (i)
                    {
                        /*case 1:
                            crossoverList.Add((0.75f, new CyclicCrossover()));
                            crossoverList.Add((0.25f, new OrderedCrossover()));
                            break;
                        case 2:
                            crossoverList.Add((0.25f, new CyclicCrossover()));
                            crossoverList.Add((0.75f, new OrderedCrossover()));
                            break;
                        case 3:
                            crossoverList.Add((0.1f, new CyclicCrossover()));
                            crossoverList.Add((0.9f, new OrderedCrossover()));
                            break;
                        case 4:
                            crossoverList.Add((1f, new OrderedCrossover()));
                            break;
                        case 5:
                            crossoverList.Add((0.77f, new CyclicCrossover()));
                            crossoverList.Add((0.23f, new OrderedCrossover()));
                            break;
                        case 6:*/
                        default:
                            crossoverList.Add((1f, new CyclicCrossover()));
                            break;
                    }
                    
                    return crossoverList;
                },
                (i) => {
                    List<(float, IMutation)> mutationList = new List<(float, IMutation)>();
                    switch (i)
                    {
                        /*case 1:
                            mutationList.Add((1f, new ReverseSegmentMutation(mutationProbability: 0.2f)));
                            break;
                        case 2:
                            mutationList.Add((0.1f, new ReverseSegmentMutation(mutationProbability: 0.1f)));
                            break;
                        case 3:
                            mutationList.Add((0.4f, new InvertDirectionMutation(mutationProbability: 0.6f)));
                            mutationList.Add((0.4f, new ReverseSegmentMutation(mutationProbability: 0.6f)));
                            break;
                        case 4:
                            mutationList.Add((0.4f, new ReverseSegmentMutation(mutationProbability: 0.6f)));
                            break;
                        case 5:
                            mutationList.Add((1f, new InvertDirectionMutation(mutationProbability: 0.25f)));
                            break;
                        case 6:*/
                        default:
                            mutationList.Add((0.95f, new ReverseSegmentMutation(mutationProbability: 0.01f)));
                            mutationList.Add((0.05f, new InvertDirectionMutation(mutationProbability: 0.1f)));
                            break;
                    }

                    return mutationList;
                },
                (i) => {

                    return (c) => {
                        if (g > 0) { g--; return; }

                        AbstractIndividual best = c.bestIndividual;
                        Queue.Enqueue(new Info(best.GetCopy(), c.currentGenerationNumber, i, c.MigrationCount));
                        g = k * island_count;
                    }; 
                },
                token
            );
            UpdateButtonContent();

            CanvasDraw.Children.Clear();
            islandsCanvases = new Canvas[island_count];
            for (int i = 0; i < islandsCanvases.Length; i++)
            {
                islandsCanvases[i] = new Canvas();
                int dimension = (int)Math.Sqrt(GA.IslandCount);

                int row = (int)Math.Floor((float)i / (float)dimension);
                int column = i % dimension;

                Vector2 _offset = new Vector2(column * (GA.SizeX - GA.X) + space * column, row * (GA.SizeY - GA.Y) + space * row);

                CanvasDraw.Children.Add(islandsCanvases[i]);

                islandsCanvases[i].RenderTransform = new TranslateTransform(_offset.X, _offset.Y);
            }

            GA.Start();

            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += ForReset;
            dispatcherTimer.Tick += VisualizeTimer;            
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            dispatcherTimer.Start();
            CancellationTokenRegistration registration = default;
            registration = token.Register(() =>
            {
                dispatcherTimer.Stop();
                registration.Dispose();
            });

            void ForReset(object sender, EventArgs e) 
            {
                ResetMovement();
                dispatcherTimer.Tick -= ForReset;
            }

            
        }

        private void Draw(Info infos, Canvas canvas, int countLeft) 
        {
            textBox.Clear();

            //int c = 0;
            //for (; c < islandsCanvases.Length; c++)
            //{
            //    if (islandsCanvases[c] == canvas) break;
            //}

            //textBox.AppendText($"islandID: {c} children: {canvas.Children.Count}");

            canvas.Children.Clear();

            textBox.AppendText($"Queue count: {countLeft}\n" +
                $"{infos}\n" +
                $"Лучший в поколении: {infos.Individual}");

            IEnumerator<SolidColorBrush> _colors = Colors().GetEnumerator();
            Segment[] segment_array = infos.Individual.Segments.ToArray();

            for (int i = 0; i < segment_array.Length; i++)
            {
                _colors.MoveNext();
                Arrow segmentArrow = new Arrow(segment_array[i].Start, segment_array[i].End, _thickness);
                segmentArrow.SetColor(_colors.Current);
                canvas.Children.Add(segmentArrow);
            }

            for (int i = 0; i < segment_array.Length - 1; i++)
            {
                Arrow LinkArrow = new Arrow(segment_array[i].End, segment_array[i + 1].Start, _thickness / 3f);
                LinkArrow.SetColor(new SolidColorBrush(Code.GetRainbow(1023 / infos.Individual.Size() * i)));
                canvas.Children.Add(LinkArrow);
            }
        }

        private void VisualizeTimer(object sender, EventArgs e) {
            if (Queue.Count == 0) {
                return;
            }
            else {
                if (!Queue.TryDequeue(out Info DequeueIndidvidual)) { return; }

                Draw(DequeueIndidvidual, islandsCanvases[DequeueIndidvidual.islandID], Queue.Count);
            }
        }

        private IEnumerable<SolidColorBrush> Colors() 
        {
            yield return new SolidColorBrush(Color.FromRgb(255, 0, 0));
            SolidColorBrush others = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            while (true) 
            {
                yield return others;
            }
        }

        public MainWindow()
        {
            Code = new VisualiserFunctions();
            InitializeComponent();

            last_pos = new Point();

            this.MouseWheel += Box_MouseWheel;
            this.MouseMove += MainWindow_MouseMove;
            this.MouseLeftButtonDown += MainWindow_MouseLeftButtonDown;
            this.MouseLeftButtonUp += MainWindow_MouseLeftButtonUp;
        }

        #region movement

        #region translate

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

        #endregion

        #region scale

        void Box_MouseWheel(object sender, MouseWheelEventArgs e) 
        {
            Point mousePoint = e.GetPosition(CanvasDraw);

            double _scale = MathExtensions.Clamp(e.Delta/1000f + 1f, 0.1f, float.PositiveInfinity);

            CanvasDraw.RenderTransform = new MatrixTransform(Matrix.Multiply(new ScaleTransform(_scale, _scale, mousePoint.X, mousePoint.Y).Value, CanvasDraw.RenderTransform.Value));
        }

        #endregion

        private void ResetMovement() 
        {
            TransformGroup transformGroup = new TransformGroup();
            transformGroup.Children.Add(new TranslateTransform(0, 0));
            transformGroup.Children.Add(new ScaleTransform(10, 10));
            transformGroup.Children.Add(new RotateTransform(0));

            CanvasDraw.RenderTransform = transformGroup;
        }

        #endregion

        #region PlayPause

        private void Button_Click(object sender, RoutedEventArgs e) {
            if (GA.IsRunning)
            {
                GA.Pause();//осановка
            }
            else
            {
                GA.Continue();//запуск
            }

            UpdateButtonContent();
        }

        private void UpdateButtonContent() 
        {
            if (GA.IsRunning)
            {
                stop.Content = "stop";
            }
            else
            {
                stop.Content = "continue";
            }
        }

        #endregion

        private void OnOpenDocument(object sender, RoutedEventArgs e)
        {
            OpenDocument();
        }

        private void OpenDocument() 
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            //dlg.FileName = "Document"; // Default file name
            dlg.DefaultExt = ".xml"; // Default file extension
            dlg.Filter = "Xml documents (.xml)|*.xml"; // Filter files by extension

            // Show open file dialog box
            bool? result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result.HasValue && result == true)
            {
                GeneticAlgotithmFunc(dlg.FileName);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            OpenDocument();
        }

        private void ResetPosition(object sender, RoutedEventArgs e)
        {
            ResetMovement();
        }
    }
}

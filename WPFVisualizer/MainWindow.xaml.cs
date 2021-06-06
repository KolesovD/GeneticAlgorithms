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
using System.Windows.Media.Media3D;
using Quaternion = System.Windows.Media.Media3D.Quaternion;

namespace WPFVisualizer
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CancellationTokenSource CancellationTokenSource;

        private VisualiserFuncs Code;
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

            int generationSize = 5000;
            int island_count = 6;
            int migration_count = (int)(generationSize * 0.3f);
            double migrationProbability = 0.1f;
            int k = 5;
            int g = k * island_count;
            Mutator mutator = new Mutator(segmentFlipProbability: 0.01, mutationProbability: 0.01);

            GA = new MasterControl(migration_count, island_count, path, generationSize, migrationProbability,
                (i) => {
                    return Crosser.CyclicCrossover;
                }, 
                (i) => {
                    return mutator.ReverseSegmentMutation;
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

            double _circle = new Vector3D((GA.SizeX - GA.X), (GA.SizeY - GA.Y), 0).Length * island_count;

            Vector3D _startOffset = new Vector3D(_circle/(2 * Math.PI), 0f, 0f);
            for (int i = 0; i < islandsCanvases.Length; i++)
            {
                islandsCanvases[i] = new Canvas();
                CanvasDraw.Children.Add(islandsCanvases[i]);

                Quaternion quaternion = new Quaternion(new Vector3D(0,0,1), (360.0f / (float)island_count) * i);
                Vector3D _offset = MathExtensions.RotateVector3(quaternion, _startOffset);

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

            var _intervals = GetDrawSegments(
                                        segment_array
                                            .SelectMany(_segment => LinqExtetions.FromParams(_segment.Start, _segment.End))
                                    ).ToArray();

            _intervals.ForEach((_data, i) =>
            {
                Arrow LinkArrow = new Arrow(_data.Item1, _data.Item2, _thickness / 3f);
                double _normalizedNum = ((double)i).Remap(0, ((double)(_intervals.Length-1)), 0d, 1d);
                LinkArrow.SetColor(new SolidColorBrush(Code.GetRainbowColorNormalized(_normalizedNum)));
                canvas.Children.Add(LinkArrow);
            });
        }

        public IEnumerable<(Vector2, Vector2)> GetDrawSegments(IEnumerable<Vector2> collection) 
        {
            Vector2? _last = default;

            foreach (var item in collection)
            {
                if (_last.HasValue)
                {
                    if (!(_last.Value - item).Length().IsApproximatelyEqualTo(0)) 
                    {
                        yield return (_last.Value, item);
                    }
                }

                _last = item;
            }
        }

        private void VisualizeTimer(object sender, EventArgs e) {
            if (Queue.Count == 0) 
            {
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
            Code = new VisualiserFuncs();
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

            double _scale =  MathExtensions.Clamp(e.Delta/1000f + 1f, 0.1f, float.PositiveInfinity);

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

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
using System.Windows.Media.Media3D;
using Quaternion = System.Windows.Media.Media3D.Quaternion;
using GeneticAlgorithms.Information;
using System.Text.RegularExpressions;
using System.Diagnostics;
using WPFVisualizer.VisualControlls;

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

        private void GeneticAlgotithmFunc(ILoader plateLoader) 
        {
            CancellationTokenSource?.Cancel();
            CancellationTokenSource?.Dispose();
            CancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = CancellationTokenSource.Token;

            int generationSize = 5;
            int island_count = 2;
            int migration_count = (int)(generationSize * 0.3f);
            double migrationProbability = 0.1f;
            int k = 5;
            int g = k * island_count;

            //IMutation mutator = new ReverseSegmentMutation(mutationProbability: 0.01);
            //ICrossover crossover = new CyclicCrossover();

            GA = new MasterControl(
                migration_count, 
                island_count, 
                plateLoader, 
                generationSize, 
                migrationProbability,
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
                        //if (g > 0) { g--; return; }

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

            Vector3D _plateSize = new Vector3D((GA.SizeX), (GA.SizeY), 0);
            Vector3D _iselandSize = new Vector3D((GA.SizeX-GA.X), (GA.SizeY-GA.Y), 0);
            double _circle = _plateSize.Length * island_count;

            Vector3D _startOffset = new Vector3D(0f, -Math.Max(_iselandSize.Length, _circle / (2 * Math.PI)), 0f);
            for (int i = 0; i < islandsCanvases.Length; i++)
            {
                islandsCanvases[i] = new Canvas();
                CanvasDraw.Children.Add(islandsCanvases[i]);

                Quaternion quaternion = new Quaternion(new Vector3D(0,0,1), (360.0f / (float)island_count) * i);
                Vector3D _offset = MathExtensions.RotateVector3(quaternion, _startOffset);

                islandsCanvases[i].RenderTransform = MathExtensions
                    .GetTransformGroup
                    (
                        new TranslateTransform(_offset.X, _offset.Y),
                        new TranslateTransform(-_plateSize.X/2f, -_plateSize.Y/2f)
                    );

                Vector3D _offsetNormal = _offset;
                _offsetNormal.Normalize();

                AddText(_offset - (_offsetNormal* _iselandSize.Length/2f), $"Island {i}", new SolidColorBrush(Color.FromRgb(0, 0, 0)), CanvasDraw);
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

        private void AddText(Vector3D position, string text, Brush color, Canvas canvas)
        {
            TextBlock textBlock = new TextBlock();
            textBlock.Text = text;
            textBlock.Foreground = color;

            canvas.Children.Add(textBlock);

            var transformGroup = MathExtensions.GetTransformGroup
                (
                    new ScaleTransform(0.1f, 0.1f),
                    new TranslateTransform(position.X, position.Y)
                );

            textBlock.RenderTransform = transformGroup;
        }
        private async void Draw(Info infos, Canvas canvas, int countLeft) 
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


            SolidColorBrush others = new SolidColorBrush(Color.FromRgb(0, 0, 0));

            //await Task.WhenAll(
            //    infos
            //        .Individual
            //        .Segments
            //        .Select(async _segment =>
            //        {
            //            await Task.Yield();
            //            Arrow segmentArrow = new Arrow(_segment.Start, _segment.End, _thickness);
            //            segmentArrow.SetColor(others);
            //            canvas.Children.Add(segmentArrow);
            //        })
            //    );

            //var shape = new PlateShape();
            //shape.SetSegments(infos.Individual.Segments, _thickness);
            //shape.SetColor(others);
            //canvas.Children.Add(shape);

            //var _path = PlateShape.SetPath(infos.Individual.Segments, _thickness / 3f);
            ////_path.Stroke = Code.GetBrushRainbow();
            //canvas.Children.Add(_path);

            //IEnumerator<SolidColorBrush> _colors = Colors().GetEnumerator();
            //Segment[] segment_array = infos.Individual.Segments.ToArray();

            //for (int i = 0; i < segment_array.Length; i++)
            //{

            //    _colors.MoveNext();
            //    Arrow segmentArrow = new Arrow(segment_array[i].Start, segment_array[i].End, _thickness);
            //    segmentArrow.SetColor(_colors.Current);
            //    canvas.Children.Add(segmentArrow);
            //}

            //var _intervals = GetDrawSegments(
            //                            segment_array
            //                                .SelectMany(_segment => LinqExtetions.FromParams(_segment.Start, _segment.End))
            //                        ).ToArray();

            //_intervals.ForEach((_data, i) =>
            //{
            //    Arrow LinkArrow = new Arrow(_data.Item1, _data.Item2, _thickness / 3f);
            //    double _normalizedNum = ((double)i).Remap(0, ((double)(_intervals.Length - 1)), 0d, 1d);
            //    LinkArrow.SetColor(new SolidColorBrush(Code.GetRainbowColorNormalized(_normalizedNum)));
            //    canvas.Children.Add(LinkArrow);
            //});

            try
            {
                Pen drawingpen = new Pen(Brushes.Black, _thickness);
                DrawingVisual visual = new DrawingVisual();
                using (DrawingContext dc = visual.RenderOpen())
                {
                    foreach (var item in infos.Individual.Segments)
                    {
                        VisualPlateControll.DrawArrow(dc, item.Start, item.End, drawingpen);
                    }
                }

                canvas.Children.Add(visual.Wrap());
            }
            catch (Exception err) 
            {
                Console.WriteLine(err);
            }

            
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
            //dlg.DefaultExt = ".xml"; // Default file extension
            dlg.Filter = "Plate documents (.xml)|*.xml|Gerber documents (*.GTL)|*.GTL|Gerber documents (*.GBL)|*.GBL";

            // Show open file dialog box
            bool? result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result.HasValue && result == true)
            {
                Regex xml = new Regex(@"xml", RegexOptions.IgnoreCase);
                Regex gerber = new Regex(@"(GTL)|(GBL)", RegexOptions.IgnoreCase);

                var _extensions = System.IO.Path.GetExtension(dlg.FileName);

                if (xml.IsMatch(_extensions))
                {
                    GeneticAlgotithmFunc(new XMLLoader(dlg.FileName));
                }
                else if (gerber.IsMatch(_extensions)) 
                {
                    GeneticAlgotithmFunc(new GerberLoader(dlg.FileName));
                }
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

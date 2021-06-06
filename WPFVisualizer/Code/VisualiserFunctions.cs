using System;
using System.Windows.Media;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GeneticAlgorithms;

namespace WPFVisualizer.Code
{
    public static class GradientStopCollectionExtensions
    {
        public static Color GetRelativeColor(this GradientStopCollection gsc, double offset)
        {
            GradientStop[] stops = gsc.OrderBy(x => x.Offset).ToArray();
            if (offset <= 0) return stops[0].Color;
            if (offset >= 1) return stops[stops.Length - 1].Color;

            var GenerationAxis = Enumerable
                .Range(0, stops.Length - 2)
                .Select<int, (GradientStop start, GradientStop end)> (_i => (stops[_i], stops[_i + 1]))
                .ToArray();

            int first = 0;
            int last = GenerationAxis.Length - 1;
            int index = 0;

            GradientStop before = stops[0];
            GradientStop after = stops[stops.Length - 1];

            while (first <= last)
            {
                index = first + ((last - first) / 2);
                (GradientStop start, GradientStop end) choose = GenerationAxis[index];

                if (choose.start.Offset > offset)
                {
                    //last change
                    last = index - 1;
                }
                else if (choose.end.Offset <= offset)
                {
                    //first change
                    first = index + 1;
                }
                else
                {
                    before = choose.start;
                    after = choose.end;
                    break;
                }
            }

            var color = new Color();

            color.ScA = (float)((offset - before.Offset) * (after.Color.ScA - before.Color.ScA) / (after.Offset - before.Offset) + before.Color.ScA);
            color.ScR = (float)((offset - before.Offset) * (after.Color.ScR - before.Color.ScR) / (after.Offset - before.Offset) + before.Color.ScR);
            color.ScG = (float)((offset - before.Offset) * (after.Color.ScG - before.Color.ScG) / (after.Offset - before.Offset) + before.Color.ScG);
            color.ScB = (float)((offset - before.Offset) * (after.Color.ScB - before.Color.ScB) / (after.Offset - before.Offset) + before.Color.ScB);

            return color;
        }
    }

    public class VisualiserFunctions {

        GradientStopCollection _rainbow;

        public VisualiserFunctions() 
        {
            _rainbow = new GradientStopCollection();

            var _c = RainbowColors().ToArray();

            foreach (var item in _c.Select((_color, _i) => (_color, (1d/(double)_c.Length)*(double)_i)))
            {
                _rainbow.Add(new GradientStop(item._color, item.Item2));
            }
        }

        private IEnumerable<Color> RainbowColors() 
        {
            yield return Colors.Red;
            yield return Colors.Orange;
            yield return Colors.Yellow;
            yield return Colors.Green;
            yield return Colors.Blue;
            yield return Colors.DarkBlue;
            yield return Colors.Purple;
        }

        public Color GetRainbowColorNormalized(double num)
        {
            return _rainbow.GetRelativeColor(num);
        }

        public Brush GetBrushRainbow() 
        {
            return new LinearGradientBrush(_rainbow);
        }
    }
}

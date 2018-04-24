using System;
using System.Windows.Media;

namespace WPFVisualizer.Code
{
    public class VisualiserFuncs {
        public Color GetRainbow(int index)
        {
            int _index = index % 1023;
            byte r = 0;
            byte g = 0;
            byte b = 0;

            if (_index >= 0 && _index < 170)
            {
                g = Convert.ToByte(1.5 * (_index - 0));
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
            else if (_index >= 511 && _index <= 852)
            {
                b = 255;
            }
            else if (_index > 852 && _index < 1023)
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
    }
}

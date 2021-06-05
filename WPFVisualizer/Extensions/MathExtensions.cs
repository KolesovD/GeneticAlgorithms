using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFVisualizer.Extensions
{
    public static class MathExtensions
    {
        public static T Clamp<T>(this T val, T min, T max) where T : IComparable<T>
        {
            if (val.CompareTo(min) < 0) return min;
            else if (val.CompareTo(max) > 0) return max;
            else return val;
        }
        public static float Remap(this float value, float from1, float to1, float from2, float to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }

        public static double Remap(this double value, double from1, double to1, double from2, double to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }

        public static bool IsApproximatelyEqualTo(this double initialValue, double value)
        {
            return IsApproximatelyEqualTo(initialValue, value, 0.00001);
        }

        public static bool IsApproximatelyEqualTo(this double initialValue, double value, double maximumDifferenceAllowed)
        {
            // Handle comparisons of floating point values that may not be exactly the same
            return (Math.Abs(initialValue - value) < maximumDifferenceAllowed);
        }

        public static bool IsApproximatelyEqualTo(this float initialValue, float value)
        {
            return IsApproximatelyEqualTo(initialValue, value, 0.00001);
        }

        public static bool IsApproximatelyEqualTo(this float initialValue, float value, float maximumDifferenceAllowed)
        {
            // Handle comparisons of floating point values that may not be exactly the same
            return (Math.Abs(initialValue - value) < maximumDifferenceAllowed);
        }
    }
}

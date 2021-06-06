using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using Quaternion = System.Windows.Media.Media3D.Quaternion;

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

        public static Vector3D RotateVector3(Quaternion quaternion, Vector3D vector) 
        {
            //Quaternion qConjugate = Quaternion.Conjugate(quaternion);
            //Quaternion qPoint = new Quaternion(vector.X, vector.Y, vector.Z, 0);
            //Quaternion qRotatePoint = quaternion * qPoint * qConjugate;

            //return new Vector3(qRotatePoint.X, qRotatePoint.Y, qRotatePoint.Z);

            /* work
            Matrix3D m = Matrix3D.Identity;
            m.Rotate(quaternion);
            return m.Transform(vector);
            */

            Quaternion qConjugate = quaternion;
            qConjugate.Conjugate();
            Quaternion qPoint = new Quaternion(vector.X, vector.Y, vector.Z, 0);
            Quaternion qRotatePoint = quaternion * qPoint * qConjugate;

            return new Vector3D(qRotatePoint.X, qRotatePoint.Y, qRotatePoint.Z);
        }

        public static Vector2 ToVector2(this Vector3 vector3) 
        {
            return new Vector2(vector3.X, vector3.Y);
        }

        public static Vector3 ToVector3(this Vector2 vector3)
        {
            return new Vector3(vector3.X, vector3.Y, 0);
        }
    }
}

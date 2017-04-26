using System;

namespace Assets
{
    namespace MyRandoms
    {
        public static class MyRandom
        {
            public static readonly Random rnd;

            static MyRandom()
            {
                rnd = new Random();
            }

            public static double GetRandomDouble()
            {
                return rnd.NextDouble();
            }
            public static double GetRandomDouble(double max)
            {
                return GetRandomDouble() * max;
            }
            public static double GetRandomDouble(double min, double max)
            {
                return rnd.NextDouble() * (max - min) + min;
            }
        }
    }
}
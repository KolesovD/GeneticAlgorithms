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

            public static double GetRandomDouble() //вернёт случайное число в диапазое 0 - 1
            {
                return rnd.NextDouble();
            }
            public static double GetRandomDouble(double max) //вернёт случайное число в диапазое 0 - max
            {
                return GetRandomDouble() * max;
            }
            public static double GetRandomDouble(double min, double max) //вернёт случайное число в диапазое min - max
            {
                return rnd.NextDouble() * (max - min) + min;
            }
        }
    }
}
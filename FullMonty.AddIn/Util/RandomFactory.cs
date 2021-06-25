using System;

namespace FullMonty.AddIn.Util
{
    public static class RandomFactory
    {
        private static readonly Random GlobalRandom = new Random();
        [ThreadStatic] private static Random perThreadRandom;

        public static Random ThisThreadsRandom
        {
            get
            {
                if (perThreadRandom == null)
                    lock (GlobalRandom) { perThreadRandom = CreateInstance(); }

                return perThreadRandom;
            }
        }

        public static Random CreateInstance()
        {
            lock (GlobalRandom) { return new Random(GlobalRandom.Next(int.MinValue, int.MaxValue)); }
        }
    }
}
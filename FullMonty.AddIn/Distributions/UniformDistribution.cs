using System;
using FullMonty.AddIn.Util;

namespace FullMonty.AddIn.Distributions
{
    public class UniformDistribution : IDistribution
    {
        private readonly int maxExclusive;
        private readonly Random random = RandomFactory.CreateInstance();

        private UniformDistribution(int min, int max)
        {
            if (min > max) throw new ArgumentOutOfRangeException(nameof(min), "min must be less than or equal to max");

            Min = min;
            Max = max;
            maxExclusive = Max + 1;
        }

        public int Min { get; }

        public int Max { get; }

        public double Sample()
        {
            return random.Next(Min, maxExclusive);
        }

        public void Sample(Span<double> samples)
        {
            for (var i = 0; i < samples.Length; i++) samples[i] = Sample();
        }

        public double Median => (Min + Max) / 2.0;

        public static UniformDistribution FromDiscreteBounds(int min, int max)
        {
            return new UniformDistribution(min, max);
        }

        public static UniformDistribution FromContinuousBounds(double min, double max)
        {
            return new UniformDistribution((int)Math.Floor(min), (int)Math.Ceiling(max));
        }

        protected bool Equals(UniformDistribution other)
        {
            return Min == other.Min && Max == other.Max;
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((UniformDistribution) obj);
        }

        public override int GetHashCode()
        {
            unchecked { return (Min * 397) ^ Max; }
        }
    }
}
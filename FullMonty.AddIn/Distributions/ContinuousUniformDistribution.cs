using System;
using FullMonty.AddIn.Util;

namespace FullMonty.AddIn.Distributions
{
    public class ContinuousUniformDistribution : IDistribution
    {
        private readonly Random random = RandomFactory.CreateInstance();

        public ContinuousUniformDistribution(double min, double max)
        {
            if (min > max) throw new ArgumentOutOfRangeException(nameof(min), "min must be less than or equal to max");

            Min = min;
            Max = max;
        }

        public double Min { get; }

        public double Max { get; }

        public double Sample()
        {
            return Min + random.NextDouble() * (Max - Min);
        }

        public void Sample(Span<double> samples)
        {
            for (var i = 0; i < samples.Length; i++) samples[i] = Sample();
        }

        private bool Equals(ContinuousUniformDistribution other)
        {
            return Min.Equals(other.Min) && Max.Equals(other.Max);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ContinuousUniformDistribution)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Min.GetHashCode() * 397) ^ Max.GetHashCode();
            }
        }
    }
}
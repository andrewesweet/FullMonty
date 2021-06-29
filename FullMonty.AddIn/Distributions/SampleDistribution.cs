using System;
using System.Collections.Generic;
using System.Linq;
using FullMonty.AddIn.Util;
using MathNet.Numerics.Statistics;

namespace FullMonty.AddIn.Distributions
{
    public class SampleDistribution : IDistribution
    {
        private readonly Random random = RandomFactory.CreateInstance();

        public SampleDistribution(IList<double> samples)
        {
            Samples = samples ?? throw new ArgumentNullException(nameof(samples), "can not be null");
            samples.Median();
        }

        public IList<double> Samples { get; }

        public double Sample()
        {
            return Samples[random.Next(Samples.Count)];
        }

        public void Sample(Span<double> samples)
        {
            for (var i = 0; i < samples.Length; i++) samples[i] = Sample();
        }

        private bool Equals(SampleDistribution other)
        {
            return Samples.SequenceEqual(other.Samples);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((SampleDistribution) obj);
        }

        public override int GetHashCode()
        {
            const int seed = 487;
            const int modifier = 31;

            unchecked { return Samples.Aggregate(seed, (current, item) => current * modifier + item.GetHashCode()); }
        }
    }
}
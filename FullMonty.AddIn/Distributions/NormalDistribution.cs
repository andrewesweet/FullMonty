using System;
using FullMonty.AddIn.Util;
using MathNet.Numerics;

namespace FullMonty.AddIn.Distributions
{
    public class NormalDistribution : IDistribution
    {
        private readonly Random random = RandomFactory.CreateInstance();

        private NormalDistribution(double mean, double stdDev)
        {
            if (stdDev < 0) throw new ArgumentException("must be greater than zero", nameof(stdDev));

            Mean = mean;
            StdDev = stdDev;
        }

        public double Mean { get; }

        public double StdDev { get; }

        public double Sample()
        {
            return SampleUnchecked(random, Mean, StdDev);
        }

        public void Sample(Span<double> samples)
        {
            SamplesUnchecked(random, samples, Mean, StdDev);
        }

        public double Median => Mean;

        internal static double SampleUnchecked(Random rnd, double mean, double stdDev)
        {
            double x;
            while (!PolarTransform(rnd.NextDouble(), rnd.NextDouble(), out x, out _)) { }

            return mean + stdDev * x;
        }

        protected bool Equals(NormalDistribution other)
        {
            return Mean.Equals(other.Mean) && StdDev.Equals(other.StdDev);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((NormalDistribution) obj);
        }

        public override int GetHashCode()
        {
            unchecked { return (Mean.GetHashCode() * 397) ^ StdDev.GetHashCode(); }
        }

        public static NormalDistribution FromMeanAndStandardDeviation(double mean, double stdDev)
        {
            return new NormalDistribution(mean, stdDev);
        }

        public static NormalDistribution FromNinetyPercentConfidenceIntervalBounds(double lower, double upper)
        {
            var mean = (upper + lower) / 2.0;
            var stdDev = (upper - lower) / 3.29;
            return FromMeanAndStandardDeviation(mean, stdDev);
        }

        internal static void SamplesUnchecked(Random rnd, Span<double> values, double mean, double stddev)
        {
            if (values.Length == 0) return;

            // Since we only accept points within the unit circle
            // we need to generate roughly 4/pi=1.27 times the numbers needed.
            var n = (int) Math.Ceiling(values.Length * 4 * Constants.InvPi);
            if (n.IsOdd()) n++;

            Span<double> uniform = stackalloc double[n];
            for (var i = 0; i < values.Length; ++i) uniform[i] = rnd.NextDouble();

            // Polar transform
            double x, y;
            var index = 0;
            for (var i = 0; i < uniform.Length && index < values.Length; i += 2)
            {
                if (!PolarTransform(uniform[i], uniform[i + 1], out x, out y)) continue;

                values[index++] = mean + stddev * x;
                if (index == values.Length) return;

                values[index++] = mean + stddev * y;
                if (index == values.Length) return;
            }

            // remaining, if any
            while (index < values.Length)
            {
                if (!PolarTransform(rnd.NextDouble(), rnd.NextDouble(), out x, out y)) continue;

                values[index++] = mean + stddev * x;
                if (index == values.Length) return;

                values[index++] = mean + stddev * y;
                if (index == values.Length) return;
            }
        }

        private static bool PolarTransform(double a, double b, out double x, out double y)
        {
            var v1 = 2.0 * a - 1.0;
            var v2 = 2.0 * b - 1.0;
            var r = v1 * v1 + v2 * v2;
            if (r >= 1.0 || r == 0.0)
            {
                x = 0;
                y = 0;
                return false;
            }

            var fac = Math.Sqrt(-2.0 * Math.Log(r) / r);
            x = v1 * fac;
            y = v2 * fac;
            return true;
        }
    }
}
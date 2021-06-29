using System;
using System.Buffers;
using FullMonty.AddIn.Util;
using MathNet.Numerics.Statistics;

namespace FullMonty.AddIn.Distributions
{
    public class BetaDistribution : IDistribution
    {
        private readonly Lazy<double> median;
        private readonly Random random = RandomFactory.CreateInstance();

        private BetaDistribution(double a, double b, double min, double max)
        {
            if (a <= 0) throw new ArgumentException("must be positive", nameof(a));
            if (b <= 0) throw new ArgumentException("must be positive", nameof(a));
            if (min > max) throw new ArgumentException("min must be less that or equal to max", nameof(min));

            Min = min;
            Max = max;
            A = a;
            B = b;
            Scale = Max - Min;
            median = new Lazy<double>(EstimateMedian);
        }

        public double Max { get; }
        public double Min { get; }

        public double Scale { get; }

        public double B { get; }

        public double A { get; }

        public double Sample()
        {
            var x = GammaSampleUnchecked(random, A, 1.0);
            var y = GammaSampleUnchecked(random, B, 1.0);
            return Scale * x / (x + y) + Min;
        }

        public void Sample(Span<double> samples)
        {
            for (var i = 0; i < samples.Length; i++) samples[i] = Sample();
        }

        private static double GammaSampleUnchecked(Random rnd, double shape, double rate)
        {
            if (double.IsPositiveInfinity(rate)) return shape;

            var a = shape;
            var alphafix = 1.0;

            // Fix when alpha is less than one.
            if (shape < 1.0)
            {
                a = shape + 1.0;
                alphafix = Math.Pow(rnd.NextDouble(), 1.0 / shape);
            }

            var d = a - 1.0 / 3.0;
            var c = 1.0 / Math.Sqrt(9.0 * d);
            while (true)
            {
                var x = NormalDistribution.SampleUnchecked(rnd, 0.0, 1.0);
                var v = 1.0 + c * x;
                while (v <= 0.0)
                {
                    x = NormalDistribution.SampleUnchecked(rnd, 0.0, 1.0);
                    v = 1.0 + c * x;
                }

                v = v * v * v;
                var u = rnd.NextDouble();
                x *= x;
                if (u < 1.0 - 0.0331 * x * x) return alphafix * d * v / rate;

                if (Math.Log(u) < 0.5 * x + d * (1.0 - v + Math.Log(v))) return alphafix * d * v / rate;
            }
        }

        private double EstimateMedian()
        {
            const int numSamples = 1000;
            var samples = ArrayPool<double>.Shared.Rent(numSamples);
            Sample(samples);
            var result = samples.Median();
            ArrayPool<double>.Shared.Return(samples);
            return result;
        }

        public static BetaDistribution FromRelativeMean(double min, double max, double relativeMean)
        {
            var a = Math.Pow(relativeMean, 2.0) * (1.0 - relativeMean) * Math.Pow(6.0, 2.0) - 1.0;
            var b = (1 - relativeMean) / relativeMean * a;

            return new BetaDistribution(a, b, min, max);
        }

        public static BetaDistribution FromMode(double min, double max, double mode)
        {
            var relativeMean = ((mode - min) / (max - min) * 4.0 + 1.0) / 6.0;
            return FromRelativeMean(min, max, relativeMean);
        }

        protected bool Equals(BetaDistribution other)
        {
            return Max.Equals(other.Max) && Min.Equals(other.Min) && B.Equals(other.B) && A.Equals(other.A);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((BetaDistribution) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Max.GetHashCode();
                hashCode = (hashCode * 397) ^ Min.GetHashCode();
                hashCode = (hashCode * 397) ^ B.GetHashCode();
                hashCode = (hashCode * 397) ^ A.GetHashCode();
                return hashCode;
            }
        }
    }
}
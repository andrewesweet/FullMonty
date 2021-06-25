using System.Linq;
using FullMonty.AddIn.Distributions;
using MathNet.Numerics.Statistics;
using NUnit.Framework;

namespace FullMonty.UnitTests.Distributions
{
    public class BetaDistributionTest
    {
        [Test]
        public void SampleUsingMaxMinAndRelativeMean()
        {
            const int min = 10;
            const int max = 28;
            var beta = BetaDistribution.FromRelativeMean(min, max, 0.611111);
            Assert.AreEqual(4.228395062, beta.A, 0.000001);
            Assert.AreEqual(2.690796857, beta.B, 0.000001);

            const int numSamples = 1000;
            var samples = new double[numSamples];
            for (var i = 0; i < numSamples; i++) samples[i] = beta.Sample();

            var minSample = samples.Min();
            Assert.GreaterOrEqual(minSample, min);

            var maxSample = samples.Max();
            Assert.LessOrEqual(maxSample, max);

            Assert.AreEqual(beta.Median, samples.Median(), 0.5);
        }

        [Test]
        public void SampleUsingMaxMinAndMode()
        {
            const int min = 10;
            const int max = 28;
            var beta = BetaDistribution.FromMode(min, max, 22);
            Assert.AreEqual(4.228395062, beta.A, 0.000001);
            Assert.AreEqual(2.690796857, beta.B, 0.000001);

            const int numSamples = 1000;
            var samples = new double[numSamples];
            for (var i = 0; i < numSamples; i++) samples[i] = beta.Sample();

            var minSample = samples.Min();
            Assert.GreaterOrEqual(minSample, min);

            var maxSample = samples.Max();
            Assert.LessOrEqual(maxSample, max);

            Assert.AreEqual(beta.Median, samples.Median(), 0.5);
        }
    }
}
using System.Linq;
using FullMonty.AddIn.Distributions;
using MathNet.Numerics.Statistics;
using NUnit.Framework;

namespace FullMonty.UnitTests.Distributions
{
    public class SampleDistributionTest
    {
        [Test]
        public void ShouldBeSound()
        {
            var sampleDistribution = new SampleDistribution(new[] {1.0, 2.0, 3.0, 4.0, 5.0});

            const int numSamples = 1000;
            var samples = new double[numSamples];
            for (var i = 0; i < numSamples; i++) samples[i] = sampleDistribution.Sample();

            Assert.AreEqual(1.0, samples.Min());
            Assert.AreEqual(5.0, samples.Max());

            Assert.AreEqual(3.0, samples.Median());
            Assert.AreEqual(3.0, sampleDistribution.Median);
        }
    }
}
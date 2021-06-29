using System.Linq;
using FullMonty.AddIn.Distributions;
using MathNet.Numerics.Statistics;
using NUnit.Framework;

namespace FullMonty.UnitTests.Distributions
{
    public class NormalDistributionTest
    {
        [Test]
        public void ShouldBeSound()
        {
            const double lower = 1.0;
            const double upper = 10.0;
            var normal = NormalDistribution.FromNinetyPercentConfidenceIntervalBounds(lower, upper);

            const int numSamples = 1000;
            var samples = new double[numSamples];
            for (var i = 0; i < numSamples; i++) samples[i] = normal.Sample();

            Assert.GreaterOrEqual(samples.Count(x => x >= lower), 925);
            Assert.GreaterOrEqual(samples.Count(x => x <= upper), 925);
        }
    }
}
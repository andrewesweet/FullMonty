using System.Linq;
using FullMonty.AddIn.Distributions;
using MathNet.Numerics.Statistics;
using NUnit.Framework;

namespace FullMonty.UnitTests.Distributions
{
    public class DiscreteUniformDistributionTest
    {
        [Test]
        public void ShouldBeSound()
        {
            var uniform = DiscreteUniformDistribution.FromContinuousBounds(1.1, 4.9);
            Assert.AreEqual(1, uniform.Min);
            Assert.AreEqual(5, uniform.Max);

            const int numSamples = 1000;
            var samples = new double[numSamples];
            for (var i = 0; i < numSamples; i++) samples[i] = uniform.Sample();

            var minSample = samples.Min();
            Assert.AreEqual(minSample, 1.0);

            var maxSample = samples.Max();
            Assert.AreEqual(maxSample, 5.0);

            Assert.AreEqual(3.0, uniform.Median);
            Assert.AreEqual(3.0, samples.Median());
        }
    }
}
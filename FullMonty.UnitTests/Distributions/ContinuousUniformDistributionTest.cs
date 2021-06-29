using System.Linq;
using FullMonty.AddIn.Distributions;
using MathNet.Numerics.Statistics;
using NUnit.Framework;

namespace FullMonty.UnitTests.Distributions
{
    public class ContinuousUniformDistributionTest
    {
        [Test]
        public void ShouldBeSound()
        {
            var uniform = new ContinuousUniformDistribution(1.1, 4.9);
            Assert.AreEqual(1.1, uniform.Min);
            Assert.AreEqual(4.9, uniform.Max);

            const int numSamples = 1000;
            var samples = new double[numSamples];
            for (var i = 0; i < numSamples; i++) samples[i] = uniform.Sample();

            Assert.That(samples.All(x => x >= 1.1), "Some samples were smaller than the expected minimum");

            Assert.That(samples.All(x => x <= 4.9), "Some samples were larger than the expected maximum");

            Assert.AreEqual(3.0, uniform.Median);
            Assert.AreEqual(3.0, samples.Median(), 0.1);
        }
    }
}
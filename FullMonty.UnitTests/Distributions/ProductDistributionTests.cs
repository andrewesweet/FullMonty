using System;
using System.Collections.Generic;
using System.Linq;
using FullMonty.AddIn;
using FullMonty.AddIn.Distributions;
using NUnit.Framework;

namespace FullMonty.UnitTests.Distributions
{
    public class ProductDistributionTests
    {
        [Test]
        public void ShouldBeADistribution()
        {
            Assert.That(
                new ProductDistribution(new List<IDistribution>()) is IDistribution,
                "IDistribution is not implemented");
        }

        [Test]
        public void CanSample()
        {
            var distribution = new ProductDistribution(
                new List<IDistribution>
                {
                    DiscreteUniformDistribution.FromContinuousBounds(1, 1),
                    DiscreteUniformDistribution.FromContinuousBounds(2, 2),
                    DiscreteUniformDistribution.FromContinuousBounds(3, 3)
                });

            Assert.AreEqual(6.0, distribution.Sample());
        }

        [Test]
        public void CanSampleSpan()
        {
            var distribution = new ProductDistribution(
                new List<IDistribution>
                {
                    DiscreteUniformDistribution.FromContinuousBounds(1.0, 1.0),
                    DiscreteUniformDistribution.FromContinuousBounds(2.0, 2.0),
                    DiscreteUniformDistribution.FromContinuousBounds(3.0, 3.0)
                });

            var samples = new double[100];
            distribution.Sample(samples);

            Assert.That(samples.All(x => Math.Abs(x - 6.0) < double.Epsilon),
                "Some samples deviated from the expectation");
        }
    }
}
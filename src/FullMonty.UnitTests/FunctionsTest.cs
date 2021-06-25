using FullMonty.AddIn;
using FullMonty.AddIn.Distributions;
using NUnit.Framework;

namespace FullMonty.UnitTests
{
    public class FunctionsTest
    {
        private const string Name = "Dave";

        [Test]
        public void ShouldCreateSampledDistribution()
        {
            var samples = new object[] {1.0, 2.0};
            var handle = Functions.CreateSampledDistribution(samples);
            AssertIsValidHandle(handle);
            CollectionAssert.AreEquivalent(
                samples,
                Functions.HandleManager[handle]
                    .GetPayloadOrThrow<SampleDistribution>().Samples);
        }

        [Test]
        public void ShouldCreateNamedSampledDistribution()
        {
            var samples = new object[] {1.0, 2.0};
            var handle = Functions.CreateNamedSampledDistribution(Name, samples);
            Assert.AreEqual(Name, handle);
            CollectionAssert.AreEquivalent(
                samples,
                Functions.HandleManager[handle]
                    .GetPayloadOrThrow<SampleDistribution>().Samples);
        }

        [Test]
        public void ShouldCreateBetaDistribution()
        {
            const double min = 1.0;
            const double max = 3.0;
            var handle = Functions.CreateBetaDistribution(min, max, 2.0);
            AssertIsValidHandle(handle);
            AssertIsExpectedBetaDistribution(handle, min, max);
        }

        [Test]
        public void ShouldCreateNamedBetaDistribution()
        {
            const double min = 1.0;
            const double max = 3.0;
            var handle = Functions.CreateNamedBetaDistribution(Name, min, max, 2.0);
            Assert.AreEqual(Name, handle);
            AssertIsExpectedBetaDistribution(handle, min, max);
        }

        [Test]
        public void ShouldCreateNormalDistribution()
        {
            const double lower = 1.0;
            const double upper = 2.0;
            var handle = Functions.CreateNormalDistribution(lower, upper);
            AssertIsValidHandle(handle);
            AssertIsExpectedNormalDistribution(handle, lower, upper);
        }

        [Test]
        public void ShouldCreateNamedNormalDistribution()
        {
            const double lower = 1.0;
            const double upper = 2.0;
            var handle = Functions.CreateNamedNormalDistribution(Name, lower, upper);
            Assert.AreEqual(Name, handle);
            AssertIsExpectedNormalDistribution(handle, lower, upper);
        }

        [Test]
        public void ShouldCreateUniformDistribution()
        {
            const double min = 1.0;
            const double max = 3.0;
            var handle = Functions.CreateUniformDistribution(min, max);
            AssertIsValidHandle(handle);
            AssertIsExpectedUniformDistribution(handle, min, max);
        }

        [Test]
        public void ShouldCreateNamedUniformDistribution()
        {
            const double min = 1.0;
            const double max = 3.0;
            var handle = Functions.CreateNamedUniformDistribution(Name, min, max);
            Assert.AreEqual(Name, handle);
            AssertIsExpectedUniformDistribution(handle, min, max);
        }

        [Test]
        public void ShouldSample()
        {
            var distributionHandle = Functions.CreateSampledDistribution(new object[] {1.0});
            var sample = Functions.Sample(distributionHandle);
            Assert.AreEqual(1.0, sample);
        }

        [Test]
        public void ShouldTakeSamples()
        {
            var distributionHandle = Functions.CreateSampledDistribution(new object[] { 1.0, 2.0 });
            const double n = 42.0;
            var handle = Functions.TakeSamples(distributionHandle, n);
            AssertIsValidHandle(handle);
            var samples = Functions.HandleManager[handle].GetPayloadOrThrow<Samples>();
            Assert.AreEqual(n, samples.NumberOfSamples);

            for (int i = 0; i < n; i++)
            {
                Assert.That(samples[i] == 1.0 || samples[i] == 2.0, "Sampled value is not consistent with distribution");
            }
        }

        [Test]
        public void ShouldSumSamples()
        {
            var distributionHandle = Functions.CreateSampledDistribution(new object[] { 1.0 });
            const double n = 42.0;
            var samplesHandle = Functions.TakeSamples(distributionHandle, n);
            var sum = Functions.SumSamples(samplesHandle);
            Assert.AreEqual(n, sum);
        }

        [Test]
        public void ShouldDisplaySamples()
        {
            var distributionHandle = Functions.CreateSampledDistribution(new object[] { 1.0 });
            var samplesHandle = Functions.TakeSamples(distributionHandle, 5);
            var result = Functions.DisplayObj(samplesHandle);
            Assert.IsInstanceOf<double[]>(result);
            CollectionAssert.AreEquivalent(new[] { 1.0, 1.0, 1.0, 1.0, 1.0 }, (double[])result);
        }

        private static void AssertIsValidHandle(string handle)
        {
            Assert.IsNotNull(handle);
            Assert.IsNotEmpty(handle);
        }

        private static void AssertIsExpectedBetaDistribution(string handle, double min, double max)
        {
            var distribution = Functions.HandleManager[handle]
                .GetPayloadOrThrow<BetaDistribution>();
            Assert.AreEqual(min, distribution.Min);
            Assert.AreEqual(max, distribution.Max);
        }

        private static void AssertIsExpectedNormalDistribution(string handle, double lower, double upper)
        {
            var distribution = Functions.HandleManager[handle]
                .GetPayloadOrThrow<NormalDistribution>();
            Assert.AreEqual((lower + upper) / 2.0, distribution.Mean);
        }

        private static void AssertIsExpectedUniformDistribution(string handle, double min, double max)
        {
            var distribution = Functions.HandleManager[handle]
                .GetPayloadOrThrow<UniformDistribution>();
            Assert.AreEqual(min, distribution.Min);
            Assert.AreEqual(max, distribution.Max);
        }
    }
}
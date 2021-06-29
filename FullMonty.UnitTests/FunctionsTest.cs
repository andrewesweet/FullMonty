using System.Collections.Generic;
using System.Linq;
using ExcelDna.Integration;
using FullMonty.AddIn;
using FullMonty.AddIn.Distributions;
using NUnit.Framework;

namespace FullMonty.UnitTests
{
    public class FunctionsTest
    {
        private const string Name = "Dave";

        private static IEnumerable<TestCaseData> NullNameProvider()
        {
            yield return new TestCaseData(null);
            yield return new TestCaseData(string.Empty);
            yield return new TestCaseData(" \t\r\n");
        }

        [Test]
        [TestCaseSource(nameof(NullNameProvider))]
        public void ShouldCreateSampledDistributionWhenNameNotSpecified(string name)
        {
            var handle = Functions.CreateSampledDistribution(name, new object[] {1.0, 2.0});
            AssertIsValidHandle(handle);
        }

        [Test]
        public void ShouldCreateNamedSampledDistribution()
        {
            var samples = new object[] {1.0, 2.0};
            var handle = Functions.CreateSampledDistribution(Name, samples);
            Assert.AreEqual(Name, handle);
            CollectionAssert.AreEquivalent(
                samples,
                Functions.HandleManager[handle]
                    .GetPayloadOrThrow<SampleDistribution>().Samples);
        }

        [Test]
        [TestCaseSource(nameof(NullNameProvider))]
        public void ShouldCreateBetaDistributionWhenNameNotSpecified(string name)
        {
            var handle = Functions.CreateBetaDistribution(name, 1.0, 3.0, 2.0);
            AssertIsValidHandle(handle);
        }

        [Test]
        public void ShouldCreateNamedBetaDistribution()
        {
            const double min = 1.0;
            const double max = 3.0;
            var handle = Functions.CreateBetaDistribution(Name, min, max, 2.0);
            Assert.AreEqual(Name, handle);
            AssertIsExpectedBetaDistribution(handle, min, max);
        }

        [Test]
        [TestCaseSource(nameof(NullNameProvider))]
        public void ShouldCreateNormalDistributionWhenNameNotSpecified(string name)
        {
            var handle = Functions.CreateNormalDistribution(name, 1.0, 2.0);
            AssertIsValidHandle(handle);
        }

        [Test]
        public void ShouldCreateNamedNormalDistribution()
        {
            const double lower = 1.0;
            const double upper = 2.0;
            var handle = Functions.CreateNormalDistribution(Name, lower, upper);
            Assert.AreEqual(Name, handle);
            AssertIsExpectedNormalDistribution(handle, lower, upper);
        }

        [Test]
        [TestCaseSource(nameof(NullNameProvider))]
        public void ShouldCreateDiscreteUniformDistributionWhenNameNotSpecified(string name)
        {
            var handle = Functions.CreateDiscreteUniformDistribution(name, 1.0, 3.0);
            AssertIsValidHandle(handle);
        }

        [Test]
        public void ShouldCreateNamedDiscreteUniformDistribution()
        {
            const double min = 1.0;
            const double max = 3.0;
            var handle = Functions.CreateDiscreteUniformDistribution(Name, min, max);
            Assert.AreEqual(Name, handle);
            AssertIsExpectedDiscreteUniformDistribution(handle, min, max);
        }

        [Test]
        [TestCaseSource(nameof(NullNameProvider))]
        public void ShouldCreateContinuousUniformDistributionWhenNameNotSpecified(string name)
        {
            var handle = Functions.CreateContinuousUniformDistribution(name, 1.0, 3.0);
            AssertIsValidHandle(handle);
        }

        [Test]
        public void ShouldCreateNamedContinuousUniformDistribution()
        {
            const double min = 1.0;
            const double max = 3.0;
            var handle = Functions.CreateContinuousUniformDistribution(Name, min, max);
            Assert.AreEqual(Name, handle);
            AssertIsExpectedContinuousUniformDistribution(handle, min, max);
        }

        [Test]
        public void ShouldExportCreateProductDistributionAsExcelFunction()
        {
            var method = typeof(Functions).GetMethod(nameof(Functions.CreateProductDistribution));
            Assert.IsNotNull(method, $"There is not method named {nameof(Functions.CreateProductDistribution)}");
            Assert.IsTrue(
                method.GetCustomAttributes(false).Any(x => x is ExcelFunctionAttribute),
                $"{nameof(Functions.CreateProductDistribution)} is not marked with {nameof(ExcelFunctionAttribute)}");
        }

        [Test]
        public void ShouldCreateProductDistribution()
        {
            var distributionOne = Functions.CreateContinuousUniformDistribution(null, 1.0, 1.0);
            var distributionTwo = Functions.CreateContinuousUniformDistribution(null, 2.0, 2.0);
            var distributionThree = Functions.CreateContinuousUniformDistribution(null, 3.0, 3.0);

            var productDistribution =
                Functions.CreateProductDistribution(null, distributionOne, distributionTwo, distributionThree);

            Assert.IsInstanceOf<ProductDistribution>(Functions.HandleManager[productDistribution].Payload);
            CollectionAssert.AreEquivalent(
                new[] {distributionOne, distributionTwo, distributionThree}.Select(x =>
                    Functions.HandleManager[x].Payload),
                Functions.HandleManager[productDistribution].GetPayloadOrThrow<ProductDistribution>().Distributions);
        }

        [Test]
        public void ShouldSample()
        {
            var distributionHandle = Functions.CreateSampledDistribution(null, new object[] {1.0});
            var sample = Functions.Sample(distributionHandle);
            Assert.AreEqual(1.0, sample);
        }

        [Test]
        public void ShouldTakeSamples()
        {
            var distributionHandle = Functions.CreateSampledDistribution(null, new object[] {1.0, 2.0});
            const double n = 42.0;
            var handle = Functions.TakeSamples(distributionHandle, n);
            AssertIsValidHandle(handle);
            var samples = Functions.HandleManager[handle].GetPayloadOrThrow<Samples>();
            Assert.AreEqual(n, samples.NumberOfSamples);

            for (var i = 0; i < n; i++)
                // ReSharper disable twice CompareOfFloatsByEqualityOperator
                Assert.That(samples[i] == 1.0 || samples[i] == 2.0,
                    "Sampled value is not consistent with distribution");
        }

        [Test]
        public void ShouldSumSamples()
        {
            var distributionHandle = Functions.CreateSampledDistribution(null, new object[] {1.0});
            const double n = 42.0;
            var samplesHandle = Functions.TakeSamples(distributionHandle, n);
            var sum = Functions.SumSamples(samplesHandle);
            Assert.AreEqual(n, sum);
        }

        [Test]
        public void ShouldDisplaySamples()
        {
            var distributionHandle = Functions.CreateSampledDistribution(null, new object[] {1.0});
            var samplesHandle = Functions.TakeSamples(distributionHandle, 5);
            var result = Functions.DisplayObj(samplesHandle);
            Assert.IsInstanceOf<double[]>(result);
            CollectionAssert.AreEquivalent(new[] {1.0, 1.0, 1.0, 1.0, 1.0}, (double[]) result);
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

        private static void AssertIsExpectedDiscreteUniformDistribution(string handle, double min, double max)
        {
            var distribution = Functions.HandleManager[handle]
                .GetPayloadOrThrow<DiscreteUniformDistribution>();
            Assert.AreEqual(min, distribution.Min);
            Assert.AreEqual(max, distribution.Max);
        }

        private static void AssertIsExpectedContinuousUniformDistribution(string handle, double min, double max)
        {
            var distribution = Functions.HandleManager[handle]
                .GetPayloadOrThrow<ContinuousUniformDistribution>();
            Assert.AreEqual(min, distribution.Min);
            Assert.AreEqual(max, distribution.Max);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using ExcelDna.Integration;
using FullMonty.AddIn.Distributions;

namespace FullMonty.AddIn
{
    public static class Functions
    {
        public static HandleManager HandleManager { get; } = new HandleManager();

        private static string Wrap(Func<string> f)
        {
            try
            {
                return f();
            }
            catch (Exception e)
            {
                return "ERROR: " + e.Message;
            }
        }

        private static object Wrap(Func<object> f)
        {
            try
            {
                return f();
            }
            catch (Exception e)
            {
                return "ERROR: " + e.Message;
            }
        }

        private static string CreateName()
        {
            return Guid.NewGuid().ToString();
        }

        private static IList<double> SanitiseSamples(object[] samples)
        {
            return samples.OfType<double>().ToList();
        }

        [ExcelFunction("Gets the underlying type of a handle")]
        public static string GetType([ExcelArgument("The handle name")] string name)
        {
            return Wrap(() => HandleManager[name].Payload.GetType().Name);
        }

        [ExcelFunction("Creates a named distribution based on a list of samples")]
        public static string CreateSampledDistribution(
            [ExcelArgument("The handle name for the distribution")]
            string name,
            [ExcelArgument("A list samples. Nulls and blanks will be ignored.", Name = "samples")]
            object[] samples
        )
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                name = CreateName();
            }

            return Wrap(() => HandleManager.Register(new SampleDistribution(SanitiseSamples(samples)), name).Name);
        }

        [ExcelFunction(
            "Creates a named beta distribution characterised by a floor value, a ceiling value and a modal value, as defined by Hubbard in How to Measure Anything: Finding the Value of Intangibles in Business, 3rd Edition."
        )]
        public static string CreateBetaDistribution(
            [ExcelArgument("The handle name for the distribution")]
            string name,
            [ExcelArgument("The smallest possible value of the distribution")]
            double min,
            [ExcelArgument("The largest possible value of the distribution")]
            double max,
            [ExcelArgument("The most likely value of the distribution")]
            double mode
        )
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                name = CreateName();
            }

            return Wrap(() => HandleManager.Register(BetaDistribution.FromMode(min, max, mode), name).Name);
        }

        [ExcelFunction(
            "Creates a named normal distribution calibrated such that P(lower <= sample <= upper) is 90%, with P(sample < lower) and P(sample > upper) both being 5%."
        )]
        public static string CreateNormalDistribution(
            [ExcelArgument("The handle name for the distribution")]
            string name,
            [ExcelArgument("The lower bound")] double lower,
            [ExcelArgument("The upper bound")] double upper
        )
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                name = CreateName();
            }

            return Wrap(
                () => HandleManager.Register(
                        NormalDistribution.FromNinetyPercentConfidenceIntervalBounds(lower, upper),
                        name
                    )
                    .Name
            );
        }

        [ExcelFunction(
            "Creates a named discrete uniform distribution with the given minimum and maximum values. If these values are not integers, the floor and ceiling respectively are taken."
        )]
        public static string CreateDiscreteUniformDistribution(
            [ExcelArgument("The handle name for the distribution")]
            string name,
            [ExcelArgument("The smallest possible value of the distribution")]
            double min,
            [ExcelArgument("The largest possible value of the distribution")]
            double max
        )
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                name = CreateName();
            }

            return Wrap(() => HandleManager.Register(DiscreteUniformDistribution.FromContinuousBounds(min, max), name).Name);
        }

        [ExcelFunction(
            "Creates a named continuous uniform distribution with the given minimum and maximum values. If these values are not integers, the floor and ceiling respectively are taken."
        )]
        public static string CreateContinuousUniformDistribution(
            [ExcelArgument("The handle name for the distribution")]
            string name,
            [ExcelArgument("The smallest possible value of the distribution")]
            double min,
            [ExcelArgument("The largest possible value of the distribution")]
            double max
        )
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                name = CreateName();
            }

            return Wrap(() => HandleManager.Register(new ContinuousUniformDistribution(min, max), name).Name);
        }

        [ExcelFunction("Takes a sample from a distribution", IsVolatile = true)]
        public static object Sample(
            [ExcelArgument("The handle of the distribution to be sampled")]
            string distribution
        )
        {
            return Wrap(() => HandleManager[distribution].GetPayloadOrThrow<IDistribution>("distribution").Sample());
        }

        [ExcelFunction("Takes N samples from a distribution", IsVolatile = true)]
        public static string TakeSamples(
            [ExcelArgument("The handle of the distribution to be sampled")]
            string distribution,
            [ExcelArgument("The number of samples to take from the distribution")]
            double n
        )
        {
            return Wrap(() =>
            {
                var numberOfSamples = Convert.ToInt32(Math.Floor(n));
                var dist = HandleManager[distribution].GetPayloadOrThrow<IDistribution>();
                return HandleManager.Register(new Samples(numberOfSamples, dist)).Name;
            });
        }

        [ExcelFunction("Sums a set of samples")]
        public static object SumSamples(
            [ExcelArgument("The handle of the sample set to be summed")]
            string samples)
        {
            return Wrap(() => HandleManager[samples].GetPayloadOrThrow<Samples>().Sum());
        }

        [ExcelFunction("Renders a FullMonty object")]
        public static object DisplayObj(
            [ExcelArgument("The handle of the object to be displayed")]
            string handle)
        {
            return Wrap(() => HandleManager[handle].GetPayloadOrThrow<Samples>().Display());
        }
    }
}
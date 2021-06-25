using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
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
        public static string CreateNamedSampledDistribution(
            [ExcelArgument("The handle name for the distribution")]
            string name,
            [ExcelArgument("A list samples. Nulls and blanks will be ignored.", Name = "samples")]
            object[] samples
        )
        {
            return Wrap(() => HandleManager.Register(new SampleDistribution(SanitiseSamples(samples)), name).Name);
        }

        [ExcelFunction("Creates a distribution based on a list of samples")]
        public static string CreateSampledDistribution(
            [ExcelArgument("A list samples. Nulls and blanks will be ignored.", Name = "samples")]
            object[] samples
        )
        {
            return Wrap(() => CreateNamedSampledDistribution(CreateName(), samples));
        }

        [ExcelFunction(
            "Creates a beta distribution characterised by a floor value, a ceiling value and a modal value, as defined by Hubbard in How to Measure Anything: Finding the Value of Intangibles in Business, 3rd Edition."
        )]
        public static string CreateBetaDistribution(
            [ExcelArgument("The smallest possible value of the distribution")]
            double min,
            [ExcelArgument("The largest possible value of the distribution")]
            double max,
            [ExcelArgument("The most likely value of the distribution")]
            double mode
        )
        {
            return Wrap(() => CreateNamedBetaDistribution(CreateName(), min, max, mode));
        }

        [ExcelFunction(
            "Creates a named beta distribution characterised by a floor value, a ceiling value and a modal value, as defined by Hubbard in How to Measure Anything: Finding the Value of Intangibles in Business, 3rd Edition."
        )]
        public static string CreateNamedBetaDistribution(
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
            return Wrap(() => HandleManager.Register(BetaDistribution.FromMode(min, max, mode), name).Name);
        }

        [ExcelFunction(
            "Creates a normal distribution calibrated such that P(lower <= sample <= upper) is 90%, with P(sample < lower) and P(sample > upper) both being 5%."
        )]
        public static string CreateNormalDistribution(
            [ExcelArgument("The lower bound")] double lower,
            [ExcelArgument("The upper bound")] double upper
        )
        {
            return Wrap(() => CreateNamedNormalDistribution(CreateName(), lower, upper));
        }

        [ExcelFunction(
            "Creates a named normal distribution calibrated such that P(lower <= sample <= upper) is 90%, with P(sample < lower) and P(sample > upper) both being 5%."
        )]
        public static string CreateNamedNormalDistribution(
            [ExcelArgument("The handle name for the distribution")]
            string name,
            [ExcelArgument("The lower bound")] double lower,
            [ExcelArgument("The upper bound")] double upper
        )
        {
            return Wrap(
                () => HandleManager.Register(
                        NormalDistribution.FromNinetyPercentConfidenceIntervalBounds(lower, upper),
                        name
                    )
                    .Name
            );
        }

        [ExcelFunction(
            "Creates a discrete uniform distribution with the given minimum and maximum values. If these values are not integers, the floor and ceiling respectively are taken."
        )]
        public static string CreateUniformDistribution(
            [ExcelArgument("The smallest possible value of the distribution")]
            double min,
            [ExcelArgument("The largest possible value of the distribution")]
            double max
        )
        {
            return Wrap(() => CreateNamedUniformDistribution(CreateName(), min, max));
        }

        [ExcelFunction(
            "Creates a named discrete uniform distribution with the given minimum and maximum values. If these values are not integers, the floor and ceiling respectively are taken."
        )]
        public static string CreateNamedUniformDistribution(
            [ExcelArgument("The handle name for the distribution")]
            string name,
            [ExcelArgument("The smallest possible value of the distribution")]
            double min,
            [ExcelArgument("The largest possible value of the distribution")]
            double max
        )
        {
            return Wrap(() => HandleManager.Register(UniformDistribution.FromContinuousBounds(min, max), name).Name);
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
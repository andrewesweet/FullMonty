using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FullMonty.AddIn.Distributions
{
    public class ProductDistribution : IDistribution
    {
        public ProductDistribution(IList<IDistribution> distributions)
        {
            Distributions = distributions;
        }

        public IList<IDistribution> Distributions { get; }

        public double Sample()
        {
            return Distributions.Aggregate(1.0, (acc, dist) => acc * dist.Sample());
        }

        public void Sample(Span<double> samples)
        {
            double[][] distributionSamples = new double[Distributions.Count][];
            for (int i = 0; i < Distributions.Count; i++)
            {
                distributionSamples[i] = ArrayPool<double>.Shared.Rent(samples.Length);
                Distributions[i].Sample(distributionSamples[i]);
            }

            for (int i = 0; i < samples.Length; i++)
            {
                samples[i] = 1.0;
                for (int j = 0; j < Distributions.Count; j++)
                {
                    samples[i] *= distributionSamples[j][i];
                }
            }

            for (int i = 0; i < Distributions.Count; i++)
            {
                ArrayPool<double>.Shared.Return(distributionSamples[i]);
            }
        }
    }
}
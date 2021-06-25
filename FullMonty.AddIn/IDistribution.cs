using System;

namespace FullMonty.AddIn
{
    public interface IDistribution
    {
        double Median { get; }

        double Sample();

        void Sample(Span<double> samples);
    }
}
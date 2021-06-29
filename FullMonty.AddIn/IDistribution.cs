using System;

namespace FullMonty.AddIn
{
    public interface IDistribution
    {
        double Sample();

        void Sample(Span<double> samples);
    }
}
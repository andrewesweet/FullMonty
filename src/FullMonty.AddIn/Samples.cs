using System.Linq;

namespace FullMonty.AddIn
{
    public class Samples
    {
        private readonly double[] samples;

        public Samples(int numberOfSamples, IDistribution distribution)
        {
            samples = new double[numberOfSamples];
            distribution.Sample(samples);
        }

        public int NumberOfSamples => samples.Length;

        public double this[int i] => samples[i];

        public double Sum()
        {
            return samples.Sum();
        }

        public object Display()
        {
            return samples;
        }
    }
}

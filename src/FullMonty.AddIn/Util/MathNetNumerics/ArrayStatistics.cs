using System;

namespace FullMonty.AddIn.Util.MathNetNumerics
{
    public static class ArrayStatistics
    {
        /// <summary>
        ///     Returns the smallest value from the unsorted data array.
        ///     Returns NaN if data is empty or any entry is NaN.
        /// </summary>
        /// <param name="data">Contiguous sample data, no sorting is assumed.</param>
        public static double Minimum(ReadOnlySpan<double> data)
        {
            if (data.Length == 0) return double.NaN;

            var min = double.PositiveInfinity;
            for (var i = 0; i < data.Length; i++)
                if (data[i] < min || double.IsNaN(data[i]))
                    min = data[i];

            return min;
        }

        /// <summary>
        ///     Returns the largest value from the unsorted data array.
        ///     Returns NaN if data is empty or any entry is NaN.
        /// </summary>
        /// <param name="data">Contiguous sample data, no sorting is assumed.</param>
        public static double Maximum(ReadOnlySpan<double> data)
        {
            if (data.Length == 0) return double.NaN;

            var max = double.NegativeInfinity;
            for (var i = 0; i < data.Length; i++)
                if (data[i] > max || double.IsNaN(data[i]))
                    max = data[i];

            return max;
        }

        /// <summary>
        ///     Estimates the tau-th quantile (Excel-style) from the unsorted data array.
        ///     The tau-th quantile is the data value where the cumulative distribution
        ///     function crosses tau. The quantile definition can be specified to be compatible
        ///     with an existing system.
        ///     WARNING: Works inplace and can thus causes the data array to be reordered.
        /// </summary>
        /// <param name="data">Contiguous sample data, no sorting is assumed. Will be reordered.</param>
        /// <param name="tau">Quantile selector, between 0.0 and 1.0 (inclusive)</param>
        public static double QuantileCustomInplace(Span<double> data, double tau)
        {
            if (tau < 0d || tau > 1d || data.Length == 0) return double.NaN;

            if (tau == 0d || data.Length == 1) return Minimum(data);

            if (tau == 1d) return Maximum(data);

            var h = (data.Length - 1) * tau + 1d;
            var hf = (int) h;
            var lower = SelectInplace(data, hf - 1);
            var upper = SelectInplace(data, hf);
            return lower + (h - hf) * (upper - lower);
        }

        private static double SelectInplace(Span<double> workingData, int rank)
        {
            // Numerical Recipes: select
            // http://en.wikipedia.org/wiki/Selection_algorithm
            if (rank <= 0) return Minimum(workingData);

            if (rank >= workingData.Length - 1) return Maximum(workingData);

            var a = workingData;
            var low = 0;
            var high = a.Length - 1;

            while (true)
            {
                if (high <= low + 1)
                {
                    if (high == low + 1 && a[high] < a[low])
                    {
                        var tmp = a[low];
                        a[low] = a[high];
                        a[high] = tmp;
                    }

                    return a[rank];
                }

                var middle = (low + high) >> 1;

                var tmp1 = a[middle];
                a[middle] = a[low + 1];
                a[low + 1] = tmp1;

                if (a[low] > a[high])
                {
                    var tmp = a[low];
                    a[low] = a[high];
                    a[high] = tmp;
                }

                if (a[low + 1] > a[high])
                {
                    var tmp = a[low + 1];
                    a[low + 1] = a[high];
                    a[high] = tmp;
                }

                if (a[low] > a[low + 1])
                {
                    var tmp = a[low];
                    a[low] = a[low + 1];
                    a[low + 1] = tmp;
                }

                var begin = low + 1;
                var end = high;
                var pivot = a[begin];

                while (true)
                {
                    do { begin++; }
                    while (a[begin] < pivot);

                    do { end--; }
                    while (a[end] > pivot);

                    if (end < begin) break;

                    var tmp = a[begin];
                    a[begin] = a[end];
                    a[end] = tmp;
                }

                a[low + 1] = a[end];
                a[end] = pivot;

                if (end >= rank) high = end - 1;

                if (end <= rank) low = begin;
            }
        }
    }
}
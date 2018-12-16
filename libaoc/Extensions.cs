using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using MoreLinq.Extensions;

namespace AoC
{
    public static class Extensions
    {
        public static IEnumerable<int> SelectInts(this string @this) => Regex
            .Matches(@this, @"\d+")
            .Select(m => int.Parse(m.Value));

        public static IEnumerable<IReadOnlyList<T>> BatchList<T>(this IEnumerable<T> @this, int batchSize) =>
            @this.Batch(batchSize).Select(b => b.ToList());
        
        public static IEnumerable<TR> Batch<T, TR>(this IEnumerable<T> @this, int batchSize, Func<IReadOnlyList<T>, TR> selector) =>
            BatchExtension.Batch(@this, batchSize, b => selector(b.ToList()));

        public static void Copy<T>(this IReadOnlyList<T> @this, int srcOffset, T[] dst, int dstOffset, int count)
        {
            for (var i = 0; i < count; ++i)
                dst[i + dstOffset] = @this[i + srcOffset];
        }
        
        public static T[] SliceArray<T>(this IReadOnlyList<T> @this, int offset, int count)
        {
            var sliced = new T[count];
            @this.Copy(offset, sliced, 0, count);
            return sliced;
        }
    }
}

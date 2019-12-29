using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Aoc2019
{
    public static partial class EnumerableExtensions
    {
        public static IEnumerable<(T value, int index)> WithIndices<T>([NotNull] this IEnumerable<T> @this) =>
            @this.Select((value, index) => (value, index));
    }
}

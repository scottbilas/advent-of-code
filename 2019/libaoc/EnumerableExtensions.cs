using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Aoc2019
{
    public static partial class EnumerableExtensions
    {
        public static IOrderedEnumerable<T> Ordered<T>([NotNull] this IEnumerable<T> @this)
            => @this.OrderBy(_ => _);
    }
}

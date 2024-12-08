using Combinatorics.Collections;

static partial class Extensions
{
    public static Combinations<T> Combinations<T>(this IEnumerable<T> @this, int desiredSetSize) =>
        new(@this, desiredSetSize);

    public static IEnumerable<IReadOnlyList<T>> Combinations<T>(this IEnumerable<T> @this, int minDesiredSetSize, int maxDesiredSetSize) => Enumerable
        .Range(minDesiredSetSize, maxDesiredSetSize - minDesiredSetSize + 1)
        .SelectMany(i => @this.Combinations(i));

    public static IEnumerable<(T a, T b)> Combinations2<T>(this IEnumerable<T> @this) =>
        @this.Combinations(2).Select(l => (l[0], l[1]));
}

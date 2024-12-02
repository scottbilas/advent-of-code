static class Extensions
{
    public static int Abs(this int @this) => Math.Abs(@this);

    public static IEnumerable<int> SelectInts(this string @this) => @this
        .RegexMatches(@"[-+]?\d+")
        .Select(m => int.Parse(m.Value));

    public static List<int> GetInts(this string @this) => @this.SelectInts().ToList();

    public static IEnumerable<T> WhereIndex<T>(this IEnumerable<T> @this, Func<int, bool> predicate) =>
        @this.Where((_, i) => predicate(i));

    public static IEnumerable<T> Stride<T>(this IEnumerable<T> @this, int step, int start = 0) =>
        @this.WhereIndex(i => i % step == start);

    public static IEnumerable<TResult> GroupJoin<TItem, TResult>(
        this IEnumerable<TItem> outer, IEnumerable<TItem> inner,
        Func<TItem, IEnumerable<TItem>, TResult> resultSelector) =>
            outer.GroupJoin(inner, k => k, k => k, resultSelector);

    public static IEnumerable<(T a, T b)> SelectWithPrev<T>(this IEnumerable<T> @this)
    {
        using var e = @this.GetEnumerator();
        if (!e.MoveNext()) yield break;

        var prev = e.Current;
        while (e.MoveNext())
        {
            yield return (prev, e.Current);
            prev = e.Current;
        }
    }
}

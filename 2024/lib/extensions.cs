using System.Numerics;
using System.Text.RegularExpressions;

static class Extensions
{
    // Math

    public static int Abs(this int @this) => Math.Abs(@this);

    public static T Multiply<T>(this IEnumerable<T> @this) where T : INumber<T> =>
        @this.Aggregate(T.One, (a, b) => a * b);

    // Enumerables

    public static IEnumerable<string> GroupValues(this Match @this) =>
        @this.Groups.Values.Select(g => g.Value);

    public static IEnumerable<int> Ints(this string @this) => @this
        .RegexMatches(@"[-+]?\d+")
        .Select(m => int.Parse(m.Value));
    public static IEnumerable<int> SelectWhereInts(this IEnumerable<string> @this) => @this
        .SelectWhere(v => (int.TryParse(v, out var i), i));

    public static List<int> GetInts(this string @this) => @this.Ints().ToList();

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

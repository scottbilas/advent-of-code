using System.Text.RegularExpressions;

static partial class Extensions
{
    // Math

    public static int Abs(this int @this) => Math.Abs(@this);

    public static int Int(this char @this) => @this switch
    {
        >= '0' and <= '9' => @this - '0',
        _ => throw new InvalidOperationException()
    };

    public static bool HasCoord(this Int2 @this, Int2 coord) =>
        coord >= Int2.Zero == true && coord < @this == true;

    // Misc Selection

    public static IEnumerable<(T1 a, T2 b)> Swapped<T1, T2>(this IEnumerable<(T2 x, T1 y)> @this) =>
        @this.Select(p => p.Swap());

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

    public static IEnumerable<LinkedListNode<T>> Nodes<T>(this LinkedList<T> @this)
    {
        for (var node = @this.First; node != null; node = node.Next)
            yield return node;
    }

    public static IEnumerable<LinkedListNode<T>> NodesReverse<T>(this LinkedList<T> @this)
    {
        for (var node = @this.Last; node != null; node = node.Previous)
            yield return node;
    }

    public static IEnumerable<T> Reverse<T>(this LinkedList<T> @this)
    {
        for (var node = @this.Last; node != null; node = node.Previous)
            yield return node.Value;
    }

    public static IEnumerable<IGrouping<T, T>> Grouped<T>(this IEnumerable<T> @this) =>
        @this.GroupBy(v => v);
    public static IEnumerable<(T key, int count)> SelectByCount<T>(this IEnumerable<T> @this) =>
        @this.Grouped().Select(g => (g.Key, g.Count()));

    // Item Parsing

    public static int Int(this string @this) => int.Parse(@this);
    public static long Long(this string @this) => long.Parse(@this);

    public static IEnumerable<int> Ints(this string @this) => @this
        .RegexMatches(@"[-+]?\d+")
        .Select(m => int.Parse(m.Value));
    public static IEnumerable<long> Longs(this string @this) => @this
        .RegexMatches(@"[-+]?\d+")
        .Select(m => long.Parse(m.Value));

    public static IEnumerable<int> Ints(this IEnumerable<string> @this) => @this
        .Select(int.Parse);
    public static IEnumerable<int> TryInts(this IEnumerable<string> @this) => @this
        .SelectWhere(v => (int.TryParse(v, out var i), i));
    public static IEnumerable<long> Longs(this IEnumerable<string> @this) => @this
        .Select(long.Parse);
    public static IEnumerable<long> TryLongs(this IEnumerable<string> @this) => @this
        .SelectWhere(v => (long.TryParse(v, out var i), i));

    public static List<int> GetInts(this string @this) => @this.Ints().ToList();
    public static List<long> GetLongs(this string @this) => @this.Longs().ToList();

    public static IEnumerable<int> Ints(this Match @this) =>
        @this.GroupValues().Ints();
    public static int Int(this Group @this) =>
        @this.Value.Int();
    public static int Int(this Match @this, int index = 0) =>
        @this.Groups[index+1].Int();
    public static IEnumerable<long> Longs(this Match @this) =>
        @this.GroupValues().Longs();
    public static long Long(this Group @this) =>
        @this.Value.Long();
    public static long Long(this Match @this, int index = 0) =>
        @this.Groups[index+1].Long();

    // Section Parsing

    public static IEnumerable<IReadOnlyList<string>> Sections(this string @this)
    {
        var section = new List<string>();
        foreach (var line in @this.SelectLines(true))
        {
            if (line != "")
                section.Add(line);
            else if (section.Any())
            {
                yield return section;
                section = [];
            }
        }

        if (section.Any())
            yield return section;
    }

    public static (TA a, TB b) Sections<TA, TB>(
        this string @this,
        Func<IReadOnlyList<string>, TA> selectA,
        Func<IReadOnlyList<string>, TB> selectB)
    {
        var (a, b) = @this.Sections().ToTuple2();
        return (selectA(a), selectB(b));
    }
}

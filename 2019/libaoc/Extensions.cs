using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using Combinatorics.Collections;
using JetBrains.Annotations;
using MoreLinq.Extensions;
using Unity.Coding.Utils;

namespace Aoc2019
{
    public static partial class Extensions
    {
        /// <summary>Segment an enumerable into batches of the given size</summary>
        public static IEnumerable<IReadOnlyList<T>> BatchList<T>([NotNull] this IEnumerable<T> @this, int batchSize) =>
            @this.Batch(batchSize).Select(b => b.ToList());
        /// <summary>Reduce an enumerable using an operator that takes batches of the given size and outputs one element per batch</summary>
        public static IEnumerable<TR> Batch<T, TR>([NotNull] this IEnumerable<T> @this, int batchSize, [NotNull] Func<IReadOnlyList<T>, TR> selector) =>
            BatchExtension.Batch(@this, batchSize, b => selector(b.ToList()));
        /// <summary>Stream enumerable batches into 2-tuples</summary>
        public static IEnumerable<(T a, T b)> Batch2<T>([NotNull] this IEnumerable<T> @this) =>
            @this.Batch(2).Select(b => b.First2());
        /// <summary>Stream enumerable batches into 3-tuples</summary>
        public static IEnumerable<(T a, T b, T c)> Batch3<T>([NotNull] this IEnumerable<T> @this) =>
            @this.Batch(3).Select(b => b.First3());
        /// <summary>Stream enumerable batches into 4-tuples</summary>
        public static IEnumerable<(T a, T b, T c, T d)> Batch4<T>([NotNull] this IEnumerable<T> @this) =>
            @this.Batch(4).Select(b => b.First4());

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

        public static int RemoveWhere<T>([NotNull] this List<T> @this, Func<T, bool> predicate)
        {
            var count = 0;

            @this.SetRange(@this.Where(item =>
            {
                if (!predicate(item))
                    return true;
                ++count;
                return false;
            }).ToList());

            return count;
        }

        public static int Count<T>(this IEnumerable<T> @this, T value) where T : IEquatable<T> =>
            @this.Count(v => v.Equals(value));

        public static IReadOnlyDictionary<T, int> DistinctWithCount<T>(this IEnumerable<T> @this)
        {
            var dict = new Dictionary<T, int>().ToAutoDictionary();
            foreach (var item in @this)
                ++dict[item];
            return dict;
        }

        public static Int2 GetDimensions<T>(this T[,] @this) =>
            new Int2(@this.GetLength(0), @this.GetLength(1));
        
        public static IEnumerable<string> ToLines<T>(this T[,] @this)
        {
            var size = @this.GetDimensions();
            
            var sb = new StringBuilder();
            for (var y = 0; y < size.Y; ++y)
            {
                sb.Clear();
                for (var x = 0; x < size.X; ++x)
                    sb.Append(@this[x, y]);
                yield return sb.ToString();
            }
        }

        public static string ToText<T>(this T[,] @this) =>
            string.Join("\n", @this.ToLines());

        public static int ParseInt([NotNull] this string @this) =>
            int.Parse(@this);

        public static IEnumerable<int> ParseInts([NotNull] this IEnumerable<string> @this) =>
            @this.Select(int.Parse);

        public static int Int([NotNull] this Match @this, int groupNum) =>
            @this.Groups[groupNum].Value.ParseInt();
        public static int Int([NotNull] this Match @this, string groupName) =>
            @this.Groups[groupName].Value.ParseInt();

        public static string Text([NotNull] this Match @this, int groupNum) =>
            @this.Groups[groupNum].Value;
        public static string Text([NotNull] this Match @this, string groupName) =>
            @this.Groups[groupName].Value;

        public static bool Success([NotNull] this Match @this, int groupNum) =>
            @this.Groups[groupNum].Success;
        public static bool Success([NotNull] this Match @this, string groupName) =>
            @this.Groups[groupName].Success;

        public static IEnumerable<(Int2 pos, T cell)> SelectCells<T>([NotNull] this T[,] @this, Int2 max)
        {
            for (var y = 0; y < max.Y; ++y)
                for (var x = 0; x < max.X; ++x)
                    yield return (new Int2(x, y), cell: @this[x, y]);
        }

        public static IEnumerable<(Int2 pos, T cell)> SelectCells<T>([NotNull] this T[,] @this) =>
            @this.SelectCells(@this.GetDimensions());
        
        public static IEnumerable<Int2> SelectCoords<T>(this T[,] @this)
        {
            var size = @this.GetDimensions();
            for (var y = 0; y < size.Y; ++y)
                for (var x = 0; x < size.X; ++x)
                    yield return new Int2(x, y);
        }

        public static IEnumerable<(Int2 pos, T cell)> SelectBorderCells<T>([NotNull] this T[,] @this, int borderWidth = 1) =>
            @this.SelectBorderCoords(borderWidth).Select(pos => (pos, @this[pos.X, pos.Y]));

        public static IEnumerable<(int x, int y, T cell)> SelectXy<T>([NotNull] this IEnumerable<(Int2 pos, T cell)> @this) =>
            @this.Select(c => (c.pos.X, c.pos.Y, c.cell));

        public static IEnumerable<Int2> SelectBorderCoords<T>([NotNull] this T[,] @this, int borderWidth = 1)
        {
            var (cx, cy) = @this.GetDimensions();

            for (var b = 0; b < borderWidth; ++b)
            {
                for (var x = 0; x < cx; ++x)
                {
                    yield return new Int2(x, b);
                    yield return new Int2(x, cy - b - 1);
                }
                for (var y = borderWidth; y < cy - borderWidth; ++y)
                {
                    yield return new Int2(b, y);
                    yield return new Int2(cx - b - 1, y);
                }
            }
        }

        public static TValue? GetValueOrNull<TKey, TValue>([NotNull] this IDictionary<TKey, TValue> @this, TKey key) where TValue : struct =>
            @this.TryGetValue(key, out var value) ? value : (TValue?)null;

        public static bool IsAsciiLetter(this char @this) => @this.IsAsciiLower() || @this.IsAsciiUpper();
        public static bool IsAsciiLower(this char @this) => @this >= 'a' && @this <= 'z';
        public static bool IsAsciiUpper(this char @this) => @this >= 'A' && @this <= 'Z';
        public static char ToAsciiLower(this char @this) => @this.IsAsciiUpper() ? (char)(@this - 'A' + 'a') : @this;
        public static char ToAsciiUpper(this char @this) => @this.IsAsciiLower() ? (char)(@this - 'a' + 'A') : @this;

        public static ValueTuple<T, T> First2<T>([NotNull] this IEnumerable<T> @this)
        {
            using (var e = @this.GetEnumerator())
            {
                e.MoveNext();
                var t0 = e.Current;
                e.MoveNext();
                var t1 = e.Current;

                return ValueTuple.Create(t0, t1);
            }
        }

        public static ValueTuple<T, T, T> First3<T>([NotNull] this IEnumerable<T> @this)
        {
            using (var e = @this.GetEnumerator())
            {
                e.MoveNext();
                var t0 = e.Current;
                e.MoveNext();
                var t1 = e.Current;
                e.MoveNext();
                var t2 = e.Current;

                return ValueTuple.Create(t0, t1, t2);
            }
        }

        public static ValueTuple<T, T, T, T> First4<T>([NotNull] this IEnumerable<T> @this)
        {
            using (var e = @this.GetEnumerator())
            {
                e.MoveNext();
                var t0 = e.Current;
                e.MoveNext();
                var t1 = e.Current;
                e.MoveNext();
                var t2 = e.Current;
                e.MoveNext();
                var t3 = e.Current;

                return ValueTuple.Create(t0, t1, t2, t3);
            }
        }

        public static T[,] FillNew<T>(this T[,] @this) where T : new() =>
            @this.Fill(_ => new T());

        public static T[,] Fill<T>(this T[,] @this, T value) =>
            @this.Fill(_ => value);

        public static T[,] Fill<T>(this T[,] @this, Func<Int2, T> generator)
        {
            foreach (var coord in @this.SelectCoords())
                @this[coord.X, coord.Y] = generator(coord);

            return @this;
        }

        public static Rectangle Bounds(this IEnumerable<Rectangle> @this)
        {
            var (l, t, r, b) = (int.MaxValue, int.MaxValue, int.MinValue, int.MinValue);  
            foreach (var rect in @this)
            {
                l = Math.Min(l, rect.Left);
                t = Math.Min(t, rect.Top);
                r = Math.Max(r, rect.Right);
                b = Math.Max(b, rect.Bottom);
            }

            return Rectangle.FromLTRB(l, t, r, b);
        }

        public static Point BottomRight(in this Rectangle @this) =>
            new Point(@this.Right, @this.Bottom);

        public static IEnumerable<T> OrderByReading<T>(this IEnumerable<T> @this, Func<T, Point> selector)
        {
            return
                from item in @this
                let pos = selector(item)
                orderby pos.Y, pos.X
                select item;
        }

        public static IEnumerable<Point> OrderByReading(this IEnumerable<Point> @this) =>
            @this.OrderByReading(_ => _);

        public static IEnumerable<Point> SelectAdjacent(this Point @this)
        {
            yield return new Point(@this.X, @this.Y - 1);
            yield return new Point(@this.X - 1, @this.Y);
            yield return new Point(@this.X + 1, @this.Y);
            yield return new Point(@this.X, @this.Y + 1);
        }

        public static IEnumerable<Int2> SelectAdjacent(this Int2 @this)
        {
            yield return new Int2(@this.X, @this.Y - 1);
            yield return new Int2(@this.X - 1, @this.Y);
            yield return new Int2(@this.X + 1, @this.Y);
            yield return new Int2(@this.X, @this.Y + 1);
        }

        public static IEnumerable<T> SelectAdjacent<T>(this Point @this, Func<Point, T> selector) =>
            @this.SelectAdjacent().Select(selector);

        public static IEnumerable<T> SelectAdjacent<T>(this Int2 @this, Func<Int2, T> selector) =>
            @this.SelectAdjacent().Select(selector);

        public static IEnumerable<Point> SelectAdjacentWithDiagonals(this Point @this)
        {
            yield return new Point(@this.X - 1, @this.Y - 1);
            yield return new Point(@this.X, @this.Y - 1);
            yield return new Point(@this.X + 1, @this.Y - 1);
            
            yield return new Point(@this.X - 1, @this.Y);
            yield return new Point(@this.X + 1, @this.Y);
            
            yield return new Point(@this.X - 1, @this.Y + 1);
            yield return new Point(@this.X, @this.Y + 1);
            yield return new Point(@this.X + 1, @this.Y + 1);
        }

        public static IEnumerable<Int2> SelectAdjacentWithDiagonals(this Int2 @this)
        {
            yield return new Int2(@this.X - 1, @this.Y - 1);
            yield return new Int2(@this.X, @this.Y - 1);
            yield return new Int2(@this.X + 1, @this.Y - 1);

            yield return new Int2(@this.X - 1, @this.Y);
            yield return new Int2(@this.X + 1, @this.Y);

            yield return new Int2(@this.X - 1, @this.Y + 1);
            yield return new Int2(@this.X, @this.Y + 1);
            yield return new Int2(@this.X + 1, @this.Y + 1);
        }

        public static IEnumerable<T> SelectAdjacentWithDiagonals<T>(this Point @this, Func<Point, T> selector) =>
            @this.SelectAdjacentWithDiagonals().Select(selector);

        public static IEnumerable<T> SelectAdjacentWithDiagonals<T>(this Int2 @this, Func<Int2, T> selector) =>
            @this.SelectAdjacentWithDiagonals().Select(selector);

        /// <summary>Convert a `Dictionary` to an `AutoDictionary` and assign it the given default-get delegate</summary>
        public static AutoDictionary<TKey, TValue> ToAutoDictionary<TKey, TValue>([NotNull] this IDictionary<TKey, TValue> @this, Func<TKey, TValue> getDefault) =>
            new AutoDictionary<TKey, TValue>(@this, getDefault);

        /// <summary>Convert a `Dictionary` to an `AutoDictionary` and assign it a default-get delegate that just returns `defaultValue`</summary>
        public static AutoDictionary<TKey, TValue> ToAutoDictionary<TKey, TValue>([NotNull] this IDictionary<TKey, TValue> @this, TValue defaultValue = default) =>
            new AutoDictionary<TKey, TValue>(@this, defaultValue);

        /// <summary>Rotate a grid - useful for Dump() in LINQPad to match x,y</summary>
        public static T[,] Transpose<T>([NotNull] this T[,] @this)
        {
            var (cx, cy) = @this.GetDimensions();
            var newGrid = new T[cy, cx];
            foreach (var ((x, y), cell) in @this.SelectCells())
                newGrid[y, x] = cell;

            return newGrid;
        }

        public static T[,] AddBorder<T>([NotNull] this T[,] @this, T borderValue, int borderWidth = 1)
        {
            var (cx, cy) = @this.GetDimensions();
            var newGrid = new T[cx + borderWidth * 2, cy + borderWidth * 2];
            foreach (var (x, y) in newGrid.SelectBorderCoords(borderWidth))
                newGrid[x, y] = borderValue;
            foreach (var (pos, c) in @this.SelectCells())
                newGrid[pos.X + borderWidth, pos.Y + borderWidth] = c;
            return newGrid;
        }

        public static Size ToSize(this Int2 @this) => new Size(@this.X, @this.Y);

        public static T GetAt<T>(this T[,] @this, in Int2 pos) => @this[pos.X, pos.Y];
        public static T SetAt<T>(this T[,] @this, in Int2 pos, T value) => @this[pos.X, pos.Y] = value;

        public static Bitmap ToBitmap<T>([NotNull] this T[,] @this, Func<T, Color> selector, int scaleFactor = 4)
        {
            var (cx, cy) = @this.GetDimensions();
            var bitmap = new Bitmap(cx, cy);
            foreach (var (x, y, c) in @this.SelectCells().SelectXy())
                bitmap.SetPixel(x, y, selector(c));

            if (scaleFactor != 1)
            {
                var scaled = new Bitmap(cx * scaleFactor, cy * scaleFactor);

                using var graphics = Graphics.FromImage(scaled);
                graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
                graphics.DrawImage(bitmap, 0, 0, scaled.Width, scaled.Height);

                bitmap.Dispose();
                bitmap = scaled;
            }

            return bitmap;
        }

        public static Int2 ReduceFraction(this Int2 @this)
        {
            if (@this.X == 0)
            {
                if (@this.Y != 0)
                    @this.Y = @this.Y > 0 ? 1 : -1;
            }
            else if (@this.Y == 0)
                @this.X = @this.X > 0 ? 1 : -1;
            else
            {
                var gcd = Utils.Gcd(@this.X, @this.Y);
                @this.X /= gcd;
                @this.Y /= gcd;
            }

            return @this;
        }

        public static TTo Translate<TFrom, TTo>([NotNull] this TFrom @this, params (TFrom, TTo)[] map) =>
            map.Single(v => v.Item1.Equals(@this)).Item2;

        public static T PatternSeekingGetItemAt<T>([NotNull] this IEnumerable<T> @this, int index, int minRepeat = 10)
        {
            // TODO: use proper deque
            var history = new Dictionary<T, List<int>>().ToAutoDictionary(_ => new List<int>());

            var i = 0;
            foreach (var item in @this)
            {
                // no pattern yet, but we hit the index anyway
                if (i == index)
                    return item;

                var list = history[item];
                list.Add(i);

                if (list.Count >= minRepeat)
                {
                    // $$$$ don't use list[] or Equals(), work with indices and deltas..
                    var ok = true;
                    var match = list[list.Count - 1];
                    for (var repeat = 1; repeat < minRepeat; ++repeat)
                    {
                        if (!Equals(list[list.Count - 1 - repeat], match))
                        {
                            ok = false;
                            break;
                        }
                    }

                    if (ok)
                    {
                        var delta = list[list.Count - 1] - list[list.Count - 2];
                        if ((index - i) % delta == 0)
                            return item;
                    }
                }

                ++i;
            }

            throw new IndexOutOfRangeException();
        }
    }

    public static class MiscStatics
    {
        public static void With<T0>(T0 v0, Action<T0> action) => action(v0);
        public static void With<T0, T1>(T0 v0, T1 v1, Action<T0, T1> action) => action(v0, v1);
        public static void With<T0, T1, T2>(T0 v0, T1 v1, T2 v2, Action<T0, T1, T2> action) => action(v0, v1, v2);
        public static void With<T0, T1, T2, T3>(T0 v0, T1 v1, T2 v2, T3 v3, Action<T0, T1, T2, T3> action) => action(v0, v1, v2, v3);
        public static void With<T0, T1, T2, T3, T4>(T0 v0, T1 v1, T2 v2, T3 v3, T4 v4, Action<T0, T1, T2, T3, T4> action) => action(v0, v1, v2, v3, v4);

        public static TR With<T0, TR>(T0 v0, Func<T0, TR> action) => action(v0);
        public static TR With<T0, T1, TR>(T0 v0, T1 v1, Func<T0, T1, TR> action) => action(v0, v1);
        public static TR With<T0, T1, T2, TR>(T0 v0, T1 v1, T2 v2, Func<T0, T1, T2, TR> action) => action(v0, v1, v2);
        public static TR With<T0, T1, T2, T3, TR>(T0 v0, T1 v1, T2 v2, T3 v3, Func<T0, T1, T2, T3, TR> action) => action(v0, v1, v2, v3);
        public static TR With<T0, T1, T2, T3, T4, TR>(T0 v0, T1 v1, T2 v2, T3 v3, T4 v4, Func<T0, T1, T2, T3, T4, TR> action) => action(v0, v1, v2, v3, v4);

        public static T[] Arr<T>(params T[] items) => items;
        public static BigInteger[] BArr(params BigInteger[] items) => items;

        public static IEnumerable<T> Generate<T>(T initialState, Func<T, bool> condition, Func<T, T> iterate) =>
            EnumerableEx.Generate(initialState, condition, iterate, _ => _);
        public static IEnumerable<TR> Generate<T, TR>(T initialState, Func<T, T> iterate, Func<T, TR> resultSelector) =>
            EnumerableEx.Generate(initialState, _ => true, iterate, resultSelector);
        public static IEnumerable<T> Generate<T>(T initialState, Func<T, T> iterate) =>
            EnumerableEx.Generate(initialState, _ => true, iterate, _ => _);

        public static (long index, T value) BinarySearch<T>(long lower, long upper, Func<long, T> valueProducer, Func<T, long> comparer)
        {
            while (lower <= upper)
            {
                var mid = (lower + upper) / 2;

                var value = valueProducer(mid);
                var test = comparer(value);

                if (test < 0)
                    upper = mid - 1;
                else if (test > 0)
                    lower = mid + 1;
                else
                    return (mid, value);
            }

            return (upper, valueProducer(upper));
        }

        public static Combinations<T> Combinations<T>([NotNull] this IEnumerable<T> items, int count) =>
            new Combinations<T>(items.EnsureList(), 2);

        public static IEnumerable<(T a, T b)> Combinations2<T>([NotNull] this IEnumerable<T> items) =>
            Combinations(items, 2).Select(l => (l[0], l[1]));
    }
}

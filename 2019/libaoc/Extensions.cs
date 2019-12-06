using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using MoreLinq.Extensions;

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
        public static IEnumerable<ValueTuple<T, T>> Batch2<T>([NotNull] this IEnumerable<T> @this) =>
            @this.Batch(2).Select(b => b.First2());
        /// <summary>Stream enumerable batches into 3-tuples</summary>
        public static IEnumerable<ValueTuple<T, T, T>> Batch3<T>([NotNull] this IEnumerable<T> @this) =>
            @this.Batch(3).Select(b => b.First3());
        /// <summary>Stream enumerable batches into 4-tuples</summary>
        public static IEnumerable<ValueTuple<T, T, T, T>> Batch4<T>([NotNull] this IEnumerable<T> @this) =>
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

        public static Size GetDimensions<T>(this T[,] @this) =>
            new Size(@this.GetLength(0), @this.GetLength(1));
        
        public static IEnumerable<string> ToLines(this char[,] @this)
        {
            var size = @this.GetDimensions();
            
            var sb = new StringBuilder();
            for (var y = 0; y < size.Height; ++y)
            {
                sb.Clear();
                for (var x = 0; x < size.Width; ++x)
                    sb.Append(@this[x, y]);
                yield return sb.ToString();
            }
        }

        public static string ToText(this char[,] @this) =>
            string.Join("\n", @this.ToLines());

        public static int ParseInt([NotNull] this string @this) =>
            int.Parse(@this);

        public static int GroupInt([NotNull] this Match @this, int groupNum) =>
            @this.Groups[groupNum].Value.ParseInt();
        public static int GroupInt([NotNull] this Match @this, string groupName) =>
            @this.Groups[groupName].Value.ParseInt();

        public static IEnumerable<(T cell, int x, int y)> SelectCells<T>(this T[,] @this, Size max)
        {
            for (var y = 0; y < max.Height; ++y)
                for (var x = 0; x < max.Width; ++x)
                    yield return (cell: @this[x, y], x, y);
        }

        public static IEnumerable<(T cell, int x, int y)> SelectCells<T>(this T[,] @this) =>
            @this.SelectCells(@this.GetDimensions());
        
        public static IEnumerable<Point> SelectCoords<T>(this T[,] @this)
        {
            var size = @this.GetDimensions();
            for (var y = 0; y < size.Height; ++y)
                for (var x = 0; x < size.Width; ++x)
                    yield return new Point(x, y);
        }

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

        public static T[,] Fill<T>(this T[,] @this, Func<Point, T> generator)
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

        public static IEnumerable<T> SelectAdjacent<T>(this Point @this, Func<Point, T> selector) =>
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

        public static IEnumerable<T> SelectAdjacentWithDiagonals<T>(this Point @this, Func<Point, T> selector) =>
            @this.SelectAdjacentWithDiagonals().Select(selector);

        /// <summary>Convert a `Dictionary` to an `AutoDictionary` and assign it the given default-get delegate</summary>
        public static AutoDictionary<TKey, TValue> ToAutoDictionary<TKey, TValue>([NotNull] this IDictionary<TKey, TValue> @this, Func<TKey, TValue> getDefault) =>
            new AutoDictionary<TKey, TValue>(@this, getDefault);

        /// <summary>Convert a `Dictionary` to an `AutoDictionary` and assign it a default-get delegate that just returns `defaultValue`</summary>
        public static AutoDictionary<TKey, TValue> ToAutoDictionary<TKey, TValue>([NotNull] this IDictionary<TKey, TValue> @this, TValue defaultValue = default) =>
            new AutoDictionary<TKey, TValue>(@this, _ => defaultValue);

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

        public static T[] Arr<T>(params T[] items) => items;

        public static IEnumerable<T> Generate<T>(T initialState, Func<T, bool> condition, Func<T, T> iterate) =>
            EnumerableEx.Generate(initialState, condition, iterate, _ => _);
        
        public static IEnumerable<TResult> Generate<TState, TResult>(TState initialState, Func<TState, TResult> resultSelector) =>
            EnumerableEx.Generate(initialState, _ => true, _ => _, resultSelector);
    }
}

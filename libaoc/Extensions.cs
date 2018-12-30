using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using MoreLinq.Extensions;

namespace AoC
{
    public static partial class Extensions
    {
        public static IEnumerable<T> WhereNotNull<T>([NotNull] this IEnumerable<T> @this) where T : class
            => @this.Where(t => !(t is null));

        public static IEnumerable<T> EmptyIfNull<T>([CanBeNull] this IEnumerable<T> @this)
            => @this ?? Enumerable.Empty<T>();

        public static T OrNull<T>([CanBeNull] this T @this, [NotNull] Func<T, T> operation) where T : class
            => @this != null ? operation(@this) : null;

        public static IEnumerable<TResult> SelectWhere<TSource, TResult>(
            [NotNull] this IEnumerable<TSource> @this,
            [NotNull] Func<TSource, (TResult selected, bool where)> selectWhere)
        {
            foreach (var item in @this)
            {
                var (selected, where) = selectWhere(item);
                if (where)
                    yield return selected;
            }
        }

        public static IEnumerable<IReadOnlyList<T>> BatchList<T>([NotNull] this IEnumerable<T> @this, int batchSize) =>
            @this.Batch(batchSize).Select(b => b.ToList());
        public static IEnumerable<TR> Batch<T, TR>([NotNull] this IEnumerable<T> @this, int batchSize, [NotNull] Func<IReadOnlyList<T>, TR> selector) =>
            BatchExtension.Batch(@this, batchSize, b => selector(b.ToList()));
        public static IEnumerable<ValueTuple<T, T>> Batch2<T>([NotNull] this IEnumerable<T> @this) =>
            @this.Batch(2).Select(b => b.First2());
        public static IEnumerable<ValueTuple<T, T, T>> Batch3<T>([NotNull] this IEnumerable<T> @this) =>
            @this.Batch(3).Select(b => b.First3());
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

        public static T FirstOrDefault<T>(this IEnumerable<T> @this, T defaultValue)
        {
            using (var e = @this.GetEnumerator())
                return e.MoveNext() ? e.Current : defaultValue;
        }

        public static TR FirstOrDefault<T, TR>(this IEnumerable<T> @this, Func<T, bool, TR> selector)
        {
            using (var e = @this.GetEnumerator())
                return e.MoveNext()
                    ? selector(e.Current, true)
                    : selector(default, false);
        }

        public static T? FirstOrNull<T>(this IEnumerable<T> @this) where T : struct
            => @this.Select(item => (T?)item).FirstOrDefault();

        public static T? SingleOrNull<T>(this IEnumerable<T> @this) where T : struct
            => @this.Select(item => (T?)item).SingleOrDefault();

        public static Size GetDimensions<T>(this T[,] @this)
            => new Size(@this.GetLength(0), @this.GetLength(1));
        
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

        public static string ToText(this char[,] @this)
            => string.Join("\n", @this.ToLines());

        public static int ParseInt([NotNull] this string @this)
            => int.Parse(@this);

        public static int GroupInt([NotNull] this Match @this, int groupNum)
            => @this.Groups[groupNum].Value.ParseInt();
        public static int GroupInt([NotNull] this Match @this, string groupName)
            => @this.Groups[groupName].Value.ParseInt();

        public static IEnumerable<(T cell, int x, int y)> SelectCells<T>(this T[,] @this, Size max)
        {
            for (var y = 0; y < max.Height; ++y)
                for (var x = 0; x < max.Width; ++x)
                    yield return (cell: @this[x, y], x, y);
        }

        public static IEnumerable<(T cell, int x, int y)> SelectCells<T>(this T[,] @this)
            => @this.SelectCells(@this.GetDimensions());
        
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

        public static T[,] FillNew<T>(this T[,] @this) where T : new()
            => @this.Fill(_ => new T());

        public static T[,] Fill<T>(this T[,] @this, T value)
            => @this.Fill(_ => value);

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

        public static Point BottomRight(in this Rectangle @this)
            => new Point(@this.Right, @this.Bottom);

        public static IEnumerable<T> OrderByReading<T>(this IEnumerable<T> @this, Func<T, Point> selector)
        {
            return
                from item in @this
                let pos = selector(item)
                orderby pos.Y, pos.X
                select item;
        }

        public static IEnumerable<Point> OrderByReading(this IEnumerable<Point> @this)
            => @this.OrderByReading(_ => _);

        public static IEnumerable<Point> SelectAdjacent(this Point @this)
        {
            yield return new Point(@this.X, @this.Y - 1);
            yield return new Point(@this.X - 1, @this.Y);
            yield return new Point(@this.X + 1, @this.Y);
            yield return new Point(@this.X, @this.Y + 1);
        }

        public static IEnumerable<T> SelectAdjacent<T>(this Point @this, Func<Point, T> selector)
            => @this.SelectAdjacent().Select(selector);

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

        public static IEnumerable<T> SelectAdjacentWithDiagonals<T>(this Point @this, Func<Point, T> selector)
            => @this.SelectAdjacentWithDiagonals().Select(selector);

        public static AutoDictionary<TKey, TValue> ToAutoDictionary<TKey, TValue>([NotNull] this IDictionary<TKey, TValue> @this, Func<TKey, TValue> getDefault)
            => new AutoDictionary<TKey, TValue>(@this, getDefault);

        public static AutoDictionary<TKey, TValue> ToAutoDictionary<TKey, TValue>([NotNull] this IDictionary<TKey, TValue> @this, TValue defaultValue = default)
            => new AutoDictionary<TKey, TValue>(@this, _ => defaultValue);

        public static T Clamp<T>([NotNull] this T @this, T min, T max) where T : IComparable<T>
        {
            if (min.CompareTo(max) > 0)
                throw new ArgumentException("'min' cannot be greater than 'max'", nameof(min));

            if (@this.CompareTo(min) < 0) return min;
            if (@this.CompareTo(max) > 0) return max;
            return @this;
        }

        public static T PatternSeekingGetItemAt<T>([NotNull] this IEnumerable<T> @this, int index, int minRepeat = 10)
        {
            // TODO: use proper deque
            var history = new Dictionary<T, List<int>>().ToAutoDictionary(_ => new List<int>());

            var i = 0;
            foreach (var item in @this)
            {
                // no pattern yet, but hit the index anyway
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

    // exactly like a Dictionary except it will guarantee that entries exist for any `get` call by ensuring already filled by delegate from ctor
    public class AutoDictionary<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>, IDictionary<TKey, TValue>
    {
        readonly IDictionary<TKey, TValue> m_Dictionary;
        readonly Func<TKey, TValue> m_GetDefault;

        public AutoDictionary(IDictionary<TKey, TValue> dictionary, Func<TKey, TValue> getDefault)
            => (m_Dictionary, m_GetDefault) = (dictionary, getDefault);

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
            => m_Dictionary.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator()
            => m_Dictionary.GetEnumerator();

        public void Add(TKey key, TValue value)
            => m_Dictionary.Add(key, value);
        public void Add(KeyValuePair<TKey, TValue> item)
            => m_Dictionary.Add(item);
        public void Clear()
            => m_Dictionary.Clear();
        public bool ContainsKey(TKey key)
            => m_Dictionary.ContainsKey(key);
        public bool Contains(KeyValuePair<TKey, TValue> item)
            => m_Dictionary.Contains(item);
        public int Count
            => m_Dictionary.Count;
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
            => m_Dictionary.CopyTo(array, arrayIndex);
        public bool IsReadOnly
            => m_Dictionary.IsReadOnly;
        public bool Remove(TKey key)
            => m_Dictionary.Remove(key);
        public bool Remove(KeyValuePair<TKey, TValue> item)
            => m_Dictionary.Remove(item);
        public bool TryGetValue(TKey key, out TValue value)
            => m_Dictionary.TryGetValue(key, out value);


        public ICollection<TKey> Keys
            => m_Dictionary.Keys;
        public ICollection<TValue> Values
            => m_Dictionary.Values;
        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys
            => m_Dictionary.Keys;
        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values
            => m_Dictionary.Values;

        public TValue this[TKey key]
        {
            get
            {
                if (!m_Dictionary.TryGetValue(key, out var value))
                    m_Dictionary.Add(key, value = m_GetDefault(key));
                return value;
            }
            set => m_Dictionary[key] = value;
        }
    }
}

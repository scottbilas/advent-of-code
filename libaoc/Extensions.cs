using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
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

        public static TR FirstOrDefault<T, TR>(this IEnumerable<T> @this, Func<T, bool, TR> selector)
        {
            using (var e = @this.GetEnumerator())
                return e.MoveNext()
                    ? selector(e.Current, true)
                    : selector(default(T), false);
        }

        public static (int cx, int cy) GetDimensions<T>(this T[,] @this)
            => (cx: @this.GetLength(0), cy: @this.GetLength(1));
        
        public static IEnumerable<string> ToLines(this char[,] @this)
        {
            var (cx, cy) = @this.GetDimensions();
            
            var sb = new StringBuilder();
            for (var y = 0; y < cy; ++y)
            {
                sb.Clear();
                for (var x = 0; x < cx; ++x)
                    sb.Append(@this[x, y]);
                yield return sb.ToString();
            }
        }

        public static string ToText(this char[,] @this)
            => string.Join('\n', @this.ToLines());

        public static IEnumerable<(T cell, int x, int y)> SelectCells<T>(this T[,] @this)
        {
            var (cx, cy) = @this.GetDimensions();
            for (var y = 0; y < cy; ++y)
                for (var x = 0; x < cx; ++x)
                    yield return (cell: @this[x, y], x, y);
        }
        
        public static IEnumerable<(int x, int y)> SelectCoords<T>(this T[,] @this)
        {
            var (cx, cy) = @this.GetDimensions();
            for (var y = 0; y < cy; ++y)
                for (var x = 0; x < cx; ++x)
                    yield return (x, y);
        }
        
        public static T[,] Fill<T>(this T[,] @this, T value)
            => @this.Fill(_ => value);

        public static T[,] Fill<T>(this T[,] @this, Func<(int x, int y), T> generator)
        {
            foreach (var coord in @this.SelectCoords())
                @this[coord.x, coord.y] = generator(coord);

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
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using MoreLinq.Extensions;

namespace AoC
{
    public static class Extensions
    {
        public static IEnumerable<int> SelectInts(this string @this) => Regex
            .Matches(@this, @"\d+")
            .Cast<Match>()
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
                    : selector(default, false);
        }

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

        public static IEnumerable<(T cell, int x, int y)> SelectCells<T>(this T[,] @this)
        {
            var size = @this.GetDimensions();
            for (var y = 0; y < size.Height; ++y)
                for (var x = 0; x < size.Width; ++x)
                    yield return (cell: @this[x, y], x, y);
        }
        
        public static IEnumerable<Point> SelectCoords<T>(this T[,] @this)
        {
            var size = @this.GetDimensions();
            for (var y = 0; y < size.Height; ++y)
                for (var x = 0; x < size.Width; ++x)
                    yield return new Point(x, y);
        }

        public static char[,] ToGrid([NotNull] this string @this)
        {
            var lines = @this
                .Trim()
                .Split('\n')
                .Select(l => l.Trim())
                .ToList();

            if (lines.Any(l => l.Length != lines[0].Length))
                throw new InvalidOperationException("Non-regular grid");
            
            return new char[lines[0].Length, lines.Count]
                .Fill(coord => lines[coord.Y][coord.X]);
        }
        
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
    }

}

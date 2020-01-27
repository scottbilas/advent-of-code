using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using Unity.Coding.Utils;
using JetBrains.Annotations;

// ReSharper disable UnusedMember.Global

namespace Aoc2017
{
    public static class StringExtensions
    {
        public static string[] Split([NotNull] this string @this, char delim, int count)
            => @this.Split(new[] { delim }, count);

        public static string[] Split([NotNull] this string @this, string splitText)
        {
            var found = @this.IndexOf(splitText);
            return found >= 0
                ? new [] { @this.Left(found), @this.Mid(found + splitText.Length) }
                : new [] { @this };
        }

        public static char[,] ParseGrid([NotNull] this string @this, char defaultFill, int border)
        {
            var lines = @this
                .SelectLines(false)
                .SkipWhile(l => l.Trim().IsEmpty())
                .TakeWhile(l => l.Trim().Any())
                .Select(l => l.TrimEnd())
                .ToList();

            var skip = lines.Min(l => l.Length - l.TrimStart().Length);
            var grid = new char[
                    lines.Select(l => l.Length).Max() - skip + border * 2,
                    lines.Count + border * 2]
                .Fill(defaultFill);

            for (var y = 0; y < lines.Count; ++y)
                for (var x = skip; x < lines[y].Length; ++x)
                    grid[x - skip + border, y + border] = lines[y][x];

            return grid;
        }

        public static char[,] ParseRectGrid([NotNull] this string @this, bool trim = true)
        {
            var lines = @this
                .Trim()
                .SelectLines(trim)
                .ToList();

            if (lines.Any(l => l.Length != lines[0].Length))
                throw new InvalidOperationException("Non-regular grid");

            return new char[lines[0].Length, lines.Count]
                .Fill(coord => lines[coord.Y][coord.X]);
        }

        public static string TrimBlock(this string @this)
            => @this.Split('\n').Select(l => l.Trim()).StringJoin('\n');

        public static (Int2 min, Int2 max) MinMax(this IEnumerable<Int2> @this)
        {
            var (min, max) = (Int2.MaxValue, Int2.MinValue);
            foreach (var pos in @this)
            {
                Utils.Minimize(ref min, pos);
                Utils.Maximize(ref max, pos);
            }
            return (min, max);
        }

        public static (Int2 min, Int2 max) MinMax<T>(this IEnumerable<KeyValuePair<Int2, T>> @this) =>
            MinMax(@this.Select(v => v.Key));

        // TODO: this function sucks. need to be able to
        // * only visit each cell max one time for convert function
        // * trim unused space
        // * add a border (or maybe just add this during final ToText)
        // * know difference between unused and default
        public static TR[,] ToGrid<T, TR>(this IEnumerable<KeyValuePair<Int2, T>> @this, Func<T, TR> convert, bool trim = true)
        {
            var defValue = convert(default);

            if (trim)
                @this = @this.Where(c => !Equals(convert(c.Value), defValue));

            @this = @this.UnDefer();

            const int border = 1;
            const int extra = border * 2 + 1;

            var (min, max) = MinMax(@this);
            var size = max - min;
            var grid = new TR[size.X + extra, size.Y + extra];

            grid.Fill(defValue);

            foreach (var (pos, cell) in @this)
                grid[pos.X - min.X + border, pos.Y - min.Y + border] = convert(cell);

            return grid;
        }

        public static string[][] MultiSplit([NotNull] this string @this, string delim0, string delim1)
        {
            var firstLevel = @this.Split(delim0);
            var result = new string[firstLevel.Length][];
            for (var i = 0; i < firstLevel.Length; ++i)
                result[i] = firstLevel[i].Split(delim1);
            return result;
        }

        public static IEnumerable<string> SelectLines([NotNull] this string @this, bool trim = true)
        {
            var reader = new StringReader(@this);
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (trim)
                {
                    line = line.Trim();
                    if (line.Any())
                        yield return line;
                }
                else
                    yield return line;

            }
        }

        public static string[] Lines([NotNull] this string @this, bool trim = true) => @this
            .SelectLines(trim).ToArray();

        public static string ReplaceMatches([NotNull] this string @this, string pattern, string replacement) =>
            Regex.Replace(@this, pattern, replacement);

        public static IEnumerable<Match> SelectMatches([NotNull] this string @this, string pattern) => Regex
            .Matches(@this, pattern)
            // ReSharper disable once RedundantEnumerableCastCall (required for .net framework to link to this, because MatchCollection is not yet an IList<Match>)
            .Cast<Match>();

        public static IEnumerable<Match> Matches([NotNull] this string @this, string pattern) => @this
            .SelectMatches( pattern).ToArray();

        public static IEnumerable<string> SelectWords([NotNull] this string @this) => @this
            .SelectMatches(@"\w+")
            .Select(m => m.Value);

        public static string[] Words([NotNull] this string @this) => @this
            .SelectWords().ToArray();

        public static IEnumerable<int> SelectInts([NotNull] this string @this) => @this
            .SelectMatches(@"[-+]?\d+")
            .Select(m => int.Parse(m.Value));

        public static int[] Ints([NotNull] this string @this) => @this
            .SelectInts().ToArray();

        public static IEnumerable<BigInteger> SelectBigInts([NotNull] this string @this) => @this
            .SelectMatches(@"[-+]?\d+")
            .Select(m => BigInteger.Parse(m.Value));

        public static IEnumerable<Int2> SelectInt2s([NotNull] this string @this) => @this
            .SelectInts()
            .Batch2()
            .Select(v => new Int2(v));

        public static Int2[] Int2s([NotNull] this string @this) => @this
            .SelectInt2s().ToArray();

        public static IEnumerable<Int3> SelectInt3s([NotNull] this string @this) => @this
            .SelectInts()
            .Batch3()
            .Select(v => new Int3(v));

        public static Int3[] Int3s([NotNull] this string @this) => @this
            .SelectInt3s().ToArray();

        public static IEnumerable<Int4> SelectInt4s([NotNull] this string @this) => @this
            .SelectInts()
            .Batch4()
            .Select(v => new Int4(v));

        public static Int4[] Int4s([NotNull] this string @this) => @this
            .SelectInt4s().ToArray();

        public static IEnumerable<float> SelectFloats([NotNull] this string @this) => @this
            .SelectMatches(@"[-+]?\d*\.?\d+(?:[eE][-+]?\d+)?")
            .Select(m => float.Parse(m.Value));

        public static float[] Floats([NotNull] this string @this) => @this
            .SelectFloats().ToArray();

        public static string ToStringRepeat([NotNull] this string text, int repeatCount)
        {
            var sb = new StringBuilder(text.Length * repeatCount);
            for (var i = 0; i < repeatCount; ++i)
                sb.Append(text);
            return sb.ToString();
        }

        public static string ToStringFromChars([NotNull] this IEnumerable<char> @this) =>
            new string(@this.ToArray());

        public static string ToStringFromChars([NotNull] this IEnumerable<char> @this, int count) =>
            new string(@this.Take(count).ToArray());

        public static int[] ToDigits([NotNull] this string @this) => @this.Select(c => c - '0').ToArray();

        public static string FromDigits([NotNull] this IEnumerable<int> @this, int offset, int count) => FromDigits(@this.Skip(offset).Take(count));
        public static string FromDigits([NotNull] this IEnumerable<int> @this, int offset) => FromDigits(@this.Skip(offset));
        public static string FromDigits([NotNull] this IEnumerable<int> @this) => new string(@this.Select(i => (char)(i + '0')).ToArray());

        public static char? TryGetAt([NotNull] this string @this, int offset) =>
            offset >= 0 && offset < @this.Length ? (char?)@this[offset] : null;
        public static char TryGetAt([NotNull] this string @this, int offset, char defValue) =>
            @this.TryGetAt(offset) ?? defValue;
    }
}

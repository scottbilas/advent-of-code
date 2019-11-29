using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using JetBrains.Annotations;

// ReSharper disable UnusedMember.Global

namespace Aoc2019
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

        public static IEnumerable<int> SelectInts([NotNull] this string @this) => Regex
            .Matches(@this, @"[-+]?\d+")
            .Cast<Match>()
            .Select(m => int.Parse(m.Value));

        public static IEnumerable<float> SelectFloats([NotNull] this string @this) => Regex
            .Matches(@this, @"[-+]?\d*\.?\d+(?:[eE][-+]?\d+)?")
            .Cast<Match>()
            .Select(m => float.Parse(m.Value));

        // string-enumerable extensions

        [NotNull]
        public static string Left([NotNull] this string @this, int maxChars)
            => @this.Substring(0, Math.Min(maxChars, @this.Length));

        [NotNull]
        public static string Mid([NotNull] this string @this, int offset, int maxChars = -1)
        {
            if (offset < 0)
                throw new ArgumentException("offset must be >= 0", nameof(offset));

            var safeOffset = offset.Clamp(0, @this.Length);
            var actualMaxChars = @this.Length - safeOffset;

            var safeMaxChars = maxChars < 0 ? actualMaxChars : Math.Min(maxChars, actualMaxChars);

            return @this.Substring(safeOffset, safeMaxChars);
        }

        [NotNull]
        public static string Right([NotNull] this string @this, int maxChars)
        {
            var safeMaxChars = Math.Min(maxChars, @this.Length);
            return @this.Substring(@this.Length - safeMaxChars, safeMaxChars);
        }

        public static string StringJoin([NotNull] this IEnumerable @this, [NotNull] string separator)
            => string.Join(separator, @this.Cast<object>());

        [NotNull]
        public static string StringJoin([NotNull] this IEnumerable @this, char separator)
            => string.Join(new string(separator, 1), @this.Cast<object>());

        [NotNull]
        public static string StringJoin<T, TSelected>([NotNull] this IEnumerable<T> @this, [NotNull] Func<T, TSelected> selector, [NotNull] string separator)
            => string.Join(separator, @this.Select(selector));

        [NotNull]
        public static string StringJoin<T, TSelected>([NotNull] this IEnumerable<T> @this, [NotNull] Func<T, TSelected> selector, char separator)
            => string.Join(new string(separator, 1), @this.Select(selector));
    }
}

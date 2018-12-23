using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using JetBrains.Annotations;

// ReSharper disable UnusedMember.Global

namespace AoC
{
    public static class StringExtensions
    {
        public static string[] Split([NotNull] this string @this, char delim, int count)
            => @this.Split(new[] { delim }, count);

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

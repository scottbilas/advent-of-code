using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Unity.Coding.Utils;
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
    }
}

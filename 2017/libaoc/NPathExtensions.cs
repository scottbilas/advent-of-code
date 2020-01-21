using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using Unity.Coding.Utils;

namespace Aoc2017
{
    public static class NPathExtensions
    {
        public static byte[] ReadAllBytes(this NPath @this) => File.ReadAllBytes(@this);
        public static IEnumerable<string> ReadAllWords(this NPath @this) => @this.ReadAllText().SelectWords();
        public static IEnumerable<int> ReadAllInts(this NPath @this) => @this.ReadAllText().SelectInts();
        public static IEnumerable<BigInteger> ReadAllBigInts(this NPath @this) => @this.ReadAllText().SelectBigInts();
        public static IEnumerable<float> ReadAllFloats(this NPath @this) => @this.ReadAllText().SelectFloats();
        public static char[,] ReadGrid(this NPath @this) => @this.ReadAllText().ToGrid();

        public static NPath WriteAllBytes(this NPath @this, byte[] data)
        {
            File.WriteAllBytes(@this, data);
            return @this;
        }

        public static string ToUri(this NPath @this) => new Uri(@this.ToString()).AbsoluteUri;
    }
}

using System;
using System.Collections.Generic;
using System.Numerics;
using Unity.Coding.Utils;

namespace Aoc2019
{
    public static class NPathExtensions
    {
        public static IEnumerable<string> ReadAllWords(this NPath @this) => @this.ReadAllText().SelectWords();
        public static IEnumerable<int> ReadAllInts(this NPath @this) => @this.ReadAllText().SelectInts();
        public static IEnumerable<BigInteger> ReadAllBigInts(this NPath @this) => @this.ReadAllText().SelectBigInts();
        public static IEnumerable<float> ReadAllFloats(this NPath @this) => @this.ReadAllText().SelectFloats();
    }
}

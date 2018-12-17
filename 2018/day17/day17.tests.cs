using System;
using System.IO;
using AoC;
using NUnit.Framework;
using Shouldly;

namespace Day17
{
    class Day17 : AocFixture
    {
        [Test]
        public void Samples()
        {
            Solver
                .CountReachableTiles((500, 0), @"
                    x=495, y=2..7
                    y=7, x=495..501
                    x=501, y=3..7
                    x=498, y=2..4
                    x=506, y=1..2
                    x=498, y=10..13
                    x=504, y=10..13
                    y=13, x=498..504")
                .touched.ShouldBe(57);
        }

        [Test]
        public void Problem()
        {
            var result = Solver
                .CountReachableTiles((500, 0), ScriptDir.Combine("input.txt").ReadAllText());
                
            result.touched.ShouldBe(39162);
            result.retained.ShouldBe(1);
        }
    }
}

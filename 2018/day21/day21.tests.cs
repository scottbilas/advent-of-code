using AoC;
using NUnit.Framework;
using Shouldly;

namespace Day21
{
    class Day21 : AocFixture
    {
        [Test]
        public void Part1()
            => Solver.Part1().ShouldBe(986758);

        [Test]
        public void Part2()
            => Solver.Part2().ShouldBe(16016565);
    }
}

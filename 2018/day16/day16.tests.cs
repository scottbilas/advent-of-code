using AoC;
using NUnit.Framework;
using Shouldly;

namespace Day16
{
    class Day16 : AocFixture
    {
        [Test]
        public void Samples()
        {
            Solver
                .CountOpsMatching(3, @"
                    Before: [3, 2, 1, 1]
                    9 2 1 2
                    After:  [3, 2, 2, 1]")
                .ShouldBe(1);
        }

        [Test]
        public void Part1()
        {
            Solver
                .CountOpsMatching(3, ScriptDir.Combine("input1.txt").ReadAllText())
                .ShouldBe(521);
        }

        [Test]
        public void Part2()
        {
            var ops = Solver.DeduceOps(ScriptDir.Combine("input1.txt").ReadAllText());
            
            Solver
                .RunProgram(ops, ScriptDir.Combine("input2.txt").ReadAllText())
                .ShouldBe(594);
        }
    }
}

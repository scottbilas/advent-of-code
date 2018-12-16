using System;
using System.IO;
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
                .CountOpsMatching(3, File.ReadAllText($"{ScriptDir}/input1.txt"))
                .ShouldBe(521);
        }

        [Test]
        public void Part2()
        {
            var ops = Solver.DeduceOps(File.ReadAllText($"{ScriptDir}/input1.txt"));
            
            Solver
                .RunProgram(ops, File.ReadAllText($"{ScriptDir}/input2.txt"))
                .ShouldBe(594);
        }
    }
}

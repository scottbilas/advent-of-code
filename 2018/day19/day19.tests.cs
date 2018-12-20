using System;
using System.IO;
using AoC;
using NUnit.Framework;
using Shouldly;

namespace Day19
{
    class Day19 : AocFixture
    {
        [Test]
        public void Samples()
        {
            Solver
                .RunProgram(0, 0, @"
                    #ip 0
                    seti 5 0 1
                    seti 6 0 2
                    addi 0 1 0
                    addr 1 2 3
                    setr 1 0 0
                    seti 8 0 4
                    seti 9 0 5")
                .ShouldBe(6);
        }

        [Test]
        public void Part1()
        {
            Solver
                .RunProgram(4, 0, ScriptDir.Combine("input.txt").ReadAllText())
                .ShouldBe(2240);
        }

        [Test]
        public void Part2()
        {
            /*
            Solver
                .RunProgram(4, 1, ScriptDir.Combine("input.txt").ReadAllText())
                .ShouldBe(1);*/
        }
    }
}

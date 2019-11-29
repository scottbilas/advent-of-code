using Aoc2018;
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
                .RunProgram(@"
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

        [Test, Category("Very slow")] // should just derive it
        public void Part1()
        {
            Solver
                .RunProgram(ScriptDir.Combine("input.txt").ReadAllText())
                .ShouldBe(2240);
        }

        [Test]
        public void Part2()
        {
            // resulted from manual disassembly and reduction (see disasm.txt); not a brute force thing

            var rc = 0;

            for (var i = 1; i <= 10551348; ++i)
                if ((10551348 % i) == 0)
                    rc += i;

            rc.ShouldBe(26671554);
        }
    }
}

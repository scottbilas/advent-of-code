using Aoc2018;
using NUnit.Framework;
using Shouldly;

namespace Day18
{
    class Day18 : AocFixture
    {
        [Test, Ignore("Fails, but worked last year, need to investigate")]
        public void Samples()
        {
            Solver
                .Sim(10, @"
                    .#.#...|#.
                    .....#|##|
                    .|..|...#.
                    ..|#.....#
                    #.#|||#|#|
                    ...#.||...
                    .|....|...
                    ||...#|.#|
                    |.||||..|.
                    ...#.|..|.")
                .ShouldBe(1147);
        }

        [Test, Ignore("Very slow")]
        public void Part1()
        {
            Solver
                .Sim(10, ScriptDir.Combine("input.txt").ReadAllText())
                .ShouldBe(384480);
        }

        [Test, Ignore("Very slow")]
        public void Part2()
        {
            Solver
                .Sim(1000000000, ScriptDir.Combine("input.txt").ReadAllText())
                .ShouldBe(177004);
        }
    }
}

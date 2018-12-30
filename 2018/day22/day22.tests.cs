using System.Drawing;
using AoC;
using NUnit.Framework;
using Shouldly;

namespace Day22
{
    class Day22 : AocFixture
    {
        (int depth, Point target, Size padding, int part1, int part2)
            m_SampleData = (510, new Point(10, 10), new Size(5, 5), 114, 45);
        (int depth, Point target, Size padding, int part1, int part2)
            m_ProblemData = (3879, new Point(8, 713), new Size(10, 10), 6323, 0);

        [Test]
        public void Sample_Erosion_Matches()
        {
            var verify = @"
                M=.|=.|.|=.|=|=.
                .|=|=|||..|.=...
                .==|....||=..|==
                =.|....|.==.|==.
                =|..==...=.|==..
                =||.=.=||=|=..|=
                |.=.===|||..=..|
                |..==||=.|==|===
                .=..===..=|.|||.
                .======|||=|=.|=
                .===|=|===T===||
                =|||...|==..|=.|
                =.=|=.=..=.||==|
                ||=|=...|==.=|==
                |=.=||===.|||===
                ||.|==.|.|.||=||
                "
                .ToGrid();

            var solver = new Solver(m_SampleData.target, m_SampleData.padding);
            var erosion = solver.BuildErosion(m_SampleData.depth);
            var board = solver.BuildBoard(erosion);
            board[0, 0] = 'M';
            board[solver.TargetPos.X, solver.TargetPos.Y] = 'T';
            board.ToText().ShouldBe(verify.ToText());
        }

        [Test]
        public void Sample_Part1()
        {
            var solver = new Solver(m_SampleData.target, m_SampleData.padding);
            var erosion = solver.BuildErosion(m_SampleData.depth);
            solver.CalcRisk(erosion).ShouldBe(m_SampleData.part1);
        }

        [Test]
        public void Sample_Part2_Manual()
        {
            var solver = new ManualSolver(m_SampleData.target, m_SampleData.padding);
            var erosion = solver.BuildErosion(m_SampleData.depth);
            solver.CalcMinDistToTarget(erosion).ShouldBe(m_SampleData.part2);
        }

        [Test]
        public void Sample_Part2_QuickGraph()
        {
            var solver = new QuickGraphSolver(m_SampleData.target, m_SampleData.padding);
            var erosion = solver.BuildErosion(m_SampleData.depth);
            solver.CalcMinDistToTarget(erosion).ShouldBe(m_SampleData.part2);
        }

        [Test]
        public void Problem_Part1()
        {
            var solver = new Solver(m_ProblemData.target, m_ProblemData.padding);
            var erosion = solver.BuildErosion(m_ProblemData.depth);
            solver.CalcRisk(erosion).ShouldBe(m_ProblemData.part1);
        }

        [Test]
        public void Problem_Part2_Manual()
        {
            var solver = new ManualSolver(m_ProblemData.target, m_ProblemData.padding);
            var erosion = solver.BuildErosion(m_ProblemData.depth);
            solver.CalcMinDistToTarget(erosion).ShouldBe(m_ProblemData.part2);
        }

        [Test]
        public void Problem_Part2_QuickGraph()
        {
            var solver = new QuickGraphSolver(m_ProblemData.target, m_ProblemData.padding);
            var erosion = solver.BuildErosion(m_ProblemData.depth);
            solver.CalcMinDistToTarget(erosion).ShouldBe(m_ProblemData.part2);
            // wrong: 1012
        }
    }
}

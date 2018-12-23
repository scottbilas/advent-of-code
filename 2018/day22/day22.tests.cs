using System.Drawing;
using AoC;
using NUnit.Framework;
using Shouldly;

namespace Day22
{
    class Day22 : AocFixture
    {
        (int depth, Point target, int part1, int part2) m_SampleData = (510, new Point(10, 10), 114, 45);
        (int depth, Point target, int part1, int part2) m_ProblemData = (3879, new Point(8, 713), 6323, 0);

        [Test]
        public void Sample_Part1()
        {
            var solver = new Solver(m_SampleData.target);
            var erosion = solver.BuildErosion(m_SampleData.depth);
            solver.CalcRisk(erosion).ShouldBe(m_SampleData.part1);
        }

        [Test]
        public void Sample_Part2_Manual()
        {
            var solver = new ManualSolver(m_SampleData.target);
            var erosion = solver.BuildErosion(m_SampleData.depth);
            solver.CalcMinDistToTarget(erosion).ShouldBe(m_SampleData.part2);
        }

        [Test]
        public void Sample_Part2_QuickGraph()
        {
            var solver = new QuickGraphSolver(m_SampleData.target);
            var erosion = solver.BuildErosion(m_SampleData.depth);
            solver.CalcMinDistToTarget(erosion).ShouldBe(m_SampleData.part2);
        }

        [Test]
        public void Problem_Part1()
        {
            var solver = new Solver(m_ProblemData.target);
            var erosion = solver.BuildErosion(m_ProblemData.depth);
            solver.CalcRisk(erosion).ShouldBe(m_ProblemData.part1);
        }

        [Test]
        public void Problem_Part2_Manual()
        {
            var solver = new ManualSolver(m_ProblemData.target);
            var erosion = solver.BuildErosion(m_ProblemData.depth);
            solver.CalcMinDistToTarget(erosion).ShouldBe(m_ProblemData.part2);
            // wrong: 1012
        }

        [Test]
        public void Problem_Part2_QuickGraph()
        {
            var solver = new QuickGraphSolver(m_ProblemData.target);
            var erosion = solver.BuildErosion(m_ProblemData.depth);
            solver.CalcMinDistToTarget(erosion).ShouldBe(m_ProblemData.part2);
            // wrong: 1012
        }
    }
}

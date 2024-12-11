using static Dir;

class Day10 : Fixture
{
    int Solve(string input, bool unique) => With(
        input.ToGrid(c => c != '.' ? c.Int() : 99).AddBorder(99),
        grid => grid.Cells()
            .Where(c => c.cell == 0)
            .Sum(c =>
            {
                var (sum, seen) = (0, new HashSet<Int2>());
                for (var work = new Queue<Int2>([c.pos]); work.TryDequeue(out var cur); )
                {
                    foreach (var dir in All4)
                    {
                        var (next, want) = (cur + dir.Move, grid.Get(cur) + 1);
                        if (grid.Get(next) != want)
                            continue;

                        if (want != 9)
                            work.Enqueue(next);
                        else if (!unique || seen.Add(next))
                            ++sum;
                    }
                }
                return sum;
            }));

    int Solve1(string input) => Solve(input, true);
    int Solve2(string input) => Solve(input, false);

    [TestCase(0, 1)]
    [TestCase(1, 2)]
    [TestCase(2, 4)]
    [TestCase(3, 3)]
    [TestCase(4, 36)]
    public void Sample1(int sample, int expected) => Solve1(GetSampleInput(sample)).ShouldBe(expected);

    [TestCase(5, 3)]
    [TestCase(7, 13)]
    [TestCase(8, 227)]
    [TestCase(4, 81)]
    public void Sample2(int sample, int expected) => Solve2(GetSampleInput(sample)).ShouldBe(expected);

    [Test] public void Part1() => Solve1(Input).ShouldBe(746);
    [Test] public void Part2() => Solve2(Input).ShouldBe(1541);
}

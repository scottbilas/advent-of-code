class Day6 : Fixture
{
    (bool loop, IEnumerable<Int2> path) Solve(char[,] grid)
    {
        var pos = grid.Cells().First(c => c.cell == '^').pos;
        var seen = new HashSet<(Int2 pos, Dir dir)>();
        var loop = true;

        for (var dir = N; seen.Add((pos, dir)) && loop;)
        {
            var test = pos + dir.Move;
            if (!grid.HasCoord(test))
                loop = false;
            else if (grid.Get(test) == '#')
                dir = dir.Next4;
            else
                pos = test;
        }

        return (loop, seen.Select(v => v.pos).Distinct());
    }

    char[,] Parse(string input) => input.ToGrid();

    int Solve1(string input) => Solve(Parse(input)).path.Count();

    int Solve2(string input) => With(Parse(input), grid => Solve(grid)
        .path.Skip(1).AsParallel()
        .Count(pos => Solve(grid.Copy().Set(pos, '#')).loop));

    [Test] public void Sample1() => Solve1(SampleInput).ShouldBe(41);
    [Test] public void Sample2() => Solve2(SampleInput).ShouldBe(6);

    [Test] public void Part1() => Solve1(Input).ShouldBe(5461);
    [Test] public void Part2() => Solve2(Input).ShouldBe(1836);
}

class Day4 : Fixture
{
    int Solve1(string input) => With(input.ToGrid(), grid => grid
        .Cells()
        .Sum(cell => All8.Count(dir => Enumerable.Range(0, 4)
            .Select(r => cell.pos + dir.Move * r)
            .Where(grid.HasCoord)
            .Select(p => grid[p.X, p.Y])
            .SequenceEqual(['X', 'M', 'A', 'S']))));

    int Solve2(string input) => With(input.ToGrid(), grid => grid
        .Cells(grid.Bounds().Deflate(1))
        .Where(c => c.cell == 'A')
        .Sum(cell => AllDiag4.Select(d => grid.Get(cell.pos + d.Move)).ToArray() switch
        {
            ['M', 'M', 'S', 'S'] => 1, ['S', 'M', 'M', 'S'] => 1,
            ['S', 'S', 'M', 'M'] => 1, ['M', 'S', 'S', 'M'] => 1,
            _ => 0,
        }));

    [Test] public void Sample1() => Solve1(SampleInput2).ShouldBe(18);
    [Test] public void Sample2() => Solve2(SampleInput2).ShouldBe(9);

    [Test] public void Part1() => Solve1(Input).ShouldBe(2297);
    [Test] public void Part2() => Solve2(Input).ShouldBe(1745);
}

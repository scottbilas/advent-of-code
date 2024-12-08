class Day2 : Fixture
{
    int Solve(string input, Func<IReadOnlyList<int>, bool> isSafe) => input
        .SelectLines()
        .Select(l => l.GetInts())
        .Count(isSafe);

    bool IsSafe1(IReadOnlyList<int> ints) => ints
        .SelectWithPrev()
        .Sum(v => (v.b - v.a) switch
        {
             1 or  2 or  3 =>  1,
            -1 or -2 or -3 => -1,
            _ => 0
        })
        .Abs() == ints.Count-1;

    bool IsSafe2(IReadOnlyList<int> ints) => Enumerable
        .Range(0, ints.Count)
        .Select(skip => ints.SelectWhereIndex(i => i != skip).ToList())
        .Any(IsSafe1);

    int Solve1(string input) => Solve(input, IsSafe1);
    int Solve2(string input) => Solve(input, IsSafe2);

    [Test] public void Sample1() => Solve1(SampleInput).ShouldBe(2);
    [Test] public void Sample2() => Solve2(SampleInput).ShouldBe(4);

    [Test] public void Part1() => Solve1(Input).ShouldBe(224);
    [Test] public void Part2() => Solve2(Input).ShouldBe(293);
}

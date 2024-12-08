class Day8 : Fixture
{
    int Solve(string input, bool single) => input
        // get coords of all multi-site antennas
        .ParseGrid(out var size).Where(c => char.IsAsciiLetterOrDigit(c.value))
        .Swapped().ToMultiDictionary()
        .Values.Where(set => set.Count > 1)
        // make a line in each direction out from each against every other
        .SelectMany(coords => coords.Combinations2())
        .SelectMany(p => new (Int2 seed, Int2 step)[] { (p.a, p.a - p.b), (p.b, p.b - p.a) })
        .Select(p => Generate(p.seed, i => size.HasCoord(i), i => i + p.step))
        // part 1 only wants the first step, part 2 wants origin + every step within bounds
        .SelectMany(line => single ? line.Skip(1).Take(1) : line)
        // result is number of uniques
        .Distinct().Count();

    int Solve1(string input) => Solve(input, true);
    int Solve2(string input) => Solve(input, false);

    [TestCase(1, 2)]
    [TestCase(2, 4)]
    [TestCase(3, 4)]
    [TestCase(0, 14)]
    public void Sample1(int sample, int expected) =>
        Solve1(GetSampleInput(sample)).ShouldBe(expected);

    [TestCase(5, 9)]
    [TestCase(0, 34)]
    public void Sample2(int sample, int expected) =>
        Solve2(GetSampleInput(sample)).ShouldBe(expected);

    [Test] public void Part1() => Solve1(Input).ShouldBe(341);
    [Test] public void Part2() => Solve2(Input).ShouldBe(1134);
}

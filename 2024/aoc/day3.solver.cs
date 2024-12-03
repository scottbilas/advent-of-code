class Day3 : Fixture
{
    int Solve(string input, bool always) => input
        .RegexMatches(@"(do|don't|mul)\((\d+,\d+)?\)")
        .Select(m => m.GroupValues().Skip(1).ToArray())
        .Aggregate((enabled: true, sum: 0), (acc, groups) =>
            (acc, groups) switch
            {
                (_,          ["do",    _]) => (true,   acc.sum),
                (_,          ["don't", _]) => (always, acc.sum),
                ((false, _),            _) => (false,  acc.sum),
                _                          => (true,   acc.sum + groups[1].Ints().Multiply()),
            }).sum;

    int Solve1(string input) => Solve(input, true);
    int Solve2(string input) => Solve(input, false);

    [Test] public void Sample1() => Solve1(SampleInput1).ShouldBe(161);
    [Test] public void Sample2() => Solve2(SampleInput2).ShouldBe(48);

    [Test] public void Part1() => Solve1(Input).ShouldBe(170778545);
    [Test] public void Part2() => Solve2(Input).ShouldBe(82868252);
}

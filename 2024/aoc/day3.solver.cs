class Day3 : Fixture
{
    int Solve1(string input) => input
        .RegexMatches(@"mul\((\d+),(\d+)\)")
        .Sum(m => m.Ints().Multiply());

    int Solve2(string input) => ("do()"+input)
        .Replace('\n',' ').Split("don't()")
        .Sum(s => Solve1(s.RegexMatch("do().*").Value));

    [Test] public void Sample1() => Solve1(SampleInput1).ShouldBe(161);
    [Test] public void Sample2() => Solve2(SampleInput2).ShouldBe(48);

    [Test] public void Part1() => Solve1(Input).ShouldBe(170778545);
    [Test] public void Part2() => Solve2(Input).ShouldBe(82868252);
}

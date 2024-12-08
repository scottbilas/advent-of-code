class Day1 : Fixture
{
    int Solve1(string input) => Enumerable
        .Zip(Parse(input, 1).Ordered(), Parse(input, 2).Ordered()).Sum(v => Math.Abs(v.First - v.Second));

    int Solve2(string input) => Parse(input, 1)
        .GroupJoin(Parse(input, 2), (l, r) => l * r.Count()).Sum();

    IEnumerable<int> Parse(string input, int column) =>
        input.Ints().SelectStride(2, column - 1);

    [Test] public void Sample1() => Solve1(SampleInput).ShouldBe(11);
    [Test] public void Sample2() => Solve2(SampleInput).ShouldBe(31);

    [Test] public void Part1() => Solve1(Input).ShouldBe(2580760);
    [Test] public void Part2() => Solve2(Input).ShouldBe(25358365);
}

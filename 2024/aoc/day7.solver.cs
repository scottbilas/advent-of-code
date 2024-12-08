class Day7 : Fixture
{
    long Solve(string line, bool or)
    {
        IEnumerable<long> Calc(long left, ArraySegment<long> data) =>
            data.Any()
                ? Concat(
                    Calc(left * data[0], data[1..]),
                    Calc(left + data[0], data[1..]),
                    or ? Calc($"{left}{data[0]}".Long(), data[1..]) : [])
                : [left];

        try
        {
            var data = line.GetLongs().ToArray();
            if (Calc(data[1], data[2..]).Any(calc => calc == data[0]))
                return data[0];
        }
        catch (OverflowException) {}
        return 0;
    }

    long Solve1(string input) => input.SelectLines().Sum(l => Solve(l, false));
    long Solve2(string input) => input.SelectLines().AsParallel().Sum(l => Solve(l, true));

    [Test] public void Sample1() => Solve1(SampleInput).ShouldBe(3749);
    [Test] public void Sample2() => Solve2(SampleInput).ShouldBe(11387);

    [Test] public void Part1() => Solve1(Input).ShouldBe(1153997401072);
    [Test] public void Part2() => Solve2(Input).ShouldBe(97902809384118);
}

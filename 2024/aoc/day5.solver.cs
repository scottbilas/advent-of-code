class Day5 : Fixture
{
    int Solve(IReadOnlyList<int> pages, bool ordered, Func<int, int, int> compare)
    {
        var (same, result) = (true, new List<int>());
        foreach (var page in pages)
        {
            var insert = result.Count;
            for (var i = result.Count - 1; i >= 0; --i)
            {
                var rc = compare(result[i], page);
                if (rc < 0)
                {
                    insert = i+1;
                    break;
                }
                if (rc > 0)
                    insert = i;
            }

            same &= insert == result.Count;
            result.Insert(insert, page);
        }

        return same == ordered ? result[result.Count / 2] : 0;
    }

    int Solve(string input, bool ordered) => With(input
        .Sections(
            s => s.Select(l => l.Ints().ToTuple2()).ToDefaultMultiDictionary(),
            s => s.Select(l => l.GetInts())),
        (after, orders) => orders
            .Sum(order => Solve(order, ordered, (a, b) =>
                after[a].Contains(b) ? -1 :
                after[b].Contains(a) ?  1 : 0)));

    [Test] public void Sample1() => Solve(SampleInput, true ).ShouldBe(143);
    [Test] public void Sample2() => Solve(SampleInput, false).ShouldBe(123);

    [Test] public void Part1() => Solve(Input, true ).ShouldBe(5108);
    [Test] public void Part2() => Solve(Input, false).ShouldBe(7380);
}

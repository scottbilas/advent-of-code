class Day9 : Fixture
{
    record struct Entry(int Id, int Len);
    LinkedList<Entry> Parse(string input) =>
        new(input.Select((c, i) => new Entry(i % 2 == 0 ? i / 2 : -1, c.Int())));

    long Checksum(IEnumerable<Entry> entries)
    {
        var (pos, checksum) = (0, 0L);
        foreach (var entry in entries)
        {
            if (entry.Id >= 0)
                checksum += Enumerable.Range(0, entry.Len).Sum(i => (long)(pos+i) * entry.Id);
            pos += entry.Len;
        }
        return checksum;
    }

    long Solve1(string input)
    {
        var entries = Parse(input);
        foreach (var entry in entries.Nodes().Where(e => e.Value.Id < 0))
        {
            ref var cur = ref entry.ValueRef;
            ref var end = ref entries.Last!.ValueRef;
            cur.Id = end.Id;

            if (end.Len >= cur.Len)
                end.Len -= cur.Len;
            else
            {
                entries.AddAfter(entry, new Entry(-1, cur.Len - end.Len));
                cur.Len = end.Len;

                entries.RemoveLast();
                entries.RemoveLast();
            }
        }

        return Checksum(entries);
    }

    long Solve2(string input)
    {
        var entries = Parse(input);
        foreach (var move in entries.NodesReverse().Where(e => e.Value.Id >= 0))
        foreach (var seek in entries.Nodes())
        {
            if (seek == move)
                break;
            if (seek.Value.Id >= 0 || seek.Value.Len < move.Value.Len)
                continue;

            if (seek.Value.Len > move.Value.Len)
                entries.AddAfter(seek, new Entry(-1, seek.Value.Len - move.Value.Len));
            seek.Value = move.Value;
            move.Value = move.Value with { Id = -1 };
            break;
        }

        return Checksum(entries);
    }

    new const string SampleInput = "2333133121414131402";

    [TestCase("12345", 60)]
    [TestCase(SampleInput, 1928)]
    public void Sample1(string input, int expected) => Solve1(input).ShouldBe(expected);
    [TestCase(SampleInput, 2858)]
    public void Sample2(string input, int expected) => Solve2(input).ShouldBe(expected);

    [Test] public void Part1() => Solve1(Input).ShouldBe(6340197768906L);
    [Test] public void Part2() => Solve2(Input).ShouldBe(6363913128533L);
}

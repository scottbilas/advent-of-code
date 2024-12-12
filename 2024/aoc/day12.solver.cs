using NUnit.Framework.Internal;
using static Dir;

class Day12 : Fixture
{
    class Plot(char plant)
    {
        public char Plant => plant;
        public int Id = -1;
        public readonly Plot?[] Links = new Plot?[4];
    }

    IEnumerable<IReadOnlyList<Plot>> GetRegions(string input)
    {
        var grid = input.ToGrid(c => new Plot(c));

        foreach (var (pos, plot) in grid.Cells())
        foreach (var dir in All4)
        {
            var adj = grid.SafeGet(dir.Move + pos);
            if (adj?.Plant == plot.Plant)
                plot.Links[dir.Index4] = adj;
        }

        var nextId = 0;
        foreach (var plot in grid)
        {
            if (plot.Id != -1)
                continue;

            var id = nextId++;
            var region = new List<Plot>();

            void Update(Plot p)
            {
                p.Id = id;
                region.Add(p);
                foreach (var link in p.Links.Where(l => l?.Id == -1))
                    Update(link!);
            }
            Update(plot);

            yield return region;
        }
    }

    int Solve(string input, Func<Plot, int> calcEdges) =>
        GetRegions(input).Sum(region => region.Count * region.Sum(calcEdges));

    int Solve1(string input) => Solve(input, plot => plot.Links.Count(l => l == null));

    int Solve2(string input) => Solve(input, plot => Enumerable.Range(0, 4).Count(i =>
    {
        var (a, b) = (plot.Links[i], plot.Links[(i + 1) % 4]);
        return a == null && b == null || a != null && b != null && b.Links[i] == null;
    }));

    [TestCase(0, 140)]
    [TestCase(2, 772)]
    [TestCase(3, 1930)]
    public void Sample1(int sample, int expected) => Solve1(GetSampleInput(sample)).ShouldBe(expected);

    [TestCase(0, 80)]
    [TestCase(2, 436)]
    [TestCase(5, 236)]
    [TestCase(6, 368)]
    [TestCase(3, 1206)]
    public void Sample2(int sample, int expected) => Solve2(GetSampleInput(sample)).ShouldBe(expected);

    [Test] public void Part1() => Solve1(Input).ShouldBe(1361494);
    [Test] public void Part2() => Solve2(Input).ShouldBe(830516);
}


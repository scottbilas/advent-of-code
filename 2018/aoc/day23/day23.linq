<Query Kind="Statements">
  <Reference Relative="..\..\libaoc\bin\Debug\net472\libaoc.dll">C:\proj\advent-of-code\libaoc\bin\Debug\net472\libaoc.dll</Reference>
  <NuGetReference>morelinq</NuGetReference>
  <NuGetReference>Shouldly</NuGetReference>
  <NuGetReference>YC.QuickGraph</NuGetReference>
  <Namespace>AoC</Namespace>
  <Namespace>JetBrains.Annotations</Namespace>
  <Namespace>MoreLinq.Extensions</Namespace>
  <Namespace>QuickGraph</Namespace>
  <Namespace>QuickGraph.Algorithms</Namespace>
  <Namespace>Shouldly</Namespace>
  <Namespace>System</Namespace>
  <Namespace>System.Collections</Namespace>
  <Namespace>System.Collections.Generic</Namespace>
  <Namespace>System.Drawing</Namespace>
  <Namespace>System.Linq</Namespace>
  <Namespace>System.Text</Namespace>
  <Namespace>NiceIO</Namespace>
</Query>

NPath scriptDir = new NPath(Util.CurrentQueryPath).Parent;

Part1(@"
    pos=<0,0,0>, r=4
    pos=<1,0,0>, r=1
    pos=<4,0,0>, r=3
    pos=<0,2,0>, r=1
    pos=<0,5,0>, r=3
    pos=<0,0,3>, r=1
    pos=<1,1,1>, r=1
    pos=<1,1,2>, r=1
    pos=<1,3,1>, r=1
    ").ShouldBe(7);

Part2(@"
    pos=<10,12,12>, r=2
    pos=<12,14,12>, r=2
    pos=<16,12,12>, r=4
    pos=<14,14,14>, r=6
    pos=<50,50,50>, r=200
    pos=<10,10,10>, r=5
    ").ShouldBe(36);

Part1(scriptDir.Combine("input.txt").ReadAllText()).Dump().ShouldBe(319);
Part2(scriptDir.Combine("input.txt").ReadAllText()).Dump().ShouldBe(129293598);

IEnumerable<(int id, Int3 pos, int r)> Parse(string botText)
    => botText
        .SelectInts()
        .Batch4()
        .Select((ints, id) => (id, pos: new Int3(ints.Item1, ints.Item2, ints.Item3), r: ints.Item4));

int Part1(string botText)
{
    var bots = Parse(botText).ToList();    
    var strongest = bots.MaxBy(b => b.r).First();
    return bots.Count(b => b.pos.ManhattanDistance(strongest.pos) <= strongest.r);
}

int Part2(string botText)
{
    var bots = Parse(botText).ToList();
    var bounds = bots.Select(b => b.pos).CalcAABB();

    for (;;)
    {
        var selected = bounds.Corners
            .Select(corner =>
            {
                var quadrant = AABB.FromPoints(corner, bounds.Center);
                var inside = bots.Count(bot => quadrant.Corners.Any(c => bot.pos.ManhattanDistance(c) <= bot.r));
                return (quadrant, inside);
            })
            .MaxBy(v => v.inside)
            .First();

        if (selected.quadrant.Size == Int3.One)
        {
            var max = selected.quadrant.Corners
                .Select(corner => (corner, count: bots.Count(bot => bot.pos.ManhattanDistance(corner) <= bot.r)))
                .MaxBy(v => v.count)
                .First();
            return max.corner.ManhattanDistance(Int3.Zero);
        }

        bounds = selected.quadrant;
    }
}

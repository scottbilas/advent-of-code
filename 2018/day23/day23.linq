<Query Kind="Statements">
  <Reference Relative="..\..\libaoc\bin\Debug\netstandard2.0\libaoc.dll">C:\proj\advent-of-code\libaoc\bin\Debug\netstandard2.0\libaoc.dll</Reference>
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
</Query>

string scriptDir = Path.GetDirectoryName(Util.CurrentQueryPath);

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

Part1(File.ReadAllText($"{scriptDir}/input.txt")).Dump().ShouldBe(319);

int ManhattanDistance(in Point3 a, in Point3 b) =>
     Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y) + Math.Abs(a.Z - b.Z);

IEnumerable<(int id, Point3 pos, int r)> Parse(string botText)
    => botText
        .SelectInts()
        .Batch4()
        .Select((ints, id) => (id, pos: new Point3(ints.Item1, ints.Item2, ints.Item3), r: ints.Item4));

int Part1(string botText)
{
    var bots = Parse(botText).ToList();    
    var strongest = bots.MaxBy(b => b.r).First();
    return bots.Count(b => ManhattanDistance(b.pos, strongest.pos) <= strongest.r);
}

int Part2(string botText)
{
    return 0;
}

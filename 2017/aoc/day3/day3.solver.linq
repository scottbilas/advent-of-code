<Query Kind="Program">
  <Reference Relative="..\..\..\temp\bin\libaoc2017\Debug\netstandard2.1\libaoc2017.dll">C:\proj\advent-of-code\temp\bin\libaoc2017\Debug\netstandard2.1\libaoc2017.dll</Reference>
  <Reference Relative="..\..\..\temp\bin\libaoc2017\Debug\netstandard2.1\libutils.dll">C:\proj\advent-of-code\temp\bin\libaoc2017\Debug\netstandard2.1\libutils.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\netstandard.dll</Reference>
  <NuGetReference>Combinatorics</NuGetReference>
  <NuGetReference>morelinq</NuGetReference>
  <NuGetReference>RoyT.AStar</NuGetReference>
  <NuGetReference>Shouldly</NuGetReference>
  <NuGetReference>System.Interactive</NuGetReference>
  <NuGetReference>YC.QuickGraph</NuGetReference>
  <Namespace>Aoc2017</Namespace>
  <Namespace>Combinatorics.Collections</Namespace>
  <Namespace>JetBrains.Annotations</Namespace>
  <Namespace>QuickGraph</Namespace>
  <Namespace>QuickGraph.Algorithms</Namespace>
  <Namespace>QuickGraph.Graphviz</Namespace>
  <Namespace>RoyT.AStar</Namespace>
  <Namespace>Shouldly</Namespace>
  <Namespace>static Aoc2017.MiscStatics</Namespace>
  <Namespace>static Aoc2017.Utils</Namespace>
  <Namespace>static System.Linq.Enumerable</Namespace>
  <Namespace>static System.Linq.EnumerableEx</Namespace>
  <Namespace>System.Drawing</Namespace>
  <Namespace>System.Numerics</Namespace>
  <Namespace>Unity.Coding.Editor</Namespace>
  <Namespace>Unity.Coding.Utils</Namespace>
</Query>

static readonly NPath ScriptDir = Util.CurrentQueryPath.ToNPath().Parent;

void Main()
{
    var inputPath = ScriptDir.Combine($"{ScriptDir.FileName}.input.txt");

    var input = inputPath.ReadAllInts().First();

// --- PART 1 ---

    // *SAMPLES*

    Solve1(1).ShouldBe(0);
    Solve1(12).ShouldBe(3);
    Solve1(23).ShouldBe(2);
    Solve1(1024).ShouldBe(31);

    // *PROBLEM*

    Solve1(input).Dump().ShouldBe(419);

// --- PART 2 ---

    // *SAMPLES*

    var match = Arr(1, 1, 2, 4, 5, 10, 11, 23, 25, 26, 54, 57, 59, 122, 133, 142, 147, 304, 330, 351, 362, 747, 806);
    Select().Take(match.Length).ShouldBe(match);

    // *PROBLEM*

    Solve2(input).Dump().ShouldBe(295229);
}

int Solve1(int pos)
{
    if (pos == 1)
        return 0;

    var ring = (int)Math.Ceiling(Math.Sqrt(pos));
    if (ring % 2 == 0) ++ring;

    var xoff =
        ((ring * ring) - pos)   // offset from bottom right
        % (ring - 1)            // normalized to a side
        - (ring / 2);           // offset from center of side
    return Math.Abs(xoff)       // flip if negative
        + ring / 2;             // add number of steps outward
}

IEnumerable<int> Select()
{
    var grid = new Dictionary<Int2, int>();

    int Add(Int2 pos)
    {
        var sum = pos.SelectAdjacentWithDiagonals(p => grid.TryGetValue(p, out var v) ? v : 0).Sum();
        grid.Add(pos, sum);
        return sum;
    }

    var pos = Int2.Zero;
    grid.Add(pos, 1);
    yield return 1;
    
    for (var step = 1;; ++step)
    {
        var bounds = RectInt2.FromPosSize(-step, -step, step * 2, step * 2);

        ++pos.X;
        yield return Add(pos);

        while (pos.Y != bounds.Top)
        {
            --pos.Y;
            yield return Add(pos);
        }

        do
        {
            --pos.X;
            yield return Add(pos);
        }
        while (pos.X != bounds.Left);

        do
        {
            ++pos.Y;
            yield return Add(pos);
        }
        while (pos.Y != bounds.Bottom);

        do
        {
            ++pos.X;
            yield return Add(pos);
        }
        while (pos.X != bounds.Right);

        while (pos.Y != step)
        {
            --pos.Y;
            yield return Add(pos);
        }
    }
}

int Solve2(int input) =>
    Select().SkipWhile(i => i < input).First();
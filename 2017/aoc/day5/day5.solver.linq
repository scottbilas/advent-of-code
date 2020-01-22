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
    var input = inputPath.ReadAllInts();

// --- PART 1 ---

    // *SAMPLES*

    Solve1(Arr(0, 3, 0, 1, -3)).ShouldBe(5);

    // *PROBLEM*

    Solve1(input).Dump().ShouldBe(343467);

// --- PART 2 ---

    // *SAMPLES*

    Solve2(Arr(0, 3, 0, 1, -3)).ShouldBe(10);

    // *PROBLEM*

    Solve2(input).Dump().ShouldBe(24774780);
}

int Solve(IEnumerable<int> jumps, int minOffset)
{
    var mem = jumps.ToArray();
    var steps = 0;

    for (var ip = 0; mem.IsValidIndex(ip); ++steps)
    {
        var newIp = ip + mem[ip];
        mem[ip] += mem[ip] >= minOffset ? -1 : 1;
        ip = newIp;
    }

    return steps;
}

int Solve1(IEnumerable<int> jumps) => Solve(jumps, 1000);
int Solve2(IEnumerable<int> jumps) => Solve(jumps, 3);

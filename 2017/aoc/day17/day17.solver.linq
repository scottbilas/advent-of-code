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
  <Namespace>System.Diagnostics.CodeAnalysis</Namespace>
  <Namespace>System.Globalization</Namespace>
</Query>

static readonly NPath ScriptDir = Util.CurrentQueryPath.ToNPath().Parent;

void Main()
{
    var inputPath = ScriptDir.Combine($"{ScriptDir.FileName}.input.txt");
    var inputCount = inputPath.ReadAllInts().Single();

// --- PART 1 ---

    // *SAMPLES*

    Solve1(3, 1).ShouldBe(0);
    Solve1(3, 2).ShouldBe(1);
    Solve1(3, 3).ShouldBe(1);
    Solve1(3, 4).ShouldBe(3);
    Solve1(3, 5).ShouldBe(2);
    Solve1(3, 6).ShouldBe(1);
    Solve1(3, 7).ShouldBe(2);
    Solve1(3, 8).ShouldBe(6);
    Solve1(3, 9).ShouldBe(5);
    
    Solve1(3, 2017).ShouldBe(638);

    // *PROBLEM*

    Solve1(inputCount, 2017).Dump().ShouldBe(1173);

// --- PART 2 ---

    Solve2(inputCount, 50000000).Dump().ShouldBe(1930815);
}

int Solve1(int step, int loops)
{
    var ring = new List<int> { 0 };
    var pos = 0;
    
    for (var i = 0; i < loops; ++i)
    {
        pos = (pos + step) % ring.Count + 1;
        ring.Insert(pos, i + 1);
    }
    
    return ring[(pos + 1) % ring.Count];
}

int Solve2(int step, int loops)
{
    var (pos, count, target) = (0, 1, 0);

    for (var i = 0; i < loops; ++i)
    {
        pos = (pos + step) % count++ + 1;
        if (pos == 1)
            target = i + 1;
    }

    return target;
}

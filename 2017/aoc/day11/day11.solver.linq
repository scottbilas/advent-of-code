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
</Query>

static readonly NPath ScriptDir = Util.CurrentQueryPath.ToNPath().Parent;

void Main()
{
    var inputPath = ScriptDir.Combine($"{ScriptDir.FileName}.input.txt");
    var input = inputPath.ReadAllText();

// --- PART 1 ---

    // *SAMPLES*

    Solve1("ne,ne,ne").ShouldBe(3);
    Solve1("ne,ne,sw,sw").ShouldBe(0);
    Solve1("ne,ne,s,s").ShouldBe(2);
    Solve1("se,sw,se,sw,sw").ShouldBe(3);

    // *PROBLEM*

    Solve1(input).Dump().ShouldBe(761);

// --- PART 2 ---

    // *PROBLEM*

    Solve2(input).Dump();//.ShouldBe();
}

(int final, int max) Solve(string steps)
{
    // doubled height style hex grid (https://www.redblobgames.com/grids/hexagons/#coordinates)
    
    var moves = Arr<(string, Func<Int2, Int2>)>(
        ("n" , p => p + new Int2( 0, -2)),
        ("ne", p => p + new Int2( 1, -1)),
        ("se", p => p + new Int2( 1,  1)),
        ("s" , p => p + new Int2( 0,  2)),
        ("sw", p => p + new Int2(-1,  1)),
        ("nw", p => p + new Int2(-1, -1))
        ).ToDictionary();
    
    int Dist(Int2 p) => With(p.Abs(), p => p.X + Math.Max(0, (p.Y - p.X) / 2));

    int max = 0;
    var pos = Int2.Zero;
    
    foreach (var step in steps.SelectWords())
    {
        pos = moves[step](pos);
        max = Math.Max(max, Dist(pos));
    }
    
    return (Dist(pos), max);
}

int Solve1(string steps) => Solve(steps).final;
int Solve2(string steps) => Solve(steps).max;

<Query Kind="Program">
  <Reference Relative="..\..\..\temp\bin\libaoc2017\Debug\netstandard2.1\libaoc2017.dll">C:\proj\advent-of-code\temp\bin\libaoc2017\Debug\netstandard2.1\libaoc2017.dll</Reference>
  <Reference Relative="..\..\..\temp\bin\libaoc2017\Debug\netstandard2.1\libutils.dll">C:\proj\advent-of-code\temp\bin\libaoc2017\Debug\netstandard2.1\libutils.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\netstandard.dll</Reference>
  <NuGetReference>Combinatorics</NuGetReference>
  <NuGetReference>morelinq</NuGetReference>
  <NuGetReference>RoyT.AStar</NuGetReference>
  <NuGetReference>Shouldly</NuGetReference>
  <NuGetReference>System.Interactive</NuGetReference>
  <Namespace>Aoc2017</Namespace>
  <Namespace>Combinatorics.Collections</Namespace>
  <Namespace>JetBrains.Annotations</Namespace>
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
  <Namespace>QuickGraph</Namespace>
  <Namespace>QuickGraph.Graphviz</Namespace>
  <Namespace>QuickGraph.Algorithms</Namespace>
</Query>

static readonly NPath ScriptDir = Util.CurrentQueryPath.ToNPath().Parent;

void Main()
{
    var inputPath = ScriptDir.Combine($"{ScriptDir.FileName}.input.txt");

    var input = Parse(inputPath.ReadAllText());

// --- PART 1 ---

    // *SAMPLES*

    Solve1(Parse(@"
        5 1 9 5
        7 5 3
        2 4 6 8
        ")).ShouldBe(18);

    // *PROBLEM*

    Solve1(input).Dump().ShouldBe(45351);

// --- PART 2 ---

    // *SAMPLES*

    Solve2(Parse(@"
        5 9 2 8
        9 4 7 3
        3 8 6 5    
        ")).ShouldBe(9);

    // *PROBLEM*
    
    Solve2(input).Dump().ShouldBe(275);
}

int[][] Parse(string text) =>
    text.Lines().Select(l => l.Ints()).ToArray();

int Solve(int[][] cells, Func<int[], int> solver) =>
    cells.Select(l => l.Ordered().ToArray()).Sum(solver);

int Solve1(int[][] cells) =>
    Solve(cells, line => line[^1] - line[0]);

int Solve2(int[][] cells) =>
    Solve(cells, line => 
    {
        for (var i = 1;; ++i)
            foreach (var c in line[0..(i-1)].Where(c => line[i] % c == 0))
                return line[i] / c;
    });

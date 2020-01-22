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

    var input = inputPath.ReadAllText().Trim();

// --- PART 1 ---

    // *SAMPLES*

    Solve1("1122").ShouldBe(3);
    Solve1("1111").ShouldBe(4);
    Solve1("1234").ShouldBe(0);
    Solve1("91212129").ShouldBe(9);

    // *PROBLEM*

    Solve1(input).Dump().ShouldBe(1171);

// --- PART 2 ---

    // *SAMPLES*

    Solve2("1212").ShouldBe(6);
    Solve2("1221").ShouldBe(0);
    Solve2("123425").ShouldBe(4);
    Solve2("123123").ShouldBe(12);
    Solve2("12131415").ShouldBe(4);

    // *PROBLEM*
    
    Solve2(input).Dump().ShouldBe(1024);
}

int Solve(string text, int offset) =>
    text.Where((c, i) => c == text[(i + offset) % text.Length]).Sum(c => c - '0');

int Solve1(string text) => Solve(text, 1);
int Solve2(string text) => Solve(text, text.Length / 2);
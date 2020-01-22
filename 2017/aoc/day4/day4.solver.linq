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

    var input = inputPath.ReadAllLines();

// --- PART 1 ---

    // *SAMPLES*

    IsValid1("aa bb cc dd ee").ShouldBeTrue();
    IsValid1("aa bb cc dd aa").ShouldBeFalse();
    IsValid1("aa bb cc dd aaa").ShouldBeTrue();

    // *PROBLEM*

    Solve1(input).Dump().ShouldBe(386);

// --- PART 2 ---

    // *SAMPLES*

    IsValid2("abcde fghij").ShouldBeTrue();
    IsValid2("abcde xyz ecdab").ShouldBeFalse();
    IsValid2("a ab abc abd abf abj").ShouldBeTrue();
    IsValid2("iiii oiii ooii oooi oooo").ShouldBeTrue();
    IsValid2("oiii ioii iioi iiio").ShouldBeFalse();

    // *PROBLEM*

    Solve2(input).Dump().ShouldBe(208);
}

bool IsValid1(string pass) => pass.SelectWords().IsDistinct();
int Solve1(string[] input) => input.Count(IsValid1);

bool IsValid2(string pass) =>
    pass.SelectWords().Select(w => w.Ordered().ToStringFromChars()).IsDistinct();

int Solve2(string[] input) => input.Count(IsValid2);
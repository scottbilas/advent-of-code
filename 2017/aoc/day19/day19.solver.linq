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
    var input = inputPath.ReadAllText();

// --- PART 1 ---

    // *SAMPLES*

    var sample = @"
         |          
         |  +--+    
         A  |  C    
     F---|----E|--+ 
         |  |  |  D 
         +B-+  +--+";
    Solve1(sample).ShouldBe("ABCDEF");

    // *PROBLEM*

    Solve1(input).Dump().ShouldBe("XYFDJNRCQA");

// --- PART 2 ---

    // *SAMPLES*

    Solve2(sample).ShouldBe(38);

    // *PROBLEM*

    Solve2(input).Dump().ShouldBe(17450);
}

IEnumerable<char> Solve(string gridText)
{
    var grid = gridText.ParseGrid(' ', 1);
    var pos = grid.SelectCells().First(c => c.cell == '|').pos;

    for (var dir = Dir.S;; pos += dir.GetMove())
    {
        var c = grid.GetAt(pos);
        if (c == '+')
            dir = dir.GetContinues().Single(d => grid.GetAt(pos, d) != ' ');
        else if (c == ' ')
            yield break;

        yield return c;
    }
}

string Solve1(string gridText) =>
    Solve(gridText).Where(c => c.IsAsciiLetter()).ToStringFromChars();

int Solve2(string gridText) =>
    Solve(gridText).Count();

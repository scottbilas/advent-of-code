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
    var (inputA, inputB) = inputPath.ReadAllInts().Batch2().Single();

// --- PART 1 ---

    // *SAMPLES*

    Solve1(65, 8921, 5).ShouldBe(1);
    Solve1(65, 8921).ShouldBe(588);

    // *PROBLEM*

    Solve1(inputA, inputB).Dump().ShouldBe(626);

// --- PART 2 ---

    // *SAMPLES*

    Solve2(65, 8921, 1056).ShouldBe(1);
    Solve2(65, 8921).ShouldBe(309);

    // *PROBLEM*

    Solve2(inputA, inputB).Dump().ShouldBe(306);
}

const int factorA = 16807, factorB = 48271, div = 2147483647;

int Solve1(long a, long b, int samples = 40*1000*1000)
{
    var matches = 0;
    for (var i = 0; i < samples; ++i)
    {
        a = (a * factorA) % div;
        b = (b * factorB) % div;
        
        if ((a & 0xffff) == (b & 0xffff))
            ++matches;
    }
    
    return matches;
}

int Solve2(long a, long b, int samples = 5 * 1000 * 1000)
{
    long Next(long cur, int factor, int mult)
    {
        for (;;)
        {
            cur = (cur * factor) % div;
            if (cur % mult == 0)
                return cur;
        }
    }

    var matches = 0;
    for (var i = 0; i < samples; ++i)
    {
        a = Next(a, factorA, 4);
        b = Next(b, factorB, 8);

        if ((a & 0xffff) == (b & 0xffff))
            ++matches;
    }

    return matches;
}

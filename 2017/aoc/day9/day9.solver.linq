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

    Solve1("<>").ShouldBe(0);
    Solve1("<random characters>").ShouldBe(0);
    Solve1("<<<<>").ShouldBe(0);
    Solve1("<{!>}>").ShouldBe(0);
    Solve1("<!!>").ShouldBe(0);
    Solve1("<!!!>>").ShouldBe(0);
    Solve1("<{o\"i!a,<{i<a>").ShouldBe(0);

    Solve1("{}").ShouldBe(1);
    Solve1("{{{}}}").ShouldBe(1 + 2 + 3);
    Solve1("{{},{}}").ShouldBe(1 + 2 + 2);
    Solve1("{{{},{},{{}}}}").ShouldBe(1 + 2 + 3 + 3 + 3 + 4);
    Solve1("{<a>,<a>,<a>,<a>}").ShouldBe(1);
    Solve1("{{<ab>},{<ab>},{<ab>},{<ab>}}").ShouldBe(1 + 2 + 2 + 2 + 2);
    Solve1("{{<!!>},{<!!>},{<!!>},{<!!>}}").ShouldBe(1 + 2 + 2 + 2 + 2);
    Solve1("{{<a!>},{<a!>},{<a!>},{<ab>}}").ShouldBe(1 + 2);

    // *PROBLEM*

    Solve1(input).Dump().ShouldBe(12396);

// --- PART 2 ---

    // *SAMPLES*

    Solve2("<>").ShouldBe(0);
    Solve2("<random characters>").ShouldBe(17);
    Solve2("<<<<>").ShouldBe(3);
    Solve2("<{!>}>").ShouldBe(2);
    Solve2("<!!>").ShouldBe(0);
    Solve2("<!!!>>").ShouldBe(0);
    Solve2("<{o\"i!a,<{i<a>").ShouldBe(10);

    // *PROBLEM*

    Solve2(input).Dump().ShouldBe(6346);
}

(int score, int garbage) Solve(string text)
{
    var (depth, score, garbage) = (0, 0, 0);
    
    for (var i = 0; i != text.Length;)
    {
        char Next()
        {
            for (;; ++i)
            {
                var c = text[i++];
                if (c != '!')
                    return c;
            }
        }

        switch (Next())
        {
            case '<': while (Next() != '>') ++garbage; break;
            case '{': score += ++depth; break;
            case '}': --depth; break;
        }
    }

    return (score, garbage);
}

int Solve1(string text) => Solve(text).score;
int Solve2(string text) => Solve(text).garbage;

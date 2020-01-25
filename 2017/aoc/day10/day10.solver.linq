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

// --- PART 1 ---

    // *SAMPLES*

    Solve1(5, Arr(3, 4, 1, 5)).ShouldBe(12);

    // *PROBLEM*

    Solve1(256, inputPath.ReadAllInts()).Dump().ShouldBe(29240);

// --- PART 2 ---

    // *SAMPLES*

    Solve2("")        .ShouldBe("a2582a3a0e66e6e86e3812dcb672a272");
    Solve2("AoC 2017").ShouldBe("33efeb34ea91902bb2f59c9920caa6cd");
    Solve2("1,2,3")   .ShouldBe("3efbe78a8d82f29979031a4aa0b16a9d");
    Solve2("1,2,4")   .ShouldBe("63960835bcdc130f0b66d7ff4f6a5a8e");

    // *PROBLEM*

    Solve2(inputPath.ReadAllText().Trim()).Dump().ShouldBe("4db3799145278dc9f73dcdbc680bd53d");
}

int Solve1(int count, IEnumerable<int> spans)
{
    var elements = new RingArray<int>(Range(0, count));
    var (pos, skip) = (0, 0);
    
    foreach (var span in spans)
    {
        elements.Reverse(pos, span);
        pos = pos + span + skip++;
    }
    
    return elements[0] * elements[1];
}

string Solve2(string text) => Solve2(text.ToCharArray().Select(c => (int)c));

string Solve2(IEnumerable<int> chars)
{
    var elements = new RingArray<int>(Range(0, 256));
    var (pos, skip) = (0, 0);

    for (var i = 0; i < 64; ++i)
    {
        foreach (var span in chars.Concat(Arr(17, 31, 73, 47, 23)))
        {
            elements.Reverse(pos, span);
            pos = pos + span + skip++;
        }
    }

    return elements
        .Batch(16, l => l.Aggregate(0, (a, b) => a ^ b))
        .Select(i => $"{i:x2}")
        .StringJoin();
}

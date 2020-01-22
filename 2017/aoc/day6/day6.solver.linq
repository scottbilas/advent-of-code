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
    var input = inputPath.ReadAllInts();

// --- PART 1 ---

    // *SAMPLES*

    var sample = Arr(0, 2, 7, 0);
    Solve1(sample).ShouldBe(5);

    // *PROBLEM*

    Solve1(input).Dump().ShouldBe(11137);

// --- PART 2 ---

    // *SAMPLES*

    Solve2(sample).ShouldBe(4);

    // *PROBLEM*

    Solve2(input).Dump().ShouldBe(1037);
}

int Find(int[] mem, Predicate<int[]> test)
{
    for (var steps = 1; ; ++steps)
    {
        var max = mem.WithIndices().MaxBy(v => v.value).First();
        mem[max.index] = 0;

        var rem = max.value % mem.Length;
        for (var i = 0; i < mem.Length; ++i)
            mem[(max.index + i + 1) % mem.Length] += max.value / mem.Length + (rem-- > 0 ? 1 : 0);

        if (test(mem))
            return steps;
    }
}

(int[] mem, int steps) Sim(IEnumerable<int> counts)
{
    var seen = new HashSet<int[]>(Comparers.Array<int>());
    var mem = counts.ToArray();
    var steps = Find(mem, m => !seen.Add(m.ToArray()));
    
    return (mem, steps);
}

int Solve1(IEnumerable<int> counts) =>
    Sim(counts).steps;

int Solve2(IEnumerable<int> counts) =>
    With(Sim(counts).mem, target => Find(target.ToArray(), m => Comparers.ArrayEquals(m, target)));

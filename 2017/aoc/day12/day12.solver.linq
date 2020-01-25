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

    var sample = @"
        0 <-> 2
        1 <-> 1
        2 <-> 0, 3, 4
        3 <-> 2, 4
        4 <-> 2, 3, 6
        5 <-> 6
        6 <-> 4, 5";
    Solve1(sample).ShouldBe(6);

    // *PROBLEM*

    Solve1(input).Dump().ShouldBe(175);

// --- PART 2 ---

    // *SAMPLES*

    Solve2(sample).ShouldBe(2);

    // *PROBLEM*

    Solve2(input).Dump().ShouldBe(213);
}

Dictionary<int, int[]> Parse(string graphText) => graphText.Trim()
    .SelectLines().Select(l => l.Ints())
    .ToDictionary(l => l[0], l => l.Skip(1).ToArray());

IReadOnlyCollection<T> Gather<T>(Dictionary<T, T[]> graph, T node)
{
    var visited = new HashSet<T>();
    
    void Gather(T node)
    {
        if (visited.Add(node))
            graph[node].ForEach(Gather);
    }
    
    Gather(node);
    return visited;
}

int Solve1(string graphText) =>
    Gather(Parse(graphText), 0).Count;

int Solve2(string graphText)
{
    var groups = 0;
    for (var graph = Parse(graphText); graph.Any(); ++groups)
        graph.RemoveRange(Gather(graph, graph.Keys.First()));

    return groups;
}

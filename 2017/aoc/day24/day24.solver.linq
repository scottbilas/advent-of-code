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

    var sample = @"0/2 2/2 2/3 3/4 3/5 0/1 10/1 9/10";
    Solve1(sample).ShouldBe(31);

    // *PROBLEM*

    Solve1(input).Dump().ShouldBe(1868);

// --- PART 2 ---

    // *SAMPLES*

    Solve2(sample).ShouldBe(19);

    // *PROBLEM*

    Solve2(input).Dump().ShouldBe(1841);
}

void Solve(string componentsText, Action<int, int> onNode)
{
    var edgesDict = new AutoDictionary<int, List<(int, int)>>(_ => new List<(int, int)>());

    var nextId = 0;
    foreach (var component in componentsText.SelectInts().Batch2())
    {
        edgesDict[component.a].Add((component.b, nextId));
        edgesDict[component.b].Add((component.a, nextId));
        nextId++;
    }

    var edges = new (int pins, int id)[edgesDict.Keys.Max() + 1][];
    foreach (var item in edgesDict)
        edges[item.Key] = item.Value.ToArray();

    var used = new bool[nextId];

    void Walk(int str, int len, int pins)
    {
        onNode(str, len);

        foreach (var other in edges[pins].Where(e => !used[e.id]))
        {
            used[other.id] = true;
            Walk(str + pins + other.pins, len + 1, other.pins);
            used[other.id] = false;
        }
    }

    Walk(0, 0, 0);
}

int Solve1(string componentsText)
{
    var max = 0;
    Solve(componentsText, (str, _) => Utils.Maximize(ref max, str));
    return max;
}

int Solve2(string componentsText)
{
    var max = (len: 0, str: 0);
    Solve(componentsText, (str, len) =>
    {
        if (len > max.len)
            max = (len, str);
        else if (len == max.len && str > max.str)
            max.str = str;
    });
    return max.str;
}

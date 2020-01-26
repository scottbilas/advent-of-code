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
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

static readonly NPath ScriptDir = Util.CurrentQueryPath.ToNPath().Parent;

void Main()
{
    var inputPath = ScriptDir.Combine($"{ScriptDir.FileName}.input.txt");
    var inputText = inputPath.ReadAllText();

// --- PART 1 ---

    // *SAMPLES*

    var sample = @"
        0: 3
        1: 2
        4: 4
        6: 4";
    Solve1(sample).ShouldBe(24);

    // *PROBLEM*

    Solve1(inputText).Dump().ShouldBe(1840);

// --- PART 2 ---

    // *SAMPLES*

    Solve2(sample).ShouldBe(10);

    // *PROBLEM*

    Solve2(inputText).Dump().ShouldBe(3850260);
}

int[] Parse(string specText)
{
    var specs = specText.SelectInts().Batch2().ToDictionary();
    var ranges = new int[specs.Keys.Max() + 1];
    specs.ForEach(kv => ranges[kv.Key] = kv.Value);
    return ranges;
}

bool Solve(int[] ranges, int start, Action<int, int> onScanned)
{
    for (var pos = 0; pos < ranges.Length; ++pos)
    {
        var range = ranges[pos];
        if (range != 0 && Utils.BounceIndex(start + pos, range) == 0)
        {
            if (onScanned == null)
                return false;
            onScanned(pos, range);
        }
    }
    return true;
}

int Solve1(string specText)
{
    var severity = 0;
    Solve(Parse(specText), 0, (depth, range) => severity += depth * range);
    return severity;
}

int Solve2(string specText)
{
    var ranges = Parse(specText);
    for (var start = 0; ; ++start)
        if (Solve(ranges, start, null))
            return start;
}

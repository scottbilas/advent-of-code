<Query Kind="Program">
  <Reference Relative="..\..\..\temp\bin\libaoc2019\Debug\netstandard2.1\libaoc2019.dll">C:\proj\advent-of-code\temp\bin\aoc2019\Debug\netstandard2.1\libaoc2019.dll</Reference>
  <Reference Relative="..\..\..\temp\bin\libaoc2019\Debug\netstandard2.1\libutils.dll">C:\proj\advent-of-code\temp\bin\aoc2019\Debug\netstandard2.1\libutils.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\netstandard.dll</Reference>
  <NuGetReference>morelinq</NuGetReference>
  <NuGetReference>Shouldly</NuGetReference>
  <NuGetReference>System.Interactive</NuGetReference>
  <Namespace>Aoc2019</Namespace>
  <Namespace>JetBrains.Annotations</Namespace>
  <Namespace>MoreLinq.Extensions</Namespace>
  <Namespace>Shouldly</Namespace>
  <Namespace>static Aoc2019.MiscStatics</Namespace>
  <Namespace>static System.Linq.Enumerable</Namespace>
  <Namespace>static System.Linq.EnumerableEx</Namespace>
  <Namespace>System.Drawing</Namespace>
  <Namespace>Unity.Coding.Editor</Namespace>
  <Namespace>Unity.Coding.Utils</Namespace>
</Query>

void Main()
{
    var folder = Util.CurrentQueryPath.ToNPath().Parent;
    var inputPath = folder.Combine($"{folder.FileName}.input.txt");
    
    var input = Parse(inputPath.ReadAllText());

// --- PART 1 ---

    // *SAMPLES*

    Solve1(Parse(@"
        COM)B
        B)C
        C)D
        D)E
        E)F
        B)G
        G)H
        D)I
        E)J
        J)K
        K)L
        ")).ShouldBe(42);

    // *PROBLEM*

    Solve1(input).Dump().ShouldBe(130681);

// --- PART 2 ---

    // *SAMPLES*

    Solve2(Parse(@"
        COM)B
        B)C
        C)D
        D)E
        E)F
        B)G
        G)H
        D)I
        E)J
        J)K
        K)L
        K)YOU
        I)SAN
        ")).ShouldBe(4);

    // *PROBLEM*
    
    Solve2(input).Dump().ShouldBe(313);
}

IDictionary<string, string> Parse(string text) =>
    text.SelectWords().Select(w => w.Split(')')).ToDictionary(p => p[1], p => p[0]);

IEnumerable<string> SelectAncestors(IDictionary<string, string> graph, string start) =>
    Generate(start, i => i != "COM", i => graph[i]);

int Solve1(IDictionary<string, string> graph) =>
    graph.Keys.Sum(k => SelectAncestors(graph, k).Count());

int Solve2(IDictionary<string, string> graph)
{
    var src = SelectAncestors(graph, "YOU").Reverse().ToArray();
    var dst = SelectAncestors(graph, "SAN").Reverse().ToArray();
    
    var common = src.Zip(dst).Count(v => v.First == v.Second);
    return src.Length + dst.Length - 2 - (2 * common);
}

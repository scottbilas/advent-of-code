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
        pbga (66)
        xhth (57)
        ebii (61)
        havc (66)
        ktlj (57)
        fwft (72) -> ktlj, cntj, xhth
        qoyq (66)
        padx (45) -> pbga, havc, qoyq
        tknk (41) -> ugml, padx, fwft
        jptl (61)
        ugml (68) -> gyxo, ebii, jptl
        gyxo (61)
        cntj (57)
        ";
    Solve1(sample).ShouldBe("tknk");

    // *PROBLEM*

    Solve1(input).Dump().ShouldBe("qibuqqg");

// --- PART 2 ---

    // *SAMPLES*

    Solve2(sample).ShouldBe(60);

    // *PROBLEM*

    Solve2(input).Dump().ShouldBe(1079);
}

IEnumerable<(string parent, int weight, string[] children)> Parse(string text)
{
    var parsed = text.Trim().SelectMatches(@"(?mnx-)
        (?<parent>\w+).*?
        (?<weight>\d+).*?
        (
          (?<child>\w+)(,\s*)?
        )*\s*$");
    foreach (var match in parsed)
        yield return (match.Text("parent"), match.Int("weight"), match.Texts("child"));
}

string Solve1(string text) => Enumerable
    .Except(
        Parse(text).Select(v => v.parent),
        Parse(text).SelectMany(v => v.children))
    .Single();

int Solve2(string text)
{
    var parsed = Parse(text);
    var nodes = Tree.WithTag<int>().Create(parsed.Select(l => (l.parent, l.children)));
    parsed.ForEach(l => nodes[l.parent].Tag = l.weight);
    
    var fixedWeight = 0;
    (int self, int children) CalcWeight(Tree<string, int> node)
    {
        var weights = node.Children.Select(CalcWeight);
        var groups = weights.GroupBy(v => v.self + v.children);
        if (groups.Count() > 1)
        {
            var ordered = groups.OrderBy(g => g.Count()).ToArray();
            fixedWeight = ordered[0].First().self + ordered[1].Key - ordered[0].Key;
        }
        
        return (node.Tag, weights.Sum(w => w.self + w.children));
    }

    CalcWeight(nodes.GetRoot());
    
    return fixedWeight;
}
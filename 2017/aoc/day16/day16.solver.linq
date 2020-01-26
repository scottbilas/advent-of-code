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

    var sample = "s1,x3/4,pe/b";
    Solve1(sample, 5).ShouldBe("baedc");

    // *PROBLEM*

    Solve1(input).Dump().ShouldBe("kpbodeajhlicngmf");

// --- PART 2 ---

    // *PROBLEM*

    Solve2(input).Dump().ShouldBe("ahgpjdkcbfmneloi");
}

RingArray<char> Init(int count) =>
    Range(0, count).Select(i => (char)(i + 'a')).ToRingArray();

(char, string, string)[] Parse(string movesText) => movesText
    .SelectMatches(@"([sxp])(\w+)(?:/(\w+))?")
    .Select(move => (move.Text(1)[0], move.Text(2), move.Text(3)))
    .ToArray();

string Solve(RingArray<char> programs, (char op, string a, string b)[] moves)
{
    foreach (var move in moves)
    {
        if (move.op == 's')
            programs.Offset -= move.a.ParseInt();
        else
        {
            var (i, j) = move.op == 'x'
                ? (move.a.ParseInt(), move.b.ParseInt())
                : (programs.IndexOf(move.a[0]), programs.IndexOf(move.b[0]));
            Utils.Swap(ref programs[i], ref programs[j]);
        }
    }
    
    return programs.ToStringFromChars();
}

string Solve1(string movesText, int count = 16) =>
    Solve(Init(count), Parse(movesText));

string Solve2(string movesText)
{
    var programs = Init(16);
    var moves = Parse(movesText);
    var seen = new Dictionary<string, int>();

    while (seen.TryAdd(Solve(programs, moves), seen.Count));

    var offset = 1*1000*1000*1000 % seen.Count;
    return seen.First(kvp => kvp.Value == offset - 1).Key;
}

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
        b inc 5 if a > 1
        a inc 1 if b < 5
        c dec -10 if a >= 1
        c inc -20 if c == 10";
    Solve1(sample).ShouldBe(1);

    // *PROBLEM*

    Solve1(input).Dump().ShouldBe(4902);

// --- PART 2 ---

    // *SAMPLES*

    Solve2(sample).ShouldBe(10);

    // *PROBLEM*

    Solve2(input).Dump().ShouldBe(7037);
}

(int end, int any) Solve(string text)
{
    var instrs = text
        .SelectMatches(@"\S+").Select(m => m.Value)
        .Batch(7, l => (
            t: l[^2], tr: l[^3], tv: l[^1].ParseInt(),
            o: l[1] == "inc" ? 1 : -1, or: l[0], ov: l[2].ParseInt()));

    var tests = Arr<(string, Func<int, int, bool>)>(
        (">" , (a, b) => a >  b),
        (">=", (a, b) => a >= b),
        ("<" , (a, b) => a <  b),
        ("<=", (a, b) => a <= b),
        ("==", (a, b) => a == b),
        ("!=", (a, b) => a != b)
        ).ToDictionary();
    
    var mem = new AutoDictionary<string, int>();
    var max = 0;
    
    foreach (var instr in instrs)
    {
        if (!tests[instr.t](mem[instr.tr], instr.tv))
            continue;

        mem[instr.or] += instr.ov * instr.o;
        max = Math.Max(max, mem[instr.or]);
    }
    
    return (mem.Values.Max(), max);
}

int Solve1(string text) => Solve(text).end;
int Solve2(string text) => Solve(text).any;

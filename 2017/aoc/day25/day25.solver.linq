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

    // *SAMPLES*

    Solve1(@"
        Begin in state A.
        Perform a diagnostic checksum after 6 steps.
        
        In state A:
          If the current value is 0:
            - Write the value 1.
            - Move one slot to the right.
            - Continue with state B.
          If the current value is 1:
            - Write the value 0.
            - Move one slot to the left.
            - Continue with state B.
        
        In state B:
          If the current value is 0:
            - Write the value 1.
            - Move one slot to the left.
            - Continue with state A.
          If the current value is 1:
            - Write the value 1.
            - Move one slot to the right.
            - Continue with state A.
        ").ShouldBe(3);

    // *PROBLEM*

    Solve1(input).Dump().ShouldBe(4230);
}

int Solve1(string blueprintText)
{
    (bool write, int offset, int nextState) ParseOp(IReadOnlyList<string> strs, int start) =>
        (strs[start] == "1", strs[start + 1] == "right" ? 1 : -1, strs[start + 2][0] - 'A');

    var instrs = blueprintText.SelectMatches(@"\b[A-Z]\b|\d+|left|right").Select(m => m.Value).ToArray();
    var states = instrs[2..].Batch(9, l => (zero: ParseOp(l, 2), one: ParseOp(l, 6))).ToArray();
    var steps  = instrs[1].ParseInt();
    var state  = states[instrs[0][0] - 'A'];

    var tape = new bool[20000];
    var pos  = tape.Length / 2;
    var used = 0;

    for (var i = 0; i < steps; ++i)
    {
        var op = tape[pos] ? state.one : state.zero;
        
        if (tape[pos] != op.write)
        {
            tape[pos] = op.write;
            used += op.write ? 1 : -1;
        }
        
        pos += op.offset;
        state = states[op.nextState];
    }
    
    return used;
}

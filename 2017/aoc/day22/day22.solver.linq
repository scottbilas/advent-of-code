<Query Kind="Program">
  <Reference Relative="..\..\..\temp\bin\libaoc2017\Debug\netstandard2.1\libaoc2017.dll">C:\proj\advent-of-code\temp\bin\libaoc2017\Debug\netstandard2.1\libaoc2017.dll</Reference>
  <Reference Relative="..\..\..\temp\bin\libaoc2017\Debug\netstandard2.1\libutils.dll">C:\proj\advent-of-code\temp\bin\libaoc2017\Debug\netstandard2.1\libutils.dll</Reference>
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
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Collections.Concurrent</Namespace>
</Query>

static readonly NPath ScriptDir = Util.CurrentQueryPath.ToNPath().Parent;

void Main()
{
    var inputPath = ScriptDir.Combine($"{ScriptDir.FileName}.input.txt");
    var input = inputPath.ReadAllText();

// --- PART 1 ---

    // *SAMPLES*

    var sample = @"
        ..#
        #..
        ...";
    Solve1(sample, 7).ShouldBe(5);
    Solve1(sample, 70).ShouldBe(41);
    Solve1(sample, 10000).ShouldBe(5587);

    // *PROBLEM*

    Solve1(input, 10000).Dump().ShouldBe(5259);

// --- PART 2 ---

    // *SAMPLES*

    Solve2(sample, 100).ShouldBe(26);
    Solve2(sample, 10000000).ShouldBe(2511944);

    // *PROBLEM*

    Solve2(input, 10000000).Dump().ShouldBe(2511722);
}

int Solve1(string gridText, int bursts)
{
    var (pos, board) = With(gridText.ParseRectGrid(), grid =>
        (grid.GetDimensions() / 2,
         grid.SelectCells().SelectWhere(c => (c.pos, c.cell == '#')).ToHashSet()));

    var infected = 0;
    var dir = Dir.N;
    
    for (var burst = 0; burst < bursts; ++burst)
    {
        if (board.Remove(pos))
            dir = dir.TurnRight();
        else
        {
            dir = dir.TurnLeft();
            board.Add(pos);
            ++infected;
        }
        
        pos += dir.GetMove();
    }
        
    return infected;
}

enum State { Clean, Weakened, Infected, Flagged }

int Solve2(string gridText, int bursts)
{
    var (pos, board) = With(gridText.ParseRectGrid(), grid =>
        (grid.GetDimensions() / 2,
         grid.SelectCells()
            .SelectWhere(c => (c.pos, c.cell == '#'))
            .ToDictionary(p => p, State.Infected)));

    var infected = 0;
    var dir = Dir.N;

    for (var burst = 0; burst < bursts; ++burst)
    {
        if (!board.TryGetValue(pos, out var state))
        {
            dir = dir.TurnLeft();
            board.Add(pos, State.Weakened);
        }
        else if (state == State.Weakened)
        {
            ++infected;
            board[pos] = State.Infected;
        }
        else if (state == State.Infected)
        {
            dir = dir.TurnRight();
            board[pos] = State.Flagged;
        }
        else
        {
            dir = dir.GetReverse();
            board.Remove(pos);
        }

        pos += dir.GetMove();
    }

    return infected;
}

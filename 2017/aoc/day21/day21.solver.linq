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

    Solve1(2, @"
        ../.# => ##./#../...
        .#./..#/### => #..#/..../..../#..#
        ").ShouldBe(12);

    // *PROBLEM*

    Solve1(5, input).Dump().ShouldBe(139);

// --- PART 2 ---

    // *PROBLEM*

    Solve1(18, input).Dump().ShouldBe(1857134);
}

uint Bits(IEnumerable<char> cells) =>
    cells.Reverse().WithIndices().Aggregate(0u, (bits, cell) =>
        bits | (cell.value == '#' ? 1u << cell.index : 0));

Dictionary<(int size, uint bits), uint> Parse(string rulesText)
{
    uint BitsX(char[] match, IEnumerable<int> indices) =>
        Bits(indices.Select(i => match[i]));
    
    IEnumerable<((int size, uint bits) key, uint value)> RulePairs(int size, char[] match, uint replace, IEnumerable<int[]> indices) =>
        indices.SelectMany(i => Arr(
            ((size, BitsX(match, i)), replace),
            ((size, BitsX(match, i.Reverse())), replace)));

    return rulesText
        .SelectLines()
        .Select(l => l.Matches(@"[.#]").Select(m => m.Value[0]).ToArray())
        .SelectMany(rule => rule.Length == 2 * 2 + 3 * 3
            ? RulePairs(2, rule[0..4], Bits(rule[4..]), Arr( // 2x2 -> 3x3
                Arr(0, 1, 2, 3), Arr(0, 2, 1, 3),
                Arr(1, 3, 0, 2), Arr(1, 0, 3, 2)))
            : RulePairs(3, rule[0..9], Bits(rule[9..]), Arr( // 3x3 -> 4x4
                Arr(0, 1, 2, 3, 4, 5, 6, 7, 8), Arr(0, 3, 6, 1, 4, 7, 2, 5, 8),
                Arr(2, 1, 0, 5, 4, 3, 8, 7, 6), Arr(2, 5, 8, 1, 4, 7, 0, 3, 6))))
        .Distinct()
        .ToDictionary();
}


int Solve1(int iterations, string rulesText)
{
    var rules = Parse(rulesText);

    var grid = @"
        .#.
        ..#
        ###
        ".ParseRectGrid();

    for (var i = 0; i < iterations; ++i)
    {
        var size = grid.GetDimensions().X;
        var squareSize = size % 2 == 0 ? 2 : 3;
        var squareCount = size / squareSize;

        var newGrid = new char[size + squareCount, size + squareCount];
        var newSquareSize = squareSize + 1;

        for (var ys = 0; ys < squareCount; ++ys)
        for (var xs = 0; xs < squareCount; ++xs)
        {
            var match = 0u;

            {
                var bit = 1u << squareSize * squareSize - 1;

                for (int y = ys * squareSize, ye = y + squareSize; y < ye; ++y)
                for (int x = xs * squareSize, xe = x + squareSize; x < xe; ++x)
                {
                    match |= grid[x, y] == '#' ? bit : 0;
                    bit >>= 1;
                }
            }

            var replace = rules[(squareSize, match)];

            {
                var bit = 1u << newSquareSize * newSquareSize - 1;

                for (int y = ys * newSquareSize, ye = y + newSquareSize; y < ye; ++y)
                for (int x = xs * newSquareSize, xe = x + newSquareSize; x < xe; ++x)
                {
                    newGrid[x, y] = (replace & bit) == 0 ? '.' : '#';
                    bit >>= 1;
                }
            }
        }

        grid = newGrid;
    }

    //Util.Image(grid.ToPng(c => c == '#' ? Color.White : Color.Black, 1, 0)).Dump();
    
    return grid.SelectCells().Count(c => c.cell == '#');
}

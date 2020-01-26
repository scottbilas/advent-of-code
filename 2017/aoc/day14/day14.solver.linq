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
    var inputWord = inputPath.ReadAllWords().Single();

// --- PART 1 ---

    // *SAMPLES*

    var sampleWord = "flqrgnkx";
    Solve1(sampleWord).ShouldBe(8108);

    // *PROBLEM*

    Solve1(inputWord).Dump().ShouldBe(8106);

// --- PART 2 ---

    // *SAMPLES*

    Solve2(sampleWord).ShouldBe(1242);

    // *PROBLEM*

    Solve2(inputWord).Dump().ShouldBe(1164);
}

int[,] HashToGrid(string word)
{
    var grid = new int[128, 128];

    for (var row = 0; row < 128; ++row)
    {
        var chars = $"{word}-{row}".ToCharArray()
            .Select(c => (int)c)
            .Concat(Arr(17, 31, 73, 47, 23))
            .ToArray();
    
        var elements = new RingArray<int>(Range(0, 256));
        var (pos, skip) = (0, 0);
    
        for (var i = 0; i < 64; ++i)
        {
            foreach (var span in chars)
            {
                elements.Reverse(pos, span);
                pos += span + skip++;
            }
        }

        foreach (var xor in elements.Batch(16, l => l.Aggregate(0, (a, b) => a ^ b)).WithIndices())
        {
            for (var x = 0; x < 8; ++x)
            {
                if ((xor.value & (1 << (7 - x))) != 0)
                    grid[xor.index * 8 + x, row] = -1;
            }
        }
    }

    return grid;
}

int Solve1(string word) =>
    HashToGrid(word).Select().Count(c => c != 0);

int Solve2(string word)
{
    var grid = HashToGrid(word);
    var size = grid.GetDimensions();
    
    void Walk(Int2 pos)
    {
        grid.SetAt(pos, 1);
        foreach (var adj in pos.SelectValidAdjacent(size).Where(p => grid.GetAt(p) < 0))
            Walk(adj);
    }
    
    return grid.SelectCells().Where(c => c.cell == -1).Do(c => Walk(c.pos)).Count();
}

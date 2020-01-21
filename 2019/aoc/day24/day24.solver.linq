<Query Kind="Program">
  <Reference Relative="..\..\..\temp\bin\libaoc2019\Debug\netstandard2.1\libaoc2019.dll">C:\proj\advent-of-code\temp\bin\libaoc2019\Debug\netstandard2.1\libaoc2019.dll</Reference>
  <Reference Relative="..\..\..\temp\bin\libaoc2019\Debug\netstandard2.1\libutils.dll">C:\proj\advent-of-code\temp\bin\libaoc2019\Debug\netstandard2.1\libutils.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\netstandard.dll</Reference>
  <NuGetReference>Combinatorics</NuGetReference>
  <NuGetReference>morelinq</NuGetReference>
  <NuGetReference>Open.Numeric.Primes</NuGetReference>
  <NuGetReference>Shouldly</NuGetReference>
  <NuGetReference>System.Interactive</NuGetReference>
  <Namespace>Aoc2019</Namespace>
  <Namespace>Combinatorics.Collections</Namespace>
  <Namespace>JetBrains.Annotations</Namespace>
  <Namespace>Shouldly</Namespace>
  <Namespace>static Aoc2019.MiscStatics</Namespace>
  <Namespace>static Aoc2019.Utils</Namespace>
  <Namespace>static System.Linq.Enumerable</Namespace>
  <Namespace>static System.Linq.EnumerableEx</Namespace>
  <Namespace>System.Drawing</Namespace>
  <Namespace>System.Numerics</Namespace>
  <Namespace>Unity.Coding.Editor</Namespace>
  <Namespace>Unity.Coding.Utils</Namespace>
  <Namespace>Open.Numeric.Primes</Namespace>
</Query>

void Main()
{
    var folder = Util.CurrentQueryPath.ToNPath().Parent;
    var inputPath = folder.Combine($"{folder.FileName}.input.txt");

    var input = inputPath.ReadGrid();

// --- PART 1 ---

    // *SAMPLES*

    var sample = @"
        ....#
        #..#.
        #..##
        ..#..
        #....
        ".ToGrid();

    Solve1(sample).ShouldBe(2129920);

    // *PROBLEM*

    Solve1(input).Dump().ShouldBe(32513278);

// --- PART 2 ---

    // *SAMPLES*
    
    Solve2(sample, 10).ShouldBe(99);

    // *PROBLEM*

    Solve2(input, 200).Dump().ShouldBe(1912);
}

int Solve1(char[,] cells)
{
    var dims = cells.GetDimensions();
    var seen = new HashSet<int>();

    for (;;)
    {
        var dst = new char[dims.X, dims.Y];
        var bits = 0;

        foreach (var cell in cells.SelectCells())
        {
            var adj = cell.pos.SelectValidAdjacent(dims).Count(c => cells.GetAt(c) == '#');
            if (adj == 1 || (cell.cell != '#' && adj == 2))
            {
                dst.SetAt(cell.pos, '#');
                bits |= 1 << (cell.pos.Y * dims.Y + cell.pos.X);
            }
        }

        if (!seen.Add(bits))
            return bits;

        cells = dst;
    }
}

class Cell
{
    public bool Bug;
    public int AdjCount;
}

IEnumerable<(int layer, int x, int y)> SelectRelevantCells(int layer, Int2 pos)
{
    if (pos.X == 2 && pos.Y == 2)
        yield break;
    
    foreach (var (x, y) in pos.SelectAdjacent())
    {
        if (x == 2 && y == 2)
        {
            if (pos.X == 2)
            {
                for (var ix = 0; ix < 5; ++ix)
                    yield return (layer + 1, ix, pos.Y == 1 ? 0 : 4);
            }
            else
            {
                for (var iy = 0; iy < 5; ++iy)
                    yield return (layer + 1, pos.X == 1 ? 0 : 4, iy);
            }
        }
        else if (x < 0) yield return (layer - 1, 1, 2);
        else if (x > 4) yield return (layer - 1, 3, 2);
        else if (y < 0) yield return (layer - 1, 2, 1);
        else if (y > 4) yield return (layer - 1, 2, 3);
        else            yield return (layer, x, y);
    }
}

int Solve2(char[,] cells, int loops)
{
    var layers = new Cell[5, 5]
        .Fill(pos => new Cell { Bug = cells.GetAt(pos) == '#' } )
        .WrapInEnumerable().ToList();

    for (var loop = 0; loop < loops; ++loop)
    {
        // expand outer/inner as needed
        if (layers.First().SelectBorderCells().Any(c => c.cell.Bug))
            layers.Insert(0, new Cell[5, 5].FillNew());
        if (layers.Last().SelectCells(RectInt2.FromPosSize(1, 1, 3, 3)).Any(c => c.cell.Bug))
            layers.Add(new Cell[5, 5].FillNew());

        // calc adjacents
        for (var layer = 0; layer < layers.Count; ++layer)
        {
            foreach (var pos in layers[layer].SelectCoords())
            {
                var adjCount = SelectRelevantCells(layer, pos)
                    .Where(v => v.layer >= 0 && v.layer < layers.Count)
                    .Count(v => layers[v.layer][v.x, v.y].Bug);
                layers[layer][pos.X, pos.Y].AdjCount = adjCount;
            }
        }

        // sim
        foreach (var cell in layers.SelectMany(l => l.Select()))
            cell.Bug = cell.AdjCount == 1 || (!cell.Bug && cell.AdjCount == 2);
    }

    return layers.SelectMany(layer => layer.Select()).Count(c => c.Bug);
}

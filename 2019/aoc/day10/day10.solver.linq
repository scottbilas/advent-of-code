<Query Kind="Program">
  <Reference Relative="..\..\libaoc\bin\Debug\netstandard2.1\libaoc2019.dll">C:\proj\advent-of-code\2019\libaoc\bin\Debug\netstandard2.1\libaoc2019.dll</Reference>
  <Reference Relative="..\..\libaoc\bin\Debug\netstandard2.1\libutils.dll">C:\proj\advent-of-code\2019\libaoc\bin\Debug\netstandard2.1\libutils.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\netstandard.dll</Reference>
  <NuGetReference>Combinatorics</NuGetReference>
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
</Query>

void Main()
{
    var folder = Util.CurrentQueryPath.ToNPath().Parent;
    var inputPath = folder.Combine($"{folder.FileName}.input.txt");

    var map = inputPath.ReadAllText().ToGrid();

// --- PART 1 ---

    // *SAMPLES*

    {
        var sample = @"
            .#..#
            .....
            #####
            ....#
            ...##
            ".ToGrid();

        var expected = @"
            .7..7
            .....
            67775
            ....7
            ...87
            ".ToGrid();

        foreach (var item in Solve1Coords(sample))
            (expected[item.pos.X, item.pos.Y] - '0').ShouldBe(item.viz);

        Solve1(sample).ShouldBe((new Int2(3, 4), 8));
    }

    Solve1(@"
        #.........
        ...A......
        ...B..a...
        .EDCG....a
        ..F.c.b...
        .....c....
        ..efd.c.gb
        .......c..
        ....f...c.
        ...e..d..c
        ".ToGrid()).ShouldBe((new Int2(0, 0), 7));

    Solve1(@"
        ......#.#.
        #..#.#....
        ..#######.
        .#.#.###..
        .#..#.....
        ..#....#.#
        #..#....#.
        .##.#..###
        ##...#..#.
        .#....####
        ".ToGrid()).ShouldBe((new Int2(5, 8), 33));

    Solve1(@"
        #.#...#.#.
        .###....#.
        .#....#...
        ##.#.#.#.#
        ....#.#.#.
        .##..###.#
        ..#...##..
        ..##....##
        ......#...
        .####.###.
        ".ToGrid()).ShouldBe((new Int2(1, 2), 35));

    Solve1(@"
        .#..#..###
        ####.###.#
        ....###.#.
        ..###.##.#
        ##.##.#.#.
        ....###..#
        ..#.#..#.#
        #..#.#.###
        .##...##.#
        .....#.#..
        ".ToGrid()).ShouldBe((new Int2(6, 3), 41));

    var bigSample = @"
        .#..##.###...#######
        ##.############..##.
        .#.######.########.#
        .###.#######.####.#.
        #####.##.#.##.###.##
        ..#####..#.#########
        ####################
        #.####....###.#.#.##
        ##.#################
        #####.##.###..####..
        ..######..##.#######
        ####.##.####...##..#
        .#####..#.######.###
        ##...#.##########...
        #.##########.#######
        .####.#.###.###.#.##
        ....##.##.###..#####
        .#.#.###########.###
        #.#.#.#####.####.###
        ###.##.####.##.#..##    
        ".ToGrid();

    var bigSampleResult = Solve1(bigSample);
    bigSampleResult.ShouldBe((new Int2(11, 13), 210));

    // *PROBLEM*

    var result = Solve1(map);
    result.viz.Dump().ShouldBe(269);

// --- PART 2 ---

    // *SAMPLES*

    {
        var sresult = Solve2Coords(@"
            .#....#####...#..
            ##...##.#####..##
            ##...#...#.#####.
            ..#.....X...###..
            ..#.#.....#....##
            ".ToGrid());//...
    }

    Solve2(bigSampleResult.pos, bigSample).ShouldBe(802);

    // *PROBLEM*

    Solve2(result.pos, map).Dump().ShouldBe(612);
}

IEnumerable<IGrouping<Int2, Int2>> GroupAsteroidsByLine(Int2 src, char[,] map) => map
    .SelectCells().Where(cell => cell.cell != '.' && !cell.pos.Equals(src))
    .Select(cell => cell.pos)
    .GroupBy(pos => (pos - src).ReduceFraction());

IEnumerable<(Int2 pos, int viz)> Solve1Coords(char[,] map) => map
    .SelectCells().Where(c => c.cell == '#')
    .Select(src => (src.pos, GroupAsteroidsByLine(src.pos, map).Count()));

(Int2 pos, int viz) Solve1(char[,] map) =>
    Solve1Coords(map).MaxBy(v => v.viz).Single();

IEnumerable<Int2> Solve2Coords(Int2 src, char[,] map)
{
    var lines = GroupAsteroidsByLine(src, map)
        .Select(g => g.OrderBy(p => p.LengthSq(src)).ToList())
        .OrderBy(l => Math.PI - Math.Atan2(l[0].X - src.X, l[0].Y - src.Y))
        .ToArray();

    for (;;)
    {
        foreach (var line in lines.Where(l => l.Any()))
        {
            yield return line[0];
            line.RemoveAt(0);
        }
    }
}

IEnumerable<Int2> Solve2Coords(char[,] map) =>
    Solve2Coords(map.SelectCells().First(c => c.cell == 'X').pos, map);

int Solve2(Int2 src, char[,] map) => With(
    Solve2Coords(src, map).ElementAt(199),
    f => f.X * 100 + f.Y);

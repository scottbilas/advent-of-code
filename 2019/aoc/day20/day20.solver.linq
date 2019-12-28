<Query Kind="Program">
  <Reference Relative="..\..\..\temp\bin\libaoc2019\Debug\netstandard2.1\libaoc2019.dll">C:\proj\advent-of-code\temp\bin\libaoc2019\Debug\netstandard2.1\libaoc2019.dll</Reference>
  <Reference Relative="..\..\..\temp\bin\libaoc2019\Debug\netstandard2.1\libutils.dll">C:\proj\advent-of-code\temp\bin\libaoc2019\Debug\netstandard2.1\libutils.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\netstandard.dll</Reference>
  <NuGetReference>Combinatorics</NuGetReference>
  <NuGetReference>morelinq</NuGetReference>
  <NuGetReference>RoyT.AStar</NuGetReference>
  <NuGetReference>Shouldly</NuGetReference>
  <NuGetReference>System.Interactive</NuGetReference>
  <Namespace>Aoc2019</Namespace>
  <Namespace>Combinatorics.Collections</Namespace>
  <Namespace>JetBrains.Annotations</Namespace>
  <Namespace>RoyT.AStar</Namespace>
  <Namespace>Shouldly</Namespace>
  <Namespace>static Aoc2019.MiscStatics</Namespace>
  <Namespace>static Aoc2019.Utils</Namespace>
  <Namespace>static System.Linq.Enumerable</Namespace>
  <Namespace>static System.Linq.EnumerableEx</Namespace>
  <Namespace>System.Drawing</Namespace>
  <Namespace>System.Numerics</Namespace>
  <Namespace>Unity.Coding.Editor</Namespace>
  <Namespace>Unity.Coding.Utils</Namespace>
  <Namespace>QuickGraph</Namespace>
  <Namespace>QuickGraph.Graphviz</Namespace>
  <Namespace>QuickGraph.Algorithms</Namespace>
</Query>

static readonly NPath ScriptDir = Util.CurrentQueryPath.ToNPath().Parent;

void Main()
{
    var inputPath = ScriptDir.Combine($"{ScriptDir.FileName}.input.txt");

    var input = inputPath.ReadAllText();

// --- PART 1 ---

    // *SAMPLES*

    var k_Sample1 = (text: @"
                 A
                 A
          #######.#########
          #######.........#
          #######.#######.#
          #######.#######.# 
          #######.#######.#
          #####  B    ###.#
        BC...##  C    ###.#
          ##.##       ###.#
          ##...DE  F  ###.#
          #####    G  ###.#
          #########.#####.#
        DE..#######...###.#
          #.#########.###.#
        FG..#########.....#
          ###########.#####
                     Z
                     Z",
        border: RectInt2.FromBounds(2, 2, 0, 2));
    Solve1("sample11", k_Sample1.text, k_Sample1.border).ShouldBe(23);

    var k_Sample2 = @"
                           A
                           A
          #################.#############
          #.#...#...................#.#.#
          #.#.#.###.###.###.#########.#.#
          #.#.#.......#...#.....#.#.#...#
          #.#########.###.#####.#.#.###.#
          #.............#.#.....#.......#
          ###.###########.###.#####.#.#.#
          #.....#        A   C    #.#.#.#
          #######        S   P    #####.#
          #.#...#                 #......VT
          #.#.#.#                 #.#####
          #...#.#               YN....#.#
          #.###.#                 #####.#
        DI....#.#                 #.....#
          #####.#                 #.###.#
        ZZ......#               QG....#..AS
          ###.###                 #######
        JO..#.#.#                 #.....#
          #.#.#.#                 ###.#.#
          #...#..DI             BU....#..LF
          #####.#                 #.#####
        YN......#               VT..#....QG
          #.###.#                 #.###.#
          #.#...#                 #.....#
          ###.###    J L     J    #.#.###
          #.....#    O F     P    #.#...#
          #.###.#####.#.#####.#####.###.#
          #...#.#.#...#.....#.....#.#...#
          #.#####.###.###.#.#.#########.#
          #...#.#.....#...#.#.#.#.....#.#
          #.###.#####.###.###.#.#.#######
          #.#.........#...#.............#
          #########.###.###.#############
                   B   J   C
                   U   P   P";
    Solve1("sample12", k_Sample2).ShouldBe(58);

    // *PROBLEM*
    
    Solve1("part1", input).Dump().ShouldBe(528);

// --- PART 2 ---

    // *SAMPLES*

    Solve2("sample21", k_Sample1.text, k_Sample1.border).ShouldBe(26);

    Solve2("sample22", k_Sample2).ShouldBe(0);

    Solve2("sample2x", @"
                 A
                 A
          #######.#########
          #######.#########
          #######.#########
          #######.######### 
          #######.#########
          #####  A    #####
          #####  B    #####
          #####       ###..AC
        AB.....AC A A ###.#
          #####   E D ###..AD
          ########.#.######
          ########.#.......AE
          ########.########
          ########....#####
          ###########.#####
                     Z
                     Z
        ").ShouldBe(30);

    Solve2("sample23", @"
                     Z L X W       C
                     Z P Q B       K
          ###########.#.#.#.#######.###############
          #...#.......#.#.......#.#.......#.#.#...#
          ###.#.#.#.#.#.#.#.###.#.#.#######.#.#.###
          #.#...#.#.#...#.#.#...#...#...#.#.......#
          #.###.#######.###.###.#.###.###.#.#######
          #...#.......#.#...#...#.............#...#
          #.#########.#######.#.#######.#######.###
          #...#.#    F       R I       Z    #.#.#.#
          #.###.#    D       E C       H    #.#.#.#
          #.#...#                           #...#.#
          #.###.#                           #.###.#
          #.#....OA                       WB..#.#..ZH
          #.###.#                           #.#.#.#
        CJ......#                           #.....#
          #######                           #######
          #.#....CK                         #......IC
          #.###.#                           #.###.#
          #.....#                           #...#.#
          ###.###                           #.#.#.#
        XF....#.#                         RF..#.#.#
          #####.#                           #######
          #......CJ                       NM..#...#
          ###.#.#                           #.###.#
        RE....#.#                           #......RF
          ###.###        X   X       L      #.#.#.#
          #.....#        F   Q       P      #.#.#.#
          ###.###########.###.#######.#########.###
          #.....#...#.....#.......#...#.....#.#...#
          #####.#.###.#######.#######.###.###.#.#.#
          #.......#.......#.#.#.#.#...#...#...#.#.#
          #####.###.#####.#.#.#.#.###.###.#.###.###
          #.......#.....#.#...#...............#...#
          #############.#.#.###.###################
                       A O F   N
                       A A D   M
        ").ShouldBe(396);

    // *PROBLEM*

    Solve2("part2", input).Dump().ShouldBe(6214);
}

class MazeEdge : TaggedUndirectedEdge<string, int>
    { public MazeEdge(string va, string vb, int steps) : base(va, vb, steps) { } }
class MazeGraph : UndirectedGraph<string, MazeEdge> { }

MazeGraph Parse(string text, RectInt2 border)
{
    char[,] grid;

    {
        var lines = text.Split('\n').Select(l => l.TrimEnd()).Where(l => l.Any()).ToList();
        var skip = lines.Select(l => l.IndexOf('#')).Where(i => i > 0).First() - 2;
        var width = lines.Select(l => l.Length).Max() - skip;

        grid = new char[width, lines.Count];

        foreach (var (x, y) in grid.SelectCoords())
            grid[x, y] = lines[y].TryGetAt(x + skip, ' ');
    }

    var portals = new Dictionary<string, Position>();

    {
        void AddPortals(RectInt2 rect, bool inner = false)
        {
            foreach (var c in grid.SelectCells(rect.SelectBorderCoords(1)).Where(c => c.cell == '.'))
            {
                Int2 a, b;
                if (c.pos.X == (inner ? rect.RightIn : rect.Left))
                    (a, b) = (new Int2(-2, 0), new Int2(-1, 0));
                else if (c.pos.X == (inner ? rect.Left : rect.RightIn))
                    (a, b) = (new Int2(1, 0), new Int2(2, 0));
                else if (c.pos.Y == (inner ? rect.BottomIn : rect.Top))
                    (a, b) = (new Int2(0, -2), new Int2(0, -1));
                else
                    (a, b) = (new Int2(0, 1), new Int2(0, 2));

                var id = Arr(grid.GetAt(c.pos + a), grid.GetAt(c.pos + b)).ToStringFromChars();
                portals.Add((inner ? "+" : "-") + id, c.pos.ToPosition());
            }
        }

        var boardRect = grid.GetRect().Deflate(border);
        AddPortals(boardRect);

        var innerRect = With(
            grid.SelectCells(boardRect).Where(c => c.cell == ' '),
            cells => RectInt2.FromBoundsIn(cells.First().pos, cells.Last().pos)).Inflate(1);
        AddPortals(innerRect, true);
    }

    var graph = new MazeGraph();

    {
        var (cx, cy) = grid.GetDimensions();
        var pathGrid = new Grid(cx, cy);
        foreach (var (x, y, _) in grid.SelectCells().SelectXy().Where(c => c.cell != '.'))
            pathGrid.BlockCell(new Position(x, y));

        foreach (var (a, b) in
            new Combinations<string>(portals.Keys.Ordered().ToList(), 2)
            .Select(l => l.First2()))
        {
            var (apos, bpos) = (portals[a], portals[b]);
            var p = pathGrid.GetPath(apos, bpos, MovementPatterns.LateralOnly);
            if (p.Length > 0)
            {
                var steps = p.Length - 1;
                graph.AddVerticesAndEdge(new MazeEdge(a, b, steps));
            }
        }
    }

    return graph;
}

int FindSolutionSteps(MazeGraph graph)
{
    graph.ShortestPathsDijkstra(edge => edge.Tag, "-AA")("-ZZ", out var result);
    return result != null ? result.Sum(i => i.Tag) : 0;
}

int Solve1(string name, string text) =>
    Solve1(name, text, RectInt2.FromBounds(2, 2, 2, 2));

int Solve1(string name, string text, RectInt2 border)
{
    var graph = Parse(text, border);

    foreach (var portal in graph.Vertices.Where(p => p[0] == '+'))
    {
        var other = '-' + portal.Substring(1);
        if (graph.ContainsVertex(other))
            graph.AddVerticesAndEdge(new MazeEdge(portal, other, 1));
    }

    Render(graph, name);
    
    return FindSolutionSteps(graph);
}

int Solve2(string name, string text) =>
    Solve2(name, text, RectInt2.FromBounds(2, 2, 2, 2));

int Solve2(string name, string text, RectInt2 border, int maxNesting = 50)
{
    var graph = Parse(text, border);
    
    var baseEdges = graph.Edges.ToArray();
    var baseVerts = graph.Vertices.SelectWhere(v => (v.Substring(1), v[0] == '+')).ToArray();

    for (var i = 1; i <= maxNesting; ++i)
    {
        var result = FindSolutionSteps(graph);
        if (result != 0)
        {
            Render(graph, name);
            return result;
        }

        // copy outer to inner
        foreach (var edge in baseEdges)
            graph.AddVerticesAndEdge(new MazeEdge(edge.Source + i, edge.Target + i, edge.Tag));

        // link outer to inner
        foreach (var vert in baseVerts)
        {
            var outerName = $"+{vert}{(i == 1 ? "" : (i - 1).ToString())}";
            var innerName = $"-{vert}{i}";
            graph.AddVerticesAndEdge(new MazeEdge(innerName, outerName, 1));
        }
    }

    return 0;
}

bool g_Render = false;

void Render(MazeGraph graph, string name)
{
    if (!g_Render) return;
    
    foreach (var vert in graph.Vertices.ToList())
        if (vert != "-AA" && FindSolutionSteps(graph) == 0)
            graph.RemoveVertex(vert);
    
    var png = Utils.RenderGraphViz(graph.ToGraphviz(g =>
        {
            g.FormatVertex += (_, args) => args.VertexFormatter.Comment = args.Vertex.ToString();
            g.FormatEdge += (_, args) =>
            {
                args.EdgeFormatter.Label.Value = $"  {args.Edge.Tag}  ";
                args.EdgeFormatter.Length = (int)Math.Log(args.Edge.Tag);
            };
        })/*, "-Kneato"*/);

    var graphDir = ScriptDir.Combine("graphs").CreateDirectory();
    graphDir.Combine(name + ".png").WriteAllBytes(png);
        
    Util.Image(png);//.Dump();
}

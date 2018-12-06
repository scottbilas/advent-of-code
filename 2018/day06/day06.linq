<Query Kind="Statements">
  <NuGetReference>morelinq</NuGetReference>
  <NuGetReference>Shouldly</NuGetReference>
  <Namespace>MoreLinq.Extensions</Namespace>
  <Namespace>Shouldly</Namespace>
  <Namespace>System.Drawing</Namespace>
  <Namespace>System.Drawing.Imaging</Namespace>
</Query>

// sample

var sample = Parse(new[] {
    "1, 1",
    "1, 6",
    "8, 3",
    "3, 4",
    "5, 5",
    "8, 9",
    }).ToList();

LargestFiniteArea(DistanceGrids(sample)).ShouldBe(17);
SafeRegionSize(sample, 32).ShouldBe(16);

// problem

var scriptDir = Path.GetDirectoryName(Util.CurrentQueryPath);
var input = Parse(File.ReadLines($"{scriptDir}/input.txt")).ToList();

var distanceGrids = DistanceGrids(input);
//LargestFiniteArea(distanceGrids).Dump().ShouldBe(3420);
Render(distanceGrids, $"{scriptDir}/map.png");
//SafeRegionSize(input, 10000).Dump().ShouldBe(46667);

IEnumerable<(int x, int y)> Parse(IEnumerable<string> textCoords)
{
    return textCoords
        .Select(tc => tc.Split(','))
        .Select(ts => (x: int.Parse(ts[0]), y: int.Parse(ts[1]))).ToList();
}

int LargestFiniteArea((int[,] ids, int[,] dist, bool[] inf) grids)
{
    return grids.ids.Cast<int>()
        .Where(i => i >= 0 && !grids.inf[i - 1])
        .GroupBy(i => i)
        .Max(g => g.Count());
}

(int[,] ids, int[,] dist, bool[] inf) DistanceGrids(IReadOnlyList<(int x, int y)> coords)
{
    var bounds = (
        l: coords.Min(c => c.x), t: coords.Min(c => c.y),
        r: coords.Max(c => c.x) + 1, b: coords.Max(c => c.y) + 1);

    var isInfinite = new bool[coords.Count];

    var grid = new int[bounds.r + 1, bounds.b + 1];
    var gridDist = new int[bounds.r + 1, bounds.b + 1];
    for (int dist = 1; ; ++dist)
    {
        var written = false;
        for (int icoord = 0; icoord < coords.Count; ++icoord)
        {
            var coord = coords[icoord];
            var radius = dist * 2 + 1;
            
            var cx = 1;
            var sx = 0;
            var shrink = false;
            
            for (var iy = 0; iy <= radius; ++iy)
            {
                var x = coord.y - dist + iy;
                for (var ix = 0; ix < cx; ++ix)
                {
                    var y = coord.x + sx + ix;
                    if (x < 0 || x > bounds.r || y < 0 || y > bounds.b)
                    {
                    }
                    else
                    {
                        if (grid[x, y] == 0 || (gridDist[x, y] != 0 && gridDist[x, y] > dist))
                        {
                            grid[x, y] = icoord + 1;
                            gridDist[x, y] = dist;
                            written = true;
                            if (x == 0 || x == bounds.r || y == 0 || y == bounds.b)
                                isInfinite[icoord] = true;
                        }
                        else if (gridDist[x, y] == dist)
                        {
                            grid[x, y] = -1;
                        }
                    }
                }
                if (cx >= radius)
                    shrink = true;
                cx += shrink ? -2 : 2;
                sx += shrink ? 1 : -1;
            }
        }
        if (!written)
            break;
    }
    
    return (grid, gridDist, isInfinite);
}

void Render((int[,] ids, int[,] dist, bool[] inf) grids, string path)
{
    var (cx, cy) = (grids.ids.GetLength(0), grids.ids.GetLength(1));
    var maxId = grids.ids.Cast<int>().Max();
    var maxDist = grids.dist.Cast<int>().Max();

    /*var idColors = typeof(Color)
        .GetProperties(BindingFlags.Static | BindingFlags.Public)
        .Select(p => p.GetGetMethod().Invoke(null, null))
        .Cast<Color>()
        .Where(c => c.A == 255 && ((c.R + c.G + c.B) < (600) && (c.R + c.G + c.B) > 50))
        .Dump()
        .Repeat()
        .Take(maxId + 1)
        .RandomSubset(maxId + 1)
        .ToList();*/
        
    var r = new Random();
    var idColors = Enumerable
        .Range(0, maxId + 1)
        .Select(_ => Color.FromArgb(255, r.Next(100, 255), r.Next(100, 255), r.Next(100, 255)))
        .ToList();

    using (var b = new Bitmap(cx, cy))
    {
        for (int y = 0; y < cy; ++y)
        {
            for (int x = 0; x < cx; ++x)
            {
                Color c;
                
                var id = grids.ids[x, y];
                if (id < 0)
                    c = Color.White;
                else if (id == 0)
                    c = Color.Black;
                else
                {
                    var scale = 1.0 - ((double)(grids.dist[x, y]) / maxDist);
                    c = idColors[id];
                    c = Color.FromArgb(255, (int)(scale * c.R), (int)(scale * c.G), (int)(scale * c.B));
                }

                b.SetPixel(x, y, c);
            }
        }

        b.Save(path, ImageFormat.Png);
    }
}

int SafeRegionSize(IReadOnlyList<(int x, int y)> coords, int max)
{
    var bounds = (
        l: coords.Min(c => c.x), t: coords.Min(c => c.y),
        r: coords.Max(c => c.x) + 1, b: coords.Max(c => c.y) + 1);

    var grid = new int[bounds.r, bounds.b];
    for (int y = 0; y < bounds.b; ++y)
    {
        for (int x = 0; x < bounds.r; ++x)
        {
            var dist = 0;
            for (int icoord = 0; icoord < coords.Count; ++icoord)
            {
                dist += Math.Abs(coords[icoord].x - x) + Math.Abs(coords[icoord].y - y);
            }
            grid[x, y] = dist;
        }
    }
    
    return grid.Cast<int>().Where(i => i < max).Count();
}
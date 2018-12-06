<Query Kind="Statements">
  <NuGetReference>morelinq</NuGetReference>
  <NuGetReference>Shouldly</NuGetReference>
  <Namespace>MoreLinq.Extensions</Namespace>
  <Namespace>Shouldly</Namespace>
</Query>

// sample

var sampleBoard = new[] {
    "1, 1",
    "1, 6",
    "8, 3",
    "3, 4",
    "5, 5",
    "8, 9",
    };

LargestFiniteArea(sampleBoard).ShouldBe(17);
SafeRegionSize(sampleBoard, 32).ShouldBe(16);

// problem

var scriptDir = Path.GetDirectoryName(Util.CurrentQueryPath);

LargestFiniteArea(File.ReadLines($"{scriptDir}/input.txt")).Dump().ShouldBe(3420);
SafeRegionSize(File.ReadLines($"{scriptDir}/input.txt"), 10000).Dump().ShouldBe(46667);

int LargestFiniteArea(IEnumerable<string> textCoords)
{
    var coords = textCoords
        .Select(tc => tc.Split(','))
        .Select(ts => (x: int.Parse(ts[0]), y: int.Parse(ts[1]))).ToList();

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
    
    return grid.Cast<int>()
        .Where(i => i >= 0 && !isInfinite[i-1])
        .GroupBy(i => i)
        .Max(g => g.Count());
}

int SafeRegionSize(IEnumerable<string> textCoords, int max)
{
    var coords = textCoords
        .Select(tc => tc.Split(','))
        .Select(ts => (x: int.Parse(ts[0]), y: int.Parse(ts[1]))).ToList();

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
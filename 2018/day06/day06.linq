<Query Kind="Program">
  <NuGetReference>morelinq</NuGetReference>
  <NuGetReference>Shouldly</NuGetReference>
  <Namespace>MoreLinq.Extensions</Namespace>
  <Namespace>Shouldly</Namespace>
  <Namespace>System.Drawing</Namespace>
  <Namespace>System.Drawing.Imaging</Namespace>
</Query>

void Main()
{
    // sample

    var sample = Parse(new[] {
        "1, 1",
        "1, 6",
        "8, 3",
        "3, 4",
        "5, 5",
        "8, 9",
        }).ToList();

    LargestFiniteArea(sample).ShouldBe(17);
    SafeRegionSize(sample, 32).ShouldBe(16);

    // problem

    var scriptDir = Path.GetDirectoryName(Util.CurrentQueryPath);
    var input = Parse(File.ReadLines($"{scriptDir}/input.txt")).ToList();

    LargestFiniteArea(input).Dump().ShouldBe(3420);
    Render(input, $"{scriptDir}/map.png");
    SafeRegionSize(input, 10000).Dump().ShouldBe(46667);
    Render(input, $"{scriptDir}/safe.png", 10000);

    IEnumerable<(int x, int y)> Parse(IEnumerable<string> textCoords) => textCoords
        .Select(tc => tc.Split(','))
        .Select(ts => (x: int.Parse(ts[0]), y: int.Parse(ts[1])));
}

struct Rect
{
    public int l, t, r, b;
    public int cx { get { return r - l; } }
    public int cy { get { return b - t; } }
}

Rect GetBounds(IReadOnlyList<(int x, int y)> coords) => new Rect {
    l = coords.Min(c => c.x), t = coords.Min(c => c.y),
    r = coords.Max(c => c.x) + 1, b = coords.Max(c => c.y) + 1 };

int LargestFiniteArea(IReadOnlyList<(int x, int y)> coords)
{
    var bounds = GetBounds(coords);

    return Enumerable
        .Range(0, bounds.cy)
        .AsParallel()
        .Select(y =>
        {
            var claimCount = new int[coords.Count];
            for (var x = 0; x < bounds.cx; ++x)
            {
                var claims = coords
                    .Select((c, i) => (id: i, dist: Math.Abs(c.x - bounds.l - x) + Math.Abs(c.y - bounds.t - y)))
                    .MinBy(v => v.dist);

                int? id = null;
                var claimed = false;
                
                foreach (var claim in claims)
                {
                    id = claimed ? (int?)null : claim.id;
                    claimed = true;
                }

                if (id != null && claimCount[id.Value] != -1)
                {
                    if (x == 0 || y == 0 || x == bounds.cx - 1 || y == bounds.cy - 1)
                        claimCount[id.Value] = -1;
                    else
                        ++claimCount[id.Value];
                }
            }
            return claimCount;
        })
        .Aggregate(
            new int[coords.Count],
            (totals, claims) =>
            {
                claims.ForEach((c, i) => totals[i] += c);
                return totals;
            })
        .Max();
}

int SafeRegionSize(IReadOnlyList<(int x, int y)> coords, int max)
{
    var bounds = GetBounds(coords);
    return (
        from y in Enumerable.Range(bounds.t, bounds.b - bounds.t)
        from x in Enumerable.Range(bounds.l, bounds.r - bounds.l)
        let dist = coords.Sum(c => Math.Abs(c.x - x) + Math.Abs(c.y - y))
        where dist < max
        select dist)
        .Count();
}

void Render(IReadOnlyList<(int x, int y)> coords, string path, int safeAreasMax = 0)
{
    var bounds = GetBounds(coords);
    var grid = new (int id, int dist)[bounds.cx, bounds.cy];
    var (maxId, maxDist) = (0, 0);

    for (var y = 0; y < bounds.cy; ++y)
    {
        for (var x = 0; x < bounds.cx; ++x)
        {
            var claims = coords
                .Select((c, i) => (id: i, dist: Math.Abs(c.x - bounds.l - x) + Math.Abs(c.y - bounds.t - y)))
                .MinBy(v => v.dist);

            int? id = null;
            int dist = 0;
            var claimed = false;

            foreach (var claim in claims)
            {
                maxId = Math.Max(maxId, claim.id);
                maxDist = Math.Max(maxDist, claim.dist);
                
                id = claimed ? (int?)null : claim.id;
                dist = claim.dist;
                claimed = true;
            }

            grid[x, y] = (id: id.HasValue ? id.Value : -1, dist);
        }
    }

    var r = new Random(0);
    var idColors = Enumerable
        .Range(0, maxId + 1)
        .Select(_ => Color.FromArgb(255, r.Next(100, 255), r.Next(100, 255), r.Next(100, 255)))
        .ToList();

    using (var b = new Bitmap(bounds.cx, bounds.cy))
    {
        for (int y = 0; y < bounds.cy; ++y)
        {
            for (int x = 0; x < bounds.cx; ++x)
            {
                Color c;
                
                var id = grid[x, y].id;
                if (id < 0)
                    c = Color.White;
                else if (safeAreasMax != 0)
                    c = Color.FromArgb(255, idColors[id].R, idColors[id].R, idColors[id].R);
                else
                {
                    var scale = 1.0 - ((double)(grid[x, y].dist) / maxDist);
                    c = idColors[id];
                    c = Color.FromArgb(255, (int)(scale * c.R), (int)(scale * c.G), (int)(scale * c.B));
                }

                b.SetPixel(x, y, c);
            }
        }

        if (safeAreasMax != 0)
        {
            foreach (var point in 
                from y in Enumerable.Range(bounds.t, bounds.b - bounds.t)
                from x in Enumerable.Range(bounds.l, bounds.r - bounds.l)
                let dist = coords.Sum(c => Math.Abs(c.x - bounds.l - x) + Math.Abs(c.y - bounds.t - y))
                where dist < safeAreasMax
                select (x, y, dist))
            {
                b.SetPixel(point.x, point.y, Color.LawnGreen);
            }
        }
        
        foreach (var coord in coords)
        {
            b.SetPixel(coord.x - bounds.l, coord.y - bounds.t, Color.Red);
        }

        b.Save(path, ImageFormat.Png);
    }
}
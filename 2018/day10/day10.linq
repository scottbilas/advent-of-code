<Query Kind="Statements">
  <NuGetReference>morelinq</NuGetReference>
  <NuGetReference>Shouldly</NuGetReference>
  <NuGetReference>YC.QuickGraph</NuGetReference>
  <Namespace>MoreLinq.Extensions</Namespace>
  <Namespace>QuickGraph</Namespace>
  <Namespace>Shouldly</Namespace>
  <Namespace>System.Linq</Namespace>
</Query>

var scriptDir = Path.GetDirectoryName(Util.CurrentQueryPath);

// sample

Render(File.ReadLines($"{scriptDir}/sample.txt"), 10);

// problem

Render(File.ReadLines($"{scriptDir}/input.txt"), 15);

void Render(IEnumerable<string> lines, int windowHeight)
{
    var positions = new List<(int x, int y)>();
    var velocities = new List<(int x, int y)>();

    lines
        .Select(line => Regex
            .Matches(line, @"-?\d+").Cast<Match>()
            .Select(m => int.Parse(m.Value))
            .ToList())
        .ForEach(ints =>
        {
            positions.Add((ints[0], ints[1]));
            velocities.Add((ints[2], ints[3]));
        });

    var rendering = false;
    for (var time = 0;; ++time)
    {
        var bounds = (
            l: positions.Min(v => v.x), r: positions.Max(v => v.x) + 1,
            t: positions.Min(v => v.y), b: positions.Max(v => v.y) + 1);
        var dims = (w: bounds.r - bounds.l, h: bounds.b - bounds.t);
        if (dims.h < windowHeight)
        {
            rendering = true;

            var grid = new char[bounds.r - bounds.l, bounds.b - bounds.t];
            for (int y = 0; y < dims.h; ++y)
                for (int x = 0; x < dims.w; ++x)
                    grid[x, y] = ' ';

            foreach (var pos in positions)
                grid[pos.x - bounds.l, pos.y - bounds.t] = '#';

            var sb = new StringBuilder();
            for (int y = 0; y < dims.h; ++y)
            {
                for (int x = 0; x < dims.w; ++x)
                    sb.Append(grid[x, y]);
                sb.Append('\n');
            }

            Console.WriteLine(time);
            Console.WriteLine(sb.ToString());
        }
        else if (rendering)
            return;

        for (int i = 0; i < positions.Count; ++i)
        {
            var pos = positions[i];
            var vel = velocities[i];
            positions[i] = (pos.x + vel.x, pos.y + vel.y);
        }
    }
}

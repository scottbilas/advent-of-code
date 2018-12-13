<Query Kind="Program">
  <NuGetReference>morelinq</NuGetReference>
  <NuGetReference>Shouldly</NuGetReference>
  <Namespace>MoreLinq.Extensions</Namespace>
  <Namespace>Shouldly</Namespace>
  <Namespace>System.Linq</Namespace>
</Query>

void Main()
{
    var scriptDir = Path.GetDirectoryName(Util.CurrentQueryPath);

    // sample

    int GridPos(int x, int y) => ((y-1)*300)+x-1;
    MakeGrid(57)[GridPos(122, 79)].ShouldBe(-5);
    MakeGrid(39)[GridPos(217, 196)].ShouldBe(0);
    MakeGrid(71)[GridPos(101, 153)].ShouldBe(4);

    var (sums18, sums42) = (MakeGridSums(18), MakeGridSums(42));

    FindLargest(sums18, 3).pos.ShouldBe(new Pos(33, 45));
    FindLargest(sums42, 3).pos.ShouldBe(new Pos(21, 61));

    FindLargestLargest(sums18).ShouldBe((new Pos(90, 269), 16));
    FindLargestLargest(sums42).ShouldBe((new Pos(232, 251), 12));

    // problem

    var sums8979 = MakeGridSums(8979);
    var largest8979_3 = FindLargest(sums8979, 3);
    largest8979_3.pos.ToString().Dump();
    largest8979_3.pos.ShouldBe(new Pos(33, 34));

    var largest8979 = FindLargestLargest(sums8979);
    $"{largest8979.pos},{largest8979.size}".Dump();
    largest8979.ShouldBe((new Pos(235, 118), 14));
}

struct Pos
{
    public int X, Y;
    public Pos(int x, int y) { X = x; Y = y; }
    public override string ToString() => $"{X},{Y}";
}

(Pos pos, int size) FindLargestLargest(int[] sums)
{
    int maxSize = 0;
    int maxPower = 0;
    var pos = new Pos();
    
    for (var s = 1; s <= 300; ++s)
    {
        var largest = FindLargest(sums, s);

        if (largest.power > maxPower)
        {
            maxSize = s;
            maxPower = largest.power;
            pos = largest.pos;
        }
    };
    
    return (pos, maxSize);
}

(Pos pos, int power) FindLargest(int[] sums, int size)
{
    return Enumerable
        .Range(0, 300 - (size - 1))
        .AsParallel()
        .Select(y =>
        {
            var maxPower = 0;
            var maxPos = new Pos();

            var y0off = y * 301;
            var y1off = (y + size) * 301;

            for (var x = 0; x < (300 - (size - 1)); ++x)
            {
                var total
                    = sums[x+size+y1off] - sums[x+y1off] - sums[x+size+y0off]
                    + sums[x+y0off];
                if (total > maxPower)
                {
                    maxPower = total;
                    maxPos = new Pos(x + 1, y + 1);
                }
            }
            
            return (maxPos, maxPower);
        })
        .MaxBy(v => v.maxPower)
        .First();
}

int[] MakeGrid(int serial)
{
    var grid = new int[300 * 300];
    for (var (y, i) = (0, 0); y < 300; ++y)
    {
        for (var x = 0; x < 300; ++x)
        {
            var rackId = x + 1 + 10;
            var power = rackId * (y + 1);
            power += serial;
            power *= rackId;
            power = (power / 100) % 10;
            power -= 5;
            
            grid[i++] = power;
        }
    }

    return grid;
}

int[] MakeGridSums(int serial)
{
    var grid = MakeGrid(serial);
    var sums = new int[301 * 301];

    int GridPos(int x, int y) => y * 300 + x;
    int SumPos(int x, int y) => y * 301 + x;

    for (var y = 1; y < 301; ++y)
        for (var x = 1; x < 301; ++x)
            sums[SumPos(x, y)] = sums[SumPos(x-1, y)] + sums[SumPos(x, y-1)] - sums[SumPos(x-1, y-1)] + grid[GridPos(x-1, y-1)];

    return sums;
}

<Query Kind="Program">
  <NuGetReference>morelinq</NuGetReference>
  <NuGetReference>Shouldly</NuGetReference>
  <Namespace>MoreLinq.Extensions</Namespace>
  <Namespace>Shouldly</Namespace>
  <Namespace>System.Drawing</Namespace>
  <Namespace>System.Drawing.Imaging</Namespace>
  <Namespace>System.Linq</Namespace>
  <Namespace>System.Drawing.Drawing2D</Namespace>
</Query>

void Main()
{
    var scriptDir = Path.GetDirectoryName(Util.CurrentQueryPath);

    // sample

    MakeGrid(57)[122-1, 79-1].ShouldBe(-5);
    MakeGrid(39)[217-1, 196-1].ShouldBe(0);
    MakeGrid(71)[101-1, 153-1].ShouldBe(4);

    var (grid18, grid42) = (MakeGrid(18), MakeGrid(42));

    FindLargest(grid18, 3).pos.ShouldBe(new Pos(33, 45));
    FindLargest(grid42, 3).pos.ShouldBe(new Pos(21, 61));

    FindLargestLargest(grid18).ShouldBe((new Pos(90, 269), 16));
    FindLargestLargest(grid42).ShouldBe((new Pos(232, 251), 12));

    // problem

    var grid8979 = MakeGrid(8979);
    FindLargest(grid8979, 3).pos.Dump().ShouldBe(new Pos(33, 34));

    //FindLargestLargest(8979).Dump().ShouldBe(235, 118, 14);
}

struct Pos
{
    public int X, Y;
    
    public Pos(int x, int y) { X = x; Y = y; }

    public override string ToString() => $"{X},{Y}";
}

(Pos pos, int size) FindLargestLargest(int[,] grid)
{
    int maxSize = 0;
    int maxPower = 0;
    var pos = new Pos();
    
    for (var s = 1; s <= 300; ++s)
    {
        var largest = FindLargest(grid, s);

        if (largest.power > maxPower)
        {
            maxSize = s;
            maxPower = largest.power;
            pos = largest.pos;

            Console.WriteLine($"[{maxSize}: {pos.X},{pos.Y}] = {maxPower}");
        }
    };
    
    return (pos, maxSize);
}

(Pos pos, int power) FindLargest(int[,] grid, int size)
{
    var maxPower = 0;
    var maxPos = new Pos();

    for (var y = 0; y < (300 - (size - 1)); ++y)
    {
        for (var x = 0; x < (300 - (size - 1)); ++x)
        {
            var total = 0;
            for (var dy = 0; dy < size; ++dy)
                for (var dx = 0; dx < size; ++dx)
                    total += grid[x + dx, y + dy];
            if (total > maxPower)
            {
                maxPower = total;
                maxPos = new Pos(x + 1, y + 1);
            }
        }
    }
    
    return (maxPos, maxPower);
}

int[,] MakeGrid(int serial)
{
    var grid = new int[300, 300];
    for (var y = 0; y < 300; ++y)
    {
        for (var x = 0; x < 300; ++x)
        {
            var rackId = x + 1 + 10;
            var power = rackId * (y + 1);
            power += serial;
            power *= rackId;
            power = (power / 100) % 10;
            power -= 5;
            
            grid[x, y] = power;
        }
    }
    
    return grid;
}
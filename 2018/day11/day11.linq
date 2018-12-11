<Query Kind="Statements">
  <NuGetReference>morelinq</NuGetReference>
  <NuGetReference>Shouldly</NuGetReference>
  <Namespace>MoreLinq.Extensions</Namespace>
  <Namespace>Shouldly</Namespace>
  <Namespace>System.Drawing</Namespace>
  <Namespace>System.Drawing.Imaging</Namespace>
  <Namespace>System.Linq</Namespace>
  <Namespace>System.Drawing.Drawing2D</Namespace>
</Query>

var scriptDir = Path.GetDirectoryName(Util.CurrentQueryPath);

// sample

MakeGrid(57)[122, 79].ShouldBe(-5);
MakeGrid(39)[217,196].ShouldBe(0);
MakeGrid(71)[101,153].ShouldBe(4);

//FindLargest(18, 3).ShouldBe((33, 45));
//FindLargest(42, 3).ShouldBe((21, 61));

// problem

// FindLargest()/ ?? lost it

FindLargestLargest(8979).Dump();


(int x, int y, int size) FindLargestLargest(int serial)
{
    int maxSize = 0;
    int maxPower = 0;
    int x = 0, y = 0;
    
    var grid = MakeGrid(serial);
    
    for (var s = 1; s <= 300; ++s)
    {
        var largest = FindLargest(grid, serial, s);

        if (largest.power > maxPower)
        {
            maxSize = s;
            maxPower = largest.power;
            x = largest.x;
            y = largest.y;
        }

        Console.WriteLine($"[{s}: {maxPower}]");
    };
    
    return (x, y, maxSize);
}


(int x, int y, int power) FindLargest(int[,] grid, int serial, int size)
{
    (int lx, int ly) = (0, 0);
    var maxPower = 0;
    
    for (var y = 1; y <= (300 - (size - 1)); ++y)
    {
        for (var x = 1; x <= (300 - (size - 1)); ++x)
        {
            var total = 0;
            for (var dy = 0; dy < size; ++dy)
                for (var dx = 0; dx < size; ++dx)
                    total += grid[x + dx, y + dy];
            if (total > maxPower)
            {
                maxPower = total;
                lx = x;
                ly = y;
            }
        }
    }
    
    return (lx, ly, maxPower);
}

int[,] MakeGrid(int serial)
{
    var grid = new int[301, 301];
    for (var y = 1; y <= 300; ++y)
    {
        for (var x = 1; x <= 300; ++x)
        {
            var rackId = x + 10;
            var power = rackId * y;
            power += serial;
            power *= rackId;
            power = (power / 100) % 10;
            power -= 5;
            
            grid[x, y] = power;
        }
    }
    
    return grid;
}

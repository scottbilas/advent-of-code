<Query Kind="Program">
  <Reference Relative="..\..\libaoc\bin\Debug\net472\libaoc2018.dll">C:\proj\advent-of-code\2018\libaoc\bin\Debug\net472\libaoc2018.dll</Reference>
  <NuGetReference>JonSkeet.MiscUtil</NuGetReference>
  <NuGetReference>morelinq</NuGetReference>
  <NuGetReference>Shouldly</NuGetReference>
  <Namespace>Aoc2018</Namespace>
  <Namespace>JetBrains.Annotations</Namespace>
  <Namespace>MoreLinq.Extensions</Namespace>
  <Namespace>NiceIO</Namespace>
  <Namespace>Shouldly</Namespace>
  <Namespace>System</Namespace>
  <Namespace>System.Collections</Namespace>
  <Namespace>System.Collections.Generic</Namespace>
  <Namespace>System.Linq</Namespace>
  <Namespace>System.Text</Namespace>
</Query>

NPath scriptDir = new NPath(Util.CurrentQueryPath).Parent;

void Main()
{
    Part1(@"
         0,0,0,0
         3,0,0,0
         0,3,0,0
         0,0,3,0
         0,0,0,3
         0,0,0,6
         9,0,0,0
        12,0,0,0
        ").ShouldBe(2);
    
    Part1(@"
        -1,2,2,0
        0,0,2,-2
        0,0,0,-2
        -1,2,0,0
        -2,-2,-2,2
        3,0,2,-1
        -1,3,2,2
        -1,0,-1,0
        0,2,1,-2
        3,0,0,0    
        ").ShouldBe(4);
    
    Part1(@"
        1,-1,0,1
        2,0,-1,0
        3,2,-1,0
        0,0,3,1
        0,0,-1,-1
        2,3,-2,0
        -2,2,0,0
        2,-2,0,-1
        1,-1,0,-1
        3,2,0,2
        ").ShouldBe(3);
    
    Part1(@"
        1,-1,-1,-2
        -2,-2,0,1
        0,2,1,3
        -2,3,-2,1
        0,2,3,-2
        -1,-1,1,-2
        0,-2,-1,0
        -2,2,3,-1
        1,2,2,0
        -1,-2,0,-2
        ").ShouldBe(8);
    
    Part1(scriptDir.Combine("input.txt").ReadAllText()).Dump().ShouldBe(373);
}

class Point
{
    public readonly int Index;
    public readonly Int4 Pos;

    public Int4 Next;
    public int GroupIndex = -1;

    public Point(int index, Int4 pos)
    {
        Index = index;
        Pos = pos;
        Next = new Int4(index);
    }

    public override string ToString()
        => $"#{Index} ({Pos}) >{Next}";

    public bool InGroup => GroupIndex != -1;

    public IEnumerable<Point> GetRing(IReadOnlyList<Point> coords, int componentIndex)
    {
        int i = Index;
        do
        {
            var coord = coords[i];
            yield return coord;
            i = coord.Next[componentIndex];
        }
        while (i != Index);
    }
}

IEnumerable<Point> Parse(string coordsText)
    => coordsText
        .SelectInts()
        .Batch4()
        .Select((b, i) => new Point(i, new Int4(b)));

int Part1(string coordsText, int maxSpacing = 3)
{
    var coords = Parse(coordsText).ToList();

    // build constellation rings along each axis
    for (var i = 0; i < 4; ++i)
    {
        (int p, Point c)? last = null;
        foreach (var coord in coords
            .Select(c => (p: c.Pos[i], c))
            .OrderBy(v => v.p))
        {
            if (last != null && (coord.p - last.Value.p) <= maxSpacing)
            {
                coord.c.Next[i] = last.Value.c.Next[i];
                last.Value.c.Next[i] = coord.c.Index;
            }

            last = coord;
        }
    }

    var groupIndex = 0;
    var walking = new Queue<Point>();

    foreach (var outerCoord in coords.Where(c => !c.InGroup))
    {
        walking.Enqueue(outerCoord);
        while (walking.Any())
        {
            var coord = walking.Dequeue();
            
            foreach (var near in Enumerable
                .Range(0, 4)
                .SelectMany(i => coord.GetRing(coords, i))
                .Where(c => !c.InGroup && c.Pos.ManhattanDistance(coord.Pos) <= maxSpacing))
            {
                near.GroupIndex = groupIndex;
                walking.Enqueue(near);
            }
        }
        
        ++groupIndex;
    }

    return groupIndex;
}
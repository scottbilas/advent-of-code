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
</Query>

void Main()
{
    var folder = Util.CurrentQueryPath.ToNPath().Parent;
    var inputPath = folder.Combine($"{folder.FileName}.input.txt");

    var mem = inputPath.ReadAllInts();

// --- PART 1 ---

    var (grid, sum) = Solve1(mem);
    sum.Dump().ShouldBe(5740);

// --- PART 2 ---

    Solve2(mem, grid).Dump().ShouldBe(1022165);
}

(char[,] grid, int sum) Solve1(IEnumerable<int> mem)
{
    var grid = new IntCodeVM(mem).Run().Select(i => (char)i).ToStringFromChars().ToGrid();

    var sum = 0;
    
    var dims = grid.GetDimensions();
    for (var y = 1; y < dims.Y - 1; ++y)
    {
        for (var x = 1; x < dims.X - 1; ++x)
        {
            if (grid[x, y] != '#') continue;
            var count =
                (grid[x - 1, y] == '#' ? 1 : 0) +
                (grid[x + 1, y] == '#' ? 1 : 0) +
                (grid[x, y - 1] == '#' ? 1 : 0) +
                (grid[x, y + 1] == '#' ? 1 : 0);
            if (count > 2)
            {
                sum += x * y;
            }
        }
    }
    
    return (grid, sum);
}

enum Dir { N, E, S, W }

int Solve2(IEnumerable<int> mem, char[,] grid)
{
    // this isn't really a "solver" - it just runs the program i generated by spending a few minutes
    // tetris-ing segments with renderer assist.

    var program = @"
        A,C,C,A,B,A,B,A,B,C
        R,6,R,6,R,8,L,10,L,4
        L,4,L,12,R,6,L,10
        R,6,L,10,R,8
        ".SelectLines().ToArray();
    
    var input = program
        .Append("\n", "")
        .StringJoin('\n')
        .Select(c => (int)c)
        .ToQueue();

    var answer = new IntCodeVM(mem.Skip(1).Prepend(2), () => input.Dequeue()).Run().Last();

    /*
    // render segments
    
    grid = grid.AddBorder('.');
    var (pos, dir) = grid
        .SelectCells()
        .Where(c => c.cell != '#' && c.cell != '.')
        .Select(c => (c.pos, c.cell.Translate(('^', Dir.N), ('>', Dir.E), ('v', Dir.S), ('<', Dir.W))))
        .Single();
    Render(Sim(program, grid, pos, dir));
    */

    return answer;
}

// --- SUPPORT ---

void Render(char[,] grid)
{
    Color select(char c)
    {
        switch (c)
        {
            case '.': return Color.Black;
            case '#': return Color.White;
            case '^': case '<': case '>': case 'v': return Color.Gray;
            case 'A': return Color.Aqua;
            case 'B': return Color.BlueViolet;
            case 'C': return Color.Chocolate;
            case 'X': return Color.Red;
        }
        return Color.Pink;
    }

    Util.Image(grid.ToPng(select, 20, 10)).Dump();
}

(Dir r, Dir l, Int2 o)[] Moves = new[]
{
    (Dir.E, Dir.W, new Int2( 0, -1)),
    (Dir.S, Dir.N, new Int2( 1,  0)),
    (Dir.W, Dir.E, new Int2( 0,  1)),
    (Dir.N, Dir.S, new Int2(-1,  0)),
};

char[,] Sim(string[] program, char[,] grid, Int2 pos, Dir dir)
{
    grid = (char[,])grid.Clone();

    foreach (var (name, op) in program[0]
        .Split(',')
        .Select(part => (name: part[0], prog: program[part[0] - 'A' + 1]))
        .SelectMany(sub => sub.prog.Split(",").Select(op => (sub.name, op))))
    {
        if (int.TryParse(op, out var count))
        {
            for (var i = 0; i < count; ++i)
            {
                pos += Moves[(int)dir].o;
                if (grid.GetAt(pos) != '.')
                    grid[pos.X, pos.Y] = name;
                else
                    grid[pos.X, pos.Y] = 'X';
            }
        }
        else
        {
            var move = Moves[(int)dir];
            dir = op == "L" ? move.l : move.r;
        }
    }

    return grid;
}
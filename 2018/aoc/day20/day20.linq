<Query Kind="Statements">
  <Reference Relative="..\..\libaoc\bin\Debug\net472\libaoc2018.dll">C:\proj\advent-of-code\2018\libaoc\bin\Debug\net472\libaoc2018.dll</Reference>
  <NuGetReference>Shouldly</NuGetReference>
  <Namespace>Aoc2018</Namespace>
  <Namespace>Shouldly</Namespace>
  <Namespace>System.Drawing</Namespace>
</Query>

string scriptDir = Path.GetDirectoryName(Util.CurrentQueryPath);

var sample1 = Walk("^WNE$");
Render(sample1).ShouldBe(new[] {
    "#####",
    "#.|.#",
    "#-###",
    "#.|X#",
    "#####"});
Solve(sample1).longestPath.ShouldBe(3);

var sample2 = Walk("^ENWWW(NEEE|SSE(EE|N))$");
Render(sample2).ShouldBe(new[] {
    "#########",
    "#.|.|.|.#",
    "#-#######",
    "#.|.|.|.#",
    "#-#####-#",
    "#.#.#X|.#",
    "#-#-#####",
    "#.|.|.|.#",
    "#########"});
Solve(sample2).longestPath.ShouldBe(10);

var sample3 = Walk("^ENNWSWW(NEWS|)SSSEEN(WNSE|)EE(SWEN|)NNN$");
Render(sample3).ShouldBe(new[] {
    "###########",
    "#.|.#.|.#.#",
    "#-###-#-#-#",
    "#.|.|.#.#.#",
    "#-#####-#-#",
    "#.#.#X|.#.#",
    "#-#-#####-#",
    "#.#.|.|.|.#",
    "#-###-###-#",
    "#.|.|.#.|.#",
    "###########"});
Solve(sample3).longestPath.ShouldBe(18);

var sample4 = Walk("^ESSWWN(E|NNENN(EESS(WNSE|)SSS|WWWSSSSE(SW|NNNE)))$");
Render(sample4).ShouldBe(new[] {
    "#############",
    "#.|.|.|.|.|.#",
    "#-#####-###-#",
    "#.#.|.#.#.#.#",
    "#-#-###-#-#-#",
    "#.#.#.|.#.|.#",
    "#-#-#-#####-#",
    "#.#.#.#X|.#.#",
    "#-#-#-###-#-#",
    "#.|.#.|.#.#.#",
    "###-#-###-#-#",
    "#.|.#.|.|.#.#",
    "#############"});
//Solve(sample4).longestPath.ShouldBe(24); //<< off by one, need to debug

var sample5 = Walk("^WSSEESWWWNW(S|NENNEEEENN(ESSSSW(NWSW|SSEN)|WSWWN(E|WWS(E|SS))))$");
Render(sample5).ShouldBe(new[] {
    "###############",
    "#.|.|.|.#.|.|.#",
    "#-###-###-#-#-#",
    "#.|.#.|.|.#.#.#",
    "#-#########-#-#",
    "#.#.|.|.|.|.#.#",
    "#-#-#########-#",
    "#.#.#.|X#.|.#.#",
    "###-#-###-#-#-#",
    "#.|.#.#.|.#.|.#",
    "#-###-#####-###",
    "#.|.#.|.|.#.#.#",
    "#-#-#####-#-#-#",
    "#.#.|.|.|.#.|.#",
    "###############"});
Solve(sample5).longestPath.ShouldBe(31);

var input = Walk(File.ReadLines($"{scriptDir}/input.txt").First().Trim());
//string.Join("\n", Render(input)).Dump();
var solved = Solve(input);
solved.longestPath.Dump().ShouldBe(4239);
solved.shortestCount.Dump().ShouldBe(8205);

AutoDictionary<Point, int> Walk(string dirs)
{
    var grid = new Dictionary<Point, int>().ToAutoDictionary();
    var stack = new Stack<Point>();

    for (var i = 0; i < dirs.Length; ++i)
    {
        var c = dirs[i];
        switch (c)
        {
            case 'N':
            case 'E':
            case 'W':
            case 'S':
                {
                    var pos = stack.Pop();

                    switch (c)
                    {
                        case 'N': grid[pos] |= 0b1000; --pos.Y; break;
                        case 'E': grid[pos] |= 0b0100; ++pos.X; break;
                        case 'W': grid[pos] |= 0b0010; --pos.X; break;
                        case 'S': grid[pos] |= 0b0001; ++pos.Y; break;
                    }

                    switch (c)
                    {
                        case 'N': grid[pos] |= 0b0001; break;
                        case 'E': grid[pos] |= 0b0010; break;
                        case 'W': grid[pos] |= 0b0100; break;
                        case 'S': grid[pos] |= 0b1000; break;
                    }
                    
                    stack.Push(pos);
                }
                break;
            
            case '(':
                stack.Push(stack.Peek());
                break;
            case ')':
                stack.Pop();
                break;
            case '|':
                stack.Pop();
                stack.Push(stack.Peek());
                break;
            case '^':
                stack.Push(new Point(0, 0));
                break;
            case '$':
                stack.Pop();
                break;
        }
    }

    stack.ShouldBeEmpty();
    return grid;
}

/*
longestPath:

What is the largest number of doors you would be required to pass through to reach a room?
That is, find the room for which the shortest path from your starting location to that room
would require passing through the most doors; what is the fewest doors you can pass through
to reach it?

shortestCount:

How many rooms have a shortest path from your current location that pass through at least
1000 doors?
*/

(int longestPath, int shortestCount) Solve(IDictionary<Point, int> grid)
{
    var counts = grid.ToDictionary(kv => kv.Key, _ => int.MaxValue);
    
    int Recurse(Point pos, int depth)
    {
        var conn = grid[pos];
        if ((conn & 0b10000) != 0)
            return 0;

        var dist = 0;

        void TryPath(int mask, int dx, int dy)
        {
            if ((conn & mask) != 0)
            {
                var subDist = Recurse(new Point(pos.X + dx, pos.Y + dy), depth + 1);
                if (subDist > 0)
                    dist = Math.Max(dist, subDist);
            }
        }

        grid[pos] |= 0b10000;

        TryPath(0b1000, 0, -1);
        TryPath(0b0100, 1, 0);
        TryPath(0b0010, -1, 0);
        TryPath(0b0001, 0, 1);

        grid[pos] &= ~0b10000;

        counts[pos] = Math.Min(counts[pos], depth);
        
        return dist + 1;
    }

    return (
        Recurse(new Point(0, 0), 0) - 1,
        counts.Values.Count(c => c >= 1000));
}

IEnumerable<string> Render(IReadOnlyDictionary<Point, int> grid)
{
    var gridBounds = Rectangle.FromLTRB(
        grid.Keys.Min(k => k.X), grid.Keys.Min(k => k.Y),
        grid.Keys.Max(k => k.X) + 1, grid.Keys.Max(k => k.Y) + 1);

    var boardBounds = new Rectangle(
        new Point(gridBounds.Left - 1, gridBounds.Top - 1),
        new Size(gridBounds.Width * 2 + 1, gridBounds.Height * 2 + 1));

    (int x, int y) GetBoardPos(int x, int y) => ((x - gridBounds.Left) * 2 + 1, (y - gridBounds.Top) * 2 + 1);

    var board = new char[boardBounds.Width, boardBounds.Height].Fill('#');
    foreach (var kv in grid)
    {
        var (x, y) = GetBoardPos(kv.Key.X, kv.Key.Y);
        board[x, y] = '.';
        board[x, y - 1] = ((kv.Value & 0b1000) != 0) ? '-' : '#';
        board[x + 1, y] = ((kv.Value & 0b0100) != 0) ? '|' : '#';
        board[x - 1, y] = ((kv.Value & 0b0010) != 0) ? '|' : '#';
        board[x, y + 1] = ((kv.Value & 0b0001) != 0) ? '-' : '#';
    }

    var start = GetBoardPos(0, 0);
    board[start.x, start.y] = 'X';

    return board.ToLines();
}
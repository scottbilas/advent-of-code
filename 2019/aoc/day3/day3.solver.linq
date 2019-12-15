<Query Kind="Program">
  <Reference Relative="..\..\..\temp\bin\aoc2019\Debug\netstandard2.1\libaoc2019.dll">C:\proj\advent-of-code\temp\bin\aoc2019\Debug\netstandard2.1\libaoc2019.dll</Reference>
  <Reference Relative="..\..\..\temp\bin\aoc2019\Debug\netstandard2.1\libutils.dll">C:\proj\advent-of-code\temp\bin\aoc2019\Debug\netstandard2.1\libutils.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\netstandard.dll</Reference>
  <NuGetReference>morelinq</NuGetReference>
  <NuGetReference>Shouldly</NuGetReference>
  <NuGetReference>System.Interactive</NuGetReference>
  <Namespace>Aoc2019</Namespace>
  <Namespace>JetBrains.Annotations</Namespace>
  <Namespace>MoreLinq.Extensions</Namespace>
  <Namespace>Shouldly</Namespace>
  <Namespace>System.Drawing</Namespace>
  <Namespace>Unity.Coding.Editor</Namespace>
  <Namespace>Unity.Coding.Utils</Namespace>
</Query>

void Main()
{
    var folder = Util.CurrentQueryPath.ToNPath().Parent;
    var inputPath = folder.Combine($"{folder.FileName}.input.txt");

    var (sample00, sample01) = (Parse("R8,U5,L5,D3"), Parse("U7,R6,D4,L4"));
    var (sample10, sample11) = (Parse("R75,D30,R83,U83,L12,D49,R71,U7,L72"), Parse("U62,R66,U55,R34,D71,R55,D58,R83"));
    var (sample20, sample21) = (Parse("R98,U47,R26,D63,R33,U87,L62,D20,R33,U53,R51"), Parse("U98,R91,D20,R16,D67,R40,U7,R15,U6,R7"));

    var (input0, input1) = inputPath.ReadAllLines().Select(Parse).Batch2().Single();

    // PART 1

    // sample

    Render(sample00, sample01).ShouldBe(
      @"...........
        .+-----+...
        .|.....|...
        .|..+--X-+.
        .|..|..|.|.
        .|.-X--+.|.
        .|..|....|.
        .|.......|.
        .o-------+.
        ...........
        ".TrimBlock());

    Solve1(sample00, sample01).ShouldBe(6);
    Solve1(sample10, sample11).ShouldBe(159);
    Solve1(sample20, sample21).ShouldBe(135);

    // problem
    
    Solve1(input0, input1).Dump().ShouldBe(529);

    // PART 2

    // sample

    Solve2(sample00, sample01).ShouldBe(30);
    Solve2(sample10, sample11).ShouldBe(610);
    Solve2(sample20, sample21).ShouldBe(410);

    // problem

    Solve2(input0, input1).Dump().ShouldBe(20386);
}

IEnumerable<(char, int)> Parse(string text) =>
    text.Split(',').Select(t => (t[0], int.Parse(t.Substring(1))));

int Solve(IEnumerable<(char, int)> wire0, IEnumerable<(char, int)> wire1, Func<Int2, Int2, int> costFunc)
{
    var board = new Dictionary<Int2, Int2>();
    
    Trace(wire0, 0, board);
    Trace(wire1, 1, board);

    var closest = int.MaxValue;
    foreach (var entry in board)
    {
        if (entry.Value.X != int.MaxValue && entry.Value.Y != int.MaxValue)
            closest = Math.Min(costFunc(entry.Key, entry.Value), closest);
    }

    return closest;
}

int Solve1(IEnumerable<(char, int)> wire0, IEnumerable<(char, int)> wire1)
    => Solve(wire0, wire1, (pos, _) => pos.ManhattanDistance());

int Solve2(IEnumerable<(char, int)> wire0, IEnumerable<(char, int)> wire1)
    => Solve(wire0, wire1, (_, steps) => steps.X + steps.Y);

void Trace(IEnumerable<(char dir, int len)> wire, int which, IDictionary<Int2, Int2> board)
{
    var pos = Int2.Zero;
    var steps = 0;

    foreach (var move in wire)
    {
        for (var i = 0; i < move.len; ++i)
        {
            ++steps;
            switch (move.dir)
            {
                case 'R': ++pos.X; break;
                case 'L': --pos.X; break;
                case 'D': ++pos.Y; break;
                case 'U': --pos.Y; break;
            }

            var old = board.GetValueOr(pos, Int2.MaxValue);
            old[which] = Math.Min(old[which], steps);
            board[pos] = old;
        }
    }
}

string Render(IEnumerable<(char, int)> wire0, IEnumerable<(char, int)> wire1)
{
    var board = new Dictionary<Int2, Int2>().ToAutoDictionary(_ => Int2.MaxValue);
    board[Int2.Zero] = Int2.Zero;

    Trace(wire0, 0, board);
    Trace(wire1, 1, board);

    var sb = new StringBuilder();

    var bounds = Rectangle.FromLTRB(
        board.Keys.Min(k => k.X), board.Keys.Min(k => k.Y),
        board.Keys.Max(k => k.X) + 1, board.Keys.Max(k => k.Y) + 1);

    for (var y = bounds.Top - 1; y <= bounds.Bottom; ++y)
    {
        for (var x = bounds.Left - 1; x <= bounds.Right; ++x)
        {
            var (l, t, r, b, o) = (
                board[new Int2(x - 1, y    )],
                board[new Int2(x    , y - 1)],
                board[new Int2(x + 1, y    )],
                board[new Int2(x    , y + 1)],
                board[new Int2(x    , y    )]);

            char c;
            if (o.Equals(Int2.Zero))
                c = 'o';
            else if (o.Equals(Int2.MaxValue))
                c = '.';
            else if (o.X != int.MaxValue && o.Y != int.MaxValue)
                c = 'X';
            else if (l.Equals(Int2.MaxValue) && r.Equals(Int2.MaxValue))
                c = '|';
            else if (t.Equals(Int2.MaxValue) && b.Equals(Int2.MaxValue))
                c = '-';
            else
                c = '+';

            sb.Append(c);
        }
        
        sb.Append('\n');
    }
    
    return sb.ToString();
}
<Query Kind="Program">
  <Reference Relative="..\..\..\temp\bin\libaoc2019\Debug\netstandard2.1\libaoc2019.dll">C:\proj\advent-of-code\temp\bin\aoc2019\Debug\netstandard2.1\libaoc2019.dll</Reference>
  <Reference Relative="..\..\..\temp\bin\libaoc2019\Debug\netstandard2.1\libutils.dll">C:\proj\advent-of-code\temp\bin\aoc2019\Debug\netstandard2.1\libutils.dll</Reference>
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

    // sample
    Solve1(new Simulator(@"
        ______
        ___##_
        __#X.#
        __O.#_
        ___#__
        ")).ShouldBe(2);

    // problem
    Solve1(new IntCodeRunner(mem)).Dump().ShouldBe(208);

// --- PART 2 ---

    // sample
    Solve2(new Simulator(@"
        _##___
        #.X##_
        #.#..#
        #.O.#_
        _###__
        ")).ShouldBe(4);

    // problem
    Solve2(new IntCodeRunner(mem)).Dump().ShouldBe(306);
}

enum Dir { North = 1, South = 2, West = 3, East = 4 }
enum Status { Wall = 0, Open = 1, Found = 2 }

static (Dir dir, Dir undo, Int2 move)[] WalkList = new[]
{
    (Dir.North, Dir.South, new Int2( 0, -1)),
    (Dir.South, Dir.North, new Int2( 0,  1)),
    (Dir.West,  Dir.East,  new Int2(-1,  0)),
    (Dir.East,  Dir.West,  new Int2( 1,  0)),
};

IReadOnlyDictionary<Int2, (Status status, int dist)> Fill(IRunner runner)
{
    var map = new Dictionary<Int2, (Status status, int dist)>();

    void Walk(Int2 pos, int dist)
    {
        foreach (var walk in WalkList)
        {
            var next = pos + walk.move;

            if (map.TryGetValue(next, out var v) && v.dist <= dist)
                continue;

            var status = runner.Next(walk.dir);
            map[next] = (status, dist);

            if (status != Status.Wall)
            {
                Walk(next, dist + 1);
                runner.Next(walk.undo);
            }
        }
    }

    map.Add(Int2.Zero, (Status.Open, 0));
    Walk(Int2.Zero, 1);

    return map;
}

int Solve1(IRunner runner) =>
    Fill(runner)
        .Values
        .Single(v => v.status == Status.Found)
        .dist;

int Solve2(IRunner runner) =>
    Fill(new Simulator(Fill(runner)))
        .Values
        .Where(v => v.status != Status.Wall)
        .Max(v => v.dist);

interface IRunner
{
    Status Next(Dir move);
}

class IntCodeRunner : IRunner
{
    int _input;
    IEnumerator<int> _enum;

    public IntCodeRunner(IEnumerable<int> mem) =>
        _enum = new IntCodeVM(mem, () => _input).Run().GetEnumerator();

    public Status Next(Dir move)
    {
        _input = (int)move;
        _enum.MoveNext();
        return (Status)_enum.Current;
    }
}

class Simulator : IRunner
{
    Int2 _pos = Int2.Zero;
    IReadOnlyDictionary<Int2, Status> _map;

    public Simulator(IReadOnlyDictionary<Int2, (Status status, int dist)> map) =>
        _map = map.ToDictionary(v => v.Key, v =>
        {
            if (v.Value.status == Status.Found)
                _pos = v.Key;
            return v.Value.status;
        });

    public Simulator(string grid) =>
        _map = grid.ToGrid().SelectCells().ToDictionary(c => c.pos, c =>
        {
            switch (c.cell)
            {
                case '#': case '_':
                    return Status.Wall;
                case 'O':
                    return Status.Found;
                case 'X':
                    _pos = c.pos;
                    break;
            }

            return Status.Open;
        });

    public Status Next(Dir move)
    {
        var hit = _pos + WalkList.Single(l => l.dir == move).move;
        var status = _map.GetValueOrDefault(hit);
        if (status != Status.Wall)
            _pos = hit;
        return status;
    }
}

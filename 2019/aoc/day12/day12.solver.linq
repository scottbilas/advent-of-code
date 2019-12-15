<Query Kind="Program">
  <Reference Relative="..\..\libaoc\bin\Debug\netstandard2.1\libaoc2019.dll">C:\proj\advent-of-code\2019\libaoc\bin\Debug\netstandard2.1\libaoc2019.dll</Reference>
  <Reference Relative="..\..\libaoc\bin\Debug\netstandard2.1\libutils.dll">C:\proj\advent-of-code\2019\libaoc\bin\Debug\netstandard2.1\libutils.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\netstandard.dll</Reference>
  <NuGetReference>Combinatorics</NuGetReference>
  <NuGetReference>morelinq</NuGetReference>
  <NuGetReference>Shouldly</NuGetReference>
  <NuGetReference>System.Interactive</NuGetReference>
  <Namespace>Aoc2019</Namespace>
  <Namespace>Combinatorics.Collections</Namespace>
  <Namespace>JetBrains.Annotations</Namespace>
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

    var input = inputPath.ReadAllText();

// --- PART 1 ---

    // *SAMPLES*

    var sample = @"
        <x=-1, y=0, z=2>
        <x=2, y=-10, z=-7>
        <x=4, y=-8, z=8>
        <x=3, y=5, z=-1>
        ";

    Solve1(10, sample).ShouldBe(179);
        
    Solve1(100, @"
        <x=-8, y=-10, z=0>
        <x=5, y=5, z=10>
        <x=2, y=-7, z=3>
        <x=9, y=-8, z=-3>
        ").ShouldBe(1940);

    // *PROBLEM*

    Solve1(1000, input).Dump().ShouldBe(7179);

// --- PART 2 ---

    // *SAMPLES*

    Solve2(sample).ShouldBe(2772);

    Solve2(@"
        <x=-8, y=-10, z=0>
        <x=5, y=5, z=10>
        <x=2, y=-7, z=3>
        <x=9, y=-8, z=-3>
        ").ShouldBe(4686774924);

    // *PROBLEM*

    Solve2(input).Dump().ShouldBe(428576638953552);

}

class Sim
{
    public Sim(string startingPositions)
    {
        _startPos = startingPositions.SelectInt3s().ToArray();
        _pos = _startPos.ToArray();
        _vel = new Int3[_pos.Length];
    }

    public void Step()
    {
        foreach (var (a, b) in Range(0, _pos.Length).Combinations2())
        {
            void UpdateVel(int c)
            {
                _vel[a][c] += _pos[b][c].CompareTo(_pos[a][c]);
                _vel[b][c] += _pos[a][c].CompareTo(_pos[b][c]);
            }

            for (var c = 0; c < 3; ++c)
                UpdateVel(c);
        }

        for (var i = 0; i < _pos.Length; ++i)
            _pos[i] += _vel[i];
    }

    public bool TestAtStart(int component) =>
        _pos.Select(p => p[component]).SequenceEqual(_startPos.Select(p => p[component])) &&
        _vel.All(v => v[component] == 0);

    public int CalcEnergy()
    {
        var total = 0;
        foreach (var (p, v) in _pos.Zip(_vel))
            total += With(p.Abs(), p => p.X + p.Y + p.Z) * With(v.Abs(), v => v.X + v.Y + v.Z);
        return total;
    }

    Int3[] _startPos, _pos, _vel;
}

int Solve1(int steps, string startingPositions)
{
    var sim = new Sim(startingPositions);
    
    for (var step = 0; step < steps; ++step)
        sim.Step();

    return sim.CalcEnergy();
}

long Solve2(string startingPositions)
{
    var found = new int[3]; // xyz

    var sim = new Sim(startingPositions);
    for (var step = 1; ; ++step)
    {
        sim.Step();
        
        for (var c = 0; c < 3; ++c)
            if (found[c] == 0 && sim.TestAtStart(c))
                found[c] = step;

        if (found.All(f => f != 0))
            return Lcm(found);
    }
}

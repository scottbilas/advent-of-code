<Query Kind="Program">
  <Reference Relative="..\..\..\temp\bin\libaoc2019\Debug\netstandard2.1\libaoc2019.dll">C:\proj\advent-of-code\temp\bin\libaoc2019\Debug\netstandard2.1\libaoc2019.dll</Reference>
  <Reference Relative="..\..\..\temp\bin\libaoc2019\Debug\netstandard2.1\libutils.dll">C:\proj\advent-of-code\temp\bin\libaoc2019\Debug\netstandard2.1\libutils.dll</Reference>
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
  <Namespace>System.Numerics</Namespace>
  <Namespace>Unity.Coding.Editor</Namespace>
  <Namespace>Unity.Coding.Utils</Namespace>
  <Namespace>System.Drawing</Namespace>
</Query>

void Main()
{
    var folder = Util.CurrentQueryPath.ToNPath().Parent;
    var inputPath = folder.Combine($"{folder.FileName}.input.txt");

    _mem = inputPath.ReadAllInts().ToArray();

// --- PART 1 ---

    Solve1().Dump().ShouldBe(116);

// --- PART 2 ---

    Solve2().Dump().ShouldBe(10311666);
}

int[] _mem;

bool TestPos(int x, int y) =>
    new IntCodeVM(_mem).Run(x, y).Single() == 1;

int Solve1()
{
    var count = 0;
    for (var y = 0; y < 50; ++y)
    {
        for (var x = 0; x < 50; ++x)
        {
            if (TestPos(x, y))
                ++count;
        }
    }

    return count;
}

int Solve2()
{
    const int target = 100;
    var (l, r, y) = (500, 0, 1000);

    var rhist = new Queue<int>();
    void NextLine()
    {
        while (!TestPos(l, y)) ++l;
        while (TestPos(r + 1, y)) ++r;
        rhist.Enqueue(r);

        ++y;
    }

    NextLine();
    r = l;

    while (rhist.Count < target || (rhist.Dequeue() - l + 1) < target)
        NextLine();

    return (l * 10000) + (y - target);
}

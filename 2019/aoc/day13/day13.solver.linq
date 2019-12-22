<Query Kind="Program">
  <Reference Relative="..\..\..\temp\bin\libaoc2019\Debug\netstandard2.1\libaoc2019.dll">C:\proj\advent-of-code\temp\bin\aoc2019\Debug\netstandard2.1\libaoc2019.dll</Reference>
  <Reference Relative="..\..\..\temp\bin\libaoc2019\Debug\netstandard2.1\libutils.dll">C:\proj\advent-of-code\temp\bin\aoc2019\Debug\netstandard2.1\libutils.dll</Reference>
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

    var mem = inputPath.ReadAllInts().ToArray();

// --- PART 1 ---

    Solve1(mem).Dump().ShouldBe(324);

// --- PART 2 ---

    Solve2(mem).Dump().ShouldBe(15957);
}

int Solve1(int[] mem) =>
    new IntCodeVM(mem).Run().Batch3().Count(v => v.Item3 == 2);

int Solve2(int[] mem)
{
    mem[0] = 2;
    var (score, ball, paddle) = (0, 0, 0);

    var vm = new IntCodeVM(mem, () => ball.CompareTo(paddle));
    foreach (var (x, _, id) in vm.Run().Batch3())
    {
        if (x < 0)
            score = id;
        else if (id == 3)
            paddle = x;
        else if (id == 4)
            ball = x;
    }

    return score;
}

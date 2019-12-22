<Query Kind="Program">
  <Reference Relative="..\..\..\temp\bin\libaoc2019\Debug\netstandard2.1\libaoc2019.dll">C:\proj\advent-of-code\temp\bin\aoc2019\Debug\netstandard2.1\libaoc2019.dll</Reference>
  <Reference Relative="..\..\..\temp\bin\libaoc2019\Debug\netstandard2.1\libutils.dll">C:\proj\advent-of-code\temp\bin\aoc2019\Debug\netstandard2.1\libutils.dll</Reference>
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

    // PART 1

    // sample

    Solve1(12).ShouldBe(2);
    Solve1(14).ShouldBe(2);
    Solve1(1969).ShouldBe(654);
    Solve1(100756).ShouldBe(33583);

    // problem

    inputPath.ReadAllInts()
        .Sum(Solve1)
        .Dump()
        .ShouldBe(3249140);
        
    // PART 2
    
    // sample
    
    Solve2(14).ShouldBe(2);
    Solve2(1969).ShouldBe(966);
    Solve2(100756).ShouldBe(50346);

    // problem

    inputPath.ReadAllInts()
        .Sum(Solve2)
        .Dump()
        .ShouldBe(4870838);
}

int Solve1(int mass) => mass / 3 - 2;

int Solve2(int mass) => EnumerableEx
    .Generate(Solve1(mass), fuel => fuel > 0, Solve1, _ => _)
    .Sum();

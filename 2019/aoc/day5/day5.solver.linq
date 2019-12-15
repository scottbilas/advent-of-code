<Query Kind="Program">
  <Reference Relative="..\bin\Debug\netstandard2.1\aoc2019.dll">C:\proj\advent-of-code\2019\aoc\bin\Debug\netstandard2.1\aoc2019.dll</Reference>
  <Reference Relative="..\bin\Debug\netstandard2.1\libaoc2019.dll">C:\proj\advent-of-code\2019\aoc\bin\Debug\netstandard2.1\libaoc2019.dll</Reference>
  <Reference Relative="..\bin\Debug\netstandard2.1\libutils.dll">C:\proj\advent-of-code\2019\aoc\bin\Debug\netstandard2.1\libutils.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\netstandard.dll</Reference>
  <NuGetReference>morelinq</NuGetReference>
  <NuGetReference>Shouldly</NuGetReference>
  <NuGetReference>System.Interactive</NuGetReference>
  <Namespace>Aoc2019</Namespace>
  <Namespace>JetBrains.Annotations</Namespace>
  <Namespace>MoreLinq.Extensions</Namespace>
  <Namespace>Shouldly</Namespace>
  <Namespace>static Aoc2019.MiscStatics</Namespace>
  <Namespace>static System.Linq.Enumerable</Namespace>
  <Namespace>static System.Linq.EnumerableEx</Namespace>
  <Namespace>System.Drawing</Namespace>
  <Namespace>Unity.Coding.Editor</Namespace>
  <Namespace>Unity.Coding.Utils</Namespace>
</Query>

void Main()
{
    var folder = Util.CurrentQueryPath.ToNPath().Parent;
    var inputPath = folder.Combine($"{folder.FileName}.input.txt");

    var input = inputPath.ReadAllInts().ToArray();

// --- PART 1 ---

    // *SAMPLES*

    With(Generate(new Random(12345), _ => _, r => r.Next()).Take(50).ToArray(),
        sample => Run(Arr(3, 0, 4, 0, 99), sample).ShouldBe(sample));

    // *PROBLEM*

    Run(input, 1).Dump().ShouldBe(10987514);

// --- PART 2 ---

    // *SAMPLES*

    // Using position mode, consider whether the input is equal to 8; output 1 (if it is) or 0 (if it is not).
    Run(Arr(3, 9, 8, 9, 10, 9, 4, 9, 99, -1, 8),
        Arr(7, 8, 9)).ShouldBe(
        Arr(0, 1, 0));

    // Using position mode, consider whether the input is less than 8; output 1 (if it is) or 0 (if it is not).
    Run(Arr(3, 9, 7, 9, 10, 9, 4, 9, 99, -1, 8),
        Arr(7, 8, 9)).ShouldBe(
        Arr(1, 0, 0));

    // Using immediate mode, consider whether the input is equal to 8; output 1 (if it is) or 0 (if it is not).
    Run(Arr(3, 3, 1108, -1, 8, 3, 4, 3, 99),
        Arr(7, 8, 9)).ShouldBe(
        Arr(0, 1, 0));

    // Using immediate mode, consider whether the input is less than 8; output 1 (if it is) or 0 (if it is not).
    Run(Arr(3, 3, 1107, -1, 8, 3, 4, 3, 99),
        Arr(7, 8, 9)).ShouldBe(
        Arr(1, 0, 0));

    // Here are some jump tests that take an input, then output 0 if the input was zero or 1 if the input was non - zero:

    // (using position mode)
    Run(Arr(3, 12, 6, 12, 15, 1, 13, 14, 13, 4, 13, 99, -1, 0, 1, 9),
        Arr(-10, 0, 15)).ShouldBe(
        Arr(  1, 0,  1));

    // (using immediate mode)
    Run(Arr(3, 3, 1105, -1, 9, 1101, 0, 0, 12, 4, 12, 99, 1),
        Arr(-10, 0, 15)).ShouldBe(
        Arr(  1, 0,  1));

    // Here's a larger example:

    Run(Arr(3, 21, 1008, 21, 8, 20, 1005, 20, 22, 107, 8, 21, 20, 1006, 20, 31,
            1106, 0, 36, 98, 0, 0, 1002, 21, 125, 20, 4, 20, 1105, 1, 46, 104,
            999, 1105, 1, 46, 1101, 1000, 1, 20, 4, 20, 1105, 1, 46, 98, 99),
        Arr(-100,   7,    8,    9,  150)).ShouldBe(
        Arr( 999, 999, 1000, 1001, 1001));

    // *PROBLEM*

    Run(input, 5).Dump().ShouldBe(14195011);
}

int[] Run(int[] mem, int[] input) =>
    input.Select(i => Run(mem, i)).ToArray();

int Run(int[] mem, int input) =>
    new IntCodeVM(mem, () => input).Run().Last();

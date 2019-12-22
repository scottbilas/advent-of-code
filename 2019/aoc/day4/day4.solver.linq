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
    var (input0, input1) = (123257, 647015);
    var input = Enumerable.Range(input0, input1 - input0 + 1);

    // PART 1

    // sample

    Test1(111111).ShouldBe(true);
    Test1(223450).ShouldBe(false);
    Test1(123789).ShouldBe(false);

    // problem

    input.Count(Test1).Dump().ShouldBe(2220);

    // PART 2

    // sample

    Test2(112233).ShouldBe(true);
    Test2(123444).ShouldBe(false);
    Test2(111122).ShouldBe(true);

    // problem

    input.Count(Test2).Dump().ShouldBe(1515);
}

bool Test(int test, int part)
{
    var digits = (test + "x").ToString().Select(c => c - '0').ToArray();
    var (doubled, tripled) = (new int[10], new int[10]);

    for (var i = 0; i < 5; ++i)
    {
        if (digits[i]  > digits[i + 1])
            return false;

        if (digits[i] == digits[i + 1])
        {
            ++doubled[digits[i]];
            if (digits[i] == digits[i + 2])
                ++tripled[digits[i]];
        }
    }

    return part == 1
        ? doubled.Sum() > 0
        : Enumerable.Range(0, 10).Any(i => doubled[i] != 0 && tripled[i] == 0);
}

bool Test1(int test) => Test(test, 1);
bool Test2(int test) => Test(test, 2);
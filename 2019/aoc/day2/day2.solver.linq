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

    Solve1(1, 9, 10, 3, 2, 3, 11, 0, 99, 30, 40, 50).ShouldBe(3500);
    Solve1(1,0,0,0,99).ShouldBe(2);
    Solve1(2,3,0,3,99).ShouldBe(2);
    Solve1(2,4,4,5,99,0).ShouldBe(2);
    Solve1(1,1,1,4,99,5,6,0,99).ShouldBe(30);

    // problem

    var input = inputPath.ReadAllInts().ToArray();

    var part1 = input.ToArray();
    part1[1] = 12;
    part1[2] = 2;
    Solve1(part1).Dump().ShouldBe(6730673);

    // PART 2

    // sample

    Solve2(input, 6730673).ShouldBe(1202);

    // problem

    Solve2(input, 19690720).Dump().ShouldBe(3749);
}

int Solve1(params int[] mem)
{
    for (var ip = 0; ; ip += 4)
    {
        int op = mem[ip];
        if (op == 99)
            return mem[0];
            
        int src0 = mem[mem[ip + 1]];
        int src1 = mem[mem[ip + 2]];
        int dst = mem[ip + 3];
        
        mem[dst] = op == 1 ? src0 + src1 : src0 * src1;
    }
}

int Solve2(int[] mem, int target0)
{
    for (int noun = 0; noun < 100; ++noun)
    {
        for (int verb = 0; verb < 100; ++verb)
        {
            var testMem = mem.ToArray();
            testMem[1] = noun;
            testMem[2] = verb;
 
            var test = Solve1(testMem);
            if (test == target0)
                return 100 * noun + verb;
        }
    }
    
    return -1;
}
<Query Kind="Program">
  <Reference Relative="..\bin\Debug\netstandard2.1\aoc2019.dll">C:\proj\advent-of-code\2019\aoc\bin\Debug\netstandard2.1\aoc2019.dll</Reference>
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

    var mem = inputPath.ReadAllBigInts().ToArray();

// --- PART 1 ---

    // *SAMPLES*

    {
        const int k_Expected = 7734;
        var vm = new IntCodeVM(BArr(109, 19, 204, -34, 99));
        vm.Mem[1985] = k_Expected;
        vm.BaseOffset = 2000;

        vm.Run().Single().ShouldBe(k_Expected);
    }

    // takes no input and produces a copy of itself as output.
    With(BArr(109, 1, 204, -1, 1001, 100, 1, 100, 1008, 100, 16, 101, 1006, 101, 0, 99),
        mem => Run(mem).ShouldBe(mem));
        
    // should output a 16-digit number
    Run(BArr(1102,34915192,34915192,7,4,7,99,0)).Single().ToString().Length.ShouldBe(16);

    // should output the large number in the middle
    With(BArr(104, 1125899906842624, 99),
        mem => Run(mem).Single().ShouldBe(mem[1]));

    // *PROBLEM*

    Run(1, mem).Single().Dump().ShouldBe(2745604242);

// --- PART 2 ---

    Run(2, mem).Single().Dump().ShouldBe(51135);
}

IEnumerable<BigInteger> Run(BigInteger[] mem) => new IntCodeVM(mem).BRun();
IEnumerable<BigInteger> Run(int input, BigInteger[] mem) => new IntCodeVM(mem, () => input).BRun();

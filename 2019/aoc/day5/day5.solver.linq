<Query Kind="Program">
  <Reference Relative="..\..\libaoc\bin\Debug\netstandard2.1\libaoc2019.dll">C:\proj\advent-of-code\2019\libaoc\bin\Debug\netstandard2.1\libaoc2019.dll</Reference>
  <Reference Relative="..\..\libaoc\bin\Debug\netstandard2.1\libutils.dll">C:\proj\advent-of-code\2019\libaoc\bin\Debug\netstandard2.1\libutils.dll</Reference>
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

    With(Generate(new Random(12345), r => r.Next()).Take(50).ToArray(),
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

int Run(int[] mem, int input)
{
    mem = mem.ToArray();

    var lastOutput = 0;

    for (var ip = 0; ; )
    {
        int NextMem() => mem[ip++];

        // ABCDE
        //  1002
        // 
        // DE - two-digit opcode,      02 == opcode 2
        //  C - mode of 1st parameter,  0 == position mode
        //  B - mode of 2nd parameter,  1 == immediate mode
        //  A - mode of 3rd parameter,  0 == position mode,
        //                                   omitted due to being a leading zero

        int op = NextMem();
        if (op == 99)
            return lastOutput;

        var modes = op / 100;
        
        int Next()
        {
            var mode = modes % 10;
            modes /= 10;

            var item = NextMem();
            return mode == 0 ? mem[item] : item;
        }
        
        (int a, int b) Next2() => (Next(), Next());
        
        switch (op % 100)
        {
            // add: adds together numbers read from two positions and stores the result in a third position
            case 1:
                With((src: Next2(), dst: NextMem()), v => mem[v.dst] = v.src.a + v.src.b);
                break;

            // multiply: works exactly like opcode 1, except it multiplies the two inputs instead of adding them
            case 2:
                With((src: Next2(), dst: NextMem()), v => mem[v.dst] = v.src.a * v.src.b);
                break;

            // input: takes a single integer as input and saves it to the position given by its only parameter
            case 3:
                mem[NextMem()] = input;
                break;

            // output: outputs the value of its only parameter
            case 4:
                lastOutput.ShouldBe(0, "nonzero means something went wrong in sim");
                lastOutput = Next();
                break;

            // jump-if-true: if the first parameter is non-zero, it sets the instruction pointer to the value from the second parameter. Otherwise, it does nothing.
            case 5:
                With((src: Next(), dst: Next()), v => { if (v.src != 0) ip = v.dst; });
                break;

            // jump-if-false: if the first parameter is zero, it sets the instruction pointer to the value from the second parameter. Otherwise, it does nothing.
            case 6:
                With((src: Next(), dst: Next()), v => { if (v.src == 0) ip = v.dst; });
                break;

            // less-than: if the first parameter is less than the second parameter, it stores 1 in the position given by the third parameter. Otherwise, it stores 0.
            case 7:
                With((src: Next2(), dst: NextMem()), v => mem[v.dst] = v.src.a < v.src.b ? 1 : 0);
                break;

            // equals: if the first parameter is equal to the second parameter, it stores 1 in the position given by the third parameter.Otherwise, it stores 0.
            case 8:
                With((src: Next2(), dst: NextMem()), v => mem[v.dst] = v.src.a == v.src.b ? 1 : 0);
                break;
                
            default: throw new InvalidOperationException();
        }
    }
}

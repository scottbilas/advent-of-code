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
  <Namespace>static System.Linq.Enumerable</Namespace>
  <Namespace>static System.Linq.EnumerableEx</Namespace>
  <Namespace>System.Drawing</Namespace>
  <Namespace>Unity.Coding.Editor</Namespace>
  <Namespace>Unity.Coding.Utils</Namespace>
  <Namespace>System.Numerics</Namespace>
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
        var vm = new VM(null, BArr(109, 19, 204, -34, 99));
        vm.Mem[1985] = k_Expected;
        vm.BaseOffset = 2000;

        vm.Run().Single().ShouldBe(k_Expected);
    }

    // takes no input and produces a copy of itself as output.
    With(BArr(109, 1, 204, -1, 1001, 100, 1, 100, 1008, 100, 16, 101, 1006, 101, 0, 99),
        mem => Run(null, mem).ShouldBe(mem));
        
    // should output a 16-digit number
    Run(null, BArr(1102,34915192,34915192,7,4,7,99,0)).Single().ToString().Length.ShouldBe(16);

    // should output the large number in the middle
    With(BArr(104, 1125899906842624, 99),
        mem => Run(null, mem).Single().ShouldBe(mem[1]));

    // *PROBLEM*

    Run(Arr(1), mem).Single().Dump().ShouldBe(2745604242);

// --- PART 2 ---

    Run(Arr(2), mem).Single().Dump().ShouldBe(51135);
}

BigInteger[] BArr(params BigInteger[] bigints) => bigints;
IEnumerable<BigInteger> Run(int[] input, BigInteger[] mem) => new VM(input, mem).Run();

class VM
{
    public VM(int[] input, BigInteger[] mem)
    {
        for (var i = 0; i < mem.Length; ++i)
            Mem[i] = mem[i];
        
        Input = input;
    }

    public IDictionary<BigInteger, BigInteger> Mem = new AutoDictionary<BigInteger, BigInteger>();
    public BigInteger MemPtr, BaseOffset;
    public int[] Input;
    public int InputPtr;

    public IEnumerable<BigInteger> Run()
    {
        for (; ; )
        {
            BigInteger NextMem() => Mem[MemPtr++];

            var op = (int)NextMem();
            var modes = op / 100;

            BigInteger Next(bool forWrite)
            {
                var mode = modes % 10;
                modes /= 10;

                var item = NextMem();
                if (mode == 2)
                    item += BaseOffset;

                return forWrite || mode == 1 ? item : Mem[item];
            }

            BigInteger NextRead() => Next(false);
            BigInteger NextWrite() => Next(true);
            (BigInteger a, BigInteger b) NextSrc2() => (NextRead(), NextRead());

            switch (op % 100)
            {
                // add: adds together numbers read from two positions and stores the result in a third position
                case 1:
                    With((src: NextSrc2(), dst: NextWrite()), v => Mem[v.dst] = v.src.a + v.src.b);
                    break;

                // multiply: works exactly like opcode 1, except it multiplies the two inputs instead of adding them
                case 2:
                    With((src: NextSrc2(), dst: NextWrite()), v => Mem[v.dst] = v.src.a * v.src.b);
                    break;

                // input: takes a single integer as input and saves it to the position given by its only parameter
                case 3:
                    Mem[NextWrite()] = Input[InputPtr++];
                    break;

                // output: outputs the value of its only parameter
                case 4:
                    yield return NextRead();
                    break;

                // jump-if-true: if the first parameter is non-zero, it sets the instruction pointer to the value from the second parameter. Otherwise, it does nothing.
                case 5:
                    With((src: NextRead(), tgt: NextRead()), v => { if (v.src != 0) MemPtr = v.tgt; });
                    break;

                // jump-if-false: if the first parameter is zero, it sets the instruction pointer to the value from the second parameter. Otherwise, it does nothing.
                case 6:
                    With((src: NextRead(), tgt: NextRead()), v => { if (v.src == 0) MemPtr = v.tgt; });
                    break;

                // less-than: if the first parameter is less than the second parameter, it stores 1 in the position given by the third parameter. Otherwise, it stores 0.
                case 7:
                    With((src: NextSrc2(), dst: NextWrite()), v => Mem[v.dst] = v.src.a < v.src.b ? 1 : 0);
                    break;

                // equals: if the first parameter is equal to the second parameter, it stores 1 in the position given by the third parameter.Otherwise, it stores 0.
                case 8:
                    With((src: NextSrc2(), dst: NextWrite()), v => Mem[v.dst] = v.src.a == v.src.b ? 1 : 0);
                    break;

                // relative base offset: adjusts the relative base by the value of its only parameter. The relative base increases (or decreases, if the value is negative) by the value of the parameter.
                case 9:
                    BaseOffset += NextRead();
                    break;

                case 99:
                    --MemPtr;
                    yield break;
            }
        }
    }
}

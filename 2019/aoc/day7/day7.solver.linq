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
</Query>

void Main()
{
    var folder = Util.CurrentQueryPath.ToNPath().Parent;
    var inputPath = folder.Combine($"{folder.FileName}.input.txt");

    var mem = inputPath.ReadAllInts().ToArray();

// --- PART 1 ---

    // *SAMPLES*
    
    With(Solve(1, Arr(3, 15, 3, 16, 1002, 16, 10, 16, 1, 16, 15, 15, 4, 15, 99, 0, 0)), r =>
    {
        r.phase.ShouldBe(Arr(4, 3, 2, 1, 0));
        r.signal.ShouldBe(43210);
    });

    With(Solve(1, Arr(3, 23, 3, 24, 1002, 24, 10, 24, 1002, 23, -1, 23, 101, 5, 23, 23, 1, 24, 23, 23, 4, 23, 99, 0, 0)), r =>
    {
        r.phase.ShouldBe(Arr(0, 1, 2, 3, 4));
        r.signal.ShouldBe(54321);
    });

    With(Solve(1, Arr(3, 31, 3, 32, 1002, 32, 10, 32, 1001, 31, -2, 31, 1007, 31, 0, 33, 1002, 33, 7, 33, 1, 33, 31, 31, 1, 32, 31, 31, 4, 31, 99, 0, 0, 0)), r =>
    {
        r.phase.ShouldBe(Arr(1, 0, 4, 3, 2));
        r.signal.ShouldBe(65210);
    });

    // *PROBLEM*

    Solve(1, mem).signal.Dump().ShouldBe(440880);

// --- PART 2 ---

    // *SAMPLES*

    With(Solve(2, Arr(3, 26, 1001, 26, -4, 26, 3, 27, 1002, 27, 2, 27, 1, 27, 26, 27, 4, 27, 1001, 28, -1, 28, 1005, 28, 6, 99, 0, 0, 5)), r =>
    {
        r.phase.ShouldBe(Arr(9, 8, 7, 6, 5));
        r.signal.ShouldBe(139629729);
    });
    
    With(Solve(2, Arr(3, 52, 1001, 52, -5, 52, 3, 53, 1, 52, 56, 54, 1007, 54, 5, 55, 1005, 55, 26, 1001, 54, -5, 54, 1105, 1, 12, 1, 53, 54, 53, 1008, 54, 0, 55, 1001, 55, 1, 55, 2, 53, 55, 53, 4, 53, 1001, 56, -1, 56, 1005, 56, 6, 99, 0, 0, 0, 0, 10)), r =>
    {
        r.phase.ShouldBe(Arr(9, 7, 8, 5, 6));
        r.signal.ShouldBe(18216);
    });

    // *PROBLEM*

    Solve(2, mem).signal.Dump().ShouldBe(3745599);
}

(int signal, IList<int> phase) Solve(int which, int[] mem) =>
    new Permutations<int>(Range(which == 1 ? 0 : 5, 5).ToList())
    .Select(phase => (signal: Run(which, mem, phase), phase))
    .MaxBy(v => v.signal)
    .First();

int Run(int which, int[] mem, IList<int> phase) =>
    which == 1 ? Run1(mem, phase) : Run2(mem, phase);

int Run1(int[] mem, IList<int> phases) =>
    phases.Aggregate(0, (signal, phase) => new VM(mem, phase, () => signal).Run().Value);

int Run2(int[] mem, IList<int> phase)
{
    var signals = new int[5];
   
    var loop = Range(0, signals.Length)
        .Select(i => (
            dst:  (i + 1) % signals.Length,
            vm : new VM(mem, phase[i], () => signals[i])))
        .ToArray().Repeat();

    foreach (var item in loop)
    {
        var o = item.vm.Run();
        if (o == null)
            break;
        signals[item.dst] = o.Value;
    }
    
    return signals[0];
}

class VM
{
    public VM(int[] mem, int phase, Func<int> getInput)
    {
        Mem = mem.ToArray();

        GetInput = () =>
        {
            GetInput = getInput;
            return phase;
        };
    }
    
    public int[] Mem;
    public int IP;
    public Func<int> GetInput;

    public int? Run()
    {
        for (;;)
        {
            int NextMem() => Mem[IP++];
    
            // ABCDE
            //  1002
            // 
            // DE - two-digit opcode,      02 == opcode 2
            //  C - mode of 1st parameter,  0 == position mode
            //  B - mode of 2nd parameter,  1 == immediate mode
            //  A - mode of 3rd parameter,  0 == position mode,
            //                                   omitted due to being a leading zero
    
            int op = NextMem();
            var modes = op / 100;
    
            int Next()
            {
                var mode = modes % 10;
                modes /= 10;
    
                var item = NextMem();
                return mode == 0 ? Mem[item] : item;
            }
    
            (int a, int b) Next2() => (Next(), Next());
    
            switch (op % 100)
            {
                // add: adds together numbers read from two positions and stores the result in a third position
                case 1:
                    With((src: Next2(), dst: NextMem()), v => Mem[v.dst] = v.src.a + v.src.b);
                    break;
    
                // multiply: works exactly like opcode 1, except it multiplies the two inputs instead of adding them
                case 2:
                    With((src: Next2(), dst: NextMem()), v => Mem[v.dst] = v.src.a * v.src.b);
                    break;
    
                // input: takes a single integer as input and saves it to the position given by its only parameter
                case 3:
                    Mem[NextMem()] = GetInput();
                    break;
    
                // output: outputs the value of its only parameter
                case 4:
                    return Next();
    
                // jump-if-true: if the first parameter is non-zero, it sets the instruction pointer to the value from the second parameter. Otherwise, it does nothing.
                case 5:
                    With((src: Next(), dst: Next()), v => { if (v.src != 0) IP = v.dst; });
                    break;
    
                // jump-if-false: if the first parameter is zero, it sets the instruction pointer to the value from the second parameter. Otherwise, it does nothing.
                case 6:
                    With((src: Next(), dst: Next()), v => { if (v.src == 0) IP = v.dst; });
                    break;
    
                // less-than: if the first parameter is less than the second parameter, it stores 1 in the position given by the third parameter. Otherwise, it stores 0.
                case 7:
                    With((src: Next2(), dst: NextMem()), v => Mem[v.dst] = v.src.a < v.src.b ? 1 : 0);
                    break;
    
                // equals: if the first parameter is equal to the second parameter, it stores 1 in the position given by the third parameter.Otherwise, it stores 0.
                case 8:
                    With((src: Next2(), dst: NextMem()), v => Mem[v.dst] = v.src.a == v.src.b ? 1 : 0);
                    break;
    
                case 99:
                    return null;
            }
        }
    }
}

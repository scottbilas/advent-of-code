<Query Kind="Program">
  <Reference Relative="..\..\..\temp\bin\aoc2019\Debug\netstandard2.1\aoc2019.dll">C:\proj\advent-of-code\temp\bin\aoc2019\Debug\netstandard2.1\aoc2019.dll</Reference>
  <Reference Relative="..\..\..\temp\bin\aoc2019\Debug\netstandard2.1\libaoc2019.dll">C:\proj\advent-of-code\temp\bin\aoc2019\Debug\netstandard2.1\libaoc2019.dll</Reference>
  <Reference Relative="..\..\..\temp\bin\aoc2019\Debug\netstandard2.1\libutils.dll">C:\proj\advent-of-code\temp\bin\aoc2019\Debug\netstandard2.1\libutils.dll</Reference>
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
    .Select(phases => (signal: Run(which, mem, phases), phases))
    .MaxBy(v => v.signal)
    .First();

int Run(int which, int[] mem, IList<int> phases) =>
    which == 1 ? Run1(mem, phases) : Run2(mem, phases);

int Run1(int[] mem, IList<int> phases) =>
    phases.Aggregate(0, (signal, phase) => CreateVM(mem, phase, () => signal).Run().First());

int Run2(int[] mem, IList<int> phases)
{
    var signals = new int[phases.Count];

    var loop = phases
        .Select((phase, i) => (
            dst: (i + 1) % phases.Count,
            vm: CreateVM(mem, phase, () => signals[i])))
        .ToArray();

    foreach (var item in loop.Repeat())
    {
        var o = item.vm.Run().FirstOrNull();
        if (o == null)
            break;
        signals[item.dst] = (int)o.Value;
    }
    
    return signals[0];
}

IntCodeVM CreateVM(int[] mem, int phase, Func<int> nextInput)
{
    var vm = new IntCodeVM(mem);
    vm.NextInput = () =>
    {
        vm.NextInput = nextInput;
        return phase;
    };
    return vm;
}

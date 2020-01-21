<Query Kind="Program">
  <Reference Relative="..\..\..\temp\bin\libaoc2019\Debug\netstandard2.1\libaoc2019.dll">C:\proj\advent-of-code\temp\bin\libaoc2019\Debug\netstandard2.1\libaoc2019.dll</Reference>
  <Reference Relative="..\..\..\temp\bin\libaoc2019\Debug\netstandard2.1\libutils.dll">C:\proj\advent-of-code\temp\bin\libaoc2019\Debug\netstandard2.1\libutils.dll</Reference>
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
  <Namespace>System.Numerics</Namespace>
  <Namespace>Unity.Coding.Editor</Namespace>
  <Namespace>Unity.Coding.Utils</Namespace>
  <Namespace>System.Drawing</Namespace>
</Query>

void Main()
{
    var folder = Util.CurrentQueryPath.ToNPath().Parent;
    var inputPath = folder.Combine($"{folder.FileName}.input.txt");

    var mem = inputPath.ReadAllBigInts().ToArray();

// --- PART 1 ---

    Solve1(mem).Dump().ShouldBe(20225);

// --- PART 2 ---

    Solve2(mem).Dump().ShouldBe(14348);

}

int Solve1(BigInteger[] mem) => Solve2(mem, true);

int Solve2(BigInteger[] mem, bool stopAtNat = false)
{
    const int k_PuterCount = 50;
    const int k_NatAddress = 255;

    var puters = Range(0, k_PuterCount)
        .Select(i =>
        {
            var queue = new Queue<BigInteger>(BArr(i));
            var vm = new BigIntCodeVM(mem);
            vm.NextInput = () =>
            {
                if (queue.TryDequeue(out var result))
                    return result;

                vm.Paused = true;
                return -1;
            };

            return (vm, queue);
        })
        .ToArray();

    void Send(BigInteger ip, BigInteger x, BigInteger y) =>
        With(puters[(int)ip].queue, q => q.EnqueueRange(x, y));

    (BigInteger x, BigInteger y) natData = default;
    var natYs = new HashSet<BigInteger>();

    for (;;)
    {
        var idle = true;
        foreach (var vm in puters.Select(v => v.vm))
        {
            vm.Paused = false;
            foreach (var (ip, x, y) in vm.Run().Batch3())
            {
                if (ip == k_NatAddress)
                {
                    if (stopAtNat)
                        return (int)y;
                    natData = (x, y);
                }
                else
                    Send(ip, x, y);

                idle = false;
            }
        }

        if (idle)
        {
            if (!natYs.Add(natData.y))
                return (int)natData.y;
            Send(0, natData.x, natData.y);
        }
    }
}

<Query Kind="Statements">
  <Reference Relative="..\..\libaoc\bin\Debug\netstandard2.0\libaoc.dll">C:\proj\advent-of-code\libaoc\bin\Debug\netstandard2.0\libaoc.dll</Reference>
  <NuGetReference>Shouldly</NuGetReference>
  <Namespace>AoC</Namespace>
  <Namespace>Shouldly</Namespace>
  <Namespace>System.Drawing</Namespace>
</Query>

int? Sim(int target, int maxLoops)
{
    for (var (loop, r) = (1, 0); loop < maxLoops; ++loop)
    {
        var a = r | 0x10000;

        r = 9450265;
        r = (((r + ((a)       & 0xff)) & 0xffffff) * 65899) & 0xffffff;
        r = (((r + ((a >>  8) & 0xff)) & 0xffffff) * 65899) & 0xffffff;
        r = (((r + ((a >> 16) & 0xff)) & 0xffffff) * 65899) & 0xffffff;

        if (r == target)
            return loop;
    }
    
    return null;
}

// part 1

int Part1()
{
    for (var (target, limit) = (0, 1000); ; ++target)
    {
        var result = Sim(target, limit);
        if (result != null)
        {
            limit = result.Value;
            if (limit == 1)
                return target;
        }
    }
}

Part1().Dump().ShouldBe(986758);

// part 2

{
    var last = DateTime.Now;
    var limit = 12500;
    int maxLoopCount = 0;
    int minTarget = int.MaxValue;
    var obj = new object();
    foreach (var item in Enumerable
        .Range(0, 16777215)
        .Select(v => 16777215 - v)
        .AsParallel()
        .WithMergeOptions(ParallelMergeOptions.NotBuffered)
        .Select(target =>
        {
            if (target % 1000000 == 0)
            {
                $"..{target}.. {(DateTime.Now - last).TotalSeconds}".Dump();
                last = DateTime.Now;
            }
            return (result: Sim(target, limit), target);
        })
        .Where(r => r.result != null)) // TODO: WhereNotNull
    {
        if (item.result.Value >= maxLoopCount)
        {
            lock (obj)
            {
                if (item.result.Value > maxLoopCount || item.target < minTarget)
                {
                    maxLoopCount = Math.Max(maxLoopCount, item.result.Value);
                    minTarget = Math.Min(minTarget, item.target);

                    $"target: {item.target}; loopCount: {maxLoopCount}".Dump();
                }
            }
        }
    }
}

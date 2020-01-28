<Query Kind="Program">
  <Reference Relative="..\..\..\temp\bin\libaoc2017\Debug\netstandard2.1\libaoc2017.dll">C:\proj\advent-of-code\temp\bin\libaoc2017\Debug\netstandard2.1\libaoc2017.dll</Reference>
  <Reference Relative="..\..\..\temp\bin\libaoc2017\Debug\netstandard2.1\libutils.dll">C:\proj\advent-of-code\temp\bin\libaoc2017\Debug\netstandard2.1\libutils.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\netstandard.dll</Reference>
  <NuGetReference>Combinatorics</NuGetReference>
  <NuGetReference>morelinq</NuGetReference>
  <NuGetReference>RoyT.AStar</NuGetReference>
  <NuGetReference>Shouldly</NuGetReference>
  <NuGetReference>System.Interactive</NuGetReference>
  <NuGetReference>YC.QuickGraph</NuGetReference>
  <Namespace>Aoc2017</Namespace>
  <Namespace>Combinatorics.Collections</Namespace>
  <Namespace>JetBrains.Annotations</Namespace>
  <Namespace>QuickGraph</Namespace>
  <Namespace>QuickGraph.Algorithms</Namespace>
  <Namespace>QuickGraph.Graphviz</Namespace>
  <Namespace>RoyT.AStar</Namespace>
  <Namespace>Shouldly</Namespace>
  <Namespace>static Aoc2017.MiscStatics</Namespace>
  <Namespace>static Aoc2017.Utils</Namespace>
  <Namespace>static System.Linq.Enumerable</Namespace>
  <Namespace>static System.Linq.EnumerableEx</Namespace>
  <Namespace>System.Drawing</Namespace>
  <Namespace>System.Numerics</Namespace>
  <Namespace>Unity.Coding.Editor</Namespace>
  <Namespace>Unity.Coding.Utils</Namespace>
  <Namespace>System.Diagnostics.CodeAnalysis</Namespace>
  <Namespace>System.Globalization</Namespace>
</Query>

static readonly NPath ScriptDir = Util.CurrentQueryPath.ToNPath().Parent;

void Main()
{
    var inputPath = ScriptDir.Combine($"{ScriptDir.FileName}.input.txt");
    var input = inputPath.ReadAllText();

// --- PART 1 ---

    // *SAMPLES*

    Solve1(@"
        p=< 3,0,0>, v=< 2,0,0>, a=<-1,0,0>
        p=< 4,0,0>, v=< 0,0,0>, a=<-2,0,0>
        ").ShouldBe(0);

    // *PROBLEM*

    Solve1(input).Dump().ShouldBe(364);

// --- PART 2 ---

    // *SAMPLES*

    Solve2(@"
        p=<-6,0,0>, v=< 3,0,0>, a=< 0,0,0>    
        p=<-4,0,0>, v=< 2,0,0>, a=< 0,0,0>
        p=<-2,0,0>, v=< 1,0,0>, a=< 0,0,0>
        p=< 3,0,0>, v=<-1,0,0>, a=< 0,0,0>
        ").ShouldBe(1);

    // *PROBLEM*

    Solve2(input).Dump();//.ShouldBe(306);
}

(Int3 p, Int3 v, Int3 a)[] Parse(string particleText) =>
    particleText.SelectInt3s().Batch3().ToArray();

int Solve1(string particleText)
{
    var particles = Parse(particleText);
    var minCounts = new int[particles.Length];
    
    for (var loop = 0; loop < 1000; ++loop)
    {
        var min = (i: int.MaxValue, d: int.MaxValue);
        
        for (var i = 0; i < particles.Length; ++i)
        {
            particles[i].v += particles[i].a;
            particles[i].p += particles[i].v;
            var d = particles[i].p.ManhattanDistance();
            if (d < min.d)
                min = (i, d);
        }

        ++minCounts[min.i];
    }
    
    return minCounts.WithIndices().MaxBy(i => i.value).First().index;
}

int Solve2(string particleText)
{
    var particles = Parse(particleText);
    var count = particles.Length;

    for (var loop = 0; loop < 1000; ++loop)
    {
        for (var i = 0; i < count; ++i)
        {
            particles[i].v += particles[i].a;
            particles[i].p += particles[i].v;
        }
        
        foreach (var dups in particles.Take(count).GroupBy(p => p.p).Where(g => g.Count() > 1))
            for (var i = 0; i < count; ++i)
                if (particles[i].p.Equals(dups.Key))
                    particles[i--] = particles[--count];
    }

    return count;
}

<Query Kind="Program">
  <Reference Relative="..\..\..\temp\bin\libaoc2017\Debug\netstandard2.1\libaoc2017.dll">C:\proj\advent-of-code\temp\bin\libaoc2017\Debug\netstandard2.1\libaoc2017.dll</Reference>
  <Reference Relative="..\..\..\temp\bin\libaoc2017\Debug\netstandard2.1\libutils.dll">C:\proj\advent-of-code\temp\bin\libaoc2017\Debug\netstandard2.1\libutils.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\netstandard.dll</Reference>
  <NuGetReference>Combinatorics</NuGetReference>
  <NuGetReference>morelinq</NuGetReference>
  <NuGetReference>Open.Numeric.Primes</NuGetReference>
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
  <Namespace>System.Diagnostics.CodeAnalysis</Namespace>
  <Namespace>System.Drawing</Namespace>
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Numerics</Namespace>
  <Namespace>Unity.Coding.Editor</Namespace>
  <Namespace>Unity.Coding.Utils</Namespace>
  <Namespace>Open.Numeric.Primes</Namespace>
</Query>

static readonly NPath ScriptDir = Util.CurrentQueryPath.ToNPath().Parent;

void Main()
{
    var inputPath = ScriptDir.Combine($"{ScriptDir.FileName}.input.txt");
    var input = inputPath.ReadAllText();

// --- PART 1 ---

    Solve1(input).Dump().ShouldBe(5929);

// --- PART 2 ---

    Solve2(input).Dump().ShouldBe(907);
}

int Solve1(string asm)
{
    var instrs = asm.Trim().SelectLines().Select(l => l.Split(' ')).ToArray();
    var regs = new AutoDictionary<string, long>();

    long Resolve(string param) =>
        param.TryParseInt(out var value) ? value : regs[param];

    var muls = 0;
    for (var ip = 0; instrs.IsValidIndex(ip);)
    {
        var instr = instrs[ip++];
        switch (instr[0])
        {
            case "jnz":
                if (Resolve(instr[1]) != 0)
                    ip += (int)Resolve(instr[2]) - 1;
                break;

            case "set": regs[instr[1]]  = Resolve(instr[2]); break;
            case "sub": regs[instr[1]] -= Resolve(instr[2]); break;
            case "mul": regs[instr[1]] *= Resolve(instr[2]); ++muls; break;
        }
    }
    
    return muls;
}

int Solve2(string asm) =>
    Range(0, 1001).Count(i => !Prime.Numbers.IsPrime(i * 17 + 107900));

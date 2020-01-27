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
        set a 1
        add a 2
        mul a a
        mod a 5
        snd a
        set a 0
        rcv a
        jgz a -1
        set a 1
        jgz a -2    
        ").ShouldBe(4);

    // *PROBLEM*

    Solve1(input).Dump().ShouldBe(3423);

// --- PART 2 ---

    // *SAMPLES*

    Solve2(@"
        snd 1
        snd 2
        snd p
        rcv a
        rcv b
        rcv c
        rcv d    
        ").ShouldBe(3);

    // *PROBLEM*

    Solve2(input).Dump().ShouldBe(7493);
}

long Solve1(string asm)
{
    var instrs = asm.Trim().SelectLines().Select(l => l.Split(' ')).ToArray();
    var regs = new AutoDictionary<string, long>();
    
    long Resolve(string param)
    {
        if (param.TryParseInt(out var value))
            return value;
        return regs[param];
    }

    for (var ip = 0; instrs.IsValidIndex(ip);)
    {
        var instr = instrs[ip++];
        switch (instr[0])
        {
            case "snd":
                // snd X plays a sound with a frequency equal to the value of X.
                regs["snd"] = Resolve(instr[1]);
                break;
            case "set":
                // set X Y sets register X to the value of Y.
                regs[instr[1]] = Resolve(instr[2]);
                break;
            case "add":
                // add X Y increases register X by the value of Y.
                regs[instr[1]] += Resolve(instr[2]);
                break;
            case "mul":
                // mul X Y sets register X to the result of multiplying the value contained in register X by the value of Y.
                regs[instr[1]] *= Resolve(instr[2]);
                break;
            case "mod":
                // mod X Y sets register X to the remainder of dividing the value contained in register X by the value of Y(that is, it sets X to the result of X modulo Y).
                regs[instr[1]] %= Resolve(instr[2]);
                break;
            case "rcv":
                // rcv X recovers the frequency of the last sound played, but only when the value of X is not zero. (If it is zero, the command does nothing.)
                if (Resolve(instr[1]) != 0)
                    return regs["snd"];
                break;
            case "jgz":
                // jgz X Y jumps with an offset of the value of Y, but only if the value of X is greater than zero. (An offset of 2 skips the next instruction, an offset of - 1 jumps to the previous instruction, and so on.)
                if (regs[instr[1]] > 0)
                    ip += (int)Resolve(instr[2]) - 1;
                break;
        }
    }
    
    return -1;
}

int Solve2(string asm)
{
    IEnumerable<Unit> Run(string asm, Action<long> send, Queue<long> receive)
    {
        var instrs = asm.Trim().SelectLines().Select(l => l.Split(' ')).ToArray();
        var regs = new AutoDictionary<string, long>();

        long Resolve(string param) =>
            param.TryParseInt(out var value) ? value : regs[param];

        for (var ip = 0; instrs.IsValidIndex(ip);)
        {
            var instr = instrs[ip++];
            switch (instr[0])
            {
                case "snd":
                    send(Resolve(instr[1]));
                    break;

                case "rcv":
                    if (receive.TryDequeue(out var rcv))
                        regs[instr[1]] = rcv;
                    else
                    {
                        yield return null;
                        --ip;
                    }
                    break;

                case "jgz":
                    if (Resolve(instr[1]) > 0)
                        ip += (int)Resolve(instr[2]) - 1;
                    break;

                case "set": regs[instr[1]] = Resolve(instr[2]); break;
                case "add": regs[instr[1]] += Resolve(instr[2]); break;
                case "mul": regs[instr[1]] *= Resolve(instr[2]); break;
                case "mod": regs[instr[1]] %= Resolve(instr[2]); break;
            }
        }
    }

    var (queue0, queue1) = (new Queue<long>(), new Queue<long>());
    var sendCount = 0;

    foreach (var _ in Enumerable.Zip(
        Run(asm, queue0.Enqueue, queue1),
        Run("set p 1\n" + asm, v => { ++sendCount; queue1.Enqueue(v); }, queue0)))
    {
        if (!queue0.Any() && !queue1.Any())
            break;
    }
    
    return sendCount;
}

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AoC;
using Day16;

namespace Day19
{
    class Instr : InstrData
    {
        public Op Op;

        public Instr(Op op, IReadOnlyList<int> ints)
            : base(ints) => Op = op;
    }

    static class Solver
    {
        public static int RunProgram(int boundIp, int start0, string instrText)
        {
            var ops = Op.All.ToDictionary(o => o.Name);

            var instrs = instrText
                .Split('\n')
                .Select(l => l.Trim())
                .Where(l => l.Any())
                .Skip(1)
                .Select(l => new Instr(
                    ops[l.Split(' ')[0].Trim()],
                    l.SelectInts().ToList()))
                .ToList();

            var regs = new int[6];
            regs[0] = start0;

            for (; regs[boundIp] >= 0 && regs[boundIp] < instrs.Count ;)
            {
                var instr = instrs[regs[boundIp]];

                var oldRegs = regs.ToList();
                var oldIp = regs[boundIp];
                instr.Op.Exec(instr, regs);

                Debug.WriteLine(
                    $"ip={oldIp} [{oldRegs[0]}, {oldRegs[1]}, {oldRegs[2]}, {oldRegs[3]}, {oldRegs[4]}, {oldRegs[5]}] " +
                    $"{instr.Op} {instr.A} {instr.B} {instr.C} " +
                    $"[{regs[0]}, {regs[1]}, {regs[2]}, {regs[3]}, {regs[4]}, {regs[5]}]");

                ++regs[boundIp];
            }
            
            return regs[0] - 1;
        }
    }
}

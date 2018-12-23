using System.Diagnostics;
using System.Linq;
using SantaVM;

namespace Day19
{
    static class Solver
    {
        public static int RunProgram(string programText)
        {
            var (boundIP, instrs) = SantaVM.Parser.Parse(programText);
            var regs = new int[6];

            for (; regs[boundIP] >= 0 && regs[boundIP] < instrs.Count ;)
            {
                var instr = instrs[regs[boundIP]];

                var oldRegs = regs.ToList();
                var oldIp = regs[boundIP];
                instr.Op.Exec(instr, regs);

                Debug.WriteLine(
                    $"ip={oldIp} [{oldRegs[0]}, {oldRegs[1]}, {oldRegs[2]}, {oldRegs[3]}, {oldRegs[4]}, {oldRegs[5]}] " +
                    $"{instr.Op} {instr.A} {instr.B} {instr.C} " +
                    $"[{regs[0]}, {regs[1]}, {regs[2]}, {regs[3]}, {regs[4]}, {regs[5]}]");

                ++regs[boundIP];
            }
            
            return regs[0] - 1;
        }
    }
}

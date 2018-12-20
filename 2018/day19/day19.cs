using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AoC;

namespace Day19
{
    class Op
    {
        public string Name { get; }
        public Action<Instr, int[]> Exec { get; }

        Op(string name, Action<Instr, int[]> exec)
            => (Name, Exec) = (name, exec);

        public override string ToString()
            => Name;

        public static IEnumerable<Op> All { get; } = new List<Op>
        {
            // ReSharper disable CommentTypo, StringLiteralTypo
            
            // addr (add register) stores into register C the result of adding register A and register B.
            new Op("addr", (instr, regs) => regs[instr.C] = regs[instr.A] + regs[instr.B]),
            // addi (add immediate) stores into register C the result of adding register A and value B.
            new Op("addi", (instr, regs) => regs[instr.C] = regs[instr.A] + instr.B),
            
            // mulr (multiply register) stores into register C the result of multiplying register A and register B.
            new Op("mulr", (instr, regs) => regs[instr.C] = regs[instr.A] * regs[instr.B]),
            // muli (multiply immediate) stores into register C the result of multiplying register A and value B.
            new Op("muli", (instr, regs) => regs[instr.C] = regs[instr.A] * instr.B),

            // banr (bitwise AND register) stores into register C the result of the bitwise AND of register A and register B.
            new Op("banr", (instr, regs) => regs[instr.C] = regs[instr.A] & regs[instr.B]),
            // bani (bitwise AND immediate) stores into register C the result of the bitwise AND of register A and value B.
            new Op("bani", (instr, regs) => regs[instr.C] = regs[instr.A] & instr.B),
            
            // borr (bitwise OR register) stores into register C the result of the bitwise OR of register A and register B.
            new Op("borr", (instr, regs) => regs[instr.C] = regs[instr.A] | regs[instr.B]),
            // bori (bitwise OR immediate) stores into register C the result of the bitwise OR of register A and value B.
            new Op("bori", (instr, regs) => regs[instr.C] = regs[instr.A] | instr.B),

            // setr (set register) copies the contents of register A into register C. (Input B is ignored.)
            new Op("setr", (instr, regs) => regs[instr.C] = regs[instr.A]),
            // seti (set immediate) stores value A into register C. (Input B is ignored.)
            new Op("seti", (instr, regs) => regs[instr.C] = instr.A),
            
            // gtir (greater-than immediate/register) sets register C to 1 if value A is greater than register B. Otherwise, register C is set to 0.
            new Op("gtir", (instr, regs) => regs[instr.C] = (instr.A > regs[instr.B]) ? 1 : 0),
            // gtri (greater-than register/immediate) sets register C to 1 if register A is greater than value B. Otherwise, register C is set to 0.
            new Op("gtri", (instr, regs) => regs[instr.C] = (regs[instr.A] > instr.B) ? 1 : 0),
            // gtrr (greater-than register/register) sets register C to 1 if register A is greater than register B. Otherwise, register C is set to 0.
            new Op("gtrr", (instr, regs) => regs[instr.C] = (regs[instr.A] > regs[instr.B]) ? 1 : 0),
            
            // eqir (equal immediate/register) sets register C to 1 if value A is equal to register B. Otherwise, register C is set to 0.
            new Op("eqir", (instr, regs) => regs[instr.C] = (instr.A == regs[instr.B]) ? 1 : 0),
            // eqri (equal register/immediate) sets register C to 1 if register A is equal to value B. Otherwise, register C is set to 0.
            new Op("eqri", (instr, regs) => regs[instr.C] = (regs[instr.A] == instr.B) ? 1 : 0),
            // eqrr (equal register/register) sets register C to 1 if register A is equal to register B. Otherwise, register C is set to 0.
            new Op("eqrr", (instr, regs) => regs[instr.C] = (regs[instr.A] == regs[instr.B]) ? 1 : 0),
            
            // ReSharper restore CommentTypo, StringLiteralTypo
        };        
    }

    class Instr
    {
        public Op Op;
        public int A, B, C;
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
                .Select(l =>
                {
                    var ints = l.SelectInts().ToList();
                    return new Instr
                    {
                        Op = ops[l.Split(' ')[0].Trim()],
                        A = ints[0],
                        B = ints[1],
                        C = ints[2],
                    };
                })
                .ToList();

            var regs = new int[6];
            regs[0] = start0;
            //var regs = new int[6] { 0, 0, 10551340, 10551348, 3, 1 };
            //var regs = new int[6] { 1, 0, 5275670, 10551348, 3, 2 };
            //var regs = new int[6] { 3, 0, 10551340, 10551348, 3, 2 };
            //var regs = new int[6] { 3, 0, 10551340, 10551348, 3, 3 };

            //ip = 3[0, 0, 941, 10551348, 3, 1]

            for (; regs[boundIp] >= 0 && regs[boundIp] < instrs.Count ;)
            {
                var instr = instrs[regs[boundIp]];

                /*
                var mbind = Regex.Match(line, @"#ip (\d+)");
                if (mbind.Success)
                    boundIp = int.Parse(mbind.Groups[1].Value);
                else*/
                {
                    var oldRegs = regs.ToList();
                    var oldIp = regs[boundIp];
                    instr.Op.Exec(instr, regs);

                    Debug.WriteLine(
                        $"ip={oldIp} [{oldRegs[0]}, {oldRegs[1]}, {oldRegs[2]}, {oldRegs[3]}, {oldRegs[4]}, {oldRegs[5]}] " +
                        $"{instr.Op} {instr.A} {instr.B} {instr.C} " +
                        $"[{regs[0]}, {regs[1]}, {regs[2]}, {regs[3]}, {regs[4]}, {regs[5]}]");

                    ++regs[boundIp];
                }
            }
            
            return regs[0] - 1;
        }
    }
}

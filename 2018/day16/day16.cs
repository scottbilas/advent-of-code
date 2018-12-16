using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using MoreLinq.Extensions;

namespace Day16
{
    class Op
    {
        public string Name;
        public Action<Instr, int[]> Exec;

        Op(string name, Action<Instr, int[]> exec) =>
            (Name, Exec) = (name, exec);

        public override string ToString() => Name;

        public static List<Op> All { get; } = new List<Op>
        {
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
            new Op("gtir", (instr, regs) => regs[instr.C] = instr.A > regs[instr.B] ? 1 : 0),
            // gtri (greater-than register/immediate) sets register C to 1 if register A is greater than value B. Otherwise, register C is set to 0.
            new Op("gtri", (instr, regs) => regs[instr.C] = regs[instr.A] > instr.B ? 1 : 0),
            // gtrr (greater-than register/register) sets register C to 1 if register A is greater than register B. Otherwise, register C is set to 0.
            new Op("gtrr", (instr, regs) => regs[instr.C] = regs[instr.A] > regs[instr.B] ? 1 : 0),
            
            // eqir (equal immediate/register) sets register C to 1 if value A is equal to register B. Otherwise, register C is set to 0.
            new Op("eqir", (instr, regs) => regs[instr.C] = instr.A == regs[instr.B] ? 1 : 0),
            // eqri (equal register/immediate) sets register C to 1 if register A is equal to value B. Otherwise, register C is set to 0.
            new Op("eqri", (instr, regs) => regs[instr.C] = regs[instr.A] == instr.B ? 1 : 0),
            // eqrr (equal register/register) sets register C to 1 if register A is equal to register B. Otherwise, register C is set to 0.
            new Op("eqrr", (instr, regs) => regs[instr.C] = regs[instr.A] == regs[instr.B] ? 1 : 0),
        };        
    }

    class Instr
    {
        public int Id;
        public int A, B, C;

        public Instr(IReadOnlyList<int> ints, int offset = 0) =>
            (Id, A, B, C) = (ints[0 + offset], ints[1 + offset], ints[2 + offset], ints[3 + offset]);

        public static IEnumerable<Instr> Parse(string text) => Regex
            .Matches(text, @"\d+")
            .Cast<Match>()
            .Select(m => int.Parse(m.Value))
            .Batch(4, b => new Instr(b.ToList()));
    }

    class Sample
    {
        public int[] Before, After;
        public Instr Instr;

        Sample(IReadOnlyList<int> ints) =>
            (Before, Instr, After) = (ints.Slice(0, 4).ToArray(), new Instr(ints, 4), ints.Slice(8, 4).ToArray());

        public static IEnumerable<Sample> Parse(string text) => Regex
            .Matches(text, @"\d+")
            .Cast<Match>()
            .Select(m => int.Parse(m.Value))
            .Batch(12, b => new Sample(b.ToList()));
    }

    static class Solver
    {
        static bool TestOp(Op op, Sample sample)
        {
            var working = (int[])sample.Before.Clone();
            op.Exec(sample.Instr, working);
            return working.SequenceEqual(sample.After);
        }
        
        public static int CountOpsMatching(int matchCountPerOp, string samplesText) => Sample
            .Parse(samplesText)
            .Select(sample => Op.All.Count(op => TestOp(op, sample)))
            .Count(c => c >= matchCountPerOp);

        public static IReadOnlyDictionary<int, Op> DeduceOps(string samplesText)
        {
            var successes = Sample
                .Parse(samplesText)
                .SelectMany(s => Op.All.Select(o => (o, s)).Where(v => TestOp(v.o, v.s)))
                .GroupBy(v => v.s.Instr.Id)
                .ToDictionary(g => g.Key, g => g.Select(v => v.o).Distinct().ToList());

            var ops = new Dictionary<int, Op>();
            while (successes.Any())
            {
                var singleton = successes.First(kvp => kvp.Value.Count == 1);
                var (id, op) = (singleton.Key, singleton.Value[0]);
                
                foreach (var o in successes.Values)
                    o.Remove(op);
                
                ops.Add(id, op);
                successes.Remove(id);
            }

            return ops;
        }
        
        public static int RunProgram(IReadOnlyDictionary<int, Op> ops, string instrText)
        {
            var regs = new int[4];

            foreach (var instr in Instr.Parse(instrText))
                ops[instr.Id].Exec(instr, regs);
            
            return regs[0];
        }
    }
}

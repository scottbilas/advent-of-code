using System;
using System.Collections.Generic;
using System.Linq;
using AoC;
using SantaVM;

namespace Day16
{
    class Instr : InstrData
    {
        public int Id;

        public Instr(IEnumerable<int> ints)
            : base(ints.Skip(1)) => Id = ints.First();

        public static IEnumerable<Instr> Parse(string text)
            => text
                .SelectInts()
                .Batch(4, b => new Instr(b));
    }

    class Sample
    {
        public int[] Before, After;
        public Instr Instr;

        Sample(IReadOnlyList<int> ints)
            => (Before, Instr, After) = (ints.SliceArray(0, 4), new Instr(ints.Skip(4)), ints.SliceArray(8, 4));

        public static IEnumerable<Sample> Parse(string text)
            => text.SelectInts().Batch(12, b => new Sample(b));
    }

    static class Extensions
    {
        public static bool Test(this Op @this, Sample sample)
        {
            var working = (int[])sample.Before.Clone();
            @this.Exec(sample.Instr, working);
            return working.SequenceEqual(sample.After);
        }
    }

    static class Solver
    {
        public static int CountOpsMatching(int matchCountPerOp, string samplesText)
            => Sample
                .Parse(samplesText)
                .Select(sample => Op.All.Values.Count(op => op.Test(sample)))
                .Count(c => c >= matchCountPerOp);

        public static IReadOnlyDictionary<int, Op> DeduceOps(string samplesText)
        {
            var successes = Sample
                .Parse(samplesText)
                .SelectMany(sample => Op.All.Values
                    .Select(op => (op, sample))
                    .Where(v => v.op.Test(v.sample)))
                .GroupBy(v => v.sample.Instr.Id)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(v => v.op).Distinct().ToList());

            var ops = new Dictionary<int, Op>();
            while (successes.Any())
            {
                var (id, value) = successes.First(kv => kv.Value.Count == 1);
                var op = value[0];

                foreach (var o in successes.Values)
                    o.Remove(op);
                
                successes.Remove(id);
                ops.Add(id, op);
            }
            
            return ops;
        }
        
        public static int RunProgram(IReadOnlyDictionary<int, Op> ops, string programText)
        {
            var regs = new int[4];

            foreach (var instr in Instr.Parse(programText))
                ops[instr.Id].Exec(instr, regs);
            
            return regs[0];
        }
    }
}

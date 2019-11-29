using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Aoc2018;
using Shouldly;

namespace SantaVM
{

    public class Op
    {
        public string Name { get; }
        public Action<InstrData, int[]> Exec { get; }

        Op(string name, Action<InstrData, int[]> exec)
            => (Name, Exec) = (name, exec);

        public override string ToString() => Name;

        public static IReadOnlyDictionary<string, Op> All => k_All;

        static readonly IReadOnlyDictionary<string, Op> k_All = typeof(Ops)
            .GetFields(BindingFlags.Static | BindingFlags.Public)
            .Select(f => f.GetValue(null))
            .Cast<Op>()
            .ToDictionary(o => o.Name);

        class Ops
        {
            // ReSharper disable CommentTypo, StringLiteralTypo, InconsistentNaming, IdentifierTypo, UnusedMember.Local, UnusedMember.Global

            // addr (add register) stores into register C the result of adding register A and register B.
            public static readonly Op Op_addr = new Op("addr", (instr, regs) => regs[instr.C] = regs[instr.A] + regs[instr.B]);
            // addi (add immediate) stores into register C the result of adding register A and value B.
            public static readonly Op Op_addi = new Op("addi", (instr, regs) => regs[instr.C] = regs[instr.A] + instr.B);
            
            // mulr (multiply register) stores into register C the result of multiplying register A and register B.
            public static readonly Op Op_mulr = new Op("mulr", (instr, regs) => regs[instr.C] = regs[instr.A] * regs[instr.B]);
            // muli (multiply immediate) stores into register C the result of multiplying register A and value B.
            public static readonly Op Op_muli = new Op("muli", (instr, regs) => regs[instr.C] = regs[instr.A] * instr.B);

            // banr (bitwise AND register) stores into register C the result of the bitwise AND of register A and register B.
            public static readonly Op Op_banr = new Op("banr", (instr, regs) => regs[instr.C] = regs[instr.A] & regs[instr.B]);
            // bani (bitwise AND immediate) stores into register C the result of the bitwise AND of register A and value B.
            public static readonly Op Op_bani = new Op("bani", (instr, regs) => regs[instr.C] = regs[instr.A] & instr.B);
            
            // borr (bitwise OR register) stores into register C the result of the bitwise OR of register A and register B.
            public static readonly Op Op_borr = new Op("borr", (instr, regs) => regs[instr.C] = regs[instr.A] | regs[instr.B]);
            // bori (bitwise OR immediate) stores into register C the result of the bitwise OR of register A and value B.
            public static readonly Op Op_bori = new Op("bori", (instr, regs) => regs[instr.C] = regs[instr.A] | instr.B);

            // setr (set register) copies the contents of register A into register C. (Input B is ignored.)
            public static readonly Op Op_setr = new Op("setr", (instr, regs) => regs[instr.C] = regs[instr.A]);
            // seti (set immediate) stores value A into register C. (Input B is ignored.)
            public static readonly Op Op_seti = new Op("seti", (instr, regs) => regs[instr.C] = instr.A);
            
            // gtir (greater-than immediate/register) sets register C to 1 if value A is greater than register B. Otherwise, register C is set to 0.
            public static readonly Op Op_gtir = new Op("gtir", (instr, regs) => regs[instr.C] = instr.A > regs[instr.B] ? 1 : 0);
            // gtri (greater-than register/immediate) sets register C to 1 if register A is greater than value B. Otherwise, register C is set to 0.
            public static readonly Op Op_gtri = new Op("gtri", (instr, regs) => regs[instr.C] = regs[instr.A] > instr.B ? 1 : 0);
            // gtrr (greater-than register/register) sets register C to 1 if register A is greater than register B. Otherwise, register C is set to 0.
            public static readonly Op Op_gtrr = new Op("gtrr", (instr, regs) => regs[instr.C] = regs[instr.A] > regs[instr.B] ? 1 : 0);
            
            // eqir (equal immediate/register) sets register C to 1 if value A is equal to register B. Otherwise, register C is set to 0.
            public static readonly Op Op_eqir = new Op("eqir", (instr, regs) => regs[instr.C] = instr.A == regs[instr.B] ? 1 : 0);
            // eqri (equal register/immediate) sets register C to 1 if register A is equal to value B. Otherwise, register C is set to 0.
            public static readonly Op Op_eqri = new Op("eqri", (instr, regs) => regs[instr.C] = regs[instr.A] == instr.B ? 1 : 0);
            // eqrr (equal register/register) sets register C to 1 if register A is equal to register B. Otherwise, register C is set to 0.
            public static readonly Op Op_eqrr = new Op("eqrr", (instr, regs) => regs[instr.C] = regs[instr.A] == regs[instr.B] ? 1 : 0);
        }
        // ReSharper restore CommentTypo, StringLiteralTypo, InconsistentNaming, IdentifierTypo, UnusedMember.Local, UnusedMember.Global
    }

    public class InstrData
    {
        public int A, B, C;
        public InstrData(IEnumerable<int> ints)
            => (A, B, C) = ints.First3();
    }

    public class Instr : InstrData
    {
        public Op Op;

        public Instr(Op op, IEnumerable<int> ints)
            : base(ints) => Op = op;
    }

    public static class Parser
    {
        public static (int boundIP, IReadOnlyList<Instr> instrs) Parse(string programText)
        {
            var boundIP = -1;
            var instrs = new List<Instr>();

            foreach (var line in programText
                .Split('\n')
                .Select(l => l.Trim())
                .Where(l => l.Any()))
            {
                var parts = line.Split(' ', 2);

                var name = parts[0].Trim();
                if (name == "#ip")
                {
                    boundIP.ShouldBe(-1);
                    boundIP = int.Parse(parts[1]);
                }
                else
                    instrs.Add(new Instr(Op.All[name], parts[1].SelectInts()));
            }

            return (boundIP, instrs);
        }
    }

    public static class Disassembler
    {
        public static IReadOnlyList<string> Disassemble(int boundIP, IReadOnlyList<Instr> instrs)
        {
            var vars = instrs
                .SelectMany(i => new[] { i.A, i.B, i.C })
                .Where(i => i != boundIP)
                .Distinct()
                .Ordered()
                .Select((i, index) => (i, name: new string((char)('a' + index), 1)))
                .ToDictionary(v => v.i, v => v.name);
            vars.Add(boundIP, "ip");

            var lines = new List<string>();

            foreach (var instr in instrs)
            {
                string ToText()
                {
                    switch (instr.Op.Name)
                    {
                        case "addr": return $"{vars[instr.C]} = {vars[instr.A]} + {vars[instr.B]};";
                        case "addi": return $"{vars[instr.C]} = {vars[instr.A]} + {instr.B};";
                        case "mulr": return $"{vars[instr.C]} = {vars[instr.A]} * {vars[instr.B]};";
                        case "muli": return $"{vars[instr.C]} = {vars[instr.A]} * {instr.B};";
                        case "banr": return $"{vars[instr.C]} = {vars[instr.A]} & {vars[instr.B]};";
                        case "bani": return $"{vars[instr.C]} = {vars[instr.A]} & {instr.B};";
                        case "borr": return $"{vars[instr.C]} = {vars[instr.A]} | {vars[instr.B]};";
                        case "bori": return $"{vars[instr.C]} = {vars[instr.A]} | {instr.B};";
                        case "setr": return $"{vars[instr.C]} = {vars[instr.A]};";
                        case "seti": return $"{vars[instr.C]} = {instr.A};";
                        case "gtir": return $"{vars[instr.C]} = {instr.A} > {vars[instr.B]} ? 1 : 0;";
                        case "gtri": return $"{vars[instr.C]} = {vars[instr.A]} > {instr.B} ? 1 : 0;";
                        case "gtrr": return $"{vars[instr.C]} = {vars[instr.A]} > {vars[instr.B]} ? 1 : 0;";
                        case "eqir": return $"{vars[instr.C]} = {instr.A} == {vars[instr.B]} ? 1 : 0;";
                        case "eqri": return $"{vars[instr.C]} = {vars[instr.A]} == {instr.B} ? 1 : 0;";
                        case "eqrr": return $"{vars[instr.C]} = {vars[instr.A]} == {vars[instr.B]} ? 1 : 0;";
                        default: throw new InvalidOperationException();
                    }
                }

                var line = ToText();


                // do these very last. also, order matters among them.
                line = Regex.Replace(line, @"(\w+) = \1 ([+\-*/|&]) (\S+);", "$1 $2= $3;");
                line = Regex.Replace(line, @"(\w+) = (\S+) ([+*|&]) \1;", "$1 $3= $2;");
                line = Regex.Replace(line, @"(\w+) += 1;", "++$1;");

                lines.Add(line);
            }

            var gotoTargets = new HashSet<int>();

            for (int i = 0; i < lines.Count; ++i)
            {
                var m = Regex.Match(lines[i], @"ip = (\d+)");
                if (m.Success)
                {
                    var target = int.Parse(m.Groups[1].Value) + 1;
                    gotoTargets.Add(target);
                    lines[i] = $"goto l_{target};";
                }
            }

            foreach (var gotoTarget in gotoTargets)
            {
                lines[gotoTarget] = $"l_{gotoTarget}: {lines[gotoTarget]}";
            }

            return lines;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Unity.Coding.Utils;

namespace Aoc2019
{
    public class IntCodeVM
    {
        public IntCodeVM(IEnumerable<BigInteger> mem, Func<int> nextInput)
            : this(mem) => NextInput = nextInput;

        public IntCodeVM(IEnumerable<int> mem, Func<int> nextInput)
            : this(mem) => NextInput = nextInput;

        public IntCodeVM(IEnumerable<int> mem)
            : this(mem.Select(m => (BigInteger)m).ToList()) { }

        public IntCodeVM(IEnumerable<BigInteger> mem)
        {
            var ip = 0;
            foreach (var m in mem)
                Mem[ip++] = m;
        }

        public IDictionary<BigInteger, BigInteger> Mem = new AutoDictionary<BigInteger, BigInteger>();
        public BigInteger MemPtr, BaseOffset;
        public Func<int> NextInput;

        BigInteger NextMem() => Mem[MemPtr++];

        public IEnumerable<string> Disassemble()
        {
            var disasm = new List<Instruction>();
            var count = BRun(disasm).Count();
            if (count != 0)
                throw new InvalidOperationException("unexpected yield while disassembling");

            return disasm.SelectStrings();
        }

        public IEnumerable<int> Run() =>
            BRun().Select(bi => (int)bi);

        public IEnumerable<BigInteger> BRun(List<Instruction> disasm = null)
        {
            for (; ; )
            {
                if (MemPtr < 0 || MemPtr >= Mem.Count)
                    break;

                var instr = (int)NextMem();
                var modes = instr / 100;
                var operation = (Operation)(instr % 100);

                Instruction instruction = null;
                disasm?.Add(instruction = new Instruction
                {
                    MemPtr = MemPtr - 1,
                    Operation = operation,
                    RawOperation = instr,
                });

                BigInteger Next(bool forWrite)
                {
                    var mode = modes % 10;
                    modes /= 10;

                    var item = NextMem();
                    if (mode == 2)
                        item += BaseOffset;

                    return forWrite || mode == 1 ? item : Mem[item];
                }

                BigInteger NextRead() => Next(false);
                BigInteger NextWrite() => Next(true);

                Param NextDisasm(string name)
                {
                    var mode = modes % 10;
                    modes /= 10;
                    return new Param { Name = name, Mode = (ParamMode)mode, Value = NextMem() };
                }

                void SrcSrcDst(Action<((BigInteger a, BigInteger b) src, BigInteger dst)> action, string opName, Func<Instruction, string> getComment)
                {
                    if (instruction == null)
                        action(((NextRead(), NextRead()), NextWrite()));
                    else
                    {
                        instruction.GetComment = getComment;
                        instruction.OperationText = opName;
                        instruction.Params = new[] { NextDisasm("src.a"), NextDisasm("src.b"), NextDisasm("dst") };
                    }
                }

                void SrcTgt(Action<(BigInteger src, BigInteger tgt)> action, string opName, Func<Instruction, string> getComment)
                {
                    if (instruction == null)
                        action((NextRead(), NextRead()));
                    else
                    {
                        instruction.GetComment = getComment;
                        instruction.OperationText = opName;
                        instruction.Params = new[] { NextDisasm("src"), NextDisasm("tgt") };
                    }
                }
                void Src(Action<BigInteger> action, string opName, Func<Instruction, string> getComment)
                {
                    if (instruction == null)
                        action(NextRead());
                    else
                    {
                        instruction.GetComment = getComment;
                        instruction.OperationText = opName;
                        instruction.Params = new[] { NextDisasm("src") };
                    }
                }

                void InpDst(Action<(int inp, BigInteger dst)> action, string opName, Func<Instruction, string> getComment)
                {
                    if (instruction == null)
                        action((NextInput(), NextWrite()));
                    else
                    {
                        instruction.GetComment = getComment;
                        instruction.OperationText = opName;
                        instruction.Params = new[] { NextDisasm("dst") };
                    }
                }

                switch (operation)
                {
                    // simple ops

                    case Operation.Add:
                        SrcSrcDst(
                            v => Mem[v.dst] = v.src.a + v.src.b,
                            "add", i => $"{i.Param("dst")} = {i.Param("src.a")} + {i.Param("src.b")}");
                        break;

                    case Operation.Multiply:
                        SrcSrcDst(
                            v => Mem[v.dst] = v.src.a * v.src.b,
                            "mul", i => $"{i.Param("dst")} = {i.Param("src.a")} * {i.Param("src.b")}");
                        break;

                    case Operation.LessThan:
                        SrcSrcDst(
                            v => Mem[v.dst] = v.src.a < v.src.b ? 1 : 0,
                            "les", i => $"{i.Param("dst")} = {i.Param("src.a")} < {i.Param("src.b")}");
                        break;

                    case Operation.Equals:
                        SrcSrcDst(
                            v => Mem[v.dst] = v.src.a == v.src.b ? 1 : 0,
                            "equ", i => $"{i.Param("dst")} = {i.Param("src.a")} == {i.Param("src.b")}");
                        break;

                    // jumps

                    case Operation.JumpIfTrue:
                        SrcTgt(
                            v => { if (v.src != 0) MemPtr = v.tgt; },
                            "jnz", i => $"if {i.Param("src")} goto {i.Param("tgt")}");
                        break;

                    case Operation.JumpIfFalse:
                        SrcTgt(
                            v => { if (v.src == 0) MemPtr = v.tgt; },
                            "jez", i => $"if !{i.Param("src")} goto {i.Param("tgt")}");
                        break;

                    // special

                    case Operation.RelativeOffset:
                        Src(v => BaseOffset += v, "rbo", i => $"rbo += {i.Param("src")}");
                        break;

                    case Operation.Input:
                        InpDst(v => Mem[v.dst] = v.inp, "inp", i => $"{i.Param("dst")} = input");
                        break;

                    case Operation.Output:
                        if (instruction == null)
                            yield return NextRead();
                        else
                        {
                            instruction.GetComment = i => $"output {i.Param("src")}";
                            instruction.OperationText = "out";
                            instruction.Params = new[] { NextDisasm("src") };
                        }
                        break;

                    case Operation.Halt:
                        if (instruction == null)
                        {
                            --MemPtr;
                            yield break;
                        }
                        instruction.GetComment = _ => "halt";
                        instruction.OperationText = "hlt";
                        break;

                    default:
                        if (instruction == null)
                            throw new InvalidOperationException();
                        instruction.GetComment = i => $"{i.Param("data")}";
                        instruction.Operation = Operation.Data;
                        instruction.OperationText = "dat";
                        instruction.Params = new[] { new Param { Name = "data", Mode = ParamMode.Immediate, Value = instr } };
                        break;
                }
            }
        }

        public enum Operation
        {
            // adds together numbers read from two positions and stores the result in a third position
            Add = 1,
            // works exactly like opcode 1, except it multiplies the two inputs instead of adding them
            Multiply = 2,
            // takes a single integer as input and saves it to the position given by its only parameter
            Input = 3,
            // outputs the value of its only parameter
            Output = 4,
            // if the first parameter is non-zero, it sets the instruction pointer to the value from the second parameter. Otherwise, it does nothing.
            JumpIfTrue = 5,
            // if the first parameter is zero, it sets the instruction pointer to the value from the second parameter. Otherwise, it does nothing.
            JumpIfFalse = 6,
            // if the first parameter is less than the second parameter, it stores 1 in the position given by the third parameter. Otherwise, it stores 0.
            LessThan = 7,
            // if the first parameter is equal to the second parameter, it stores 1 in the position given by the third parameter.Otherwise, it stores 0.
            Equals = 8,
            // adjusts the relative base by the value of its only parameter. The relative base increases (or decreases, if the value is negative) by the value of the parameter.
            RelativeOffset = 9,
            // stop execution
            Halt = 99,
            // pseudo op, placeholder for disasm
            Data = 100,
        }

        public enum ParamMode { Position = 0, Immediate = 1, Relative = 2 }

        public class Param
        {
            public string Name;
            public ParamMode Mode;
            public BigInteger Value;

            public override string ToString()
            {
                switch (Mode)
                {
                    case ParamMode.Position:
                        return $"[{Value}]";
                    case ParamMode.Immediate:
                        return $"#{Value}";
                    case ParamMode.Relative:
                        return Value < 0 ? $"[R-{-Value}]" : $"[R+{Value}]";
                }

                throw new InvalidOperationException();
            }
        }

        public class Instruction
        {
            public BigInteger MemPtr;
            public int RawOperation;
            public Operation Operation;
            public string OperationText;
            public Param[] Params;
            public Func<Instruction, string> GetComment;

            public Param Param(string name) => Params.Single(p => p.Name == name);

            public override string ToString()
            {
                var text = $"{MemPtr:00000}  ";
                text += Operation == Operation.Data
                    ? "   <data> "
                    : $"{RawOperation,5}:{OperationText} ";
                var paramText = Params != null ? Params.Select(p => p.Value).StringJoin(' ') : "";
                text += $"{paramText,-10} | {GetComment(this)}";

                return text;
            }
        }
    }
}

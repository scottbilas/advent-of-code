using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Unity.Coding.Utils;

// ReSharper disable RedundantCast

namespace Aoc2019
{
    public class IntCodeVM
    {
        public bool Paused;
        public int MemPtr, BaseOffset;
        public Func<int> NextInput;

        int[] m_ArrayMem;
        IDictionary<int, int> m_ExtraMem = new AutoDictionary<int, int>();

        public IntCodeVM(IEnumerable<int> mem, Func<int> nextInput = null)
        {
            m_ArrayMem = mem.ToArray();
            NextInput = nextInput;
        }

        public int GetMemAt(int ptr) =>
            ptr < m_ArrayMem.Length ? m_ArrayMem[(int)ptr] : m_ExtraMem[ptr];
        public int SetMemAt(int ptr, int value) =>
            ptr < m_ArrayMem.Length ? m_ArrayMem[(int)ptr] = value : m_ExtraMem[ptr] = value;
        int NextMem() => GetMemAt(MemPtr++);

        public IEnumerable<string> Disassemble()
        {
            var disasm = new List<Instruction>();
            var count = Run(disasm).Count();
            if (count != 0)
                throw new InvalidOperationException("unexpected yield while disassembling");

            return disasm.SelectStrings();
        }

        public IEnumerable<int> Run(params int[] inputs)
        {
            var index = 0;
            var oldFunc = NextInput;

            NextInput = () => inputs[index++];

            foreach (var i in Run())
                yield return i;

            NextInput = oldFunc;
        }

        public IEnumerable<int> Run(List<Instruction> disasm = null)
        {
            while (!Paused)
            {
                if (MemPtr == m_ArrayMem.Length)
                    break;

                var instr = NextMem();
                var modes = instr / 100;
                var operation = (Operation)(int)(instr % 100);

                Instruction instruction = null;
                disasm?.Add(instruction = new Instruction
                {
                    MemPtr = MemPtr - 1,
                    Operation = operation,
                    RawOperation = instr,
                });

                int Next(bool forWrite)
                {
                    var mode = modes % 10;
                    modes /= 10;

                    var item = NextMem();
                    if (mode == 2)
                        item += BaseOffset;

                    return forWrite || mode == 1 ? item : GetMemAt(item);
                }

                int NextRead() => Next(false);
                int NextWrite() => Next(true);

                Param NextDisasm(string name)
                {
                    var mode = (int)(modes % 10);
                    modes /= 10;
                    return new Param { Name = name, Mode = (ParamMode)mode, Value = NextMem() };
                }

                void SrcSrcDst(Action<((int a, int b) src, int dst)> action, string opName, Func<Instruction, string> getComment)
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

                void SrcTgt(Action<(int src, int tgt)> action, string opName, Func<Instruction, string> getComment)
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
                void Src(Action<int> action, string opName, Func<Instruction, string> getComment)
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

                void InpDst(Action<(int inp, int dst)> action, string opName, Func<Instruction, string> getComment)
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
                            v => SetMemAt(v.dst, v.src.a + v.src.b),
                            "add", i => $"{i.Param("dst")} = {i.Param("src.a")} + {i.Param("src.b")}");
                        break;

                    case Operation.Multiply:
                        SrcSrcDst(
                            v => SetMemAt(v.dst, v.src.a * v.src.b),
                            "mul", i => $"{i.Param("dst")} = {i.Param("src.a")} * {i.Param("src.b")}");
                        break;

                    case Operation.LessThan:
                        SrcSrcDst(
                            v => SetMemAt(v.dst, v.src.a < v.src.b ? 1 : 0),
                            "les", i => $"{i.Param("dst")} = {i.Param("src.a")} < {i.Param("src.b")}");
                        break;

                    case Operation.Equals:
                        SrcSrcDst(
                            v => SetMemAt(v.dst, v.src.a == v.src.b ? 1 : 0),
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
                        InpDst(v => SetMemAt(v.dst, v.inp), "inp", i => $"{i.Param("dst")} = input");
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
            public int Value;

            public override string ToString() =>
                Mode switch
                {
                    ParamMode.Position  => $"[{Value}]",
                    ParamMode.Immediate => Value.ToString(),
                    ParamMode.Relative  => (Value < 0 ? $"[R-{-Value}]" : $"[R+{Value}]"),
                    _ => throw new InvalidOperationException()
                };
        }

        public class Instruction
        {
            public int MemPtr;
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
                text += $"{paramText,-14} | {GetComment(this)}";

                return text;
            }
        }
    }

    public class BigIntCodeVM
    {
        public bool Paused;
        public BigInteger MemPtr, BaseOffset;
        public Func<BigInteger> NextInput;

        BigInteger[] m_ArrayMem;
        IDictionary<BigInteger, BigInteger> m_ExtraMem = new AutoDictionary<BigInteger, BigInteger>();

        public BigIntCodeVM(IEnumerable<BigInteger> mem, Func<BigInteger> nextInput = null)
        {
            m_ArrayMem = mem.ToArray();
            NextInput = nextInput;
        }

        public BigInteger GetMemAt(BigInteger ptr) =>
            ptr < m_ArrayMem.Length ? m_ArrayMem[(int)ptr] : m_ExtraMem[ptr];
        public BigInteger SetMemAt(BigInteger ptr, BigInteger value) =>
            ptr < m_ArrayMem.Length ? m_ArrayMem[(int)ptr] = value : m_ExtraMem[ptr] = value;
        BigInteger NextMem() => GetMemAt(MemPtr++);

        public IEnumerable<string> Disassemble()
        {
            var disasm = new List<Instruction>();
            var count = Run(disasm).Count();
            if (count != 0)
                throw new InvalidOperationException("unexpected yield while disassembling");

            return disasm.SelectStrings();
        }

        public IEnumerable<BigInteger> Run(params BigInteger[] inputs)
        {
            var index = 0;
            var oldFunc = NextInput;

            NextInput = () => inputs[index++];

            foreach (var i in Run())
                yield return i;

            NextInput = oldFunc;
        }

        public IEnumerable<BigInteger> Run(List<Instruction> disasm = null)
        {
            while (!Paused)
            {
                if (MemPtr >= m_ArrayMem.Length)
                    break;

                var instr = NextMem();
                var modes = instr / 100;
                var operation = (Operation)(int)(instr % 100);

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

                    return forWrite || mode == 1 ? item : GetMemAt(item);
                }

                BigInteger NextRead() => Next(false);
                BigInteger NextWrite() => Next(true);

                Param NextDisasm(string name)
                {
                    var mode = (int)(modes % 10);
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

                void InpDst(Action<(BigInteger inp, BigInteger dst)> action, string opName, Func<Instruction, string> getComment)
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
                            v => SetMemAt(v.dst, v.src.a + v.src.b),
                            "add", i => $"{i.Param("dst")} = {i.Param("src.a")} + {i.Param("src.b")}");
                        break;

                    case Operation.Multiply:
                        SrcSrcDst(
                            v => SetMemAt(v.dst, v.src.a * v.src.b),
                            "mul", i => $"{i.Param("dst")} = {i.Param("src.a")} * {i.Param("src.b")}");
                        break;

                    case Operation.LessThan:
                        SrcSrcDst(
                            v => SetMemAt(v.dst, v.src.a < v.src.b ? 1 : 0),
                            "les", i => $"{i.Param("dst")} = {i.Param("src.a")} < {i.Param("src.b")}");
                        break;

                    case Operation.Equals:
                        SrcSrcDst(
                            v => SetMemAt(v.dst, v.src.a == v.src.b ? 1 : 0),
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
                        InpDst(v => SetMemAt(v.dst, v.inp), "inp", i => $"{i.Param("dst")} = input");
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
                        instruction.GetComment = i =>
                        {
                            var data = i.Param("data");
                            var text = data.ToString();
                            if (instr >= 32 && instr < 127)
                                text += $" '{(char)instr}'";
                            return text;
                        };
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

            public override string ToString() =>
                Mode switch
                {
                    ParamMode.Position  => $"[{Value}]",
                    ParamMode.Immediate => Value.ToString(),
                    ParamMode.Relative  => (Value < 0 ? $"[R-{-Value}]" : $"[R+{Value}]"),
                    _ => $"???{Value}" //throw new InvalidOperationException()
                };
        }

        public class Instruction
        {
            public BigInteger MemPtr;
            public BigInteger RawOperation;
            public Operation Operation;
            public string OperationText;
            public Param[] Params;
            public Func<Instruction, string> GetComment;

            public Param Param(string name) => Params.Single(p => p.Name == name);

            public override string ToString()
            {
                var text = $"{MemPtr:00000}  ";
                var paramText = "";

                if (Operation == Operation.Data)
                    text += "   <data> ";
                else
                {
                    text += $"{RawOperation,5}:{OperationText} ";
                    if (Params != null)
                        paramText = Params.Select(p => p.Value).StringJoin(' ');
                }

                text += $"{paramText,-14} | {GetComment(this)}";

                return text;
            }
        }
    }

}

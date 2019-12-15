using System;
using NUnit.Framework;
using Unity.Coding.Utils;
using static Aoc2019.MiscStatics;

namespace Aoc2019
{
    class IntCodeVMTests
    {
        [Test]
        public void Test()
        {
            var disasm = new IntCodeVM(Arr(3, 15, 3, 16, 1002, 16, 10, 16, 1, 16, 15, 15, 4, 15, 99, 0, 0)).Disassemble();

            Console.WriteLine(disasm.StringJoin("\n"));
        }

        [Test]
        public void Test2()
        {
            var disasm = new IntCodeVM(Arr(
                3, 21, 1008, 21, 8, 20, 1005, 20, 22, 107, 8, 21, 20, 1006, 20, 31,
                1106, 0, 36, 98, 0, 0, 1002, 21, 125, 20, 4, 20, 1105, 1, 46, 104,
                999, 1105, 1, 46, 1101, 1000, 1, 20, 4, 20, 1105, 1, 46, 98, 99
                )).Disassemble();

            Console.WriteLine(disasm.StringJoin("\n"));

        }

        [Test]
        public void Test3()
        {
            var disasm = new IntCodeVM(@"C:\proj\advent-of-code\2019\aoc\day5\day5.input.txt".ToNPath().ReadAllInts()).Disassemble();
            Console.WriteLine(disasm.StringJoin("\n"));
        }
    }
}

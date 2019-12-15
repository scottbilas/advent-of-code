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
            var disasm = new IntCodeVM(BArr(3, 15, 3, 16, 1002, 16, 10, 16, 1, 16, 15, 15, 4, 15, 99, 0, 0)).Disassemble();

            Console.WriteLine(disasm.StringJoin("\n"));
        }
    }
}

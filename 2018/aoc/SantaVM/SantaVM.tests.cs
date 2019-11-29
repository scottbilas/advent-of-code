using NUnit.Framework;
using System.Diagnostics;
using AoC;

namespace SantaVM
{
    class DisassemblerTests : AocFixture
    {
        [Test]
        public void Basics()
        {
            var parsed = Parser.Parse(@"
                #ip 0
                seti 5 0 1
                seti 6 0 2
                addi 0 1 0
                addr 1 2 3
                setr 1 0 0
                seti 8 0 4
                seti 9 0 5");

            var text = Disassembler
                .Disassemble(parsed.boundIP, parsed.instrs)
                .StringJoin('\n');
            Debug.WriteLine(text);
        }

        [Test]
        public void Day19()
        {
            var parsed = Parser.Parse(ScriptDir.Combine("../Day19/input.txt").ReadAllText());

            var text = Disassembler
                .Disassemble(parsed.boundIP, parsed.instrs)
                .StringJoin('\n');
            Debug.WriteLine(text);
        }
    }
}

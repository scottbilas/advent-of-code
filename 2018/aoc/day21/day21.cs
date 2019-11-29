using System.Collections.Generic;
using System.Linq;

namespace Day21
{
    static class Solver
    {
        static IEnumerable<int> RunProgram()
        {
            for (var r = 0; ;)
            {
                var a = r | 0x10000;

                r = 9450265;
                r = (((r + ( a        & 0xff)) & 0xffffff) * 65899) & 0xffffff;
                r = (((r + ((a >> 8)  & 0xff)) & 0xffffff) * 65899) & 0xffffff;
                r = (((r + ((a >> 16) & 0xff)) & 0xffffff) * 65899) & 0xffffff;

                yield return r;
            }
        }

        public static int Part1()
            => RunProgram().First();

        public static int Part2()
        {
            var matched = new HashSet<int>();
            return RunProgram().TakeWhile(r => matched.Add(r)).Last();
        }
    }
}

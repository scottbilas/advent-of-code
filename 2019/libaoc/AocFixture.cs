using NUnit.Framework;
using Unity.Coding.Utils;

namespace Aoc2019
{
    [Parallelizable(ParallelScope.All)]
    public class AocFixture
    {
        protected NPath ScriptDir => new NPath(TestContext.CurrentContext.TestDirectory).Combine("../../..", GetType().Name);
    }
}

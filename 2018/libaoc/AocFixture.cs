using NiceIO;
using NUnit.Framework;

namespace AoC
{
    [Parallelizable(ParallelScope.All)]
    public class AocFixture
    {
        protected NPath ScriptDir => new NPath(TestContext.CurrentContext.TestDirectory).Combine("../../..", GetType().Name);
    }
}

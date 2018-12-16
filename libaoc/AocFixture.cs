using NiceIO;
using NUnit.Framework;

[Parallelizable(ParallelScope.All)]
public class AocFixture
{
    protected NPath ScriptDir => new NPath(TestContext.CurrentContext.TestDirectory).Combine("../../..", GetType().Name);
}

using NiceIO;
using NUnit.Framework;

[Parallelizable(ParallelScope.All)]
public class AocFixture
{
    protected string ScriptDir => new NPath(TestContext.CurrentContext.TestDirectory).Parent.Parent.Combine(GetType().Name);
}

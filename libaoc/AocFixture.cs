using NiceIO;
using NUnit.Framework;

public class AocFixture
{
    NPath m_scriptDir = new NPath(TestContext.CurrentContext.TestDirectory).Parent.Parent.Combine("Day15");

    public string ScriptDir => m_scriptDir;
}

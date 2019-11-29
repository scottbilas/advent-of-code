using NUnit.Framework;
using Unity.Coding.Utils;

namespace NiceIO.Tests
{
    [TestFixture]
    public class Elements
    {
        [Test]
        public void Test()
        {
            CollectionAssert.AreEqual(new[] {"my", "path", "to", "somewhere.txt"}, new NPath("/my/path/to/somewhere.txt").Elements);
        }
    }
}

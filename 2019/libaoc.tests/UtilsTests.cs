using NUnit.Framework;
using Shouldly;

namespace Aoc2019
{
    partial class UtilsTests
    {
        [Test]
        public void Lcm()
        {
            Utils.Lcm(new[] { 2028, 5898, 4702 }).ShouldBe(4686774924);
            Utils.Lcm(new[] { 2028L, 5898L, 4702L }).ShouldBe(4686774924);
        }
    }
}

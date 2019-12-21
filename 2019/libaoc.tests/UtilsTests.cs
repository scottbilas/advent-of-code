using System;
using NUnit.Framework;
using Shouldly;

namespace Aoc2019
{
    partial class UtilsTests
    {
        [Test]
        public void Lcm()
        {
            Utils.Lcm(new[] { 202, 589, 470 }).ShouldBe(27959830);
            Utils.Lcm(2028, 5898).ShouldBe(1993524);
            Should.Throw<OverflowException>(() => Utils.Lcm(new[] { 2028, 5898, 4702 }));
            Utils.Lcm(new[] { 2028L, 5898L, 4702L }).ShouldBe(4686774924);
        }
    }
}

using System;
using System.Linq;
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

        [Test]
        public void RenderGraphViz()
        {
            var bytes = Utils.RenderGraphViz(@"
                graph {
                    a -- b;
                    b -- c;
                    a -- c;
                    d -- c;
                    e -- c;
                    e -- a;
                }");

            bytes.Take(8).ShouldBe(new byte[] { 137, 80, 78, 71, 13, 10, 26, 10 });
        }
    }
}

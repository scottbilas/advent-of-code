using System;
using System.Linq;
using NUnit.Framework;
using Shouldly;
using static Aoc2017.MiscStatics;

namespace Aoc2017
{
    partial class UtilsTests
    {
        [Test] public void WrapIndex()
        {
            Utils.WrapIndex(1, 5).ShouldBe(1);
            Utils.WrapIndex(9, 5).ShouldBe(4);
            Utils.WrapIndex(-2, 5).ShouldBe(3);
            Utils.WrapIndex(-10, 5).ShouldBe(0);
        }

        [Test] public void BounceIndex()
        {
            var expected = Arr(
                0, 1, 2, 3, 2, 1, 0, 1, 2, 3, 2, 1,        // negatives
                0, 1, 2, 3, 2, 1, 0, 1, 2, 3, 2, 1, 0);    // positives

            var actual = Enumerable
                .Range(-12, 25)
                .Select(i => Utils.BounceIndex(i, 4))
                .ToArray();

            actual.ShouldBe(expected);
        }

        [Test] public void Lcm()
        {
            Utils.Lcm(202, 589, 470).ShouldBe(27959830);
            Utils.Lcm(2028, 5898).ShouldBe(1993524);
            Should.Throw<OverflowException>(() => Utils.Lcm(2028, 5898, 4702));
            Utils.Lcm(2028L, 5898L, 4702L).ShouldBe(4686774924);
        }

        [Test] public void RenderGraphViz()
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

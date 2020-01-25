using System.Linq;
using NUnit.Framework;
using Shouldly;
using static Aoc2017.MiscStatics;

namespace Aoc2017
{
    class RingArrayTests
    {
        [Test] public void Indexing()
        {
            var ring = Ring.Array(Arr(1, 2, 3, 4, 5));

            ring[1] = 6;
            ring[1].ShouldBe(6);
            ring.ShouldBe(Arr(1, 6, 3, 4, 5));

            ring[9] = 7;
            ring[9].ShouldBe(7);
            ring[4].ShouldBe(7);
            ring.ShouldBe(Arr(1, 6, 3, 4, 7));

            ring[-2] = 8;
            ring[-2].ShouldBe(8);
            ring[3].ShouldBe(8);
            ring.ShouldBe(Arr(1, 6, 3, 8, 7));

            ring[-10] = 9;
            ring[-10].ShouldBe(9);
            ring.ShouldBe(Arr(9, 6, 3, 8, 7));
        }
    }

    class RingTests
    {
        [Test] public void RemoveAt()
        {
            var ring = Ring.List(Arr(1, 2, 3, 4, 5, 6, 7, 8, 9, 10));

            ring.RemoveAt(1);
            ring.ToArray().ShouldBe(Arr(1, 3, 4, 5, 6, 7, 8, 9, 10));

            ring.RemoveAt(10);
            ring.ToArray().ShouldBe(Arr(1, 4, 5, 6, 7, 8, 9, 10));

            ring.RemoveAt(-1);
            ring.ToArray().ShouldBe(Arr(1, 4, 5, 6, 7, 8, 9));

            ring.RemoveAt(-10);
            ring.ToArray().ShouldBe(Arr(1, 4, 5, 6, 8, 9));
        }

        [Test] public void Insert()
        {
            var ring = Ring.List(Arr(1, 2, 3, 4, 5));

            ring.Insert(1, 6);
            ring.ToArray().ShouldBe(Arr(1, 6, 2, 3, 4, 5));

            ring.Insert(10, 7);
            ring.ToArray().ShouldBe(Arr(1, 6, 2, 3, 7, 4, 5));

            ring.Insert(-1, 8);
            ring.ToArray().ShouldBe(Arr(1, 6, 2, 3, 7, 4, 8, 5));

            ring.Insert(-10, 9);
            ring.ToArray().ShouldBe(Arr(1, 6, 2, 3, 7, 4, 9, 8, 5));
        }
    }
}

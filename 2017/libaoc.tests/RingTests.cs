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
            var ring = Arr(1, 2, 3, 4, 5).ToRingArray();

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

        [Test] public void Offset()
        {
            var ring = Arr(1, 2, 3, 4, 5).ToRingArray();
            ring.ShouldBe(Arr(1, 2, 3, 4, 5));
            ring.ToArray().ShouldBe(Arr(1, 2, 3, 4, 5));

            ring.Offset = 2;
            ring[1] = 6;
            ring[1].ShouldBe(6);
            ring.Data.ShouldBe(Arr(1, 2, 3, 6, 5));
            ring.ShouldBe(Arr(3, 6, 5, 1, 2));
            ring.ToArray().ShouldBe(Arr(3, 6, 5, 1, 2));

            ring.Offset = -2;
            ring[9] = 7;
            ring[9].ShouldBe(7);
            ring[4].ShouldBe(7);
            ring.Data.ShouldBe(Arr(1, 2, 7, 6, 5));
            ring.ShouldBe(Arr(6, 5, 1, 2, 7));
            ring.ToArray().ShouldBe(Arr(6, 5, 1, 2, 7));

            ring.Offset = 8;
            ring[-2] = 8;
            ring[-2].ShouldBe(8);
            ring[3].ShouldBe(8);
            ring.Data.ShouldBe(Arr(1, 8, 7, 6, 5));
            ring.ShouldBe(Arr(6, 5, 1, 8, 7));
            ring.ToArray().ShouldBe(Arr(6, 5, 1, 8, 7));

            ring.Offset = 3;
            ring[-10] = 9;
            ring[-10].ShouldBe(9);
            ring.Data.ShouldBe(Arr(1, 8, 7, 9, 5));
            ring.ShouldBe(Arr(9, 5, 1, 8, 7));
            ring.ToArray().ShouldBe(Arr(9, 5, 1, 8, 7));
        }
    }

    class RingListTests
    {
        [Test] public void RemoveAt()
        {
            var ring = Arr(1, 2, 3, 4, 5, 6, 7, 8, 9, 10).ToRingList();

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
            var ring = Arr(1, 2, 3, 4, 5).ToRingList();

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

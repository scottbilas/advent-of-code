using System;
using System.Linq;
using NUnit.Framework;
using Shouldly;

namespace Aoc2017
{
    class Int2Tests
    {
        [Test]
        public void Indexers_WithInvalidOffsets_ShouldThrow()
        {
            var int2 = new Int2(1, 2);

            Should.Throw<ArgumentOutOfRangeException>(() => int2[-1]);
            Should.Throw<ArgumentOutOfRangeException>(() => int2[2] = 5);
        }

        [Test]
        public void Indexers_WithValidOffsets_ShouldSetGetIndividualComponents()
        {
            var int2 = new Int2(1, 2);

            int2.X.ShouldBe(1);
            int2.Y.ShouldBe(2);

            int2[0] = -10;
            int2[1] = -20;

            int2.X.ShouldBe(-10);
            int2.Y.ShouldBe(-20);
        }
    }

    class Int3Tests
    {
        [Test]
        public void Indexers_WithInvalidOffsets_ShouldThrow()
        {
            var int3 = new Int3(1, 2, 3);

            Should.Throw<ArgumentOutOfRangeException>(() => int3[-1]);
            Should.Throw<ArgumentOutOfRangeException>(() => int3[3] = 5);
        }

        [Test]
        public void Indexers_WithValidOffsets_ShouldSetGetIndividualComponents()
        {
            var int3 = new Int3(1, 2, 3);

            int3.X.ShouldBe(1);
            int3.Y.ShouldBe(2);
            int3.Z.ShouldBe(3);

            int3[0] = -10;
            int3[1] = -20;
            int3[2] = -30;

            int3.X.ShouldBe(-10);
            int3.Y.ShouldBe(-20);
            int3.Z.ShouldBe(-30);
        }
    }

    class Int4Tests
    {
        [Test]
        public void Indexers_WithInvalidOffsets_ShouldThrow()
        {
            var int4 = new Int4(1, 2, 3, 4);

            Should.Throw<ArgumentOutOfRangeException>(() => int4[-1]);
            Should.Throw<ArgumentOutOfRangeException>(() => int4[4] = 5);
        }

        [Test]
        public void Indexers_WithValidOffsets_ShouldSetGetIndividualComponents()
        {
            var int4 = new Int4(1, 2, 3, 4);

            int4.X.ShouldBe(1);
            int4.Y.ShouldBe(2);
            int4.Z.ShouldBe(3);
            int4.W.ShouldBe(4);

            int4[0] = -10;
            int4[1] = -20;
            int4[2] = -30;
            int4[3] = -40;

            int4.X.ShouldBe(-10);
            int4.Y.ShouldBe(-20);
            int4.Z.ShouldBe(-30);
            int4.W.ShouldBe(-40);
        }
    }

    class AABBTests
    {
        [Test]
        public void Corners_Basics_YieldsExpectedOrdering()
        {
            var aabb = new AABB(new Int3(0, 0, 0), new Int3(1, 1, 1));
            var corners = aabb.Corners.ToList();

            corners[0].ShouldBe(new Int3(0, 0, 0));
            corners[1].ShouldBe(new Int3(1, 0, 0));
            corners[2].ShouldBe(new Int3(1, 1, 0));
            corners[3].ShouldBe(new Int3(0, 1, 0));
            corners[4].ShouldBe(new Int3(0, 1, 1));
            corners[5].ShouldBe(new Int3(0, 0, 1));
            corners[6].ShouldBe(new Int3(1, 0, 1));
            corners[7].ShouldBe(new Int3(1, 1, 1));
        }

        [Test]
        public void Intersects_BasicsEachAxis_ReturnsValid()
        {
            new AABB(0, 0, 0, 2, 2, 2).Intersects(new AABB(-2, -2, -2, 3, 3, 3)).ShouldBeTrue();
            new AABB(0, 0, 0, 2, 2, 2).Intersects(new AABB(-2, -2, -2, 3, 3, -1)).ShouldBeFalse();
            new AABB(0, 0, 0, 2, 2, 2).Intersects(new AABB(-2, -2, -2, 3, 3, 1)).ShouldBeTrue();
            new AABB(0, 0, 0, 2, 2, 2).Intersects(new AABB(-2, -2, -2, 3, 1, 1)).ShouldBeTrue();
            new AABB(0, 0, 0, 2, 2, 2).Intersects(new AABB(1, -2, -2, 3, 1, 1)).ShouldBeTrue();
            new AABB(0, 0, 0, 2, 2, 2).Intersects(new AABB(3, -2, -2, 3, 1, 1)).ShouldBeFalse();
        }

        [Test]
        public void Intersect_BasicsEachAxis_ReturnsValid()
        {
            AABB test;
                
            test = new AABB(0, 0, 0, 2, 2, 2);
            test.Intersect(new AABB(-2, -2, -2, 3, 3, 3)).ShouldBeTrue();
            test.ShouldBe(new AABB(0, 0, 0, 2, 2, 2));

            test = new AABB(0, 0, 0, 2, 2, 2);
            test.Intersect(new AABB(-2, -2, -2, 3, 3, -1)).ShouldBeFalse();

            test = new AABB(0, 0, 0, 2, 2, 2);
            test.Intersect(new AABB(-2, -2, -2, 3, 3, 1)).ShouldBeTrue();
            test.ShouldBe(new AABB(0, 0, 0, 2, 2, 1));

            test = new AABB(0, 0, 0, 2, 2, 2);
            test.Intersect(new AABB(-2, -2, -2, 3, 1, 1)).ShouldBeTrue();
            test.ShouldBe(new AABB(0, 0, 0, 2, 1, 1));

            test = new AABB(0, 0, 0, 2, 2, 2);
            test.Intersect(new AABB(1, -2, -2, 3, 1, 1)).ShouldBeTrue();
            test.ShouldBe(new AABB(1, 0, 0, 2, 1, 1));

            test = new AABB(0, 0, 0, 2, 2, 2);
            test.Intersect(new AABB(3, -2, -2, 3, 1, 1)).ShouldBeFalse();
        }
    }

    partial class UtilsTests
    {
        [Test]
        public void Midpoint3_Basics_ReturnsValid()
        {
            var (a, b) = (new Int3(0, 1, 2), new Int3(4, 7, 14));
            var midpoint = Utils.Midpoint(a, b);

            midpoint.ShouldBe(new Int3(2, 4, 8));
        }

        [Test]
        public void Midpoint4_Basics_ReturnsValid()
        {
            var (a, b) = (new Int4(0, 1, 2, 3), new Int4(4, 7, 14, 29));
            var midpoint = Utils.Midpoint(a, b);

            midpoint.ShouldBe(new Int4(2, 4, 8, 16));
        }
    }
}

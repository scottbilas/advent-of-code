using System.Linq;
using NUnit.Framework;
using Shouldly;

namespace AoC
{
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

    class UtilsTests
    {
        [Test]
        public void Midpoint_Basics_ReturnsValid()
        {
            var (a, b) = (new Int3(0, 1, 2), new Int3(4, 7, 14));
            var midpoint = Utils.Midpoint(a, b);

            midpoint.ShouldBe(new Int3(2, 4, 8));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Aoc2019
{
    public struct AABB : IEquatable<AABB>
    {
        public Int3 Min;
        public Int3 Max;

        public AABB(in Int3 min, in Int3 max)
            => (Min, Max) = (min, max);

        public AABB(int xMin, int yMin, int zMin, int xMax, int yMax, int zMax)
            : this(new Int3(xMin, yMin, zMin), new Int3(xMax, yMax, zMax)) { }

        public bool Equals(AABB other)
            => Min.Equals(other.Min) && Max.Equals(other.Max);

        public override bool Equals(object obj)
            => !ReferenceEquals(obj, null) && obj is AABB other && Equals(other);

        public override int GetHashCode()
            { unchecked { return (Min.GetHashCode() * 397) ^ Max.GetHashCode(); } }

        public static bool operator ==(in AABB left, in AABB right) => left.Equals(right);
        public static bool operator !=(in AABB left, in AABB right) => !left.Equals(right);

        public void Validate()
        {
            if (!(Min <= Max))
                throw new Exception("Min must be <= max");
        }

        public override string ToString()
            => $"{Min} => {Max}";

        public Int3 Size
            => Max - Min;

        public Int3 Center
            => Min.Midpoint(in Max);

        public void Encapsulate(in AABB aabb)
            => (Min, Max) = (Min.Min(in aabb.Min), Max = Max.Max(in aabb.Max));

        public void Encapsulate(in Int3 point)
            => (Min, Max) = (Min.Min(in point), Max.Max(in point));

        public bool Intersect(in AABB aabb)
        {
            Min = Min.Max(in aabb.Min);
            Max = Max.Min(in aabb.Max);
            return Min <= Max;
        }

        public bool Contains(in Int3 point)
            => point.X >= Min.X && point.Y >= Min.Y && point.Z >= Min.Z &&
               point.X <= Max.X && point.Y <= Max.Y && point.Z <= Max.Z;

        public bool Contains(in AABB aabb)
            => aabb.Min >= Min && aabb.Max <= Max;

        public bool Intersects(in AABB aabb)
            => aabb.Min.X <= Max.X && Min.X <= aabb.Max.X &&
               aabb.Min.Y <= Max.Y && Min.Y <= aabb.Max.Y &&
               aabb.Min.Z <= Max.Z && Min.Z <= aabb.Max.Z;

        public IEnumerable<Int3> Corners
        {
            get
            {
                yield return Min;                               //       4+------+7 Max
                yield return new Int3(Max.X, Min.Y, Min.Z);     //       /|     /|
                yield return new Int3(Max.X, Max.Y, Min.Z);     //     3+------+2|
                yield return new Int3(Min.X, Max.Y, Min.Z);     //      | |   2| |   
                yield return new Int3(Min.X, Max.Y, Max.Z);     //      |5+----|-+6
                yield return new Int3(Min.X, Min.Y, Max.Z);     //      |/     |/    
                yield return new Int3(Max.X, Min.Y, Max.Z);     // Min 0+------+1
                yield return Max;
            }
        }

        public static AABB FromExtents(in Int3 center, in Int3 extents)
        {
            if (!(extents > 0))
                throw new ArgumentException("Extents cannot be negative", nameof(extents));
            return new AABB(center - extents, center + extents);
        }

        public static AABB FromExtents(in Int3 center, in int extents)
        {
            if (extents < 0)
                throw new ArgumentException("Extents cannot be negative", nameof(extents));
            return new AABB(center - extents, center + extents);
        }

        public static AABB FromPoints([NotNull] IEnumerable<Int3> points)
        {
            var aabb = new AABB(new Int3(int.MaxValue), new Int3(int.MinValue));
            foreach (var point in points)
            {
                aabb.Min.Minimize(in point);
                aabb.Max.Maximize(in point);
            }
            return aabb;
        }

        public static AABB FromAABBs([NotNull] IEnumerable<AABB> aabbs)
        {
            var combined = new AABB(new Int3(int.MaxValue), new Int3(int.MinValue));
            foreach (var aabb in aabbs)
                combined.Encapsulate(in aabb);
            return combined;
        }

        public static AABB FromPoints([NotNull] params Int3[] points)
            => FromPoints(points.AsEnumerable());
    }

    public static partial class Utils
    {
        public static AABB? IntersectAll([NotNull] IEnumerable<AABB> aabbs)
        {
            var intersection = new AABB(new Int3(int.MinValue), new Int3(int.MaxValue));
            return aabbs.All(i => intersection.Intersect(in i)) ? intersection : (AABB?)null;
        }
    }

    public static partial class Extensions
    {
        public static AABB CalcAABB([NotNull] this IEnumerable<Int3> @this) => AABB.FromPoints(@this);
        public static AABB CalcAABB([NotNull] this IEnumerable<AABB> @this) => AABB.FromAABBs(@this);
        public static AABB? IntersectAll([NotNull] this IEnumerable<AABB> @this) => Utils.IntersectAll(@this);
    }
}

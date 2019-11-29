using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace AoC
{
    public struct Int3 : IEquatable<Int3>
    {
        public int X, Y, Z;

        public Int3(int x, int y, int z)
            => (X, Y, Z) = (x, y, z);
        public Int3(int v)
            => (X, Y, Z) = (v, v, v);
        public Int3(IEnumerable<int> xyz)
            => (X, Y, Z) = xyz.First3();
        public Int3(ValueTuple<int, int, int> xyz)
            => (X, Y, Z) = xyz;

        public unsafe int this[int index]
        {
            get
            {
                if (index < 0 || index > 2)
                    throw new ArgumentOutOfRangeException(nameof(index));
                fixed (int* i = &X)
                {
                    return i[index];
                }
            }
            set
            {
                if (index < 0 || index > 2)
                    throw new ArgumentOutOfRangeException(nameof(index));
                fixed (int* i = &X)
                {
                    i[index] = value;
                }
            }
        }

        public bool Equals(Int3 other)
            => X == other.X && Y == other.Y && Z == other.Z;

        public override bool Equals(object obj)
            => !ReferenceEquals(obj, null) && obj is Int3 other && Equals(other);

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = X;
                hashCode = (hashCode * 397) ^ Y;
                hashCode = (hashCode * 397) ^ Z;
                return hashCode;
            }
        }

        public static bool operator ==(in Int3 left, in Int3 right) => left.Equals(right);
        public static bool operator !=(in Int3 left, in Int3 right) => !left.Equals(right);

        public override string ToString()
            => $"{X}, {Y}, {Z}";

        static readonly Int3 k_Zero = new Int3(0), k_One = new Int3(1);
        public static ref readonly Int3 Zero => ref k_Zero;
        public static ref readonly Int3 One => ref k_One;

        public Int3 Abs()
            => new Int3(Math.Abs(X), Math.Abs(Y), Math.Abs(Z));

        public static Int3 operator +(in Int3 a, in Int3 b)
            => new Int3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        public static Int3 operator +(in Int3 a, int d)
            => new Int3(a.X + d, a.Y + d, a.Z + d);
        public static Int3 operator -(in Int3 a, in Int3 b)
            => new Int3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        public static Int3 operator -(in Int3 a, int d)
            => new Int3(a.X - d, a.Y - d, a.Z - d);
        public static Int3 operator *(in Int3 a, in Int3 b)
            => new Int3(a.X * b.X, a.Y * b.Y, a.Z * b.Z);
        public static Int3 operator *(in Int3 a, int d)
            => new Int3(a.X * d, a.Y * d, a.Z * d);
        public static Int3 operator /(in Int3 a, in Int3 b)
            => new Int3(a.X / b.X, a.Y / b.Y, a.Z / b.Z);
        public static Int3 operator /(in Int3 a, int d)
            => new Int3(a.X / d, a.Y / d, a.Z / d);

        public static bool operator <(in Int3 a, in Int3 b)
            => a.X < b.X && a.Y < b.Y && a.Z < b.Z;
        public static bool operator <(in Int3 a, int b)
            => a.X < b && a.Y < b && a.Z < b;
        public static bool operator <(int a, in Int3 b)
            => a < b.X && a < b.Y && a < b.Z;

        public static bool operator <=(in Int3 a, in Int3 b)
            => a.X <= b.X && a.Y <= b.Y && a.Z <= b.Z;
        public static bool operator <=(in Int3 a, int b)
            => a.X <= b && a.Y <= b && a.Z <= b;
        public static bool operator <=(int a, in Int3 b)
            => a <= b.X && a <= b.Y && a <= b.Z;

        public static bool operator >(in Int3 a, in Int3 b)
            => a.X > b.X && a.Y > b.Y && a.Z > b.Z;
        public static bool operator >(in Int3 a, int b)
            => a.X > b && a.Y > b && a.Z > b;
        public static bool operator >(int a, in Int3 b)
            => a > b.X && a > b.Y && a > b.Z;

        public static bool operator >=(in Int3 a, in Int3 b)
            => a.X >= b.X && a.Y >= b.Y && a.Z >= b.Z;
        public static bool operator >=(in Int3 a, int b)
            => a.X >= b && a.Y >= b && a.Z >= b;
        public static bool operator >=(int a, in Int3 b)
            => a >= b.X && a >= b.Y && a >= b.Z;
    }

    public struct Int4 : IEquatable<Int4>
    {
        public int X, Y, Z, W;

        public Int4(int x, int y, int z, int w)
            => (X, Y, Z, W) = (x, y, z, w);
        public Int4(int v)
            => (X, Y, Z, W) = (v, v, v, v);
        public Int4(IEnumerable<int> xyzw)
            => (X, Y, Z, W) = xyzw.First4();
        public Int4(ValueTuple<int, int, int, int> xyzw)
            => (X, Y, Z, W) = xyzw;

        public unsafe int this[int index]
        {
            get
            {
                if (index < 0 || index > 3)
                    throw new ArgumentOutOfRangeException(nameof(index));
                fixed (int* i = &X)
                {
                    return i[index];
                }
            }
            set
            {
                if (index < 0 || index > 3)
                    throw new ArgumentOutOfRangeException(nameof(index));
                fixed (int* i = &X)
                {
                    i[index] = value;
                }
            }
        }

        public bool Equals(Int4 other)
            => X == other.X && Y == other.Y && Z == other.Z && W == other.W;

        public override bool Equals(object obj)
            => !ReferenceEquals(obj, null) && obj is Int4 other && Equals(other);

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = X;
                hashCode = (hashCode * 397) ^ Y;
                hashCode = (hashCode * 397) ^ Z;
                hashCode = (hashCode * 397) ^ W;
                return hashCode;
            }
        }

        public static bool operator ==(in Int4 left, in Int4 right) => left.Equals(right);
        public static bool operator !=(in Int4 left, in Int4 right) => !left.Equals(right);

        public override string ToString()
            => $"{X}, {Y}, {Z}, {W}";

        static readonly Int4 k_Zero = new Int4(0), k_One = new Int4(1);
        public static ref readonly Int4 Zero => ref k_Zero;
        public static ref readonly Int4 One => ref k_One;

        public Int4 Abs()
            => new Int4(Math.Abs(X), Math.Abs(Y), Math.Abs(Z), Math.Abs(W));

        public static Int4 operator +(in Int4 a, in Int4 b)
            => new Int4(a.X + b.X, a.Y + b.Y, a.Z + b.Z, a.W + b.W);
        public static Int4 operator +(in Int4 a, int d)
            => new Int4(a.X + d, a.Y + d, a.Z + d, a.W + d);
        public static Int4 operator -(in Int4 a, in Int4 b)
            => new Int4(a.X - b.X, a.Y - b.Y, a.Z - b.Z, a.W - b.W);
        public static Int4 operator -(in Int4 a, int d)
            => new Int4(a.X - d, a.Y - d, a.Z - d, a.W - d);
        public static Int4 operator *(in Int4 a, in Int4 b)
            => new Int4(a.X * b.X, a.Y * b.Y, a.Z * b.Z, a.W * b.W);
        public static Int4 operator *(in Int4 a, int d)
            => new Int4(a.X * d, a.Y * d, a.Z * d, a.W * d);
        public static Int4 operator /(in Int4 a, in Int4 b)
            => new Int4(a.X / b.X, a.Y / b.Y, a.Z / b.Z, a.W / b.W);
        public static Int4 operator /(in Int4 a, int d)
            => new Int4(a.X / d, a.Y / d, a.Z / d, a.W / d);

        // TODO: should all return bool4

        public static bool operator <(in Int4 a, in Int4 b)
            => a.X < b.X && a.Y < b.Y && a.Z < b.Z && a.W < b.W;
        public static bool operator <(in Int4 a, int b)
            => a.X < b && a.Y < b && a.Z < b && a.W < b;
        public static bool operator <(int a, in Int4 b)
            => a < b.X && a < b.Y && a < b.Z && a < b.W;

        public static bool operator <=(in Int4 a, in Int4 b)
            => a.X <= b.X && a.Y <= b.Y && a.Z <= b.Z && a.W <= b.W;
        public static bool operator <=(in Int4 a, int b)
            => a.X <= b && a.Y <= b && a.Z <= b && a.W <= b;
        public static bool operator <=(int a, in Int4 b)
            => a <= b.X && a <= b.Y && a <= b.Z && a <= b.W;

        public static bool operator >(in Int4 a, in Int4 b)
            => a.X > b.X && a.Y > b.Y && a.Z > b.Z && a.W > b.W;
        public static bool operator >(in Int4 a, int b)
            => a.X > b && a.Y > b && a.Z > b && a.W > b;
        public static bool operator >(int a, in Int4 b)
            => a > b.X && a > b.Y && a > b.Z && a > b.W;

        public static bool operator >=(in Int4 a, in Int4 b)
            => a.X >= b.X && a.Y >= b.Y && a.Z >= b.Z && a.W >= b.W;
        public static bool operator >=(in Int4 a, int b)
            => a.X >= b && a.Y >= b && a.Z >= b && a.W >= b;
        public static bool operator >=(int a, in Int4 b)
            => a >= b.X && a >= b.Y && a >= b.Z && a >= b.W;
    }

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
        public static int ManhattanDistance(in Int3 a, in Int3 b) =>
            Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y) + Math.Abs(a.Z - b.Z);
        public static int ManhattanDistance(in Int4 a, in Int4 b) =>
            Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y) + Math.Abs(a.Z - b.Z) + Math.Abs(a.W - b.W);
        public static Int3 Min(in Int3 a, in Int3 b) =>
            new Int3(Math.Min(a.X, b.X), Math.Min(a.Y, b.Y), Math.Min(a.Z, b.Z));
        public static Int4 Min(in Int4 a, in Int4 b) =>
            new Int4(Math.Min(a.X, b.X), Math.Min(a.Y, b.Y), Math.Min(a.Z, b.Z), Math.Min(a.W, b.W));
        public static Int3 Max(in Int3 a, in Int3 b) =>
            new Int3(Math.Max(a.X, b.X), Math.Max(a.Y, b.Y), Math.Max(a.Z, b.Z));
        public static Int4 Max(in Int4 a, in Int4 b) =>
            new Int4(Math.Max(a.X, b.X), Math.Max(a.Y, b.Y), Math.Max(a.Z, b.Z), Math.Max(a.W, b.W));
        public static void Minimize(ref Int3 a, in Int3 b)
            => a = Min(a, in b);
        public static void Minimize(ref Int4 a, in Int4 b)
            => a = Min(a, in b);
        public static void Maximize(ref Int3 a, in Int3 b)
            => a = Max(a, in b);
        public static void Maximize(ref Int4 a, in Int4 b)
            => a = Max(a, in b);
        public static Int3 Midpoint(in Int3 a, in Int3 b)
            => new Int3(a.X + (b.X - a.X) / 2, a.Y + (b.Y - a.Y) / 2, a.Z + (b.Z - a.Z) / 2);
        public static Int4 Midpoint(in Int4 a, in Int4 b)
            => new Int4(a.X + (b.X - a.X) / 2, a.Y + (b.Y - a.Y) / 2, a.Z + (b.Z - a.Z) / 2, a.W + (b.W - a.W) / 2);

        public static AABB? IntersectAll([NotNull] IEnumerable<AABB> aabbs)
        {
            var intersection = new AABB(new Int3(int.MinValue), new Int3(int.MaxValue));
            return aabbs.All(i => intersection.Intersect(in i)) ? intersection : (AABB?)null;
        }
    }

    public static partial class Extensions
    {
        public static int ManhattanDistance(this in Int3 a, in Int3 b) => Utils.ManhattanDistance(in a, in b);
        public static int ManhattanDistance(this in Int4 a, in Int4 b) => Utils.ManhattanDistance(in a, in b);
        public static Int3 Min(this in Int3 a, in Int3 b) => Utils.Min(in a, in b);
        public static Int4 Min(this in Int4 a, in Int4 b) => Utils.Min(in a, in b);
        public static Int3 Max(this in Int3 a, in Int3 b) => Utils.Max(in a, in b);
        public static Int4 Max(this in Int4 a, in Int4 b) => Utils.Max(in a, in b);
        public static void Minimize(ref this Int3 a, in Int3 b) => Utils.Minimize(ref a, in b);
        public static void Minimize(ref this Int4 a, in Int4 b) => Utils.Minimize(ref a, in b);
        public static void Maximize(ref this Int3 a, in Int3 b) => Utils.Maximize(ref a, in b);
        public static void Maximize(ref this Int4 a, in Int4 b) => Utils.Maximize(ref a, in b);
        public static Int3 Midpoint(this in Int3 a, in Int3 b) => Utils.Midpoint(in a, in b);
        public static Int4 Midpoint(this in Int4 a, in Int4 b) => Utils.Midpoint(in a, in b);

        public static AABB CalcAABB([NotNull] this IEnumerable<Int3> @this) => AABB.FromPoints(@this);
        public static AABB CalcAABB([NotNull] this IEnumerable<AABB> @this) => AABB.FromAABBs(@this);
        public static AABB? IntersectAll([NotNull] this IEnumerable<AABB> @this) => Utils.IntersectAll(@this);
    }
}

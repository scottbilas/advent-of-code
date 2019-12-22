using System;
using System.Collections.Generic;

// ReSharper disable IdentifierTypo
// ReSharper disable NonReadonlyMemberInGetHashCode
// ReSharper disable ShiftExpressionRealShiftCountIsZero
// ReSharper disable UnusedMember.Global

namespace Aoc2019
{
    public struct Int2 : IEquatable<Int2>
    {
        public int X, Y;

        public Int2(int x, int y) =>
            (X, Y) = (x, y);
        public Int2(int v) =>
            (X, Y) = (v, v);
        public Int2(IEnumerable<int> xy) =>
            (X, Y) = xy.First2();
        public Int2(ValueTuple<int, int> xy) =>
            (X, Y) = xy;

        public unsafe int this[int index]
        {
            get
            {
                if (index < 0 || index > 1)
                    throw new ArgumentOutOfRangeException(nameof(index));
                fixed (int* i = &X) { return i[index]; }
            }
            set
            {
                if (index < 0 || index > 1)
                    throw new ArgumentOutOfRangeException(nameof(index));
                fixed (int* i = &X) { i[index] = value; }
            }
        }

        public void Deconstruct(out int x, out int y) =>
            (x, y) = (X, Y);

        public bool Equals(Int2 other) =>
            X == other.X && Y == other.Y;

        public override bool Equals(object obj) =>
            !ReferenceEquals(obj, null) && obj is Int2 other && Equals(other);

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = X;
                hashCode = (hashCode * 397) ^ Y;
                return hashCode;
            }
        }

        public static Bool2 operator ==(in Int2 left, in Int2 right) =>
            new Bool2(left.X == right.X, left.Y == right.Y);
        public static Bool2 operator !=(in Int2 left, in Int2 right) =>
            new Bool2(left.X != right.X, left.Y != right.Y);

        public override string ToString() =>
            $"{X}, {Y}";

        static readonly Int2 k_Zero = new Int2(0), k_One = new Int2(1);
        static readonly Int2 k_MaxValue = new Int2(Int32.MaxValue, Int32.MaxValue);
        static readonly Int2 k_MinValue = new Int2(Int32.MinValue, Int32.MinValue);

        public static ref readonly Int2 Zero => ref k_Zero;
        public static ref readonly Int2 One => ref k_One;
        public static ref readonly Int2 MaxValue => ref k_MaxValue;
        public static ref readonly Int2 MinValue => ref k_MinValue;

        public bool IsZero => Equals(Zero);
        public bool IsOne  => Equals(One);

        public static Int2 operator-(in Int2 i) =>
            new Int2(-i.X, -i.Y);

        public static Int2 operator +(in Int2 a, in Int2 b) =>
            new Int2(a.X + b.X, a.Y + b.Y);
        public static Int2 operator +(in Int2 a, int d) =>
            new Int2(a.X + d, a.Y + d);
        public static Int2 operator -(in Int2 a, in Int2 b) =>
            new Int2(a.X - b.X, a.Y - b.Y);
        public static Int2 operator -(in Int2 a, int d) =>
            new Int2(a.X - d, a.Y - d);
        public static Int2 operator *(in Int2 a, in Int2 b) =>
            new Int2(a.X * b.X, a.Y * b.Y);
        public static Int2 operator *(in Int2 a, int d) =>
            new Int2(a.X * d, a.Y * d);
        public static Int2 operator /(in Int2 a, in Int2 b) =>
            new Int2(a.X / b.X, a.Y / b.Y);
        public static Int2 operator /(in Int2 a, int d) =>
            new Int2(a.X / d, a.Y / d);

        public static Bool2 operator <(in Int2 a, in Int2 b) =>
            new Bool2(a.X < b.X, a.Y < b.Y);
        public static Bool2 operator <(in Int2 a, int b) =>
            new Bool2(a.X < b, a.Y < b);
        public static Bool2 operator <(int a, in Int2 b) =>
            new Bool2(a < b.X, a < b.Y);

        public static Bool2 operator <=(in Int2 a, in Int2 b) =>
            new Bool2(a.X <= b.X, a.Y <= b.Y);
        public static Bool2 operator <=(in Int2 a, int b) =>
            new Bool2(a.X <= b, a.Y <= b);
        public static Bool2 operator <=(int a, in Int2 b) =>
            new Bool2(a <= b.X, a <= b.Y);

        public static Bool2 operator >(in Int2 a, in Int2 b) =>
            new Bool2(a.X > b.X, a.Y > b.Y);
        public static Bool2 operator >(in Int2 a, int b) =>
            new Bool2(a.X > b, a.Y > b);
        public static Bool2 operator >(int a, in Int2 b) =>
            new Bool2(a > b.X, a > b.Y);

        public static Bool2 operator >=(in Int2 a, in Int2 b) =>
            new Bool2(a.X >= b.X, a.Y >= b.Y);
        public static Bool2 operator >=(in Int2 a, int b) =>
            new Bool2(a.X >= b, a.Y >= b);
        public static Bool2 operator >=(int a, in Int2 b) =>
            new Bool2(a >= b.X, a >= b.Y);
    }

    public struct Int3 : IEquatable<Int3>
    {
        public int X, Y, Z;

        public Int3(int x, int y, int z) =>
            (X, Y, Z) = (x, y, z);
        public Int3(int v) =>
            (X, Y, Z) = (v, v, v);
        public Int3(IEnumerable<int> xyz) =>
            (X, Y, Z) = xyz.First3();
        public Int3(ValueTuple<int, int, int> xyz) =>
            (X, Y, Z) = xyz;

        public unsafe int this[int index]
        {
            get
            {
                if (index < 0 || index > 2)
                    throw new ArgumentOutOfRangeException(nameof(index));
                fixed (int* i = &X) { return i[index]; }
            }
            set
            {
                if (index < 0 || index > 2)
                    throw new ArgumentOutOfRangeException(nameof(index));
                fixed (int* i = &X) { i[index] = value; }
            }
        }

        public void Deconstruct(out int x, out int y, out int z) =>
            (x, y, z) = (X, Y, Z);

        public bool Equals(Int3 other) =>
            X == other.X && Y == other.Y && Z == other.Z;

        public override bool Equals(object obj) =>
            !ReferenceEquals(obj, null) && obj is Int3 other && Equals(other);

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

        public static Bool3 operator ==(in Int3 left, in Int3 right) =>
            new Bool3(left.X == right.X, left.Y == right.Y, left.Z == right.Z);
        public static Bool3 operator !=(in Int3 left, in Int3 right) =>
            new Bool3(left.X != right.X, left.Y != right.Y, left.Z != right.Z);

        public override string ToString() =>
            $"{X}, {Y}, {Z}";

        static readonly Int3 k_Zero = new Int3(0), k_One = new Int3(1);
        static readonly Int3 k_MaxValue = new Int3(Int32.MaxValue, Int32.MaxValue, Int32.MaxValue);
        static readonly Int3 k_MinValue = new Int3(Int32.MinValue, Int32.MinValue, Int32.MinValue);

        public static ref readonly Int3 Zero => ref k_Zero;
        public static ref readonly Int3 One => ref k_One;
        public static ref readonly Int3 MaxValue => ref k_MaxValue;
        public static ref readonly Int3 MinValue => ref k_MinValue;

        public bool IsZero => Equals(Zero);
        public bool IsOne  => Equals(One);

        public static Int3 operator-(in Int3 i) =>
            new Int3(-i.X, -i.Y, -i.Z);

        public static Int3 operator +(in Int3 a, in Int3 b) =>
            new Int3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        public static Int3 operator +(in Int3 a, int d) =>
            new Int3(a.X + d, a.Y + d, a.Z + d);
        public static Int3 operator -(in Int3 a, in Int3 b) =>
            new Int3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        public static Int3 operator -(in Int3 a, int d) =>
            new Int3(a.X - d, a.Y - d, a.Z - d);
        public static Int3 operator *(in Int3 a, in Int3 b) =>
            new Int3(a.X * b.X, a.Y * b.Y, a.Z * b.Z);
        public static Int3 operator *(in Int3 a, int d) =>
            new Int3(a.X * d, a.Y * d, a.Z * d);
        public static Int3 operator /(in Int3 a, in Int3 b) =>
            new Int3(a.X / b.X, a.Y / b.Y, a.Z / b.Z);
        public static Int3 operator /(in Int3 a, int d) =>
            new Int3(a.X / d, a.Y / d, a.Z / d);

        public static Bool3 operator <(in Int3 a, in Int3 b) =>
            new Bool3(a.X < b.X, a.Y < b.Y, a.Z < b.Z);
        public static Bool3 operator <(in Int3 a, int b) =>
            new Bool3(a.X < b, a.Y < b, a.Z < b);
        public static Bool3 operator <(int a, in Int3 b) =>
            new Bool3(a < b.X, a < b.Y, a < b.Z);

        public static Bool3 operator <=(in Int3 a, in Int3 b) =>
            new Bool3(a.X <= b.X, a.Y <= b.Y, a.Z <= b.Z);
        public static Bool3 operator <=(in Int3 a, int b) =>
            new Bool3(a.X <= b, a.Y <= b, a.Z <= b);
        public static Bool3 operator <=(int a, in Int3 b) =>
            new Bool3(a <= b.X, a <= b.Y, a <= b.Z);

        public static Bool3 operator >(in Int3 a, in Int3 b) =>
            new Bool3(a.X > b.X, a.Y > b.Y, a.Z > b.Z);
        public static Bool3 operator >(in Int3 a, int b) =>
            new Bool3(a.X > b, a.Y > b, a.Z > b);
        public static Bool3 operator >(int a, in Int3 b) =>
            new Bool3(a > b.X, a > b.Y, a > b.Z);

        public static Bool3 operator >=(in Int3 a, in Int3 b) =>
            new Bool3(a.X >= b.X, a.Y >= b.Y, a.Z >= b.Z);
        public static Bool3 operator >=(in Int3 a, int b) =>
            new Bool3(a.X >= b, a.Y >= b, a.Z >= b);
        public static Bool3 operator >=(int a, in Int3 b) =>
            new Bool3(a >= b.X, a >= b.Y, a >= b.Z);
    }

    public struct Int4 : IEquatable<Int4>
    {
        public int X, Y, Z, W;

        public Int4(int x, int y, int z, int w) =>
            (X, Y, Z, W) = (x, y, z, w);
        public Int4(int v) =>
            (X, Y, Z, W) = (v, v, v, v);
        public Int4(IEnumerable<int> xyzw) =>
            (X, Y, Z, W) = xyzw.First4();
        public Int4(ValueTuple<int, int, int, int> xyzw) =>
            (X, Y, Z, W) = xyzw;

        public unsafe int this[int index]
        {
            get
            {
                if (index < 0 || index > 3)
                    throw new ArgumentOutOfRangeException(nameof(index));
                fixed (int* i = &X) { return i[index]; }
            }
            set
            {
                if (index < 0 || index > 3)
                    throw new ArgumentOutOfRangeException(nameof(index));
                fixed (int* i = &X) { i[index] = value; }
            }
        }

        public void Deconstruct(out int x, out int y, out int z, out int w) =>
            (x, y, z, w) = (X, Y, Z, W);

        public bool Equals(Int4 other) =>
            X == other.X && Y == other.Y && Z == other.Z && W == other.W;

        public override bool Equals(object obj) =>
            !ReferenceEquals(obj, null) && obj is Int4 other && Equals(other);

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

        public static Bool4 operator ==(in Int4 left, in Int4 right) =>
            new Bool4(left.X == right.X, left.Y == right.Y, left.Z == right.Z, left.W == right.W);
        public static Bool4 operator !=(in Int4 left, in Int4 right) =>
            new Bool4(left.X != right.X, left.Y != right.Y, left.Z != right.Z, left.W != right.W);

        public override string ToString() =>
            $"{X}, {Y}, {Z}, {W}";

        static readonly Int4 k_Zero = new Int4(0), k_One = new Int4(1);
        static readonly Int4 k_MaxValue = new Int4(Int32.MaxValue, Int32.MaxValue, Int32.MaxValue, Int32.MaxValue);
        static readonly Int4 k_MinValue = new Int4(Int32.MinValue, Int32.MinValue, Int32.MinValue, Int32.MinValue);

        public static ref readonly Int4 Zero => ref k_Zero;
        public static ref readonly Int4 One => ref k_One;
        public static ref readonly Int4 MaxValue => ref k_MaxValue;
        public static ref readonly Int4 MinValue => ref k_MinValue;

        public bool IsZero => Equals(Zero);
        public bool IsOne  => Equals(One);

        public static Int4 operator-(in Int4 i) =>
            new Int4(-i.X, -i.Y, -i.Z, -i.W);

        public static Int4 operator +(in Int4 a, in Int4 b) =>
            new Int4(a.X + b.X, a.Y + b.Y, a.Z + b.Z, a.W + b.W);
        public static Int4 operator +(in Int4 a, int d) =>
            new Int4(a.X + d, a.Y + d, a.Z + d, a.W + d);
        public static Int4 operator -(in Int4 a, in Int4 b) =>
            new Int4(a.X - b.X, a.Y - b.Y, a.Z - b.Z, a.W - b.W);
        public static Int4 operator -(in Int4 a, int d) =>
            new Int4(a.X - d, a.Y - d, a.Z - d, a.W - d);
        public static Int4 operator *(in Int4 a, in Int4 b) =>
            new Int4(a.X * b.X, a.Y * b.Y, a.Z * b.Z, a.W * b.W);
        public static Int4 operator *(in Int4 a, int d) =>
            new Int4(a.X * d, a.Y * d, a.Z * d, a.W * d);
        public static Int4 operator /(in Int4 a, in Int4 b) =>
            new Int4(a.X / b.X, a.Y / b.Y, a.Z / b.Z, a.W / b.W);
        public static Int4 operator /(in Int4 a, int d) =>
            new Int4(a.X / d, a.Y / d, a.Z / d, a.W / d);

        public static Bool4 operator <(in Int4 a, in Int4 b) =>
            new Bool4(a.X < b.X, a.Y < b.Y, a.Z < b.Z, a.W < b.W);
        public static Bool4 operator <(in Int4 a, int b) =>
            new Bool4(a.X < b, a.Y < b, a.Z < b, a.W < b);
        public static Bool4 operator <(int a, in Int4 b) =>
            new Bool4(a < b.X, a < b.Y, a < b.Z, a < b.W);

        public static Bool4 operator <=(in Int4 a, in Int4 b) =>
            new Bool4(a.X <= b.X, a.Y <= b.Y, a.Z <= b.Z, a.W <= b.W);
        public static Bool4 operator <=(in Int4 a, int b) =>
            new Bool4(a.X <= b, a.Y <= b, a.Z <= b, a.W <= b);
        public static Bool4 operator <=(int a, in Int4 b) =>
            new Bool4(a <= b.X, a <= b.Y, a <= b.Z, a <= b.W);

        public static Bool4 operator >(in Int4 a, in Int4 b) =>
            new Bool4(a.X > b.X, a.Y > b.Y, a.Z > b.Z, a.W > b.W);
        public static Bool4 operator >(in Int4 a, int b) =>
            new Bool4(a.X > b, a.Y > b, a.Z > b, a.W > b);
        public static Bool4 operator >(int a, in Int4 b) =>
            new Bool4(a > b.X, a > b.Y, a > b.Z, a > b.W);

        public static Bool4 operator >=(in Int4 a, in Int4 b) =>
            new Bool4(a.X >= b.X, a.Y >= b.Y, a.Z >= b.Z, a.W >= b.W);
        public static Bool4 operator >=(in Int4 a, int b) =>
            new Bool4(a.X >= b, a.Y >= b, a.Z >= b, a.W >= b);
        public static Bool4 operator >=(int a, in Int4 b) =>
            new Bool4(a >= b.X, a >= b.Y, a >= b.Z, a >= b.W);
    }

    public struct Bool2 : IEquatable<Bool2>
    {
        public bool X, Y;

        public Bool2(bool x, bool y) =>
            (X, Y) = (x, y);
        public Bool2(bool v) =>
            (X, Y) = (v, v);
        public Bool2(IEnumerable<bool> xy) =>
            (X, Y) = xy.First2();
        public Bool2(ValueTuple<bool, bool> xy) =>
            (X, Y) = xy;

        public unsafe bool this[int index]
        {
            get
            {
                if (index < 0 || index > 1)
                    throw new ArgumentOutOfRangeException(nameof(index));
                fixed (bool* i = &X) { return i[index]; }
            }
            set
            {
                if (index < 0 || index > 1)
                    throw new ArgumentOutOfRangeException(nameof(index));
                fixed (bool* i = &X) { i[index] = value; }
            }
        }

        public bool Equals(Bool2 other) =>
            X == other.X && Y == other.Y;

        public override bool Equals(object obj) =>
            !ReferenceEquals(obj, null) && obj is Bool2 other && Equals(other);

        public override int GetHashCode() =>
            (X?1:0) << 0 | (Y?1:0) << 1;

        public static bool operator ==(in Bool2 left, in Bool2 right) => left.Equals(right);
        public static bool operator !=(in Bool2 left, in Bool2 right) => !left.Equals(right);

        public override string ToString() => $"{X}, {Y}";
        public object ToDump() => ToString(); // linqpad

        static readonly Bool2 k_False = new Bool2(false), k_True = new Bool2(true);

        public static ref readonly Bool2 False => ref k_False;
        public static ref readonly Bool2 True => ref k_True;

        public bool All() => X && Y;
        public bool Any() => X || Y;
    }

    public struct Bool3 : IEquatable<Bool3>
    {
        public bool X, Y, Z;

        public Bool3(bool x, bool y, bool z) =>
            (X, Y, Z) = (x, y, z);
        public Bool3(bool v) =>
            (X, Y, Z) = (v, v, v);
        public Bool3(IEnumerable<bool> xyz) =>
            (X, Y, Z) = xyz.First3();
        public Bool3(ValueTuple<bool, bool, bool> xyz) =>
            (X, Y, Z) = xyz;

        public unsafe bool this[int index]
        {
            get
            {
                if (index < 0 || index > 2)
                    throw new ArgumentOutOfRangeException(nameof(index));
                fixed (bool* i = &X) { return i[index]; }
            }
            set
            {
                if (index < 0 || index > 2)
                    throw new ArgumentOutOfRangeException(nameof(index));
                fixed (bool* i = &X) { i[index] = value; }
            }
        }

        public bool Equals(Bool3 other) =>
            X == other.X && Y == other.Y && Z == other.Z;

        public override bool Equals(object obj) =>
            !ReferenceEquals(obj, null) && obj is Bool3 other && Equals(other);

        public override int GetHashCode() =>
            (X?1:0) << 0 | (Y?1:0) << 1 | (Z?1:0) << 2;

        public static bool operator ==(in Bool3 left, in Bool3 right) => left.Equals(right);
        public static bool operator !=(in Bool3 left, in Bool3 right) => !left.Equals(right);

        public override string ToString() => $"{X}, {Y}, {Z}";
        public object ToDump() => ToString(); // linqpad

        static readonly Bool3 k_False = new Bool3(false), k_True = new Bool3(true);

        public static ref readonly Bool3 False => ref k_False;
        public static ref readonly Bool3 True => ref k_True;

        public bool All() => X && Y && Z;
        public bool Any() => X || Y || Z;
    }

    public struct Bool4 : IEquatable<Bool4>
    {
        public bool X, Y, Z, W;

        public Bool4(bool x, bool y, bool z, bool w) =>
            (X, Y, Z, W) = (x, y, z, w);
        public Bool4(bool v) =>
            (X, Y, Z, W) = (v, v, v, v);
        public Bool4(IEnumerable<bool> xyzw) =>
            (X, Y, Z, W) = xyzw.First4();
        public Bool4(ValueTuple<bool, bool, bool, bool> xyzw) =>
            (X, Y, Z, W) = xyzw;

        public unsafe bool this[int index]
        {
            get
            {
                if (index < 0 || index > 3)
                    throw new ArgumentOutOfRangeException(nameof(index));
                fixed (bool* i = &X) { return i[index]; }
            }
            set
            {
                if (index < 0 || index > 3)
                    throw new ArgumentOutOfRangeException(nameof(index));
                fixed (bool* i = &X) { i[index] = value; }
            }
        }

        public bool Equals(Bool4 other) =>
            X == other.X && Y == other.Y && Z == other.Z && W == other.W;

        public override bool Equals(object obj) =>
            !ReferenceEquals(obj, null) && obj is Bool4 other && Equals(other);

        public override int GetHashCode() =>
            (X?1:0) << 0 | (Y?1:0) << 1 | (Z?1:0) << 2 | (W?1:0) << 3;

        public static bool operator ==(in Bool4 left, in Bool4 right) => left.Equals(right);
        public static bool operator !=(in Bool4 left, in Bool4 right) => !left.Equals(right);

        public override string ToString() => $"{X}, {Y}, {Z}, {W}";
        public object ToDump() => ToString(); // linqpad

        static readonly Bool4 k_False = new Bool4(false), k_True = new Bool4(true);

        public static ref readonly Bool4 False => ref k_False;
        public static ref readonly Bool4 True => ref k_True;

        public bool All() => X && Y && Z && W;
        public bool Any() => X || Y || Z || W;
    }

    public static partial class Utils
    {
        public static Int2 Abs(in Int2 a) =>
            new Int2(Math.Abs(a.X), Math.Abs(a.Y));
        public static Int3 Abs(in Int3 a) =>
            new Int3(Math.Abs(a.X), Math.Abs(a.Y), Math.Abs(a.Z));
        public static Int4 Abs(in Int4 a) =>
            new Int4(Math.Abs(a.X), Math.Abs(a.Y), Math.Abs(a.Z), Math.Abs(a.W));

        public static int LengthSq(in Int2 a) =>
             a.X * a.X + a.Y * a.Y;
        public static int LengthSq(in Int2 a, in Int2 b) =>
             (a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y);
        public static int LengthSq(in Int3 a) =>
             a.X * a.X + a.Y * a.Y + a.Z * a.Z;
        public static int LengthSq(in Int3 a, in Int3 b) =>
             (a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y) + (a.Z - b.Z) * (a.Z - b.Z);
        public static int LengthSq(in Int4 a) =>
             a.X * a.X + a.Y * a.Y + a.Z * a.Z + a.W * a.W;
        public static int LengthSq(in Int4 a, in Int4 b) =>
             (a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y) + (a.Z - b.Z) * (a.Z - b.Z) + (a.W - b.W) * (a.W - b.W);

        public static int ManhattanDistance(in Int2 a) =>
            Math.Abs(a.X) + Math.Abs(a.Y);
        public static int ManhattanDistance(in Int2 a, in Int2 b) =>
            Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
        public static int ManhattanDistance(in Int3 a) =>
            Math.Abs(a.X) + Math.Abs(a.Y) + Math.Abs(a.Z);
        public static int ManhattanDistance(in Int3 a, in Int3 b) =>
            Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y) + Math.Abs(a.Z - b.Z);
        public static int ManhattanDistance(in Int4 a) =>
            Math.Abs(a.X) + Math.Abs(a.Y) + Math.Abs(a.Z) + Math.Abs(a.W);
        public static int ManhattanDistance(in Int4 a, in Int4 b) =>
            Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y) + Math.Abs(a.Z - b.Z) + Math.Abs(a.W - b.W);

        public static Int2 Min(in Int2 a, in Int2 b) =>
            new Int2(Math.Min(a.X, b.X), Math.Min(a.Y, b.Y));
        public static Int2 Max(in Int2 a, in Int2 b) =>
            new Int2(Math.Max(a.X, b.X), Math.Max(a.Y, b.Y));
        public static Int3 Min(in Int3 a, in Int3 b) =>
            new Int3(Math.Min(a.X, b.X), Math.Min(a.Y, b.Y), Math.Min(a.Z, b.Z));
        public static Int3 Max(in Int3 a, in Int3 b) =>
            new Int3(Math.Max(a.X, b.X), Math.Max(a.Y, b.Y), Math.Max(a.Z, b.Z));
        public static Int4 Min(in Int4 a, in Int4 b) =>
            new Int4(Math.Min(a.X, b.X), Math.Min(a.Y, b.Y), Math.Min(a.Z, b.Z), Math.Min(a.W, b.W));
        public static Int4 Max(in Int4 a, in Int4 b) =>
            new Int4(Math.Max(a.X, b.X), Math.Max(a.Y, b.Y), Math.Max(a.Z, b.Z), Math.Max(a.W, b.W));

        public static void Minimize(ref Int2 a, in Int2 b) => a = Min(in a, in b);
        public static void Maximize(ref Int2 a, in Int2 b) => a = Max(in a, in b);
        public static void Minimize(ref Int3 a, in Int3 b) => a = Min(in a, in b);
        public static void Maximize(ref Int3 a, in Int3 b) => a = Max(in a, in b);
        public static void Minimize(ref Int4 a, in Int4 b) => a = Min(in a, in b);
        public static void Maximize(ref Int4 a, in Int4 b) => a = Max(in a, in b);

        public static Int2 Midpoint(in Int2 a, in Int2 b) =>
            new Int2(a.X + (b.X - a.X) / 2, a.Y + (b.Y - a.Y) / 2);
        public static Int3 Midpoint(in Int3 a, in Int3 b) =>
            new Int3(a.X + (b.X - a.X) / 2, a.Y + (b.Y - a.Y) / 2, a.Z + (b.Z - a.Z) / 2);
        public static Int4 Midpoint(in Int4 a, in Int4 b) =>
            new Int4(a.X + (b.X - a.X) / 2, a.Y + (b.Y - a.Y) / 2, a.Z + (b.Z - a.Z) / 2, a.W + (b.W - a.W) / 2);
    }

    public static partial class Extensions
    {
        public static Int2 Abs(this in Int2 a) => Utils.Abs(a);
        public static Int3 Abs(this in Int3 a) => Utils.Abs(a);
        public static Int4 Abs(this in Int4 a) => Utils.Abs(a);

        public static int LengthSq(this in Int2 a) => Utils.LengthSq(a);
        public static int LengthSq(this in Int2 a, in Int2 b) => Utils.LengthSq(a, b);
        public static int LengthSq(this in Int3 a) => Utils.LengthSq(a);
        public static int LengthSq(this in Int3 a, in Int3 b) => Utils.LengthSq(a, b);
        public static int LengthSq(this in Int4 a) => Utils.LengthSq(a);
        public static int LengthSq(this in Int4 a, in Int4 b) => Utils.LengthSq(a, b);

        public static int ManhattanDistance(this in Int2 a, in Int2 b) => Utils.ManhattanDistance(in a, in b);
        public static int ManhattanDistance(this in Int2 a) => Utils.ManhattanDistance(in a);
        public static int ManhattanDistance(this in Int3 a, in Int3 b) => Utils.ManhattanDistance(in a, in b);
        public static int ManhattanDistance(this in Int3 a) => Utils.ManhattanDistance(in a);
        public static int ManhattanDistance(this in Int4 a, in Int4 b) => Utils.ManhattanDistance(in a, in b);
        public static int ManhattanDistance(this in Int4 a) => Utils.ManhattanDistance(in a);

        public static Int2 Min(this in Int2 a, in Int2 b) => Utils.Min(in a, in b);
        public static Int2 Max(this in Int2 a, in Int2 b) => Utils.Max(in a, in b);
        public static Int3 Min(this in Int3 a, in Int3 b) => Utils.Min(in a, in b);
        public static Int3 Max(this in Int3 a, in Int3 b) => Utils.Max(in a, in b);
        public static Int4 Min(this in Int4 a, in Int4 b) => Utils.Min(in a, in b);
        public static Int4 Max(this in Int4 a, in Int4 b) => Utils.Max(in a, in b);

        public static void Minimize(ref this Int2 a, in Int2 b) => Utils.Minimize(ref a, in b);
        public static void Maximize(ref this Int2 a, in Int2 b) => Utils.Maximize(ref a, in b);
        public static void Minimize(ref this Int3 a, in Int3 b) => Utils.Minimize(ref a, in b);
        public static void Maximize(ref this Int3 a, in Int3 b) => Utils.Maximize(ref a, in b);
        public static void Minimize(ref this Int4 a, in Int4 b) => Utils.Minimize(ref a, in b);
        public static void Maximize(ref this Int4 a, in Int4 b) => Utils.Maximize(ref a, in b);

        public static Int2 Midpoint(this in Int2 a, in Int2 b) => Utils.Midpoint(in a, in b);
        public static Int3 Midpoint(this in Int3 a, in Int3 b) => Utils.Midpoint(in a, in b);
        public static Int4 Midpoint(this in Int4 a, in Int4 b) => Utils.Midpoint(in a, in b);
    }
}

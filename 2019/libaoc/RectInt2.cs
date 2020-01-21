using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Aoc2019
{
    public struct RectInt2
    {
        public Int2 TopLeft, BottomRight;

        public static RectInt2 FromSize(Int2 size) =>
            new RectInt2 { BottomRight = size };

        public static RectInt2 FromPosSize(Int2 pos, Int2 size) =>
            new RectInt2 { TopLeft = pos, BottomRight = pos + size };
        public static RectInt2 FromPosSize(int x, int y, int width, int height) =>
            FromPosSize(new Int2(x, y), new Int2(width, height));
        public static RectInt2 FromPosSize((int x, int y, int width, int height) ps) =>
            FromPosSize(ps.x, ps.y, ps.width, ps.height);

        public static RectInt2 FromBounds(Int2 topLeft, Int2 bottomRight) =>
            new RectInt2 { TopLeft = topLeft, BottomRight = bottomRight };
        public static RectInt2 FromBounds(int left, int top, int right, int bottom) =>
            FromBounds(new Int2(left, top), new Int2(right, bottom));
        public static RectInt2 FromBounds((int left, int top, int right, int bottom) ltrb) =>
            FromBounds(ltrb.left, ltrb.top, ltrb.right, ltrb.bottom);

        public static RectInt2 FromBoundsIn(Int2 topLeft, Int2 bottomRightIn) =>
            new RectInt2 { TopLeft = topLeft, BottomRightIn = bottomRightIn };
        public static RectInt2 FromBoundsIn(int left, int top, int rightIn, int bottomIn) =>
            FromBoundsIn(new Int2(left, top), new Int2(rightIn, bottomIn));
        public static RectInt2 FromBoundsIn((int left, int top, int rightIn, int bottomIn) ltrb) =>
            FromBoundsIn(ltrb.left, ltrb.top, ltrb.rightIn, ltrb.bottomIn);

        public void Deconstruct(out int left, out int top, out int right, out int bottom) =>
            (left, top, right, bottom) = (Left, Top, Right, Bottom);

        public int Left     => TopLeft.X;
        public int Top      => TopLeft.Y;
        public int Right    => BottomRight.X;
        public int RightIn  => BottomRight.X - 1;
        public int Bottom   => BottomRight.Y;
        public int BottomIn => BottomRight.Y - 1;

        public Int2 BottomRightIn
        {
            get => BottomRight - Int2.One;
            set => BottomRight = value + Int2.One;
        }

        public override string ToString() => $"{Left}, {Top}, {Right}, {Bottom}";
        public object ToDump() => ToString(); // linqpad
    }

    public static partial class Extensions
    {
        public static RectInt2 PosSizeToRect(this (Int2 pos, Int2 size) @this) =>
            RectInt2.FromPosSize(@this.pos, @this.size);
        public static RectInt2 SizeToRect(this Int2 @this) =>
            RectInt2.FromPosSize(Int2.Zero, @this);
        public static RectInt2 BoundsToRect(this (Int2 topLeft, Int2 bottomRight) @this) =>
            RectInt2.FromBounds(@this.topLeft, @this.bottomRight);

        public static IEnumerable<Int2> SelectCoords(this Int2 @this) =>
            RectInt2.FromSize(@this).SelectCoords();

        public static IEnumerable<Int2> SelectCoords(this RectInt2 @this)
        {
            for (var y = @this.Top; y < @this.Bottom; ++y)
                for (var x = @this.Left; x < @this.Right; ++x)
                    yield return new Int2(x, y);
        }

        // TODO: move all this stuff to a IVector or something and write algorithms against the interface using where T : struct, IVector2 etc.
        public static IEnumerable<Int2> SelectBorderCoords(this Int2 @this, Int2 border) =>
            RectInt2.FromSize(@this).SelectBorderCoords(border);
        public static IEnumerable<Int2> SelectBorderCoords(this Int2 @this, int border) =>
            RectInt2.FromSize(@this).SelectBorderCoords(new Int2(border));
        public static IEnumerable<Int2> SelectBorderCoords(this Int2 @this) =>
            @this.SelectBorderCoords(Int2.One);

        public static IEnumerable<Int2> SelectBorderCoords(this RectInt2 @this, Int2 border)
        {
            for (var b = 0; b < border.Y; ++b)
                for (var x = @this.Left; x < @this.Right; ++x)
                    yield return new Int2(x, @this.Top + b);

            for (var b = 0; b < border.X; ++b)
            {
                for (var y = @this.Top + border.Y; y < @this.Bottom - border.Y; ++y)
                {
                    yield return new Int2(@this.Left + b, y);
                    yield return new Int2(@this.Right - b - 1, y);
                }
            }

            for (var b = 0; b < border.Y; ++b)
                for (var x = @this.Left; x < @this.Right; ++x)
                    yield return new Int2(x, @this.Bottom - b - 1);
        }

        public static IEnumerable<Int2> SelectBorderCoords(this RectInt2 @this, int border) =>
            @this.SelectBorderCoords(new Int2(border));
        public static IEnumerable<Int2> SelectBorderCoords(this RectInt2 @this) =>
            @this.SelectBorderCoords(Int2.One);

        public static RectInt2 Inflate(this RectInt2 @this, int left, int top, int right, int bottom) =>
            RectInt2.FromBounds(@this.Left - left, @this.Top - top, @this.Right + right, @this.Bottom + bottom);
        public static RectInt2 Inflate(this RectInt2 @this, in RectInt2 edges) =>
            RectInt2.FromBounds(@this.Left - edges.Left, @this.Top - edges.Top, @this.Right + edges.Right, @this.Bottom + edges.Bottom);
        public static RectInt2 Inflate(this RectInt2 @this, int width, int height) =>
            @this.Inflate(width, height, width, height);
        public static RectInt2 Inflate(this RectInt2 @this, int value) =>
            @this.Inflate(value, value);

        public static RectInt2 Deflate(this RectInt2 @this, int left, int top, int right, int bottom) =>
            @this.Inflate(-left, -top, -right, -bottom);
        public static RectInt2 Deflate(this RectInt2 @this, in RectInt2 edges) =>
            @this.Deflate(edges.Left, edges.Top, edges.Right, edges.Bottom);
        public static RectInt2 Deflate(this RectInt2 @this, int width, int height) =>
            @this.Deflate(width, height, width, height);
        public static RectInt2 Deflate(this RectInt2 @this, int value) =>
            @this.Deflate(value, value);

        public static RectInt2 CalcBoundingBox([NotNull] this IEnumerable<RectInt2> @this)
        {
            var (l, t, r, b) = (int.MaxValue, int.MaxValue, int.MinValue, int.MinValue);
            foreach (var rect in @this)
            {
                l = Math.Min(l, rect.Left);
                t = Math.Min(t, rect.Top);
                r = Math.Max(r, rect.Right);
                b = Math.Max(b, rect.Bottom);
            }

            return RectInt2.FromBounds(l, t, r, b);
        }
    }
}

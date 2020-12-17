package libaoc

import "math"

type Int4 struct {
	X, Y, Z, W int
}

func MakeInt4(x, y, z, w int) Int4 {
	return Int4{x, y, z, w}
}

func MakeInt43(x, y, z int) Int4 {
	return Int4{x, y, z, 0}
}

func MakeInt42(x, y int) Int4 {
	return Int4{x, y, 0, 0}
}

func (value *Int4) Offset(other Int4) Int4 {
	return value.Offset4(other.X, other.Y, other.Z, other.W)
}

func (value *Int4) Offset4(x, y, z, w int) Int4 {
	return Int4{value.X + x, value.Y + y, value.Z + z, value.W + w}
}

type AABB4 struct {
	Min, Max Int4
}

func DefaultAABB4() AABB4 {
	return AABB4{
		Int4{math.MaxInt64, math.MaxInt64, math.MaxInt64, math.MaxInt64},
		Int4{math.MinInt64, math.MinInt64, math.MinInt64, math.MinInt64},
	}
}

func (aabb *AABB4) Encapsulate(point Int4) {
	UpdateBoundsInt(&aabb.Min.X, point.X, &aabb.Max.X)
	UpdateBoundsInt(&aabb.Min.Y, point.Y, &aabb.Max.Y)
	UpdateBoundsInt(&aabb.Min.Z, point.Z, &aabb.Max.Z)
	UpdateBoundsInt(&aabb.Min.W, point.W, &aabb.Max.W)
}

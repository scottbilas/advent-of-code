package main

import (
	"strings"
	"testing"
)

import . "github.com/ahmetalpbalkan/go-linq"
import . "scottbilas/advent-of-code/2020/libaoc"

var sample = `
    .#.
    ..#
    ###
	`

var input = ReadInputFile()

type Cells map[Int4]bool

func sim(cells Cells, level int) Cells {
	aabb := DefaultAABB4()
	for pos, cell := range cells {
		if cell {
			aabb.Encapsulate(pos)
		}
	}

	minw, maxw, mindw, maxdw := 0, 0, 0, 0
	if level == 4 {
		minw, maxw = aabb.Min.W-1, aabb.Max.W+1
		mindw, maxdw = -1, 1
	}

	next, pos := make(map[Int4]bool), Int4{}
	for pos.W = minw; pos.W <= maxw; pos.W++ {
		for pos.Z = aabb.Min.Z - 1; pos.Z <= aabb.Max.Z+1; pos.Z++ {
			for pos.Y = aabb.Min.Y - 1; pos.Y <= aabb.Max.Y+1; pos.Y++ {
				for pos.X = aabb.Min.X - 1; pos.X <= aabb.Max.X+1; pos.X++ {
					count := 0
					for dw := mindw; dw <= maxdw; dw++ {
						for dz := -1; dz <= 1; dz++ {
							for dy := -1; dy <= 1; dy++ {
								for dx := -1; dx <= 1; dx++ {
									if cells[pos.Offset4(dx, dy, dz, dw)] {
										count++
									}
								}
							}
						}
					}

					if count == 3 || (count == 4 && cells[pos]) {
						next[pos] = true
					}
				}
			}
		}
	}

	return next
}

func solve(source string, level int) int {
	cells := make(map[Int4]bool)

	for y, row := range strings.Fields(source) {
		for x, cell := range row {
			if cell == '#' {
				cells[MakeInt42(x, y)] = true
			}
		}
	}

	for i := 0; i < 6; i++ {
		cells = sim(cells, level)
	}

	return From(cells).WhereT(func(i KeyValue) bool {
		return i.Value.(bool)
	}).Count()
}

// PART 1

func Test_Part1(t *testing.T) {
	AssertEqualSub("Sample", t, func() int { return solve(sample, 3) }, 112)
	AssertEqualSub("Problem", t, solve(input, 3), 382)
}

// PART 2

func Test_Part2(t *testing.T) {
	AssertEqualSub("Sample", t, solve(sample, 4), 848)
	AssertEqualSub("Problem", t, solve(input, 4), 2552)
}

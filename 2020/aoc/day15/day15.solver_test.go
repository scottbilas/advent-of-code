package main

import (
	"testing"
)

import . "scottbilas/advent-of-code/2020/libaoc"

func solve(nums []int, cycles int) int {
	hist := make([]struct{ a, b int }, cycles+1)
	last, turn := 1, 1

	for ; turn <= len(nums); turn++ {
		last = nums[turn-1]
		hist[last].b = turn
	}

	for ; turn <= cycles; turn++ {
		if hist[last].a == 0 {
			if hist[last].b == 0 {
				hist[last].b = turn
			}
			last = 0
		} else {
			last = hist[last].b - hist[last].a
		}

		hist[last].a = hist[last].b
		hist[last].b = turn
	}

	return last
}

// PART 1

func Test_Part1(t *testing.T) {
	AssertEqualSub("Sample1-10", t, solve([]int{0, 3, 6}, 10), 0)

	AssertEqualSub("Sample1-2020", t, solve([]int{0, 3, 6}, 2020), 436)
	AssertEqualSub("Sample2-2020", t, solve([]int{1, 3, 2}, 2020), 1)
	AssertEqualSub("Sample3-2020", t, solve([]int{2, 1, 3}, 2020), 10)
	AssertEqualSub("Sample4-2020", t, solve([]int{1, 2, 3}, 2020), 27)
	AssertEqualSub("Sample5-2020", t, solve([]int{2, 3, 1}, 2020), 78)
	AssertEqualSub("Sample6-2020", t, solve([]int{3, 2, 1}, 2020), 438)
	AssertEqualSub("Sample7-2020", t, solve([]int{3, 1, 2}, 2020), 1836)

	AssertEqualSub("Problem", t, solve([]int{0, 20, 7, 16, 1, 18, 15}, 2020), 1025)
}

// PART 2

func Test_Part2(t *testing.T) {
	AssertEqualSub("Sample1-30000000", t, func() int { return solve([]int{0, 3, 6}, 30000000) }, 175594)
	AssertEqualSub("Sample2-30000000", t, func() int { return solve([]int{1, 3, 2}, 30000000) }, 2578)
	AssertEqualSub("Sample3-30000000", t, func() int { return solve([]int{2, 1, 3}, 30000000) }, 3544142)
	AssertEqualSub("Sample4-30000000", t, func() int { return solve([]int{1, 2, 3}, 30000000) }, 261214)
	AssertEqualSub("Sample5-30000000", t, func() int { return solve([]int{2, 3, 1}, 30000000) }, 6895259)
	AssertEqualSub("Sample6-30000000", t, func() int { return solve([]int{3, 2, 1}, 30000000) }, 18)
	AssertEqualSub("Sample7-30000000", t, func() int { return solve([]int{3, 1, 2}, 30000000) }, 362)

	AssertEqualSub("Problem", t, func() int { return solve([]int{0, 20, 7, 16, 1, 18, 15}, 30000000) }, 129262)
}

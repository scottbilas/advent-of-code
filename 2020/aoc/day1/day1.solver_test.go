package main

import "testing"
import . "scottbilas/advent-of-code/2020/libaoc"

var sample = []int{
	1721,
	979,
	366,
	299,
	675,
	1456}

var input = ParseInts(ReadInputFile())

// PART 1

func solve1(nums []int) int {
	for i1 := 0; ; i1++ {
		for i0 := i1 + 1; i0 < len(nums); i0++ {
			if nums[i1]+nums[i0] == 2020 {
				return nums[i1] * nums[i0]
			}
		}
	}
}

func Test_Part1(t *testing.T) {
	AssertEqualSub("Sample", t, solve1(sample), 514579)
	AssertEqualSub("Problem", t, solve1(input), 1003971)
}

// PART 2

func solve2(nums []int) int {
	for i2 := 0; ; i2++ {
		for i1 := i2 + 1; i1 < len(nums); i1++ {
			for i0 := i1 + 1; i0 < len(nums); i0++ {
				if nums[i2]+nums[i1]+nums[i0] == 2020 {
					return nums[i2] * nums[i1] * nums[i0]
				}
			}
		}
	}
}

func Test_Part2(t *testing.T) {
	AssertEqualSub("Sample", t, solve2(sample), 241861950)
	AssertEqualSub("Problem", t, solve2(input), 84035952)
}

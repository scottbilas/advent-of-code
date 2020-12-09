package main

import (
	"math"
	"testing"
)

import . "scottbilas/advent-of-code/2020/libaoc"

var sample = ParseInts(`
        35
        20
        15
        25
        47
        40
        62
        55
        65
        95
        102
        117
        150
        182
        127
        219
        299
        277
        309
        576
        `)

var input = ParseInts(ReadInputFile())

// PART 1

func solve1(nums []int, width int) int {
	for i := width; i < len(nums); i++ {
		for x := 0; x < width-1; x++ {
			for y := x + 1; y < width; y++ {
				if nums[i] == nums[i-x-1]+nums[i-y-1] {
					goto ok
				}
			}
		}

		return nums[i]
	ok:
	}
	return 0
}

func Test_Part1_Samples(t *testing.T) {

	sample2 := make([]int, 26)
	for i := 0; i < 25; i++ {
		sample2[i] = i + 1
	}

	sample2[25] = 26
	AssertEqualSub("Sample2 26 [valid]", t, solve1(sample2, 25), 0)
	sample2[25] = 49
	AssertEqualSub("Sample2 49 [valid]", t, solve1(sample2, 25), 0)
	sample2[25] = 100
	AssertEqualSub("Sample2 100 [invalid]", t, solve1(sample2, 25), 100)
	sample2[25] = 50
	AssertEqualSub("Sample2 50 [invalid]", t, solve1(sample2, 25), 50)

	sample3 := make([]int, 27)
	sample3[0] = 20
	for i := 1; i < 20; i++ {
		sample3[i] = i
	}
	for i := 20; i < 25; i++ {
		sample3[i] = i + 1
	}
	sample3[25] = 45

	sample3[26] = 26
	AssertEqualSub("Sample3 26 [valid]", t, solve1(sample3, 25), 0)
	sample3[26] = 64
	AssertEqualSub("Sample3 64 [valid]", t, solve1(sample3, 25), 0)
	sample3[26] = 65
	AssertEqualSub("Sample3 65 [invalid]", t, solve1(sample3, 25), 65)
	sample3[26] = 66
	AssertEqualSub("Sample3 66 [valid]", t, solve1(sample3, 25), 0)

	AssertEqualSub("Sample [invalid]", t, solve1(sample, 5), 127)
}

var part1 = 542529149

func Test_Part1_Problem(t *testing.T) {
	AssertEqual(t, solve1(input, 25), part1)
}

// PART 2

func solve2(nums []int, seek int) int {
	for lower, upper := 0, 1; ; {
		sum, min, max := 0, math.MaxInt64, 0
		for i := upper; i >= lower; i-- {
			num := nums[i]
			if num < min {
				min = num
			}
			if num > max {
				max = num
			}

			sum += num
			if sum == seek {
				return min + max
			}
		}

		if sum < seek {
			upper++
		} else {
			lower++
		}
	}
}

func Test_Part2(t *testing.T) {
	AssertEqualSub("Sample", t, solve2(sample, 127), 62)
	AssertEqualSub("Problem", t, solve2(input, part1), 75678618)
}

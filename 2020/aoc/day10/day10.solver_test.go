package main

import (
	"sort"
	"testing"
)

import . "scottbilas/advent-of-code/2020/libaoc"

var sample1 = parse(`
    16
    10
    15
    5
    1
    11
    7
    19
    6
    12
    4
    `)

var sample2 = parse(`
    28
    33
    18
    42
    31
    14
    46
    20
    48
    47
    24
    23
    49
    45
    19
    38
    39
    11
    1
    32
    25
    35
    8
    17
    7
    9
    4
    2
    34
    10
    3
    `)

var input = parse(ReadInputFile())

func parse(source string) []int {
	nums := append(ParseInts(source), 0)
	sort.Ints(nums)
	return append(nums, nums[len(nums)-1]+3)
}

// PART 1

func solve1(nums []int) int {
	counts := make(map[int]int)
	for i := 1; i < len(nums); i++ {
		counts[nums[i]-nums[i-1]]++
	}
	return counts[1] * counts[3]
}

func Test_Part1(t *testing.T) {
	AssertEqualSub("Sample1", t, solve1(sample1), 7*5)
	AssertEqualSub("Sample2", t, solve1(sample2), 22*10)
	AssertEqualSub("Problem", t, solve1(input), 2048)
}

// PART 2

func solve2(nums []int) int {
	combos := 1
	for i, ones := 1, 0; i < len(nums); i++ {
		if nums[i]-nums[i-1] == 1 {
			ones++
		} else {
			combos *= map[int]int{4: 7, 3: 4, 2: 2, 1: 1, 0: 1}[ones]
			ones = 0
		}
	}

	return combos
}

func Test_Part2(t *testing.T) {
	AssertEqualSub("Sample1", t, solve2(sample1), 8)
	AssertEqualSub("Sample2", t, solve2(sample2), 19208)
	AssertEqualSub("Problem", t, solve2(input), 1322306994176)
}

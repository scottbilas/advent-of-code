package main

import (
	"testing"
)

import . "scottbilas/advent-of-code/2020/libaoc"

var sample = NormalizeSample(`
	`)

var input = ReadInputFile()

// PART 1

func solve1(source string) int {
	total := -1
	return total
}

func Test_Part1(t *testing.T) {
	AssertEqualSub("Sample", t, solve1(sample), -1)
	AssertEqualSub("Problem", t, solve1(input), -1)
}

func solve2(source string) int {
	total := -1
	return total
}

func Test_Part2(t *testing.T) {
	AssertEqualSub("Sample", t, solve2(sample), -1)
	AssertEqualSub("Problem", t, solve2(input), -1)
}

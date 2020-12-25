package main

import (
	"testing"
)

import . "scottbilas/advent-of-code/2020/libaoc"

var sample = "389125467"
var input = "364289715"

// PART 1

func solve(source string, count int, rounds int) []int {
	next, last, current := make([]int, count), count-1, int(source[0]-'1')

	for _, c := range source {
		index := int(c - '1')
		next[last] = index
		last = index
	}

	if count > len(source) {
		next[last] = len(source)
		for i := len(source); i < count-1; i++ {
			next[i] = i + 1
		}
	} else {
		next[last] = current
	}

	for round := 0; round < rounds; round++ {
		pick0 := next[current]
		pick1 := next[pick0]
		pick2 := next[pick1]
		next[current] = next[pick2]

		seek := current
		for {
			seek--
			if seek < 0 {
				seek = count - 1
			}
			if seek != pick0 && seek != pick1 && seek != pick2 {
				next[seek], next[pick2] = pick0, next[seek]
				break
			}
		}

		current = next[current]
	}

	return next
}

func solve1(source string, rounds int) int {
	next, result := solve(source, len(source), rounds), 0
	for i := next[0]; i != 0; i = next[i] {
		result = result*10 + i + 1
	}
	return result
}

func Test_Part1(t *testing.T) {
	AssertEqualSub("Sample1", t, solve1(sample, 10), 92658374)
	AssertEqualSub("Sample2", t, solve1(sample, 100), 67384529)
	AssertEqualSub("Problem", t, solve1(input, 100), 98645732)
}

// PART 2

func solve2(source string, count, rounds int) int {
	next := solve(source, count, rounds)
	cup0, cup1 := next[0]+1, next[next[0]]+1
	return cup0 * cup1
}

func Test_Part2(t *testing.T) {
	AssertEqualSub("Sample", t, solve2(sample, 1000000, 10000000), 149245887792)
	AssertEqualSub("Problem", t, solve2(input, 1000000, 10000000), 689500518476)
}

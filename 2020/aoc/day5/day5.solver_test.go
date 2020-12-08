package main

import (
	"sort"
	"strings"
	"testing"
)

import . "scottbilas/advent-of-code/2020/libaoc"

var input = strings.Fields(ReadInputFile())

// PART 1

func findPos(instr string, left byte) int {
	low, high := 0, (1<<len(instr))-1
	for i := 0; i < len(instr); i++ {
		half := DivCeil(high-low, 2)
		if instr[i] == left {
			high -= half
		} else {
			low += half
		}
	}
	return low
}

func getSeatId(ticket string) int {
	return findPos(ticket[:7], 'F')*8 + findPos(ticket[7:], 'L')
}

func solve1(tickets []string) int {
	max := 0
	for _, ticket := range tickets {
		id := getSeatId(ticket)
		if id > max {
			max = id
		}
	}
	return max
}

func Test_Part1(t *testing.T) {
	t.Run("Sample", func(t *testing.T) {
		AssertEqual(t, getSeatId("FBFBBFFRLR"), 357)
		AssertEqual(t, getSeatId("BFFFBBFRRR"), 567)
		AssertEqual(t, getSeatId("FFFBBBFRRR"), 119)
		AssertEqual(t, getSeatId("BBFFBBFRLL"), 820)
	})

	AssertEqualSub("Problem", t, solve1(input), 832)
}

// PART 2

func solve2(tickets []string) int {
	ids := make([]int, 0)
	for _, ticket := range tickets {
		ids = append(ids, getSeatId(ticket))
	}
	sort.Ints(ids)
	for i := 1; i < len(ids); i++ {
		if ids[i-1] == ids[i]-2 {
			return ids[i] - 1
		}
	}

	panic("not found")
}

func Test_Part2(t *testing.T) {
	AssertEqualSub("Problem", t, solve2(input), 517)
}

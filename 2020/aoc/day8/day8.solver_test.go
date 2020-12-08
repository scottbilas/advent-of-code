package main

import (
	"strings"
	"testing"
)

import . "scottbilas/advent-of-code/2020/libaoc"

var sample = strings.Fields(`
    nop +0
    acc +1
    jmp +4
    acc +3
    jmp -3
    acc -99
    acc +1
    jmp -4
    acc +6
	`)

var input = strings.Fields(ReadInputFile())

func sim(source []string) (acc int, ok bool) {
	acc, inf, seen := 0, false, make(map[int]bool)
	for ip := 0; ip < len(source); ip += 2 {
		if _, inf = seen[ip]; inf {
			break
		}
		seen[ip] = true

		op, off := source[ip], ParseInt(source[ip+1])
		if op == "acc" {
			acc += off
		} else if op == "jmp" {
			ip += (off - 1) * 2
		}
	}

	return acc, !inf
}

// PART 1

func solve1(source []string) int {
	acc, _ := sim(source)
	return acc
}

func Test_Part1(t *testing.T) {
	AssertEqualSub("Sample", t, solve1(sample), 5)
	AssertEqualSub("Problem", t, solve1(input), 1262)
}

// PART 2

func solve2(source []string) int {
	for i := 0; ; i += 2 {
		op := source[i]
		if op == "acc" {
			continue
		}

		source[i] = map[string]string{"nop": "jmp", "jmp": "nop"}[op]

		acc, ok := sim(source)
		if ok {
			return acc
		}

		source[i] = op
	}
}

func Test_Part2(t *testing.T) {
	AssertEqualSub("Sample", t, solve2(sample), 8)
	AssertEqualSub("Problem", t, solve2(input), 1643)
}

package main

import (
	"strings"
	"testing"
)

import . "scottbilas/advent-of-code/2020/libaoc"

var sample = parseRules(`
    939
    7,13,x,x,59,x,31,19
	`)

type Rules struct {
	timestamp int
	ids       []int
}

func parseRules(source string) Rules {
	lines := ParseLines(source)
	return Rules{ParseInt(lines[0]), parseIds(lines[1])}
}

func parseIds(line string) []int {
	fields := strings.Split(line, ",")
	ids := make([]int, len(fields))
	for i, field := range fields {
		if field != "x" {
			ids[i] = ParseInt(field)
		}
	}
	return ids
}

// PART 1

func solve1(rules Rules) int {
	min, found := 1000, 0
	for _, id := range rules.ids {
		if id != 0 {
			if dt := id - rules.timestamp%id; id != 0 && dt < min {
				min, found = dt, id
			}
		}
	}
	return min * found
}

func Test_Part1(t *testing.T) {
	AssertEqualSub("Sample", t, solve1(sample), 295)
	AssertEqualSub("Problem", t, solve1(parseRules(ReadInputFile())), 8063)
}

// PART 2

type Target struct {
	id     int
	offset int
}

func solve2(ids []int) int {
	targets, offset := []Target{}, 0
	for _, id := range ids {
		if id != 0 {
			targets = append(targets, Target{id, offset % id})
		}
		offset++
	}

	base, mult := ids[0], ids[0]
	for _, target := range targets[1:] {
		for i := 1; ; i++ {
			if target.id-(base+mult*i)%target.id == target.offset {
				base, mult = base+mult*i, mult*target.id
				break
			}
		}
	}

	return base
}

func Test_Part2(t *testing.T) {
	AssertEqualSub("Micro1", t, solve2(parseIds("17,x,13,19")), 3417)
	AssertEqualSub("Micro2", t, solve2(parseIds("67,7,59,61")), 754018)
	AssertEqualSub("Micro3", t, solve2(parseIds("67,x,7,59,61")), 779210)
	AssertEqualSub("Micro4", t, solve2(parseIds("67,7,x,59,61")), 1261476)
	AssertEqualSub("Micro5", t, solve2(parseIds("1789,37,47,1889")), 1202161486)

	AssertEqualSub("Sample", t, solve2(sample.ids), 1068781)
	AssertEqualSub("Problem", t, solve2(parseRules(ReadInputFile()).ids), 775230782877242)
}

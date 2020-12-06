package main

import (
	"regexp"
	"strings"
	"testing"
)

import . "scottbilas/advent-of-code/2020/libaoc"

var sample = parse(NormalizeSample(`
	1-3 a: abcde
	1-3 b: cdefg
	2-9 c: ccccccccc
	`))

var input = parse(ReadInputFile())

type line struct {
	p0    int
	p1    int
	match string
	pwd   string
}

func parse(source string) []line {
	matches := regexp.
		MustCompile(`(\d+)-(\d+) ([a-z]): ([a-z]+)`).
		FindAllStringSubmatch(source, -1)

	lines := make([]line, len(matches))
	for i, item := range matches {
		lines[i] = line{ParseInt(item[1]), ParseInt(item[2]), item[3], item[4]}
	}

	return lines
}

// PART 1

func solve1(lines []line) int {
	valid := 0
	for _, line := range lines {
		found := strings.Count(line.pwd, line.match)
		valid += ToInt(found >= line.p0 && found <= line.p1)
	}
	return valid
}

func Test_Part1(t *testing.T) {
	AssertEqualSub("Sample", t, solve1(sample), 2)
	AssertEqualSub("Problem", t, solve1(input), 628)
}

// PART 2

func solve2(lines []line) int {
	valid := 0
	for _, line := range lines {
		valid += ToInt((line.pwd[line.p0-1] == line.match[0]) != (line.pwd[line.p1-1] == line.match[0]))
	}
	return valid
}

func Test_Part2(t *testing.T) {
	AssertEqualSub("Sample", t, solve2(sample), 1)
	AssertEqualSub("Problem", t, solve2(input), 705)
}

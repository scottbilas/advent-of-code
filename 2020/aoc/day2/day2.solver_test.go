package main

import (
	"io/ioutil"
	"regexp"
	"strconv"
	"strings"
	"testing"
)

var sample = `
	1-3 a: abcde
	1-3 b: cdefg
	2-9 c: ccccccccc
	`

var file, _ = ioutil.ReadFile("day2.input.txt")
var input = string(file)

type line struct {
	p0    int
	p1    int
	match string
	pwd   string
}

func parse(source string) []line {
	matches := regexp.MustCompile(`(\d+)-(\d+) ([a-z]): ([a-z]+)`).FindAllStringSubmatch(source, -1)
	lines := make([]line, len(matches))
	for i, item := range matches {
		p0, _ := strconv.Atoi(item[1])
		p1, _ := strconv.Atoi(item[2])
		lines[i] = line{p0, p1, item[3], item[4]}
	}

	return lines
}

// PART 1

func solve1(input string) int {
	valid := 0
	for _, line := range parse(input) {
		found := strings.Count(line.pwd, line.match)
		if found >= line.p0 && found <= line.p1 {
			valid++
		}
	}
	return valid
}

// samples

func TestSample1(t *testing.T) {
	got, expected := solve1(sample), 2
	if got != expected {
		t.Error("Expected", expected, "got", got)
	}
}

// problem

func TestSolve1(t *testing.T) {
	got, expected := solve1(input), 628
	if got != expected {
		t.Error("Expected", expected, "got", got)
	}
}

// PART 2

func solve2(input string) int {
	valid := 0
	for _, line := range parse(input) {
		if (line.pwd[line.p0-1] == line.match[0]) != (line.pwd[line.p1-1] == line.match[0]) {
			valid++
		}
	}
	return valid
}

// samples

func TestSample2(t *testing.T) {
	got, expected := solve2(sample), 1
	if got != expected {
		t.Error("Expected", expected, "got", got)
	}
}

// problem

func TestSolve2(t *testing.T) {
	got, expected := solve2(input), 705
	if got != expected {
		t.Error("Expected", expected, "got", got)
	}
}

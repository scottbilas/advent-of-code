package main

import (
	"io/ioutil"
	"strings"
	"testing"
)

var sample = `
	..##.......
	#...#...#..
	.#....#..#.
	..#.#...#.#
	.#...##..#.
	..#.##.....
	.#.#.#....#
	.#........#
	#.##...#...
	#...##....#
	.#..#...#.#
	`

var file, _ = ioutil.ReadFile("day3.input.txt")
var input = string(file)

func parse(source string) ([]string, int) {
	grid := strings.Fields(source)
	return grid, len(grid[0])
}

type Slope struct { dx int; dy int }

func solve(input string, slopes []Slope) int {
	grid, cx := parse(input)

	result := 1
	for _, slope := range slopes {
		trees := 0
		for x, y := slope.dx, slope.dy; y < len(grid); x, y = x + slope.dx, y + slope.dy {
			if grid[y][x % cx] == '#' {
				trees++
			}
		}
		result *= trees
	}

	return result
}

// PART 1

var slopes1 = []Slope{ { 3, 1 } }

// samples

func TestSample1(t *testing.T) {
	got, expected := solve(sample, slopes1), 7
	if got != expected {
		t.Error("Expected", expected, "got", got)
	}
}

// problem

func TestSolve1(t *testing.T) {
	got, expected := solve(input, slopes1), 230
	if got != expected {
		t.Error("Expected", expected, "got", got)
	}
}

// PART 2

var slopes2 = []Slope {
	{ 1, 1 },
	{ 3, 1 },
	{ 5, 1 },
	{ 7, 1 },
	{ 1, 2 } }

// samples

func TestSample2(t *testing.T) {
	got, expected := solve(sample, slopes2), 336
	if got != expected {
		t.Error("Expected", expected, "got", got)
	}
}

// problem

func TestSolve2(t *testing.T) {
	got, expected := solve(input, slopes2), 9533698720
	if got != expected {
		t.Error("Expected", expected, "got", got)
	}
}

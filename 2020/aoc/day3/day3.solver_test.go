package main

import (
	"strings"
	"testing"
)

import . "scottbilas/advent-of-code/2020/libaoc"

var sample = parse(NormalizeSample(`
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
	`))

var input = parse(ReadInputFile())

func parse(source string) []string {
	return strings.Fields(source)
}

type Slope struct {
	dx int
	dy int
}

func solve(board []string, slopes []Slope) int {
	result := 1
	for _, slope := range slopes {
		trees := 0
		for x, y := slope.dx, slope.dy; y < len(board); x, y = x+slope.dx, y+slope.dy {
			if board[y][x%len(board[0])] == '#' {
				trees++
			}
		}
		result *= trees
	}

	return result
}

// PART 1

var slopes1 = []Slope{{3, 1}}

func Test_Part1(t *testing.T) {
	t.Run("Sample", func(t *testing.T) {
		AssertEqual(t, solve(sample, slopes1), 7)
	})

	t.Run("Problem", func(t *testing.T) {
		AssertEqual(t, solve(input, slopes1), 230)
	})
}

// PART 2

var slopes2 = []Slope{
	{1, 1},
	{3, 1},
	{5, 1},
	{7, 1},
	{1, 2}}

func Test_Part2(t *testing.T) {
	t.Run("Sample", func(t *testing.T) {
		AssertEqual(t, solve(sample, slopes2), 336)
	})

	t.Run("Problem", func(t *testing.T) {
		AssertEqual(t, solve(input, slopes2), 9533698720)
	})
}

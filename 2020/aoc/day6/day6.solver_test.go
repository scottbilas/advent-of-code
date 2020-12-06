package main

import (
	"strings"
	"testing"
)

import . "scottbilas/advent-of-code/2020/libaoc"

var input = ParseBlocks(ReadInputFile())

var sample = ParseBlocks(NormalizeSample(`
	abc

	a
	b
	c

	ab
	ac

	a
	a
	a
	a

	b
	`))

func countChars(text string) map[rune]int {
	uniqs := make(map[rune]int)
	for _, char := range text {
		if char >= 'a' && char <= 'z' {
			uniqs[char]++
		}
	}
	return uniqs
}

// PART 1

func solve1(blocks []string) int {
	total := 0
	for _, block := range blocks {
		total += len(countChars(block))
	}
	return total
}

func Test_Part1(t *testing.T) {
	t.Run("Sample", func(t *testing.T) {
		AssertEqual(t, solve1(sample), 11)
	})

	t.Run("Problem", func(t *testing.T) {
		AssertEqual(t, solve1(input), 6714)
	})
}

func solve2(blocks []string) int {
	total := 0
	for _, block := range blocks {
		for _, v := range countChars(block) {
			if v == len(strings.Split(block, "\n")) {
				total++
			}
		}
	}
	return total
}

func Test_Part2(t *testing.T) {
	t.Run("Sample", func(t *testing.T) {
		AssertEqual(t, solve2(sample), 6)
	})

	t.Run("Problem", func(t *testing.T) {
		AssertEqual(t, solve2(input), 3435)
	})
}

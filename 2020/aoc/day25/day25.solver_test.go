package main

import (
	"testing"
)

import . "scottbilas/advent-of-code/2020/libaoc"

var sample = [2]int{5764801, 17807724}
var input = [2]int{15628416, 11161639}

func solve1(public [2]int) int {
	for i, card := 1, 1; ; i++ {
		if card = (card * 7) % 20201227; card == public[0] {
			key := 1
			for j := 0; j < j; j++ {
				key *= public[1]
				key %= 20201227
			}
			return key
		}
	}

}

func Test_Part1(t *testing.T) {
	AssertEqualSub("Sample", t, solve1(sample), 14897079)
	AssertEqualSub("Problem", t, solve1(input), 19774660)
}

package main

import (
	"testing"
)

import . "scottbilas/advent-of-code/2020/libaoc"

var sample = parse(NormalizeSample(`
    Player 1:
    9
    2
    6
    3
    1

    Player 2:
    5
    8
    4
    7
    10
	`))

var input = parse(ReadInputFile())

type Input struct {
	player1, player2 []int
}

func parse(source string) Input {
	block1, block2 := ParseBlocks2(source)
	return Input{ParseInts(block1)[1:], ParseInts(block2)[1:]}
}

func score(deck []int) int {
	result := 0
	for i := range deck {
		result += deck[len(deck)-i-1] * (i + 1)
	}
	return result
}

func round(deck1, deck2 []int, recurse bool) (winner int, deck []int) {
	deck1, deck2 = append([]int{}, deck1...), append([]int{}, deck2...)
	seen := make(map[uint64]bool)

	for {
		if len(deck1) == 0 {
			return 2, deck2
		}
		if len(deck2) == 0 {
			return 1, deck1
		}

		h := HashInit()
		h = HashInts(h, deck1)
		h = HashInt(h, 0)
		h = HashInts(h, deck2)

		if seen[h] {
			return 1, deck1
		}
		seen[h] = true

		winner := 1

		card1, card2 := deck1[0], deck2[0]
		deck1, deck2 = deck1[1:], deck2[1:]

		if recurse && len(deck1) >= card1 && len(deck2) >= card2 {
			winner, _ = round(deck1[:card1], deck2[:card2], true)
		} else if card1 < card2 {
			winner = 2
		}

		if winner == 1 {
			deck1 = append(deck1, card1, card2)
		} else {
			deck2 = append(deck2, card2, card1)
		}

	}
}

// PART 1

func solve1(input Input) int {
	_, deck := round(input.player1, input.player2, false)
	return score(deck)
}

func Test_Part1(t *testing.T) {
	AssertEqualSub("Sample", t, solve1(sample), 306)
	AssertEqualSub("Problem", t, solve1(input), 33393)
}

// PART 2

func solve2(input Input) int {
	_, deck := round(input.player1, input.player2, true)
	return score(deck)
}

func Test_Part2(t *testing.T) {
	AssertEqualSub("Sample", t, solve2(sample), 291)
	AssertEqualSub("Problem", t, solve2(input), 31963)
}

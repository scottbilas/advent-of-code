package main

import (
	"regexp"
	"testing"
)

import . "scottbilas/advent-of-code/2020/libaoc"

var sample = parse(`
    sesenwnenenewseeswwswswwnenewsewsw
    neeenesenwnwwswnenewnwwsewnenwseswesw
    seswneswswsenwwnwse
    nwnwneseeswswnenewneswwnewseswneseene
    swweswneswnenwsewnwneneseenw
    eesenwseswswnenwswnwnwsewwnwsene
    sewnenenenesenwsewnenwwwse
    wenwwweseeeweswwwnwwe
    wsweesenenewnwwnwsenewsenwwsesesenwne
    neeswseenwwswnwswswnw
    nenwswwsewswnenenewsenwsenwnesesenew
    enewnwewneswsewnwswenweswnenwsenwsw
    sweneswneswneneenwnewenewwneswswnese
    swwesenesewenwneswnwwneseswwne
    enesenwswwswneneswsenwnewswseenwsese
    wnwnesenesenenwwnenwsewesewsesesew
    nenewswnwewswnenesenwnesewesw
    eneswnwswnwsenenwnwnwwseeswneewsenese
    neswnwewnwnwseenwseesewsenwsweewe
    wseweeenwnesenwwwswnew
	`)

var input = parse(ReadInputFile())

var offsets = map[string]Coord{
	"e": {2, 0}, "se": {1, 1}, "sw": {-1, 1},
	"w": {-2, 0}, "nw": {-1, -1}, "ne": {1, -1},
}

type Coord struct{ x2, y int }
type Floor map[Coord]bool

func parse(source string) Floor {
	paths := make([][]string, 0)
	rx := regexp.MustCompile(`e|se|sw|w|nw|ne`)
	for _, line := range ParseLines(source) {
		paths = append(paths, rx.FindAllString(line, -1))
	}

	floor := make(Floor)
	for _, path := range paths {
		pos := Coord{}
		for _, step := range path {
			pos.x2 += offsets[step].x2
			pos.y += offsets[step].y
		}
		floor[pos] = !floor[pos]
	}

	return floor
}

func black(floor Floor) int {
	count := 0
	for _, black := range floor {
		if black {
			count++
		}
	}
	return count
}

// PART 1

func solve1(floor Floor) int {
	return black(floor)
}

func Test_Part1(t *testing.T) {
	AssertEqualSub("Sample", t, solve1(sample), 10)
	AssertEqualSub("Problem", t, solve1(input), 473)
}

// PART 2

func solve2(floor Floor) int {

	for i := 0; i < 100; i++ {
		for coord, tile := range floor {
			if tile {
				for _, offset := range offsets {
					rel := Coord{coord.x2 + offset.x2, coord.y + offset.y}
					floor[rel] = floor[rel]
				}
			}
		}

		next := make(Floor)

		for coord, tile := range floor {
			blacks := 0
			for _, offset := range offsets {
				if floor[Coord{coord.x2 + offset.x2, coord.y + offset.y}] {
					blacks++
				}
			}
			if (tile && blacks == 1) || blacks == 2 {
				next[coord] = true
			}
		}

		floor = next
	}

	return black(floor)
}

func Test_Part2(t *testing.T) {
	AssertEqualSub("Sample", t, func() int { return solve2(sample) }, 2208)
	AssertEqualSub("Problem", t, func() int { return solve2(input) }, 4070)
}

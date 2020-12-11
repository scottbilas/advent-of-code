package main

import (
	"fmt"
	"strings"
	"testing"
)

import . "scottbilas/advent-of-code/2020/libaoc"

var sample = makeBoard(`
    L.LL.LL.LL
    LLLLLLL.LL
    L.L.L..L..
    LLLL.LL.LL
    L.LL.LL.LL
    L.LLLLL.LL
    ..L.L.....
    LLLLLLLLLL
    L.LLLLLL.L
    L.LLLLL.LL
	`)

var input = makeBoard(ReadInputFile())

type Board struct {
	grid   []byte
	cx, cy int
}

func makeBoard(source string) Board {
	rows := strings.Fields(source)
	board := Board{nil, len(rows[0]), len(rows)}

	board.grid = make([]byte, board.cx*board.cy)
	for y, row := range rows {
		offset := y * board.cx
		copy(board.grid[offset:offset+board.cx], row)
	}

	return board
}

func (board *Board) offset(x, y int) int   { return y*board.cx + x }
func (board *Board) get(x, y int) byte     { return board.grid[board.offset(x, y)] }
func (board *Board) set(x, y int, to byte) { board.grid[board.offset(x, y)] = to }
func (board *Board) valid(x, y int) bool   { return x >= 0 && x < board.cx && y >= 0 && y < board.cy }
func (board *Board) used(x, y int) bool    { return board.valid(x, y) && board.get(x, y) == '#' }

func (board *Board) clone() Board {
	grid := make([]byte, len(board.grid))
	copy(grid, board.grid)
	return Board{grid, board.cx, board.cy}
}

func (board *Board) dump() {
	for y := 0; y < board.cy; y++ {
		offset := board.offset(0, y)
		fmt.Println(string(board.grid[offset : offset+board.cx]))
	}
	fmt.Println()
}

func (board *Board) occupied(x, y int, seekFunc func(board *Board, x, y, dx, dy int) bool) int {
	count := 0
	for _, offset := range []struct{ x, y int }{{-1, -1}, {0, -1}, {1, -1}, {-1, 0}, {1, 0}, {-1, 1}, {0, 1}, {1, 1}} {
		count += ToInt(seekFunc(board, x, y, offset.x, offset.y))
	}
	return count
}

func (board *Board) solve(max int, seekFunc func(board *Board, x, y, dx, dy int) bool) int {
	for current, next := board.clone(), board.clone(); ; {
		modified := false
		for y := 0; y < current.cy; y++ {
			for x := 0; x < current.cx; x++ {
				seat, count := current.get(x, y), current.occupied(x, y, seekFunc)
				if seat == 'L' && count == 0 {
					next.set(x, y, '#')
					modified = true
				} else if seat == '#' && count >= max {
					next.set(x, y, 'L')
					modified = true
				} else {
					next.set(x, y, seat)
				}
			}
		}

		if !modified {
			count := 0
			for _, seat := range current.grid {
				if seat == '#' {
					count++
				}
			}
			return count
		}

		current, next = next, current
	}
}

// PART 1

func solve1(board Board) int {
	return board.solve(4, func(board *Board, x, y, dx, dy int) bool {
		return board.used(x+dx, y+dy)
	})
}

func Test_Part1(t *testing.T) {
	AssertEqualSub("Sample", t, solve1(sample), 37)
	AssertEqualSub("Problem", t, solve1(input), 2263)
}

// PART 2

func seek2(board *Board, x, y, dx, dy int) bool {
	var found byte = 0
	for tx, ty := x, y; found == 0; {
		tx, ty = tx+dx, ty+dy
		if !board.valid(tx, ty) {
			found = '.'
		} else if c := board.get(tx, ty); c != '.' {
			found = c
		}
	}
	return found == '#'
}

func solve2(board Board) int {
	return board.solve(5, seek2)
}

func Test_Part2(t *testing.T) {

	micro1 := makeBoard(`
        .......#.
        ...#.....
        .#.......
        .........
        ..#L....#
        ....#....
        .........
        #........
        ...#.....
        `)
	AssertEqualSub("Micro1", t, micro1.occupied(3, 4, seek2), 8)

	micro2 := makeBoard(`
        .............
        .L.L.#.#.#.#.
        .............
        `)
	AssertEqualSub("Micro2", t, micro2.occupied(1, 1, seek2), 0)

	micro3 := makeBoard(`
        .##.##.
        #.#.#.#
        ##...##
        ...L...
        ##...##
        #.#.#.#
        .##.##.
        `)
	AssertEqualSub("Micro3", t, micro3.occupied(3, 3, seek2), 0)

	AssertEqualSub("Sample", t, solve2(sample), 26)
	AssertEqualSub("Problem", t, solve2(input), 2002)
}

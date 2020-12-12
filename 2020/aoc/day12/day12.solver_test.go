package main

import (
	"strings"
	"testing"
)

import . "scottbilas/advent-of-code/2020/libaoc"

var sample = strings.Fields(`
    F10
    N3
    F7
    R90
    F11
	`)

var input = strings.Fields(ReadInputFile())

// PART 1

func solve1(instrs []string) int {
	x, y, dir := 0, 0, 1
	for _, instr := range instrs {
		cmd, dist := instr[0], ParseInt(instr[1:])
		if cmd == 'F' {
			cmd = "NESW"[dir]
		}
		switch cmd {
		case 'N':
			y += dist
		case 'S':
			y -= dist
		case 'E':
			x += dist
		case 'W':
			x -= dist
		case 'L':
			dist = -dist
			fallthrough
		case 'R':
			dir = (dir + dist/90 + 4) % 4
		}
	}
	return ManhattanZero(x, y)
}

func Test_Part1(t *testing.T) {
	AssertEqualSub("Sample", t, solve1(sample), 25)
	AssertEqualSub("Problem", t, solve1(input), 759)
}

// PART 2

func solve2(instrs []string) int {
	x, y, wx, wy := 0, 0, 10, 1
	for _, instr := range instrs {
		cmd, dist := instr[0], ParseInt(instr[1:])
		if cmd == 'F' {
			x, y = x+wx*dist, y+wy*dist
		}
		switch cmd {
		case 'N':
			wy += dist
		case 'S':
			wy -= dist
		case 'E':
			wx += dist
		case 'W':
			wx -= dist
		case 'L':
			dist = 360 - dist
			fallthrough
		case 'R':
			if dist == 90 {
				wx, wy = wy, -wx
			} else if dist == 180 {
				wx, wy = -wx, -wy
			} else if dist == 270 {
				wx, wy = -wy, wx
			}
		}
	}
	return ManhattanZero(x, y)
}

func Test_Part2(t *testing.T) {
	AssertEqualSub("Sample", t, solve2(sample), 286)
	AssertEqualSub("Problem", t, solve2(input), 45763)
}

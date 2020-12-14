package main

import (
	"testing"
)

import . "scottbilas/advent-of-code/2020/libaoc"

var input = ParseLines(ReadInputFile())

func solve(instrs []string, writer func(mem map[int]int, mask string, addr int, num int)) int {
	mem, mask := make(map[int]int), ""
	for _, instr := range instrs {
		l, r := SplitTrim2(instr, "=")
		if l == "mask" {
			mask = r
		} else {
			nums := ParseInts(instr)
			writer(mem, mask, nums[0], nums[1])
		}
	}

	sum := 0
	for _, v := range mem {
		sum += v
	}
	return sum
}

func setBit(num int, ibit int, on bool) int {
	bit := 1 << ibit
	if on {
		num |= bit
	} else {
		num &= ^bit
	}
	return num
}

// PART 1

func solve1(instrs []string) int {
	return solve(instrs,
		func(mem map[int]int, mask string, addr int, num int) {
			for i, c := range mask {
				if c != 'X' {
					num = setBit(num, 35-i, c == '1')
				}
			}
			mem[addr] = num
		})
}

func Test_Part1(t *testing.T) {
	var sample = ParseLines(`
        mask = XXXXXXXXXXXXXXXXXXXXXXXXXXXXX1XXXX0X
        mem[8] = 11
        mem[7] = 101
        mem[8] = 0
        `)
	AssertEqualSub("Sample", t, solve1(sample), 165)

	AssertEqualSub("Problem", t, solve1(input), 12610010960049)
}

// PART 2

func solve2(instrs []string) int {
	return solve(instrs,
		func(mem map[int]int, mask string, addr int, num int) {
			floats := make([]int, 0)
			for i, c := range mask {
				if c == '1' {
					addr = setBit(addr, 35-i, true)
				} else if c == 'X' {
					floats = append(floats, 35-i)
				}
			}

			for combo := 0; combo < 1<<len(floats); combo++ {
				for ibit := 0; ibit < len(floats); ibit++ {
					addr = setBit(addr, floats[ibit], (combo&(1<<ibit)) != 0)
				}
				mem[addr] = num
			}
		})
}

func Test_Part2(t *testing.T) {
	sample := ParseLines(`
        mask = 000000000000000000000000000000X1001X
        mem[42] = 100
        mask = 00000000000000000000000000000000X0XX
        mem[26] = 1
        `)
	AssertEqualSub("Sample", t, solve2(sample), 208)

	AssertEqualSub("Problem", t, solve2(input), 3608464522781)
}

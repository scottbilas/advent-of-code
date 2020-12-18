package main

import (
	"regexp"
	"testing"
)

import . "scottbilas/advent-of-code/2020/libaoc"

var sample1 = "1 + 2 * 3 + 4 * 5 + 6"
var sample2 = "1 + (2 * 3) + (4 * (5 + 6))"
var sample3 = "2 * 3 + (4 * 5)"
var sample4 = "5 + (8 * 3 + 9 + 3 * 4 * 3)"
var sample5 = "5 * 9 * (7 * 3 * 3 + 9 * 3 + (8 + 6 * 4))"
var sample6 = "((2 + 4 * 9) * (6 + 9 * 8 + 6) + 6) + 2 + 4 * 2"
var input = ReadInputFile()

type Node []interface{}

func parseTokens(tokens []string) (Node, int) {
	root, i := make(Node, 0), 0
	for ; i < len(tokens); i++ {
		token := tokens[i]
		if token == "(" {
			node, skip := parseTokens(tokens[i+1:])
			root, i = append(root, node), i+skip+1
		} else if token == ")" {
			break
		} else if value := ParseIntSafe(token, -1); value >= 0 {
			root = append(root, value)
		} else {
			root = append(root, token)
		}
	}
	return root, i
}

func eval(node Node, index int, process func(node Node) int) int {
	if child, ok := node[index].(Node); ok {
		return process(child)
	}
	return node[index].(int)
}

func solve(source string, process func(Node) int) int {
	total, rx := 0, regexp.MustCompile(`[()*+]|\d+`)
	for _, line := range ParseLines(source) {
		node, _ := parseTokens(rx.FindAllString(line, -1))
		total += process(node)
	}
	return total
}

// PART 1

func solve1(source string) int {
	var process func(node Node) int
	process = func(node Node) int {
		accum := eval(node, 0, process)
		for i := 2; i < len(node); i += 2 {
			right := eval(node, i, process)
			if node[i-1] == "*" {
				accum *= right
			} else {
				accum += right
			}
		}
		return accum
	}

	return solve(source, process)
}

func Test_Part1(t *testing.T) {
	AssertEqualSub("Sample1", t, solve1(sample1), 71)
	AssertEqualSub("Sample2", t, solve1(sample2), 51)
	AssertEqualSub("Sample3", t, solve1(sample3), 26)
	AssertEqualSub("Sample4", t, solve1(sample4), 437)
	AssertEqualSub("Sample5", t, solve1(sample5), 12240)
	AssertEqualSub("Sample6", t, solve1(sample6), 13632)
	AssertEqualSub("Problem", t, solve1(input), 24650385570008)
}

// PART 2

func solve2(source string) int {
	var process func(node Node) int
	process = func(node Node) int {
		for i := 2; i < len(node); {
			if node[i-1] == "+" {
				left, right := eval(node, i-2, process), eval(node, i, process)
				node = append(append(node[:i-2], left+right), node[i+1:]...)
			} else {
				i += 2
			}
		}

		accum := eval(node, 0, process)
		for i := 2; i < len(node); i += 2 {
			accum *= eval(node, i, process)
		}
		return accum
	}

	return solve(source, process)
}

func Test_Part2(t *testing.T) {
	AssertEqualSub("Sample1", t, solve2(sample1), 231)
	AssertEqualSub("Sample2", t, solve2(sample2), 51)
	AssertEqualSub("Sample3", t, solve2(sample3), 46)
	AssertEqualSub("Sample4", t, solve2(sample4), 1445)
	AssertEqualSub("Sample5", t, solve2(sample5), 669060)
	AssertEqualSub("Sample6", t, solve2(sample6), 23340)
	AssertEqualSub("Problem", t, solve2(input), 158183007916215)
}

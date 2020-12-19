package main

import (
	"strings"
	"testing"
)

import . "scottbilas/advent-of-code/2020/libaoc"

var input = ReadInputFile()

var sample2 = NormalizeSample(`
    42: 9 14 | 10 1
    9: 14 27 | 1 26
    10: 23 14 | 28 1
    1: "a"
    11: 42 31
    5: 1 14 | 15 1
    19: 14 1 | 14 14
    12: 24 14 | 19 1
    16: 15 1 | 14 14
    31: 14 17 | 1 13
    6: 14 14 | 1 14
    2: 1 24 | 14 4
    0: 8 11
    13: 14 3 | 1 12
    15: 1 | 14
    17: 14 2 | 1 7
    23: 25 1 | 22 14
    28: 16 1
    4: 1 1
    20: 14 14 | 1 15
    3: 5 14 | 16 1
    27: 1 6 | 14 18
    14: "b"
    21: 14 1 | 1 14
    25: 1 1 | 1 14
    22: 14 14
    8: 42
    26: 14 22 | 1 20
    18: 15 15
    7: 14 5 | 1 21
    24: 14 1

    abbbbbabbbaaaababbaabbbbabababbbabbbbbbabaaaa
    bbabbbbaabaabba
    babbbbaabbbbbabbbbbbaabaaabaaa
    aaabbbbbbaaaabaababaabababbabaaabbababababaaa
    bbbbbbbaaaabbbbaaabbabaaa
    bbbababbbbaaaaaaaabbababaaababaabab
    ababaaaaaabaaab
    ababaaaaabbbaba
    baabbaaaabbaaaababbaababb
    abbbbabbbbaaaababbbbbbaaaababb
    aaaaabbaabaaaaababaa
    aaaabbaaaabbaaa
    aaaabbaabbaaaaaaabbbabbbaaabbaabaaa
    babaaabbbaaabaababbaabababaaab
    aabbbbbaabbbaaaaaabbbbbababaaaaabbaaabba
    `)

type Data struct {
	rules    map[string]string
	messages []string
}

func makeData(source string) Data {
	rules := make(map[string]string)
	blocks := ParseBlocks(source)

	for _, rule := range ParseLines(blocks[0]) {
		num, pattern := SplitTrim2(rule, ":")
		rules[num] = pattern
	}

	return Data{rules, ParseLines(blocks[1])}
}

func (data *Data) match(rule string, message string) []int {
	matches := make([]int, 0)

	if rule[0] == '"' {
		if rule[1] == message[0] {
			matches = append(matches, 1)
		}
	} else if strings.Contains(rule, "|") {
		for _, clause := range SplitTrim(rule, "|") {
			matches = append(matches, data.match(clause, message)...)
		}
	} else {
		parts := strings.SplitN(rule, " ", 2)
		for _, subMatch0 := range data.match(data.rules[parts[0]], message) {
			if len(parts) == 1 {
				matches = append(matches, subMatch0)
			} else if subMatch0 < len(message) {
				for _, subMatch1 := range data.match(parts[1], message[subMatch0:]) {
					matches = append(matches, subMatch0+subMatch1)
				}
			}
		}
	}

	return matches
}

func (data *Data) solve() int {
	sum := 0
	for _, message := range data.messages {
		for _, match := range data.match(data.rules["0"], message) {
			if match == len(message) {
				sum++
				break
			}
		}
	}
	return sum
}

// PART 1

func solve1(source string) int {
	data := makeData(source)
	return data.solve()
}

func Test_Part1(t *testing.T) {
	var sample = NormalizeSample(`
        0: 4 1 5
        1: 2 3 | 3 2
        2: 4 4 | 5 5
        3: 4 5 | 5 4
        4: "a"
        5: "b"

        ababbb
        bababa
        abbbab
        aaabbb
        aaaabbb
        `)

	AssertEqualSub("Sample", t, solve1(sample), 2)
	AssertEqualSub("Sample2", t, solve1(sample2), 3)
	AssertEqualSub("Problem", t, solve1(input), 272)
}

// PART 2

func solve2(source string) int {
	data := makeData(source)
	data.rules["8"] = "42 | 42 8"
	data.rules["11"] = "42 31 | 42 11 31"
	return data.solve()
}

func Test_Part2(t *testing.T) {
	AssertEqualSub("Sample", t, solve2(sample2), 12)
	AssertEqualSub("Problem", t, solve2(input), 374)
}

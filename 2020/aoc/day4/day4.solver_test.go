package main

import (
	"regexp"
	"strings"
	"testing"
)

import . "scottbilas/advent-of-code/2020/libaoc"

var input = ParseBlocks(ReadInputFile())

// PART 1

func solve1(blocks []string) int {
	valid := 0
	for _, block := range blocks {
		valid += ToInt(len(strings.Fields(block)) == 7+ToInt(strings.Contains(block, "cid")))
	}
	return valid
}

func Test_Part1(t *testing.T) {
	var sample1 = ParseBlocks(`
		ecl:gry pid:860033327 eyr:2020 hcl:#fffffd
		byr:1937 iyr:2017 cid:147 hgt:183cm

		iyr:2013 ecl:amb cid:350 eyr:2023 pid:028048884
		hcl:#cfa07d byr:1929

		hcl:#ae17e1 iyr:2013
		eyr:2024
		ecl:brn pid:760753108 byr:1931
		hgt:179cm

		hcl:#cfa07d eyr:2025 pid:166559648
		iyr:2011 ecl:brn hgt:59in
		`)

	AssertEqualSub("Sample", t, solve1(sample1), 2)
	AssertEqualSub("Problem", t, solve1(input), 196)
}

// PART 2

func checkIntRange(values []string, index int, lowerInclusive int, upperInclusive int) bool {
	if values == nil {
		return false
	}

	i := ParseIntSafe(values[index], lowerInclusive-1)
	return i >= lowerInclusive && i <= upperInclusive
}

func solve2(blocks []string) int {
	byrrx := regexp.MustCompile(`byr:(\d+)`)
	eyrrx := regexp.MustCompile(`eyr:(\d+)`)
	iyrrx := regexp.MustCompile(`iyr:(\d+)`)
	hgtrx := regexp.MustCompile(`hgt:(\d+)(in|cm)`)
	hclrx := regexp.MustCompile(`hcl:#[0-9a-f]{6}`)
	eclrx := regexp.MustCompile(`ecl:(amb|blu|brn|gry|grn|hzl|oth)`)
	pidrx := regexp.MustCompile(`pid:[0-9]{9}\b`)

	valid := 0
	for _, block := range blocks {
		ok := 0

		ok += ToInt(checkIntRange(byrrx.FindStringSubmatch(block), 1, 1920, 2002))
		ok += ToInt(checkIntRange(iyrrx.FindStringSubmatch(block), 1, 2010, 2020))
		ok += ToInt(checkIntRange(eyrrx.FindStringSubmatch(block), 1, 2020, 2030))

		if hgt := hgtrx.FindStringSubmatch(block); hgt != nil {
			if hgt[2] == "cm" {
				ok += ToInt(checkIntRange(hgt, 1, 150, 193))
			} else {
				ok += ToInt(checkIntRange(hgt, 1, 59, 76))
			}
		}

		ok += ToInt(hclrx.MatchString(block))
		ok += ToInt(eclrx.MatchString(block))
		ok += ToInt(pidrx.MatchString(block))

		if ok == 7 {
			valid++
		}
	}
	return valid
}

func Test_Part2(t *testing.T) {

	var invalid = ParseBlocks(`
		eyr:1972 cid:100
		hcl:#18171d ecl:amb hgt:170 pid:186cm iyr:2018 byr:1926

		iyr:2019
		hcl:#602927 eyr:1967 hgt:170cm
		ecl:grn pid:012533040 byr:1946

		hcl:dab227 iyr:2012
		ecl:brn hgt:182cm pid:021572410 eyr:2020 byr:1992 cid:277

		hgt:59cm ecl:zzz
		eyr:2038 hcl:74454a iyr:2023
		pid:3556412378 byr:2007
		`)

	AssertEqualSub("Sample (Invalid)", t, solve2(invalid), 0)

	var valid = ParseBlocks(`
		pid:087499704 hgt:74in ecl:grn iyr:2012 eyr:2030 byr:1980
		hcl:#623a2f

		eyr:2029 ecl:blu cid:129 byr:1989
		iyr:2014 pid:896056539 hcl:#a97842 hgt:165cm

		hcl:#888785
		hgt:164cm byr:2001 iyr:2015 cid:88
		pid:545766238 ecl:hzl
		eyr:2022

		iyr:2010 hgt:158cm hcl:#b6652a ecl:blu byr:1944 eyr:2021 pid:093154719
		`)

	AssertEqualSub("Sample (Valid)", t, solve2(valid), 4)
	AssertEqualSub("Problem", t, solve2(input), 114)
}

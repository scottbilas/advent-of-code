package main

import (
	"strings"
	"testing"
)

import . "scottbilas/advent-of-code/2020/libaoc"

var input = ReadInputFile()

type Field struct {
	name   string
	ranges []int
}

type Doc struct {
	fields []Field
	yours  []int
	nearby [][]int
}

func makeDoc(source string) Doc {
	blocks := ParseBlocks(source)
	var doc Doc

	for _, fields := range ParseLines(blocks[0]) {
		name, nums := SplitTrim2(fields, ":")
		ranges := ParseInts(strings.Replace(nums, "-", " ", -1))
		doc.fields = append(doc.fields, Field{name, ranges})
	}

	doc.yours = ParseInts(blocks[1])

	for _, tickets := range ParseLines(blocks[2])[1:] {
		doc.nearby = append(doc.nearby, ParseInts(tickets))
	}

	return doc
}

func (doc *Doc) discardInvalid() []int {
	invalid := make([]int, 0)
	for inearby := 0; inearby < len(doc.nearby); inearby++ {
		pre := len(invalid)
		for _, slot := range doc.nearby[inearby] {
			valid := 0
			for _, field := range doc.fields {
				valid += ToInt(
					slot >= field.ranges[0] && slot <= field.ranges[1] ||
						slot >= field.ranges[2] && slot <= field.ranges[3])
			}
			if valid == 0 {
				invalid = append(invalid, slot)
			}
		}
		if len(invalid) != pre {
			doc.nearby = append(doc.nearby[:inearby], doc.nearby[inearby+1:]...)
			inearby--
		}
	}

	return invalid
}

func (doc *Doc) resolveFields() map[string]int {
	hits := make(map[string][]int)
	for _, field := range doc.fields {
		hits[field.name] = make([]int, len(doc.nearby[0]))
	}

	for _, nearby := range doc.nearby {
		for islot, slot := range nearby {
			for igroup := 0; igroup < 2; igroup++ {
				for _, field := range doc.fields {
					if slot >= field.ranges[igroup*2] && slot <= field.ranges[igroup*2+1] {
						hits[field.name][islot]++
					}
				}
			}
		}
	}

	resolved := make(map[string]int)

	for len(resolved) != len(doc.fields) {
		for name, hit := range hits {
			where := -1
			for slot, count := range hit {
				if count == len(doc.nearby) {
					if where != -1 {
						where = -1
						break
					}
					where = slot
				}
			}
			if where != -1 {
				resolved[name] = doc.yours[where]
				for otherName, otherHit := range hits {
					if otherName != name {
						otherHit[where] = 0
					}
				}
			}
		}
	}

	return resolved
}

// PART 1

func solve1(source string) int {
	sum, doc := 0, makeDoc(source)
	for _, invalid := range doc.discardInvalid() {
		sum += invalid
	}
	return sum
}

func Test_Part1(t *testing.T) {
	var sample = `
        class: 1-3 or 5-7
        row: 6-11 or 33-44
        seat: 13-40 or 45-50

        your ticket:
        7,1,14

        nearby tickets:
        7,3,47
        40,4,50
        55,2,20
        38,6,12
        `

	AssertEqualSub("Sample", t, solve1(sample), 71)
	AssertEqualSub("Problem", t, solve1(input), 20058)
}

// PART 2

func solve2(source string) int {
	doc := makeDoc(source)
	doc.discardInvalid()

	result := 1
	for name, value := range doc.resolveFields() {
		if strings.HasPrefix(name, "departure") {
			result *= value
		}
	}

	return result
}

func Test_Part2(t *testing.T) {
	sample := makeDoc(`
        class: 0-1 or 4-19
        row: 0-5 or 8-19
        seat: 0-13 or 16-19

        your ticket:
        11,12,13

        nearby tickets:
        3,9,18
        15,1,5
        5,14,9
        `)
	sample.discardInvalid()

	AssertEqualSub("Sample", t,
		sample.resolveFields(),
		map[string]int{"class": 12, "row": 11, "seat": 13})

	AssertEqualSub("Problem", t, solve2(input), 366871907221)
}

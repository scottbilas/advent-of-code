package main

import (
    "strings"
    "testing"
)

import . "scottbilas/advent-of-code/2020/libaoc"

var input = ReadInputFile()

type Field struct {
    Name   string
    Ranges []int
}

type Doc struct {
    Fields []Field
    Yours  []int
    Nearby [][]int
}

func makeDoc(source string) Doc {
    blocks := ParseBlocks(source)
    var doc Doc

    for _, fields := range ParseLines(blocks[0]) {
        name, nums := SplitTrim2(fields, ":")
        ranges := ParseInts(strings.Replace(nums, "-", " ", -1))
        doc.Fields = append(doc.Fields, Field{name, ranges})
    }

    doc.Yours = ParseInts(blocks[1])

    for _, tickets := range ParseLines(blocks[2])[1:] {
        doc.Nearby = append(doc.Nearby, ParseInts(tickets))
    }

    return doc
}

func (doc *Doc) discardInvalid() []int {
    invalid := make([]int, 0)
    for inearby := 0; inearby < len(doc.Nearby); inearby++ {
        pre := len(invalid)
        for _, slot := range doc.Nearby[inearby] {
            valid := 0
            for _, field := range doc.Fields {
                valid += ToInt(
                    slot >= field.Ranges[0] && slot <= field.Ranges[1] ||
                    slot >= field.Ranges[2] && slot <= field.Ranges[3])
            }
            if valid == 0 {
                invalid = append(invalid, slot)
            }
        }
        if len(invalid) != pre {
            doc.Nearby = append(doc.Nearby[:inearby], doc.Nearby[inearby+1:]...)
            inearby--
        }
    }

    return invalid
}

func (doc *Doc) resolveFields() map[string]int {
    hits := make(map[string][]int)
    for _, field := range doc.Fields {
        hits[field.Name] = make([]int, len(doc.Nearby[0]))
    }

    for _, nearby := range doc.Nearby {
        for islot, slot := range nearby {
            for igroup := 0; igroup < 2; igroup++ {
                for _, field := range doc.Fields {
                    if slot >= field.Ranges[igroup*2] && slot <= field.Ranges[igroup*2+1] {
                        hits[field.Name][islot]++
                    }
                }
            }
        }
    }

    resolved := make(map[string]int)

    for len(resolved) != len(doc.Fields) {
        for name, hit := range hits {
            where := -1
            for slot, count := range hit {
                if count == len(doc.Nearby) {
                    if where != -1 {
                        where = -1
                        break
                    }
                    where = slot
                }
            }
            if where != -1 {
                resolved[name] = doc.Yours[where]
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

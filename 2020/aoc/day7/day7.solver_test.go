package main

import (
	"regexp"
	"testing"
)

import . "scottbilas/advent-of-code/2020/libaoc"

var sample1 = parse(NormalizeSample(`
    light red bags contain 1 bright white bag, 2 muted yellow bags.
    dark orange bags contain 3 bright white bags, 4 muted yellow bags.
    bright white bags contain 1 shiny gold bag.
    muted yellow bags contain 2 shiny gold bags, 9 faded blue bags.
    shiny gold bags contain 1 dark olive bag, 2 vibrant plum bags.
    dark olive bags contain 3 faded blue bags, 4 dotted black bags.
    vibrant plum bags contain 5 faded blue bags, 6 dotted black bags.
    faded blue bags contain no other bags.
    dotted black bags contain no other bags.
    `))

var sample2 = parse(NormalizeSample(`
    shiny gold bags contain 2 dark red bags.
    dark red bags contain 2 dark orange bags.
    dark orange bags contain 2 dark yellow bags.
    dark yellow bags contain 2 dark green bags.
    dark green bags contain 2 dark blue bags.
    dark blue bags contain 2 dark violet bags.
    dark violet bags contain no other bags.
    `))

var input = parse(ReadInputFile())

type Constraint struct {
	name     string
	children []string
	counts   []int
}

func parse(source string) []Constraint {
	rx := regexp.MustCompile(`(\d+)?\s*(\w+ \w+) bag`)
	lines := ParseLines(source)
	constraints := make([]Constraint, len(lines))

	for iline := 0; iline < len(lines); iline++ {
		constraint := &constraints[iline]

		matches := rx.FindAllStringSubmatch(lines[iline], -1)
		constraint.name = matches[0][2]

		if matches[1][1] != "" {
			constraint.children = make([]string, len(matches)-1)
			constraint.counts = make([]int, len(matches)-1)

			for imatch := 1; imatch < len(matches); imatch++ {
				constraint.children[imatch-1] = matches[imatch][2]
				constraint.counts[imatch-1] = ParseInt(matches[imatch][1])
			}
		}
	}

	return constraints
}

// PART 1

func solve1(constraints []Constraint) int {
	parents := make(map[string][]string)
	for _, constraint := range constraints {
		for _, child := range constraint.children {
			children := parents[child]
			if children == nil {
				children = make([]string, 0)
			}
			parents[child] = append(parents[child], constraint.name)
		}
	}

	work := []string{"shiny gold"}
	visited := make(map[string]bool)

	for i := 0; i < len(work); i++ {
		for _, parent := range parents[work[i]] {
			if !visited[parent] {
				work = append(work, parent)
			}
			visited[parent] = true
		}
	}

	return len(work) - 1
}

func Test_Part1(t *testing.T) {
	AssertEqualSub("Sample", t, solve1(sample1), 4)
	AssertEqualSub("Problem", t, solve1(input), 335)
}

func solve2(constraints []Constraint) int {
	resolved, unresolved := make(map[string]int), make(map[string]Constraint)
	for _, constraint := range constraints {
		if len(constraint.children) == 0 {
			resolved[constraint.name] = 0
		} else {
			unresolved[constraint.name] = constraint
		}
	}

	for {
		for pname, parent := range unresolved {
			total, ok := 0, true
			for i, child := range parent.children {
				if count, exists := resolved[child]; exists {
					total += parent.counts[i] * (count + 1)
				} else {
					ok = false
					break
				}
			}
			if ok {
				if pname == "shiny gold" {
					return total
				}
				delete(unresolved, pname)
				resolved[pname] = total
				break
			}
		}
	}
}

func Test_Part2(t *testing.T) {
	AssertEqualSub("Sample1", t, solve2(sample1), 32)
	AssertEqualSub("Sample2", t, solve2(sample2), 126)
	AssertEqualSub("Problem", t, solve2(input), 2431)
}

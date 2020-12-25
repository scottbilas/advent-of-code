package main

import (
	"regexp"
	"sort"
	"strings"
	"testing"
)

import . "scottbilas/advent-of-code/2020/libaoc"

var sample = parse(NormalizeSample(`
    mxmxvkd kfcds sqjhc nhms (contains dairy, fish)
    trh fvjkl sbzzf mxmxvkd (contains dairy)
    sqjhc fvjkl (contains soy)
    sqjhc mxmxvkd sbzzf (contains fish)
	`))

var input = parse(ReadInputFile())

type Food struct {
	ingredients []string
	allergens   []string
}

type Match struct {
	ingredient string
	allergen   string
}

func parse(source string) []Food {
	foods := make([]Food, 0)
	for _, line := range regexp.MustCompile(`(.*) \(contains (.*)\)`).FindAllStringSubmatch(source, -1) {
		ingredients, allergens := SplitTrim(line[1], " "), SplitTrim(line[2], ", ")
		foods = append(foods, Food{ingredients, allergens})
	}
	return foods
}

func solve(foods []Food) []Match {
	db := make(map[string] /*allergen*/ map[string] /*ingredient*/ int /*count*/)
	for _, food := range foods {
		for _, allergen := range food.allergens {
			counts := db[allergen]
			if counts == nil {
				counts = make(map[string]int)
				db[allergen] = counts
			}
			for _, ingredient := range food.ingredients {
				counts[ingredient]++
			}
		}
	}

	matches := make([]Match, 0)
	for len(db) > 0 {
		for allergen, counts := range db {
			max := 0
			for _, count := range counts {
				MaximizeInt(&max, count)
			}
			var found string
			for ingredient, count := range counts {
				if count < max {
					delete(counts, ingredient)
				} else {
					found = ingredient
				}
			}
			if len(counts) == 1 {
				matches = append(matches, Match{found, allergen})
				delete(db, allergen)
				for _, counts2 := range db {
					delete(counts2, found)
				}
			}
		}
	}

	return matches
}

// PART 1

func solve1(foods []Food) int {
	allergenic := make(map[string]bool)
	for _, match := range solve(foods) {
		allergenic[match.ingredient] = true
	}

	sum := 0
	for _, food := range foods {
		for _, ingredient := range food.ingredients {
			if !allergenic[ingredient] {
				sum++
			}
		}
	}

	return sum
}

func Test_Part1(t *testing.T) {
	AssertEqualSub("Sample", t, func() int { return solve1(sample) }, 5)
	AssertEqualSub("Problem", t, func() int { return solve1(input) }, 1945)
}

// PART 2

func solve2(foods []Food) string {
	matches := solve(foods)
	sort.Slice(matches, func(i, j int) bool {
		return matches[i].allergen < matches[j].allergen
	})

	ingredientNames := make([]string, 0)
	for _, pair := range matches {
		ingredientNames = append(ingredientNames, pair.ingredient)
	}

	return strings.Join(ingredientNames, ",")
}

func Test_Part2(t *testing.T) {
	AssertEqualSub("Sample", t, solve2(sample), "mxmxvkd,sqjhc,fvjkl")
	AssertEqualSub("Problem", t, solve2(input), "pgnpx,srmsh,ksdgk,dskjpq,nvbrx,khqsk,zbkbgp,xzb")
}

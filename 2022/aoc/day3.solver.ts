import _ = require('lodash')
import u = require('./utils')

const day = 3

const sample = u.parseWords(`
    vJrwpWtwJgWrhcsFMMfFFhFp
    jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL
    PmmdzqPrVvPwwTWBwg
    wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn
    ttgJtRGJQctTZtZT
    CrZsJsPPZsGzwwsLwLmpwMDw`)

const input = u.parseWords(u.getProblemInput(day))

function solve(hunks: string[]) {
    const match = _.chain(hunks)
        .map(_.uniq)                            // find unique letters in each hunk
        .flatten()                              // combine all letters
        .groupBy(_.identity)                    // group to get counts
        .find(g => g.length == hunks.length)    // find the one contained by all hunks
        .value()[0]                             // get the char

    // score it
    const [base, off] = match >= 'a' ? ['a', 1] : ['A', 27]
    return match.charCodeAt(0) - base.charCodeAt(0) + off
}

// solve by halves
const solve1 = rucksacks => _.chain(rucksacks)
    .map(r => solve([r.slice(0, r.length/2), r.slice(r.length/2)]))
    .sum().value()

// solve by triplets
const solve2 = rucksacks => _.chain(rucksacks)
    .chunk(3).map(solve)
    .sum().value()

test(`Day ${day}.1 Sample`, () => { expect(solve1(sample)).toBe(157); });
test(`Day ${day}.1 Problem`, () => { expect(solve1(input)).toBe(7848); });
test(`Day ${day}.2 Sample`, () => { expect(solve2(sample)).toBe(70); });
test(`Day ${day}.2 Problem`, () => { expect(solve2(input)).toBe(2616); });

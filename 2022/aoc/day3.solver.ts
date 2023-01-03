import { check, getProblemInput, parseWords } from './utils'
import _ = require('lodash')

const day = 3

const sample = parseWords(`
    vJrwpWtwJgWrhcsFMMfFFhFp
    jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL
    PmmdzqPrVvPwwTWBwg
    wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn
    ttgJtRGJQctTZtZT
    CrZsJsPPZsGzwwsLwLmpwMDw`)

const input = parseWords(getProblemInput(day))

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

check(`Day ${day}.1 Sample`,  () => solve1(sample),  157)
check(`Day ${day}.1 Problem`, () => solve1(input),  7848)
check(`Day ${day}.2 Sample`,  () => solve2(sample),   70)
check(`Day ${day}.2 Problem`, () => solve2(input),  2616)

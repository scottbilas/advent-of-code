import { readFileSync } from 'fs'
import { from, range } from 'linq-to-typescript'
const day = 3

function parse(text: string) {
    return text.trim().split(/\s+/g)
}

const sample = parse(`
    vJrwpWtwJgWrhcsFMMfFFhFp
    jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL
    PmmdzqPrVvPwwTWBwg
    wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn
    ttgJtRGJQctTZtZT
    CrZsJsPPZsGzwwsLwLmpwMDw
    `)

const input = parse(readFileSync(`aoc/day${day}.input.txt`, 'utf-8'))

function solve(hunks: string[]) {
    let match = from(hunks)
        .selectMany(h => from(h).distinct())    // combine all hunks after removing dups
        .groupBy(c => c)                        // count dup chars
        .where(g => g.count() == hunks.length)  // find the one where they all contain it
        .first().key                            // get the char

    // score it
    let [base, off] = match >= 'a' ? ['a', 1] : ['A', 27]
    return match.charCodeAt(0) - base.charCodeAt(0) + off
}

// solve by halves
function solve1(rucksacks: string[]) {
    return from(rucksacks).sum(r => solve([
        r.slice(0, r.length/2), r.slice(r.length/2)]))
}

// solve by triplets
function solve2(rucksacks: string[]) {
    return range(0, rucksacks.length/3).sum(i => solve(
        rucksacks.slice(i*3, i*3+3)))
}

solve1(sample)

test(`Day ${day}.1 Sample`, () => { expect(solve1(sample)).toBe(157); });
test(`Day ${day}.1 Problem`, () => { expect(solve1(input)).toBe(7848); });
test(`Day ${day}.2 Sample`, () => { expect(solve2(sample)).toBe(70); });
test(`Day ${day}.2 Problem`, () => { expect(solve2(input)).toBe(2616); });

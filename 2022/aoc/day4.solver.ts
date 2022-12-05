import { readFileSync } from 'fs'
import { range } from 'linq-to-typescript'
const day = 4

function parse(text: string) {
    return text.match(/\d+/g).map(Number)
}

const sample = parse(`
    2-4,6-8
    2-3,4-5
    5-7,7-9
    2-8,3-7
    6-6,4-6
    2-6,4-8
    `)

const input = parse(readFileSync(`aoc/day${day}.input.txt`, 'utf-8'))

function solve1(ranges) {
    return range(0, ranges.length/4)
        .select(i => ranges.slice(i*4, i*4+4))
        .count(r => r[2] >= r[0] && r[3] <= r[1] || r[0] >= r[2] && r[1] <= r[3])
}

function solve2(ranges) {
    return range(0, ranges.length/4)
        .select(i => ranges.slice(i*4, i*4+4))
        .count(r => r[1] >= r[2] && r[0] <= r[3])
}

test(`Day ${day}.1 Sample`, () => { expect(solve1(sample)).toBe(2); });
test(`Day ${day}.1 Problem`, () => { expect(solve1(input)).toBe(540); });
test(`Day ${day}.2 Sample`, () => { expect(solve2(sample)).toBe(4); });
test(`Day ${day}.2 Problem`, () => { expect(solve2(input)).toBe(872); });

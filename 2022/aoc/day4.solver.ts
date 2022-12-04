import { readFileSync } from 'fs'
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

function contained(range): boolean {
    return (
        range[0] >= range[2] && range[1] <= range[3] ||
        range[2] >= range[0] && range[3] <= range[1])
}

function intersects(range): boolean {
    return (
        range[0] >= range[2] && range[0] <= range[3] ||
        range[1] >= range[2] && range[1] <= range[3])
}

function solve(ranges, predicate) {
    let total = 0
    for (let i = 0; i < ranges.length; i += 4) {
        if (predicate(ranges.slice(i, i+4)))
            ++total
    }
    return total
}

function solve1(ranges) {
    return solve(ranges, contained)
}

function solve2(ranges) {
    return solve(ranges, range => intersects(range) || contained(range))
}

test(`Day ${day}.1 Sample`, () => { expect(solve1(sample)).toBe(2); });
test(`Day ${day}.1 Problem`, () => { expect(solve1(input)).toBe(540); });
test(`Day ${day}.2 Sample`, () => { expect(solve2(sample)).toBe(4); });
test(`Day ${day}.2 Problem`, () => { expect(solve2(input)).toBe(872); });

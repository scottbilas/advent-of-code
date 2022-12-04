import * as fs from 'fs'
import { range } from 'linq-to-typescript'
const day = 2

function parse(text) {
    return text.trim().split(/\s+/g)
}

const sample = parse(`
    A Y
    B X
    C Z`)

const input = parse(fs.readFileSync(`aoc/day${day}.input.txt`, 'utf-8'))

function solve(moves, predicate) {
    return range(0, moves.length/2)
        .select(i => predicate(moves[i*2], moves[i*2+1]))
        .sum()
}

function solve1(moves) {
    return solve(moves, (them, me) =>
        ({ XA: 3, YB: 3, ZC: 3, XC: 6, YA: 6, ZB: 6 }[me + them] ?? 0) +
        { X: 1, Y: 2, Z: 3 }[me])
}

function solve2(moves) {
    return solve(moves, (them, end) => (
        { X: 0, Y: 3, Z: 6 }[end] +
        { A: 1, B: 2, C: 3 }[{
            Y: them,
            X: { A: 'C', B: 'A', C: 'B' }[them],
            Z: { A: 'B', B: 'C', C: 'A' }[them]
        }[end]]))
}

test(`Day ${day}.1 Sample`, () => { expect(solve1(sample)).toBe(15); });
test(`Day ${day}.1 Problem`, () => { expect(solve1(input)).toBe(17189); });
test(`Day ${day}.2 Sample`, () => { expect(solve2(sample)).toBe(12); });
test(`Day ${day}.2 Problem`, () => { expect(solve2(input)).toBe(13490); });

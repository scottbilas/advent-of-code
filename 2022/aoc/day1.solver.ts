import * as fs from 'fs'
import { from, NumberComparer } from 'linq-to-typescript'
const day = 1

function parse(text) {
    return text.trim().replace(/\r/g, '').split(/\n\n/)
}

const sample = parse(`
    1000
    2000
    3000

    4000

    5000
    6000

    7000
    8000
    9000

    10000`)

const input = parse(fs.readFileSync(`aoc/day${day}.input.txt`, 'utf-8'))

function solve(blocks :string[], count) {
    return from(blocks)
        .select(block =>
            from(block.split(/\n/))
            .aggregate(0, (total, line) => total + parseInt(line)))
        .orderByDescending(_ => _, NumberComparer)
        .take(count)
        .sum();
}

function solve1(text) { return solve(text, 1) }
function solve2(text) { return solve(text, 3) }

test(`Day ${day}.1 Sample`, () => { expect(solve1(sample)).toBe(24000); });
test(`Day ${day}.1 Problem`, () => { expect(solve1(input)).toBe(69177); });
test(`Day ${day}.2 Sample`, () => { expect(solve2(sample)).toBe(45000); });
test(`Day ${day}.2 Problem`, () => { expect(solve2(input)).toBe(207456); });

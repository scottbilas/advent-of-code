import _ = require('lodash')
import u = require('./utils')

const day = 4

const sample = `
    2-4,6-8
    2-3,4-5
    5-7,7-9
    2-8,3-7
    6-6,4-6
    2-6,4-8`

const input = u.getProblemInput(day)

const solve = (text, op) => _.chunk(u.parseNums(text), 4).filter(op).length
const solve1 = (text) => solve(text, r => r[2] >= r[0] && r[3] <= r[1] || r[0] >= r[2] && r[1] <= r[3])
const solve2 = (text) => solve(text, r => r[1] >= r[2] && r[0] <= r[3])

test(`Day ${day}.1 Sample`, () => { expect(solve1(sample)).toBe(2); });
test(`Day ${day}.1 Problem`, () => { expect(solve1(input)).toBe(540); });
test(`Day ${day}.2 Sample`, () => { expect(solve2(sample)).toBe(4); });
test(`Day ${day}.2 Problem`, () => { expect(solve2(input)).toBe(872); });

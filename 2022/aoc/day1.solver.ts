import _ = require('lodash')
import u = require('./utils')

const day = 1

const sample = `
    1000
    2000
    3000

    4000

    5000
    6000

    7000
    8000
    9000

    10000`

const input = u.getProblemInput(day)

const solve = (text, count) => _(u.parseBlocks(text))
    .map(block => _.sum(u.parseNums(block)))
    .sort((a, b) => b - a)
    .take(count)
    .sum()

const solve1 = text => solve(text, 1)
const solve2 = text => solve(text, 3)

test(`Day ${day}.1 Sample`, () => { expect(solve1(sample)).toBe(24000); });
test(`Day ${day}.1 Problem`, () => { expect(solve1(input)).toBe(69177); });
test(`Day ${day}.2 Sample`, () => { expect(solve2(sample)).toBe(45000); });
test(`Day ${day}.2 Problem`, () => { expect(solve2(input)).toBe(207456); });

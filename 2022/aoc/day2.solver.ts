import _ = require('lodash')
import u = require('./utils')

const day = 2

const sample = u.parseWords(`
    A Y
    B X
    C Z`)

const input = u.parseWords(u.getProblemInput(day))

const solve = (moves, predicate) => _.chain(moves)
    .chunk(2)
    .map(move => predicate(move[0], move[1]))
    .sum().value()

const solve1 = (moves) => solve(moves, (them, me) =>
    ({ XA: 3, YB: 3, ZC: 3, XC: 6, YA: 6, ZB: 6 }[me + them] ?? 0) +
    { X: 1, Y: 2, Z: 3 }[me])

const solve2 = (moves) => solve(moves, (them, end) => (
    { X: 0, Y: 3, Z: 6 }[end] +
    { A: 1, B: 2, C: 3 }[{
        Y: them,
        X: { A: 'C', B: 'A', C: 'B' }[them],
        Z: { A: 'B', B: 'C', C: 'A' }[them]
    }[end]]))

test(`Day ${day}.1 Sample`, () => { expect(solve1(sample)).toBe(15); });
test(`Day ${day}.1 Problem`, () => { expect(solve1(input)).toBe(17189); });
test(`Day ${day}.2 Sample`, () => { expect(solve2(sample)).toBe(12); });
test(`Day ${day}.2 Problem`, () => { expect(solve2(input)).toBe(13490); });

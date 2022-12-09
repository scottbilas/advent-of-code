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

u.test(`Day ${day}.1 Sample`,  () => solve1(sample),   2)
u.test(`Day ${day}.1 Problem`, () => solve1(input),  540)
u.test(`Day ${day}.2 Sample`,  () => solve2(sample),   4)
u.test(`Day ${day}.2 Problem`, () => solve2(input),  872)

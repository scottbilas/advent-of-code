import { check, getProblemInput, parseBlocks, parseNumbers } from './utils'

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

const input = getProblemInput(day)

const solve = (text, count) => parseBlocks(text)
    .map(block => parseNumbers(block).sum())
    .sort((a, b) => b - a)
    .take(count)
    .sum()

const solve1 = text => solve(text, 1)
const solve2 = text => solve(text, 3)

check(`Day ${day}.1 Sample`,  () => solve1(sample),  24000)
check(`Day ${day}.1 Problem`, () => solve1(input),   69177)
check(`Day ${day}.2 Sample`,  () => solve2(sample),  45000)
check(`Day ${day}.2 Problem`, () => solve2(input),  207456)

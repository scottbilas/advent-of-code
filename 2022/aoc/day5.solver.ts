import _ = require('lodash')
import u = require('./utils')

const day = 5

function parse(text): [number[][], number[]] {
    const blocks = u.parseBlocks(text)

    const spec = blocks[0].split('\n').reverse()
    const header = spec.shift()
    const stacks = u.arrayOfArrays(u.parseNums(header).pop())
    const offset = header.indexOf('1')

    for (const line of spec) {
        for (let x = 0; x < stacks.length; ++x) {
            const c = line[x*4 + offset]
            if (c && c != ' ')
                stacks[x].push(c)
        }
    }

    return [stacks, u.parseNums(blocks[1])]
}

const sample = `
        [D]
    [N] [C]
    [Z] [M] [P]
     1   2   3

    move 1 from 2 to 1
    move 3 from 1 to 3
    move 2 from 2 to 1
    move 1 from 1 to 2`

const input = u.getProblemInput(day)

function solve(text, op) {
    const [stacks, moves] = parse(text)
    while (moves.length) {
        const [count, from, to] = moves.splice(0, 3)
        const chunk = stacks[from-1].splice(-count)
        stacks[to-1].push(...op(chunk))
    }

    return stacks.map(s => _.last(s)).join('')
}

const solve1 = text => solve(text, a => a.reverse())
const solve2 = text => solve(text, _.identity)

test(`Day ${day}.1 Sample`, () => { expect(solve1(sample)).toBe('CMZ'); });
test(`Day ${day}.1 Problem`, () => { expect(solve1(input)).toBe('HBTMTBSDC'); });
test(`Day ${day}.2 Sample`, () => { expect(solve2(sample)).toBe('MCD'); });
test(`Day ${day}.2 Problem`, () => { expect(solve2(input)).toBe('PQTJRSHWS'); });

import { arrayOfArrays, check, getProblemInput, parseBlocks, parseNumbers } from './utils'

const day = 5

function parse(text): [number[][], number[]] {
    const blocks = parseBlocks(text)

    const spec = blocks[0].split('\n').reverse()
    const header = spec.shift()
    const stacks = arrayOfArrays(parseNumbers(header).pop())
    const offset = header.indexOf('1')

    for (const line of spec) {
        for (let x = 0; x < stacks.length; ++x) {
            const c = line[x*4 + offset]
            if (c && c != ' ')
                stacks[x].push(c)
        }
    }

    return [stacks, parseNumbers(blocks[1])]
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

const input = getProblemInput(day)

function solve(text, op) {
    const [stacks, moves] = parse(text)
    while (moves.length) {
        const [count, from, to] = moves.splice(0, 3)
        const chunk = stacks[from-1].splice(-count)
        stacks[to-1].push(...op(chunk))
    }

    return stacks.map(s => s.last()).join('')
}

const solve1 = text => solve(text, a => a.reverse())
const solve2 = text => solve(text, a => a)

check(`Day ${day}.1 Sample`,  () => solve1(sample),       'CMZ')
check(`Day ${day}.1 Problem`, () => solve1(input),  'HBTMTBSDC')
check(`Day ${day}.2 Sample`,  () => solve2(sample),       'MCD')
check(`Day ${day}.2 Problem`, () => solve2(input),  'PQTJRSHWS')

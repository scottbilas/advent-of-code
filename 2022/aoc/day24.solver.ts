import { getProblemInput, check, parseGrid, Grid, number2, number3 } from './utils'
import _ = require('lodash')

const day = 24

const sample = `
    #.######
    #>>.<^<#
    #.<..<<#
    #>v.><>#
    #<^v^^>#
    ######.#`
const input = getProblemInput(day)

// TODO: ok problem is that i need to wait at start or end and include those in the move testing.
// so just keep the non-shrunk board, make sure '#' is counted as no-entry, and update begin/end to be in the open spaces.
// (this ought to also fix the problem with the sample board)

// also: update rendering to be faster. instead of this dumb cell-array stuff, just have arrays of blizzards moving in
// each direction. they are totally independent of each other so sim them all individually. can render them into a slice
// as simply 'wall' (do a bool grid) and be done with it.

const Offsets = { 'o': [0, 0], '>': [1, 0], 'v': [0, 1], '<': [-1, 0], '^': [0, -1] }

const gridToString = (g, e?: number2[]) => {
    let old
    if (e) {
        old = e.map(p => [p, g.get(p)])
        e.forEach(p => g.set(p, ['E']))
    }

    let s = ''
    for (let y = 0; y < g.size.y; ++y) {
        for (let x = 0; x < g.size.x; ++x) {
            let c = g.get2(x, y)
            switch (c.length) {
                case 0: s += '.'; break
                case 1: s += c[0]; break
                default: s += c.length
            }
        }
        s += '\n'
    }

    if (old)
        old.forEach(([p, s]) => g.set(p, s))

    return s
}

const combine = (a, b) => _
    .zip(a.split('\n'), b.split('\n'))
    .map(([a, b]) => a + '  ' + b)
    .join('\n')

function generate(text) : Grid<string[]>[] {
    let board = Array<Grid<string[]>>()
    let seen = new Set()

    board.push(parseGrid<string[]>(text, c => c == '.' ? [] : [c]).shrink())
    seen.add(gridToString(board[0]))

    for (;;) {
        let cur = board.at(-1)
        let next = new Grid<string[]>(cur.cells.map(_ => []), cur.cx, cur.cy)

        for (let i = 0; i < cur.cells.length; ++i) {
            for (let [cell, move] of cur.cells[i].map(c => [c, Offsets[c]]).filter(v => v[1])) {
                let pos = cur.posAt(i)
                    .add(number2.fromTuple(move))
                    .add(cur.size).mod(cur.size)
                next.cells[cur.offsetAt(pos)].push(cell)
            }
        }

        let str = gridToString(next)
        if (seen.has(str))
            break

        seen.add(str)
        board.push(next)
    }

    return board
}

function solve(board: Grid<string[]>[], minute: number, start: number2, end: number2) {

    let nextWork = [start]

    for (; nextWork.length; ++minute) {

        let work = nextWork
        nextWork = Array<number2>()
        let nextSlice = board[(minute+1) % board.length]

        let text = gridToString(board[minute % board.length], work)
        console.log(minute)
        console.log(combine(text, gridToString(nextSlice)))

        for (let from of work) {
            for (let offset in Offsets) {
                let to = from.add(number2.fromTuple(Offsets[offset]))
                if (to.equals(end)) {
                    ++minute
                    console.log(minute)
                    console.log(combine(
                        gridToString(board[minute % board.length], [to]),
                        gridToString(board[(minute+1)%board.length])))
                    return minute
                }

                if (nextWork.find(p => p.equals(to)))
                    continue

                if (nextSlice.try(to)?.length !== 0)
                    continue

                nextWork.push(to)
            }
        }
    }
}

function solve1(text) {
    let board = generate(text)
    let start = number2.zero
    let end = board.at(-1).size.subS(1)

    return solve(board, 1, start, end) + 1
}

function solve2(text) {
    let board = generate(text)

    let start = number2.zero
    let end = board.at(-1).size.subS(1)

    let m0 = 1
    let m1 = solve(board, m0, start, end) + 1

    console.log('BACK: ' + m1)
    console.log(combine(
        gridToString(board[m1 % board.length]),
        gridToString(board[(m1+1)%board.length])))

    let m2 = solve(board, m1 + 1, end, start) + 1
    let m3 = solve(board, m2 + 1, start, end) + 1

    return m3
}

//check(`Day ${day}.1 Sample`,  () => solve1(sample), 18)
//check(`Day ${day}.1 Problem`, () => solve1(input), 308)
check(`Day ${day}.2 Sample`,  () => solve2(sample), 54)
//check(`Day ${day}.2 Problem`, () => solve2(input),  -1)

import { check, getProblemInput, Grid, number2, parseGrid } from './utils'
import _ = require('lodash')

const day = 12

const sample = `
    Sabqponm
    abcryxxl
    accszExk
    acctuvwj
    abdefghi`

const input = getProblemInput(day)

class Cell {
    height: number
    steps:  number = Infinity

    constructor(public value: string) {
        let char = { S: 'a', E: 'z' }[value] ?? value
        this.height = char.charOffset('a')
    }
}

function solve(text: string): Grid<Cell> {
    let grid = parseGrid(text, s => new Cell(s))
    let start = grid.posAt(grid.cells.findIndex(c => c.value[0] == 'E'))

    grid.get(start).steps = 0

    let queue: number2[] = [start]
    let moves = [[-1, 0], [1, 0], [0, -1], [0, 1]]

    while (queue.length) {
        let work = queue.shift()
        let src = grid.get(work)

        for (let move of moves.map(m => work.add2a(m))) {
            let dst = grid.try(move)
            if (!dst || dst.steps != Infinity || src.height - dst.height > 1)
                continue

            dst.steps = src.steps + 1
            queue.splice(_.sortedIndexBy(queue, move, m => grid.get(m).steps), 0, move)
        }
    }

    return grid
}

const solve1 = text => solve(text).cells
    .find(c => c.value[0] == 'S')
    .steps

const solve2 = text => solve(text).cells
    .filter(c => c.value[0] == 'a')
    .map(c => c.steps)
    .min()

check(`Day ${day}.1 Sample`,  () => solve1(sample),  31)
check(`Day ${day}.1 Problem`, () => solve1(input),  468)
check(`Day ${day}.2 Sample`,  () => solve2(sample),  29)
check(`Day ${day}.2 Problem`, () => solve2(input),  459)

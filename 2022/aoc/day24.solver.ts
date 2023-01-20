import { getProblemInput, check, number2, parseStringGrid, CharGrid, prettyMs } from './utils'

const day = 24

const startParse = new Date().getTime()

const sample = parse(`
    #.######
    #>>.<^<#
    #.<..<<#
    #>v.><>#
    #<^v^^>#
    ######.#`)

const input = parse(getProblemInput(day))

const endParse = new Date().getTime()

console.log(`Parsed in ${prettyMs(endParse - startParse)}`)

function parse(text): [CharGrid[], number, number] {

    let start = parseStringGrid(text)
    let innerSize = start.size.sub2([2, 2])

    // all starting blizzard positions in order >v<^
    let allBlizzards: number2[][] = [[],[],[],[]]

    // find all the blizzards, clearing the board as we go (we will clone it later)
    start.forEach((c, p) => {
        let o = '>v<^'.indexOf(c)
        if (o >= 0) {
            start.set(p, '.')
            allBlizzards[o].push(p.sub(number2.one))
        }
    })

    // render for any given minute the blizzards and overlaps into a new CharGrid
    function render(minute): CharGrid {
        let grid = start.clone()
        let offsets = [[1, 0], [0, 1], [innerSize.x-1, 0], [0, innerSize.y-1]] // pre-offset positive for later mod

        for (let [idir, blizzards] of allBlizzards.entries()) {
            let offset = offsets[idir].map(v => v*minute)

            // mostly for pretty printing, not strictly necessary to have the number progression..
            let next = {
                '.': '>v<^'[idir],
                '>': '2', 'v': '2', '<': '2', '^': '2',
                '2': '3', '3': '4' }

            // render the blizzard (or update count if already there)
            for (let blizzard of blizzards) {
                let off = grid.offsetAt2(
                    (blizzard.x + offset[0]) % innerSize.x + 1,
                    (blizzard.y + offset[1]) % innerSize.y + 1)
                grid.cells[off] = next[grid.cells[off]]
            }
        }

        // convert into flat array for better perf later
        return CharGrid.fromStringGrid(grid)
    }

    let board: CharGrid[] = []
    let seen = new Set()

    // we only need to render slices until the modulos line up and we start to cycle
    for (;;) {
        let slice = render(board.length)
        if (seen.has(slice.cells))
            break

        seen.add(slice.cells)
        board.push(slice)
    }

    return [board,
        start.cells.indexOf('.'),
        start.cells.lastIndexOf('.')]
}

function solve(board: CharGrid[], minute: number, begin: number, end: number): number {
    let nextWork = [begin]
    let offsets = [0, 1, board[0].cx, -1, -board[0].cx]

    // simple BFS

    for (; nextWork.length; ++minute) {
        let work = nextWork
        nextWork = []
        let nextSlice = board[(minute+1) % board.length]

        // could speed this up a little with a heuristic and priority queue

        for (let from of work) {
            for (let offset of offsets) {
                let to = from + offset
                if (to == end)
                    return minute + 1
                if (nextSlice.cells[to] == '.' && nextWork.indexOf(to) < 0)
                    nextWork.push(to)
            }
        }
    }
}

function solve1([board, begin, end]) {
    return solve(board, 1, begin, end)
}

function solve2([board, begin, end]) {
    let minute = 1
    minute = solve(board, minute, begin, end)
    minute = solve(board, minute, end, begin)
    minute = solve(board, minute, begin, end)
    return minute
}

check(`Day ${day}.1 Sample`,  () => solve1(sample), 18)
check(`Day ${day}.1 Problem`, () => solve1(input), 308)
check(`Day ${day}.2 Sample`,  () => solve2(sample), 54)
check(`Day ${day}.2 Problem`, () => solve2(input), 908)

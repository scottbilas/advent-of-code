import { getProblemInput, check, parseStringGrid, number2 } from './utils'
import { HashMap, HashSet } from 'tstl'

const day = 23

const sample = `
    ....#..
    ..###.#
    #...#.#
    .#...##
    #.###..
    ##.#.##
    .#..#..`
const input = getProblemInput(day)

enum Dir { NW, N, NE, E, SE, S, SW, W }
const DirNext = { N: Dir.S, S: Dir.W, W: Dir.E, E: Dir.N }
const DirOffsets = [[-1, -1], [0, -1], [1, -1], [1, 0], [1, 1], [0, 1], [-1, 1], [-1, 0]].map(number2.fromArray)

function elvesToString(elves: HashSet<number2>) {
    let [min, max] = [number2.zero, number2.minNumber]
    for (let elf of elves)
        [min, max] = [min.min(elf), max.max(elf)]
    let result = ''
    for (let y = min.y; y <= max.y; ++y) {
        for (let x = min.x; x <= max.x; ++x)
            result += elves.has(new number2(x, y)) ? '#' : '.'
        result += '\n'
    }
    return result
}

function solve(elves: HashSet<number2>, maxRounds: number = null): number {
    let mainDir = Dir.N

    const exist = (base: number2, dir: Dir) => elves.has(base.add(DirOffsets[dir]))

    //console.log(elvesToString(elves))

    let round = 1
    for (; !maxRounds || round <= maxRounds; ++round) {

        // first half of round

        let moves = new HashMap<number2, number2>() // to, from

        for (let elf of elves) {
            if (!DirOffsets.some(o => elves.has(elf.add(o))))
                continue

            let testDir = mainDir
            for (let i = 0; i < 4; ++i) {
                let ok = !exist(elf, testDir-1) && !exist(elf, testDir) && !exist(elf, (testDir+1)%8)
                if (ok) {
                    let move = elf.add(DirOffsets[testDir])
                    if (moves.has(move))
                        moves.set(move, null)
                    else
                        moves.set(move, elf)
                    break
                }
                testDir = DirNext[Dir[testDir]]
            }
        }

        // second half of round

        let moved = false
        for (let pair of moves) {
            if (pair.second) {
                elves.erase(pair.second)
                elves.insert(pair.first)
                moved = true
            }
        }
        if (!moved)
            break

        // end of round

        mainDir = DirNext[Dir[mainDir]]

        //console.log(elvesToString(elves))
    }

    return round
}

function parse(text): HashSet<number2> {
    let grid = parseStringGrid(text)
    return new HashSet(grid.cells
        .map((c, i) => c === '#' ? grid.posAt(i) : null)
        .filter(p => p))
}

function solve1(text): number {
    let elves = parse(text)
    solve(elves, 10)
    return elvesToString(elves).split('').filter(c => c === '.').length
}

function solve2(text) {
    let elves = parse(text)
    return solve(elves)
}

check(`Day ${day}.1 Sample`,  () => solve1(sample),  110)
check(`Day ${day}.1 Problem`, () => solve1(input),  3849)
check(`Day ${day}.2 Sample`,  () => solve2(sample),   20)
check(`Day ${day}.2 Problem`, () => solve2(input),   995) // 57s - slow AF

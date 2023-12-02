import { check, getProblemInput, getProblemSample, parseWords } from './utils'

const day = 10

const sample = parseWords(getProblemSample(day))
const input = parseWords(getProblemInput(day))

function solve1(instrs) {
    let x = 1
    let cycles = 1
    let score = 0

    let watch = [20, 60, 100, 140, 180, 220]
    function check() {
        let match = watch.find(c => c == cycles)
        if (match) {
            score += match * x
        }
    }

    for (let i = 0; i < instrs.length; ++i) {
        if (instrs[i] == 'noop') {
            check()
            ++cycles
            continue
        }

        check()
        ++cycles
        check()
        ++cycles
        x += +instrs[i++ + 1]
    }

    return score
}

function solve2(instrs) {
    let sprite = 1

    let display = Array(6).fill(0).map(_ => Array(40).fill('.'))

    let [x, y] = [0, 0]
    function draw() {
        if (x == sprite || x == sprite-1 || x == sprite+1) {
            display[y][x] = '#'
        }
        ++x
        if (x == 40) {
            x = 0
            ++y
        }
    }

    for (let i = 0; i < instrs.length; ++i) {
        if (instrs[i] == 'noop') {
            draw()
            continue
        }

        draw()
        draw()
        sprite += +instrs[i++ + 1]
    }

    return display.map(l => l.join(''))
}

check(`Day ${day}.1 Sample`,  () => solve1(sample), 13140)
check(`Day ${day}.1 Problem`, () => solve1(input),  13220)
check(`Day ${day}.2 Sample`,  () => solve2(sample), [
    '##..##..##..##..##..##..##..##..##..##..',
    '###...###...###...###...###...###...###.',
    '####....####....####....####....####....',
    '#####.....#####.....#####.....#####.....',
    '######......######......######......####',
    '#######.......#######.......#######.....'])
check(`Day ${day}.2 Problem`, () => solve2(input), 0)

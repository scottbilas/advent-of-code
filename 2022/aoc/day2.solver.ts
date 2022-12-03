import * as fs from 'fs'
const day = 2

function parse(text) {
    return text.trim().split(/\s+/g)
}

const sample = parse(`
    A Y
    B X
    C Z`)

const input = parse(fs.readFileSync(`aoc/day${day}.input.txt`, 'utf-8'))

function solve1(moves) {
    let score = 0

    for (let i = 0; i < moves.length; i += 2) {
        let [me, them] = [moves[i+1], moves[i] ]

        score += { XA: 3, YB: 3, ZC: 3, XC: 6, YA: 6, ZB: 6 }[me + them] ?? 0
        score += { X: 1, Y: 2, Z: 3 }[me]
    }
    return score
}

function solve2(moves) {
    let score = 0

    for (let i = 0; i < moves.length; i += 2) {
        let [me, them, end] = ['', moves[i], moves[i+1] ]

        if (end === 'X')
            me = { A: 'C', B: 'A', C: 'B' }[them]
        else if (end === 'Y') {
            me = them
            score += 3
        }
        else if (end === 'Z') {
            me = { A: 'B', B: 'C', C: 'A' }[them]
            score += 6
        }

        score += { A: 1, B: 2, C: 3 }[me]
    }

    return score
}

test(`Day ${day}.1 Sample`, () => { expect(solve1(sample)).toBe(15); });
test(`Day ${day}.1 Problem`, () => { expect(solve1(input)).toBe(17189); });
test(`Day ${day}.2 Sample`, () => { expect(solve2(sample)).toBe(12); });
test(`Day ${day}.2 Problem`, () => { expect(solve2(input)).toBe(13490); });

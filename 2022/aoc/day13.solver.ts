import { check, getProblemInput, getProblemSample, parseBlocks, parseLines } from './utils'

const day = 13

const sample = getProblemSample(day)
const input = getProblemInput(day)

function compare(lt: string, rt: string): boolean {
    let [l, r] = [lt, rt].map(l => l.match(/[[\]]|\d+/g))
    for (let i = 0;;++i) {
        if (l[i] == r[i])
            continue

        if (l[i] == '[') {
            if (r[i] == ']')
                return false;
            r.splice(i, 1, '[', r[i], ']')
        }
        else if (r[i] == '[') {
            if (l[i] == ']')
                return true;
            l.splice(i, 1, '[', l[i], ']')
        }
        else if (l[i] == ']')
            return true;
        else if (r[i] == ']')
            return false;
        else
            return +l[i] < +r[i];
    }
}

const solve1 = text =>
    parseBlocks(text)
    .map(b => parseLines(b))
    .map((pair, index) => compare(pair[0], pair[1]) ? index+1 : 0)
    .sum()

function solve2(text) {
    const extra = ['[[2]]', '[[6]]']
    let packets =
        parseLines(text + `\n${extra[0]}\n${extra[1]}`)
        .filter(l => l.length)
        .sort((a, b) => compare(a, b) ? -1 : 1)
    return extra
        .map(e => packets.indexOf(e) + 1)
        .product()
}

check(`Day ${day}.1 Sample`,  () => solve1(sample),    13)
check(`Day ${day}.1 Problem`, () => solve1(input),   6076)
check(`Day ${day}.2 Sample`,  () => solve2(sample),   140)
check(`Day ${day}.2 Problem`, () => solve2(input),  24805)

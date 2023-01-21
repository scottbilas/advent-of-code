import { getProblemInput, check, parseLines } from './utils'
import * as math from 'mathjs'

const day = 21

const parse = text => new Map(parseLines(text)
    .map(l => l.match(/\w+|\d+|[-+*/]/g))
    .map(p => [p[0], p.slice(1)]))

const sample = parse(`
    root: pppw + sjmn
    dbpl: 5
    cczh: sllz + lgvd
    zczc: 2
    ptdq: humn - dvpt
    dvpt: 3
    lfqf: 4
    humn: 5
    ljgn: 2
    sjmn: drzm * dbpl
    sllz: 4
    pppw: cczh / lfqf
    lgvd: ljgn * ptdq
    drzm: hmdt - zczc
    hmdt: 32`)

const input = parse(getProblemInput(day))

const calc = (a: number, op: string, b: number): number => math.evaluate(a+op+b)

function solve1(parsed) {
    function resolve(name: string): number {
        let [a, op, b] = parsed.get(name)
        return op == undefined
            ? +a
            : calc(resolve(a), op, resolve(b))
    }
    return resolve('root')
}

function solve2(parsed) {
    let result

    function resolve(name: string, expect: number = undefined): number {
        if (name == 'humn')
            return result = expect

        let [a, op, b] = parsed.get(name)
        if (op == undefined)
            return +a

        let [ar, br] = [resolve(a), resolve(b)]
        if (ar != undefined && br != undefined)
            return calc(ar, op, br)

        if (expect == undefined && op != '=')
            return undefined

        let r = {
            '=': [[a, br], [b, ar]],
            '+': [[a, expect - br], [b, expect - ar]],
            '*': [[a, expect / br], [b, expect / ar]],
            '-': [[a, expect + br], [b, ar - expect]],
            '/': [[a, expect * br], [b, ar / expect]]
        }[op][ar == undefined ? 0 : 1]
        resolve(r[0], r[1])
    }

    parsed.get('root')[1] = '='
    resolve('root')

    return result
}

check(`Day ${day}.1 Sample`,  () => solve1(sample),             152)
check(`Day ${day}.1 Problem`, () => solve1(input),  309248622142100)
check(`Day ${day}.2 Sample`,  () => solve2(sample),             301)
check(`Day ${day}.2 Problem`, () => solve2(input),    3757272361782)

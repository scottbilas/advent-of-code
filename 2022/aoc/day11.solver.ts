import { check, getProblemInput, getProblemSample, parseBlocks, parseWords } from './utils'
import pf = require('prime-factorization');

const day = 11

const sample = getProblemSample(day)
const input = getProblemInput(day)

class Monkey {
    inspected: number = 0
    items: number[]
    op: string[]
    test: number
    throw: number[]

    constructor(text) {
        let matches = text.match(/\d+|(\S+ [*+] \S+)/g)
        this.items = matches.slice(1, -4).map(Number)
        this.op = matches.at(-4).split(' ')
        this.test = +matches.at(-3)
        this.throw = [+matches.at(-2), +matches.at(-1)]
    }

    update(monkeys: Monkey[]) {
        while (this.items.length) {
            let item = this.items.shift()
            ++this.inspected
            let a = this.op[0] == 'old' ? item : +this.op[0]
            let b = this.op[2] == 'old' ? item : +this.op[2]
            item = this.op[1] == '+' ? a + b : a * b
            item = Math.floor(item / 3)
            if (item % this.test == 0)
                monkeys[this.throw[0]].items.push(item)
            else
                monkeys[this.throw[1]].items.push(item)
        }
    }
}

class Item2 {
    id: number
    value: bigint
    inspected: number = 0

    constructor(value: bigint) {
        this.value = value
    }
}

class Monkey2 {
    id: number
    inspected: number = 0
    items: Item2[]
    op: string[]
    test: bigint
    throw: number[]

    constructor(text, id) {
        this.id = id
        let matches = text.match(/\d+|(\S+ [*+] \S+)/g)
        this.items = matches.slice(1, -4).map(i => new Item2(BigInt(i)))
        this.op = matches.at(-4).split(' ')
        this.test = BigInt(matches.at(-3))
        this.throw = [+matches.at(-2), +matches.at(-1)]
    }

    update(monkeys: Monkey2[], combo: bigint) {
        while (this.items.length) {
            let item = this.items.shift()
            ++this.inspected
            let a = this.op[0] == 'old' ? item.value : BigInt(this.op[0])
            let b = this.op[2] == 'old' ? item.value : BigInt(this.op[2])
            item.value = this.op[1] == '+' ? a + b : a * b
            item.value = item.value % combo
            ++item.inspected
            let where = this.throw[(item.value % this.test === 0n) ? 0 : 1]
            monkeys[where].items.push(item)
        }
    }
}

function solve1(text) {
    let monkeys = parseBlocks(text).map(b => new Monkey(b))

    for (let round = 0; round < 20; round++)
        for (let monkey of monkeys)
            monkey.update(monkeys)

    return monkeys
        .map(m => m.inspected)
        .sort((a, b) => b - a)
        .slice(0, 2)
        .product()
}

function solve2(text) {
    let mid = 0
    let monkeys = parseBlocks(text).map(b => new Monkey2(b, mid++))

    let id = 0
    let modulo = 1n
    for (let monkey of monkeys) {
        for (let item of monkey.items) {
            item.id = id++
        }
        modulo *= monkey.test
    }

    for (let round = 0; round < 10000; round++)
        for (let monkey of monkeys)
            monkey.update(monkeys, modulo)

    return monkeys
        .map(m => m.inspected)
        .sort((a, b) => b - a)
        .slice(0, 2)
        .product()
}

check(`Day ${day}.1 Sample`,  () => solve1(sample), 10605)
check(`Day ${day}.1 Problem`, () => solve1(input),  120056)
check(`Day ${day}.2 Sample`,  () => solve2(sample), 2713310158)
check(`Day ${day}.2 Problem`, () => solve2(input),  21816744824)
